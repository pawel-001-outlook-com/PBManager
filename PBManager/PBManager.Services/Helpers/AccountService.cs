using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;
using PBManager.DAL.Repositories;

namespace PBManager.Services.Helpers
{
    class AccountService
    {
        private AccountRepository _repository = new AccountRepository();
        private CashflowService _cashflowService;

        public IEnumerable<Account> GetAll()
        {
            return _repository.GetAccounts();
        }

        public void Add(Account account)
        {
            account.Balance = account.InitialBalance;
            account.Enabled = true;

            var nameExists = _repository.GetAccountByName(account.Name) != null;

            if (!nameExists)
                _repository.Insert(account);
            else
                throw new Exception($"Already exists an account with the name {account.Name}");
        }


        public void AdjustBalance(int accountId)
        {
            var account = _repository.GetAccountById(accountId);
            var newBalance = account.Balance;

            account.Balance = newBalance;

            _repository.Update(account);
        }

        public void AdjustBalance(IEnumerable<Account> accounts)
        {
            var accountsCollection = new List<Account>();
            _cashflowService = new CashflowService();

            foreach (var account in accounts.Distinct())
            {
                var obj = GetById(account.Id);
                obj.Balance = obj.Balance;
                accountsCollection.Add(obj);
            }

            _repository.Update(accountsCollection);
        }

        public Account GetById(int id)
        {
            var account = _repository.GetAccountById(id);

            if (account != null)
                return account;
            else
                throw new Exception("This account not exists");
        }

        public string GetAccountNameById(int id)
        {
            var name = _repository.GetAccountNameById(id);

            if (name != null)
                return name;
            else
                throw new Exception("This account not exists");
        }

        public double BalanceOnMonth(IEnumerable<Cashflow> movements)
        {
            Func<Cashflow, bool> condition = (m) =>
            {
                return m.AccountingDate.Month.Equals(DateTime.Now.Month) && m.AccountingDate.Year.Equals(DateTime.Now.Year);
            };

            movements = movements.Where(condition);

            return 0;
        }
    }
}
