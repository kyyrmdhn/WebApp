using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string FullName { get; set; }
        [StringLength(50)]
        [Required]
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        //public User User { get; set; }
        public Employee()
        {

        }
    }
}
