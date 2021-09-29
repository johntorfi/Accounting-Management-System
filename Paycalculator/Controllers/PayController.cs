using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paycalculator.Entity;
using Paycalculator.Models;
using Paycalculator.Services;
using RotativaCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paycalculator.Controllers
{
    [Authorize(Roles ="Admin,Manager")]
    public class PayController : Controller
    {
        private readonly IPayCalculatorService _payCalculatorService;
        private readonly IEmployeeService _employeeService;
        private readonly ITaxService _taxService;
        private readonly INationalInsuranceContributionService _nationalInsuranceContributionService;
        private decimal overTimeHrs;
        private decimal contractualEarnings;
        private decimal overtimeEarnings;
        private decimal totalEarnings;
        private decimal tax;
        private decimal StudentLoanRepayment;
        private decimal nationalInsurance;
        private decimal totalReduction;
        private object overtimeHrs;

        public PayController(IPayCalculatorService PayCalculatorService, IEmployeeService employeeService, ITaxService taxService, INationalInsuranceContributionService nationalInsuranceContributionService)
        {
            _payCalculatorService = PayCalculatorService;
            _employeeService = employeeService;
            _taxService = taxService;
            _nationalInsuranceContributionService = nationalInsuranceContributionService;
        }
        public IActionResult Index()
        {
            var payRecords = _payCalculatorService.GetAll().Select(pay => new PaymentRecordIndexViewModel
            {
                Id = pay.Id,
                EmployeeId = pay.EmployeeId,
                FullName = pay.FullName,
                PayDate = pay.PayDate,
                PayMonth = pay.PayMonth,
                TaxYearId = pay.TaxYearId,
                Year = _payCalculatorService.GetTaxYearById(pay.TaxYearId).YearOfTax,
                TotalEarnings = pay.TotalEarnings,
                TotalReduction = pay.TotalReduction,
                NetPayment = pay.NetPayment,
                Employee = pay.Employee


            }
                );
            return View(payRecords);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Employees = _employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYear = _payCalculatorService.GetAllTaxYear();
            var model = new PaymentRecordCreateViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PaymentRecordCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var payrecord = new PaymentRecord()
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    FullName = _employeeService.GetById(model.EmployeeId).FullName,
                    NiNo = _employeeService.GetById(model.EmployeeId).NationalInsuranceNumber,
                    PayDate = model.PayDate,
                    PayMonth = model.PayMonth,
                    TaxYearId = model.TaxYearId,
                    TaxCode = model.TaxCode,
                    HourlyRate = model.HourlyRate,
                    HoursWorked = model.HoursWorked,
                    ContractualHours = model.ContractualHours,
                    OvertimeHours =overTimeHrs= _payCalculatorService.OvertimeHours(model.HoursWorked, model.ContractualHours),
                    ContractualEarnings = contractualEarnings = _payCalculatorService.ContractualEarnings(model.ContractualHours, model.HoursWorked, model.HourlyRate),
                    OvertimeEarnings = overtimeEarnings = _payCalculatorService.OvertimeEarnings(_payCalculatorService.OvertimeRate(model.HourlyRate), overTimeHrs),
                    TotalEarnings =totalEarnings= _payCalculatorService.TotalEarnings(overtimeEarnings, contractualEarnings),
                    Tax =tax= _taxService.TaxAmount(totalEarnings),
                    SLC = _employeeService.StudentLoanRepaymentAmount(model.EmployeeId, totalEarnings),
                    NIC = nationalInsurance = _nationalInsuranceContributionService.NIContribution(totalEarnings),
                    TotalReduction =totalReduction= _payCalculatorService.TotalReduction(tax, nationalInsurance, StudentLoanRepayment),
                    NetPayment = _payCalculatorService.NetPay(totalEarnings, totalReduction)
                };
                await _payCalculatorService.CreateAsync(payrecord);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.employees = _employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears = _payCalculatorService.GetAllTaxYear();
            return View();
        }
        public IActionResult Detail(int id)
        {
            var paymentRecord = _payCalculatorService.GetById(id);
            if (paymentRecord == null)
            {
                return NotFound();
            }

            var model = new PaymentRecordDetailViewModel()
            {
                Id = paymentRecord.Id,
                EmployeeId = paymentRecord.EmployeeId,
                FullName = paymentRecord.FullName,
                NiNo = paymentRecord.NiNo,
                PayDate = paymentRecord.PayDate,
                PayMonth = paymentRecord.PayMonth,
                TaxYearId = paymentRecord.TaxYearId,
                Year = _payCalculatorService.GetTaxYearById(paymentRecord.TaxYearId).YearOfTax,
                TaxCode = paymentRecord.TaxCode,
                HourlyRate = paymentRecord.HourlyRate,
                HoursWorked = paymentRecord.HoursWorked,
                ContractualHours = paymentRecord.ContractualHours,
                OvertimeHours = paymentRecord.OvertimeHours,
                OvertimeRate = _payCalculatorService.OvertimeRate(paymentRecord.HourlyRate),
                ContractualEarnings = paymentRecord.ContractualEarnings,
                OvertimeEarnings = paymentRecord.OvertimeEarnings,
                Tax = paymentRecord.Tax,
                NIC = paymentRecord.NIC,
                SLC = paymentRecord.SLC,
                TotalEarnings = paymentRecord.TotalEarnings,
                TotalReduction = paymentRecord.TotalReduction,
                Employee = paymentRecord.Employee,
                TaxYear = paymentRecord.TaxYear,
                NetPayment = paymentRecord.NetPayment
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Payslip(int id)
        {
            var paymentRecord = _payCalculatorService.GetById(id);
            if (paymentRecord == null)
            {
                return NotFound();
            }

            var model = new PaymentRecordDetailViewModel()
            {
                Id = paymentRecord.Id,
                EmployeeId = paymentRecord.EmployeeId,
                FullName = paymentRecord.FullName,
                NiNo = paymentRecord.NiNo,
                PayDate = paymentRecord.PayDate,
                PayMonth = paymentRecord.PayMonth,
                TaxYearId = paymentRecord.TaxYearId,
                Year = _payCalculatorService.GetTaxYearById(paymentRecord.TaxYearId).YearOfTax,
                TaxCode = paymentRecord.TaxCode,
                HourlyRate = paymentRecord.HourlyRate,
                HoursWorked = paymentRecord.HoursWorked,
                ContractualHours = paymentRecord.ContractualHours,
                OvertimeHours = paymentRecord.OvertimeHours,
                OvertimeRate = _payCalculatorService.OvertimeRate(paymentRecord.HourlyRate),
                ContractualEarnings = paymentRecord.ContractualEarnings,
                OvertimeEarnings = paymentRecord.OvertimeEarnings,
                Tax = paymentRecord.Tax,
                NIC = paymentRecord.NIC,
                SLC = paymentRecord.SLC,
                TotalEarnings = paymentRecord.TotalEarnings,
                TotalReduction = paymentRecord.TotalReduction,
                Employee = paymentRecord.Employee,
                TaxYear = paymentRecord.TaxYear,
                NetPayment = paymentRecord.NetPayment
            };
            return View(model);
        }

        public IActionResult GeneratePayslipPdf(int id)
        {
            var payslip = new ActionAsPdf("Payslip", new { id = id })
            {
                FileName="payslip.pdf"

            };

            return payslip;
                
        }
    }
  
    
    }

