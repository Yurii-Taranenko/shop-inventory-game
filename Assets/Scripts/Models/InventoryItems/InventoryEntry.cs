using Game.Models.Items;

namespace Game.Models.Inventory
{
    /// <summary>
    /// Represents a single owned inventory item.
    public class InventoryEntry
    {
        public ItemDefinition Item { get; }

        public InventoryEntry(ItemDefinition item)
        {
            Item = item;
        }
    }
}
