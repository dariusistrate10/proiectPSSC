using System.Text;
using Domain.Models;
using static LanguageExt.Prelude;
using LanguageExt;
using proiectPSSC.Domain.Models;
using static Domain.Models.Cart;
using System.Linq;

namespace proiectPSSC.Domain.Operations
{
    public static class CartOperation
	{
        public static Task<ICart> ValidateCart(Func<ClientID, Option<ClientID>> checkProductExists, UnvalidatedCart cart) =>
            cart.ProductList
                .Select(ValidateProduct(checkProductExists))
                .Aggregate(CreateEmptyValidatedProductList().ToAsync(), ReduceValidProducts)
                .MatchAsync(
                    Right: validatedProducts => new ValidatedCart(validatedProducts),
                    LeftAsync: errorMessage => Task.FromResult((ICart)new InvalidatedCart(cart.ProductList, errorMessage))
                );

        private static Func<UnvalidatedProduct, EitherAsync<string, ValidatedProduct>> ValidateProduct(Func<ClientID, Option<ClientID>> checkProductExists) =>
            unvalidatedProduct => ValidateProduct(checkProductExists, unvalidatedProduct);

        private static EitherAsync<string, ValidatedProduct> ValidateProduct(Func<ClientID, Option<ClientID>> checkProductExists, UnvalidatedProduct unvalidatedProduct) =>
            from quantity in Quantity.TryParseQuantity(unvalidatedProduct.quantity)
                                   .ToEitherAsync(() => $"Invalid quantity ({unvalidatedProduct.clientId}, {unvalidatedProduct.quantity})")
            from price in Price.TryParsePrice(unvalidatedProduct.price)
                                   .ToEitherAsync(() => $"Invalid price ({unvalidatedProduct.clientId}, {unvalidatedProduct.price})")
            from clientId in ClientID.TryParse(unvalidatedProduct.clientId)
                                   .ToEitherAsync(() => $"Invalid product ID ({unvalidatedProduct.clientId})")
            from clientExists in checkProductExists(clientId)
                                   .ToEitherAsync($"Client {clientId.Value} does not exist.")
            select new ValidatedProduct(clientId, quantity, price);

        private static Either<string, List<ValidatedProduct>> CreateEmptyValidatedProductList() =>
            Right(new List<ValidatedProduct>());

        private static EitherAsync<string, List<ValidatedProduct>> ReduceValidProducts(EitherAsync<string, List<ValidatedProduct>> acc, EitherAsync<string, ValidatedProduct> next) =>
            from list in acc
            from nextGrade in next
            select list.AppendValidProduct(nextGrade);

        private static List<ValidatedProduct> AppendValidProduct(this List<ValidatedProduct> list, ValidatedProduct validProduct)
        {
            list.Add(validProduct);
            return list;
        }


        public static ICart CalculateCart(ICart cart) => cart.Match(
            whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
            whenInvalidatedCart: invalidatedCart => invalidatedCart,
            whenFailedCart: failedCart => failedCart,
            whenCalculatedCart: calculatedCart => calculatedCart,
            whenPaidCart: paidCart => paidCart,
            whenValidatedCart: CalculateCart
        );

        private static ICart CalculateCart(ValidatedCart validatedCart) =>
            new CalculatedCart(validatedCart.ProductList
                                                    .Select(CalculateProduct)
                                                    .ToList()
                                                    .AsReadOnly());

        private static CalculatedProduct CalculateProduct(ValidatedProduct validProduct) =>
            new CalculatedProduct(validProduct.clientID,
                                      validProduct.quantity,
                                      validProduct.price,
                                      validProduct.price * validProduct.quantity);

        public static ICart MergeProducts(ICart cart, IEnumerable<CalculatedProduct> existingProducts) => cart.Match(
           whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
           whenInvalidatedCart: invalidatedCart => invalidatedCart,
           whenFailedCart: failedCart => failedCart,
           whenValidatedCart: validatedCart => validatedCart,
           whenPaidCart: paidCart => paidCart,
           whenCalculatedCart: calculatedCart => MergeProducts(calculatedCart.ProductList, existingProducts));

        private static CalculatedCart MergeProducts(IEnumerable<CalculatedProduct> newList, IEnumerable<CalculatedProduct> existingList)
        {
            var updatedAndNewProducts = newList.Select(product => product with { ProductId = existingList.FirstOrDefault(p => p.clientID == product.clientID)?.ProductId ?? 0, IsUpdated = true });
            var oldProducts = existingList.Where(product => !newList.Any(p => p.clientID == product.clientID));
            var allGrades = updatedAndNewProducts.Union(oldProducts)
                                               .ToList()
                                               .AsReadOnly();
            return new CalculatedCart(allGrades);
        }

        public static ICart PayCart(ICart cart) => cart.Match(
            whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
            whenInvalidatedCart: invalidatedCart => invalidatedCart,
            whenFailedCart: failedCart => failedCart,
            whenValidatedCart: validatedCart => validatedCart,
            whenPaidCart: paidCart => paidCart,
            whenCalculatedCart: GenerateExport);

        private static ICart GenerateExport(CalculatedCart calculatedCart) =>
            new PaidCart(calculatedCart.ProductList,
                                    calculatedCart.ProductList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(),
                                    DateTime.Now);

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedProduct product) =>
            export.AppendLine($"{product.clientID}, {product.quantity}, {product.price}, {product.totalPrice}");
    }
}