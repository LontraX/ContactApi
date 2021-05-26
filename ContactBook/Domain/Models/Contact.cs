using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Domain.Models
{
    public class Contact : IdentityUser
    {
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }

        public string PhotoURL { get; set; }
        [Required]
        public Address Address { get; set; }

    }
}
