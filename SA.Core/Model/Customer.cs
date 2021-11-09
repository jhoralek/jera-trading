using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{
    public class Customer : Entity<int>
    {
        [Required]
        public virtual string FirstName { get; set; }

        [Required]
        public virtual string LastName { get; set; }
        
        public virtual string BirthNumber { get; set; }

        [Required]
        [Phone]
        public virtual string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]        
        public virtual string Email { get; set; }

        public virtual string TitleBefore { get; set; }

        public virtual string TitleAfter { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool IsDealer { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool IsFeePayed { get; set; }

        [Url]
        public virtual string WebPageUrl { get; set; }
        
        public virtual string CompanyNumber { get; set; }
        
        public virtual string CompanyName { get; set; }

        public virtual string CompanyLegalNumber { get; set; }

        [Required]
        public virtual int AddressId { get; set; }

        public virtual Address Address { get; set; }
    }
}
