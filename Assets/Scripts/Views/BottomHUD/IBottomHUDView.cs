using Game.Models.Inventory;
using System;
using System.Collections.Generic;

namespace Game.Controllers.Inventory
{
    public interface IBottomHUDView
    {
        event Action OnClickShop;
        event Action OnClickInventory;
    }
}