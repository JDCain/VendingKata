using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingCore;

namespace Vending.Tests
{
    [TestClass]
    public class VendingMachineTests
    {
        [TestMethod]
        public void AddMoneyDefaultStrings()
        {
            AddMoneyStringAssert("Five", 5m);
            AddMoneyStringAssert("Dollar", 1m);
            AddMoneyStringAssert("Quarter", 0.25m);
            AddMoneyStringAssert("Dime", 0.10m);
            AddMoneyStringAssert("Nickel", 0.05m);
            var machine = new VendingCore.VendingCore();
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
            var machine = new VendingCore.VendingCore();
            Assert.IsFalse(machine.AddMoney(0.36m));
        }
        private static void AddMoneyStringAssert(string amount, decimal expectedValue)
        {
            var machine = new VendingCore.VendingCore();
            var orginalCount = machine.Money.FirstOrDefault(x => x.Name == amount)?.Count;
            machine.AddMoney(amount);            
            Assert.IsTrue(machine.AvailableFunds == expectedValue);
            Assert.IsTrue(machine.Money.FirstOrDefault(x=>x.Name == amount)?.Count == (orginalCount + 1));
        }
        private static void AddMoneyDecimalAssert(decimal amount)
        {
            var machine = new VendingCore.VendingCore();
            var orginalCount = machine.Money.FirstOrDefault(x => x.Value == amount)?.Count;
            machine.AddMoney(amount);
            Assert.IsTrue(machine.AvailableFunds == amount);
            Assert.IsTrue(machine.Money.FirstOrDefault(x => x.Value == amount)?.Count == (orginalCount + 1));
        }

        [TestMethod]
        public void ReturnMoneyDefault()
        {
            var machine = new VendingCore.VendingCore();
            machine.AddMoney(0.25m);
            machine.AddMoney(0.25m);
            var change = machine.ReturnMoney();
            Assert.IsTrue(change.FirstOrDefault(x=>x.Value == 0.25m)?.Count == 2);
        }
        [TestMethod]
        public void VendWithChangeDefault()
        {
            var machine = new VendingCore.VendingCore();
            machine.AddMoney(1m);
            var selectedItem = machine.Shelves.FirstOrDefault(x => x.Name == "Gum");
            var orginalCount = selectedItem?.Count;
            Assert.IsTrue(machine.Vend(selectedItem));
            Assert.IsTrue(selectedItem?.Count == (orginalCount - 1));
            var change = machine.ReturnMoney();
            Assert.IsTrue(change.FirstOrDefault(x => x.Value == 0.25m)?.Count == 2);
        }
    }
}
