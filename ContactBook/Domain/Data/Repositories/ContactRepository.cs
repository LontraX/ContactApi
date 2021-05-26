using ContactBook.Domain.DTO;
using ContactBook.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Domain.Data.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private ContactDataContext _db;
        private UserManager<Contact> _usermanager;

        public ContactRepository(ContactDataContext contactDataContext,UserManager<Contact> userManager)
        {
            _db = contactDataContext;
            _usermanager = userManager;
           
        }
        
        public async Task<Contact> Create(ContactDto contact)
        {

            Contact contact1 = new Contact()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                UserName = contact.Email,
                Address = contact.Address,
                PhoneNumber = contact.PhoneNumber,
                PhotoURL = contact.PhotoURL,
                Id = contact.Id,
                
            };
            await _usermanager.CreateAsync(contact1,"mide");

           
           // _db.Add(contact1);
           // await _db.SaveChangesAsync();
           return contact1;
        }

        public async Task Delete(string id)
        {
            
            var profileToDelete = await _db.Contacts.Where(o => o.Id == id).FirstOrDefaultAsync();
            var deleteAddress = await _db.Addresses.Where(x => x.Id == ( profileToDelete.Address.Id)).FirstOrDefaultAsync();

            _db.Contacts.Remove(profileToDelete);
            _db.Addresses.Remove(deleteAddress);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactDto>> Get([FromQuery] PaginationFilter filter)
        {
            List<ContactDto> contactsToGet = new List<ContactDto>();
            
            var validPageFilter = new PaginationFilter( filter.CurrentPage);
            var users = await _db.Contacts.Include(x => x.Address)
                .Skip((validPageFilter.CurrentPage - 1) * 5)
                .Take(5).ToListAsync();




            var num = users.Count();
           
            for (int i = 0; i < num; i++)
            {
                ContactDto contactDto = new ContactDto();
                contactDto.Id = users[i].Id;
                contactDto.FirstName = users[i].FirstName;
                contactDto.LastName = users[i].LastName;
                contactDto.PhoneNumber = users[i].PhoneNumber;
                contactDto.Email = users[i].Email;
                contactDto.Address = users[i].Address;
                contactDto.PhotoURL = users[i].PhotoURL;
                contactsToGet.Add(contactDto);
            }
            
            return contactsToGet;
        }

        public async Task<ContactDto> GetByEmail(string email)
        {
           
            var userReturned = await _db.Contacts.Where(x => x.Email == email).Include(o => o.Address).FirstOrDefaultAsync();
            ContactDto contactDto = new ContactDto()
            {
                
                FirstName = userReturned.FirstName,
                LastName = userReturned.LastName,
                PhoneNumber = userReturned.PhoneNumber,
                PhotoURL = userReturned.PhotoURL,
                Address = userReturned.Address,
                Email = userReturned.Email,
                Id = userReturned.Id,
            };
            return contactDto;
        }

        public async Task<ContactDto> GetById(string id)
        {
            
            var userReturned = await _db.Contacts.Where(x => x.Id == id).Include(a => a.Address).FirstOrDefaultAsync();
            ContactDto contactDto = new ContactDto()
            {
                FirstName = userReturned.FirstName,
                LastName = userReturned.LastName,
                PhoneNumber = userReturned.PhoneNumber,
                PhotoURL = userReturned.PhotoURL,
                Address = userReturned.Address,
                Email = userReturned.Email,
                Id = userReturned.Id,
            };
            return contactDto;
        }

        public async Task Update(Contact contact)
        {
            _db.Entry(contact).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task UpdatePhoto(EditPhotoDto contact)
        {
            var getByid = await _db.Contacts.Where(x => x.Id == contact.id).Include(a => a.Address).FirstOrDefaultAsync();
            getByid.PhotoURL = contact.photourl;

            _db.Update(getByid);
            await _db.SaveChangesAsync();
        }
    }
}
