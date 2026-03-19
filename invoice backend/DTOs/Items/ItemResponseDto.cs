namespace invoice_backend.DTOs.Items;

public class ItemResponseDto
{
    public int Id { set; get; }

    public required string Name { set; get; }

    public decimal Amount { set; get; } = 0;

    public int Quantity { set; get; } = 1;
}