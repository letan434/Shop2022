using System;
using System.Threading.Tasks;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace eShopSolution.Application.Catalog.orders
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;


        public OrderService(EShopDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
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
            var user = _httpContextAccessor.HttpContext.User.Identity.Name;

            var userEntity = await _userManager.FindByNameAsync(user);
            order.UserId = userEntity.Id;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            if (checkoutRequest.OrderDetails.Count > 0)
            {

                checkoutRequest.OrderDetails.ForEach(
                    value =>
                    {
                        var orderdetail = new OrderDetail()
                        {
                            ProductId = value.ProductId,
                            OrderId = order.Id,
                            Quantity = value.Quantity,
                            Price = value.Price
                        };
                        _context.OrderDetails.Add(orderdetail);
                    });

            }
            var result =  await _context.SaveChangesAsync();
            return result == 1;
        }
    }
}
