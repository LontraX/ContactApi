using ContactBook.Domain.DTO;
using ContactBook.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Domain.Data.Repositories
{
    public interface IContactRepository
    {
        public Task<IEnumerable<ContactDto>> Get([FromQuery] PaginationFilter filter);

        public Task<ContactDto> GetById(string id);

        public Task<ContactDto> GetByEmail(string email);

        public Task<Contact> Create(ContactDto contact);

        public Task Update(Contact contact);
        public Task UpdatePhoto(EditPhotoDto contact);
        public Task Delete(string id);
    }
}
