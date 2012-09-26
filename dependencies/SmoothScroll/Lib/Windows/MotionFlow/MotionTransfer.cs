using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace Logitech.Windows.MotionFlow
{
  #region IMotionTransferOutput
  public interface IMotionTransferOutput
  {
    IMotionInfo MotionInfo { get; }
    double       Remainder { get; }

    bool Transfer(IMotionTarget target, object context);
  }
  #endregion

  #region INativeMotionTransferOutput
  public interface INativeMotionTransferOutput : IMotionTransferOutput
  {
    int NativeRemainder { get; }

    bool Transfer(INativeMotionTarget target, object context);
  }
  #endregion

  #region IMotionTransfer
  public interface IMotionTransfer: IMotionInput, IMotionTransferOutput { }
  #endregion

  #region INativeMotionTransfer
  public interface INativeMotionTransfer : INativeMotionInput, INativeMotionTransferOutput { }
  #endregion 

  #region MotionTransfer
  public class MotionTransfer : MotionElement, IMotionTransfer
  {
    #region Constants
    public static readonly RoutedEvent TransferingEvent = EventManager.RegisterRoutedEvent("Transfering", RoutingStrategy.Bubble, typeof(MotionTransferEventHandler), typeof(MotionTransfer));
    public static readonly RoutedEvent  TransferedEvent = EventManager.RegisterRoutedEvent("Transfered",  RoutingStrategy.Bubble, typeof(MotionTransferEventHandler), typeof(MotionTransfer));
    #endregion

    #region Fields
    private IMotionInfo _info;
    private double _sourceDelta;
    #endregion

    #region Initialization
    public MotionTransfer()
    {
      Name = Id.ToString("'T'00");
    }
    #endregion

    #region Object
    public override string ToString()
    {
      return string.Format("{0} : Remainder={1}", Name, Remainder);
    }
    #endregion

    #region IMotionInput
    public void Transmit(IMotionInfo info, double delta, IMotionOutput source)
    {
      if (info != null)
        _info = info;
      _sourceDelta += delta;
    }
    public void OnCoupledTransfer(IMotionInfo info, double delta, IMotionTransferOutput source)
    {
      if (source != this)
        _sourceDelta -= delta;
    }
    public void Reset()
    {
      _sourceDelta = 0;
    }
    #endregion

    #region IMotionTransferOutput
    public IMotionInfo MotionInfo { get { return _info; } }
    public double       Remainder { get { return _sourceDelta; } }

    public bool Transfer(IMotionTarget target, object context)
    {
      if (target == null)
        throw new ArgumentNullException("target");
      RaiseEvent(new MotionTransferEventArgs(_info, _sourceDelta) { RoutedEvent = TransferingEvent });

      if (Math.Sign(_sourceDelta) == _info.Direction)
      {
        var converter = _info.Source as INativeMotionConverter;
        double targetDelta = target.Coerce(_info, context, _sourceDelta);
        if (!DoubleEx.IsZero(targetDelta))
        {
          target.Move(_info, context, targetDelta);
          _sourceDelta -= targetDelta;
          RaiseEvent(new MotionTransferEventArgs(_info, targetDelta) { RoutedEvent = TransferedEvent });
        }
      }
      return target.CanMove(_info, context);
    }
    #endregion

    #region Helpers
    private bool IsDirectionWrong { get { return _info.Direction != Math.Sign(_sourceDelta); } }
    #endregion
  }
  #endregion

  #region NativeMotionTransfer
  public class NativeMotionTransfer : MotionElement, INativeMotionTransfer
  {
    #region Constants
    public static readonly RoutedEvent TransferingEvent = EventManager.RegisterRoutedEvent("Transfering", RoutingStrategy.Bubble, typeof(NativeMotionTransferEventHandler), typeof(NativeMotionTransfer));
    public static readonly RoutedEvent TransferedEvent  = EventManager.RegisterRoutedEvent("Transfered",  RoutingStrategy.Bubble, typeof(NativeMotionTransferEventHandler), typeof(NativeMotionTransfer));
    #endregion

    #region Fields
    private IMotionInfo _info;
    private int _nativeSourceDelta;
    private int _nativeTransferCreditDelta;
    #endregion

    #region Initialization
    public NativeMotionTransfer()
    {
      Name = Id.ToString("'T'00");
    }
    #endregion

    #region Object
    public override string ToString()
    {
      return string.Format("{0}: Remaining={1:F3}", Name, Remainder);
    }
    #endregion

    #region IMotionInput
    public virtual void Transmit(IMotionInfo info, int nativeDelta, INativeMotionOutput source)
    {
      if (info != null)
        _info = info;
      _nativeSourceDelta += nativeDelta;
      //if (nativeDelta != 0)
      //  Debug.WriteLine("{0}{1}(delta = {2,4}) --> {3,4}{4}",
      //    Name, TransmitMethodSuffix(info, nativeDelta),
      //    nativeDelta, _nativeSourceDelta,
      //    _nativeSourceDelta != 0 && Math.Sign(_nativeSourceDelta) != info.NativeDirection ? " (##)" : "");
    }
    public void OnCoupledTransfer(IMotionInfo info, int nativeDelta, INativeMotionTransferOutput source)
    {
      if (source != this)
      {
        _nativeSourceDelta -= nativeDelta;
        //if (nativeDelta != 0)
        //  Debug.WriteLine("{0}{1}(delta = {2,4}) --> {3,4}{4}",
        //    Name, TransmitMethodSuffix(info, nativeDelta),
        //    nativeDelta, _nativeSourceDelta,
        //    _nativeSourceDelta != 0 && Math.Sign(_nativeSourceDelta) != info.NativeDirection ? " (##)" : "");
      }
    }
    public void Reset()
    {
      _nativeSourceDelta = _nativeTransferCreditDelta = 0;
    }
    #endregion

    #region IMotionTransferOutput
    public IMotionInfo MotionInfo { get { return _info; } }

    public double Remainder
    {
      get
      {
        if (_info == null)
          return 0;
        return (_info.Source as INativeMotionConverter).NativeToNormalized(_nativeSourceDelta);
      }
    }
    public bool Transfer(IMotionTarget target, object context)
    {
      if (target == null)
        throw new ArgumentNullException("target");
      RaiseEvent(new NativeMotionTransferEventArgs(_info, _nativeSourceDelta) { RoutedEvent = TransferingEvent });

      if (Math.Sign(_nativeSourceDelta) == _info.NativeDirection)
      {
        var converter = _info.Source as INativeMotionConverter;
        double sourceDelta = converter.NativeToNormalized(_nativeSourceDelta);
        double targetDelta = target.Coerce(_info, context, sourceDelta);
        if (!DoubleEx.IsZero(targetDelta))
        {
          target.Move(_info, context, targetDelta);
          EndTransfer(converter.NormalizedToNative(targetDelta), converter.NativeResolutionFrequency);
        }
      }
      return target.CanMove(_info, context);
    }
    #endregion

    #region INativeMotionTransferOutput
    public int NativeRemainder { get { return _nativeSourceDelta; } }

    public bool Transfer(INativeMotionTarget target, object context)
    {
      if (target == null)
        throw new ArgumentNullException("target");
      RaiseEvent(new NativeMotionTransferEventArgs(_info, _nativeSourceDelta) { RoutedEvent = TransferingEvent });

      if (Math.Sign(_nativeSourceDelta) == _info.NativeDirection)
      {
        var converter = _info.Source as INativeMotionConverter;
        int nativeTargetDelta = target.Coerce(_info, context, _nativeSourceDelta);
        if (!DoubleEx.IsZero(nativeTargetDelta))
        {
          target.Move(_info, context, nativeTargetDelta);
          EndTransfer(nativeTargetDelta, converter.NativeResolutionFrequency);
        }
      }
      return target.CanMove(_info, context);
    }
    #endregion

    #region Helpers
    private void EndTransfer(int nativeTargetDelta, int resolutionFrequency)
    {
      _nativeSourceDelta -= nativeTargetDelta;

      var nativeTransferDelta = nativeTargetDelta - _nativeTransferCreditDelta;
      int nativeTransferSnappedDelta = MathEx.Snap(nativeTransferDelta, resolutionFrequency);
      _nativeTransferCreditDelta = nativeTransferSnappedDelta - nativeTransferDelta;

      if (nativeTransferSnappedDelta != 0)
        RaiseEvent(new NativeMotionTransferEventArgs(_info, nativeTransferSnappedDelta) { RoutedEvent = TransferedEvent });
    }
    #endregion
  }
  #endregion
}
