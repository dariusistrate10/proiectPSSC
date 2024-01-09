using System;
namespace proiectPSSC.Domain.Models
{
	public record CalculatedProduct(ClientID clientID, Quantity quantity, Price price, Price totalPrice)
	{
		public int ProductId { get; set; }
		public bool IsUpdated { get; set; }
        public ClientID ClientId { get; internal set; }
        public Quantity Quantity { get; internal set; }
        public Price Price { get; internal set; }
    }
}

