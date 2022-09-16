using ReportMicroService.Entities.Model;
using ReportMicroService.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.DataAccess.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(ReportServiceDbContext context) : base(context)
        {
        }
    }
}
