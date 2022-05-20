using HotChocolate.AspNetCore.Authorization;
using OrderService.Models;

namespace OrderService.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] { "BUYER" })]
        public async Task<OrderData> AddOrderAsync(
           OrderInput input,
           [Service] MealDeliveryContext context)
        {
            var order = context.Orders.Where(o => o.Id == input.Id).FirstOrDefault();
            if (order != null)
            {
                return await Task.FromResult(new OrderData());
            }
            var newOrder = new Order
            {
                Code = input.Code,
                UserId = input.UserId,
                CourierId = input.CourierId,
                Longitude = input.Longitude,
                Latitude = input.Latitude,
               
            };

            // EF
            var ret = context.Orders.Add(newOrder);
            await context.SaveChangesAsync();

            return await Task.FromResult(new OrderData
            {
                Id = newOrder.Id,
                Code = newOrder.Code,
                UserId = newOrder.UserId,
                CourierId = newOrder.CourierId,
                Longitude = newOrder.Longitude,
                Latitude = newOrder.Latitude
                
            });
        }

        [Authorize(Roles = new[] { "BUYER" })]
        public async Task<OrderDetailData> AddOrderDetailAsync(
           OrderDetailInput input,
           [Service] MealDeliveryContext context)
        {
            var orderDetail = context.OrderDetails.Where(o => o.Id == input.Id).FirstOrDefault();
            if (orderDetail != null)
            {
                return await Task.FromResult(new OrderDetailData());
            }
            var newOrderDetail = new OrderDetail
            {
                MealId = input.MealId,
                OrderId = input.OrderId,
                Quantity = input.Quantity
            };

            // EF
            var ret = context.OrderDetails.Add(newOrderDetail);
            await context.SaveChangesAsync();

            return await Task.FromResult(new OrderDetailData
            {
                Id = newOrderDetail.Id,
                MealId = newOrderDetail.MealId,
                OrderId = newOrderDetail.OrderId,
                Quantity = newOrderDetail.Quantity
            });
        }
        [Authorize(Roles = new[] { "BUYER" })]
        public async Task<Order> UpdateOrderAsync(
           UpdateOrder input,
           [Service] MealDeliveryContext context)
        {
            var order = context.Orders.Where(o => o.Id == input.Id).FirstOrDefault();
            if (order != null)
            {
                order.Code = input.Code;
                order.UserId = input.UserId;
                order.CourierId = input.CourierId;
                order.Longitude = input.Longitude;
                order.Latitude = input.Latitude;

                context.Orders.Update(order);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(order);
        }

        [Authorize(Roles = new[] { "BUYER" })]
        public async Task<OrderDetail> UpdateOrderDetailAsync(
          UpdateOrderDetail input,
          [Service] MealDeliveryContext context)
        {
            var orderdetail = context.OrderDetails.Where(o => o.Id == input.Id).FirstOrDefault();
            if (orderdetail != null)
            {
                orderdetail.MealId = input.MealId;
                orderdetail.OrderId = input.OrderId;
                orderdetail.Quantity = input.Quantity;


                context.OrderDetails.Update(orderdetail);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(orderdetail);
        }

        [Authorize(Roles = new[] { "COURIER" })]
        public async Task<Order> AddTrackingAsync(
            OrderData input, int id,
            [Service] MealDeliveryContext context)
        {
            var order = context.Orders.Where(o => o.Id == input.Id).FirstOrDefault();
            if (order != null)
            {
                order.Longitude = input.Longitude;
                order.Latitude = input.Latitude;

                context.Orders.Update(order);
                await context.SaveChangesAsync();
            }

            return await Task.FromResult(order);

        }
    }
}
