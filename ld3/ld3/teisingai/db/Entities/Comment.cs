using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Org.Ktu.T120B197.DBs.Entities
{
    public partial class Comment
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int id { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string content { get; set; }

        [Column(TypeName = "int(11)")]
        public int exampleId { get; set; }

        // Foreign key relationship
        public virtual Example Example { get; set; }
    }
}
