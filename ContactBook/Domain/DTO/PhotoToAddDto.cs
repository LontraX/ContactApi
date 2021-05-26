using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Domain.DTO
{
    public class PhotoToAddDto
    {
        public IFormFile PhotoFile { get; set; }

        //public DateTime DateCreated { get; set; }


    }
}
