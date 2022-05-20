namespace OrderService.Models
{
    public class OrderData
    {
        public int Id { get; set; }
        public string Code { get; set; } 
        public int UserId { get; set; }
        public int CourierId { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        
    }
}
