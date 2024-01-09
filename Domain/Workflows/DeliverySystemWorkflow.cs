using Domain.Models;
using LanguageExt.ClassInstances;
using Microsoft.Extensions.Logging;
using proiectPSSC.Data;
using proiectPSSC.Data.DataRepositories;
using proiectPSSC.Domain.Models;
using proiectPSSC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.Cart;

namespace proiectPSSC.Domain.Workflows
{
    public class DeliverySystemWorkflow
    {
        private readonly ProductsContext productsContext;
        private readonly ILogger logger;
        public DeliverySystemWorkflow(ProductsContext productsContext, ILogger<AddProductsToCartWorkflow> logger)
        {
            this.productsContext = productsContext;
            this.logger = logger;
        }
        private Boolean RandomlyGenerateAvailability()
        {
            return new Random().NextDouble() >= 0.5 ? false : true;
        }

        public Boolean Validate(int clientID, PaidCart paidCart)
        {
            var result = productsContext.Clients.FirstOrDefault(d => d.ClientId == clientID);
            if (result == null)
            {
                logger.LogError("Error");
                return false;
            }
            if (!(result.Address.Length > 0 && result.Address.Length < 50))
            {
                logger.LogError("Address not valid");
                return false;
            }
            if (this.RandomlyGenerateAvailability())
            {
                logger.LogError("No car available");
                return false;
            }
            return true;
        }
    }

}