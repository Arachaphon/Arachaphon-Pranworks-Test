namespace api.Dtos;

public class CreateOrderResponse
{
    public int OrderId { get; set; }
    public int MemberId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string ShippingAddress { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime OrderDate { get; set; }
}
