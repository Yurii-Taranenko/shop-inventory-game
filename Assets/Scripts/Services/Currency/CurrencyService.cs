using System;

namespace Game.Services.Currency
{
    // Implementation of game currency service. Initiate/Add/Spend Coins.
    public class CurrencyService : ICurrencyService
    {
        private int _coins;

        public int Coins => _coins;

        public event Action<int> CoinsChanged;

        public CurrencyService(int initialCoins = 0)
        {
            _coins = Math.Max(0, initialCoins);
        }

        public bool TrySpend(int amount)
        {
            if (amount <= 0)
                return true;

            if (_coins < amount)
                return false;

            _coins -= amount;
            CoinsChanged?.Invoke(_coins);
            return true;
        }

        public void AddCoins(int amount)
        {
            if (amount <= 0)
                return;

            _coins += amount;
            CoinsChanged?.Invoke(_coins);
        }
    }
}
