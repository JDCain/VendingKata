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
            var machine = new VendingMachine();
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
            var machine = new VendingMachine();
            Assert.IsFalse(machine.AddMoney(0.36m));
        }
        private static void AddMoneyStringAssert(string amount, decimal expectedValue)
        {
            var machine = new VendingMachine();
            machine.AddMoney(amount);
            Assert.IsTrue(machine.AvailableFunds == expectedValue);
        }
        private static void AddMoneyDecimalAssert(decimal amount)
        {
            var machine = new VendingMachine();
            machine.AddMoney(amount);
            Assert.IsTrue(machine.AvailableFunds == amount);
        }
    }
}
