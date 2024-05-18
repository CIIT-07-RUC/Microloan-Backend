using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class AccountStatement
    {
        public decimal Id { get; set; }
        public decimal? InvestorId { get; set; }
        public decimal? TransactionAmount { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public decimal? ClosingBalance { get; set; }
    }
}
