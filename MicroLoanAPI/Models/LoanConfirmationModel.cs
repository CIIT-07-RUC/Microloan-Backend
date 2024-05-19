using System;
namespace MicroLoanAPI.Models
{
	public class LoanConfirmationModel
	{
        public decimal InvestorId { get; set; }
        public decimal BorrowerProposalId { get; set; }
        public DateOnly ConfirmationDate { get; set; }
    }
}

