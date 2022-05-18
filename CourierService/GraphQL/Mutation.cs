using CourierService.Models;
using HotChocolate.AspNetCore.Authorization;

namespace CourierService.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<CourierData> AddCourierAsync(
          CourierInput input,
          [Service] MealAppContext context)
        {
            var courier = context.Couriers.Where(o => o.Id == input.Id).FirstOrDefault();
            if (courier != null)
            {
                return await Task.FromResult(new CourierData());
            }
            var newCourier = new Courier
            {
                Name = input.Name,
                Cost = input.Cost
               
            };

            // EF
            var ret = context.Couriers.Add(newCourier);
            await context.SaveChangesAsync();

            return await Task.FromResult(new CourierData
            {
                Id = newCourier.Id,
                Name = newCourier.Name,
                Cost = newCourier.Cost
                
            });
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Courier> UpdateCourierAsync(
            UpdateCourier input,
            [Service] MealAppContext context)
        {
            var courier = context.Couriers.Where(o => o.Id == input.Id).FirstOrDefault();
            if (courier != null)
            {
                courier.Name = input.Name;
                courier.Cost = input.Cost;

                context.Couriers.Update(courier);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(courier);
        }
    }
}
