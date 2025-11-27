using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Primitives;
using Game.Models.Inventory;
using Game.Models.Items;

namespace Game.Services.Inventory
{
    /// <summary>
    /// Simple in-memory inventory service.
    /// </summary>
    public class InventoryService : IInventoryService
    {
        private readonly List<InventoryEntry> _items = new List<InventoryEntry>();

        public IReadOnlyList<InventoryEntry> Items => _items.AsReadOnly();

        public event Action InventoryChanged;

        /// <summary>
        /// Add an item to the inventory.
        /// </summary>
        public InventoryResult Add(ItemDefinition item)
        {
            if (item == null) return InventoryResult.Error;

            // check by Id if available
            var existing = _items.Find(e => e.Item != null && e.Item.Id == item.Id);
            if (existing != null)
                return InventoryResult.AlreadyOwned;

            _items.Add(new InventoryEntry(item));
            InventoryChanged?.Invoke();
            return InventoryResult.Success;
        }

        /// <summary>
        /// Replace the whole inventory content.
        /// </summary>
        public void ReplaceAll(IEnumerable<InventoryEntry> items)
        {
            _items.Clear();
            if (items != null)
            {
                _items.AddRange(items);
            }
            InventoryChanged?.Invoke();
        }
    }
}
