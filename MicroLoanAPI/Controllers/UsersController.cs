using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataService;
using Idfy;
using Idfy.IdentificationV2;
using MicroLoanAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace MicroLoanAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
	{
        private readonly IConfiguration _configuration;
        private readonly IDataService _dataservice;
        private readonly IIdentificationV2Service _identificationV2Service;

        public UsersController(IDataService dataService, IConfiguration configuration)
		{
            _configuration = configuration;
            _dataservice = dataService;
            _identificationV2Service = new IdentificationV2Service("sandbox-stunning-bag-347", "vMsNWEBnGHDlxVmt4XRrF1RYWdcfKNnCnFNxWXJ7GOSn6obs", new List<OAuthScope> { OAuthScope.Identify});
        }

        [HttpPost]
        public IActionResult SignIn(RegisterUserModel model)
        {
    
            var registerUser = _dataservice.RegisterUser(model.EmailAdress, model.PhoneNumber, model.Password, model.ConfirmPassword, model.IsInvestor);

            bool isRegistrationSuccessful = registerUser.Item1;
            string responseMessage = registerUser.Item2;

            if (isRegistrationSuccessful == false)
            {
                return BadRequest(new { isRegistrationSuccessful = isRegistrationSuccessful, responseMessage = responseMessage });
            }

            return Ok(new { isRegistrationSuccessful = isRegistrationSuccessful, responseMessage = responseMessage });
        }

        [HttpGet("all-users")]
        public IActionResult GetBorrowerProposals()
        {
            var allUsers = _dataservice.GetAllUsers();
            return Ok(allUsers);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUserModel model)
        {
            var loginUser = _dataservice.LoginUser(model.EmailAdress, model.Password);

            bool isLoginSuccessful = loginUser.Item1;
            string responseMessage = loginUser.Item2;

            if (isLoginSuccessful == false)
            {
                return BadRequest(new { isRegistrationSuccessful = isLoginSuccessful, responseMessage = responseMessage });
            }

            var findUserByEmail = _dataservice.GetUserByMail(model.EmailAdress);
            dynamic specifingRoleId;
            if (findUserByEmail.IsInvestor)
            {
                specifingRoleId = _dataservice.GetInvestorByUserId(findUserByEmail.Id).Id;
            }
            else
            {
                specifingRoleId = _dataservice.GetBorrowerByUserId(findUserByEmail.Id).Id;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, findUserByEmail.EmailAdress),
                new Claim("IsInvestor", findUserByEmail.IsInvestor.ToString()),
                new Claim("RoleId", specifingRoleId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, findUserByEmail.Id.ToString()),
            };

            var secret = _configuration.GetSection("Authentication:Secret").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.Now.AddDays(4),
               signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            return Ok(new {
                isRegistrationSuccessful = isLoginSuccessful,
                responseMessage,
                token = jwt
            });
        }

        [HttpGet]
        public IActionResult GetUserByMail([FromQuery] string email)
        {
            var user = _dataservice.GetUserByMail(email);
            return Ok(user);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _dataservice.GetUserById(id);
            if(user == null )
            {
                return BadRequest();

            }
            return Ok(user);
        }


        [HttpPost("create-mit-id-session")]
        public async Task<IActionResult> CreateMitIdSession()
        {
            var frontendDomain = "http://localhost:3000/";
            var backendDomain = "http://localhost:5205/";

            try
            {
                var session = await _identificationV2Service.CreateSessionAsync(new IdSessionCreateOptions()
                {
                    Flow = IdSessionFlow.Redirect,
                    RedirectSettings = new RedirectSettings()
                    {
                        ErrorUrl = frontendDomain + "?error=true",
                        AbortUrl = frontendDomain + "?canceled=true",
                        SuccessUrl = backendDomain + "authentication-session"
                    },
                    AllowedProviders = new List<IdProviderType>
                {
                    IdProviderType.Mitid
                },
                    Include = new List<Include>()
                {
                    Include.Name,
                    Include.Nin
                }
                });

                // Return the URL to the client
                return Ok(new { url = session.Url });
            }
            catch (IdfyException ex)
            {
                // Log detailed error information
                Console.WriteLine($"IdfyException: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                    Console.WriteLine($"InnerException StackTrace: {ex.InnerException.StackTrace}");
                }
                else
                {
                    Console.WriteLine("InnerException is null.");
                }

                // Return a meaningful error message to the client
                return StatusCode(500, $"Internal server error occurred while creating MitID session. Exception Message: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet("get-mit-id-session")]
        public async Task<IActionResult> RetrieveMitId([FromQuery(Name = "sessionId")] string sessionId)
        {
            var frontendDomain = "http://localhost:3000/";
            var result = await _identificationV2Service.GetSessionAsync(sessionId);
            string name = result.Identity.FullName;
            string nin = result.Identity.Nin;

            Response.Headers.Add("Location", frontendDomain + "?success=true&name=" + name + "%nin=" + nin);
            return new StatusCodeResult(303);

        }



    }
}

