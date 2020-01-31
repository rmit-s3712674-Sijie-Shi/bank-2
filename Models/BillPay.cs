using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bank.Models
{
    

    public class BillPay
    {

        public int BillPayID { get; set; }

        public string Period { get; set; }
        
        public int AccountNumber { get; set; }

        public int PayeeID { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public DateTime ScheduleDate { get; set; }

        public DateTime TransactionTimeUtc { get; set; }


    }
}
