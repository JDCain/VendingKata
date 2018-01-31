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
        public ReadOnlyCollection<IInventoryItem> Shelves => _shelves.AsReadOnly();
        public ReadOnlyCollection<MoneyItem> Money => _moneyInventory.AsReadOnly();
        public decimal AvailableFunds { get; protected set; }

        public VendingMachine(List<MoneyItem> moneyList = null, List<IInventoryItem> shelvesList = null)
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
        private bool AddMoney(MoneyItem money)
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
            var result = false;
            var returnOptions = _moneyInventory.Where(x => x.CanReturn).OrderBy(x=>x.Value).ToList();
            return result;
        }

        #region Private Members

        private readonly List<MoneyItem> _defaultMoneyTypes = new List<MoneyItem>()
        {
            new MoneyItem() {Name = "Five", Count = 0, Value = 5m},
            new MoneyItem() {Name = "Dollar", Count = 0, Value = 1m},
            new MoneyItem() {Name = "Quarter", Count = 30, Value = 0.25m, CanReturn = true},
            new MoneyItem() {Name = "Dime", Count = 25, Value = 0.10m, CanReturn = true},
            new MoneyItem() {Name = "Nickle", Count = 60, Value = 0.05m, CanReturn = true},
        };
        private readonly List<IInventoryItem> _defaultShelves = new List<IInventoryItem>()
        {
            new InventoryItem() {Name = "Chips", Count = 10, Value = 0.75m},
            new InventoryItem() {Name = "Pop", Count = 10, Value = 1.25m},
            new InventoryItem() {Name = "Gum", Count = 10, Value = 0.50m},
            new InventoryItem() {Name = "Pretzels", Count = 10, Value = 0.75m},
        };
        private readonly List<MoneyItem> _moneyInventory;
        private readonly List<IInventoryItem> _shelves;

        #endregion
    }
}
