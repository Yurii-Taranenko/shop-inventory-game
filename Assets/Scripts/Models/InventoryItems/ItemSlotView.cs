using UnityEngine;
using UnityEngine.UI;
using System;
using Game.Models.Items;
using TMPro;

namespace Game.Views.UI
{
    /// UI view representing a single item in shop or inventory.
    public class ItemSlotView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button actionButton;


        /// Fired when the user presses the action button (buy/select/etc.).
        /// Provides the item definition associated with this slot. <summary>
        public event Action<ItemDefinition> Clicked;

        private ItemDefinition _item;

        private void Awake()
        {
            if (actionButton != null)
                actionButton.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            if (actionButton != null)
                actionButton.onClick.RemoveListener(OnClick);
        }

        /// Assigns model data to UI. Use this when building shop inventory.
        public void Bind(ItemDefinition item, bool showPrice, Sprite icon = null)
        {
            if (item != null)
            {
                _item = item;

                if (nameText != null)
                    nameText.text = item.Name;

                if (priceText != null)
                    priceText.text = item.Price.ToString();

                if (iconImage != null)
                    iconImage.sprite = icon;

                if (descriptionText != null)
                    descriptionText.text = item.Description;
            }

            actionButton.gameObject.SetActive(showPrice);

        }

        private void OnClick()
        {
            if (_item != null)
                Clicked?.Invoke(_item);
        }
    }
}
