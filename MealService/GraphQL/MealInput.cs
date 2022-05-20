namespace MealService.GraphQL
{
    public record MealInput
        (
            int? Id,
            string Name,
            int Stock,
            double Price
        );
}
