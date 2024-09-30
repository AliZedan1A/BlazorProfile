using Domain.ReturnsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Domain.Service.Interfaces
{
    public interface IEncreption
    {
        EncinreptionpoeRes<string> En_Code(string words);
        EncinreptionpoeRes<string> De_Code(string words);
        string Hash(string word);
        bool Compare_Hash(string word, string hash);

    }
}
