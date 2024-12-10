using Globomantics.Domain.Models;
using Globomatics.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Globomatics.Web.Components
{
    public class ProductListViewComponent : ViewComponent
    {
        private readonly IRepository<Product> productRepo;
        private readonly ILogger<ProductListViewComponent> logger;

        public ProductListViewComponent(IRepository<Product> productRepo, ILogger<ProductListViewComponent> logger)
        {
            this.productRepo = productRepo;
            this.logger = logger;
        }
        public Task<IViewComponentResult> InvokeAsync()
        {
            var products = productRepo.All();
            logger.LogInformation($"Loaded {products.Count()} products...");
            return Task.FromResult<IViewComponentResult>(View(products));
        }
    }
}
