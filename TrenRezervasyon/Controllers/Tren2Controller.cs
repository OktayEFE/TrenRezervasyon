using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrenRezervasyon.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tren2Rezervasyon.Controllers
{
	public class Tren2Controller : Controller
	{
		// GET: /<controller>/
		public IActionResult Index()
		{
			return View();
		}

	}
}

