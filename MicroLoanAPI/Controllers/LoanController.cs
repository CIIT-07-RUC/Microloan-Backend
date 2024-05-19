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

    }
}

