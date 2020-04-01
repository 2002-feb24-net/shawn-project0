using ProjectZero.Lib.Entities;
using ProjectZero.Lib;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// ProjectZero - CPU Express (Store)
/// </summary>

/// <summary>
/// I broke and rebuilt this project many times.
/// I really felt the time crunch.
/// On the next Project I'll make sure to follow ACID better.
/// </summary>

namespace ProjectZero.App
{
    class Program
    {    
        static void Main(string[] args)
        {
            var ActiveCustomer = new Customer();
            var Repo = new Repository();
            Console.WriteLine("### CPU EXPRESS ###");
            //USER OPTIONS AT STARTUP
            Console.WriteLine("\n[C]\tCreate account");
            Console.WriteLine("[S]\tSign in");
            Console.WriteLine("[E]\tExit\n");
            Console.WriteLine("Press corresponding key and hit enter." + "\n");
            string input = Console.ReadLine();
//CREATE USER
            if (input == "C" || input == "c")
                {
                try
                {
                    Console.WriteLine("First Name: ");
                    string InputFName = FilterName();
                    Console.WriteLine("Last Name: ");
                    string InputLName = FilterName();
                    Console.WriteLine("Phone Number: ");
                    string InputPNumber = FilterPhoneNumber();
                    Console.WriteLine("Prefferred Store: ");
                    Console.WriteLine("[1] Dallas");
                    Console.WriteLine("[2] LA");
                    Console.WriteLine("[3] Tampa");
                    string InputPrefLoc = FilterLocation();
                    AddPersonToDB(InputFName, InputLName, InputPNumber, InputPrefLoc);
                    ActiveCustomer.FirstName = InputFName;
                    ActiveCustomer.LastName = InputLName;
                    ActiveCustomer.PhoneNumber = InputPNumber;
                    ActiveCustomer.PrefLocation = InputPrefLoc;
                    input = "S";
                }
                catch (DbUpdateException)
                {
                        Console.WriteLine("Phone Number is already in use");
                        Console.WriteLine("Restart the app and try again");
                }
            }
//SIGNIN USER
            if (something == "S" || something == "s")
                {
                    Console.Clear();
                Console.WriteLine("### CPU EXPRESS ###\n");
                Console.WriteLine("\nPlease provide the phone number you used during account creation.\n");
                try
                {
                    Console.WriteLine("Phone Number: ");
                    string InputPrefLoc4 = FilterPhoneNumber();
                    using var context = new ProjectZeroContext();
                    var query = context.Customer
                        .Where(s => s.PhoneNumber == InputPrefLoc4)
                        .FirstOrDefault<Customer>();
                    ActiveCustomer.Id = query.Id;
                    ActiveCustomer.FirstName = query.FirstName;
                    ActiveCustomer.LastName = query.LastName;
                    ActiveCustomer.PhoneNumber = query.PhoneNumber;
                    ActiveCustomer.PrefLocation = query.PrefLocation;
                    Repo.Id = query.Id;
                    Repo.FirstName = query.FirstName;
                    Repo.LastName = query.LastName;
                    Repo.PhoneNumber = query.PhoneNumber;
                    Repo.PrefLocation = query.PrefLocation;
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("No Matching User");
                }
                }
            else { Environment.Exit(0); }
//USER IS ENTERING THE INTEL STORE
            if (ActiveCustomer.PrefLocation == "Dallas")
            {
                bool isAppOpen = true;
                do
                {
                    Console.Clear();
                    Repo.Greeting();
                    Console.WriteLine("\n");
                    Console.WriteLine("We sell Intel 7th, 8th, and 9th Gen CPU here!");
                    Console.WriteLine("\n[P]\tPurchase");
                    Console.WriteLine("[O]\tOrder History");
                    Console.WriteLine("[U]\tUser Lookup");
                    Console.WriteLine("[E]\tExit\n");
                    Console.WriteLine("Press corresponding key and hit enter." + "\n");
                    string decision = Console.ReadLine();
                    if (decision == "e" || decision == "E") { Environment.Exit(0); }
//USER LOOKUP
                    if (decision == "u" || decision == "U")
                    {
                        using var context = new ProjectZeroContext();
                        foreach (var row in context.Customer)
                            Console.WriteLine("First Name: " + row.FirstName + " Last Name: " + row.LastName + " Phone Number: " + row.PhoneNumber + " Prefferred Location: " + row.PrefLocation);
                        Console.WriteLine("Press any key to continue....");
                        var waitingOnYou = Console.ReadLine();
                        continue;
                    }
//ORDER LOOKUP
                    if (decision == "o" || decision == "O")
                    {
                        using var context = new ProjectZeroContext();
                        foreach (var row in context.Receipt)
                            Console.WriteLine("Store: " + row.Location + " CustomerID: " + row.CustomerId + " Product: " + row.Brand + " " + row.Model + " Qty: 1 " + "Price: " + row.Price + " Date: " + row.Date);
                        Console.WriteLine("Press any key to continue....");
                        var waitingOnYou = Console.ReadLine();
                        continue;
                    }
//PURCHASING INTEL
                    var temp2 = Console.ReadLine();
                    if (decision == " " || decision == "") { continue; }
                    if (decision == "p" || decision == "P")
                    {
                        Console.WriteLine(" ");
                        using var context = new ProjectZeroContext();
                        foreach (var row in context.Product)
                            if (row.Manufacturer == "Intel")
                            {
                                Console.WriteLine("[" + row.Id + "] " + row.Manufacturer + " " + row.Brand + " " + row.Model + " ($" + row.Price + ") Stock: " + row.Quantity);
                            }
                        Console.WriteLine("Type the [ID] for the item you want to purchase.");
                        string somethingagain = Console.ReadLine();
                        if (somethingagain != "4" && somethingagain != "5" && somethingagain != "6")
                        {
                            Console.WriteLine("Failed to make purchase, press any key.");
                            var waiting123 = Console.ReadLine();
                            continue;
                        }
                        Product ActiveProduct = context.Product.First(a => a.Id == int.Parse(somethingagain));
                            if (ActiveProduct.Quantity < 1)
                            {
                                Console.WriteLine("ITEM IS CURRENTLY OUT OF STOCK");
                                Console.WriteLine("Press any key to continue....");
                                var waitingOnYou = Console.ReadLine();
                                continue;
                            }
                            Console.Write("You've purchased ");
                            Console.WriteLine(ActiveProduct.Manufacturer + " " + ActiveProduct.Brand + " " + ActiveProduct.Model);
                            DateTime date1 = DateTime.Now;
                            CompletedOrder(ActiveCustomer.Id, ActiveProduct.Id, ActiveProduct.Manufacturer, ActiveProduct.Brand, ActiveProduct.Model, ActiveProduct.Price, 1, date1, ActiveCustomer.PrefLocation);
//DECREMENT 1 FROM STOCK
                            using (var db = new ProjectZeroContext())
                            {
                                var result = db.Product.SingleOrDefault(b => b.Id == ActiveProduct.Id);
                                if (result != null)
                                {
                                    result.Quantity = ActiveProduct.Quantity - 1;
                                    context.Entry(result).CurrentValues.SetValues(result.Quantity);
                                    db.SaveChanges();
                                }
                            }
                            Console.WriteLine("Press any key to continue....");
                            var waitingOnYouAgain = Console.ReadLine();
                    } 
                }
                while (isAppOpen == true);
            }
//PURCHASING RYZEN
            if (ActiveCustomer.PrefLocation == "LA")
            {
                bool isAppOpen = true;
                do
                {
                    Console.Clear();
                    Repo.Greeting();
                    Console.WriteLine("\nWe sell Ryzen 2nd and 3rd Gen CPU here!\n");
                    Console.WriteLine("\n[P]\tPurchase");
                    Console.WriteLine("[O]\tOrder History");
                    Console.WriteLine("[U]\tUser Lookup");
                    Console.WriteLine("[E]\tExit\n");
                    Console.WriteLine("Press corresponding key and hit enter." + "\n");
                    string decision = Console.ReadLine();
                    if (decision == "e" || decision == "E") { Environment.Exit(0); }
//USER LOOKUP
                    if (decision == "u" || decision == "U")
                    {
                        using var context = new ProjectZeroContext();
                        foreach (var row in context.Customer)
                            Console.WriteLine("First Name: " + row.FirstName + " Last Name: " + row.LastName + " Phone Number: " + row.PhoneNumber + " Prefferred Location: " + row.PrefLocation);
                        Console.WriteLine("Press any key to continue....");
                        var waitingOnYou = Console.ReadLine();
                        continue;
                    }
//ORDER LOOKUP
                    if (decision == "o" || decision == "O")
                    {
                        using var context = new ProjectZeroContext();
                        foreach (var row in context.Receipt)
                            Console.WriteLine("Store: " + row.Location + " CustomerID: " + row.CustomerId + " Product: " + row.Brand + " " + row.Model + " Qty: 1 " + "Price: " + row.Price + " Date: " + row.Date);
                        Console.WriteLine("Press any key to continue....");
                        var waitingOnYou = Console.ReadLine();
                        continue;
                    }
//PURCHASING RYZEN
                    if (decision == " " || decision == "") { continue; }
                    if (decision == "p" || decision == "P")
                    {
                        Console.WriteLine(" ");
                        using var context = new ProjectZeroContext();
                        foreach (var row in context.Product)
                            if (row.Manufacturer == "AMD")
                            {
                                Console.WriteLine("[" + row.Id + "] " + row.Manufacturer + " " + row.Brand + " " + row.Model + " ($" + row.Price + ") Stock: " + row.Quantity);
                            }
                        Console.WriteLine("Type the [ID] for the item you want to purchase.");
                        string somethingagain = Console.ReadLine();
                        if (somethingagain != "1" && somethingagain != "2" && somethingagain != "3")
                        {
                            Console.WriteLine("Failed to make purchase, press any key.");
                            var waiting123 = Console.ReadLine();
                            continue;
                        }
                        Product ActiveProduct = context.Product.First(a => a.Id == int.Parse(somethingagain));
                        if (ActiveProduct.Quantity < 1)
                        {
                            Console.WriteLine("ITEM IS CURRENTLY OUT OF STOCK");
                            Console.WriteLine("Press any key to continue....");
                            var waitingOnYou = Console.ReadLine();
                            continue;
                        }
                        Console.Write("You've purchased ");
                        Console.WriteLine(ActiveProduct.Manufacturer + " " + ActiveProduct.Brand + " " + ActiveProduct.Model);
                        DateTime date1 = DateTime.Now;
                        CompletedOrder(ActiveCustomer.Id, ActiveProduct.Id, ActiveProduct.Manufacturer, ActiveProduct.Brand, ActiveProduct.Model, ActiveProduct.Price, 1, date1, ActiveCustomer.PrefLocation);
//DECREMENT 1 FROM STOCK
                        using (var db = new ProjectZeroContext())
                        {
                            var result = db.Product.SingleOrDefault(b => b.Id == ActiveProduct.Id);
                            if (result != null)
                            {
                                result.Quantity = ActiveProduct.Quantity - 1;
                                context.Entry(result).CurrentValues.SetValues(result.Quantity);
                                db.SaveChanges();
                            }
                        }
                        Console.WriteLine("Press any key to continue....");
                        var waitingOnYouAgain = Console.ReadLine();
                    }
                }
                while (isAppOpen == true);
            }
//INTEL AND RYZEN
            if (ActiveCustomer.PrefLocation == "Tampa")
            {
                bool isAppOpen = true;
                do
                {
                    Console.Clear();
                    Repo.Greeting();
                    Console.WriteLine("\n");
                    Console.WriteLine("We sell the latest Intel and Ryzen CPU here!");
                    Console.WriteLine("\n[P]\tPurchase");
                    Console.WriteLine("[O]\tOrder History");
                    Console.WriteLine("[U]\tUser Lookup");
                    Console.WriteLine("[E]\tExit\n");
                    Console.WriteLine("Press corresponding key and hit enter." + "\n");
                    string decision = Console.ReadLine();
                    if (decision == "e" || decision == "E") { Environment.Exit(0); }
//USER LOOKUP
                    if (decision == "u" || decision == "U")
                    {
                        using var context = new ProjectZeroContext();
                        foreach (var row in context.Customer)
                            Console.WriteLine("First Name: " + row.FirstName + " Last Name: " + row.LastName + " Phone Number: " + row.PhoneNumber + " Prefferred Location: " + row.PrefLocation);
                        Console.WriteLine("Press any key to continue....");
                        var waitingOnYou = Console.ReadLine();
                        continue;
                    }
//ORDER LOOKUP
                    if (decision == "o" || decision == "O")
                    {
                        using var context = new ProjectZeroContext();
                        foreach (var row in context.Receipt)
                            Console.WriteLine("Store: " + row.Location + " CustomerID: " + row.CustomerId + " Product: " + row.Brand + " " + row.Model + " Qty: 1 " + "Price: " + row.Price + " Date: " + row.Date);
                        Console.WriteLine("Press any key to continue....");
                        var waitingOnYou = Console.ReadLine();
                        continue;
                    }
//PURCHASING INTEL AND RYZEN
                    if (decision == " " || decision == "") { continue; }
                    if (decision == "p" || decision == "P")
                    {
                        Console.WriteLine(" ");
                        using var context = new ProjectZeroContext();
                        foreach (var row in context.Product)
                                Console.WriteLine("[" + row.Id + "] " + row.Manufacturer + " " + row.Brand + " " + row.Model + " ($" + row.Price + ") Stock: " + row.Quantity); 
                        Console.WriteLine("Type the [ID] for the item you want to purchase.");
                        string somethingagain = Console.ReadLine();
                        if (somethingagain != "1" && somethingagain != "2" && somethingagain != "3" && somethingagain != "4" && somethingagain != "5" && somethingagain != "6")
                        {
                            Console.WriteLine("Failed to make purchase, press any key.");
                            var waiting123 = Console.ReadLine();
                            continue;
                        }
                        Product ActiveProduct = context.Product.First(a => a.Id == int.Parse(somethingagain));
                        if (ActiveProduct.Quantity < 1)
                        {
                            Console.WriteLine("ITEM IS CURRENTLY OUT OF STOCK");
                            Console.WriteLine("Press any key to continue....");
                            var waitingOnYou = Console.ReadLine();
                            continue;
                        }
                        Console.Write("You've purchased ");
                        Console.WriteLine(ActiveProduct.Manufacturer + " " + ActiveProduct.Brand + " " + ActiveProduct.Model);
                        DateTime date1 = DateTime.Now;
                        CompletedOrder(ActiveCustomer.Id, ActiveProduct.Id, ActiveProduct.Manufacturer, ActiveProduct.Brand, ActiveProduct.Model, ActiveProduct.Price, 1, date1, ActiveCustomer.PrefLocation);
//DECREMENT 1 FROM STOCK
                        using (var db = new ProjectZeroContext())
                        {
                            var result = db.Product.SingleOrDefault(b => b.Id == ActiveProduct.Id);
                            if (result != null)
                            {
                                result.Quantity = ActiveProduct.Quantity - 1;
                                context.Entry(result).CurrentValues.SetValues(result.Quantity);
                                db.SaveChanges();
                            }
                        }
                        Console.WriteLine("Press any key to continue....");
                        var waitingOnYouAgain = Console.ReadLine();
                    }
                }
                while (isAppOpen == true);
            }
        }
//STATIC METHODS
        static void AddPersonToDB(string fname, string lname, string pnumber, string ploc)
        {
            using var context = new ProjectZeroContext();
            var person = new Customer()
            {
                FirstName = fname,
                LastName = lname,
                PhoneNumber = pnumber,
                PrefLocation = ploc
            };
            context.Customer.Add(person);
            context.SaveChanges();
        }
        static void CompletedOrder(int cid, int pid, string manuf, string brand, string model, decimal? price,int qty, DateTime date, string prefloc)
        {
            using var context = new ProjectZeroContext();
            var receipt = new Receipt()
            {
                CustomerId = cid,
                ProductId = pid,
                Manufacturer = manuf,
                Brand = brand,
                Model = model,
                Price = (int)price,
                Quantity = qty,
                Date = date,
                Location = prefloc
            };
            context.Receipt.Add(receipt);
            context.SaveChanges();
        }
        static string FilterName()
        {
            string temp = Console.ReadLine();
            string temp2 = Regex.Replace(temp, "[^a-zA-Z]", "");
            if (temp2.Length < 2)
            {
                Console.WriteLine("Your name has got to be at least 2 letters.");
                temp2 = "";
                FilterName();
            }
            return temp2;
        }
        static string FilterPhoneNumber()
        {
            string temp = Console.ReadLine();
            string temp2 = Regex.Replace(temp, "[^0-9]", "");
            if (temp2.Length != 10)
            {
                Console.WriteLine("Provide 10 digits for phone number: ");
                temp2 = "";
                FilterPhoneNumber();
            }
            return temp2;
        }
        static string FilterLocation()
        {
            char temp = char.Parse(Console.ReadLine());
            string temp2 = "";
            if(temp == '1')
            {
                temp2 = "Dallas";
            }
            else if (temp == '2')
            {
                temp2 = "LA";
            }   
            else if (temp == '3')
            {

                temp2 = "Tampa";
            }
            else
            {
                Console.WriteLine("Provide location number: ");
                FilterLocation();
            }
            return temp2;
        }
    }
}