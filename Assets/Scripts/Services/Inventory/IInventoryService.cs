using Game.Core.Primitives;
using Game.Models.Inventory;
using Game.Models.Items;
using System.Collections.Generic;

namespace Game.Services.Inventory
{
    public interface IInventoryService
    {
        IReadOnlyList<InventoryEntry> Items { get; }
        InventoryResult Add(ItemDefinition item);
        void ReplaceAll(IEnumerable<InventoryEntry> items);

        event System.Action InventoryChanged;
    }
}