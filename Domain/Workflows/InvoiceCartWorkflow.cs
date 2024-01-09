using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using proiectPSSC.Domain.Models;
using static Domain.Models.Cart;
using static proiectPSSC.Domain.Models.PayCartEvent;
using LanguageExt;
using static LanguageExt.Prelude;
using proiectPSSC.Repositories;
using Microsoft.Extensions.Logging;

namespace proiectPSSC.Domain.Workflows
{
    public class InvoiceCartWorkflow
    {
        private readonly IInvoiceRepository invoiceRepository;
        private readonly ILogger<InvoiceCartWorkflow> logger;

        public InvoiceCartWorkflow(IInvoiceRepository invoiceRepository, ILogger<InvoiceCartWorkflow> logger)
        {
            this.invoiceRepository = invoiceRepository;
            this.logger = logger;
        }

        //public async Task<Either<string, Invoice>> ExecuteAsync(IPayCartEvent payCartEvent, CalculatedCart calculatedCart)
        //{
        //    return await payCartEvent.MatchAsync(
        //        async succeededEvent =>
        //        {
        //            try
        //            {
        //                var validatedProducts = calculatedCart.ProductList.Select(calculatedProduct =>
        //                    new Invoice.ValidatedProduct(
        //                        calculatedProduct.ClientId,
        //                        calculatedProduct.Quantity,
        //                        calculatedProduct.Price
        //                    )).ToList();

        //                var client = calculatedCart.Client;

        //                var invoice = new Invoice(client, validatedProducts);

        //                DisplayInvoiceDetails(invoice);
        //                await SaveInvoiceToFile(invoice);

        //                return Right<string, Invoice>(invoice);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Left<string, Invoice>($"Failed to generate or save invoice. {ex.Message}");
        //            }
        //        },
        //        failedEvent => Task.FromResult<Either<string, Invoice>>(Left<string, Invoice>(failedEvent.Reason))
        //    );
        //}


        public Invoice GenerateInvoice(CalculatedCart calculatedCart)
        {
            var validatedProducts = calculatedCart.ProductList.Select(calculatedProduct =>
                new Invoice.ValidatedProduct(
                    calculatedProduct.ClientId,
                    calculatedProduct.Quantity,
                    calculatedProduct.Price
                )).ToList();

            var invoice = new Invoice(calculatedCart.Client, validatedProducts);
            DisplayInvoiceDetails(invoice);
            SaveInvoiceToFile(invoice);
            return invoice;
        }


        public void DisplayInvoiceDetails(Invoice invoice)
        {
            Console.WriteLine($"Invoice ID: {invoice.InvoiceID}");
            Console.WriteLine($"Client: {invoice.Client}");
            Console.WriteLine($"Products: {string.Join(", ", invoice.ValidatedProducts.Select(p => p.ToString()))}");
            Console.WriteLine($"Invoice Date: {invoice.InvoiceDate}");
            Console.WriteLine($"Total Price: {invoice.TotalPrice}");
            Console.WriteLine($"Invoice Number: {invoice.InvoiceNumber}");
        }

        public async Task SaveInvoiceToFile(Invoice invoice)
        {
            string fileName = $"{invoice.InvoiceNumber}.txt";
            using (StreamWriter writer = File.CreateText(fileName))
            {
                await writer.WriteLineAsync($"Invoice ID: {invoice.InvoiceID}");
                await writer.WriteLineAsync($"Client: {invoice.Client}");
                await writer.WriteLineAsync($"Products: {string.Join(", ", invoice.ValidatedProducts.Select(p => p.ToString()))}");
                await writer.WriteLineAsync($"Invoice Date: {invoice.InvoiceDate}");
                await writer.WriteLineAsync($"Total Price: {invoice.TotalPrice}");
                await writer.WriteLineAsync($"Invoice Number: {invoice.InvoiceNumber}");
            }

            Console.WriteLine($"Invoice saved to file: {fileName}");
        }

        internal Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }

        public abstract class InvoiceWorkflowResult
        {
            public class Success : InvoiceWorkflowResult
            {
                public Invoice Invoice { get; }

                public Success(Invoice invoice)
                {
                    Invoice = invoice;
                }
            }

            public class Failure : InvoiceWorkflowResult
            {
                public string ErrorMessage { get; }

                public Failure(string errorMessage)
                {
                    ErrorMessage = errorMessage;
                }
            }
        }
    }
}
