using System;
using CSharp.Choices;
using static Domain.Models.Cart;

namespace proiectPSSC.Domain.Models
{
	[AsChoice]
	public static partial class PayCartEvent
	{
        public interface IPayCartEvent { }

        public record PayCartSucceededEvent: IPayCartEvent
		{
			public string Csv { get; }
			public DateTime PaidDate { get; }
			public PaidCart PaidCart { get; }

			internal PayCartSucceededEvent(string csv, DateTime paidDate, PaidCart paidCart)
			{
				Csv = csv;
				PaidDate = paidDate;
				PaidCart = paidCart;
			}

        }

		public record PayCartFailedEvent: IPayCartEvent
		{
			public string Reason { get; }

			internal PayCartFailedEvent(string reason)
			{
				Reason = reason;
			}
        }
	}
}

