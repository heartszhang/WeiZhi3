using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Logitech.Windows.Input
{
  public class MouseWheelFrameworkLevelController : MouseWheelController
  {
    #region Initialization
    public MouseWheelFrameworkLevelController(IFrameworkLevelElement frameworkLevelElement)
      : base(frameworkLevelElement)
    {
      frameworkLevelElement.Unloaded += OnElementUnloaded;
    }
    #endregion

    #region IDisposable
    public override void Dispose()
    {
      FrameworkLevelElement.Unloaded -= OnElementUnloaded;
      base.Dispose();
    }
    #endregion

    #region Queries
    public IFrameworkLevelElement FrameworkLevelElement { get { return InputLevelElement as IFrameworkLevelElement; } }
    #endregion

    #region Helpers

    #region Callbacks
    private void OnElementUnloaded(object sender, RoutedEventArgs e) { Unload(); }
    #endregion

    #endregion
  }
}
