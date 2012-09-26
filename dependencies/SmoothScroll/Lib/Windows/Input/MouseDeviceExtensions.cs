using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Logitech.Windows.Input
{
  public static class MouseDeviceExtensions
  {
    #region Methods
    public static MouseWheel GetWheel(this MouseDevice source)
    {
      return MouseWheel.Wheels.EnsureItem(source, mouseDevice => new MouseWheel(mouseDevice, MouseWheel.Wheels.Count));
    }
    #endregion
  }
}
