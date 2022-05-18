using HotChocolate.AspNetCore.Authorization;
using MealService.Models;

namespace MealService.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<MealData> AddMealAsync(
           MealInput input,
           [Service] MealAppContext context)
        {
            var meal = context.Meals.Where(o => o.Id == input.Id).FirstOrDefault();
            if (meal != null)
            {
                return await Task.FromResult(new MealData());
            }
            var newMeal = new Meal
            {
                Name = input.Name,
                Stock = input.Stock,
                Price = input.Price
            };

            // EF
            var ret = context.Meals.Add(newMeal);
            await context.SaveChangesAsync();

            return await Task.FromResult(new MealData
            {
                Id = newMeal.Id,
                Name = newMeal.Name,
                Stock = newMeal.Stock,
                Price = newMeal.Price
            });
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Meal> UpdateMealAsync(
            UpdateMeal input,
            [Service] MealAppContext context)
        {
            var meal = context.Meals.Where(o => o.Id == input.Id).FirstOrDefault();
            if (meal != null)
            {
                meal.Name = input.Name;
                meal.Stock = input.Stock;
                meal.Price = input.Price;

                context.Meals.Update(meal);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(meal);
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Meal> DeleteMealByIdAsync(
           int id,
           [Service] MealAppContext context)
        {
            var meal = context.Meals.Where(o => o.Id == id).FirstOrDefault();
            if (meal != null)
            {
                context.Meals.Remove(meal);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(meal);
        }
    }
}
