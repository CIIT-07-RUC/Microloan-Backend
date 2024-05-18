using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class BorrowerLiability
    {
        public decimal Id { get; set; }
        public decimal? BorrowerId { get; set; }
        public decimal? LoanTicketId { get; set; }
        public decimal? LiabilityOutstanding { get; set; }
        public decimal? MonthlyRepaymentAmount { get; set; }
        public DateOnly? LiabilityStartDate { get; set; }
        public DateOnly? LiabilityEndDate { get; set; }

        public virtual Borrower? Borrower { get; set; }
        public virtual LoanTicket? LoanTicket { get; set; }
    }
}
