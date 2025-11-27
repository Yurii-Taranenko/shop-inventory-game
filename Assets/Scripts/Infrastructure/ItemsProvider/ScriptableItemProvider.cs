using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models.Items;
using UnityEngine;

namespace Game.Infrastructure.ItemsProvider
{
    /// Provider that converts ScriptableObject assets into runtime ItemDefinition instances.
    /// Saves a dictionary for fast lookup by id. <summary>
    /// </summary>
    [CreateAssetMenu(fileName = "ScriptableItemProvider", menuName = "Game/Item Provider", order = 101)]
    public class ScriptableItemProvider : ScriptableObject
    {
        [Header("SO assets (authoring)")]
        // Use a simple empty array instead of Array.Empty to avoid potential issues on older Unity runtimes.
        [SerializeField] private SO_ItemData[] items = new SO_ItemData[0];

        private Dictionary<string, ItemDefinition> _cache;

        private Dictionary<SO_ItemData.ItemType, Func<SO_ItemData, ItemDefinition>> _factories = new()
        {
            {
                SO_ItemData.ItemType.PassiveIncome,
                so => new PassiveIncomeItem(so.id, so.itemName, so.price, so.description, so.passiveIncomePerSecond, so.Sprite)
            },
            {
                SO_ItemData.ItemType.ClickMultiplier,
                so => new ClickMultiplierItem(so.id, so.itemName, so.price, so.description, so.clickMultiplier, so.Sprite)
            },
        };

        private void EnsureBuilt()
        {
            if (_cache != null) return;

            _cache = new Dictionary<string, ItemDefinition>(StringComparer.Ordinal);

            if (items == null || items.Length == 0) return;

            foreach (var so in items)
            {
                if (so == null) continue;

                if (string.IsNullOrEmpty(so.id))
                {
                    Debug.LogWarning($"SO_ItemData with empty id: {so.name ?? "<unnamed>"}, skipping.");
                    continue;
                }

                ItemDefinition model = CreateModel(so);

                // Defensive: ensure model and model.Id are valid before adding.
                if (model == null || string.IsNullOrEmpty(model.Id))
                {
                    Debug.LogWarning($"Created item is null or has empty Id for SO: {so.name ?? so.id}, skipping.");
                    continue;
                }

                if (_cache.ContainsKey(model.Id))
                {
                    Debug.LogWarning($"Duplicate item id detected: {model.Id}. Skipping additional entry (SO: {so.name ?? so.id}).");
                    continue;
                }

                _cache.Add(model.Id, model);
            }
        }


        public ItemDefinition CreateModel(SO_ItemData so)
        {
            if (_factories.TryGetValue(so.type, out var factory))
                return factory(so);

            Debug.LogWarning($"Unknown item type {so.type} in {so.name ?? so.id}; defaulting to PassiveIncome.");

            return new PassiveIncomeItem(so.id, so.itemName, so.price, so.description, so.passiveIncomePerSecond, so.Sprite);
        }
        /// <summary>
        /// Returns all available item definitions (runtime instances).
        /// </summary>
        public IEnumerable<ItemDefinition> GetAllDefinitions()
        {
            EnsureBuilt();
            return _cache?.Values ?? Enumerable.Empty<ItemDefinition>();
        }

        /// <summary>
        /// Returns an item definition by id, or null if not found.
        /// </summary>
        public ItemDefinition GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            EnsureBuilt();
            _cache.TryGetValue(id, out var item);
            return item;
        }

        /// <summary>
        /// Utility to force rebuild cache (editor-only helper).
        /// </summary>
        [ContextMenu("Rebuild Cache")]
        private void RebuildCache()
        {
            _cache = null;
            EnsureBuilt();
            Debug.Log($"ScriptableItemProvider rebuilt: {_cache?.Count ?? 0} items.");
        }
    }
}
