using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSimchaFundData;

namespace TheSimchaFundWeb.Models
{
    public class ContributionsViewModel
    {
        public List<Person> People { get; set; }
        public Simcha Simcha { get; set; }
        public List<Contribution> Contributions { get; set; }

    }
}
