using ContactBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Domain.DTO
{
    public class ContactDto
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; } 
        public string PhotoURL { get; set; }

        public Address Address { get; set; }
    }
}
