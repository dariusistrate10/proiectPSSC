using System;
namespace proiectPSSC.Domain.Models
{
	public record CalculatedProduct(ClientID clientID, Quantity quantity, Price price, Price totalPrice)
	{
		public int ProductId { get; set; }
		public bool IsUpdated { get; set; }
	}
}

