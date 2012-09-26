using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Logitech.Windows
{
  #region IFrameworkLevelElement
  /// <summary>
  /// Represent common interface for FrameworkElement and FrameworkContentElement classes
  /// </summary>
  public interface IFrameworkLevelElement : IInputLevelElement
  {
    event EventHandler                 Initialized;
    event RoutedEventHandler                Loaded;
    event RoutedEventHandler              Unloaded;

    bool               IsInitialized { get; }
    bool                    IsLoaded { get; }
    DependencyObject TemplatedParent { get; }
  }
  #endregion

  #region FrameworkLevelElementFactory
  /// <summary>
  /// Acts as a factory for FrameworkElementProxy and FrameworkContentElementProxy
  /// </summary>
  public abstract class FrameworkLevelElementFactory : InputLevelElementProxy
  {
    #region FrameworkElementProxy
    protected class FrameworkElementProxy : UIElementProxy, IFrameworkLevelElement
    {
      #region Initialization
      public FrameworkElementProxy(FrameworkElement proxied) : base(proxied) { }
      #endregion

      #region IFrameworkLevelElement

      #region Events
      public event EventHandler Initialized
      {
        add    { Handle.Initialized += value; }
        remove { Handle.Initialized -= value; }
      }
      public event RoutedEventHandler Loaded
      {
        add    { Handle.Loaded += value; }
        remove { Handle.Loaded -= value; }
      }
      public event RoutedEventHandler Unloaded
      {
        add    { Handle.Unloaded += value; }
        remove { Handle.Unloaded -= value; }
      }
      #endregion

      #region Queries
      public bool               IsInitialized { get { return Handle.IsInitialized; } }
      public bool                    IsLoaded { get { return Handle.IsLoaded; } }
      public DependencyObject TemplatedParent { get { return Handle.TemplatedParent; } }
      #endregion

      #endregion

      #region Helpers
      private FrameworkElement Handle { get { return Proxied as FrameworkElement; } }
      #endregion
    }
    #endregion

    #region FrameworkContentElementProxy
    protected class FrameworkContentElementProxy : ContentElementProxy, IFrameworkLevelElement
    {
      #region Initialization
      public FrameworkContentElementProxy(FrameworkContentElement proxied) : base(proxied) { }
      #endregion

      #region IFrameworkLevelElement

      #region Events
      public event EventHandler Initialized
      {
        add    { Handle.Initialized += value; }
        remove { Handle.Initialized -= value; }
      }
      public event RoutedEventHandler Loaded
      {
        add    { Handle.Loaded += value; }
        remove { Handle.Loaded -= value; }
      }
      public event RoutedEventHandler Unloaded
      {
        add    { Handle.Unloaded += value; }
        remove { Handle.Unloaded -= value; }
      }
      #endregion

      #region Queries
      public bool               IsInitialized { get { return Handle.IsInitialized; } }
      public bool                    IsLoaded { get { return Handle.IsLoaded; } }
      public DependencyObject TemplatedParent { get { return Handle.TemplatedParent; } }
      #endregion

      #endregion

      #region Helpers
      private FrameworkContentElement Handle { get { return Proxied as FrameworkContentElement; } }
      #endregion
    }
    #endregion

    #region Static
    public static new IFrameworkLevelElement FromElement(DependencyObject element)
    {
      IFrameworkLevelElement frameworkLevelElement = ZoomElementFactory.FromElement(element);
      if (frameworkLevelElement != null)
        return frameworkLevelElement;
      if (element is FrameworkElement)
        return new FrameworkElementProxy(element as FrameworkElement);
      else if (element is FrameworkContentElement)
        return new FrameworkContentElementProxy(element as FrameworkContentElement);
      else
        return null;
    } 
    #endregion

    #region Instance

    #region Initialization
    protected FrameworkLevelElementFactory(DependencyObject proxiedObject) : base(proxiedObject) { }
    #endregion

    #endregion
  }
  #endregion
}
