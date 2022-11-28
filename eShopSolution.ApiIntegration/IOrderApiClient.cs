using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Sales;

namespace eShopSolution.ApiIntegration
{
    public interface IOrderApiClient
    {
        Task<bool> CreatOderDetail(CheckoutRequest request);
    }
}