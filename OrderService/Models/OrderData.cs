namespace OrderService.Models
{
    public class OrderData
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public int UserId { get; set; }
        public int? CourierId { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public bool Status { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
