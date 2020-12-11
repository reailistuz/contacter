using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Contacter.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace HelloAngularApp.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContractController : Controller
    {
        ApplicationContext db;
        IWebHostEnvironment _appEnvironment;
        public ContractController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
            if (!db.Contacts.Any())
            {
                db.Contacts.Add(new Contact { ContactName = "Apple", ContactInfo = "8008009004", ContactNote = "Apple US" });
                db.Contacts.Add(new Contact { ContactName = "Google", ContactInfo = "8005894560", ContactNote = "Google US" });
                db.Contacts.Add(new Contact { ContactName = "Samsung", ContactInfo = "8009563254", ContactNote = "Samsung US" });
                db.SaveChanges();
            }
        }
        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            return db.Contacts.ToList();
        }

        [HttpGet("{id}")]
        public Contact Get(int id)
        {
            Contact contact = db.Contacts.FirstOrDefault(x => x.ContactID == id);
            return contact;
        }

        [HttpPost]
        public IActionResult Post(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return Ok(contact);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Update(contact);
                db.SaveChanges();
                return Ok(contact);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Contact contact = db.Contacts.FirstOrDefault(x => x.ContactID == id);
            if (contact != null)
            {
                db.Contacts.Remove(contact);
                db.SaveChanges();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                Contact file = new Contact { ContactName = uploadedFile.FileName, ContactNote = path };
                db.Add(file);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}