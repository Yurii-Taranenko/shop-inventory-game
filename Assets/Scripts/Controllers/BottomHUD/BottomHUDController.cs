using System;
using Game.Services.Inventory;
using Game.Views.Windows;
using Game.Controllers.Inventory;
using Game.Controllers.Common; // for IInventoryView

namespace Game.Controllers.Inventory
{
    /// <summary>
    /// Controller for inventory UI.
    /// - Keeps view in sync with InventoryService.
    /// </summary>
    public class BottomHUDController : ViewControllerBase
    {
        private readonly IWindowService _windowService;
        private IBottomHUDView _view;

        public BottomHUDController(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void BindView(IBottomHUDView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            UnbindView();

            _view = view;
            _view.OnClickShop += OnClickShop;
            _view.OnClickInventory += OnClickInventory;
        }

        private void OnClickInventory()
        {
            _windowService.Open("InventoryWindow");
        }

        private void OnClickShop()
        {
            _windowService.Open("ShopWindow");
        }

        public void UnbindView()
        {
            if(_view == null) return;

            _view.OnClickShop -= OnClickShop;
            _view.OnClickInventory -= OnClickInventory;
            _view = null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            UnbindView();
        }
    }
}
