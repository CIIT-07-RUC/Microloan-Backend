using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class Investor
    {
        public Investor()
        {
            BorrowerRatings = new HashSet<BorrowerRating>();
            InvestorLoanConfirmations = new HashSet<InvestorLoanConfirmation>();
            PaymentMethods = new HashSet<PaymentMethod>();
        }

        public decimal Id { get; set; }
        public decimal? UserAccountId { get; set; }
        public decimal? InvestmentLimit { get; set; }
        public decimal? TotalFundsCommited { get; set; }
        public string? TaxId { get; set; }

        public virtual UserAccount? UserAccount { get; set; }
        public virtual ICollection<BorrowerRating> BorrowerRatings { get; set; }
        public virtual ICollection<InvestorLoanConfirmation> InvestorLoanConfirmations { get; set; }
        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
