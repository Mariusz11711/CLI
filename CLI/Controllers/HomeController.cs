using CLI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CLI.Controllers
{
    public class HomeController : Controller
    {        
        private double Annual_Interest_rate { get; set; }
        private bool Interest_rate_calculated_monthly { get; set; }
        private double Paid_administrative_fees { get; set; }
        private double Loan_amount { get; set; }
        private int Duration_of_loan { get; set; }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PaymentParametersModel parameters)
        {
            if (ModelState.IsValid)
            {
                Annual_Interest_rate = parameters.Annual_Interest_rate == true ? 0.05 : 0.02;
                Paid_administrative_fees = parameters.Administration_fee == true ? Calculation_administrative_fees() : 0.0;
                Interest_rate_calculated_monthly = parameters.Interest_rate_calculated_monthly;
                Loan_amount = parameters.Loan_amount ?? 0.0;
                Duration_of_loan = parameters.Duration_of_loan ?? 0;


                ViewBag.annual_Interest_rate = Annual_Interest_rate;
                ViewBag.interest_rate_calculated_monthly = Interest_rate_calculated_monthly;
                ViewBag.paid_administrative_fees = Paid_administrative_fees;
                ViewBag.duration_of_loan = Duration_of_loan;
                ViewBag.loan_amount = Loan_amount;

                ViewBag.monthly_cost = Calculation_monthly_cost_amound();
                ViewBag.paid_interest_rate = Calculation_interest_rate();
                ViewBag.aop = Calculation_AOP();
                                
                //TODO: exceeding the value of double               
                return View("LoanInformations");
            } 
            else
            {
                return View();
            }
        }

        #region main_method
        private double Calculation_AOP()
        {
            double aop = 0.0;
            double interest_value = 0.0;
            var loan_value = Loan_amount + Paid_administrative_fees;
            var monthly_count = Duration_of_loan * 12;

            if (Interest_rate_calculated_monthly)
            {
                var rate_value = loan_value / monthly_count;

                for (int counter = 1; monthly_count >= counter; counter++)
                {
                    interest_value += loan_value * (1 / 12) + loan_value * (1 / 12) * Annual_Interest_rate;
                    loan_value -= rate_value;
                }

                aop = Annual_Interest_rate * interest_value / Loan_amount;
            }
            else
            {
                for (int counter = 1; monthly_count >= counter; counter++)
                {
                    interest_value += 1 / Math.Pow(1 + Annual_Interest_rate / 12, counter);
                }

                interest_value = (loan_value / interest_value) * monthly_count;
                aop = Annual_Interest_rate * interest_value / Loan_amount;
            }

            return aop;
        }
        private double Calculation_interest_rate()
        {   

            var monthly_count = Duration_of_loan * 12;
            double interest_value = 0.0;
            var loan_value = Loan_amount + Paid_administrative_fees;

            if (Interest_rate_calculated_monthly)
            {                
                var rate_value = loan_value / monthly_count;              

                for (int counter = 1; monthly_count >= counter; counter++)
                {
                    interest_value += loan_value * (1 / 12) * Annual_Interest_rate;
                    loan_value -= rate_value;
                }
            }
            else
            {
                for (int counter = 1; monthly_count >= counter; counter++)
                {
                    interest_value += 1 / Math.Pow(1 + Annual_Interest_rate / 12, counter);
                }

                interest_value = (loan_value / interest_value) * monthly_count - loan_value;
            }

            return interest_value;
        }
        private double Calculation_monthly_cost_amound()
        {           //TODO: make test when Loan_amount is 0
            return Loan_amount * Annual_Interest_rate * 31 / 365;
        }
        private double Calculation_administrative_fees()
        {
            //TODO: make test when Loan_amount is 0
            return (Loan_amount / 10000) <= 10000 ? (Loan_amount / 10000) : 10000;
        }
        #endregion
    }
}
