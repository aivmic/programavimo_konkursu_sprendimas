using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Org.Ktu.T120B197.DBs.Entities;

public partial class Example
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int id { get; set; }

    [Column(TypeName = "int(11)")]
    public int number { get; set; }

    [Required]
    [Column(TypeName = "text")]
    public string text { get; set; }

    // Navigation properties
    public virtual Category Category { get; set; }
    public virtual Author Author { get; set; }
    public virtual ICollection<Tag> Tags { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
}
