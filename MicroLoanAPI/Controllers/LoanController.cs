using System;
using DataService;
using Idfy;
using Idfy.IdentificationV2;
using MicroLoanAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroLoanAPI.Controllers
{
    [Route("api/loan")]
    [ApiController]
    public class LoanController : ControllerBase
	{
        private readonly IDataService _dataservice;

        public LoanController(IDataService dataService)
		{
            
            _dataservice = dataService;
        }

        [HttpPost("borrower-proposal")]
        public IActionResult CreateBorrowerProposal(BorrowerProposalModel model)
        {

    
            var borrowerProposal = _dataservice.CreateBorrowerProposal(model.BorrowerId, model.ProposalInterestRate, model.ProposalAmount, model.ProposalMonths);

            bool isActionSuccess = borrowerProposal.Item1;
            string responseMessage = borrowerProposal.Item2;

            if (isActionSuccess == false)
            {
                return BadRequest(new { isRegistrationSuccessful = isActionSuccess, responseMessage = responseMessage });
            }

            return Ok(new { isRegistrationSuccessful = isActionSuccess, responseMessage = responseMessage });
        }

       

        [HttpGet("borrower-proposals")]
        public IActionResult GetBorrowerProposals()
        {
            var borrowerProposals = _dataservice.GetBorrowerProposals();
            return Ok(borrowerProposals);
        }

    }
}

