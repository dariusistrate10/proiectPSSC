using System;
using LanguageExt;
using proiectPSSC.Domain.Models;

namespace proiectPSSC.Repositories
{
    public interface IInvoiceRepository
    {
        TryAsync<List<InvoiceID>> TryGetExistingInvoices(IEnumerable<string> invoiceToCheck);
    }
}

