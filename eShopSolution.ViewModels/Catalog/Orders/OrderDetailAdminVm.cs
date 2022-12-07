using System;
using System.Collections.Generic;
using eShopSolution.ViewModels.Sales;

namespace eShopSolution.ViewModels.Catalog.Orders
{
    public class OrderDetailAdminVm
    {
        public int Id { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhoneNumber { get; set; }
        public int Status { set; get; }
        public string UserName { get; set; }
        public List<OrderDetailVm> OrderDetails { get; set; }
        public int[] Statuses = new[] { 0, 1, 2, 3, 4 };
    }
}
