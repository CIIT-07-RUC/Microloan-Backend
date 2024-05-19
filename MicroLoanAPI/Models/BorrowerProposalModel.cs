using System;
namespace MicroLoanAPI.Models
{
	public class BorrowerProposalModel
	{
        public decimal BorrowerId { get; set; }
        public decimal ProposalInterestRate { get; set; }
        public decimal ProposalAmount { get; set; }
        public decimal ProposalMonths { get; set; }
        public string Organization { get; set; }
    }
}

