using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class InvestorLoanConfirmation
    {
        public InvestorLoanConfirmation()
        {
            LoanTickets = new HashSet<LoanTicket>();
        }

        public decimal Id { get; set; }
        public decimal? InvestorId { get; set; }
        public decimal? BorrowerProposalId { get; set; }
        public DateOnly? ConfirmationDate { get; set; }

        public virtual BorrowerProposal? BorrowerProposal { get; set; }
        public virtual Investor? Investor { get; set; }
        public virtual ICollection<LoanTicket> LoanTickets { get; set; }
    }
}
