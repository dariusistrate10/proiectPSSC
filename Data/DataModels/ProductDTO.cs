using System;
namespace proiectPSSC.Data.DataModels
{
	public class ProductDTO
	{
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}

