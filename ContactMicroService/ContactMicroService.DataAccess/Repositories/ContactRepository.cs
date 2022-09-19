using ContactMicroService.Entities.Model;
using ContactMicroService.Entities.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.DataAccess.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(ContactServiceDBContext context):base(context)
        {
        }

        public async Task<IQueryable<Contact>> FindByContactInfo(Expression<Func<ContactInfo, bool>> predicate)
        {
            return _context.Contacts.Where(i=>i.ContactInfos.AsQueryable().Any(predicate));
        }

        public async Task<IEnumerable<Contact>> GetDeleteFilteredAll()
        {
            return await _context.Contacts.Where(c => c.IsDeleted == false).ToListAsync();
        }
    }
}
