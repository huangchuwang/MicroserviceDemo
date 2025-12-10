namespace ProductService.DTOs
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        // 如果需要展示库存，也可以加 public int Stock { get; set; }
    }
}
