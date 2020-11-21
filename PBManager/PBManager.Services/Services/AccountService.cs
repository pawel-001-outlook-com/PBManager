using System;
using System.Collections.Generic;
using System.Linq;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;
using Unity.Attributes;

namespace PBManager.Services.Helpers
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService()
        {
        }

        [InjectionConstructor]
        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<Account> GetByUser(int userId)
        {
            return _unitOfWork.accounts.GetAccounts(userId);
        }


        public void Add(Account account)
        {
            var n1 = _unitOfWork.accounts
                .GetAccounts()
                .Where(a => a.Name.Equals(account.Name))
                .FirstOrDefault();

            var nameExists = n1 != null;

            if (!nameExists)
            {
                _unitOfWork.accounts.Add(account);
                _unitOfWork.Complete();
            }

            else
            {
                throw new Exception($"account name already in use: {account.Name}");
            }
        }


        public Account GetById(int id)
        {
            var account = _unitOfWork.accounts.GetAccountById(id);

            if (account != null)
                return account;
            throw new Exception("!!! account does not exist !!!");
        }


        public void Remove(int id)
        {
            var account = GetById(id);
            _unitOfWork.accounts.Delete(account);
            _unitOfWork.Complete();
        }


        public void Update(Account account)
        {
            var currentAccount = GetById(account.Id);
            var quantity =
                _unitOfWork.accounts.GetAccountsByName(account.Name).Count(a => !a.Id.Equals(currentAccount.Id));

            if (quantity.Equals(0))
            {
                _unitOfWork.accounts.Update(account);
                _unitOfWork.Complete();
            }
            else
            {
                throw new AlreadyExistsException($"Already exists an account with the name {account.Name}");
            }
        }


        public int GetTotalCount()
        {
            return _unitOfWork.accounts.GetTotalCount();
        }


        public int GetFilteredCount(string searchValue)
        {
            return _unitOfWork.accounts.GetFilteredCount(searchValue);
        }


        public List<Account> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length)
        {
            var a = _unitOfWork.accounts.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start,
                length);
            return a;
        }
    }
}