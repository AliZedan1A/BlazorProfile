using ServerSide.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ServerSide.Repository.Repositorys
{
    public class UserRepository : Repository<UserModel>
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public int TableCount()
        {
            return _context.UserTable.Count();
        }
        public UserModel GetUserByName(string username)
        {
            var user = _context.UserTable.FirstOrDefault(x => x.Username == username);
            return user;
        }
        public UserModel RemoveUserRefreshToken(string refresh)
        {
            var user = _context.UserTable.Where(x => x.RefreshTokens.FirstOrDefault(x=>x.Token==refresh)!=null).Include(x=>x.RefreshTokens).FirstOrDefault(); //include for relation
            if(user != null)
            {
                var res= user.RefreshTokens.Find(x => x.Token == refresh);
                if(res!=null)
                {
                    user.RefreshTokens.Remove(res);
                    return user;
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


    }
}
