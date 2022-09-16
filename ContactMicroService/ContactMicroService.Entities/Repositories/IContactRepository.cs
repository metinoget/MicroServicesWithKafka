using ContactMicroService.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.Entities.Repositories
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<IQueryable<Contact>> FindByContactInfo(Expression<Func<ContactInfo, bool>> predicate);
    }
}
