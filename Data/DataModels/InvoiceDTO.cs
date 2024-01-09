using System;
using proiectPSSC.Domain.Models;

namespace proiectPSSC.Data.DataModels
{
	public class InvoiceDTO
	{
        public int InvoiceID { get; set; }
        public Client Client { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}

