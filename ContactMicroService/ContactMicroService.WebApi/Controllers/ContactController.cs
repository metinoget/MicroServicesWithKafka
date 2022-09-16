using ContactMicroService.Api.Helpers;
using ContactMicroService.Bussiness.Abstract;
using ContactMicroService.Entities.Model;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ContactMicroService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpGet("[action]")]
        public async Task<Response<IEnumerable<Contact>>> GetAllData()
        {
            var contacts = await _contactService.GetAllContacts();
            return new Response<IEnumerable<Contact>>().Ok(contacts.Count(), contacts);
        }


        [HttpPost("[action]")]
        public async Task<Response<Contact>> Create(Contact contact)
        {
            try
            {
                if (contact.ContactId == 0)
                    await _contactService.CreateContact(contact);
                else await _contactService.UpdateContact(contact);
                return new Response<Contact>().Ok(1, contact);
            }
            catch (Exception ex)
            {
                return new Response<Contact>().Error(1, contact, ex.ToString());
            }
        }

        [HttpPost("[action]/{id}")]
        public async Task Delete(int id)
        {
            if (id != 0)
            {
                var deleteData = await _contactService.GetContactById(id);

                await _contactService.DeleteContact(deleteData);
            }


        }

        [HttpGet("[action]")]
        public async Task<ContactReport> GetContactReportByLocation(string Location)
        {
            return await _contactService.GetReportData(Location);

        }
    }
}
