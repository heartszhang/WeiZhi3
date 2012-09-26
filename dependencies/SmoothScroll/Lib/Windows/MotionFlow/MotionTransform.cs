using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using Logitech.Maths;

namespace Logitech.Windows.MotionFlow
{
  #region IMotionTransform
  public interface IMotionTransform : IMotionInput, IMotionOutput { }
  #endregion

  #region INativeMotionTransform
  public interface INativeMotionTransform : INativeMotionInput, INativeMotionOutput { }
  #endregion

  #region NativeMotionTransform
  public class NativeMotionTransform : MotionElementLink, INativeMotionTransform
  {
    #region Initialization
    public NativeMotionTransform()
    {
      Next = NativeMotionTerminal.Current;
    }
    #endregion

    #region INativeMotionInput
    public virtual void Transmit(IMotionInfo info, int nativeDelta, INativeMotionOutput source)
    {
      var nativeDeltaY = Transform(info, nativeDelta);
      //Debug.WriteLine("{0}{1}(delta = {2,4}) --> {3,4}", Name, TransmitMethodSuffix(info, nativeDelta), nativeDelta, nativeDeltaY);
      Next.Transmit(info, nativeDeltaY, this);
    }
    public virtual void OnCoupledTransfer(IMotionInfo info, int nativeDelta, INativeMotionTransferOutput source)
    {
      Next.OnCoupledTransfer(info, nativeDelta, source);
    }
    public virtual void Reset()
    {
      Next.Reset();
    }
    #endregion

    #region INativeMotionOutput
    public virtual INativeMotionInput Next
    {
      get { return GetNext() as INativeMotionInput; }
      set { SetNext(value); }
    }
    #endregion

    #region Overridables
    protected virtual int Transform(IMotionInfo info, int nativeDelta) { return nativeDelta; }
    #endregion
  }
  #endregion

  #region NativeDebouncedMotionTransformBase
  public class NativeDebouncedMotionTransformBase : NativeMotionTransform
  {
    #region Fields
    private readonly Int32DifferentialFunctionPatternModulator _debouncingFunction;
    #endregion

    #region Initialization
    public NativeDebouncedMotionTransformBase(Int32DifferentialFunctionPatternModulator debouncingFunction)
    {
      _debouncingFunction = debouncingFunction;
    }
    #endregion

    #region NativeMotionTransform
    public override void Transmit(IMotionInfo info, int nativeDelta, INativeMotionOutput source)
    {
      var nativeDeltaY = Transform(info, nativeDelta);
      //Debug.WriteLine("{0}{1}(delta = {2,4}) --> {3,4}", Name, TransmitMethodSuffix(info, nativeDelta), nativeDelta, nativeDeltaY);
      Next.Transmit(info, nativeDeltaY, source);
    }
    public override void Reset()
    {
      _debouncingFunction.Reset();
      base.Reset();
    }
    protected override int Transform(IMotionInfo info, int nativeDelta)
    {
      return DebouncingFunction.DF(nativeDelta);
    }
    #endregion

    #region Queries
    public Int32DifferentialFunctionPatternModulator DebouncingFunction { [DebuggerStepThrough]get { return _debouncingFunction; } }
    #endregion
  }
  #endregion

  #region NativeDebouncedMotionTransform
  public class NativeDebouncedMotionTransform : NativeDebouncedMotionTransformBase
  {
    #region CompensationTransform
    public class CompensationTransform : NativeDebouncedMotionTransformBase
    {
      #region Initialization
      public CompensationTransform(Int32DifferentialFunctionPatternModulator debouncingFunction)
        : base(debouncingFunction)
      {
        Name = Id.ToString("'r'00");
      }
      #endregion

      #region NativeMotionTransform
      public override INativeMotionInput Next
      {
        get { return GetNext(false) as INativeMotionInput; }
        set { SetNext(value, false); }
      }
      #endregion
    }
    #endregion

    #region Fields
    private INativeMotionTransform _compensationInput;
    private object _transferSource;
    #endregion

    #region Initialization
    public NativeDebouncedMotionTransform(Int32DifferentialFunctionPatternModulator debouncingFunction)
      : base(debouncingFunction)
    {
      Initialize();
    }
    #endregion

    #region NativeMotionTransform
    public override void OnCoupledTransfer(IMotionInfo info, int nativeDelta, INativeMotionTransferOutput source)
    {
      if (source != _transferSource)
        CompensationInput.Transmit(info, -nativeDelta, this);
    }
    public override void Reset()
    {
      if (_compensationInput != null)
        _compensationInput.Reset();
      base.Reset();
    }
    #endregion

    #region Helpers
    private INativeMotionInput CompensationInput
    {
      get
      {
        if (_compensationInput == null)
        {
          _compensationInput = new CompensationTransform(DebouncingFunction.Clone());
          _compensationInput.Next = Next;
        }
        return _compensationInput;
      }
    }
    private void Initialize()
    {
      Name = Id.ToString("'R'00");
      AddHandler(NativeMotionTransfer.TransferingEvent, new NativeMotionTransferEventHandler(OnMotionTransfering));
    }
    private void OnMotionTransfering(object sender, NativeMotionTransferEventArgs e)
    {
      _transferSource = e.Source;
    }
    #endregion
  }
  #endregion
}
