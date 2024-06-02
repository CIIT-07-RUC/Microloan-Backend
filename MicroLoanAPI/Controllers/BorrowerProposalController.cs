using System;
using DataService;
using Idfy;
using Idfy.IdentificationV2;
using MicroLoanAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace MicroLoanAPI.Controllers
{
    [Route("api/borrower-proposals")]
    [ApiController]
    public class BorrowerProposal : ControllerBase
	{
        private readonly IDataService _dataservice;

        public BorrowerProposal(IDataService dataService)
		{
            
            _dataservice = dataService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateBorrowerProposal(BorrowerProposalModel model)
        {
            var borrowerIdClaim = Decimal.Parse(User.FindFirst("RoleId").Value);
            var borrowerProposal = _dataservice.CreateBorrowerProposal(borrowerIdClaim, model.ProposalInterestRate, model.ProposalAmount, model.ProposalMonths);

            bool isActionSuccess = borrowerProposal.Item1;
            string responseMessage = borrowerProposal.Item2;

            if (isActionSuccess == false)
            {
                return BadRequest(new { isRegistrationSuccessful = isActionSuccess, responseMessage = responseMessage });
            }

            return Ok(new { isRegistrationSuccessful = isActionSuccess, responseMessage = responseMessage });
        }

       

        [HttpGet]
        public IActionResult GetBorrowerProposals()
        {
            var borrowerProposals = _dataservice.GetBorrowerProposals();
            return Ok(borrowerProposals);
        }


        [HttpGet("proposal-by-id")]
        public IActionResult GetBorrowerProposalById([FromQuery] decimal id)
        {
            var borrowerProposals = _dataservice.GetBorrowerProposalById(id);
            return Ok(borrowerProposals);
        }


        [HttpGet("borrower-proposal-status")]
        public IActionResult BorrowerProposalStatus([FromQuery] decimal id)
        {
            var loanConfirmation = _dataservice.GetLoanConfirmationByProposalId(id);
            if (loanConfirmation == null)
            {
                return Ok(false);
            }
            return Ok(true);
        }


        [Authorize]
        [HttpPost("loan-confirmation")]
        public IActionResult CreateInvestorLoanConfirmation(LoanConfirmationModel model)
        {
            var investorIdClaim = Decimal.Parse(User.FindFirst("RoleId").Value);
            var borrowerProposal = _dataservice.InvestorLoanConfirmation(investorIdClaim, model.BorrowerProposalId, model.ConfirmationDate);

            if (borrowerProposal == false)
            {
                return BadRequest(new { borrowerProposal, message = "Borower proposal was not found"});
            }

            return Ok(new { borrowerProposal , message = "Loan-confirmation is successful"});
        }

        [HttpGet("loan-confirmation")]
        public IActionResult GetInvestorLoanConfirmationById([FromQuery] decimal id)
        {
            var borrowerProposals = _dataservice.GetLoanConfirmationById(id);
            return Ok(borrowerProposals);
        }

    }
}

