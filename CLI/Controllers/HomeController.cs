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
        private double loan_amount = 0.00;     
        private double Annual_Interest_rate { get; set; }
        private bool Interest_rate_calculated_monthly { get; set; }
        private double Paid_administrative_fees { get; set; }     
        private int Duration_of_loan_in_months { get; set; }


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
                loan_amount = parameters.Loan_amount ?? 0.00;
                Paid_administrative_fees = parameters.Administration_fee == true ? Calculation_administrative_fees() : 0.0;
                Interest_rate_calculated_monthly = parameters.Interest_rate_calculated_monthly;
                Duration_of_loan_in_months = parameters.Duration_of_loan * 12 ?? 0;

                ViewBag.paid_administrative_fees = Paid_administrative_fees;
                ViewBag.monthly_cost = Calculation_monthly_cost_amound().ToString("N2");
                ViewBag.paid_interest_rate = Calculation_interest_rate().ToString("N2");
                ViewBag.aop = Calculation_AOP().ToString("N2");
                                
                //TODO: exceeding the value of double               
                return View("LoanInformations", parameters);
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
            double rate_sum = 0.0;
            var loan_value = loan_amount + Paid_administrative_fees;

            if (Interest_rate_calculated_monthly)
            {
                var rate_value = loan_value / Duration_of_loan_in_months;
                var loan_parameter = loan_value / 12;

                for (int counter = 1; Duration_of_loan_in_months >= counter; counter++)
                {
                    rate_sum += loan_parameter + loan_parameter * Annual_Interest_rate;
                    loan_value -= rate_value;
                }

                aop = 100 * (rate_sum - loan_amount) / loan_amount;
            }
            else
            {
                for (int counter = 1; Duration_of_loan_in_months >= counter; counter++)
                {
                    rate_sum += 1 / Math.Pow(1 + Annual_Interest_rate / 12, counter);
                }
                //TODO: To small 
                rate_sum = (loan_value / rate_sum) * Duration_of_loan_in_months;
                aop = 100 * (rate_sum - loan_amount) / loan_amount;
            }

            return aop;
        }
        private double Calculation_interest_rate()
        {   

            double interest_value = 0.0;
            var loan_value = loan_amount + Paid_administrative_fees;

            if (Interest_rate_calculated_monthly)
            {                
                var rate_value = loan_value / Duration_of_loan_in_months;
              
                for (int counter = 1; Duration_of_loan_in_months >= counter; counter++)
                {
                    interest_value += loan_value / 12 * Annual_Interest_rate;
                    loan_value -= rate_value;
                }
            }
            else
            {
                for (int counter = 1; Duration_of_loan_in_months >= counter; counter++)
                {
                    interest_value += 1 / Math.Pow(1 + Annual_Interest_rate / 12, counter);
                }

                interest_value = (loan_value / interest_value) * Duration_of_loan_in_months - loan_value;
            }

            return interest_value;
        }
        private double Calculation_monthly_cost_amound()
        {           //TODO: make test when Loan_amount is 0
            var loan_value = loan_amount + Paid_administrative_fees;

            if (Interest_rate_calculated_monthly)
            {
                return loan_value / 12 + loan_value / 12 * Annual_Interest_rate;
            }
            else
            {
                double interest_value = 0.0;

                for (int counter = 1; Duration_of_loan_in_months >= counter; counter++)
                {
                    interest_value += 1 / Math.Pow(1 + Annual_Interest_rate / 12, counter);
                }

               return loan_value / interest_value;
            }
        }
        private double Calculation_administrative_fees()
        {
            //TODO: make test when Loan_amount is 0
            return (loan_amount / 10000) <= 10000 ? (loan_amount / 10000) : 10000;
        }
        #endregion
    }
}
