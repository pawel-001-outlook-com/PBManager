using PBManager.Core.Models;

namespace PBManager.DAL.Contracts
{
    public interface IUserRepository
    {
        User IfValidUser(string userName, string passwordHash);
        bool AddUser(User user);
        User GetUser(int userId);
        bool UpdateUser(User user);
    }
}
