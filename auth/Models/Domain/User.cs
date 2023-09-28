using System.ComponentModel.DataAnnotations;

namespace auth.Models.Domain
{
    public class User
    {
        [Key]
        public string Guid { get; set; }
        public string Login { get; set; }

        [MaxLength(24), MinLength(6)]
        public string Password { get; set; }
    }
}
