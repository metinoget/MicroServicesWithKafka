using ContactMicroService.Bussiness.Abstract;
using ContactMicroService.Entities.Model;
using ContactMicroService.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.Bussiness.Concrete
{
    public class ContactInfoService : IContactInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContactInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ContactInfo> CreateContactInfo(ContactInfo info)
        {
            await _unitOfWork.ContactInfo.AddAsync(info);
            await _unitOfWork.CommitAsync();
            return info;
        }

        public async Task DeleteContactInfo(ContactInfo info)
        {
            _unitOfWork.ContactInfo.Remove(info);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ContactInfo>> GetAllContactInfos()
        {
            return await _unitOfWork.ContactInfo.GetAllAsync();
        }

        public async Task<ContactInfo> GetContactInfoById(int id)
        {
            return await _unitOfWork.ContactInfo.GetByIdAsync(id);
        }

        public async Task UpdateContactInfo(ContactInfo info)
        {
            _unitOfWork.ContactInfo.Update(info);
            await _unitOfWork.CommitAsync();
        }
    }
}
