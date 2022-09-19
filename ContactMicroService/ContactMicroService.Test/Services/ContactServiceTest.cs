using Castle.Components.DictionaryAdapter;
using Castle.Core.Resource;
using ContactMicroService.Bussiness.Abstract;
using ContactMicroService.Bussiness.Concrete;
using ContactMicroService.DataAccess.Repositories;
using ContactMicroService.Entities.Enums;
using ContactMicroService.Entities.Model;
using ContactMicroService.Entities.Repositories;
using ContactMicroService.Helpers;
using ContactMicroService.WebApi.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContactMicroService.Test.Services
{
    public class ContactServiceTest
    {
        private readonly MockContacts _mockContacts;
        public ContactServiceTest()
        {
            _mockContacts = new MockContacts();
        }
        [Fact]
        public async Task GetContact_ListOfContact_ContactExistsInRepo()
        {
            var contacts = _mockContacts.GetContacts();
            var mockLogger = new Mock<ILogger<IContactService>>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.Contact.GetAllAsync()).ReturnsAsync(contacts);
            var contactServ = new ContactService(mockUoW.Object);
            var result = await contactServ.GetAllContacts();
            var count = result.Count();
            mockUoW.Verify(x => x.Contact.GetAllAsync(), Times.Once);
            Assert.Equal(contacts.Count, count);
        }

        [Fact]
        public async Task GetContact_ListOfContact_ContactExistsInRepoIsNotDeleted()
        {
            var contacts = _mockContacts.GetContacts();
            var mockLogger = new Mock<ILogger<IContactService>>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.Contact.GetDeleteFilteredAll()).ReturnsAsync(contacts);
            var contactServ = new ContactService(mockUoW.Object);
            var result = await contactServ.GetDeleteFilteredAllContacts();
            var count = result.Count();
            mockUoW.Verify(x => x.Contact.GetDeleteFilteredAll(), Times.Once);
            Assert.Equal(contacts.Count, count);
        }

        [Fact]
        public void GetContact_GetById_ContactExistInRepo()
        {

            var mockLogger = new Mock<ILogger<IContactService>>();
            var mockRepo = new Mock<IContactRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            int id = 1;

            mockUoW.Setup(x => x.Contact.GetByIdAsync(id)).Returns(mockRepo.Object.GetByIdAsync(id));
            var contactServ = new ContactService(mockUoW.Object);
            var result = contactServ.GetContactById(id).Result;
            mockRepo.Verify(x => x.GetByIdAsync(id), Times.Once);
        }


        [Fact]
        public async Task CreateContact_CreatedStatus_PassingContactObjectToCreate()
        {
            var newContact = new Contact();
            newContact.ContactId = 1;
            var mockLogger = new Mock<ILogger<IContactService>>();
            var mockRepo = new Mock<IContactRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.Contact.AddAsync(newContact)).Returns(mockRepo.Object.AddAsync(newContact));
            var contactServ = new ContactService(mockUoW.Object);
            var result = await contactServ.CreateContact(newContact);
            mockRepo.Verify(x => x.AddAsync(newContact), Times.Once);
        }

        [Fact]
        public void UpdateContact_UpdatedStatus_PassingContactObjectToUpdate()
        {

            var contacts = _mockContacts.GetContacts();
            var mockLogger = new Mock<ILogger<IContactService>>();
            var mockRepo = new Mock<IContactRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            var contact = contacts[0];
            mockUoW.Setup(x => x.Contact.Update(contact));
            var contactServ = new ContactService(mockUoW.Object);
            var result = contactServ.UpdateContact(contact);
            mockUoW.Verify(x => x.Contact.Update(contact), Times.Once);
        }

        [Fact]
        public void GetContactReportByLocation_ContactReportObject_ContactwithSpecifiedInfoLocation()
        {
            var contacts = _mockContacts.GetContacts();
            var mockLogger = new Mock<ILogger<IContactService>>();
            var mockUoW = new Mock<IUnitOfWork>();
            var filteredContact = new List<Contact> { contacts[0], contacts[1] };
            var query = filteredContact.AsQueryable();

            string Location = "Hatay";
            var predicate = PredicateBuilder.True<ContactInfo>();

            predicate = predicate.And(i => i.ContactType == ContactType.Location);
            if (!string.IsNullOrWhiteSpace(Location))
            {
                predicate = predicate.And(i => i.Information == Location);
            }

            var reportData = new ContactReport { Location = Location, NearbyPeopleCount = 2, NearbySavedPhoneCount = 1 };

            mockUoW.Setup(x => x.Contact.FindByContactInfo(It.IsAny<Expression<Func<ContactInfo,bool>>>())).ReturnsAsync(query);
            var contactServ = new ContactService(mockUoW.Object);
            var result = contactServ.GetReportData(Location).Result;
            var serializedResult= JsonConvert.SerializeObject(result);
            var serializedReportData = JsonConvert.SerializeObject(reportData);
            Assert.Equal(serializedReportData,serializedResult);
        }
    }
}
