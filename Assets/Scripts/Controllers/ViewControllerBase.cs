using System;

namespace Game.Controllers.Common
{
    /// <summary>
    /// Base helper for controllers that bind to Views.
    /// </summary>
    public abstract class ViewControllerBase : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Unbinds view and disposes controller.
        /// Derived classes should override Dispose(bool) for custom cleanup.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            Dispose(true);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
