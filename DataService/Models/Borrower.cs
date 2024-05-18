using System;
using System.Collections.Generic;

namespace DataService.Models
{
    public partial class Borrower
    {
        public Borrower()
        {
            BorrowerLiabilities = new HashSet<BorrowerLiability>();
            BorrowerProposals = new HashSet<BorrowerProposal>();
            BorrowerRatings = new HashSet<BorrowerRating>();
            InverseUserAccount = new HashSet<Borrower>();
        }

        public decimal Id { get; set; }
        public decimal? UserAccountId { get; set; }

        public virtual Borrower? UserAccount { get; set; }
        public virtual ICollection<BorrowerLiability> BorrowerLiabilities { get; set; }
        public virtual ICollection<BorrowerProposal> BorrowerProposals { get; set; }
        public virtual ICollection<BorrowerRating> BorrowerRatings { get; set; }
        public virtual ICollection<Borrower> InverseUserAccount { get; set; }
    }
}
