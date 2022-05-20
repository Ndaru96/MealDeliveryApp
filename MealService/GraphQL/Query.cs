using HotChocolate.AspNetCore.Authorization;
using MealService.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MealService.GraphQL
{
    public class Query
    {
        [Authorize] // dapat diakses kalau sudah login
        public IQueryable<Meal> GetMeals([Service] MealDeliveryContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check admin role ?
            var adminRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role).FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (adminRole.Value == "BUYER" || adminRole.Value == "MANAGER")
                {
                    return context.Meals;
                }


            }


            return new List<Meal>().AsQueryable();
        }
    }
}
