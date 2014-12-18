using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Greenpeace_Advisory.Models;

namespace Greenpeace_Advisory.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: Feedback
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<Feedback> list = db.Feedback.OrderByDescending(m => m.TimeStamp);
            return View(list);
        }
    }
}