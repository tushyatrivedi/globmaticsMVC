using System.ComponentModel.DataAnnotations;

namespace Globomantics.Domain.Models;

public class Product
{
    [Key]
    public Guid ProductId { get; set; }

    public required string Name { get; set; }

    public required decimal Price { get; set; }

    public Product()
    {
        ProductId = Guid.NewGuid();
    }
}