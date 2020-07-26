using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.DAL.Contracts
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAccounts();
        IEnumerable<Account> GetAccounts(int userId);
        Account GetAccountById(int id);
        string GetAccountNameById(int id);
        ICollection<Account> GetAccountsByName(string name);
        Account GetAccountByName(string name);
        void Insert(Account account);
        void Update(Account account);


        void Add(Account account);
        void Delete(Account account);

    }
}