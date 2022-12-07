using System;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModels.Catalog.Orders;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ApiResult<PagedResult<OrderVm>>> GetOrdersPaging(PagingRequestBase request)
        {

            var query =  from o in _context.Orders join u in _context.Users on o.UserId equals u.Id select new { o, u };


            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderVm()
                {
                    Id = x.o.Id,
                    ShipAddress = x.o.ShipAddress,
                    ShipEmail = x.o.ShipEmail,
                    ShipName= x.o.ShipName,
                    ShipPhoneNumber = x.o.ShipPhoneNumber,
                    Status = (int)x.o.Status,
                    UserName = x.u.UserName
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<OrderVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<OrderVm>>(pagedResult);
        }

        public async Task<ApiResult<OrderDetailAdminVm>> GetById(int id)
        {
            

            var order = await _context.Orders.FindAsync(id);

            var orderDetails = await (from o in _context.Orders
                                      join od in _context.OrderDetails on o.Id equals od.OrderId into odd
                                      from od in odd.DefaultIfEmpty()
                                      join prd in _context.Products on od.ProductId equals prd.Id into prdd
                                      from prd in prdd.DefaultIfEmpty()
                                      where o.Id == id

                                      select new { od, prd }
                                    ).Select(x=>new OrderDetailVm() {
                                        Price= x.od.Price,
                                        ProducName = x.prd.Name,
                                        ProducDescription = x.prd.Description,
                                        ProductId = x.prd.Id,
                                        Quantity = x.od.Quantity
                                    }).ToListAsync();
            var orderDetailAdminVm = new OrderDetailAdminVm()
            {
                Id = order.Id,
                OrderDetails = orderDetails,
                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                ShipName = order.ShipName,
                ShipPhoneNumber = order.ShipPhoneNumber,
                Status = (int)order.Status,
                UserName = order.ShipName
            };
            return new ApiSuccessResult<OrderDetailAdminVm>(orderDetailAdminVm);

        }

        public async Task<ApiResult<bool>> ChangeStatus(int id, OrderDetailAdminVm request)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return new ApiErrorResult<bool>("Phiếu mua hàng không tồn tại");
            }
            if(request.Status == (int)order.Status)
            {
                return new ApiErrorResult<bool>("Bạn chưa thay đổi trạng thái");

            }
            order.Status =(OrderStatus)request.Status;
            _context.Orders.Update(order);
            var result = await _context.SaveChangesAsync();
            if(result >0)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiSuccessResult<bool>();

        }
    }
}
