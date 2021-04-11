using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLI.Models
{
    public static class Repository
    {
        private static List<PaymentParametersModel> responses = new List<PaymentParametersModel>();

        public static IEnumerable<PaymentParametersModel> Responses
        {
            get
            {
                return responses;
            }
        }

        public static void AddResponse(PaymentParametersModel response)
        {
            responses.Add(response);
        }
    }
}
