using ContactMicroService.Api.Helpers;
using ContactMicroService.Bussiness.Abstract;
using ContactMicroService.Bussiness.Concrete;
using ContactMicroService.Entities.Model;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ContactMicroService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoService _contactInfoService;
        private readonly ILogger<ContactInfoController> _logger;
        public ContactInfoController(IContactInfoService contactInfoService, ILogger<ContactInfoController> logger)
        {
            _contactInfoService = contactInfoService;
            _logger = logger;
        }
        [HttpGet("[action]")]
        public async Task<Response<IEnumerable<ContactInfo>>> GetAllData()
        {
            try
            {
                var infos = await _contactInfoService.GetAllContactInfos();
                return new Response<IEnumerable<ContactInfo>>().Ok(infos.Count(), infos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<IEnumerable<ContactInfo>>().NotFound("Contact info cannot found.");
            }
        }

        [HttpGet("[action]")]
        public async Task<Response<IEnumerable<ContactInfo>>> GetDeleteFilteredAllData()
        {
            try
            {
                var contactInfos = await _contactInfoService.GetDeleteFilteredAllContactInfos();
                return new Response<IEnumerable<ContactInfo>>().Ok(contactInfos.Count(), contactInfos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<IEnumerable<ContactInfo>>().NotFound("ContactInfo cannot found.");
            }
        }


        [HttpPost("[action]/{id}")]
        public async Task<Response<ContactInfo>> GetById(int id)
        {
            try
            {
                if (id != 0)
                {
                    var contactInfo= await _contactInfoService.GetContactInfoById(id);
                    if (contactInfo != null)
                        return new Response<ContactInfo>().Ok(1, contactInfo);
                    else return new Response<ContactInfo>().NotFound("Contact info cannot found.");
                }
                else
                    return new Response<ContactInfo>().NotFound("Contact info cannot found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<ContactInfo>().NotFound("Unhandled Exception.");
            }

        }

        [HttpPost("[action]")]
        public async Task<Response<ContactInfo>> CreateOrUpdate(ContactInfo info)
        {
            try
            {
                if (info.ContactInfoId == 0)
                    await _contactInfoService.CreateContactInfo(info);
                else await _contactInfoService.UpdateContactInfo(info);
                return new Response<ContactInfo>().Ok(1, info);
            }
            catch (Exception ex)
            {
                return new Response<ContactInfo>().Error(1, info, ex.ToString());
            }
        }

        [HttpPost("[action]/{id}")]
        public async Task<Response<Contact>> Delete(int id)
        {
            try
            {
                if (id != 0)
                {
                    var deleteData = await _contactInfoService.GetContactInfoById(id);
                    deleteData.IsDeleted = true;
                    await _contactInfoService.UpdateContactInfo(deleteData);
                    return new Response<Contact>().Ok(1, null);
                }

                return new Response<Contact>().NotFound("Contact info can not found");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<Contact>().NotFound( "Delete contact info failed");
            }
        }


    }
}
