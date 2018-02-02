namespace Vending.Model
{
    public interface IInventoryItem
    {
        string Name { get; set; }
        int Count { get; set; }
        decimal Value { get; set; }
    }
}