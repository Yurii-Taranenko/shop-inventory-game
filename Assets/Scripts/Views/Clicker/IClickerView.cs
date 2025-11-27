using System;

namespace Game.Controllers.Clicker
{
    public interface IClickerView
    {
        event Action ClickTriggered;

        void UpdateCoins(int coins);
    }
}
