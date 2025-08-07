using System.ComponentModel.DataAnnotations;

namespace BotAdminPanel.Models.Entity
{
    public class User
    {
        public User()
        {
            
        }

        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
