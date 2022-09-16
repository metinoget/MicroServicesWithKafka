using ContactMicroService.Bussiness.Abstract;
using ContactMicroService.Entities.Enums;
using ContactMicroService.Entities.Model;
using ContactMicroService.Entities.Repositories;
using ContactMicroService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.Bussiness.Concrete
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Contact> CreateContact(Contact contact)
        {
            await _unitOfWork.Contact.AddAsync(contact);
            await _unitOfWork.CommitAsync();
            return contact;
        }

        public async Task DeleteContact(Contact contact)
        {
             _unitOfWork.Contact.Remove(contact);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            return await _unitOfWork.Contact.GetAllAsync();
        }

        public async Task<Contact> GetContactById(int id)
        {
            return await _unitOfWork.Contact.GetByIdAsync(id);
        }

        public async Task UpdateContact(Contact contact)
        {
            _unitOfWork.Contact.Update(contact);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ContactReport> GetReportData(string Location)
        {

            var predicate = PredicateBuilder.True<ContactInfo>();

            predicate = predicate.And(i => i.ContactType == ContactType.Location);
            if (!string.IsNullOrWhiteSpace(Location))
            {
                predicate = predicate.And(i => i.Information == Location);
            }

            var contacts = await _unitOfWork.Contact.FindByContactInfo(predicate);

            int peopleCount = contacts.Distinct().Count();


            var predicate2 = PredicateBuilder.True<ContactInfo>();
            predicate2 = predicate2.And(i => i.ContactType == ContactType.PhoneNumber);


            var resultContacts=contacts.Where(i => i.ContactInfos.AsQueryable().Any(predicate2));

            //var phoneCount = queryable
            //    .Where(i => i.ContactInfos.AsQueryable().Any(predicate)).Where(i => i.ContactInfos.AsQueryable().Any(predicate2))
            //    .Select(i => i);
            var phoneCount = resultContacts.Count();

            var reportDto = new ContactReport
            {
                Location = Location,
                NearbyPeopleCount = peopleCount,
                NearbySavedPhoneCount = phoneCount
            };


            return reportDto;
        }
    }
}
