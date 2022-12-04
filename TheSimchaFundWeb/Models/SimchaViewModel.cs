using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSimchaFundData;

namespace TheSimchaFundWeb.Models
{
    public class SimchaViewModel
    {
        public List<Simcha> Simchas { get; set; }
        public int PeopleCount { get; set; }

        public string Message { get; set; }
    }
}
