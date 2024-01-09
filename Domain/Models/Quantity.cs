using System;
using proiectPSSC.Domain.Exceptions;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Diagnostics;

namespace proiectPSSC.Domain.Models
{
	public record Quantity
	{
        public decimal Value { get; }

        internal Quantity(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidQuantityException($"{Value} is an invalid quantity");
            }
        }

        public static Quantity operator *(Quantity a, Quantity b) => new Quantity((a.Value * b.Value));

        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        public static Option<Quantity> TryParseQuantity(string quantityString)
        {
            if (decimal.TryParse(quantityString, out decimal numericQuantity) && IsValid(numericQuantity))
            {
                return Some<Quantity>(new Quantity(numericQuantity));
            }
            else
            {
                return None;
            }
        }

        public static bool IsValid(decimal numericPrice) => numericPrice > 0;
    }
}