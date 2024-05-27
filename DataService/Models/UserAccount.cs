using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            Investors = new HashSet<Investor>();
        }

        public decimal Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAdress { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public decimal? PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public bool IsInvestor { get; set; }

        public virtual ICollection<Investor> Investors { get; set; }
    }
}
