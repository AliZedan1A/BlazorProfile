using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        [Key]
        public int TokenId { get; set; }
        public DateTime Expires { get; set; }
        public bool IsActive => DateTime.UtcNow < Expires;
        public int UserID { get; set; }
        public UserModel User { get; set; }
    }

}
