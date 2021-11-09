using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{    
    public class Auction : Entity<int>
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool IsActive { get; set; }
        
        [Required]
        public virtual DateTime ValidFrom { get; set; }

        [Required]
        public virtual DateTime ValidTo { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool IsEnded { get; set; }

        public virtual ICollection<Record> Records { get; set; } = new List<Record>();
    }
}
