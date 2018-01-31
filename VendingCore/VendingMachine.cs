using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VendingCore
{
    public class VendingMachine
    {
        
        private Dictionary<string, decimal> _defaultMoneyTypes => new Dictionary<string, decimal>()
        {
            { "Quarter", 0.25m },
            { "Dime", 0.10m },
            { "Nickle", 0.05m }
        };

        private Dictionary<string, decimal> _moneyDecimals;
        public VendingMachine(Dictionary<string, decimal> moneyDictionary = null)
        {
            _moneyDecimals = moneyDictionary ?? _defaultMoneyTypes;
        }
    }
}
