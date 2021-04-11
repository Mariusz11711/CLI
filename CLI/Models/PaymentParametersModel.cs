using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CLI.Models
{
    public class PaymentParametersModel
    {

        [Required(ErrorMessage = "Please put 'Loan amount'")]
        [RegularExpression("^((?:[1-9][0-9]*)(?:\\.[0-9]+)?)$",
             ErrorMessage = "The value of 'Loan amount' can't be '0'")]
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        public double? Loan_amount { get; set; }
        [Required(ErrorMessage = "Please add 'Duration of loan'")]
        public int? Duration_of_loan { get; set; }
        public bool Annual_Interest_rate { get; set; }
        public bool Interest_rate_calculated_monthly { get; set; }
        public bool Administration_fee { get; set; }
    }

    public class LoanRepositorium : PaymentParametersModel
    {
        public double Paid_administrative_fees { get; set; }
        public double Annual_Interest_cost { get; set; }
        public string Monthly_cost { get; set; }
        public string Paid_interest_rate { get; set; }
        public string AOP { get; set; }
    }
}
