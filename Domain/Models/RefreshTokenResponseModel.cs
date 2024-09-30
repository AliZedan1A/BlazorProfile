using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RefreshTokenResponseModel
    {
        public string Jwt { get; set; }
        public DateTime ExpiresTime { get; set; }
        public JWTDataModel Data  { get; set; }

    }
}
