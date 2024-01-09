using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiectPSSC.Domain.Models
{
    public class Invoice
    {
        public record ValidatedProduct(ClientID ClientID, Quantity Quantity, Price Price);
        public Invoice(Client client, List<ValidatedProduct> validatedProducts)
        {
            InvoiceID = Guid.NewGuid();
            Client = client;
            ValidatedProducts = validatedProducts;
            InvoiceDate = DateTime.Now;
            TotalPrice = CalculateTotalPrice(validatedProducts);
            InvoiceNumber = GenerateInvoiceNumber(client);
        }

        public Guid InvoiceID { get; }
        public Client Client { get; }
        public List<ValidatedProduct> ValidatedProducts { get; }
        public DateTime InvoiceDate { get; }
        public Price TotalPrice { get; }
        public string InvoiceNumber { get; }

        private Price CalculateTotalPrice(List<ValidatedProduct> products)
        {
            Price totalPrice = Price.Zero;

            foreach (var product in products)
            {
                totalPrice = totalPrice.Add(product.Price);
            }

            return totalPrice;
        }


        private string GenerateInvoiceNumber(Client client)
        {
            Random random = new Random();
            string randomSuffix = random.Next(1000, 10000).ToString(); 
            string formattedDate = DateTime.Now.ToString("yyyyMMdd_HHmmss"); 

            return $"{client.ClientId}_{client.Name}_{formattedDate}_{randomSuffix}";
        }
    }
}