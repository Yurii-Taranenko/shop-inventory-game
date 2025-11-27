using System;
using Game.Services.Clicker;
using Game.Services.Currency;
using Game.Controllers.Clicker;
using Game.Controllers.Common;

namespace Game.Controllers.Clicker
{
    public class ClickerController : ViewControllerBase
    {
        private readonly IClickService _clickService;
        private readonly IPassiveIncomeTicker _passiveTicker;
        private readonly ICurrencyService _currencyService;
        private IClickerView _view;

        public ClickerController(IClickService clickService,
                                 IPassiveIncomeTicker passiveTicker,
                                 ICurrencyService currencyService)
        {
            _clickService = clickService ?? throw new ArgumentNullException(nameof(clickService));
            _passiveTicker = passiveTicker;
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));

            _currencyService.CoinsChanged += OnCoinsChanged;
        }

        public void BindView(IClickerView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            UnbindView();

            _view = view;
            _view.ClickTriggered += OnClickTriggered;
            _view.UpdateCoins(_currencyService.Coins);
        }

        public void UnbindView()
        {
            if (_view != null)
            {
                _view.ClickTriggered -= OnClickTriggered;
                _view = null;
            }
        }

        private void OnClickTriggered()
        {
            var gained = _clickService.Click();
            // If UI needs a "+X" animation, expose a method on IClickerView and call it here.
        }

        private void OnCoinsChanged(int coins) => _view?.UpdateCoins(coins);

        public void StartPassive() => _passiveTicker?.Start();
        public void StopPassive() => _passiveTicker?.Stop();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            UnbindView();
            if (_currencyService != null) _currencyService.CoinsChanged -= OnCoinsChanged;
        }
    }
}
