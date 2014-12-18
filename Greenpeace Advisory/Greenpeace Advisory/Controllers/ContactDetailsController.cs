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

namespace Greenpeace_Advisory.Controllers
{
    public class ContactDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContactDetails/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farmer farmer = db.Farmers.Find(id);
            if (farmer == null)
            {
                return HttpNotFound();
            }

            ContactDetail contactDetail = new ContactDetail();
            contactDetail.FarmerId = farmer.FarmerID;

            return View(contactDetail);
        }

        // POST: ContactDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MobileNumber,FarmerId")] ContactDetail contactDetail)
        {
            contactDetail.MobileNumber = contactDetail.MobileNumber.Trim();
            if (contactDetail.MobileNumber.Length != 12)
            {
                if (contactDetail.MobileNumber.Length == 11 && contactDetail.MobileNumber.StartsWith("09"))
                {
                    contactDetail.MobileNumber = "639" + contactDetail.MobileNumber.Remove(0, 2);
                }
                else if (contactDetail.MobileNumber.Length == 13 && contactDetail.MobileNumber.StartsWith("+639"))
                {
                    contactDetail.MobileNumber = "639" + contactDetail.MobileNumber.Remove(0, 4);
                }
                else
                {
                    ViewBag.Warning = "Number is not valid. Please input an 11 digit phone number starting with 09 or a 12 digit phone number starting with 639";
                    return View(contactDetail);
                }
            }

            if (db.ContactDetails.Any(m => m.MobileNumber == contactDetail.MobileNumber))
            {
                var contact = db.ContactDetails.Where(m => m.MobileNumber == contactDetail.MobileNumber).First();
                ViewBag.Contact = contact.Farmer.LastName + ", " + contact.Farmer.FirstName;
                ViewBag.ContactFarmerId = contact.FarmerId.ToString();
                return View(contactDetail);
            }

            if (ModelState.IsValid)
            {
                db.ContactDetails.Add(contactDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Farmers", new { id = contactDetail.FarmerId });
            }

            return View(contactDetail);
        }

        // GET: ContactDetails/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactDetail contactDetail = await db.ContactDetails.FindAsync(id);
            if (contactDetail == null)
            {
                return HttpNotFound();
            }

            return View(contactDetail);
        }

        // POST: ContactDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ContactDetailId,MobileNumber,FarmerId")] ContactDetail contactDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Farmers", new { id = contactDetail.FarmerId });
            }
            
            return View(contactDetail);
        }

        // GET: ContactDetails/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactDetail contactDetail = await db.ContactDetails.FindAsync(id);
            if (contactDetail == null)
            {
                return HttpNotFound();
            }
            return View(contactDetail);
        }

        // POST: ContactDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ContactDetail contactDetail = await db.ContactDetails.FindAsync(id);
            db.ContactDetails.Remove(contactDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Farmers", new { id = contactDetail.FarmerId });
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
