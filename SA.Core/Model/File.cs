using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{
    public class File : Entity<int>
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]        
        public virtual string Path { get; set; }

        [Required]
        public virtual int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public virtual int RecordId { get; set; }

        public Record Record { get; set; }
    }
}
