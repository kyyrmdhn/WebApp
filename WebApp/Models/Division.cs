using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Division
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public Division(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
