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
            var newCourier = new Courier
            {
                UserId = input.UserId,
                Name = input.Name,
                Phone = input.Phone
               
               
            };

            // EF
            var ret = context.Couriers.Add(newCourier);
            await context.SaveChangesAsync();

            return await Task.FromResult(new CourierData
            {
                Id = newCourier.Id,
                Name = newCourier.Name,
                Phone = newCourier.Phone,
                UserId = newCourier.UserId
            }) ;
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Courier> UpdateCourierAsync(
            UpdateCourier input,
            [Service] MealAppContext context)
        {
            var courier = context.Couriers.Where(o => o.Id == input.Id).FirstOrDefault();
            if (courier != null)
            {
                courier.UserId = input.UserId;
                courier.Name = input.Name;
                courier.Phone = input.Phone;
               

                context.Couriers.Update(courier);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(courier);
        }
    }
}
