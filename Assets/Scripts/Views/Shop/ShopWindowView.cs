using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Game.Controllers.Shop;
using Game.Models.Items;
using Game.Views.UI;
using Game.Views.Windows;

namespace Game.Views.Shop
{
    public class ShopWindowView : WindowBase, IShopWindowView
    {
        [SerializeField] private Transform contentRoot;
        [SerializeField] private GameObject itemSlotPrefab;

        public event Action<ItemDefinition> BuyClicked;

        private void Start()
        {
        }

        public void RebuildList(IReadOnlyList<ItemDefinition> items)
        {
            // clear old UI
            for (int i = contentRoot.childCount - 1; i >= 0; i--)
                Destroy(contentRoot.GetChild(i).gameObject);

            // build UI from model data
            foreach (var item in items)
            {
                var go = Instantiate(itemSlotPrefab, contentRoot);
                var slot = go.GetComponent<ItemSlotView>();

                if (slot != null)
                {
                    // slot.Bind(item, icon?) – you pass icon here if you want
                    slot.Bind(item, true, item.Sprite);

                    slot.Clicked += OnSlotClicked;
                }
            }
        }

        private void OnSlotClicked(ItemDefinition item)
        {
            BuyClicked?.Invoke(item);
        }
    }
}
