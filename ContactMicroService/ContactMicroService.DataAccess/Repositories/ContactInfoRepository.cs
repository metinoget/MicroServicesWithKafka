using ContactMicroService.Entities.Model;
using ContactMicroService.Entities.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.DataAccess.Repositories
{
    public class ContactInfoRepository : Repository<ContactInfo>,IContactInfoRepository
    {
    
        public ContactInfoRepository(ContactServiceDBContext context) : base(context)
        {

        }
        public async Task<IEnumerable<ContactInfo>> GetDeleteFilteredAll()
        {
            return await _context.ContactInfos.Where(c => c.IsDeleted == false).ToListAsync();
        }
    }
}
