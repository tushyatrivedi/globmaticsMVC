using Globomantics.Domain.Models;
using Globomatics.Infrastructure.Repositories;
using Globomatics.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Globomatics.Web.Controllers;

[Route("[controller]")]
public class CartController : Controller
{
    private readonly ILogger<CartController> logger;
    private readonly ICartRepository cartRepository;

    public CartController(ILogger<CartController> logger, ICartRepository cartRepository)
    {
        this.logger = logger;
        this.cartRepository = cartRepository;
    }

    [Route("Index")]
    public IActionResult Index(Guid? id)
    {
        return View();
    }

    [HttpPost]
    [Route("Add")]
    public IActionResult AddToCart(AddToCartModel addToCartModel)
    {
        if (addToCartModel.Product is null)
        {
            return BadRequest();
        }

        logger.LogInformation($"Attempting to add {addToCartModel.Product} to cart {addToCartModel.CartId}");

        var cart = cartRepository.CreateOrUpdate(addToCartModel.CartId, addToCartModel.Product.ProductId);

        cartRepository.SaveChanges();

        return RedirectToAction("Index", "Cart");
    }

    [HttpPost]
    public IActionResult Update(UpdateQuantitiesModel updateQuantitiesModel)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("Finalize")]
    public IActionResult Create(CreateOrderModel createOrderModel)
    {
        throw new NotImplementedException();
    }

    [HttpGet("ThankYou")]
    public IActionResult ThankYou()
    {
        return View();
    }
}
