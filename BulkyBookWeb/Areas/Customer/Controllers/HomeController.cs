using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyBookWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController(ILogger<HomeController> logger, IUnitOfWork unit) : Controller
{
    public IActionResult Index()
    {
        var products = unit.Product.GetAll(includeProperties: "Category");
        return View(products);
    }

    public IActionResult Details(int? productId)
    {
        var product = unit.Product.Get(u => u.Id == productId, includeProperties: "Category");
        return View(product);
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
