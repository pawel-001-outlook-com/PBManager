using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using System.Data.Entity;
using System.Linq;

namespace PBManager.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public User IfValidUser(string userName, string passwordHash)
        {
            return _dataContext.Users
                .SingleOrDefault(
                    u => u.UserName.ToLower() == userName.ToLower()
                         && u.Password == passwordHash
                );
        }

        public bool AddUser(User user)
        {
            if (_dataContext.Users.Any(a => a.UserName.Equals(user.UserName)) != true)
            {
                if (_dataContext.Users.Add(user) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public User GetUser(int userId)
        {
            return _dataContext.Users
                .SingleOrDefault(
                    u => u.Id.Equals(userId)
                );
        }

        public bool UpdateUser(User user)
        {
            try
            {
                _dataContext.Entry(user).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
