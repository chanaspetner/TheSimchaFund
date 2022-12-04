using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSimchaFundData;
using TheSimchaFundWeb.Models;

namespace TheSimchaFundWeb.Controllers
{
    public class ContributorController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=TheSimchaFund;Integrated Security=true;";
        public IActionResult Index()
        {
            var mgr = new TheSimchaFundManager(_connectionString);

            var vm = new ContributorsViewModel();

            string message = (string)TempData["Message"];
            if (!String.IsNullOrEmpty(message))
            {
                vm.Message = message;
            }
            vm.People = mgr.GetPeople();
            vm.Total = mgr.GetTotalBalances();
            return View(vm);
        }

        [HttpPost]
        public IActionResult New(Person person)
        {
            var mgr = new TheSimchaFundManager(_connectionString);
            int id = mgr.AddPerson(person);
            TempData["Message"] = $"New Contributor Created! Id: {id}";
            return Redirect("/contributor/index");
        }

        [HttpPost]
        public IActionResult Edit(Person person)
        {
            var mgr = new TheSimchaFundManager(_connectionString);
            mgr.EditPerson(person);
            TempData["Message"] = $"Contributor updated successfully!";
            return Redirect("/contributor/index");
        }

        [HttpPost]
        public IActionResult Deposit(Deposit deposit)
        {
            var mgr = new TheSimchaFundManager(_connectionString);
            mgr.AddDeposit(deposit);
            return Redirect("/contributor/index");
        }

        public IActionResult History(int id)
        {
            var mgr = new TheSimchaFundManager(_connectionString);

            return View(new HistoryViewModel 
            {
                Actions = mgr.GetHistory(id),
                Person = mgr.GetPerson(id)
            });
        }
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
