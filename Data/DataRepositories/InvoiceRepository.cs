using System;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using proiectPSSC.Domain.Models;
using proiectPSSC.Repositories;

namespace proiectPSSC.Data.DataRepositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly InvoiceContext invoiceContext;

        public InvoiceRepository(InvoiceContext invoiceContext)
        {
            this.invoiceContext = invoiceContext;
        }

        public TryAsync<List<InvoiceID>> TryGetExistingInvoices(IEnumerable<string> invoicesToCheck) => async () =>
        {
            var invoices = await invoiceContext.Invoices
                                              .Where(invoice => invoicesToCheck.Contains(invoice.InvoiceNumber))
                                              .AsNoTracking()
                                              .ToListAsync();
            return invoices.Select(invoice => new InvoiceID(invoice.InvoiceNumber))
                           .ToList();
        };
    }
}

