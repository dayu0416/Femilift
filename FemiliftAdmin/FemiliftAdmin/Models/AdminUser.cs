using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FemiliftAdmin.Models
{
    [Table("AdminUsers")]
    public class AdminUser
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }
    }
}
