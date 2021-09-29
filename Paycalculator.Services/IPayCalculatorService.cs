using Microsoft.AspNetCore.Mvc.Rendering;
using Paycalculator.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Paycalculator.Services
{
    public interface IPayCalculatorService
    {
        Task CreateAsync(PaymentRecord paymentRecord);
        PaymentRecord GetById(int id);
        IEnumerable<PaymentRecord> GetAll();
        TaxYear GetTaxYearById(int id);
        IEnumerable<SelectListItem> GetAllTaxYear();
        decimal OvertimeHours(decimal hoursWorked, decimal contractualHours);
        decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate);
        decimal OvertimeRate(decimal hourlyRate);
        decimal OvertimeEarnings(decimal overtimeRate,decimal overtimeHours);
        decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings);
        decimal TotalReduction(decimal Tax,decimal nic,decimal studentLoanRepayment);
        decimal NetPay(decimal totalEarnings, decimal totalReduction);


    }
}
