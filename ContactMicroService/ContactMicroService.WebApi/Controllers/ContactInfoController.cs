using ContactMicroService.Api.Helpers;
using ContactMicroService.Bussiness.Abstract;
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
        public ContactInfoController(IContactInfoService contactInfoService)
        {
            _contactInfoService = contactInfoService;
        }
        [HttpGet("[action]")]
        public async Task<Response<IEnumerable<ContactInfo>>> GetAllData()
        {
            var infos = await _contactInfoService.GetAllContactInfos();
            return new Response<IEnumerable<ContactInfo>>().Ok(infos.Count(), infos);
        }


        [HttpPost("[action]")]
        public async Task<Response<ContactInfo>> Create(ContactInfo info)
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
        public async Task Delete(int id)
        {
            if (id != 0)
            {
                var deleteData = await _contactInfoService.GetContactInfoById(id);

                await _contactInfoService.DeleteContactInfo(deleteData);
            }
        }
       
      
    }
}
