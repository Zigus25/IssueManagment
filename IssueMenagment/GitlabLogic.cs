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
    public class GitlabLogic : IssueProvider
    {
        public string Login, Token, id;
        Provider gitlab = new Provider
        {
            Name = "GitLab",
            aout = "https://gitlab.com/api/v4/user",
            getrepo = "https://gitlab.com/api/v4/users/ID/projects",
            getissue = "https://gitlab.com/api/v4/projects/ProjectID/issues",
            createupdate = "https://gitlab.com/api/v4/projects/ID/issues"
        };
        public string authentication(string login, string token)
        {
            Login = login;
            Token = token;
            try
            {
                using (var client = new HttpClient())
                {

                    var request = new HttpRequestMessage(HttpMethod.Get, gitlab.aout);
                    request.Headers.Add("Authorization", "Bearer " + token);
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    var res = client.Send(request);
                    dynamic d = JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result);
                    id = d.id;
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

        public List<Issue> getIssues(Repo repo)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, gitlab.getissue.Replace("ProjectID", repo.id.ToString()));
                    request.Headers.Add("Authorization", "Bearer " + Token);
                    request.Headers.Add("User-Agent", "IssueMenagment");

                    var res = client.Send(request);
                    var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                    List<Issue> issues = new List<Issue>();
                    foreach (dynamic ob in d)
                    {
                        issues.Add(new Issue { number = (int)ob.iid, title = (string)ob.title, body = (string)ob.description });
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

        public List<Repo> getRepos()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, gitlab.getrepo.Replace("ID",id));
                    request.Headers.Add("Authorization", "Bearer " + Token);
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    List<Repo> repos = new List<Repo>();
                    var res = client.Send(request);
                    var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                    foreach (dynamic ob in d)
                    {
                        MessageBox.Show(ob.id.ToString());
                        repos.Add(new Repo { name = (string)ob.name, id = ob.id});
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

        public void Issue(Repo repo,int id, string title, string descr)
        {
            try
            {
                
                using (var clinet = new HttpClient())
                {
                    HttpRequestMessage request;
                    string url = gitlab.createupdate.Replace("ID", repo.id.ToString());
                    if (id != -1)
                    {
                        url = url + "/" + id;
                        url = url + "?title=" + title + "&description=" + descr;
                        request = new HttpRequestMessage(HttpMethod.Put, url);
                    }
                    else
                    {
                        url = url + "?title=" + title + "&description=" + descr;
                        request = new HttpRequestMessage(HttpMethod.Post, url);
                    }
                    MessageBox.Show(url);
                    request.Headers.Add("Authorization", "Bearer " + Token);
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    clinet.Send(request);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
