using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IssueMenagment
{
    internal class Authorize
    {
        public string authentication(string login, string token,string link)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                               .GetBytes(login + ":" + token));
                    var request = new HttpRequestMessage(HttpMethod.Get, link);
                    request.Headers.Add("Authorization", "Basic " + encoded);
                    request.Headers.Add("User-Agent", "IssueMenagment");

                    var res = client.Send(request);
                    return (res.Content.ReadAsStringAsync().Result);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return "error";
            }
        }
    }
}
