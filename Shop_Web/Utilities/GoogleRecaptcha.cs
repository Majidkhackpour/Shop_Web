using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using PacketParser.Services;
using Shop_Web.Models;

namespace Shop_Web.Utilities
{
    public static class GoogleRecaptcha
    {
        public static bool Authentication(FormCollection form)
        {
            try
            {
                var urlToPost = "https://www.google.com/recaptcha/api/siteverify";
                var secretKey = "6Lca4fQUAAAAAPd_RdqtlT0oMe7pQANLFc4jsRex"; // change this
                var gRecaptchaResponse = form["g-recaptcha-response"];

                var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

                // send post data
                var request = (HttpWebRequest)WebRequest.Create(urlToPost);
                request.Method = "POST";
                request.ContentLength = postData.Length;
                request.ContentType = "application/x-www-form-urlencoded";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                }

                // receive the response now
                string result;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                }

                // validate the response from Google reCaptcha
                var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
                return captChaesponse.Success;
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                return false;
            }
        }
    }
}