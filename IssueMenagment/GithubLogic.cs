using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IssueMenagment
{
    internal class GithubLogic : IssueProvider
    {
        public void createIssue(string login, string repo, string title, string descr)
        {
            Exception e = new Exception();
            try
            {
                using (var clinet = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.github.com/repos/"+login+"/"+repo+"/issues");
                    var password = "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";
                    string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(login + ":" + password));
                    request.Headers.Add("Authorization", "Basic " + encoded);
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    request.Content = new StringContent("{\"title\":\""+title+"\",\"body\":\""+descr+"\"}", Encoding.UTF8, "application/json");
                    clinet.Send(request);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }

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

        public void updateIssue(string login, string repo, int number)
        {
            throw new NotImplementedException();
        }
    }
}
