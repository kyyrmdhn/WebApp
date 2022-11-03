using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [Required]
        public int DivisionId { get; set; }
        public Division Division { get; set; }
        public Department(int id, string name, int divisionId)
        {
            Id = id;
            Name = name;
            DivisionId = divisionId;    
        }
        public Department()
        {

        }
    }
}
