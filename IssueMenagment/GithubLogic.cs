using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IssueMenagment
{
    internal class GithubLogic : IssueProvider
    {
        public List<string> getIssues()
        {
            throw new NotImplementedException();
        }

        public List<string> getRepos(string login)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/users/"+login+"/repos");
                    request.Headers.Add("User-Agent", "IssueMenagment");

                    var res = client.Send(request);
                    var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                    List<string> repos = new List<string>();
                    foreach(dynamic ob in d)
                    {
                        repos.Add((string)ob.name);
                    }
                    MessageBox.Show(repos.Count.ToString());
                    return (repos);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
            throw new NotImplementedException();
        }
    }
}
