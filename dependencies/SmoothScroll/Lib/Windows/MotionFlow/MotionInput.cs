using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Logitech.Windows.MotionFlow
{
  #region INativeMotionInput
  public interface INativeMotionInput : IMotionElementInput
  {
    void          Transmit(IMotionInfo info, int nativeDelta, INativeMotionOutput         source);
    void OnCoupledTransfer(IMotionInfo info, int nativeDelta, INativeMotionTransferOutput source);
    void             Reset();
  }
  #endregion

  #region IMotionInput
  public interface IMotionInput : IMotionElementInput
  {
    void          Transmit(IMotionInfo info, double delta, IMotionOutput         source);
    void OnCoupledTransfer(IMotionInfo info, double delta, IMotionTransferOutput source);
    void             Reset();
  }
  #endregion
}
