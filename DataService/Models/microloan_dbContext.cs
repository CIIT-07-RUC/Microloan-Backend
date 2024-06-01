using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataService.Models
{
    public partial class microloan_dbContext : DbContext
    {
        public microloan_dbContext()
        {
        }

        public microloan_dbContext(DbContextOptions<microloan_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountStatement> AccountStatements { get; set; } = null!;
        public virtual DbSet<Borrower> Borrowers { get; set; } = null!;
        public virtual DbSet<BorrowerLiability> BorrowerLiabilities { get; set; } = null!;
        public virtual DbSet<BorrowerProposal> BorrowerProposals { get; set; } = null!;
        public virtual DbSet<BorrowerRating> BorrowerRatings { get; set; } = null!;
        public virtual DbSet<Investor> Investors { get; set; } = null!;
        public virtual DbSet<InvestorLoanConfirmation> InvestorLoanConfirmations { get; set; } = null!;
        public virtual DbSet<LoanRepaymentSchedule> LoanRepaymentSchedules { get; set; } = null!;
        public virtual DbSet<LoanTicket> LoanTickets { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<UserAccount> UserAccounts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=microloan_db;Username=postgres;Password=admin;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountStatement>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("account_statement");

                entity.Property(e => e.ClosingBalance)
                    .HasPrecision(10, 2)
                    .HasColumnName("closing_balance");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("id");

                entity.Property(e => e.InvestorId)
                    .HasPrecision(10)
                    .HasColumnName("investor_id");

                entity.Property(e => e.TransactionAmount)
                    .HasPrecision(10, 2)
                    .HasColumnName("transaction_amount");

                entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            });

            modelBuilder.Entity<Borrower>(entity =>
            {
                entity.ToTable("borrower");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UserAccountId).HasColumnName("user_account_id");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.InverseUserAccount)
                    .HasForeignKey(d => d.UserAccountId)
                    .HasConstraintName("borrower_user_account_id_fkey");
            });

            modelBuilder.Entity<BorrowerLiability>(entity =>
            {
                entity.ToTable("borrower_liability");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BorrowerId).HasColumnName("borrower_id");

                entity.Property(e => e.LiabilityEndDate).HasColumnName("liability_end_date");

                entity.Property(e => e.LiabilityOutstanding)
                    .HasPrecision(10, 2)
                    .HasColumnName("liability_outstanding");

                entity.Property(e => e.LiabilityStartDate).HasColumnName("liability_start_date");

                entity.Property(e => e.LoanTicketId).HasColumnName("loan_ticket_id");

                entity.Property(e => e.MonthlyRepaymentAmount)
                    .HasPrecision(10, 2)
                    .HasColumnName("monthly_repayment_amount");

                entity.HasOne(d => d.Borrower)
                    .WithMany(p => p.BorrowerLiabilities)
                    .HasForeignKey(d => d.BorrowerId)
                    .HasConstraintName("borrower_liability_borrower_id_fkey");

                entity.HasOne(d => d.LoanTicket)
                    .WithMany(p => p.BorrowerLiabilities)
                    .HasForeignKey(d => d.LoanTicketId)
                    .HasConstraintName("borrower_liability_loan_ticket_id_fkey");
            });

            modelBuilder.Entity<BorrowerProposal>(entity =>
            {
                entity.ToTable("borrower_proposal");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BorrowerId).HasColumnName("borrower_id");

                entity.Property(e => e.Organization)
                    .HasMaxLength(255)
                    .HasColumnName("organization");

                entity.Property(e => e.Description)
                 .HasMaxLength(255)
                 .HasColumnName("description");

                entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

                entity.Property(e => e.ProposalAmount)
                    .HasPrecision(10, 2)
                    .HasColumnName("proposal_amount");

                entity.Property(e => e.ProposalInterestRate).HasColumnName("proposal_interest_rate");

                entity.Property(e => e.ProposalMonths).HasColumnName("proposal_months");

                entity.HasOne(d => d.Borrower)
                    .WithMany(p => p.BorrowerProposals)
                    .HasForeignKey(d => d.BorrowerId)
                    .HasConstraintName("borrower_proposal_borrower_id_fkey");
            });

            modelBuilder.Entity<BorrowerRating>(entity =>
            {
                entity.ToTable("borrower_rating");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BorrowerId).HasColumnName("borrower_id");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");

                entity.Property(e => e.InvestorId).HasColumnName("investor_id");

                entity.Property(e => e.LoanTicketId).HasColumnName("loan_ticket_id");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.RatingDate).HasColumnName("rating_date");

                entity.HasOne(d => d.Borrower)
                    .WithMany(p => p.BorrowerRatings)
                    .HasForeignKey(d => d.BorrowerId)
                    .HasConstraintName("borrower_rating_borrower_id_fkey");

                entity.HasOne(d => d.Investor)
                    .WithMany(p => p.BorrowerRatings)
                    .HasForeignKey(d => d.InvestorId)
                    .HasConstraintName("borrower_rating_investor_id_fkey");

                entity.HasOne(d => d.LoanTicket)
                    .WithMany(p => p.BorrowerRatings)
                    .HasForeignKey(d => d.LoanTicketId)
                    .HasConstraintName("borrower_rating_loan_ticket_id_fkey");
            });

            modelBuilder.Entity<Investor>(entity =>
            {
                entity.ToTable("investor");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InvestmentLimit)
                    .HasPrecision(10, 2)
                    .HasColumnName("investment_limit");

                entity.Property(e => e.TaxId)
                    .HasMaxLength(40)
                    .HasColumnName("tax_id");

                entity.Property(e => e.TotalFundsCommited)
                    .HasPrecision(10, 2)
                    .HasColumnName("total_funds_commited");

                entity.Property(e => e.UserAccountId).HasColumnName("user_account_id");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Investors)
                    .HasForeignKey(d => d.UserAccountId)
                    .HasConstraintName("investor_user_account_id_fkey");
            });

            modelBuilder.Entity<InvestorLoanConfirmation>(entity =>
            {
                entity.ToTable("investor_loan_confirmation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BorrowerProposalId).HasColumnName("borrower_proposal_id");

                entity.Property(e => e.ConfirmationDate).HasColumnName("confirmation_date");

                entity.Property(e => e.InvestorId).HasColumnName("investor_id");

                entity.HasOne(d => d.BorrowerProposal)
                    .WithMany(p => p.InvestorLoanConfirmations)
                    .HasForeignKey(d => d.BorrowerProposalId)
                    .HasConstraintName("investor_loan_confirmation_borrower_proposal_id_fkey");

                entity.HasOne(d => d.Investor)
                    .WithMany(p => p.InvestorLoanConfirmations)
                    .HasForeignKey(d => d.InvestorId)
                    .HasConstraintName("investor_loan_confirmation_investor_id_fkey");
            });

            modelBuilder.Entity<LoanRepaymentSchedule>(entity =>
            {
                entity.ToTable("loan_repayment_schedule");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DueEmiAmount)
                    .HasPrecision(10, 2)
                    .HasColumnName("due_emi_amount");

                entity.Property(e => e.DueInterestAmount)
                    .HasPrecision(10, 2)
                    .HasColumnName("due_interest_amount");

                entity.Property(e => e.DuePrincipalAmount)
                    .HasPrecision(10, 2)
                    .HasColumnName("due_principal_amount");

                entity.Property(e => e.EmiAmountRecieved)
                    .HasPrecision(10, 2)
                    .HasColumnName("emi_amount_recieved");

                entity.Property(e => e.EmiDueDate).HasColumnName("emi_due_date");

                entity.Property(e => e.LoanTicketId).HasColumnName("loan_ticket_id");

                entity.Property(e => e.RecieveDate).HasColumnName("recieve_date");

                entity.Property(e => e.TotalDueAmount)
                    .HasPrecision(10, 2)
                    .HasColumnName("total_due_amount");

                entity.HasOne(d => d.LoanTicket)
                    .WithMany(p => p.LoanRepaymentSchedules)
                    .HasForeignKey(d => d.LoanTicketId)
                    .HasConstraintName("loan_repayment_schedule_loan_ticket_id_fkey");
            });

            modelBuilder.Entity<LoanTicket>(entity =>
            {
                entity.ToTable("loan_ticket");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InterestRate)
                    .HasPrecision(3, 2)
                    .HasColumnName("interest_rate");

                entity.Property(e => e.InvestorLoanConfirmationId).HasColumnName("investor_loan_confirmation_id");

                entity.Property(e => e.LoanAmount)
                    .HasPrecision(10, 2)
                    .HasColumnName("loan_amount");

                entity.Property(e => e.LoanTenureInMonths).HasColumnName("loan_tenure_in_months");

                entity.Property(e => e.RiskRating)
                    .HasMaxLength(1)
                    .HasColumnName("risk_rating");

                entity.HasOne(d => d.InvestorLoanConfirmation)
                    .WithMany(p => p.LoanTickets)
                    .HasForeignKey(d => d.InvestorLoanConfirmationId)
                    .HasConstraintName("loan_ticket_investor_loan_confirmation_id_fkey");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("payment_method");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountHolderName)
                    .HasMaxLength(255)
                    .HasColumnName("account_holder_name");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(255)
                    .HasColumnName("account_number");

                entity.Property(e => e.AccountType)
                    .HasMaxLength(1)
                    .HasColumnName("account_type");

                entity.Property(e => e.BankName)
                    .HasMaxLength(255)
                    .HasColumnName("bank_name");

                entity.Property(e => e.InvestorId).HasColumnName("investor_id");

                entity.Property(e => e.WireTransferCode).HasColumnName("wire_transfer_code");

                entity.HasOne(d => d.Investor)
                    .WithMany(p => p.PaymentMethods)
                    .HasForeignKey(d => d.InvestorId)
                    .HasConstraintName("payment_method_investor_id_fkey");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("user_account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");

                entity.Property(e => e.EmailAdress)
                    .HasMaxLength(100)
                    .HasColumnName("email_adress");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsInvestor).HasColumnName("is_investor");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
