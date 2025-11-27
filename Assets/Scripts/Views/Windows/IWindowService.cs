using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Game.Views.Windows
{
    /// <summary>
    /// Service interface for opening/closing UI windows by ID.
    /// The WindowManager implements this interface and can be registered
    /// in the ServiceLocator for views/controllers to consume.
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Open window by its unique ID. Optional payload can be provided for parameterized opening.
        /// Returns true if the window was found and an open request was issued.
        /// </summary>
        bool Open(string id, object payload = null);

        /// <summary>
        /// Close window by its unique ID. Returns true if the window was found and close request was issued.
        /// </summary>
        bool Close(string id);

        /// <summary>
        /// Returns true if the window with given ID is currently open/visible.
        /// </summary>
        bool IsOpen(string id);

        /// <summary>
        /// Fired after a window was opened. The argument is the window id.
        /// </summary>
        event Action<string> WindowOpened;

        /// <summary>
        /// Fired after a window was closed. The argument is the window id.
        /// </summary>
        event Action<string> WindowClosed;
    }
}
