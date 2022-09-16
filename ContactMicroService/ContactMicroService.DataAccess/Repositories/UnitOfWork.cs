using ContactMicroService.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContactServiceDBContext _context;
        private ContactRepository? _contactRepository;
        private ContactInfoRepository? _contactInfoRepository;
        public UnitOfWork(ContactServiceDBContext context)
        {
            _context = context;
        }

        public IContactRepository Contact => _contactRepository = _contactRepository ?? new ContactRepository(_context);

        public IContactInfoRepository ContactInfo => _contactInfoRepository = _contactInfoRepository ?? new ContactInfoRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
