using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace Logitech.Windows.MotionFlow
{
  #region INativeMotionSourceInput
  public interface INativeMotionSourceInput : INativeMotionInput
  {
    void Transmit(int timeStamp, int nativeDelta);
 }
  #endregion

  #region INativeMotionConverter
  public interface INativeMotionConverter
  {
    int NativeResolutionFrequency { get; }

    double NativeToNormalized(int    value);
    int    NormalizedToNative(double value);
  }
  #endregion

  #region INativeMotionSource
  public interface INativeMotionSource : IMotionInfo, INativeMotionSourceInput, INativeMotionOutput, INativeMotionConverter { } 
  #endregion

  #region NativeMotionSource
  public abstract class NativeMotionSource : MotionElementLink, INativeMotionSource
  {
    #region Fields
    private long _timeStamp = -1;
    private TimeSpan _delay = TimeSpan.Zero;
    private int _previousNativeDirection;
    private int _nativeDirection;
    private double _velocity;
    #endregion

    #region Initialization
    public NativeMotionSource()
    {
      Next = NativeMotionTerminal.Current;
    }
    #endregion

    #region IMotionInfo
    public IMotionInfo    Source { get { return this; } }
    public TimeSpan         Time { get { return TimeSpan.FromMilliseconds(_timeStamp); } }
    public TimeSpan        Delay { get { return _delay; } internal set { _delay = value; } }
    public double       Velocity { get { return _velocity; } }
    public double          Speed { get { return Math.Abs(_velocity); } }
    public int         Direction { get { return -_nativeDirection; } }
    public bool DirectionChanged { get { return _previousNativeDirection != _nativeDirection; } }

    public int NativeDirection
    {
      get { return _nativeDirection; }
      private set
      {
        if (_nativeDirection == value) return;
        _previousNativeDirection = _nativeDirection;
        _nativeDirection = value;
      }
    }
    #endregion

    #region INativeMotionSourceInput
    public void Transmit(int timeStamp, int nativeDelta)
    {
      var info = PreTransmit(timeStamp, nativeDelta);
      Transmit(info, nativeDelta, null);
    }
    #endregion

    #region INativeMotionInput
    public void Transmit(IMotionInfo info, int nativeDelta, INativeMotionOutput source)
    {
      Next.Transmit(info, nativeDelta, this);
    }
    public void OnCoupledTransfer(IMotionInfo info, int nativeDelta, INativeMotionTransferOutput source)
    {
      Next.OnCoupledTransfer(info, nativeDelta, source);
    }
    public void Reset()
    {
      Next.Reset();
    }
    #endregion

    #region INativeMotionConverter
    public abstract int NativeResolutionFrequency { get; }

    public abstract double NativeToNormalized(int    value);
    public abstract int    NormalizedToNative(double value);
    #endregion

    #region INativeMotionOutput
    public INativeMotionInput Next
    {
      [DebuggerStepThrough]
      get { return GetNext() as INativeMotionInput; }
      [DebuggerStepThrough]
      set { SetNext(value); }
    }
    #endregion

    #region Methods
    public virtual IMotionInfo PreTransmit(int timeStamp, int nativeDelta)
    {
      if (nativeDelta != 0)
      {
        NativeDirection = Math.Sign(nativeDelta);

        UpdateTimings(timeStamp);

        var delta = NativeToNormalized(nativeDelta);
        UpdateVelocity(delta);
      }
      return this;
    }
    #endregion

    #region Helpers
    private void UpdateTimings(long timeStamp)
    {
      // for information on timestamp seee http://msdn.microsoft.com/en-us/library/ms644939(VS.85).aspx

      if (timeStamp < 0)
        throw new ArgumentOutOfRangeException("timeStamp");

      if (_timeStamp >= 0)
      {
        // fix timestamp wrapping issue
        if (timeStamp < _timeStamp) timeStamp += int.MaxValue;
        _delay = TimeSpan.FromMilliseconds(timeStamp - _timeStamp);
      }
      _timeStamp = timeStamp;
    }
    private void UpdateVelocity(double delta)
    {
      if (_delay != TimeSpan.Zero)
        _velocity = delta / _delay.TotalSeconds;
    }
    #endregion
  }
  #endregion
}
