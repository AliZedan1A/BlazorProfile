using Domain.ReturnsModels;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service.Interfaces
{
    public interface IJwtService<T> where T : class
    {
        string Create(T data);
        ReturnJwtModel Get(string EncodedToken);
        RefreshToken GenerateRefreshToken();
    }
}
