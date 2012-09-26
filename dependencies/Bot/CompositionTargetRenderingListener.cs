using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Media;

namespace Bot
{
    internal class CompositionTargetRenderingListener :
        System.Windows.Threading.DispatcherObject,
        IDisposable
    {
        public CompositionTargetRenderingListener() { }

        public void StartListening()
        {
            requireAccessAndNotDisposed();

            if (!m_isListening)
            {
                IsListening = true;
                CompositionTarget.Rendering += compositionTarget_Rendering;
            }
        }

        public void StopListening()
        {
            requireAccessAndNotDisposed();

            if (m_isListening)
            {
                IsListening = false;
                CompositionTarget.Rendering -= compositionTarget_Rendering;
            }
        }

        public void WireParentLoadedUnloaded(FrameworkElement parent)
        {
            Contract.Requires(parent != null);
            requireAccessAndNotDisposed();

            parent.Loaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.StartListening();
                };

            parent.Unloaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.StopListening();
                };
        }

        public bool IsListening
        {
            get { return m_isListening; }
            private set
            {
                if (value != m_isListening)
                {
                    m_isListening = value;
                    OnIsListeneningChanged(EventArgs.Empty);
                }
            }
        }

        public bool IsDisposed
        {
            get
            {
                VerifyAccess();
                return m_disposed;
            }
        }

        public event EventHandler Rendering;

        protected virtual void OnRendering(EventArgs args)
        {
            requireAccessAndNotDisposed();

            EventHandler handler = Rendering;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public event EventHandler IsListeningChanged;

        protected virtual void OnIsListeneningChanged(EventArgs args)
        {
            var handler = IsListeningChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void Dispose()
        {
            requireAccessAndNotDisposed();
            StopListening();

            Rendering
                .GetInvocationList()
                .ForEach(d => Rendering -= (EventHandler)d);

            m_disposed = true;
        }

        #region Implementation

        [DebuggerStepThrough]
        private void requireAccessAndNotDisposed()
        {
#if !SILVERLIGHT
            VerifyAccess();
#endif
            Util.ThrowUnless<ObjectDisposedException>(!m_disposed, "This object has been disposed");
        }

        private void compositionTarget_Rendering(object sender, EventArgs e)
        {
#if FRAME_RATE
      m_count++;
#endif
            OnRendering(e);
        }

        private bool m_isListening;
        private bool m_disposed;

#if FRAME_RATE
    private int m_count = 0;
    private int m_startTicks;
#endif

        #endregion
    }
}