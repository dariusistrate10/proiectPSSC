using System;
using Domain.Models;

namespace proiectPSSC.Domain.Commands
{
	public record PaymentCartCommand
	{
		public PaymentCartCommand(IReadOnlyCollection<UnvalidatedProduct> inputProducts)
		{
			InputProducts = inputProducts;
		}

		public IReadOnlyCollection<UnvalidatedProduct> InputProducts { get; }
	}
}

