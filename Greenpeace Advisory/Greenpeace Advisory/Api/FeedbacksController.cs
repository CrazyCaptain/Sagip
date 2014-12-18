using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Greenpeace_Advisory.Models;
using System.Web;

namespace Greenpeace_Advisory.Api
{
    public class FeedbacksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: api/Feedbacks
        [ResponseType(typeof(Feedback))]
        public HttpResponseMessage PostFeedback(FeedbackAPIModel feedback)
        {
            //db.feedTest.Add(feedback);
            //db.SaveChanges();

            if (!ModelState.IsValid || feedback.shortcode != Helper.Constants.SHORTCODE)
            {
                throw new HttpException(404, "Error");
            }

            //if (!db.Feedback.Where(m => m.RequestId == feedback.request_id).Equals(null))
            //{
            //    return Request.CreateResponse(HttpStatusCode.Accepted);
            //}
            
            Feedback f = new Feedback();
            f.RequestId = feedback.request_id;
            f.Message = feedback.message;
            f.MobileNumber = feedback.mobile_number;
            f.TimeStamp = DateTime.Now.AddHours(8);

            db.Feedback.Add(f);
            db.SaveChanges();

            ReplyToSMS(f.RequestId.ToString(), f.FeedbackId.ToString(), f.MobileNumber);

            return Request.CreateResponse(HttpStatusCode.Accepted);

            //return CreatedAtRoute("DefaultApi", new { id = f.FeedbackId }, feedback);
        }

        private DateTime FromUnixTime(double unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        // Reply Test
        private void ReplyToSMS(string request_id, string message_id, string mobile_number)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.UploadString(Helper.Constants.REQUEST_URL, Helper.SendReplyFactory(request_id, message_id, mobile_number));
                    //r.Status = "Sent";

                }
                catch (WebException)
                {
                    //r.Status = "Failed";
                }
                finally
                {

                    //db.SaveChanges();
                }
            }
        }

    }
}