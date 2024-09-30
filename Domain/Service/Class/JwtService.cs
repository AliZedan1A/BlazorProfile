using Domain.ReturnsModels;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Domain.Models;
using Domain.Service.Interfaces;
using System.Security.Cryptography;

namespace Domain.Service.Class
{
    public class JwtService<T> : IJwtService<T> where T : class
    {
        private readonly IEncreption encreption;

        public JwtService(IEncreption encreption)
        {
            this.encreption = encreption;
        }
        public string Create(T data)
        {
            string json = JsonConvert.SerializeObject(data);
            List<Claim> clams = new List<Claim>() {
            new Claim(ClaimTypes.Name,json)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("rashwer34534834yry43ehuhg8934y328yrehfhf9834yhtrfrtet34ed"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: clams, expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            EncinreptionpoeRes<string> EncodedJWT = encreption.En_Code(jwt);
            return EncodedJWT.Enc_Value;

        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddDays(60),
                
            };
        }

        public ReturnJwtModel Get(string EncodedToken)
        {
            var token = encreption.De_Code(EncodedToken);
            if (!token.IsOk)
            {
                Console.WriteLine(token.Msg);
                return new() { IsValid = false };
            }
            JwtSecurityTokenHandler handler = new();
            var isItToken = handler.CanReadToken(token.Enc_Value);
            if (!isItToken)
            {
                return new() { IsValid = false };
            }
            var decodedValue = handler.ReadJwtToken(token.Enc_Value);

            var time = decodedValue.ValidTo;
            var text = decodedValue.Claims.SingleOrDefault(x => x.ValueType == ClaimValueTypes.String).Value;
            DateTimeOffset expireDate = new DateTimeOffset(time);

            Console.WriteLine(expireDate.ToString());
            if (expireDate.UtcDateTime < DateTime.UtcNow)
            {
                return new() { IsValid = false };
            }
            JWTDataModel value = JsonConvert.DeserializeObject<JWTDataModel>(text)!;
            return new() { expireDate = expireDate.Date, Data = value, IsValid = true };
        }

    }
}
