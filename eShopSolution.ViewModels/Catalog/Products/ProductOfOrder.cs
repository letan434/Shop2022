using System;
namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductOfOrder
    {
        public int Id { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }

        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }

        public bool? IsFeatured { get; set; }

        public int Quantity { get; set; }
        public int StatusOrder { get; set; }

    }
}
