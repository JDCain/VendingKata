using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using VendingCore.Model;

namespace VendingCore
{
    public class VendingMachine
    {
        public ReadOnlyCollection<InventoryItem> Shelves => _shelves.AsReadOnly();
        public ReadOnlyCollection<InventoryItem> Money => _moneyInventory.AsReadOnly();
        public decimal AvailableFunds { get; protected set; }

        public VendingMachine(List<InventoryItem> moneyList = null, List<InventoryItem> shelvesList = null)
        {
            _moneyInventory = moneyList ?? _defaultMoneyTypes;
            _shelves = shelvesList ?? _defaultShelves;
        }

        public bool Vend(InventoryItem item)
        {
            var result = false;
            if (item != null 
                && Shelves.Contains(item) 
                && (item.Count > 0 
                    && item.Value >= AvailableFunds))
            {
                item.Count--;
                AvailableFunds -= item.Value;
                result = true;             
            }
            return result;
        }

        public bool AddMoney(string moneyName)
        {
            var money = _moneyInventory.FirstOrDefault(x => x.Name == moneyName);
            var result = AddMoney(money);
            return result;
        }
        public bool AddMoney(decimal amount)
        {
            var money = _moneyInventory.FirstOrDefault(x => x.Value == amount);
            var result = AddMoney(money);
            return result;
        }
        private bool AddMoney(InventoryItem money)
        {
            var result = false;
            if (money != null)
            {
                money.Count++;
                AvailableFunds += money.Value;
                result = true;
            }
            return result;
        }

        public bool ReturnMoney()
        {
            throw new NotImplementedException();
        }

        #region Private Members

        private readonly List<InventoryItem> _defaultMoneyTypes = new List<InventoryItem>()
        {
            new InventoryItem() {Name = "Five", Count = 0, Value = 5m},
            new InventoryItem() {Name = "Dollar", Count = 0, Value = 1m},
            new InventoryItem() {Name = "Quarter", Count = 30, Value = 0.25m},
            new InventoryItem() {Name = "Dime", Count = 25, Value = 0.10m},
            new InventoryItem() {Name = "Nuckle", Count = 60, Value = 0.05m},
        };
        private readonly List<InventoryItem> _defaultShelves = new List<InventoryItem>()
        {
            new InventoryItem() {Name = "Chips", Count = 10, Value = 0.75m},
            new InventoryItem() {Name = "Pop", Count = 10, Value = 1.25m},
            new InventoryItem() {Name = "Gum", Count = 10, Value = 0.50m},
            new InventoryItem() {Name = "Pretzels", Count = 10, Value = 0.75m},
        };
        private readonly List<InventoryItem> _moneyInventory;
        private readonly List<InventoryItem> _shelves;

        #endregion
    }
}
