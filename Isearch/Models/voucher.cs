using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Isearch.Models
{
    public class voucher
    {
        [Key]
        public int FVoucherID { get; set; }
        public DateTime FDate { get; set; }
        public int FYear { get; set; }
        public int FPeriod { get; set; }
        public int FNumber { get; set; }
        public string FExplanation { get; set; }
        public int FEntryCount { get; set; }
        public decimal FDebitTotal { get; set; }
        public decimal FCreditTotal { get; set; }
        public short FPreparerID { get; set; }


    }
}
