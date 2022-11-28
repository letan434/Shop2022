using System;
using System.Threading.Tasks;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Sales;

namespace eShopSolution.Application.Catalog.orders
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;
        public OrderService(EShopDbContext context)
        {
            _context = context;

        }

        public async Task<bool> Checkout(CheckoutRequest checkoutRequest)
        {
            var order = new Order();
            order.OrderDate = DateTime.Now;
            order.ShipPhoneNumber = checkoutRequest.PhoneNumber;
            order.ShipName = checkoutRequest.Name;
            order.ShipEmail = checkoutRequest.Email;
            order.ShipAddress = checkoutRequest.Address;
            order.Status = Data.Enums.OrderStatus.InProgress;

            await _context.Orders.AddAsync(order);
            if (checkoutRequest.OrderDetails.Count > 0)
            {

                checkoutRequest.OrderDetails.ForEach(
                    value =>
                    {
                        var orderdetail = new OrderDetail()
                        {
                            ProductId = value.ProductId,
                            OrderId = order.Id

                        };
                        _context.OrderDetails.Add(orderdetail);
                    });

            }
            return true;
        }
    }
}
