using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Entities.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IReportRepository Report { get; }
        Task<int> CommitAsync();
    }
}
