using UnityEngine;

namespace Game.Models.Items
{
    public class ItemDefinition
    {
        public string Id { get; }
        public string Name { get; }
        public int Price { get; }
        public string Description { get; }

        public Sprite Sprite;

        protected ItemDefinition(string id, string name, int price, string description, Sprite sprite)
        {
            Id = id;
            Name = name ?? string.Empty;
            Description = description;
            Price = System.Math.Max(0, price);
            Sprite = sprite;
        }
    }
}