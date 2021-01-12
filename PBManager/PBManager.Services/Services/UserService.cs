using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using PBManager.Services.Contracts;
using Unity.Attributes;

namespace PBManager.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService()
        {
        }

        [InjectionConstructor]
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User IfValidUser(string userName, string passwordHash)
        {
            return _unitOfWork.users.IfValidUser(userName, passwordHash);
        }

        public bool AddUser(User user)
        {
            if (_unitOfWork.users.AddUser(user))
            {
                _unitOfWork.Complete();
                return true;
            }
            return false;
        }


        public User GetUser(int userId)
        {
            return _unitOfWork.users.GetUser(userId);
        }

        public bool Update(User user)
        {
            if (_unitOfWork.users.UpdateUser(user))
            {
                _unitOfWork.Complete();
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}
