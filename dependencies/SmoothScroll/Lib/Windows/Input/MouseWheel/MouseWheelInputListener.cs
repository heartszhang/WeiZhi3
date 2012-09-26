using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Logitech.Windows.Input
{
  #region IMouseWheelInputListener
  public interface IMouseWheelInputListener
  {
    void OnPreviewInput(object sender, MouseWheelInputEventArgs e);
    void        OnInput(object sender, MouseWheelInputEventArgs e);
  }
  #endregion
}
