namespace OrderWorkflow.Models;

public class OrderLine
{
    public int OrderLineId { get; set; } // Cheia primară
    public int OrderId { get; set; } // Cheia externă către OrderHeader
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    // Relația cu entitatea Product
    public Product? Product { get; set; }

    // Relația cu entitatea OrderHeader
    public OrderHeader OrderHeader { get; set; } = null!;
}