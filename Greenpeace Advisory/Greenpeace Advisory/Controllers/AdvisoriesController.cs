using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Greenpeace_Advisory.Models;
using Greenpeace_Advisory;

namespace Greenpeace_Advisory.Controllers
{
    public class AdvisoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Advisories
        public async Task<ActionResult> Index()
        {
            return View(await db.Advisory.OrderByDescending(m => m.TimeStamp).ToListAsync());
        }

        // GET: Advisories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advisory advisory = await db.Advisory.FindAsync(id);
            if (advisory == null)
            {
                return HttpNotFound();
            }

            ViewBag.Advisory = advisory.Message;
            IEnumerable<Recipient> list = db.Recipients.Where(m => m.AdvisoryId == id);

            //List<Recipient> RecipientList = db.Recipients.Where(m => m.AdvisoryId == id).ToList();
            //List<ContactDetail> contactList = db.ContactDetails.ToList();
            //List<RecipientViewModel> list = new List<RecipientViewModel>();

            //foreach (var item in RecipientList)
            //{
            //    RecipientViewModel r = new RecipientViewModel();
            //    r.Status = item.Status;
            //    r.ContactNumber = contactList.Find(m => m.MobileNumber == item.ContactNumber).MobileNumber;
            //    Farmer f = contactList.Find(m => m.ContactDetailId == item.ContactId).Farmer;
            //    string name = f.LastName + ", " + f.FirstName + " " + f.MiddleName[0] + ".";
            //    r.FarmerName = name;
            //    list.Add(r);
            //}

            return View(list);
        }

        // GET: Advisories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Advisories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AdvisoryId,Message")] Advisory advisory)
        {
            advisory.TimeStamp = DateTime.Now.AddHours(8);
            advisory.Username = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Advisory.Add(advisory);
                await db.SaveChangesAsync();
                //advisory = db.Advisory.OrderByDescending(m => m.AdvisoryId).First();
                
                Helper.SendRequestFactory send = new Helper.SendRequestFactory(advisory.Message);
                List<ContactDetail> contactList = db.ContactDetails.ToList();
                foreach (var item in contactList)
                {
                    Recipient r = new Recipient();
                    r.AdvisoryId = advisory.AdvisoryId;
                    r.ContactNumber = item.MobileNumber;
                    r.Name = item.Farmer.LastName + ", " + item.Farmer.FirstName;
                    db.Recipients.Add(r);
                    db.SaveChanges();
                    //r = db.Recipients.OrderByDescending(m => m.RecipientId).First();
                    
                    using (WebClient wc = new WebClient())
                    {
                        try
                        {
                            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                            wc.UploadString(Helper.Constants.REQUEST_URL, send.ParameterString(item.MobileNumber, r.RecipientId));
                            r.Status = "Sent";

                        }
                        catch (WebException)
                        {
                            r.Status = "Failed";
                        }
                        finally
                        {
                            db.Entry(r).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }

            return View(advisory);
        }   


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
