using System;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using proiectPSSC.Domain.Models;
using proiectPSSC.Repositories;

namespace proiectPSSC.Data.DataRepositories
{
	public class ClientsRepository: IClientsRepository
	{
        private readonly ProductsContext productsContext;

        public ClientsRepository(ProductsContext productsContext)
        {
            this.productsContext = productsContext;
        }

        public TryAsync<List<ClientID>> TryGetExistingClients(IEnumerable<string> clientsToCheck) => async () =>
        {
            var clients = await productsContext.Clients
                                              .Where(client => clientsToCheck.Contains(client.ClientCode))
                                              .AsNoTracking()
                                              .ToListAsync();
            return clients.Select(client => new ClientID(client.ClientCode))
                           .ToList();
        };
    }
}

