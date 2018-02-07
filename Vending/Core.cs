using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Vending.Model;
// ReSharper disable All

namespace Vending
{
    public class Core
    {
        public ReadOnlyCollection<IInventoryItem> Inventory => _shelves.AsReadOnly();
        public ReadOnlyCollection<MoneyItem> Money => _moneyInventory.AsReadOnly();
        public decimal AvailableFunds { get; protected set; }

        public bool ExactChangeRequired => IsExactChangeRequired();

        public bool IsExactChangeRequired()
        {
            var result = true;
            var maxInput = _moneyInventory.OrderByDescending(x => x.Value).FirstOrDefault();
            var valueDecimal = Money.Sum(x => x.Value * x.Count);
            var minOutput = _moneyInventory.Where(x => x.CanReturn).OrderBy(x => x.Value).FirstOrDefault();
            var cheapestInventory = _shelves.OrderBy(x => x.Value).FirstOrDefault();

            result = (maxInput.Value - cheapestInventory.Value) > valueDecimal;
            
            return result;
        }
    

        public Core(IEnumerable<MoneyItem> money, IEnumerable<IInventoryItem> inventory)
        {
            _moneyInventory = money as List<MoneyItem>;
            _shelves = inventory as List<IInventoryItem>;
        }

        public bool Vend(IInventoryItem item)
        {
            var result = false;
            if (item != null 
                && Inventory.Contains(item) 
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
                var coinCount = (int)Math.Floor(coinNumber);
                if (coinCount > returnOption.Count)
                {
                    coinCount = returnOption.Count;
                }
                returnOption.Count -= coinCount;
                change.Add(new MoneyItem()
                {
                    Name = returnOption.Name,
                    Value = returnOption.Value,
                    Count = coinCount,
                });
                AvailableFunds -= (returnOption.Value * coinCount);             
            }
            return change.Where(x=>x.Count >0 ).ToList();
        }

        #region Private Members

        private readonly List<MoneyItem> _moneyInventory;
        private readonly List<IInventoryItem> _shelves;

        #endregion
    }
}
