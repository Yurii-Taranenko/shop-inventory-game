using System;
using System.Collections.Generic;
using System.Linq;

using Game.Controllers.Clicker;
using Game.Controllers.Inventory;
using Game.Controllers.Shop;

using Game.Core.Abstractions;
using Game.Core.DTOs;
using Game.Core.ServiceLocator;

using Game.Infrastructure.ItemsProvider;
using Game.Infrastructure.Persistence;
using Game.Infrastructure.UnityAdapters;

using Game.Models.Items;

using Game.Services.Clicker;
using Game.Services.Currency;
using Game.Services.Inventory;
using Game.Services.Scene;
using Game.Services.Shop;

using Game.Views.Clicker;
using Game.Views.Inventory;
using Game.Views.Shop;
using Game.Views.Windows;

using UnityEngine;

namespace Game.Presentation.Bootstrap
{
    /// Compose application services
    /// Register them in ServiceLocator for convenience
    /// Create controllers, bind views and keep references for proper disposal
    /// Load SaveData using SaveManager and restore state <summary>
    /// Compose application services
    
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Providers")]
        [SerializeField] private ScriptableItemProvider itemProvider;

        [Header("Scene references")]
        [SerializeField] private ShopWindowView _shopWindowView;
        [SerializeField] private InventoryWindowView _inventoryWindowView;
        [SerializeField] private BottomHUDView _bottomHudView;
        [SerializeField] private ClickerView _clickerView;

        [Header("Startup")]
        [SerializeField] private int initialCoins = 1000;
        [SerializeField] private string saveFileName = "save.json";

        // runtime fields
        private ICurrencyService _currencyService;
        private IInventoryService _inventoryService;
        private IShopService _shopService;
        private IIncomeCalculator _incomeCalculator;
        private IClickService _clickService;
        private IPassiveIncomeTicker _passiveTicker;
        private IUnityTimeProvider _timeProvider;
        private IWindowService _windowsService;
        private IRepo<SaveData> _repo;
        private ISaveManager _saveManager;

        // keep created controllers to Dispose()
        private readonly List<IDisposable> _createdControllers = new();

        private void Awake()
        {
            // Clear any previous registrations
            ServiceLocator.ClearAll();

            _repo = new JsonSaveSystem(saveFileName);
            ServiceLocator.Register<IRepo<Game.Core.DTOs.SaveData>>(_repo);
            // wrap repo into SaveManager
            _saveManager = new SaveManager(_repo);
            ServiceLocator.Register<ISaveManager>(_saveManager);

            // Core services
            _currencyService = new CurrencyService(initialCoins);
            ServiceLocator.Register<ICurrencyService>(_currencyService);

            _inventoryService = new InventoryService();
            ServiceLocator.Register<IInventoryService>(_inventoryService);

            // Restore saved state 
            var saved = _saveManager.Load();
            if (saved != null)
            {
                //I think it's ok for bootstraper in current version
                RestoreData(saved);
            }

            // Window service
            _windowsService = new WindowsService(new List<WindowBase>() { _shopWindowView, _inventoryWindowView, _bottomHudView, _clickerView });
            ServiceLocator.Register<IWindowService>(_windowsService);

            // items / Shop
            var items = (itemProvider != null) ? itemProvider.GetAllDefinitions().ToList()
                                               : new List<ItemDefinition>();

            _shopService = new ShopService(items, _currencyService, _inventoryService);
            ServiceLocator.Register<IShopService>(_shopService);

            // If there's a serialized provider attached at editor time it will be used,
            // otherwise create one on this GameObject.
            if (_timeProvider == null)
            {
                var tpComp = gameObject.AddComponent<UnityTimeProvider>();
                _timeProvider = tpComp;
            }
            ServiceLocator.Register<IUnityTimeProvider>(_timeProvider);

            // === Income / Clicker ===
            _incomeCalculator = new IncomeCalculator(_inventoryService);
            ServiceLocator.Register<IIncomeCalculator>(_incomeCalculator);

            _clickService = new ClickService(_incomeCalculator, _currencyService);
            ServiceLocator.Register<IClickService>(_clickService);

            _passiveTicker = new PassiveIncomeTicker(_timeProvider, _incomeCalculator, _currencyService, _inventoryService);
            ServiceLocator.Register<IPassiveIncomeTicker>(_passiveTicker);
            _passiveTicker.Start();

            // Controllers: create, bind views, and register
            var shopController = new ShopController(_shopService, _inventoryService, _currencyService, _windowsService);
            if (_shopWindowView != null) shopController.BindView(_shopWindowView);
            _createdControllers.Add(shopController);
            ServiceLocator.Register<ShopController>(shopController);

            var inventoryController = new InventoryController(_inventoryService, _windowsService);
            if (_inventoryWindowView != null) inventoryController.BindView(_inventoryWindowView);
            _createdControllers.Add(inventoryController);
            ServiceLocator.Register<InventoryController>(inventoryController);

            var bottomHudController = new BottomHUDController(_windowsService);
            if (_bottomHudView != null) bottomHudController.BindView(_bottomHudView);
            _createdControllers.Add(bottomHudController);
            ServiceLocator.Register<BottomHUDController>(bottomHudController);

            var clickerController = new ClickerController(_clickService, _passiveTicker, _currencyService);
            if (_clickerView != null) clickerController.BindView(_clickerView);
            _createdControllers.Add(clickerController);
            ServiceLocator.Register<ClickerController>(clickerController);

            // Scene loader
            var sceneLoader = new SceneLoaderService();
            ServiceLocator.Register<ISceneLoader>(sceneLoader);
            sceneLoader.LoadScene(
                sceneName: "Game",
                additive: true,
                onProgress: p => Debug.Log($"Loading: {p:P0}"),
                onComplete: () => Debug.Log("Game Scene Loaded!")
            );

            Debug.Log("Bootstrapper: initialization complete. Services registered in ServiceLocator.");

            _windowsService.Open("BottomHUD");
            _windowsService.Open("ClickerView");

            DontDestroyOnLoad(this);
        }

        private void RestoreData(SaveData saved)
        {
            if (_currencyService != null && saved.Coins > 0)
                _currencyService.AddCoins(saved.Coins);

            if (itemProvider != null && saved.OwnedItemIds != null && saved.OwnedItemIds.Count > 0)
            {
                var restored = saved.OwnedItemIds
                    .Select(id => itemProvider.GetById(id))
                    .Where(d => d != null)
                    .Select(d => new Game.Models.Inventory.InventoryEntry(d))
                    .ToList();

                _inventoryService.ReplaceAll(restored);
            }
        }

        private void OnApplicationQuit()
        {
            // stop passive ticker to avoid processing
            _passiveTicker?.Stop();

            // save data
            var sd = new SaveData
            {
                Coins = _currencyService?.Coins ?? 0,
                OwnedItemIds = _inventoryService?.Items.Select(e => e.Item.Id).ToList() ?? new List<string>()
            };

            _saveManager?.Save(sd);
        }

        private void OnDestroy()
        {
            // Dispose created controllers
            foreach (var c in _createdControllers)
            {
                try { c?.Dispose(); } catch {}
            }
            _createdControllers.Clear();
        }
    }
}
