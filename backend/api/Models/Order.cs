namespace api.Models;

public class Order
{
    public int OrderId { get; set; }
    public int MemberId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public string ShippingAddress { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime OrderDate { get; set; }

    public Member Member { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
