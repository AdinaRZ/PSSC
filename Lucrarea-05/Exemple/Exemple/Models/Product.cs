namespace OrderWorkflow.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Code { get; set; } = string.Empty; // Inițializare pentru a evita null
    public int Stoc { get; set; }
}