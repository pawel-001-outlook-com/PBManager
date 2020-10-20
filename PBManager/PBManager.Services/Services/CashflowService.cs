using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;
using PBManager.DAL;
using PBManager.DAL.Contracts;
using PBManager.DAL.Repositories;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;
using Unity.Attributes;

namespace PBManager.Services.Helpers
{
    public class CashflowService : ICashflowService
    {
        // public CashflowService(){}

        // [InjectionConstructor]
        // public CashflowService(IAccountService accountService, ICashflowRepository cashflowRepository)
        // {
        //     _accountService = accountService;
        //     _cashflowRepository = cashflowRepository;
        // }


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


        public IEnumerable<Cashflow> GetAll()
        {
            return _unitOfWork.cashflows.GetAllCashflows();
        }


        public IEnumerable<Cashflow> GetAll(int userId, string userName)
        {
            var t = _unitOfWork.cashflows.GetCashflowsByUser(userId);
            return t;
        }


        public IEnumerable<Cashflow> GetAll(int accountId)
        {
            return _unitOfWork.cashflows.GetCashflowsByAccount(accountId);
        }


        public Cashflow GetById(int cashflowId)
        {
            var cashFlow = _unitOfWork.cashflows.GetCashflowById(cashflowId);
            if (cashFlow != null)
            {
                return cashFlow;
            }
            else
            {
                throw new NotFoundException("error: cashflow does not exist");
            }

        }


        public IEnumerable<Cashflow> GetAll(ReportViewModel reportViewModel)
        {
            return _unitOfWork.cashflows.GetAllCashflows(reportViewModel);
        }


        public IEnumerable<Cashflow> GetAll(AccountChartReportViewModel reportViewModel)
        {
            return _unitOfWork.cashflows.GetAllCashflows((AccountChartReportViewModel) reportViewModel);
        }



    }
}
