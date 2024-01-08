using System;
using System.Text.RegularExpressions;
using LanguageExt;
using proiectPSSC.Domain.Exceptions;
using static LanguageExt.Prelude;

namespace proiectPSSC.Domain.Models
{
    public record ProductID
    {
        private static readonly Regex ValidIDPattern = new("^LM[0-9]{5}$");
        public string Value { get; }

        internal ProductID(string value)
        {
            if(IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductIDException($"{Value} is an invalid product code");
            }
        }

          public override string ToString()
        {
            return Value;
        }

        public static Option<ProductID> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<ProductID>(new ProductID(stringValue));
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(string stringValue) => ValidIDPattern.IsMatch(stringValue);
    }
}

