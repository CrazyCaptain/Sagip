using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Greenpeace_Advisory
{
    public sealed class Helper
    {
        public sealed class Constants
        {
            public static readonly string REQUEST_URL = "https://post.chikka.com/smsapi/request";
            public static readonly string MESSAGE_TYPE = "SEND";
            public static readonly string SHORTCODE = "";
            public static readonly string CLIENT_ID = "";
            public static readonly string SECRET_KEY = "";
        }

        public sealed class SendRequestFactory
        {
            public SendRequestFactory(string message)
            {
                this.Message = message;
            }

            public string Message { get; set; }

            public string ParameterString(string mobileNumber, int messageId)
            {
                return "message_type=" + Helper.Constants.MESSAGE_TYPE
                    + "&mobile_number=" + mobileNumber
                    + "&shortcode=" + Helper.Constants.SHORTCODE
                    + "&message_id=" + messageId
                    + "&message=" + Message
                    + "&client_id=" + Helper.Constants.CLIENT_ID
                    + "&secret_key=" + Helper.Constants.SECRET_KEY;
            }
            
        }

        public static string SendReplyFactory(string request_id, string message_id, string mobile_number)
        {
            string parameters = "message_type=REPLY"
                + "&mobile_number=" + mobile_number
                + "&shortcode=" + Constants.SHORTCODE
                + "&request_id=" + request_id
                + "&message_id=" + message_id
                + "&message=Your+feedback+has+been+logged."
                + "&request_cost=FREE"
                + "&client_id=" + Constants.CLIENT_ID
                + "&secret_key=" + Constants.SECRET_KEY;
            return parameters;
        }

        //public sealed class ReplyRequestFactory()
        //{
        //    public ReplyRequestFactory()
        //    {

        //    }

        //    public string ParameterString(string mobileNumber, int messageId)
        //    {
        //        return "message_type=" + Helper.Constants.MESSAGE_TYPE
        //            + "&mobile_number=" + mobileNumber
        //            + "&shortcode=" + Helper.Constants.SHORTCODE
        //            + "&message_id=" + messageId
        //            + "&message="
        //            + "&client_id=" + Helper.Constants.CLIENT_ID
        //            + "&secret_key=" + Helper.Constants.SECRET_KEY;
        //    }
        //}

    }
}