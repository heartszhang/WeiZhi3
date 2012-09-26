using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitech.Windows.MotionFlow
{
  #region IMotionFilter
  public interface IMotionFilter
  {
    void     NewInputDelta(TimeSpan t, double delta, IMotionInfo info);
    double NextOutputDelta(TimeSpan t);
  }
  #endregion
}
