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

    public class Cart
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = default!;
        public List<CartItem> Items { get; set; } = new();
        public string Status { get; set; } = "Активний"; 
        public DateTime? ClosedDate { get; set; }
    }

}
