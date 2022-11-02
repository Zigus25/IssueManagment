using IssueManagment.DataClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace IssueMenagment
{
    public class GithubLogic : IssueProvider
    {
        public string Login, Token, Encoded;
        Provider github = new Provider
        {
            Name = "GitHub",
            Aout = "https://api.github.com/user",
            GetRepo = "https://api.github.com/users/LOGIN/repos",
            GetIssue = "https://api.github.com/repos/LOGIN/REPO/issues",
            CreateUpdate = "https://api.github.com/repos/LOGIN/REPO/issues"
        };
        
        public string authentication(string login, string token)
        {
            Login = login;
            Token = token;
            try
            {
                using (var client = new HttpClient())
                {
                   
                    var request = new HttpRequestMessage(HttpMethod.Get, github.Aout);
                    Encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(login + ":" + token));
                    request.Headers.Add("Authorization", "Basic " + Encoded);
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    var res = client.Send(request);
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
                    var request = new HttpRequestMessage(HttpMethod.Get, github.GetIssue.Replace("LOGIN",Login).Replace("REPO",repo.Name));
                    request.Headers.Add("Authorization", "Basic " + Encoded);
                    request.Headers.Add("User-Agent", "IssueMenagment");

                    var res = client.Send(request);
                    var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                    List<Issue> issues = new List<Issue>();
                    foreach (dynamic ob in d)
                    {
                        issues.Add(new Issue { Number = (int)ob.number, Title = (string)ob.title, Body = (string)ob.body});
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
                    var request = new HttpRequestMessage(HttpMethod.Get, github.GetRepo.Replace("LOGIN",Login));
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    List<Repo> repos = new List<Repo>();
                    var res = client.Send(request);
                    var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                    foreach(dynamic ob in d)
                    {
                        repos.Add(new Repo() { Name = (string)ob.name ,ID = (int)ob.id});
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
            IssueRequest iss = new IssueRequest { title = title, body = descr };
            try
            {
                string url = github.CreateUpdate.Replace("LOGIN", Login).Replace("REPO", repo.Name);
                if (id != -1)
                {
                    url = url+ "/" + id;
                }
                using (var clinet = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Headers.Add("Authorization", "Basic " + Encoded);
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    request.Content = new StringContent(JsonConvert.SerializeObject(iss), Encoding.UTF8, "application/json");
                    clinet.Send(request);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void endConnection()
        {
            Login = "";
            Token = "";
            Encoded = "";
        }
    }
}
