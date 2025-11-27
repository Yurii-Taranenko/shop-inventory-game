namespace Game.Services.Currency
{
    public interface ICurrencyService
    {
        int Coins { get; }

        bool TrySpend(int amount);
        void AddCoins(int amount);

        event System.Action<int> CoinsChanged;
    }
}