using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.ReturnsModels
{
    public class ReturnJwtModel
    {
        public DateTime? expireDate { get; set; }
        public JWTDataModel Data { get; set; }
        public bool IsValid { get; set; }
    }
}
