using ReportMicroService.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReportServiceDbContext _context;
        private ReportRepository? _reportRepository;

        public UnitOfWork(ReportServiceDbContext context)
        {
            _context = context;
        }

        public IReportRepository Report => _reportRepository = _reportRepository ?? new ReportRepository(_context);

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
