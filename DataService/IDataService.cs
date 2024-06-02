using System;
using DataService.Models;

namespace DataService
{
	public interface IDataService
	{
        public UserAccount GetUserById(int id);
        public UserAccount GetUserByMail(string email);
        public Borrower GetBorrowerByUserId(decimal userId);
        public Investor GetInvestorByUserId(decimal userIdd);


        public Borrower GetUserByBorrowerId(decimal borrowerId);



        public IEnumerable<dynamic> GetAllUsers();
        public BorrowerProposal GetBorrowerProposalById(decimal id);

        public Tuple<bool, string> RegisterUser(string email, decimal phone, string password, string confirmPassword, bool isInvestor);
        public Tuple<bool, string> LoginUser(string email, string password);
        public Tuple<bool, string> CreateBorrowerProposal(decimal borrowerId, decimal proposalInterestRate, decimal proposalAmount, decimal proposalMonthDate);
        public List<BorrowerProposal> GetBorrowerProposals();


        public InvestorLoanConfirmation GetLoanConfirmationById(decimal id);
        public InvestorLoanConfirmation GetLoanConfirmationByProposalId(decimal id);
        
        public bool InvestorLoanConfirmation(decimal investorId, decimal borrowerPropsalId, DateOnly confirmationDate);

    }
}

