namespace OrderService.Models
{
    public class OrderData
    {
        public int Id { get; set; }
        public string Code { get; set; } 
        public int UserId { get; set; }
        public int CourierId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
    }
}
