using System;
using proiectPSSC.Domain.Exceptions;
using System.Text.RegularExpressions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace proiectPSSC.Domain.Models
{
	public class InvoiceID
	{
        private static readonly Regex ValidPattern = new("^I[0-9]{5}$");
        public string Value { get; }

        internal InvoiceID(string value)
        {
            if (IsValid(value))
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

        public static Option<InvoiceID> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<InvoiceID>(new InvoiceID(stringValue));
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);
    }
}

