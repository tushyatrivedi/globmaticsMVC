using Globomantics.Domain.Models;
using Globomatics.Infrastructure.Repositories;
using Globomatics.Web.Attributes;
using Globomatics.Web.Filters;
using Globomatics.Web.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Globomatics.Web.Controllers;

public class HomeController : Controller
{
    public IRepository<Product> repos { get; }

    public ILogger<HomeController> logger { get; set; }

    public HomeController(IRepository<Product> productRepo, ILogger<HomeController> logger)
    {
        repos = productRepo;
        this.logger = logger;
    }

    //[TimerFilter]
    [ServiceFilter<TimerFilter>]
    public IActionResult Index()
    {
        return View();
    }

    [Route("/details/{productId:guid}/{slug:slugTransform}")]
    public IActionResult TicketDetails(Guid productId,
        [RegularExpression("^[a-zA-Z0-9- ]+$")] string slug)
    
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var product = repos.Get(productId);
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