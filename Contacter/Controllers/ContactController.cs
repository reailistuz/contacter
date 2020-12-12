using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Contacter.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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
        //[Route("uploadFile")]
        //public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        public IActionResult addFile()
        {
            return RedirectToAction("/create");

            //try
            //{
            //    //nothing special in PhotoMultipartFormDataStreamProvider
            //    var provider = new PhotoMultipartFormDataStreamProvider(this.workingFolder);
            //    await request.Content.ReadAsMultipartAsync(provider);
            //    var photos = new List<PhotoViewModel>();
            //    foreach (var file in provider.FileData)
            //    {
            //        var fileInfo = new FileInfo(file.LocalFileName);
            //        photos.Add(new PhotoViewModel
            //        {
            //            Name = fileInfo.Name,
            //            Size = fileInfo.Length / 1024
            //        });
            //    }
            //    return photos;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error uploading file", ex);
            //}
        }

        //[HttpPost]
        //[Route("api/contacts/uploadFile")]
        //public IActionResult UploadJson()
        //{
        //    var webRequest = WebRequest.Create("https://localhost:44303/api/contacts") as HttpWebRequest;
        //    if (webRequest == null)
        //    {
        //        return BadRequest();
        //    }
        //    webRequest.ContentType = "application/json";
        //    webRequest.UserAgent = "Nothing";

        //    using (var s = webRequest.GetResponse().GetResponseStream())
        //    {
        //        using (var sr = new StreamReader(s))
        //        {
        //            var contributorsAsJson = sr.ReadToEnd();
        //            var contributors = JsonConvert.DeserializeObject<List<Contact>>(contributorsAsJson);
        //            using (var tw = new StreamWriter(@"D:\path.json", true))
        //            {
        //                tw.WriteLine(contributors.ToString());
        //                tw.Close();
        //            }
        //        }
        //    }
        //    return Ok();
        //}
    }

    [ApiController]
    [Route("api/uploadFile")]
    public class SomethingElse : Controller
    {
        ApplicationContext db;
        IWebHostEnvironment _appEnvironment;
        public SomethingElse(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }
        [HttpPost]
        public IActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "JSON");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    using (StreamReader r = new StreamReader(fullPath))
                    {
                        string json = r.ReadToEnd();
                        List<Contact> items = JsonConvert.DeserializeObject<List<Contact>>(json);
                        dynamic array = JsonConvert.DeserializeObject(json);
                        foreach (var item in array)
                        {
                            if(item != null)
                            {
                                string itemName = Convert.ToString(item.contactName);
                                string itemContact = Convert.ToString(item.contactInfo);
                                string itemNote = Convert.ToString(item.contactNote);
                                if (db.Contacts.Where(contact => contact.ContactName == itemName && contact.ContactInfo == itemContact).Count() == 0)
                                {
                                    db.Contacts.Add(new Contact { ContactName = itemName, ContactInfo = itemContact, ContactNote = itemNote });
                                    db.SaveChanges();
                                }
                            }
                            
                        }
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}