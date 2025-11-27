using UnityEngine;
using UnityEngine.UI;
using Game.Views.UI;
using Game.Models.Inventory;
using System.Collections.Generic;
using Game.Controllers.Inventory;
using Game.Views.Windows;
using System;

namespace Game.Views.Inventory
{
    /// <summary>
    /// Window view for inventory. Inherits WindowBase to get show/hide behaviour.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class BottomHUDView : WindowBase, IBottomHUDView
    {
        [Header("UI")]
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _shopButton;

        public event Action OnClickInventory;
        public event Action OnClickShop;
        private void Start()
        {
            if (_inventoryButton != null) _inventoryButton.onClick.AddListener(OnInventoryButtonClick);
            if (_shopButton != null) _shopButton.onClick.AddListener(OnShopButtonClick);
        }

        private void OnInventoryButtonClick()
        {
            OnClickInventory?.Invoke();
        }

        private void OnShopButtonClick()
        {
            OnClickShop?.Invoke();
        }

        private void OnDestroy()
        {
            if (_inventoryButton != null) _inventoryButton.onClick.RemoveAllListeners();
            if (_shopButton != null) _shopButton.onClick.RemoveAllListeners();
        }
    }
}
