﻿using Globomantics.Domain.Models;
using Globomatics.Infrastructure.Repositories;
using Globomatics.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Globomatics.Web.Controllers;

[Route("[controller]")]
public class CartController : Controller
{
    private readonly ILogger<CartController> logger;
    private readonly ICartRepository cartRepository;
    private readonly IRepository<Customer> customerRepository;
    private readonly IRepository<Order> orderRepository;
    private readonly IStateRepository stateRepository;

    public CartController(ILogger<CartController> logger, ICartRepository cartRepository, IRepository<Customer> customerRepository, IRepository<Order> orderRepository,IStateRepository stateRepository
        )
    {
        this.logger = logger;
        this.cartRepository = cartRepository;
        this.customerRepository = customerRepository;
        this.orderRepository = orderRepository;
        this.stateRepository = stateRepository;
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

        logger.LogInformation($"Attempting to add {addToCartModel.Product.ProductId} to cart {addToCartModel.CartId}");

        var cart = cartRepository.CreateOrUpdate(addToCartModel.CartId, addToCartModel.Product.ProductId);

        //store the cart for further use
        stateRepository.SetValue("NumberOfItems", cart.LineItems.Sum(item=>item.Quantity).ToString());
        stateRepository.SetValue("CartId",cart.CartId.ToString());  

        cartRepository.SaveChanges();

        return RedirectToAction("Index", "Cart");
    }

    [HttpPost]
    [Route("Update")]
    [ValidateAntiForgeryToken]
    public IActionResult Update(UpdateQuantitiesModel updateQuantitiesModel)
    {
        var item = HttpContext.Request.Form;
        if (updateQuantitiesModel.Products is null)
        {
            return BadRequest();
        }

        Cart cart = null!;

        foreach (var product in updateQuantitiesModel.Products)
        {
            logger.LogInformation($"adding {product.ProductId} to cart {updateQuantitiesModel.CartId}");
            cart = cartRepository.CreateOrUpdate(updateQuantitiesModel.CartId, product.ProductId, product.Quantity);
        }
        //store the cart for further use
        stateRepository.SetValue("NumberOfItems", cart.LineItems.Sum(item => item.Quantity).ToString());
        stateRepository.SetValue("CartId", cart.CartId.ToString());

        cartRepository.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("Finalize")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateOrderModel createOrderModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        if (createOrderModel.Customer == null)
        {
            ModelState.AddModelError("Customer", "customer data is not present");
            return View("Index");
        }

        if (createOrderModel.Customer.Name.Length <= 2)
        {
            ModelState.AddModelError(nameof(createOrderModel.Customer.Name), "Name is too short");
            return View("Index");
        }
        //adding customer
        //attaching customer to order
        //add order for the customer by fetching cart details
        //saving to DB

        Customer customer = new Customer
        {
            Email = createOrderModel.Customer.Email,
            Name = createOrderModel.Customer.Name,
            City = createOrderModel.Customer.City,
            Country = createOrderModel.Customer.Country,
            PostalCode = createOrderModel.Customer.PostalCode,
            ShippingAddress = createOrderModel.Customer.ShippingAddress,
        };

        logger.LogInformation($"creating a new order for customer {createOrderModel.Customer.Name}");

        customerRepository.Add(customer);

        var order = new Order
        {
            CustomerId = customer.CustomerId
        };

        //need to check cart associated with user's session 
        if (createOrderModel.CartId == null || createOrderModel.CartId == Guid.Empty)
        {
            ModelState.AddModelError("Cart", "Cart has been deleted");
            return View("Index");
        }

        var cart = cartRepository.Get(createOrderModel.CartId.Value);

        if (cart == null)
        {
            ModelState.AddModelError("Cart", "Cart has been deleted");
            return View("Index");
        }

        foreach (var item in cart.LineItems)
        {
            order.LineItems.Add(item);
        }

        orderRepository.Add(order);
        cartRepository.Update(cart);
        cartRepository.SaveChanges();

        stateRepository.Remove("NumberOfItems");
        stateRepository.Remove("CartId");

        logger.LogInformation($"Order placed for Customer {customer.Name}");   

        return RedirectToAction("ThankYou");
    }

    [HttpGet("ThankYou")]
    public IActionResult ThankYou()
    {
        return View();
    }
}
