using System.Collections.Generic;
using System.Linq;
using Vending.Model;

namespace Vending
{
    public class Machine
    {
        private readonly Core _vendingCore;
        public IReadOnlyList<IInventoryItem> Shelves;

        public Machine(IEnumerable<MoneyItem> moneyTypes = null, IEnumerable<IInventoryItem> inventoryItems = null)
        {
            var monies = moneyTypes ?? Default.MoneyTypes;
            var inventory = inventoryItems ?? Default.Inventory;
            _vendingCore = new Core(monies.ToList(), inventory.ToList());
            Shelves = _vendingCore.Inventory;

        }
    }
}
