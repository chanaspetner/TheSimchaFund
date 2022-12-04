using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSimchaFundData;

namespace TheSimchaFundWeb.Models
{
    public class HistoryViewModel
    {
        public List<MoneyAction> Actions { get; set; }
        public Person Person { get; set; }
    }
}
