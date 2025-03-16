namespace OnlineStore.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string UserId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
