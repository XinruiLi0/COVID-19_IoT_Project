using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using RestSharp;

namespace Web.Models
{
    public class PredictionModel
    {
        private static string endpoint = "www.example.com";
        public static void executePredictionProgress(Dictionary<int, Dictionary<string, string>> contactList)
        {
            foreach(KeyValuePair<int, Dictionary<string, string>> entry in contactList)
            {
                var contactInfo = entry.Value;

                var client = new RestClient(endpoint);
                // client.Authenticator = new HttpBasicAuthenticator(username, password);

                var request = new RestRequest("resource/{id}", Method.POST);

                // adds to POST or URL querystring based on Method
                request.AddParameter("SourceID", contactInfo["SourceID"]);
                request.AddParameter("TargetID", contactInfo["TargetID"]);
                request.AddParameter("Age", contactInfo["Age"]);
                request.AddParameter("HasInfectedBefore", contactInfo["HasInfectedBefore"]);
                request.AddParameter("Periods", contactInfo["Periods"]);
                request.AddParameter("CloseContact", contactInfo["CloseContact"]);
                request.AddParameter("ClosePeriods", contactInfo["ClosePeriods"]);

                // easily add HTTP Headers
                // request.AddHeader("header", "value");

                // execute the request
                IRestResponse response = client.Execute(request);
                // var content = response.Content; // raw content as string
            }
        }
    }
}
