using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{
    public class UserActivation : Entity<int>
    {
        public virtual DateTime? Verified { get; set; }

        [Required]
        public virtual int UserId { get; set; }

        [Required]
        public virtual string Token { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool IsUsed { get; set; }
    }
}
