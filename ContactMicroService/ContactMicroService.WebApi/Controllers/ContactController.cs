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
        private readonly ILogger<ContactController> _logger;
        public ContactController(IContactService contactService, ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }
        [HttpGet("[action]")]
        public async Task<Response<IEnumerable<Contact>>> GetAllData()
        {
            try
            {
                var contacts = await _contactService.GetAllContacts();
                return new Response<IEnumerable<Contact>>().Ok(contacts.Count(), contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<IEnumerable<Contact>>().NotFound("Contact cannot found.");
            }
        }

        [HttpPost("[action]/{id}")]
        public async Task<Response<Contact>> GetById(int id)
        {
            try
            {

                if (id != 0)
                {
                    var contact = await _contactService.GetContactById(id);
                    if (contact != null)
                        return new Response<Contact>().Ok(1, contact);
                    else return new Response<Contact>().NotFound("Contact cannot found.");
                }
                else
                    return new Response<Contact>().NotFound("Contact cannot found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<Contact>().NotFound("Unhandled Exception.");
            }

        }


        [HttpPost("[action]")]
        public async Task<Response<Contact>> CreateOrUpdate(Contact contact)
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
            try
            {
                if (id != 0)
                {
                    var deleteData = await _contactService.GetContactById(id);

                    await _contactService.DeleteContact(deleteData);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }


        }

        [HttpGet("[action]")]
        public async Task<ContactReport> GetContactReportByLocation(string Location)
        {
            return await _contactService.GetReportData(Location);

        }
    }
}
