using Game.Models.Items;
using UnityEngine;

public class PassiveIncomeItem : ItemDefinition
{
    public int PassiveIncome { get; }

    public PassiveIncomeItem(string id, string name, int price, string description, int passiveIncome, Sprite sprite)
        : base(id, name, price, description, sprite)
    {
        PassiveIncome = passiveIncome;
    }
}
