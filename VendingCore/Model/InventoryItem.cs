using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace VendingCore.Model
{
    public class InventoryItem
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Value { get; set; }
    }
}
