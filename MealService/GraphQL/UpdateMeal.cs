namespace MealService.GraphQL
{
    public record UpdateMeal
        (
            int Id,
            string Name,
            int Stock,
            int Price
        );
    
}
