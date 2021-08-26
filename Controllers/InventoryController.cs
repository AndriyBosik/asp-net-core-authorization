using System.Collections.Generic;
using System.Linq;
using AuthorizationExample.Data;
using AuthorizationExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationExample.Controllers
{
    public class InventoryController: Controller
    {
        private ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Authorize(Policy = "Ukraine")]
        [HttpGet]
        public IActionResult Show()
        {
            IList<Product> products = _context.Products.ToList();
            return View(products);
        }

        [Authorize(Policy = "England")]
        [HttpGet]
        public IActionResult Concrete(int id)
        {
            Product product = _context.Products.FirstOrDefault(product => product.Id == id);

            return View(product);
        }
    }
}