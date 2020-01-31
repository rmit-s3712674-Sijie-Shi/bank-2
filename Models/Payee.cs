using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bank.Models
{
    public class Payee
    {
        public int PayeeID { get; set; }

        public string PayeeName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public string Phone { get; set; }

    }
}
