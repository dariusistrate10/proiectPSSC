using System;
using LanguageExt;
using proiectPSSC.Domain.Models;
using static Domain.Models.Cart;

namespace proiectPSSC.Repositories
{
	public interface IProductsRepository
	{
		TryAsync<List<CalculatedProduct>> TryGetExistingProducts();

		TryAsync<Unit> TrySaveProducts(PaidCart paidCart);
	}
}

