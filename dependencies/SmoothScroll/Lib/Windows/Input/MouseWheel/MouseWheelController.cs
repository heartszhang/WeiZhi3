using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Logitech.Windows.Input
{
  #region IMouseWheelController
  public interface IMouseWheelController
  {
    DependencyObject   Element { get; }
    IInputElement InputElement { get; }
    IInputElement  ExitElement { get; }

    void AddClient(IMouseWheelClient client);
  }
  #endregion

  #region MouseWheelController
  public partial class MouseWheelController : IMouseWheelController, IDisposable
  {
    #region ClientType
    [Flags]
    private enum ClientType
    {
      Patch   = 0x01,
      Adapter = 0x02
    }
    #endregion

    #region Constants
    public static readonly RoutedEvent PreviewMouseWheelInputEvent = EventManager.RegisterRoutedEvent("PreviewMouseWheelInput", RoutingStrategy.Tunnel, typeof(MouseWheelInputEventHandler), typeof(IInputElement));
    public static readonly RoutedEvent        MouseWheelInputEvent = EventManager.RegisterRoutedEvent(       "MouseWheelInput", RoutingStrategy.Bubble, typeof(MouseWheelInputEventHandler), typeof(IInputElement));
    #endregion

    #region Fields
    private readonly List<IMouseWheelClient> _clients = new List<IMouseWheelClient>();
    private readonly IInputLevelElement _inputLevelElement;
    private IInputElement _exitElement;
    private ClientType _clientType;
    #endregion

    #region Initialization
    public MouseWheelController(IInputLevelElement inputLevelElement)
    {
      if (inputLevelElement == null)
        throw new ArgumentNullException("inputLevelElement");
      _inputLevelElement = inputLevelElement;

      InputLevelElement.PreviewMouseWheel += OnPreviewMouseWheel;
      InputLevelElement.AddHandler(PreviewMouseWheelInputEvent, new MouseWheelInputEventHandler(OnPreviewInput));
      InputLevelElement.AddHandler(       MouseWheelInputEvent, new MouseWheelInputEventHandler(OnInput));
    }
    #endregion

    #region IDisposable
    public virtual void Dispose()
    {
      InputLevelElement.PreviewMouseWheel -= OnPreviewMouseWheel;
      InputLevelElement.RemoveHandler(PreviewMouseWheelInputEvent, new MouseWheelInputEventHandler(OnPreviewInput));
      InputLevelElement.RemoveHandler(       MouseWheelInputEvent, new MouseWheelInputEventHandler(OnInput));
      Unload();
    }
    #endregion

    #region IMouseWheelController
    public DependencyObject   Element { get { return _inputLevelElement.Proxied; } }
    public IInputElement InputElement { get { return Element as IInputElement; } }

    public IInputElement ExitElement
    {
      get
      {
        if (_exitElement == null)
          _exitElement = Element.GetFirstVisualAncestorOfType<IInputElement>();
        return _exitElement;
      }
    }
    public void AddClient(IMouseWheelClient client)
    {
      _clients.Add(client);
    }
    #endregion

    #region Queries
    public IInputLevelElement InputLevelElement { get { return _inputLevelElement; } }
    #endregion

    #region Methods
    protected void Unload()
    {
      foreach (var client in _clients)
        client.Unload();
    }
    #endregion

    #region Helpers

    #region Queries

    #endregion

    #region Methods
    private IInputElement GetOriginalSource(MouseWheelEventArgs e)
    {
      if (e.OriginalSource is ContentElement)
      {
        var ie = Element as IInputElement;
        var pt = e.GetPosition(ie);
        if (ie is Visual)
        {
          var result = VisualTreeHelper.HitTest(ie as Visual, pt);
          return GetOriginalSource(result.VisualHit);
        }
        return null;
      }
      else
        return GetOriginalSource(e.OriginalSource);
    }
    private IInputElement GetOriginalSource(object originalSource)
    {
      if (originalSource is IInputElement)
        return originalSource as IInputElement;
      else if (originalSource is DependencyObject)
        return (originalSource as DependencyObject).GetFirstVisualAncestorOfType<IInputElement>();
      return null;
    }
    #endregion

    #region Callbacks
    private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
      // update wheel motion info
      var wheel = e.MouseDevice.GetWheel();
      var info = wheel.PreTransmit(e.Timestamp, e.Delta);

      // 1. Tunneling event
      // Clients and behaviors use this tunneling event to update the wheel transfer
      // case by dynamically creating / retrieving motion shafts.
      var originalSource = GetOriginalSource(e);
      var inputEventArgs = new MouseWheelInputEventArgs(this, wheel, e) { RoutedEvent = PreviewMouseWheelInputEvent };
      originalSource.RaiseEvent(inputEventArgs);

      // In cooperation with clients and behaviors, if inputEventArgs.Handled is set to true,
      // the controller lets the underlying mouse wheel tunneling event continue its route.
      if (inputEventArgs.Handled)
        return;

      // Fill motion reservoir
      wheel.Transmit(info, e.Delta, null);
      // 2. Bubbling event
      // Clients consume the motion here
      inputEventArgs.RoutedEvent = MouseWheelInputEvent;
      originalSource.RaiseEvent(inputEventArgs);
      // 3. Remaining motion is processed here
      inputEventArgs.EndCommand();
      e.Handled = true;
    }
    private void OnPreviewInput(object sender, MouseWheelInputEventArgs e)
    {
      Debug.Assert(sender == Element);
      var client = _clients.FirstOrDefault(c => c.IsActive);
      if (client != null)
        client.OnPreviewInput(sender, e);
    }
    private void OnInput(object sender, MouseWheelInputEventArgs e)
    {
      Debug.Assert(sender == Element);
      e.Controller = this;
      var client = _clients.FirstOrDefault(c => c.IsActive);
      if (client != null)
      {
        _exitElement = client.ExitElement;
        client.OnInput(sender, e);
      }
    }
    #endregion

    #endregion
  }
  #endregion

  #region MouseWheelController - repository
  public partial class MouseWheelController
  {
    #region Constants
    private static readonly Func<IMouseWheelController, IMouseWheelClient> _adaptationClientFactory = controller => new MouseWheelAdaptationClient(controller);
    private static readonly Dictionary<Type, IEnumerable<Func<IMouseWheelController, IMouseWheelClient>>> _patchClientFactories = new Dictionary<Type, IEnumerable<Func<IMouseWheelController, IMouseWheelClient>>>();
    private static readonly Dictionary<DependencyObject, IMouseWheelController> _controllers = new Dictionary<DependencyObject, IMouseWheelController>();
    #endregion

    #region Initialization
    static MouseWheelController()
    {
      InitializePatchClientFactories();
    }
    #endregion

    #region Methods
    internal static void BeginEnsurePatchController(DependencyObject element, DependencyPropertyChangedEventArgs e) { BeginEnsureController(element, e, ClientType.Patch); }
    internal static void   BeginEnsureMapController(DependencyObject element, DependencyPropertyChangedEventArgs e) { BeginEnsureController(element, e, ClientType.Adapter); }
    #endregion

    #region Helpers

    #region Controller Repository
    private static void BeginEnsureController(DependencyObject element, DependencyPropertyChangedEventArgs e, ClientType clientType)
    {
      var inputLevelElement = InputLevelElementProxy.FromElement(element);
      if (inputLevelElement == null) return;

      element.Dispatcher.BeginInvoke(
        new PropertyChangedCallback((element1, e1) => EnsureController(inputLevelElement, element1, clientType)),
        DispatcherPriority.Loaded, element, e);
    }
    private static void EnsureController(IInputLevelElement inputLevelElement, DependencyObject element, ClientType clientType)
    {
      var clientFactories = GetClientFactories(inputLevelElement, element, clientType);
      if (clientFactories.FirstOrDefault() == null) return;
      IMouseWheelController controller;
      if (_controllers.TryGetValue(element, out controller))
      {
        if (0 == ((controller as MouseWheelController)._clientType & clientType))
        {
          foreach (var clientFactory in clientFactories)
            controller.AddClient(clientFactory(controller));
          (controller as MouseWheelController)._clientType |= clientType;
        }
      }
      else
      {
        var controllerFactory = GetControllerFactory(inputLevelElement);
        _controllers[element] = controller = controllerFactory(element);
        (controller as MouseWheelController)._clientType |= clientType;
        foreach (var clientFactory in clientFactories)
          controller.AddClient(clientFactory(controller));
      }
    }
    #endregion

    #region Controller Factory
    private static Func<DependencyObject, IMouseWheelController> GetControllerFactory(IInputLevelElement inputLevelElement)
    {
      if (inputLevelElement is IFrameworkLevelElement)
        return s => new MouseWheelFrameworkLevelController(inputLevelElement as IFrameworkLevelElement);
      else
        return s => new MouseWheelController(inputLevelElement);
    }
    #endregion

    #region Client Factories
    private static void InitializePatchClientFactories()
    {
      _patchClientFactories.Add(typeof(ScrollViewer), new Func<IMouseWheelController, IMouseWheelClient>[]
      {
        controller => new MouseWheelScrollClient(controller, Orientation.Vertical),
        controller => new MouseWheelScrollClient(controller, Orientation.Horizontal),
      });
      _patchClientFactories.Add(typeof(FlowDocumentPageViewer), new Func<IMouseWheelController, IMouseWheelClient>[]
      {
        controller => new MouseWheelFlowDocumentPageViewerScrollClient(controller),
        controller => new MouseWheelZoomClient(controller),
      });
      _patchClientFactories.Add(typeof(FlowDocumentScrollViewer), new Func<IMouseWheelController, IMouseWheelClient>[]
      {
        controller => new MouseWheelZoomClient(controller),
      });
    }
    private static IEnumerable<Func<IMouseWheelController, IMouseWheelClient>> GetClientFactories(IInputLevelElement inputLevelElement, DependencyObject element, ClientType clientType)
    {
      if (0 != (clientType & ClientType.Patch))
      {
        var elementType = element.GetType();
        var key = _patchClientFactories.Keys.FirstOrDefault(t => t.IsAssignableFrom(elementType));
        if (key != null)
        {
          foreach (var factory in _patchClientFactories[key])
            yield return factory;
        }
      }
      if (0 != (clientType & ClientType.Adapter))
        yield return _adaptationClientFactory;
    }
    #endregion

    #endregion
  }
  #endregion
}
