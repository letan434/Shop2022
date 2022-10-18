using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }

        public bool? IsFeatured { get; set; }

        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }
        //Kiểu thân

        public string BodyType { get; set; }
        //Gỗ thân

        public string TrunkWood { get; set; }
        //Finish thân

        public string FinishBody { get; set; }
        //Dáng cần đàn	
        public string NeckShape { get; set; }
        //Chất liệu mặt cần		

        public string FaceMaterial { get; set; }
        //Độ cong cần		

        public string NeedleCurvature { get; set; }
        //Độ dài scale		

        public string ScaleLength { get; set; }
        //Số phím		

        public string NumberofKeys { get; set; }
        //Inlays mặt cần	

        public string InlaysFaceNeed { get; set; }
        //Pickups	

        public string Pickups { get; set; }
        //Ngựa đàn		

        public string Horses { get; set; }
        //Pickguard	

        public string Pickguard { get; set; }
        //Dây đàn		

        public string GuitarString { get; set; }
        //Bộ khoá	

        public string LockSet { get; set; }

        //Hardware Finish	
        public string HardwareFinish	 { get; set; }

        public List<ProductInCategory> ProductInCategories { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public List<Cart> Carts { get; set; }

        public List<ProductImage> ProductImages { get; set; }
    }
}