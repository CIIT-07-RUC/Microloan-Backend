using System;
using DataService.Models;

namespace DataService
{
	public interface IDataService
	{
        public UserAccount GetUserById(int id);
        public UserAccount GetUserByMail(string email);
        public Tuple<bool, string> RegisterUser(string email, decimal phone, string password, string confirmPassword, bool isInvestor);
        public Tuple<bool, string> LoginUser(string email, string password);
    }
}

