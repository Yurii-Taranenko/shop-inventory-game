using UnityEngine;
using Game.Models.Items;

namespace Game.Infrastructure.ItemsProvider
{
    /// <summary>
    /// ScriptableObject representation of item data.
    /// </summary>
    [CreateAssetMenu(fileName = "ItemData", menuName = "Game/Item Data", order = 1)]
    public class SO_ItemData : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string itemName;
        public int price;
        public string description;
        public Sprite Sprite;

        public enum ItemType
        {
            PassiveIncome,
            ClickMultiplier
        }

        [Header("Type")]
        public ItemType type;

        [Header("Passive Income (if applicable)")]
        public int passiveIncomePerSecond;

        [Header("Click Multiplier (if applicable)")]
        public float clickMultiplier = 1f;
    }
}
