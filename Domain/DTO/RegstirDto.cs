using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class RegstirDto
    {
        [Length(5, 50)]
        public string UserName { get; set; }
        [Length(5, 50)]
        public string Password { get; set; }
        public string PhonNumber { get; set; }
    }
}
