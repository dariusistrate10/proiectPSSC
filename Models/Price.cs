using System;
using System.Diagnostics;
using LanguageExt;
using proiectPSSC.Domain.Exceptions;
using static LanguageExt.Prelude;

namespace proiectPSSC.Domain.Models
{
	public record Price
	{
		public decimal Value { get; }

		internal Price(decimal value)
		{
			if(IsValid(value))
			{
				Value = value;
			}
			else
			{
				throw new InvalidPriceException($"{Value} is an invalid price");
			}
		}

		public static Price operator *(Price a, Price b) => new Price((a.Value * b.Value));

        public override string ToString()
        {
			return $"{Value:0.##}";
        }

        public static Option<Price> TryParsePrice(string priceString)
        {
            if (decimal.TryParse(priceString, out decimal numericPrice) && IsValid(numericPrice))
            {
                return Some<Price>(new Price(numericPrice));
            }
            else
            {
                return None;
            }
        }

        public static bool IsValid(decimal numericPrice) => numericPrice > 0;
    }
}

