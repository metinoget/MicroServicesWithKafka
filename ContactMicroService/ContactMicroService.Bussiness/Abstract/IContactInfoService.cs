
using ContactMicroService.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.Bussiness.Abstract
{
    public interface IContactInfoService
    {
        Task<IEnumerable<ContactInfo>> GetAllContactInfos();

        Task<IEnumerable<ContactInfo>> GetDeleteFilteredAllContactInfos();
        Task<ContactInfo> GetContactInfoById(int id);
        Task<ContactInfo> CreateContactInfo(ContactInfo info);
        Task UpdateContactInfo(ContactInfo info);
    }
}
