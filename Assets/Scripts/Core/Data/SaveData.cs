using System;
using System.Collections.Generic;

namespace Game.Core.DTOs
{
    /// <summary>
    /// Save data DTO for the minimal project where items are unique (count = 1).
    /// Stores simple, serializable data only.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        /// <summary>
        /// Player's coins at time of save.
        /// </summary>
        public int Coins;

        /// <summary>
        /// List of owned item ids. Since items are unique, count isn't needed.
        /// </summary>
        public List<string> OwnedItemIds = new List<string>();
    }
}
