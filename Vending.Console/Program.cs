using System;
using System.Xml.Serialization;
using Vending;
using Vending.Model;
using static System.Console;

namespace VendingConsole
{
    class Program
    {
        static readonly Core _machine = new Core(Default.MoneyTypes, Default.Inventory);
        private static IInventoryItem _selectedItem;
        private static object reason;

        static void Main(string[] args)
        {
            var exit = false;
            while (exit == false)
            {
                Clear();
                DisplayFunds();
                Write($"Selected Item: {_selectedItem?.Name}");
                if (_selectedItem?.Count <= 0)
                {
                    WriteLine(" SOLD OUT");
                }
                else
                {
                    WriteLine();
                }
                WriteLine($"0.) Select Item");
                WriteLine($"1.) Add Funds");
                WriteLine($"2.) Request Change");
                WriteLine($"3.) Vend");
                WriteLine($"4.) Leave");
                var choice = GetInput();
                Clear();
                switch (choice)
                {
                    case 0:
                        DisplayInventory();
                        InputSelectAction(x => _selectedItem = _machine.Inventory[x]);
                        break;
                    case 1:
                        DisplayFunds();
                        DisplayMoney();
                        InputSelectAction(x => _machine.AddMoney(_machine.Money[x].Value));                                                
                        break;
                    case 2:
                        ReturnMoney();
                        break;
                    case 3:
                        if (_selectedItem != null)
                        {
                            if (_machine.Vend(_selectedItem))
                            {
                                WriteLine($"Vending {_selectedItem.Name}");
                                ReturnMoney();
                            }
                            else
                            {
                                if (_machine.AvailableFunds < _selectedItem.Value)
                                {
                                    reason = "Insufficient Funds";
                                }
                                else if (_selectedItem.Count <= 0)
                                {
                                    reason = "Sold Out";
                                }
                                else
                                {
                                    reason = "Unknown Error";
                                }
                                WriteLine($"Vending Failed: {reason}");
                            }
                            
                        }
                        else
                        {
                            WriteLine("No Item Selected");
                        }
                        Pause();
                        break;
                    case 4:
                        exit = true;
                        break;
                    default:
                        WriteLine("INVALID SELECTION");
                        break;
                }
            }
        }

        private static void ReturnMoney()
        {
            foreach (var money in _machine.ReturnMoney())
            {
                WriteLine($"Returned {money.Count} {money.Name}(s)");
            }
            Pause();
        }

        private static void Pause()
        {
            WriteLine("Press Any Key To Continue");
            ReadKey();
        }

        private static int? GetInput()
        {
            Write("Enter input: ");
            var input = ReadLine();
            if (int.TryParse(input, out var output))
            {
                return output;
            }
            return null;
        }

        static void DisplayInventory()
        {
            foreach (var item in _machine.Inventory)
            {
                WriteLine($"{_machine.Inventory.IndexOf(item)}). {item.Name} : {item.Value}");                
            }          
        }

        static void DisplayMoney()
        {
            foreach (var item in _machine.Money)
            {
                WriteLine($"{_machine.Money.IndexOf(item)}). {item.Name} : {item.Value}");
            }
        }

        static void DisplayMoneyOptions()
        {
            foreach (var item in _machine.Money)
            {
                WriteLine($"{_machine.Money.IndexOf(item)}). {item.Name}");
            }
        }

        static void DisplayFunds()
        {
            WriteLine($"Available Funds: {_machine.AvailableFunds}");
        }

        static void InputSelectAction(Action<int> action)
        {
            var input = GetInput();
            if (input != null)
            {
                int v = input ?? default(int);
                action.Invoke(v);
            }
        }
    }
}
