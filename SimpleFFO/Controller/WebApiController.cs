using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Linq;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimpleFFO.Controller
{
    public class WebApiController : ApiController
    {
        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// POST api/<controller>
        //public void PostToken([FromBody] string username, [FromBody] string password, [FromBody] string deviceType, [FromBody] string deviceToken)
        //{

        //}// POST api/<controller>

        ////private void initFireBaseSDK()
        ////{
        ////    var credential = GoogleCredential.FromFile("~/Content/sfadm-66075-6360e2170a59.json");
        ////    FirebaseApp.Create(new AppOptions() { 
        ////        Credential = credential
        ////    });
        ////}
        //public void SendNotification([FromBody] string jsondata)
        //{
        //    //JObject o = JObject.Parse(jsondata);
        //    //string deviceToken = (string)o["deviceToken"];
        //    //string deviceType = (string)o["deviceType"];
        //    //string title = (string)o["title"];
        //    //string body = (string)o["body"];
        //    //if (deviceToken == "")
        //    //    return;

        //    //if(deviceType == "android")
        //    //{
        //    //    initFireBaseSDK();
        //    //    var message = new MulticastMessage()
        //    //    {
        //    //        Tokens = new List<string>{ deviceToken},
        //    //        Data = new Dictionary<string, string>()
        //    //         {
        //    //         {"title", title},
        //    //         {"body", body},
        //    //         },
        //    //    };
        //    //    await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(true);
        //    //}
        //    //var result = "-1";
        //    //var httpWebRequest = (HttpWebRequest)WebRequest.Create(AppModels.fcmsendurl);

        //    //List<Tuple<string, string>>  parameters = new List<Tuple<string, string>>();

        //    //httpWebRequest.ContentType = "application/json";
        //    //httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", AppModels.fcmserverkey));
        //    //httpWebRequest.Headers.Add(string.Format("Sender: id={0}", AppModels.fcmsenderid));
        //    //httpWebRequest.Method = "POST";

        //    //if (title.Length > 100)
        //    //    title = title.Substring(0, 95) + "...";

        //    ////Message cannot exceed 100
        //    //if (message.Length > 100)
        //    //    message = message.Substring(0, 95) + "...";

        //    //JObject jObject = new JObject();
        //    //jObject.Add("to", deviceToken);
        //    //jObject.Add("priority", "high");
        //    //jObject.Add("content_available", true);

        //    //JObject jObjNotification = new JObject();
        //    //jObjNotification.Add("body", message);
        //    //jObjNotification.Add("title", title);

        //    //jObject.Add("notification", jObjNotification);

        //    //JObject jObjData = new JObject();

        //    //jObjData.Add("badge", badge);
        //    //jObjData.Add("body", message);
        //    //jObjData.Add("title", title);

        //    //foreach (Tuple<string, string> parameter in parameters)
        //    //{
        //    //    jObjData.Add(parameter.Item1, parameter.Item2);
        //    //}

        //    //jObject.Add("data", jObjData);

        //    //var serializer = new JavaScriptSerializer();
        //    //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //    //{
        //    //    string json = jObject.ToString();
        //    //    streamWriter.Write(json);
        //    //    streamWriter.Flush();
        //    //}

        //    //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //    //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //    //{
        //    //    result = streamReader.ReadToEnd();
        //    //}

        //    //return result;
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}