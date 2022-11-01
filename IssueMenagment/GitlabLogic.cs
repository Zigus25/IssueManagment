using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;

namespace IssueMenagment
{
    public class GitlabLogic : IssueProvider
    {
        public string Login, Token, id;
        Provider gitlab = new Provider
        {
            Name = "GitLab",
            Aout = "https://gitlab.com/api/v4/user",
            GetRepo = "https://gitlab.com/api/v4/users/ID/projects",
            GetIssue = "https://gitlab.com/api/v4/projects/ProjectID/issues",
            CreateUpdate = "https://gitlab.com/api/v4/projects/ID/issues"
        };
        public string authentication(string login, string token)
        {
            Login = login;
            Token = token;
            try
            {
                using (var client = new HttpClient())
                {

                    var request = new HttpRequestMessage(HttpMethod.Get, gitlab.Aout);
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
                MessageBox.Show(ex.Message);
                return "error";
            }
        }

        public List<Issue> getIssues(Repo repo)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, gitlab.GetIssue.Replace("ProjectID", repo.ID.ToString()));
                    request.Headers.Add("Authorization", "Bearer " + Token);
                    request.Headers.Add("User-Agent", "IssueMenagment");

                    var res = client.Send(request);
                    var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                    List<Issue> issues = new List<Issue>();
                    foreach (dynamic ob in d)
                    {
                        issues.Add(new Issue { Number = (int)ob.iid, Title = (string)ob.title, body = (string)ob.description });
                    }
                    return issues;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public List<Repo> getRepos()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, gitlab.GetRepo.Replace("ID",id));
                    request.Headers.Add("Authorization", "Bearer " + Token);
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    List<Repo> repos = new List<Repo>();
                    var res = client.Send(request);
                    var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                    foreach (dynamic ob in d)
                    {
                        MessageBox.Show(ob.id.ToString());
                        repos.Add(new Repo { Name = (string)ob.name, ID = ob.id});
                    }
                    return (repos);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void issue(Repo repo,int id, string title, string descr)
        {
            try
            {
                
                using (var clinet = new HttpClient())
                {
                    HttpRequestMessage request;
                    string url = gitlab.CreateUpdate.Replace("ID", repo.ID.ToString());
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
                MessageBox.Show(ex.Message);
            }
        }
    }
}
