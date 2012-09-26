using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logitech.Windows.MotionFlow;

namespace Logitech.Windows.Input
{
  #region IMouseWheelShaft
  public interface IMouseWheelShaft : INativeMotionTransfer
  {
    int Resolution { get; }
  }
  #endregion

  #region MouseWheelShaft
  public class MouseWheelShaft : NativeMotionTransfer, IMouseWheelShaft
  {
    // TODO: still to implement some use monitoring (reference counting)
    // so that parent transfer case can trash it when not used.

    #region Initialization
    public MouseWheelShaft(int resolution)
    {
      Resolution = resolution;
    }
    #endregion

    #region Properties
    public int Resolution { get; private set; }
    #endregion
  }
  #endregion

}
