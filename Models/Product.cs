using System;
using System.Diagnostics;
using LanguageExt;
using static LanguageExt.Prelude;
using proiectPSSC.Domain.Exceptions;

namespace proiectPSSC.Domain.Models
{
	public record Product
	{
		public decimal Value { get; }

		public Product(decimal value)
		{
			if(IsValid(value))
			{
				Value = value;
			}
			else
			{
				throw new InvalidProductIDException($"{value: 0.##} is an invalid product price.");
			}
		}

		public static Product operator +(Product a, Product b) => new Product((a.Value + b.Value));

        public override string ToString()
        {
            return $"{Value: 0.##}";
        }

        public static Option<Product> TryParseProduct(string productString)
        {
            if (decimal.TryParse(productString, out decimal numericProduct) && IsValid(numericProduct))
            {
                return Some<Product>(new Product(numericProduct));
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(decimal numericProduct) => numericProduct > 0;
    }
}

