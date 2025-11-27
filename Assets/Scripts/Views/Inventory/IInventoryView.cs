using Game.Models.Inventory;
using System.Collections.Generic;

namespace Game.Controllers.Inventory
{
    public interface IInventoryView
    {
        void RebuildInventory(IReadOnlyList<InventoryEntry> items, System.Func<Game.Models.Items.ItemDefinition, UnityEngine.Sprite> iconResolver = null);
    }
}