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

        }
        private static void AddMoneyStringAssert(string amount, decimal expectedValue)
        {
            var machine = new VendingMachine();
            machine.AddMoney(amount);
            Assert.IsTrue(machine.AvailableFunds == expectedValue);
        }
    }
}
