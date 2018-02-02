namespace Vending.Model
{
    public class MoneyItem : IInventoryItem
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Value { get; set; }
        public bool CanReturn { get; set; }
    }
}