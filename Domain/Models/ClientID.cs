using System;
using static LanguageExt.Prelude;
using System.Text.RegularExpressions;
using LanguageExt;
using proiectPSSC.Domain.Exceptions;

namespace proiectPSSC.Domain.Models
{
	public record ClientID
	{
        private static readonly Regex ValidPattern = new("^C[0-9]{5}$");

        public string Value { get; }

        internal ClientID(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidClientIDException("");
            }
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }

        public static Option<ClientID> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<ClientID>(new ClientID(stringValue));
            }
            else
            {
                return None;
            }
        }
    }
}

