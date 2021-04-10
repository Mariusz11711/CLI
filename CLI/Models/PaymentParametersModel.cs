using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CLI.Models
{
    public class PaymentParametersModel
    {
        //TODO: tests regular expresion
        [Required(ErrorMessage = "Please put 'Loan amount'")]
        [RegularExpression("^((?:[1-9][0-9]*)(?:\\.[0-9]+)?)$",
             ErrorMessage = "The value of 'Loan amount' can't be '0'")]
        public double? Loan_amount { get; set; }
        [Required(ErrorMessage = "Please add 'Duration of loan'")]
        public int? Duration_of_loan { get; set; }

        public bool Annual_Interest_rate { get; set; }
        public bool Interest_rate_calculated_monthly { get; set; }
        public bool Administration_fee { get; set; }
    }
}
