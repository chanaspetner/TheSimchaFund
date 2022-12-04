using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TheSimchaFundWeb.Models;
using TheSimchaFundData;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TheSimchaFundWeb.Controllers
{
    public class SimchasController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=TheSimchaFund;Integrated Security=true;";

        public IActionResult Index()
        {
            TheSimchaFundManager mgr = new TheSimchaFundManager(_connectionString);
            var vm = new SimchaViewModel();

            string message = (string)TempData["Message"];
            if (!String.IsNullOrEmpty(message))
            {
                vm.Message = message;
            }

            vm.Simchas = mgr.GetSimchas();
            vm.PeopleCount = mgr.GetPeople().Count;

            return View(vm);
        }

        [HttpPost]
        public IActionResult New(Simcha simcha) 
        {
            var mgr = new TheSimchaFundManager(_connectionString);
            int id = mgr.AddSimcha(simcha);
            TempData["Message"] = $"New Simcha Created! Id: {id}";
            return Redirect("/simchas/index");
        }

        public IActionResult Contributions(int simchaId)
        {
            var mgr = new TheSimchaFundManager(_connectionString);
            return View(new ContributionsViewModel
            {
                People = mgr.GetPeople(),
                Simcha = mgr.GetSimcha(simchaId),
                Contributions = mgr.GetContributionsForSimcha(simchaId)
            }) ;
        }

        [HttpPost]
        public IActionResult UpdateContributions(List<Person> people, int simchaId)
        {
            var mgr = new TheSimchaFundManager(_connectionString);
            mgr.AddContributionsForSimcha(people, simchaId);
            TempData["Message"] = "Simcha updated successfully!";
            return Redirect($"/simchas/index");
        }

    }

}
