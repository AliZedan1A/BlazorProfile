using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class JWTDataModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string PhonNumber { get; set; }

        public JWTDataModel()
        {
            if (Role == null) Role = "user";

        }
    }
}
