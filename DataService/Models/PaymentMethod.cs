using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class PaymentMethod
    {
        public decimal Id { get; set; }
        public decimal? InvestorId { get; set; }
        public string? AccountNumber { get; set; }
        public char? AccountType { get; set; }
        public string? AccountHolderName { get; set; }
        public decimal? WireTransferCode { get; set; }
        public string? BankName { get; set; }

        public virtual Investor? Investor { get; set; }
    }
}
