using ContactMicroService.Bussiness.Abstract;
using ContactMicroService.Bussiness.Concrete;
using ContactMicroService.Entities.Model;
using ContactMicroService.Entities.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.Test.Services
{
    public class ContactInfoServiceTest
    {
        private readonly MockContactInfos _mockContactInfos;
        public ContactInfoServiceTest()
        {
            _mockContactInfos = new MockContactInfos();
        }
        [Fact]
        public async Task GetContactInfo_ListOfContactInfo_ContactInfoExistsInRepo()
        {
            var contactInfos = _mockContactInfos.GetContactInfos();
            var mockLogger = new Mock<ILogger<IContactInfoService>>();
            var mockRepo = new Mock<IContactInfoRepository>();
            mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(contactInfos);
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.ContactInfo.GetAllAsync()).Returns(mockRepo.Object.GetAllAsync());
            var contactInfoServ = new ContactInfoService(mockUoW.Object);
            var result = await contactInfoServ.GetAllContactInfos();
            var count = result.Count();
            mockRepo.Verify(x => x.GetAllAsync(), Times.Once);
            Assert.Equal(contactInfos.Count, count);
        }

        [Fact]
        public void GetContactInfo_GetById_ContactInfoExistInRepo()
        {

            var mockLogger = new Mock<ILogger<IContactInfoService>>();
            var mockRepo = new Mock<IContactInfoRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            int id = 1;

            mockUoW.Setup(x => x.ContactInfo.GetByIdAsync(id)).Returns(mockRepo.Object.GetByIdAsync(id));
            var contactInfoServ = new ContactInfoService(mockUoW.Object);
            var result = contactInfoServ.GetContactInfoById(id).Result;
            mockRepo.Verify(x => x.GetByIdAsync(id), Times.Once);
        }


        [Fact]
        public async Task CreateContactInfo_CreatedStatus_PassingContactInfoObjectToCreate()
        {
            var newContactInfo = new ContactInfo();
            newContactInfo.ContactInfoId = 1;
            var mockLogger = new Mock<ILogger<IContactInfoService>>();
            var mockRepo = new Mock<IContactInfoRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.ContactInfo.AddAsync(newContactInfo)).Returns(mockRepo.Object.AddAsync(newContactInfo));
            var contactInfoServ = new ContactInfoService(mockUoW.Object);
            var result = await contactInfoServ.CreateContactInfo(newContactInfo);
            mockRepo.Verify(x => x.AddAsync(newContactInfo), Times.Once);
        }

        [Fact]
        public void UpdateContactInfo_UpdatedStatus_PassingContactInfoObjectToUpdate()
        {

            var contactInfos = _mockContactInfos.GetContactInfos();
            var mockLogger = new Mock<ILogger<IContactInfoService>>();
            var mockRepo = new Mock<IContactInfoRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            var contactInfo = contactInfos[0];
            mockUoW.Setup(x => x.ContactInfo.Update(It.IsAny <ContactInfo>()));
            var contactInfoServ = new ContactInfoService(mockUoW.Object);
            var result = contactInfoServ.UpdateContactInfo(contactInfo);
            mockUoW.Verify(x => x.ContactInfo.Update(It.IsAny<ContactInfo>()), Times.Once);
        }

        [Fact]
        public async Task GetContactInfo_ListOfContactInfo_ContactInfoExistsInRepoIsNotDeleted()
        {
            var contactInfos = _mockContactInfos.GetContactInfos();
            var mockLogger = new Mock<ILogger<IContactInfoService>>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.ContactInfo.GetDeleteFilteredAll()).ReturnsAsync(contactInfos);
            var contactInfoServ = new ContactInfoService(mockUoW.Object);
            var result = await contactInfoServ.GetDeleteFilteredAllContactInfos();
            var count = result.Count();
            mockUoW.Verify(x => x.ContactInfo.GetDeleteFilteredAll(), Times.Once);
            Assert.Equal(contactInfos.Count, count);
        }
    }
}
