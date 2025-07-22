using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzdamarDepo.Domain.Abstractions
{
    public class AddressDto
    {
        public string ContactName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty; // Description olacak
        public string ZipCode { get; set; } = string.Empty;
    }
}
