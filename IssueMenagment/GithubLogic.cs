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
        string Login, Token;
        public string authentication(string login, string token, string link)
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
                Login = login;
                Token = token;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return "error";
            }
        }
        public List<Issue> getIssues(string repo)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/"+Login+"/"+repo+"/issues");
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

        public List<string> getRepos()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/users/"+Login+"/repos");
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

        public void Issue(string repo, int number, string title, string descr)
        {
            try
            {
                string url = "https://api.github.com/repos/" + Login + "/" + repo + "/issues";
                if (number != -1)
                {
                    url = url+ "\\" + number;
                }
                using (var clinet = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Login + ":" + Token));
                    request.Headers.Add("Authorization", "Basic " + encoded);
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    request.Content = new StringContent("{\"title\":\"" + title + "\",\"body\":\"" + descr + "\"}", Encoding.UTF8, "application/json");
                    clinet.Send(request);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }
    }
}
