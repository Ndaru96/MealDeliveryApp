namespace OrderService.GraphQL
{
    public record UpdateOrderDetail
        (
            int? Id,
            int MealId,
            int OrderId,
            int Quantity
        );

    
}
