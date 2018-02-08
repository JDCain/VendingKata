using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Vending.Model;

namespace Vending.Tests
{
    [TestClass]
    public class VendingCoreTests
    {
        [TestMethod]
        public void AddMoneyDefaultStrings()
        {
            AddMoneyStringAssert("Five", 5m);
            AddMoneyStringAssert("Dollar", 1m);
            AddMoneyStringAssert("Quarter", 0.25m);
            AddMoneyStringAssert("Dime", 0.10m);
            AddMoneyStringAssert("Nickel", 0.05m);
            var machine = new Core(Default.MoneyTypes, Default.Inventory);
            Assert.IsFalse(machine.AddMoney("Hamburger"));
        }
        [TestMethod]
        public void AddMoneyDefaultValues()
        {
            AddMoneyDecimalAssert(5m);
            AddMoneyDecimalAssert(1m);
            AddMoneyDecimalAssert(0.25m);
            AddMoneyDecimalAssert(0.10m);
            AddMoneyDecimalAssert(0.05m);
            var machine = new Core(Default.MoneyTypes, Default.Inventory);
            Assert.IsFalse(machine.AddMoney(0.36m));
        }
        private static void AddMoneyStringAssert(string amount, decimal expectedValue)
        {
            var machine = new Core(Default.MoneyTypes, Default.Inventory);
            var orginalCount = machine.Money.FirstOrDefault(x => x.Name == amount)?.Count;
            machine.AddMoney(amount);            
            Assert.IsTrue(machine.AvailableFunds == expectedValue);
            Assert.IsTrue(machine.Money.FirstOrDefault(x=>x.Name == amount)?.Count == (orginalCount + 1));
        }
        private static void AddMoneyDecimalAssert(decimal amount)
        {
            var machine = new Core(Default.MoneyTypes, Default.Inventory);
            var orginalCount = machine.Money.FirstOrDefault(x => x.Value == amount)?.Count;
            machine.AddMoney(amount);
            Assert.IsTrue(machine.AvailableFunds == amount);
            Assert.IsTrue(machine.Money.FirstOrDefault(x => x.Value == amount)?.Count == (orginalCount + 1));
        }

        [TestMethod]
        public void ReturnMoneyDefault()
        {
            var machine = new Core(Default.MoneyTypes, Default.Inventory);
            machine.AddMoney(0.25m);
            machine.AddMoney(0.25m);
            var change = machine.ReturnMoney();
            Assert.IsTrue(change.FirstOrDefault(x=>x.Value == 0.25m)?.Count == 2);
        }
        [TestMethod]
        public void VendWithChangeDefault()
        {
            var machine = new Core(Default.MoneyTypes, Default.Inventory);
            Assert.IsFalse(machine.ExactChangeRequired);
            machine.AddMoney(1m);
            var selectedItem = machine.Inventory.FirstOrDefault(x => x.Name == "Gum");
            var orginalCount = selectedItem?.Count;
            Assert.IsTrue(machine.Vend(selectedItem));
            Assert.IsTrue(selectedItem?.Count == (orginalCount - 1));
            var change = machine.ReturnMoney();
            Assert.IsTrue(change.FirstOrDefault(x => x.Value == 0.25m)?.Count == 2);
            Assert.IsTrue(machine.AvailableFunds == 0);
        }
        [TestMethod]
        public void VendWithLimitedChange()
        {
            //this ignores exact change required in order to test handling
            var almostEmpty = new List<MoneyItem>()
            {
                new MoneyItem() {Name = "Five", Count = 0, Value = 5m},
                new MoneyItem() {Name = "Dollar", Count = 0, Value = 1m},
                new MoneyItem() {Name = "Quarter", Count = 10, Value = 0.25m, CanReturn = true},
                new MoneyItem() {Name = "Dime", Count = 6, Value = 0.10m, CanReturn = true},
                new MoneyItem() {Name = "Nickel", Count = 300, Value = 0.05m, CanReturn = true},
            };

            var machine = new Core(almostEmpty, Default.Inventory);
            machine.AddMoney(5m);
            var selectedItem = machine.Inventory.FirstOrDefault(x => x.Name == "Pop");
            var orginalCount = selectedItem?.Count;
            Assert.IsTrue(machine.Vend(selectedItem));
            Assert.IsTrue(selectedItem?.Count == (orginalCount - 1));
            var change = machine.ReturnMoney();
            decimal total = 0;
            change.ForEach(x => total += x.Value * x.Count);
            Assert.IsTrue(total == 3.75m);
            Assert.IsTrue(machine.AvailableFunds == 0);
        }

        [TestMethod]
        public void ExactChangeIsRequired()
        {
            var almostEmpty = new List<MoneyItem>()
            {
                new MoneyItem() {Name = "Five", Count = 0, Value = 5m},
                new MoneyItem() {Name = "Dollar", Count = 0, Value = 1m},
                new MoneyItem() {Name = "Quarter", Count = 1, Value = 0.25m, CanReturn = true},
                new MoneyItem() {Name = "Dime", Count = 1, Value = 0.10m, CanReturn = true},
                new MoneyItem() {Name = "Nickel", Count = 1, Value = 0.05m, CanReturn = true},
            };

            var machine = new Core(almostEmpty, Default.Inventory);
            Assert.IsTrue(machine.ExactChangeRequired);
            Assert.IsFalse(machine.AddMoney("Five"));
            
        }

        [TestMethod]
        public void ExactChangeNotRequired()
        {
            var almostEmpty = new List<MoneyItem>()
            {
                new MoneyItem() {Name = "Five", Count = 0, Value = 5m},
                new MoneyItem() {Name = "Dollar", Count = 0, Value = 1m},
                new MoneyItem() {Name = "Quarter", Count = 17, Value = 0.25m, CanReturn = true},
                new MoneyItem() {Name = "Dime", Count = 1, Value = 0.10m, CanReturn = true},
                new MoneyItem() {Name = "Nickel", Count = 3, Value = 0.05m, CanReturn = true},
            };

            var machine = new Core(almostEmpty, Default.Inventory);
            Assert.IsFalse(machine.ExactChangeRequired);
        }
    }
}
