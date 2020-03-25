using System;
using System.Collections.Generic;

namespace ProjectZero.Lib.Entities
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PrefLocation { get; set; }
    }
}
