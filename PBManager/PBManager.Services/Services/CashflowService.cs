using System.Collections.Generic;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;
using Unity.Attributes;

namespace PBManager.Services.Helpers
{
    public class CashflowService : ICashflowService
    {
        private readonly IUnitOfWork _unitOfWork;

        [InjectionConstructor]
        public CashflowService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void Add(Cashflow cashFlow)
        {
            _unitOfWork.cashflows.Add(cashFlow);
            _unitOfWork.Complete();
        }


        public void Update(Cashflow cashFlow)
        {
            _unitOfWork.cashflows.Update(cashFlow);
            _unitOfWork.Complete();
        }


        public void Delete(int cashflowId)
        {
            var cashFlow = GetById(cashflowId);
            _unitOfWork.cashflows.Delete(cashFlow);
            _unitOfWork.Complete();
        }


        public IEnumerable<Cashflow> GetAll(int userId, string userName)
        {
            var t = _unitOfWork.cashflows.GetCashflowsByUser(userId);
            return t;
        }


        public Cashflow GetById(int cashflowId)
        {
            var cashFlow = _unitOfWork.cashflows.GetCashflowById(cashflowId);
            if (cashFlow != null)
                return cashFlow;
            throw new NotFoundException("error: cashflow does not exist");
        }


        public int GetTotalCount()
        {
            return _unitOfWork.cashflows.GetTotalCount();
        }


        public int GetFilteredCount(string searchValue)
        {
            return _unitOfWork.cashflows.GetFilteredCount(searchValue);
        }
    }
}