using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Views.Windows
{
    /// <summary>
    /// Window manager that controls showing/hiding windows by unique ID.
    /// </summary>
    public class WindowsService : IWindowService
    {
        [Tooltip("List of windows managed by this WindowManager.")]

        private Dictionary<string, WindowBase> _windows;

        public event Action<string> WindowOpened;
        public event Action<string> WindowClosed;

        public WindowsService(List<WindowBase> windows) 
        {
            _windows = new Dictionary<string, WindowBase>();
            foreach (WindowBase window in windows)
            {
                if (window == null) continue;
                if (string.IsNullOrEmpty(window.Id))
                {
                    Debug.LogWarning($"Window '{window.name}' has empty ID.");
                    continue;
                }
                if (_windows.ContainsKey(window.Id))
                {
                    Debug.LogWarning($"Duplicate window ID: {window.Id}");
                    continue;
                }
                _windows.Add(window.Id, window);
            }
        }

        public bool Open(string id, object payload = null)
        {
            foreach (KeyValuePair<string,WindowBase> window in _windows)
            {
                if (window.Value.gameObject.activeSelf)
                {
                    if (window.Value.Id != "BottomHUD" && window.Value.Id != "ClickerView")
                    {
                        window.Value.gameObject.SetActive(false);
                    }
                }
            }
            if (!_windows.TryGetValue(id, out var win))
            {
                Debug.LogWarning($"Window '{id}' not found.");
                return false;
            }

            win.Show();
            WindowOpened?.Invoke(id);
            return true;
        }

        public bool Close(string id)
        {
            if (!_windows.TryGetValue(id, out var win))
            {
                Debug.LogWarning($"Window '{id}' not found.");
                return false;
            }

            win.Hide();
            WindowClosed?.Invoke(id);
            return true;
        }

        public bool IsOpen(string id)
        {
            if (!_windows.TryGetValue(id, out var win))
                return false;

            return win.gameObject.activeSelf && win.GetComponent<CanvasGroup>().alpha > 0.9f;
        }
    }
}
