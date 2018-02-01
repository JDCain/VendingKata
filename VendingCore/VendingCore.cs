using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using VendingCore.Model;

namespace VendingCore
{
    public class VendingCore
    {
        public ReadOnlyCollection<IInventoryItem> Shelves => _shelves.AsReadOnly();
        public ReadOnlyCollection<MoneyItem> Money => _moneyInventory.AsReadOnly();
        public decimal AvailableFunds { get; protected set; }

        public VendingCore(List<MoneyItem> moneyList, List<IInventoryItem> shelvesList)
        {
            _moneyInventory = moneyList;
            _shelves = shelvesList;
        }

        public bool Vend(IInventoryItem item)
        {
            var result = false;
            if (item != null 
                && Shelves.Contains(item) 
                && (item.Count > 0 
                    && item.Value <= AvailableFunds))
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

        public List<MoneyItem> ReturnMoney()
        {
            var change = new List<MoneyItem>();
            var returnOptions = _moneyInventory.Where(x => x.CanReturn).OrderByDescending(x=>x.Value).ToList();
            foreach (var returnOption in returnOptions)
            {

                var coinNumber = AvailableFunds / returnOption.Value;
                if ((coinNumber % 1) == 0)
                {
                    var coinCount = (int) coinNumber;
                    returnOption.Count -= coinCount;
                    change.Add(new MoneyItem()
                    {
                        Name = returnOption.Name,
                        Value = returnOption.Value,
                        Count = coinCount,
                    });
                }
            }
            return change;
        }

        #region Private Members

        private readonly List<MoneyItem> _moneyInventory;
        private readonly List<IInventoryItem> _shelves;

        #endregion
    }
}
