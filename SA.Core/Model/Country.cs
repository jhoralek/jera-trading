using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{
    public class Country : Entity<int>
    {
        [Required]

        public virtual string Name { get; set; }

        public virtual string Language { get; set; }
    }
}
