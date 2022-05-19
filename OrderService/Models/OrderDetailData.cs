namespace OrderService.Models
{
    public class OrderDetailData
    {
        public int Id { get; set; }
        public int MealId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
    }
}
