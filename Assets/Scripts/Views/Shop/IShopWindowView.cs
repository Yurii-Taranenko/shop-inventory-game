
namespace Game.Controllers.Shop
{
    public interface IShopWindowView
    {
        void RebuildList(System.Collections.Generic.IReadOnlyList<Game.Models.Items.ItemDefinition> items);

        event System.Action<Game.Models.Items.ItemDefinition> BuyClicked;
    }
}