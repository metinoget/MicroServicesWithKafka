using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.Entities.Repositories
{
    public interface IUnitOfWork :IDisposable
    {
        IContactRepository Contact { get; }   
        IContactInfoRepository ContactInfo { get; }
        Task<int> CommitAsync();
    }
}
