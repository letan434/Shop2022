using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.orders;
using eShopSolution.ViewModels.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(
            IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("checkout")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Checkout( CheckoutRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var check = await _orderService.Checkout(request);
            return Ok(check);
            //if (productId == 0)
            //    return BadRequest();

            //var product = await _productService.GetById(productId);

            //return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }
    }
}
