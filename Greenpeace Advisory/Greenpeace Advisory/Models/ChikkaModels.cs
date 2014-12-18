using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Greenpeace_Advisory.Models
{
  
    public class Recipient
    {
        public int RecipientId { get; set; }
        public string Status { get; set; }
        public string ContactNumber { get; set; }
        public string Name { get; set; }
        public int AdvisoryId { get; set; }

        public virtual Advisory Advisory { get; set; }
    }

    public class Advisory
    {
        public int AdvisoryId { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Username { get; set; }
    }

    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string RequestId { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public string MobileNumber { get; set; }
    }

    public class FeedbackAPIModel
    {
        //public int Id { get; set; }
        public string message_type { get; set; }
        public string request_id { get; set; }
        public string shortcode { get; set; }
        public string message { get; set; }
        public string mobile_number { get; set; }
        public double timestamp { get; set; }
    }

}