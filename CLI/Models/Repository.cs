using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLI.Models
{
    public static class Repository
    {
        private static List<LoanRepositorium> repository = new List<LoanRepositorium>();
        public static IEnumerable<LoanRepositorium> Repos => repository;

        public static void AddRepository(PaymentParametersModel parameters, double paid_administrative_fees, string monthly_cost, string paid_interest_rate, string aop, double annual_Interest_cost)
        {
            LoanRepositorium repo = new LoanRepositorium();
            repo.Interest_rate_calculated_monthly = parameters.Interest_rate_calculated_monthly;
            repo.Loan_amount = parameters.Loan_amount;
            repo.Duration_of_loan = parameters.Duration_of_loan;
            repo.Administration_fee = parameters.Administration_fee;
            repo.Annual_Interest_cost = annual_Interest_cost;
            repo.Paid_administrative_fees = paid_administrative_fees;
            repo.Monthly_cost = monthly_cost;
            repo.Paid_interest_rate = paid_interest_rate;
            repo.AOP = aop;

            repository.Add(repo);
        }
    }
}
