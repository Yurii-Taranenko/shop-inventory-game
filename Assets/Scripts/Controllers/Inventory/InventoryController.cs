using System;
using Game.Services.Inventory;
using Game.Views.Windows;
using Game.Controllers.Inventory;
using Game.Controllers.Common;

namespace Game.Controllers.Inventory
{
    public class InventoryController : ViewControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly IWindowService _windowService;
        private IInventoryView _view;

        public InventoryController(IInventoryService inventoryService, IWindowService windowService = null)
        {
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _windowService = windowService;
            _inventoryService.InventoryChanged += OnInventoryChanged;
        }

        public void BindView(IInventoryView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            UnbindView();

            _view = view;
            _view.RebuildInventory(_inventoryService.Items);
        }

        public void UnbindView()
        {
            if (_view == null) return;
            // If view had events, they'd be unsubscribed here.
            _view = null;
        }

        private void OnInventoryChanged()
        {
            _view?.RebuildInventory(_inventoryService.Items);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _inventoryService.InventoryChanged -= OnInventoryChanged;
            UnbindView();
        }
    }
}
