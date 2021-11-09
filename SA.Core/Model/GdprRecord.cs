using SA.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{    
    public class GdprRecord : Entity<int>
    {
        [Required]
        public virtual string FirstName { get; set; }

        [Required]        
        public virtual string LastName { get; set; }

        [Required]
        [EmailAddress]        
        public virtual string Email { get; set; }
        
        public virtual string CompanyNumber { get; set; }

        public virtual string BirthNumber { get; set; }

        public virtual string CompanyName { get; set; }

        [Required]
        public virtual string PhoneNumber { get; set; }

        [Required]
        public virtual string Street { get; set; }

        [Required]
        public virtual string City { get; set; }

        [Required]
        public virtual string PostCode { get; set; }

        [Required]
        public virtual int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual string Description { get; set; }

        [Required]
        public virtual GdprApplicationType Type { get; set; }

        [Required]
        public virtual bool IsProcessed { get; set; }
    }
}
