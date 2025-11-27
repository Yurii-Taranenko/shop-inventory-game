using System.Linq;
using Game.Models.Inventory;
using Game.Models.Items;
using Game.Services.Inventory;

namespace Game.Services.Clicker
{
    /// Calculates passive income per second and click multiplier based on owned inventory items.
    public class IncomeCalculator :IIncomeCalculator
    {
        private readonly IInventoryService _inventory;

        public IncomeCalculator(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        /// Total passive income per second from items in the inventory.
        public int GetPassiveIncomePerSecond()
        {
            int total = 0;

            foreach (var entry in _inventory.Items)
            {
                if (entry.Item is PassiveIncomeItem passive)
                {
                    total += passive.PassiveIncome;
                }
            }

            return total;
        }

        /// Total click multiplier that should be applied to manual click.
        public float GetClickMultiplier()
        {
            float total = 1f; // base multiplier

            foreach (var entry in _inventory.Items)
            {
                if (entry.Item is ClickMultiplierItem mult)
                {
                    total += mult.ClickMultiplier;
                }
            }

            return total;
        }
    }
}
