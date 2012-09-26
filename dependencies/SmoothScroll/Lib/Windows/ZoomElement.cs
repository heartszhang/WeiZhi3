using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Logitech.Windows
{
  #region IZoomElement
  /// <summary>
  /// Represent common interface for FlowDocumentScrollViewer and FlowDocumentPageViewer classes
  /// </summary>
  public interface IZoomElement : IFrameworkLevelElement
  {
    bool CanIncreaseZoom { get; }
    bool CanDecreaseZoom { get; }
    double       MinZoom { get; set; }
    double       MaxZoom { get; set; }
    double          Zoom { get; set; }
    double ZoomIncrement { get; set; }

    void IncreaseZoom();
    void DecreaseZoom();
  }
  #endregion

  #region ZoomElementFactory
  /// <summary>
  /// Acts as a factory for FlowDocumentScrollViewerProxy and FlowDocumentPageViewerProxy
  /// </summary>
  public abstract class ZoomElementFactory : FrameworkLevelElementFactory
  {
    #region FlowDocumentScrollViewerProxy
    protected class FlowDocumentScrollViewerProxy : FrameworkElementProxy, IZoomElement
    {
      #region Initialization
      public FlowDocumentScrollViewerProxy(FlowDocumentScrollViewer proxied) : base(proxied) { }
      #endregion

      #region IZoomElement

      #region Queries
      public bool CanIncreaseZoom { get { return Handle.CanIncreaseZoom; } }
      public bool CanDecreaseZoom { get { return Handle.CanDecreaseZoom; } }

      #endregion

      #region Properties
      public double       MinZoom { get { return Handle.MinZoom; }       set { Handle.MinZoom       = value; } }
      public double       MaxZoom { get { return Handle.MaxZoom; }       set { Handle.MaxZoom       = value; } }
      public double          Zoom { get { return Handle.Zoom; }          set { Handle.Zoom          = value; } }
      public double ZoomIncrement { get { return Handle.ZoomIncrement; } set { Handle.ZoomIncrement = value; } }
      #endregion

      #region Methods
      public void IncreaseZoom() { Handle.IncreaseZoom(); }
      public void DecreaseZoom() { Handle.DecreaseZoom(); }
      #endregion

      #endregion

      #region Helpers
      private FlowDocumentScrollViewer Handle { get { return Proxied as FlowDocumentScrollViewer; } }
      #endregion
    }
    #endregion

    #region FlowDocumentPageViewerProxy
    protected class FlowDocumentPageViewerProxy : FrameworkElementProxy, IZoomElement
    {
      #region Initialization
      public FlowDocumentPageViewerProxy(FlowDocumentPageViewer proxied) : base(proxied) { }
      #endregion

      #region IZoomElement

      #region Queries
      public bool CanIncreaseZoom { get { return Handle.CanIncreaseZoom; } }
      public bool CanDecreaseZoom { get { return Handle.CanDecreaseZoom; } }

      #endregion

      #region Properties
      public double       MinZoom { get { return Handle.MinZoom; }       set { Handle.MinZoom       = value; } }
      public double       MaxZoom { get { return Handle.MaxZoom; }       set { Handle.MaxZoom       = value; } }
      public double          Zoom { get { return Handle.Zoom; }          set { Handle.Zoom          = value; } }
      public double ZoomIncrement { get { return Handle.ZoomIncrement; } set { Handle.ZoomIncrement = value; } }
      #endregion

      #region Methods
      public void IncreaseZoom() { Handle.IncreaseZoom(); }
      public void DecreaseZoom() { Handle.DecreaseZoom(); }
      #endregion

      #endregion

      #region Helpers
      private FlowDocumentPageViewer Handle { get { return Proxied as FlowDocumentPageViewer; } }
      #endregion
    }
    #endregion

    #region Static
    public static new IZoomElement FromElement(DependencyObject element)
    {
      if (element is FlowDocumentScrollViewer)
        return new FlowDocumentScrollViewerProxy(element as FlowDocumentScrollViewer);
      else if (element is FlowDocumentPageViewer)
        return new FlowDocumentPageViewerProxy(element as FlowDocumentPageViewer);
      else
        return null;
    } 
    #endregion

    #region Instance

    #region Initialization
    protected ZoomElementFactory(DependencyObject proxiedObject) : base(proxiedObject) { }
    #endregion

    #endregion
  }
  #endregion
}
