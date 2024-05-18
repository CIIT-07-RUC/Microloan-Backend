using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class BorrowerRating
    {
        public decimal Id { get; set; }
        public decimal? LoanTicketId { get; set; }
        public decimal? BorrowerId { get; set; }
        public decimal? InvestorId { get; set; }
        public decimal? Rating { get; set; }
        public string? Comment { get; set; }
        public DateOnly? RatingDate { get; set; }

        public virtual Borrower? Borrower { get; set; }
        public virtual Investor? Investor { get; set; }
        public virtual LoanTicket? LoanTicket { get; set; }
    }
}
