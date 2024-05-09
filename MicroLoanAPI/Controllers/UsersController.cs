using System;
using DataService;
using MicroLoanAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroLoanAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
	{
        private readonly IDataService _dataservice;

        public UsersController(IDataService dataService)
		{
            
            _dataservice = dataService;
        }

        [HttpPost]
        public IActionResult SignIn(RegisterUserModel model)
        {
            Console.WriteLine("model", model);
            var registerUser = _dataservice.RegisterUser(model.EmailAdress, model.PhoneNumber, model.Password, model.ConfirmPassword);

            bool isRegistrationSuccessful = registerUser.Item1;
            string responseMessage = registerUser.Item2;

            if (isRegistrationSuccessful == false)
            {
                return BadRequest(new { isRegistrationSuccessful = isRegistrationSuccessful, responseMessage = responseMessage });
            }

            return Ok(new { isRegistrationSuccessful = isRegistrationSuccessful, responseMessage = responseMessage });
        }

        [HttpGet]
        public IActionResult GetUserByMail([FromQuery] string email)
        {
            var user = _dataservice.GetUserByMail(email);
            return Ok(user);
        }
    }
}

