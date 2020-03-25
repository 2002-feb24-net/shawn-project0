using System;
using System.Collections.Generic;

namespace ProjectZero.Lib.Entities
{
    public partial class Store
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
