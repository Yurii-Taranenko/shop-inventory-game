using UnityEngine;
using UnityEngine.UI;
using Game.Views.UI;
using Game.Models.Inventory;
using System.Collections.Generic;
using Game.Controllers.Inventory;
using Unity.VisualScripting;

namespace Game.Views.Inventory
{
    /// Window view for inventory. Inherits WindowBase to get show/hide behaviour.

    [RequireComponent(typeof(CanvasGroup))]
    public class InventoryWindowView : Game.Views.Windows.WindowBase, IInventoryView
    {
        [Header("UI")]
        [SerializeField] private Transform contentRoot;
        [SerializeField] private GameObject itemSlotPrefab;

        /// Rebuild the inventory UI from a list of InventoryEntry models
        public void RebuildInventory(IReadOnlyList<InventoryEntry> items, System.Func<Game.Models.Items.ItemDefinition, UnityEngine.Sprite> iconResolver = null)
        {
            if (contentRoot == null || itemSlotPrefab == null) return;

            // clear
            for (int i = contentRoot.childCount - 1; i >= 0; i--)
                Destroy(contentRoot.GetChild(i).gameObject);

            for (int i = 0; i < 15; i++)
            {
                var go = Instantiate(itemSlotPrefab, contentRoot, false);
                var slot = go.GetComponent<ItemSlotView>();

                if (items == null) continue;
                if (slot != null)
                {
                    if (items.Count > i)
                    {
                        //Bind real item
                        slot.Bind(items[i].Item, false, items[i].Item.Sprite);
                    }
                    else
                    {
                        //Create inventory empty cell
                        slot.Bind(null, false, null);
                    }
                }
            }
        }

    }
}
