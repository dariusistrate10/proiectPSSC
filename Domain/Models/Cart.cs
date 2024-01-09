using System;
using CSharp.Choices;
using proiectPSSC.Domain.Models;

namespace Domain.Models
{
    [AsChoice]
    public static partial class Cart
    {
        public interface ICart { }

        public record UnvalidatedCart: ICart
        {
            public UnvalidatedCart(IReadOnlyCollection<UnvalidatedProduct> productList)
            {
                ProductList = productList;
            }

            public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
        }

        public record InvalidatedCart : ICart
        {
            public InvalidatedCart(IReadOnlyCollection<UnvalidatedProduct> productList, string reason)
            {
                ProductList = productList;
                Reason = reason;
            }

            public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
            public string Reason { get; }
        }

        public record FailedCart : ICart
        {
            internal FailedCart(IReadOnlyCollection<UnvalidatedProduct> productList, Exception exception)
            {
                ProductList = productList;
                Exception = exception;
            }

            public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
            public Exception Exception { get; }
        }

        public record ValidatedCart: ICart
        {
            internal ValidatedCart(IReadOnlyCollection<ValidatedProduct> productList)
            {
                ProductList = productList;
            }

            public IReadOnlyCollection<ValidatedProduct> ProductList { get; }
        }
        public record CalculatedCart : ICart
        {
            internal CalculatedCart() { }

            internal CalculatedCart(IReadOnlyCollection<CalculatedProduct> productList)
            {
                ProductList = productList;
            }

            public IReadOnlyCollection<CalculatedProduct> ProductList { get; }
            public Client Client { get; internal set; }
        }


        public record PaidCart: ICart
        {
            internal PaidCart(IReadOnlyCollection<CalculatedProduct> productList, string csv, DateTime paidDate)
            {
                ProductList = productList;
                PaidDate = paidDate;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedProduct> ProductList { get; }
            public string Csv { get; }
            public DateTime PaidDate { get; }
        }
    }
}