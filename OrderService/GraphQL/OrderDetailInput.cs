namespace OrderService.GraphQL
{
    public record OrderDetailInput
        (
            int? Id,
            int MealId,
            int OrderId,
            int Quantity
        );
    
}
