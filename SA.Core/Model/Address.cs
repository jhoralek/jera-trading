using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{
    public class Address : Entity<int>
    {
        [Required]
        public virtual string Street { get; set; }

        [Required]        
        public virtual string City { get; set; }

        [Required]        
        public virtual string PostCode { get; set; }

        [Required]
        public virtual int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}
