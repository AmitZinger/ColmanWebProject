using ColmanWebProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanWebProject.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly ColmanWebProjectContext _context;

        public NavigationMenuViewComponent(ColmanWebProjectContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("NavigationMenu", await _context.Category.ToListAsync());
        }

        public async Task<IViewComponentResult> GetCustomerId(string email)
        {
            var customerId = from c in _context.Customer
                                where c.Email == email
                                select c.Id;
            return View("Edit/"+customerId.First(), "Customer");
        }
    }
 }
