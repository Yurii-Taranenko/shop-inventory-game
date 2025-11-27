using Game.Models.Items;
using UnityEngine;

public class ClickMultiplierItem : ItemDefinition
{
    public float ClickMultiplier { get; }

    public ClickMultiplierItem(string id, string name, int price, string description, float clickMultiplier,  Sprite sprite)
        : base(id, name, price, description, sprite)
    {
        ClickMultiplier = clickMultiplier;
    }
}
