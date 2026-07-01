namespace api.Dtos;

public class CreateOrderRequest
{
    public int MemberId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public string ShippingAddress { get; set; } = null!;
}
