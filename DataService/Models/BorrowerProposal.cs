using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class BorrowerProposal
    {
        public BorrowerProposal()
        {
            InvestorLoanConfirmations = new HashSet<InvestorLoanConfirmation>();
        }

        public decimal Id { get; set; }
        public decimal? BorrowerId { get; set; }
        public decimal? ProposalInterestRate { get; set; }
        public decimal? ProposalAmount { get; set; }
        public decimal? ProposalMonths { get; set; }
        public string? Organization { get; set; }

        public virtual Borrower? Borrower { get; set; }
        public virtual ICollection<InvestorLoanConfirmation> InvestorLoanConfirmations { get; set; }
    }
}
