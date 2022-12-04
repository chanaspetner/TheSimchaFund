using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSimchaFundData;

namespace TheSimchaFundWeb.Models
{
    public class ContributorsViewModel
    {
        public List<Person> People { get; set; }
        public decimal Total { get; set; }
        public string Message { get; set; } 
    }
}
