using UnityEngine;
using UnityEngine.UI;
using System;
using Game.Controllers.Clicker;
using TMPro;
using Game.Views.Windows;

namespace Game.Views.Clicker
{
    /// <summary>
    /// MonoBehaviour implementation of IClickerView.
    [RequireComponent(typeof(CanvasGroup))]
    public class ClickerView : WindowBase, IClickerView
    {
        [Header("UI References")]
        [SerializeField] private Button _clickButton;
        [SerializeField] private TextMeshProUGUI _coinsText;

        public event Action ClickTriggered;

        private void OnEnable()
        {
            if (_clickButton != null)
            {
                _clickButton.onClick.AddListener(OnClickButton);
            }
        }

        //On click earn coins event
        private void OnClickButton()
        {
            ClickTriggered?.Invoke();
        }

        /// Update the coins text.
        public void UpdateCoins(int coins)
        {
            if (_coinsText != null)
            {
                _coinsText.text = coins.ToString();
            }
        }
        
        //Unsubscribe OnDestory
        private void OnDestroy()
        {
            if (_clickButton != null)
            {
                _clickButton.onClick.RemoveListener(OnClickButton);
            }
        }
    }
}
