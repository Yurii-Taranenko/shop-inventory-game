using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.Windows
{
    /// <summary>
    /// Base class for all windows. Handles visibility and basic fade-in/out logic.
    /// Views inheriting from WindowBase should only contain small UI wiring logic.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class WindowBase : MonoBehaviour
    {
        [Tooltip("Unique window ID used by WindowManager.")]
        [SerializeField] private string _windowId;
        [SerializeField] private Button _closeButton;
        protected CanvasGroup canvasGroup;

        public string Id => _windowId;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            if (_closeButton != null)
            {
                _closeButton.onClick.AddListener(Hide);
            }
        }

        /// <summary>
        /// Immediately hide window without animation.
        /// </summary>
        public virtual void HideInstant()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Immediately show window without animation.
        /// </summary>
        public virtual void ShowInstant()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Animated show using alpha fade.
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f; // Replace with tween if needed
        }

        /// <summary>
        /// Animated hide using alpha fade.
        /// </summary>
        public virtual void Hide()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f; // Replace with tween if needed
            gameObject.SetActive(false);
        }
    }
}
