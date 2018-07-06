﻿using MySql.Data.EntityFrameworkCore.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{
    [MySqlCharset("utf8")]
    [MySqlCollation("utf8_general_ci")]
    public class User : Entity<int>
    {
        [Required]
        [MinLength(6)]
        [MySqlCharset("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public virtual string UserName { get; set; }

        [Required]
        [MinLength(8)]
        [MySqlCharset("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public virtual string Password { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool IsActive { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool IsAgreeementToTerms { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool SendingNews { get; set; }

        [MySqlCharset("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public virtual string Token { get; set; }

        [Required]
        [MaxLength(2)]
        [MySqlCharset("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public virtual string Language { get; set; }

        [Required]
        public virtual int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<Record> Records { get; set; } = new List<Record>();

        public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}
