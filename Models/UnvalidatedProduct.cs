using System;

namespace Domain.Models
{
    public record UnvalidatedProduct(string clientId, string quantity, string price);
}