using System;
namespace MicroLoanAPI.Models
{
	public class RegisterUserModel
	{
		public string? EmailAdress { get; set; }
        public decimal PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
	}
}

