using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    [Index(nameof(Phonnumber), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    public class UserModel
    {

        [Key]
        public int Id { get; set; }
        [Length(5, 50)]
        public string Username { get; set; }
        [Length(5, 50)]
        public string Password { get; set; }
        public string Role { get; set; }
        public string Phonnumber { get; set; }
        public List<RefreshToken> RefreshTokens  { get; set; } = new List<RefreshToken>();

    }
}
