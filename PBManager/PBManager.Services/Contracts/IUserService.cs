using PBManager.Core.Models;

namespace PBManager.Services.Contracts
{
    public interface IUserService
    {
        User IfValidUser(string userName, string hashPassword);
        bool AddUser(User user);
        User GetUser(int userId);
        bool Update(User user);
    }
}
