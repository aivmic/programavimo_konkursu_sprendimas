using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Org.Ktu.T120B197.DBs.Entities
{
    public partial class Author
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string name { get; set; }

        // Navigation property
        public virtual ICollection<Example> Examples { get; set; }
    }
}
