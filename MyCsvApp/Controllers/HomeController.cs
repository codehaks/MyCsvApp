using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCsvApp.Models;

namespace MyCsvApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    Name = "Joe",
                    Age = 25,
                    Country = "Germany"
                },
                new Customer
                {
                    Name = "Omid",
                    Age = 29,
                    Country = "Iran"
                },
                new Customer
                {
                    Name = "Bob",
                    Age = 33,
                    Country = "Denmark"
                },
            };
            return new CsvResult<Customer>(customers);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    class CsvResult<T> : IActionResult
    {
        private readonly IList<T> _list;
        public CsvResult(IList<T> list)
        {
            _list = list;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _list.Count; i++)
            {

                string[] item = GetObjectProperyValues(_list[i]).ToArray();

                for (int j = 0; j < item.Length; j++)
                {
                    sb.Append(item[j] + ',');
                }

                sb.Append("\r\n");

            }
            var objectResult = new ObjectResult(sb.ToString());
            await objectResult.ExecuteResultAsync(context);
        }

        public List<string> GetObjectProperyValues(object pObject)
        {
            List<string> propertyList = new List<string>();
            if (pObject == null)
            {
                return propertyList;
            }
            foreach (var prop in pObject.GetType().GetProperties())
            {
                propertyList.Add(prop.GetValue(pObject).ToString());
            }
            return propertyList;
        }
    }

    class Customer
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
    }
}
