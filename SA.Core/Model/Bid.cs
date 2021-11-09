using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{
    public class Bid : Entity<int>
    {
        [Required]
        public virtual decimal Price { get; set; }

        [Required]        
        public virtual int RecordId { get; set; }

        public virtual Record Record { get; set; }

        [Required]
        public virtual int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
