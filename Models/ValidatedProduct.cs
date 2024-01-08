using System;
using proiectPSSC.Domain.Models;

namespace Domain.Models
{
    public record ValidatedProduct(ClientID clientID, Quantity quantity, Price price);
}