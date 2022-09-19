using Autofac.Core;
using Castle.Core.Logging;
using ContactMicroService.Bussiness.Abstract;
using ContactMicroService.Bussiness.Concrete;
using ContactMicroService.Entities.Model;
using ContactMicroService.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ContactMicroService.Test.Controllers
{
    public class ContactInfoControllerTests
    {
        private readonly Mock<IContactInfoService> _contactInfoService;
        private readonly Mock<ILogger<ContactInfoController>> logger;
        private readonly MockContactInfos _mockContactInfo;
        public ContactInfoControllerTests()
        {
            _contactInfoService = new Mock<IContactInfoService>();
            logger = new Mock<ILogger<ContactInfoController>>();
            _mockContactInfo = new MockContactInfos();
        }


        [Fact]
        public void GetContactInfo_ListOfContactInfo_ContactInfoExistsInRepo()
        {
            var contactInfos = _mockContactInfo.GetContactInfos();
            _contactInfoService.Setup(x => x.GetAllContactInfos())
                .ReturnsAsync(contactInfos);
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);

            var actionResult = controller.GetAllData();
            var result = actionResult.Result;
            var statusCode = result.Code;
            var actual = result.Data as IEnumerable<ContactInfo>;


            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(contactInfos.Count(), actual.Count());
        }
        [Fact]
        public void GetContactInfo_ListOfContactInfo_ContactInfoIfNotExistsInRepo()
        {
            _contactInfoService.Setup(x => x.GetAllContactInfos())
                .ReturnsAsync((List<ContactInfo>)null);
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);

            var actionResult = controller.GetAllData();
            var result = actionResult.Result;
            var statusCode = result.Code;
            var actual = result.Data as IEnumerable<ContactInfo>;

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void GetContactInfo_ListOfContactInfo_ContactInfoIfNotExistsInRepoIsNotDeleted()
        {
            _contactInfoService.Setup(x => x.GetDeleteFilteredAllContactInfos())
                .ReturnsAsync((List<ContactInfo>)null);
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);

            var actionResult = controller.GetDeleteFilteredAllData();
            var result = actionResult.Result;
            var statusCode = result.Code;
            var actual = result.Data as IEnumerable<ContactInfo>;

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }


        [Fact]
        public void GetContactInfoById_ContactInfoObject_ContactInfowithSpecificeIdExists()
        {
            var contactInfos = _mockContactInfo.GetContactInfos();
            var expected = contactInfos[0];
            _contactInfoService.Setup(x => x.GetContactInfoById(1).Result)
                .Returns(expected);
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);


            var actionResult = controller.GetById(1);
            var result = actionResult.Result;
            var statusCode = result.Code;
            var actual = result.Data as ContactInfo;

            Assert.IsType<HttpStatusCode>(statusCode);
            Assert.Equal(HttpStatusCode.OK, statusCode);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetContactInfoById_shouldReturnBadRequest_ContactInfowithSpecificeIdNotExists()
        {
            _contactInfoService.Setup(x => x.GetContactInfoById(1))
                .ReturnsAsync((ContactInfo)null);
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);

            var actionResult = controller.GetById(1);
            var result = actionResult.Result;
            var statusCode = result.Code;

            Assert.IsType<HttpStatusCode>(statusCode);
            Assert.Equal(HttpStatusCode.NotFound, statusCode);

        }
        [Fact]
        public void GetContactInfoById_shouldReturnBadRequest_SendZeroAsContactInfoId()
        {
            var contactInfo = (_mockContactInfo.GetContactInfos())[0];
            _contactInfoService.Setup(x => x.GetContactInfoById(It.IsAny<int>()))
                .ReturnsAsync(contactInfo);
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);

            var actionResult = controller.GetById(0);
            var result = actionResult.Result;
            var statusCode = result.Code;

            Assert.IsType<HttpStatusCode>(statusCode);
            Assert.Equal(HttpStatusCode.NotFound, statusCode);

        }

        [Fact]
        public void CreateContactInfo_CreatedStatus_PassingContactInfoObjectToCreate()
        {
            var contactInfos = _mockContactInfo.GetContactInfos();
            var newContactInfo = contactInfos[0];
            newContactInfo.ContactInfoId = 0;
            _contactInfoService.Setup(x => x.CreateContactInfo(It.IsAny<ContactInfo>()));
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);
            var actionResult = controller.CreateOrUpdate(newContactInfo);
            var result = actionResult.Result;
            Assert.IsType<HttpStatusCode>(result.Code);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            _contactInfoService.Verify(x => x.CreateContactInfo(newContactInfo), Times.Once);
        }


        [Fact]
        public void UpdateContactInfo_UpdatedStatus_PassingContactInfoObjectToUpdate()
        {
            var contactInfos = _mockContactInfo.GetContactInfos();
            var newContactInfo = contactInfos[0];
            _contactInfoService.Setup(x => x.UpdateContactInfo(It.IsAny<ContactInfo>()));
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);
            var actionResult = controller.CreateOrUpdate(newContactInfo);
            var result = actionResult.Result;
            Assert.IsType<HttpStatusCode>(result.Code);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            _contactInfoService.Verify(x => x.UpdateContactInfo(newContactInfo), Times.Once);

        }

        [Fact]
        public void DeleteContactInfo_DeletedStatus_PassingContactInfoIdToDelete()
        {
            var contactInfos = _mockContactInfo.GetContactInfos();
            var deleteContactInfo = contactInfos[0];
            _contactInfoService.Setup(x => x.GetContactInfoById(deleteContactInfo.ContactInfoId)).ReturnsAsync(deleteContactInfo);
            _contactInfoService.Setup(x => x.UpdateContactInfo(It.IsAny<ContactInfo>()));
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);
            var actionResult = controller.Delete(deleteContactInfo.ContactInfoId);
            var result = actionResult.Result;
            _contactInfoService.Verify(x => x.UpdateContactInfo(It.IsAny<ContactInfo>()), Times.Once);
            Assert.Equal(HttpStatusCode.OK, result.Code);

        }

        [Fact]
        public void DeleteContactInfo_DeletedStatusFailed_PassingContactInfoIdToDelete()
        {
            var contactInfos = _mockContactInfo.GetContactInfos();
            var deleteContactInfo = contactInfos[0];
            _contactInfoService.Setup(x => x.GetContactInfoById(deleteContactInfo.ContactInfoId)).ReturnsAsync((ContactInfo)null);
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);
            var actionResult = controller.Delete(deleteContactInfo.ContactInfoId);
            var result = actionResult.Result;

            Assert.Equal(HttpStatusCode.NotFound, result.Code);

        }

        [Fact]
        public void DeleteContactInfo_DeletedStatusFailed_PassingZeroToDelete()
        {
            var contactInfos = _mockContactInfo.GetContactInfos();
            var deleteContactInfo = contactInfos[0];
            _contactInfoService.Setup(x => x.GetContactInfoById(It.IsAny<int>())).ReturnsAsync((ContactInfo)null);
            var controller = new ContactInfoController(_contactInfoService.Object, logger.Object);
            var actionResult = controller.Delete(0);
            var result = actionResult.Result;

            Assert.Equal(HttpStatusCode.NotFound, result.Code);

        }
    }
}
