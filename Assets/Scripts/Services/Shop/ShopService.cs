using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Primitives;
using Game.Models.Items;
using Game.Services.Currency;
using Game.Services.Inventory;

namespace Game.Services.Shop
{
    /// Shop service.
    /// - Stores available items for sale.
    /// - Provides purchase logic with currency + inventory validation.
    public class ShopService : IShopService
    {
        private readonly List<ItemDefinition> _available = new List<ItemDefinition>();

        private readonly ICurrencyService _currency;
        private readonly IInventoryService _inventory;

        public IReadOnlyList<ItemDefinition> AvailableItems => _available.AsReadOnly();

        /// Construct shop service with initial data.
        /// Provide items from SO provider, or build manually.
        public ShopService(IEnumerable<ItemDefinition> initialItems,
                           ICurrencyService currencyService,
                           IInventoryService inventoryService)
        {
            if (initialItems != null)
                _available.AddRange(initialItems);

            _currency = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
            _inventory = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        }

        /// Main purchase flow
        public PurchaseResult Purchase(ItemDefinition item)
        {
            if (item == null)
                return PurchaseResult.Error;

            // Already in Inventory
            foreach (var entry in _inventory.Items)
            {
                if (entry.Item != null && entry.Item.Id == item.Id)
                    return PurchaseResult.AlreadyOwned;
            }

            if (!_currency.TrySpend(item.Price))
                return PurchaseResult.NotEnoughMoney;

            var result = _inventory.Add(item);
            if (result != InventoryResult.Success)
            {
                // rollback currency on failure (best-effort)
                _currency.AddCoins(item.Price);
                return PurchaseResult.Error;
            }

            return PurchaseResult.Success;
        }
        public IReadOnlyList<ItemDefinition> GetItemsNotOwned()
        {
            var ownedIds = new HashSet<string>(_inventory.Items
                                                .Where(e => e.Item != null)
                                                .Select(e => e.Item.Id));

            var notOwned = _available
                           .Where(def => def != null && !ownedIds.Contains(def.Id))
                           .ToList();

            return notOwned.AsReadOnly();
        }
    }
}
