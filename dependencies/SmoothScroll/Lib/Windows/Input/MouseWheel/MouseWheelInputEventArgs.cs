using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using Logitech.Windows.MotionFlow;
using System.Diagnostics;

namespace Logitech.Windows.Input
{
  #region MouseWheelInputEventArgs
  public class MouseWheelInputEventArgs : MouseEventArgs
  {
    #region MouseWheelNativeMotionTarget
    private class MouseWheelNativeMotionTarget : INativeMotionTarget
    {
      #region Singleton
      private static readonly INativeMotionTarget _current = new MouseWheelNativeMotionTarget();

      public static INativeMotionTarget Current { get { return _current; } }
      #endregion
      
      #region INativeMotionTarget
      public bool CanMove(IMotionInfo info, object context) { return true; }
      public int   Coerce(IMotionInfo info, object context, int nativeDelta) { return nativeDelta; }
      public void    Move(IMotionInfo info, object context, int nativeDelta)
      {
        var e = context as MouseWheelInputEventArgs;
        var args = e.NativeDeltaToNativeEventArgs(nativeDelta);
        if (args != null)
          e.Controller.ExitElement.RaiseEvent(args);
      }
      #endregion
    } 
    #endregion

    #region Initialization
    public MouseWheelInputEventArgs(IMouseWheelController controller, MouseWheel wheel, MouseWheelEventArgs nativeEventArgs)
      : base(nativeEventArgs.MouseDevice, nativeEventArgs.Timestamp)
    {
      Controller = controller;
      Wheel = wheel;
      NativeEventArgs = nativeEventArgs;
    }
    #endregion

    #region Properties
    public MouseWheelEventArgs NativeEventArgs { get; private set; }
    public MouseWheel                    Wheel { get; private set; }
    public IMouseWheelController    Controller { get; set; }
    #endregion

    #region MouseEventArgs
    protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
    {
      var handler = (MouseWheelInputEventHandler)genericHandler;
      handler(genericTarget, this);
    }
    #endregion

    #region Methods
    public void RaiseNativeEvent(int nativeDelta) { Controller.InputElement.RaiseEvent(NativeDeltaToNativeEventArgs(nativeDelta)); }

    public void EndCommand()
    {
      if (Handled)
        Controller.ExitElement.RaiseEvent(CreateNativeEventArgs(Timestamp, NativeEventArgs.Delta));
      else
        Handled = Wheel.Transfer(MouseWheelNativeMotionTarget.Current, this);
    }
    #endregion

    #region Helpers
    private MouseWheelEventArgs CreateNativeEventArgs(int timestamp, int delta)
    {
      return new MouseWheelEventArgs(Wheel.MouseDevice, timestamp, delta)
      {
        RoutedEvent = UIElement.MouseWheelEvent,
        Handled = Handled
      };
    }
    private IEnumerable<MouseWheelEventArgs> NormalizedDeltaToNativeEventArgs(int normalizedDelta)
    {
      if (normalizedDelta == 0)
        yield break;
      int clickDisplacement = Wheel.NormalizedToNative(Math.Sign(normalizedDelta));
      for (int i = 0; i < Math.Abs(normalizedDelta); ++i)
        yield return CreateNativeEventArgs(Timestamp + i * 10, clickDisplacement);
    }
    private MouseWheelEventArgs NativeDeltaToNativeEventArgs(int nativeDelta)
    {
      return nativeDelta == 0 ? null : CreateNativeEventArgs(Timestamp, nativeDelta);
    }
    #endregion
  }
  #endregion

  #region MouseWheelInputEventHandler
  public delegate void MouseWheelInputEventHandler(object sender, MouseWheelInputEventArgs e);
  #endregion
}
