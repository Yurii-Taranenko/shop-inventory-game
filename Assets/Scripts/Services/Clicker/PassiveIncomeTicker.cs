using System;
using Game.Core.Abstractions;
using Game.Services.Currency;
using Game.Services.Inventory;

namespace Game.Services.Clicker
{
    /// Ticks passive income from inventory.
    public class PassiveIncomeTicker : IPassiveIncomeTicker, IDisposable
    {
        private readonly IUnityTimeProvider _timeProvider;
        private readonly IIncomeCalculator _incomeCalculator;
        private readonly ICurrencyService _currency;
        private readonly IInventoryService _inventory;

        private double _accumulator = 0.0;
        private float _incomePerSecond = 0f;
        private bool _running = false;
        private bool _disposed = false;

        public PassiveIncomeTicker(
            IUnityTimeProvider timeProvider,
            IIncomeCalculator incomeCalculator,
            ICurrencyService currency,
            IInventoryService inventory)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _incomeCalculator = incomeCalculator ?? throw new ArgumentNullException(nameof(incomeCalculator));
            _currency = currency ?? throw new ArgumentNullException(nameof(currency));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));

            RecalculateIncome();

            _inventory.InventoryChanged += OnInventoryChanged;
        }

        /// Start listening ticks.
        public void Start()
        {
            if (_running) return;

            _timeProvider.Tick += OnTick;
            _running = true;
        }

        /// Stop listening ticks.
        public void Stop()
        {
            if (!_running) return;

            _timeProvider.Tick -= OnTick;
            _running = false;
        }

        private void OnInventoryChanged()
        {
            RecalculateIncome();
        }

        private void RecalculateIncome()
        {
            _incomePerSecond = _incomeCalculator.GetPassiveIncomePerSecond();
        }

        private void OnTick(float deltaTime)
        {
            if (_incomePerSecond <= 0f) return;
            CalculateIncomePerSecond(deltaTime);
        }

        private void CalculateIncomePerSecond(float deltaTime)
        {
            _accumulator += _incomePerSecond * deltaTime;

            var whole = (int)Math.Floor(_accumulator);
            if (whole > 0)
            {
                _accumulator -= whole;
                _currency.AddCoins(whole);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            Stop();
            _inventory.InventoryChanged -= OnInventoryChanged;
            _disposed = true;
        }
    }
}
