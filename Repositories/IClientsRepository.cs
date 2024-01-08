using System;
using LanguageExt;
using proiectPSSC.Domain.Models;

namespace proiectPSSC.Repositories
{
	public interface IClientsRepository
	{
        TryAsync<List<ClientID>> TryGetExistingClients(IEnumerable<string> clientsToCheck);
    }
}