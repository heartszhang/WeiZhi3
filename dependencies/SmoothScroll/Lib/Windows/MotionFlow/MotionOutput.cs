using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitech.Windows.MotionFlow
{
  #region INativeMotionOutput
  public interface INativeMotionOutput
  {
    INativeMotionInput Next { get; set; }
  }
  #endregion

  #region IMotionOutput
  public interface IMotionOutput
  {
    IMotionInput Next { get; set; }
  }
  #endregion

}
