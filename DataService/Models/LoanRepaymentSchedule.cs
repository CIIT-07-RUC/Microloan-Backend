using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class LoanRepaymentSchedule
    {
        public decimal Id { get; set; }
        public decimal? LoanTicketId { get; set; }
        public DateOnly? EmiDueDate { get; set; }
        public decimal? DueInterestAmount { get; set; }
        public decimal? DuePrincipalAmount { get; set; }
        public decimal? DueEmiAmount { get; set; }
        public decimal? TotalDueAmount { get; set; }
        public decimal? EmiAmountRecieved { get; set; }
        public DateOnly? RecieveDate { get; set; }

        public virtual LoanTicket? LoanTicket { get; set; }
    }
}
