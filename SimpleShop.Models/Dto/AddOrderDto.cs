namespace SimpleShop.Models.Dto;

public class AddOrderDto
{
    public string UserId { get; set; }
    public List<Product> Products { get; set; }
}