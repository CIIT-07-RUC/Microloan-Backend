using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class LoanTicket
    {
        public LoanTicket()
        {
            BorrowerLiabilities = new HashSet<BorrowerLiability>();
            BorrowerRatings = new HashSet<BorrowerRating>();
            LoanRepaymentSchedules = new HashSet<LoanRepaymentSchedule>();
        }

        public decimal Id { get; set; }
        public decimal? InvestorLoanConfirmationId { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? LoanTenureInMonths { get; set; }
        public decimal? InterestRate { get; set; }
        public char? RiskRating { get; set; }

        public virtual InvestorLoanConfirmation? InvestorLoanConfirmation { get; set; }
        public virtual ICollection<BorrowerLiability> BorrowerLiabilities { get; set; }
        public virtual ICollection<BorrowerRating> BorrowerRatings { get; set; }
        public virtual ICollection<LoanRepaymentSchedule> LoanRepaymentSchedules { get; set; }
    }
}
