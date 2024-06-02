using System;
using DataService.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using DataService;
using Humanizer;
using static Humanizer.On;

namespace DataService
{
    public class DataServiceMain : IDataService
    {

        public DataServiceMain()
        {
        }
        Random rand = new Random();

        /* Reusable functions */

        public UserAccount GetUserById(int id)
        {
            microloan_dbContext db = new();
            var user = db.UserAccounts
            .Where(x => x.Id == id)
            .FirstOrDefault();
            return user;
        }

        public Borrower GetBorrowerByUserId(decimal userId)
        {
            microloan_dbContext db = new();
            var borrower = db.Borrowers
            .Where(x => x.UserAccountId == userId)
            .FirstOrDefault();
            return borrower;
        }

        public Borrower GetUserByBorrowerId(decimal borrowerId)
        {
            microloan_dbContext db = new();
            var investor = db.Borrowers
            .Include(b => b.UserAccount)
            .Include(b => b.InverseUserAccount)
            .FirstOrDefault(x => x.Id == borrowerId);

            return investor;
        }


        public Investor GetInvestorByUserId(decimal userId)
        {
            microloan_dbContext db = new();
            var investor = db.Investors
            .Where(x => x.UserAccountId == userId)
            .FirstOrDefault();
            return investor;
        }


        public UserAccount GetUserByMail(string email)
        {
            microloan_dbContext db = new();
            var user = db.UserAccounts
            .Where(x => x.EmailAdress == email)
            .FirstOrDefault();
            return user;
        }

        public IEnumerable<dynamic> GetAllUsers()
        {
            microloan_dbContext db = new();
            var users = db.UserAccounts
                .Select( user => new
                {
                    user.EmailAdress,
                    user.DateOfBirth,
                    user.FirstName,
                    user.LastName,
                    user.Id,
                    user.IsInvestor
                })
                .ToList();
            return users;
        }


        public BorrowerProposal GetBorrowerProposalById(decimal id)
        {
            microloan_dbContext db = new();
            var borrowerProposal = db.BorrowerProposals
            .Where(x => x.Id == id)
            .FirstOrDefault();
            return borrowerProposal;
        }

        public bool CreateInvestor(string email)
        {
            var user = GetUserByMail(email);

            microloan_dbContext db = new();

            var investor = new Investor
            {
                Id = rand.Next(1, 900000000 + 1),
                UserAccountId = user.Id
            };

            // Add the user to the database
            db.Investors.Add(investor);
            db.SaveChanges();
            return true;
        }

        public bool CreateBorrower(string email)
        {
            var user = GetUserByMail(email);

            microloan_dbContext db = new();

            var borrower = new Borrower
            {
                Id = rand.Next(1, 900000000 + 1),
                UserAccountId = user.Id
            };

            // Add the user to the database
            db.Borrowers.Add(borrower);
            db.SaveChanges();
            return true;
        }

        public Tuple<bool, string> RegisterUser(string email, decimal phone, string password, string confirmPassword, bool isInvestor)
        {
            var findUser = GetUserByMail(email);
            if (findUser != null)
            {
                return Tuple.Create(false, "User already exists");
            }
            if (password != confirmPassword)
            {
                return Tuple.Create(false, "Passwords do not match");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);


            var user = new UserAccount
            {
                Id = rand.Next(1, 900000000 + 1),
                EmailAdress = email,
                PhoneNumber = phone,
                Password = hashedPassword,
                IsInvestor = isInvestor
            };


            microloan_dbContext db = new();

            // Add the user to the database
            db.UserAccounts.Add(user);
            db.SaveChanges();

            if (isInvestor)
            {
                CreateInvestor(email);
            }
            else
            {
                CreateBorrower(email);
            }

            return Tuple.Create(true, "It was succesful");
        }

        public Tuple<bool, string> LoginUser(string email, string password)
        {
            var user = GetUserByMail(email);

            if (user == null)
            {
                return Tuple.Create(false, "User does not exist");
            }

            // Compare the provided password with the hashed password stored in the database
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!passwordMatch)
            {
                return Tuple.Create(false, "Incorrect password");
            }

          
            return Tuple.Create(true, "Login successful");
        }

        /* LOANS */


        public Tuple<bool, string> CreateBorrowerProposal(decimal borrowerId, decimal proposalInterestRate, decimal proposalAmount, decimal proposalMonthDate, string title, string description)
        {

            if (borrowerId == null || proposalInterestRate == null || proposalAmount == null || proposalMonthDate == null || title == null)
            {
                return Tuple.Create(false, "missing parameters");
            }

            var borrowerProposal = new BorrowerProposal
            {
                Id = rand.Next(1, 900000000 + 1),
                ProposalAmount = proposalAmount,
                ProposalInterestRate = proposalInterestRate,
                ProposalMonths = proposalMonthDate,
                BorrowerId = borrowerId,
                Title = title,
                Description = description
            };


            microloan_dbContext db = new();

            // Add the user to the database
            db.BorrowerProposals.Add(borrowerProposal);
            db.SaveChanges();


            return Tuple.Create(true, "Borrower proposal succesfully created");
        }

        public List<BorrowerProposal> GetBorrowerProposals()
        {
            microloan_dbContext db = new();
            var borrowerProposals = db.BorrowerProposals.ToList();
            return borrowerProposals;
        }

        public bool InvestorLoanConfirmation(decimal investorId, decimal borrowerPropsalId, DateOnly confirmationDate)
        {
            if (investorId == null || borrowerPropsalId == null || confirmationDate == null)
            {
                return false;
            }

            var findProposalById = GetBorrowerProposalById(borrowerPropsalId);
            if (findProposalById == null)
            {
                return false;
            }

            var investorLoanConfirmation = new InvestorLoanConfirmation
            {
                Id = rand.Next(1, 900000000 + 1),
                InvestorId = investorId,
                BorrowerProposalId = borrowerPropsalId,
                ConfirmationDate = confirmationDate
            };


            microloan_dbContext db = new();

            // Add the user to the database
            db.InvestorLoanConfirmations.Add(investorLoanConfirmation);
            db.SaveChanges();

            return true;

        }

        public InvestorLoanConfirmation GetLoanConfirmationById(decimal id)
        {
            microloan_dbContext db = new();
            var investorLoanConfirmation = db.InvestorLoanConfirmations
            .Where(x => x.Id == id)
            .FirstOrDefault();
            return investorLoanConfirmation;
        }


        public InvestorLoanConfirmation GetLoanConfirmationByProposalId(decimal id)
        {
            microloan_dbContext db = new();
            var investorLoanConfirmation = db.InvestorLoanConfirmations
            .Where(x => x.BorrowerProposalId == id)
            .FirstOrDefault();
            return investorLoanConfirmation;
        }



    }
}

