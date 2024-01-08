using System;
using System.Diagnostics;
using LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using proiectPSSC.Data.DataModels;
using proiectPSSC.Domain.Models;
using proiectPSSC.Repositories;
using static Domain.Models.Cart;
using System.Collections.Generic;
using System.Linq;
using static LanguageExt.Prelude;

namespace proiectPSSC.Data.DataRepositories
{
	public class ProductsRepository: IProductsRepository
	{
        private readonly ProductsContext dbContext;
     
        public ProductsRepository(ProductsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<CalculatedProduct>> TryGetExistingProducts() => async () => (await (
                          from p in dbContext.Products
                          join c in dbContext.Clients on p.ClientId equals c.ClientId
                          select new { c.ClientCode, p.ProductId, p.Quantity, p.Price })
                          .AsNoTracking()
                          .ToListAsync())
                          .Select(result => new CalculatedProduct(
                                                    clientID: new ClientID(result.ClientCode),
                                                    quantity: new Quantity(result.Quantity ?? 0m),
                                                    price: new Price(result.Price ?? 0m),
                                                    totalPrice: new Price(result.Price ?? 0m))
                          {
                              ProductId = result.ProductId
                          })
                          .ToList();

        public TryAsync<Unit> TrySaveProducts(PaidCart cart) => async () =>
        {
            var clients = (await dbContext.Clients.ToListAsync()).ToLookup(client => client.ClientCode);
            var newProducts = cart.ProductList
                                    .Where(p => p.IsUpdated && p.ProductId == 0)
                                    .Select(p => new ProductDTO()
                                    {
                                        ClientId = clients[p.clientID.Value].Single().ClientId,
                                        Quantity = p.quantity.Value,
                                        Price = p.price.Value,
                                        TotalPrice = p.totalPrice.Value,
                                    });
            var updatedProducts = cart.ProductList.Where(p => p.IsUpdated && p.ProductId > 0)
                                    .Select(p => new ProductDTO()
                                    {
                                        ProductId = p.ProductId,
                                        ClientId = clients[p.clientID.Value].Single().ClientId,
                                        Quantity = p.quantity.Value,
                                        Price = p.price.Value,
                                        TotalPrice = p.totalPrice.Value,
                                    });

            dbContext.AddRange(newProducts);
            foreach (var entity in updatedProducts)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();

            return unit;
        };
    }
}

