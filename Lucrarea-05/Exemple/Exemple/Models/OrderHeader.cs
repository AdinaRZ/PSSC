using System.Collections.Generic;

namespace OrderWorkflow.Models;

public class OrderHeader
{
    public int OrderId { get; set; } // Cheia primară
    public string Address { get; set; } = string.Empty; // Inițializare pentru a evita null
    public decimal Total { get; set; }

    // Colecție de linii de comandă asociate cu această comandă
    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>(); // Inițializare implicită
}