using Game.Core.Primitives;
using Game.Models.Items;
using System.Collections.Generic;

namespace Game.Services.Shop
{
    public interface IShopService
    {
        IReadOnlyList<ItemDefinition> AvailableItems { get; }

        PurchaseResult Purchase(ItemDefinition item);
        IReadOnlyList<ItemDefinition> GetItemsNotOwned();
    }
}
