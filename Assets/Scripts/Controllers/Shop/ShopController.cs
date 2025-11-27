using System;
using Game.Services.Shop;
using Game.Services.Inventory;
using Game.Services.Currency;
using Game.Core.Primitives;
using Game.Views.Windows;
using Game.Controllers.Common;

namespace Game.Controllers.Shop
{
    public class ShopController : ViewControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IInventoryService _inventoryService;
        private readonly ICurrencyService _currencyService;
        private readonly IWindowService _windowService;
        private IShopWindowView _view;

        public ShopController(IShopService shopService,
                              IInventoryService inventoryService,
                              ICurrencyService currencyService,
                              IWindowService windowService = null)
        {
            _shopService = shopService ?? throw new ArgumentNullException(nameof(shopService));
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
            _windowService = windowService;
        }

        public void BindView(IShopWindowView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            UnbindView();

            _view = view;
            _view.BuyClicked += OnBuyClicked;
            _view.RebuildList(_shopService.GetItemsNotOwned());
        }

        public void UnbindView()
        {
            if (_view == null) return;
            _view.BuyClicked -= OnBuyClicked;
            _view = null;
        }

        private void OnBuyClicked(Game.Models.Items.ItemDefinition item)
        {
            if (item == null) return;

            var result = _shopService.Purchase(item);
            HandlePurchaseResult(result);
        }

        private void HandlePurchaseResult(PurchaseResult result)
        {
            switch (result)
            {
                case PurchaseResult.Success:
                    _view?.RebuildList(_shopService.GetItemsNotOwned());
                    break;
                case PurchaseResult.NotEnoughMoney:
                    // optionally show feedback via windowService/view
                    break;
                case PurchaseResult.AlreadyOwned:
                    break;
                case PurchaseResult.Error:
                default:
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            UnbindView();
        }
    }
}
