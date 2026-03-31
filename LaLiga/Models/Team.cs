using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaLiga
{
    public class Team
    {
            //Primary Key
            [Key]
            [Column(TypeName = "char(3)")]
            
            public required string? TeamID { get; set; }
            public string? Name { get; set; }
            public string? City { get; set; }
            public string? Stadium { get; set; }
        }


 }

