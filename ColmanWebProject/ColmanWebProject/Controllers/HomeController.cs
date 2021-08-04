using ColmanWebProject.Data;
using ColmanWebProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanWebProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ColmanWebProjectContext _context;

        public HomeController(ColmanWebProjectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> About()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.Where(c => c.SubType.Equals("General")).ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
