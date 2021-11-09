using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SA.Core.Model
{
    public class Entity<T>
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual T Id { get; set; }
        
        [Required]
        public virtual DateTime Created { get; set; }
    }
}
