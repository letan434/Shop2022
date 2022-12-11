using System;
namespace eShopSolution.Data.Entities
{
    public class ProductStart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Start { get; set; }
        public Guid UserId { get; set; }
        public string Comment { get; set; }
        public AppUser AppUser { get; set; }

    }
}
