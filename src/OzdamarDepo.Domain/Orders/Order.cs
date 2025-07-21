using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OzdamarDepo.Domain.Orders
{
    public  class Order:Entity
    {
        public string OrderNumber { get; set; } = default!;
        public DateTimeOffset Date { get; set; }
        public Guid UserId { get; set; }

        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string City { get; set; } = default!;
        public string District { get; set; } = default!;
        public string FullAdress { get; set; } = default!;
        public string CartNumber { get; set; } = default!;
        public string CartOwnerName { get; set; } = default!;
        public string ExpiresDate { get; set; } = default!;
        public int Cvv { get; set; }
        public string InstallmentOptions { get; set; } = default!;
        public string Status { get; set; } = default!;

        public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    }
}


