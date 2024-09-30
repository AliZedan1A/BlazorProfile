using Domain.DTO;
using Domain.Models;
using Domain.ReturnsModels;
using Domain.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using ServerSide.Repository.Repositorys;
using ServerSide.Service.IServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ServerSide.Service
{
    public class AuthService:IAuthService
    {
        private readonly UserRepository _userRepository;
        private readonly IJwtService<JWTDataModel> _jwtService;
        private readonly IEncreption _encreption;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(UserRepository userRepository,IJwtService<JWTDataModel> jwtService,IEncreption encreption, IHttpContextAccessor  httpContextAccessor)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _encreption = encreption;
            _httpContextAccessor = httpContextAccessor;
        }
        public UserModel RemoveRefreshToken(string RefreshToken)
        {
           var response =  _userRepository.RemoveUserRefreshToken(RefreshToken);
            return response;
        }
            public RefreshTokenResponseModel GenerateRefresh(UserModel User = null, int id =0)
        {
            if(User !=null)
            {
                var model = new JWTDataModel() { Role = User.Role, UserId = User.Id, UserName = User.Username, PhonNumber = User.Phonnumber };
                var jwt = _jwtService.Create(model);
                var refresh = new RefreshTokenResponseModel()
                {
                    ExpiresTime = DateTime.UtcNow.AddMinutes(5)
              ,
                    Jwt = jwt,
                    Data = model
                };
                var token = _jwtService.GenerateRefreshToken();
                token.User = User;
                token.UserID = User.Id;
                User.RefreshTokens.Add(token);
                _userRepository.SaveChanges();
                var options = new CookieOptions
                {
                    Path = "/",
                    Expires = token.Expires,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true
                };
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("RefreshToken", token.Token, options);
                return refresh;
            }else if(id !=0)
            {
                var fetchuser = _userRepository.Get(id);
                if(fetchuser != null)
                {
                    var model = new JWTDataModel() { Role = fetchuser.Role, UserId = fetchuser.Id, UserName = fetchuser.Username, PhonNumber = fetchuser.Phonnumber };
                    var jwt = _jwtService.Create(model);
                    var refresh = new RefreshTokenResponseModel()
                    {
                        ExpiresTime = DateTime.UtcNow.AddMinutes(5)
                  ,
                        Jwt = jwt,
                        Data = model
                    };
                    var token = _jwtService.GenerateRefreshToken();
                    token.User = fetchuser;
                    token.UserID = fetchuser.Id;
                    fetchuser.RefreshTokens.Add(token);
                    _userRepository.SaveChanges();
                    var options = new CookieOptions
                    {
                        Path = "/",
                        Expires = token.Expires,
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        Secure = true
                    };
                    _httpContextAccessor.HttpContext?.Response.Cookies.Append("RefreshToken", token.Token, options);
                    return refresh;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
           

        }
            public ServiceReturnModel<RefreshTokenResponseModel> Auth(AuthDto req)
        {
            var User = _userRepository.GetUserByName(req.Username);
            if (User == null)
            {
                return new ServiceReturnModel<RefreshTokenResponseModel>() { Comment = "Username or Password not correct", IsSucceeded = false };
            }
            else
            {
              
                var res = _encreption.Compare_Hash(req.Password, User.Password);
                if (res)
                {
                    var refresh = GenerateRefresh(User);


                    return new ServiceReturnModel<RefreshTokenResponseModel>() { Value= refresh, Comment = "User Found!", IsSucceeded = true };

                }
                else
                {
                    return new ServiceReturnModel<RefreshTokenResponseModel>() {  Comment = "Username or Password not correct", IsSucceeded = true };

                }



            }

        }

        public ServiceReturnModel<string> Register(RegstirDto req)
        {
            var User = new UserModel() {
                Password = _encreption.Hash(req.Password),
                Username = req.UserName,
                Role = "USER",
                Phonnumber = req.PhonNumber
            };
            try
            {

            
            var response =  _userRepository.Insert(User);
            if(response.IsSucceeded)
            {
                return new ServiceReturnModel<string>() {
                    IsSucceeded = true,
                    Comment = "UserRegisterd",
                    Value = null
                
                };

            }
            else {
                return new ServiceReturnModel<string>()
                 {
                      IsSucceeded = false,
                      Comment = response.Comment,
                      Value = null

                 };
                }
            }catch(DbUpdateException ex)
            {
                return new ServiceReturnModel<string>()
                {
                    IsSucceeded = false,
                    Comment = ex.Message,
                    Value = null

                };
            }
            catch (Exception ex)
            {
                return new ServiceReturnModel<string>()
                {
                    IsSucceeded = false,
                    Comment = ex.Message,
                    Value = null

                };
            }
        }
    }
}
