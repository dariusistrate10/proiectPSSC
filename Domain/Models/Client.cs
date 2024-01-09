using System;
namespace proiectPSSC.Domain.Models
{
	public class Client
	{
        public Client(string name, long phoneNumber, string address)
        {
            this.ClientId = Guid.NewGuid();
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
        }

        public Guid ClientId { get; }
        public string Name { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }

        public Guid GetClientId()
        {
            return ClientId;
        }
    }
}

