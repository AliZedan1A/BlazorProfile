using Domain.DTO;
using Domain.Models;
using Domain.ReturnsModels;


namespace ServerSide.Service.IServices
{
    public interface IAuthService
    {
        ServiceReturnModel<RefreshTokenResponseModel> Auth(AuthDto req);
        ServiceReturnModel<string> Register(RegstirDto req);
        RefreshTokenResponseModel GenerateRefresh(UserModel User = null, int id = 0);
        UserModel RemoveRefreshToken( string RefreshToken);
    }
}
