using ProjectZero.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectZero;

namespace ProjectZero.Lib
{
    public class Repository : IRepository
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PrefLocation { get; set; }

        public void Greeting()
        {
            Console.WriteLine("### CPU EXPRESS ###");
            Console.WriteLine("We sell CPUs for gaming desktops.");
            Console.WriteLine(" ");
            Console.WriteLine("User: " + this.FirstName + " " + this.LastName);
            Console.WriteLine("Phone Number: " + this.PhoneNumber);
            Console.WriteLine("Selected Store: " + this.PrefLocation);
        }
    }
}
