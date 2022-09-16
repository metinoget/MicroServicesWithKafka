using ContactMicroService.Entities.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.Bussiness.Abstract
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllContacts();
        Task<Contact> GetContactById(int id);
        Task<Contact> CreateContact(Contact contact);
        Task DeleteContact(Contact contact);
        Task UpdateContact(Contact contact);

        Task<ContactReport> GetReportData(string Location);
    }
}
