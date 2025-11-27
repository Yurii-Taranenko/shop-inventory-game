using Game.Services.Currency;

namespace Game.Services.Clicker
{
    /// <summary>
    /// Concrete click service implementation.
    /// </summary>
    public class ClickService : IClickService
    {
        private readonly IIncomeCalculator _incomeCalculator;
        private readonly ICurrencyService _currencyService;

        public ClickService(IIncomeCalculator incomeCalculator, ICurrencyService currencyService)
        {
            _incomeCalculator = incomeCalculator;
            _currencyService = currencyService;
        }

        public int Click()
        {
            int gain = (int)_incomeCalculator.GetClickMultiplier();
            if (gain > 0)
            {
                _currencyService.AddCoins(gain);
            }
            return gain;
        }
    }
}
