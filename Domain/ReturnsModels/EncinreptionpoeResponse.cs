using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ReturnsModels
{
    public class EncinreptionpoeRes<T>
    {
        public string Msg { get; set; }
        public bool IsOk { get; set; }
        public T Enc_Value { get; set; }
    }
}
