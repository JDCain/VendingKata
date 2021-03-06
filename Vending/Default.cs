﻿using System.Collections.Generic;
using Vending.Model;

namespace Vending
{
    public static class Default
    {
        public static IEnumerable<MoneyItem> MoneyTypes => new List<MoneyItem>()
        {
            new MoneyItem() {Name = "Five", Count = 0, Value = 5m},
            new MoneyItem() {Name = "Dollar", Count = 0, Value = 1m},
            new MoneyItem() {Name = "Quarter", Count = 30, Value = 0.25m, CanReturn = true},
            new MoneyItem() {Name = "Dime", Count = 25, Value = 0.10m, CanReturn = true},
            new MoneyItem() {Name = "Nickel", Count = 60, Value = 0.05m, CanReturn = true},
        };
        public static IEnumerable<IInventoryItem> Inventory => new List<IInventoryItem>()
        {
            new InventoryItem() {Name = "Chips", Count = 10, Value = 0.75m},
            new InventoryItem() {Name = "Pop", Count = 10, Value = 1.25m},
            new InventoryItem() {Name = "Gum", Count = 10, Value = 0.50m},
            new InventoryItem() {Name = "Pretzels", Count = 10, Value = 0.75m},
        };
    }
}
