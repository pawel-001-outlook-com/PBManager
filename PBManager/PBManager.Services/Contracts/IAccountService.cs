﻿using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.Services.Contracts
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAll();
        IEnumerable<Account> GetByUser(int userId);
        IEnumerable<Account> GetAll(int userId);
        void Add(Account account);
        Account GetById(int id);
        string GetAccountNameById(int id);
        void Remove(int id);
        void Update(Account account);
        int GetTotalCount();
        int GetFilteredCount(string searchValue);

        List<Account> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length);
    }
}