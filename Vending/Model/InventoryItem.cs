namespace Vending.Model
{
    public class InventoryItem : IInventoryItem
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Value { get; set; }
    }
}
