using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{
    public class User : Entity<int>
    {
        [Required]
        [MinLength(6)]
        public virtual string UserName { get; set; }

        [Required]
        [MinLength(8)]
        public virtual string Password { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool IsActive { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool IsAgreementToTerms { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool SendingNews { get; set; }

        public virtual string Token { get; set; }

        [Required]
        [MaxLength(2)]
        public virtual string Language { get; set; }

        [Required]
        public virtual int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual List<Record> Records { get; set; } = new List<Record>();

        public virtual List<Bid> Bids { get; set; } = new List<Bid>();
    }
}
