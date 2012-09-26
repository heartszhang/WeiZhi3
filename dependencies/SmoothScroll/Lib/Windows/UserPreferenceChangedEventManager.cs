using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace Logitech.Windows
{
  public class UserPreferenceChangedEventManager : WeakEventManager
  {
    #region Static

    #region Queries
    private static UserPreferenceChangedEventManager CurrentManager
    {
      get
      {
        SystemParametersEx.Initialize();
        var managerType = typeof(UserPreferenceChangedEventManager);
        var currentManager = (UserPreferenceChangedEventManager)WeakEventManager.GetCurrentManager(managerType);
        if (currentManager == null)
        {
          currentManager = new UserPreferenceChangedEventManager();
          WeakEventManager.SetCurrentManager(managerType, currentManager);
        }
        return currentManager;
      }
    }
    #endregion

    #region Methods
    public static void AddListener(IWeakEventListener listener)
    {
      if (listener == null)
        throw new ArgumentNullException("listener");
      CurrentManager.ProtectedAddListener(null, listener);
    }
    public static void RemoveListener(IWeakEventListener listener)
    {
      if (listener == null)
        throw new ArgumentNullException("listener");
      CurrentManager.ProtectedRemoveListener(null, listener);
    }
    #endregion

    #endregion

    #region Instance

    #region Initialization
    private UserPreferenceChangedEventManager() { }
    #endregion

    #region WeakEventManager
    protected override void StartListening(object source) { SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged; }
    protected override void  StopListening(object source) { SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged; }
    #endregion

    #region Helpers
    private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e) { base.DeliverEvent(null, e); }
    #endregion

    #endregion
  }
}
