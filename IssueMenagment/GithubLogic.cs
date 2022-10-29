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
        public List<Issue> getIssues(string login, string repo)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/"+login+"/"+repo+"/issues");
                    request.Headers.Add("User-Agent", "IssueMenagment");

                    var res = client.Send(request);
                    var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                    List<Issue> issues = new List<Issue>();
                    foreach (dynamic ob in d)
                    {
                        issues.Add(new Issue { number = (int)ob.number, title = (string)ob.title, body = (string)ob.body});
                    }
                    return issues;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
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
                    return (repos);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }
    }
}
