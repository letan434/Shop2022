using System;
namespace eShopSolution.ViewModels.Catalog.Orders
{
    public class OrderVm
    {
        public int Id { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhoneNumber { get; set; }
        public int Status { set; get; }
        public string UserName { get; set; }

    }
}
