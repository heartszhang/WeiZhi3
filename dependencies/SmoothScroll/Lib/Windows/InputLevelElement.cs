using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Logitech.Windows
{
  #region IInputLevelElement
  /// <summary>
  /// Represent common interface for ContentElement and UIElement and UIElement3D classes
  /// </summary>
  public interface IInputLevelElement : IEquatable<IInputLevelElement>, IEquatable<DependencyObject>
  {
    event MouseWheelEventHandler PreviewMouseWheel;
    event MouseWheelEventHandler MouseWheel;
    
    DependencyObject             Proxied { get; }
    InputBindingCollection InputBindings { get; }

    void    RaiseEvent(RoutedEventArgs e);
    void    AddHandler(RoutedEvent routedEvent, Delegate handler);
    void RemoveHandler(RoutedEvent routedEvent, Delegate handler);
  }
  #endregion

  #region InputLevelElementProxy
  /// <summary>
  /// Acts as a base class and factory for ContentElementProxy, UIElementProxy and UIElement3DProxy
  /// </summary>
  public abstract class InputLevelElementProxy : IEquatable<IInputLevelElement>, IEquatable<DependencyObject>
  {
    #region UIElementProxy
    protected class UIElementProxy : InputLevelElementProxy, IInputLevelElement
    {
      #region Initialization
      public UIElementProxy(UIElement proxied) : base(proxied) { }
      #endregion

      #region IInputLevelElement

      #region Events
      public event MouseWheelEventHandler PreviewMouseWheel
      {
        add    { Handle.PreviewMouseWheel += value; }
        remove { Handle.PreviewMouseWheel -= value; }
      }
      public event MouseWheelEventHandler MouseWheel
      {
        add    { Handle.MouseWheel += value; }
        remove { Handle.MouseWheel -= value; }
      }
      #endregion

      #region Queries
      public InputBindingCollection InputBindings { get { return Handle.InputBindings; } }
      #endregion

      #region Methods
      public void    RaiseEvent(RoutedEventArgs e) { Handle.RaiseEvent(e); }
      public void    AddHandler(RoutedEvent routedEvent, Delegate handler) { Handle.AddHandler(routedEvent, handler); }
      public void RemoveHandler(RoutedEvent routedEvent, Delegate handler) { Handle.RemoveHandler(routedEvent, handler); }
      #endregion

      #endregion

      #region Helpers
      private UIElement Handle { get { return Proxied as UIElement; } }
      #endregion
    }
    #endregion

    #region ContentElementProxy
    protected class ContentElementProxy : InputLevelElementProxy, IInputLevelElement
    {
      #region Initialization
      public ContentElementProxy(ContentElement proxied) : base(proxied) { }
      #endregion

      #region IInputLevelElement

      #region Events
      public event MouseWheelEventHandler PreviewMouseWheel
      {
        add    { Handle.PreviewMouseWheel += value; }
        remove { Handle.PreviewMouseWheel -= value; }
      }
      public event MouseWheelEventHandler MouseWheel
      {
        add    { Handle.MouseWheel += value; }
        remove { Handle.MouseWheel -= value; }
      }
      #endregion

      #region Queries
      public InputBindingCollection InputBindings { get { return Handle.InputBindings; } }
      #endregion

      #region Methods
      public void    RaiseEvent(RoutedEventArgs e) { Handle.RaiseEvent(e); }
      public void    AddHandler(RoutedEvent routedEvent, Delegate handler) { Handle.AddHandler(routedEvent, handler); }
      public void RemoveHandler(RoutedEvent routedEvent, Delegate handler) { Handle.RemoveHandler(routedEvent, handler); }
      #endregion

      #endregion

      #region Helpers
      private ContentElement Handle { get { return Proxied as ContentElement; } }
      #endregion
    }
    #endregion

    #region UIElement3DProxy
    protected class UIElement3DProxy : InputLevelElementProxy, IInputLevelElement
    {
      #region Initialization
      public UIElement3DProxy(UIElement3D proxied) : base(proxied) { }
      #endregion

      #region IInputLevelElement

      #region Events
      public event MouseWheelEventHandler PreviewMouseWheel
      {
        add    { Handle.PreviewMouseWheel += value; }
        remove { Handle.PreviewMouseWheel -= value; }
      }
      public event MouseWheelEventHandler MouseWheel
      {
        add    { Handle.MouseWheel += value; }
        remove { Handle.MouseWheel -= value; }
      }
      #endregion

      #region Queries
      public InputBindingCollection InputBindings { get { return Handle.InputBindings; } }
      #endregion

      #region Methods
      public void    RaiseEvent(RoutedEventArgs e) { Handle.RaiseEvent(e); }
      public void    AddHandler(RoutedEvent routedEvent, Delegate handler) { Handle.AddHandler(routedEvent, handler); }
      public void RemoveHandler(RoutedEvent routedEvent, Delegate handler) { Handle.RemoveHandler(routedEvent, handler); }
      #endregion

      #endregion

      #region Helpers
      private UIElement3D Handle { get { return Proxied as UIElement3D; } }
      #endregion
    }
    #endregion

    #region Static
    public static IInputLevelElement FromElement(DependencyObject element)
    {
      IInputLevelElement inputLevelElement = FrameworkLevelElementFactory.FromElement(element);
      if (inputLevelElement != null)
        return inputLevelElement;

      if (element is UIElement)
        return new UIElementProxy(element as UIElement);
      else if (element is ContentElement)
        return new ContentElementProxy(element as ContentElement);
      else if (element is UIElement3D)
        return new UIElement3DProxy(element as UIElement3D);
      else
        return null;
    } 
    #endregion

    #region Instance

    #region Initialization
    protected InputLevelElementProxy(DependencyObject proxiedObject) { Proxied = proxiedObject; }
    #endregion

    #region Object
    public override string ToString() { return Proxied.ToString(); }
    public override int GetHashCode() { return Proxied.GetHashCode(); }

    public override sealed bool Equals(object obj)
    {
      if (obj is IInputLevelElement)
        return Equals(obj as IInputLevelElement);
      else
        return Equals(obj as DependencyObject);
    }
    #endregion

    #region IEquatable<IInputLevelElement>
    public bool Equals(IInputLevelElement other)
    {
      if (ReferenceEquals(this, other)) return true;
      if (ReferenceEquals(other, null)) return false;
      return ReferenceEquals(Proxied, other.Proxied);
    }
    #endregion

    #region IEquatable<DependencyObject>
    public bool Equals(DependencyObject other) { return ReferenceEquals(Proxied, other); }
    #endregion

    #region Properties
    public DependencyObject Proxied { get; private set; } 
    #endregion

    #endregion
  }
  #endregion
}
