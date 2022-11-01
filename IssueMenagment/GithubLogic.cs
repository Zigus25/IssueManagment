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
            aout = "https://api.github.com/user",
            getrepo = "https://api.github.com/users/LOGIN/repos",
            getissue = "https://api.github.com/repos/LOGIN/REPO/issues",
            createupdate = "https://api.github.com/repos/LOGIN/REPO/issues"
        };
        
        public string authentication(string login, string token)
        {
            Login = login;
            Token = token;
            try
            {
                using (var client = new HttpClient())
                {
                   
                    var request = new HttpRequestMessage(HttpMethod.Get, github.aout);
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
                    var request = new HttpRequestMessage(HttpMethod.Get, github.getissue.Replace("LOGIN",Login).Replace("REPO",repo.name));
                    request.Headers.Add("Authorization", "Basic " + Encoded);
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
                    var request = new HttpRequestMessage(HttpMethod.Get, github.getrepo.Replace("LOGIN",Login));
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    List<Repo> repos = new List<Repo>();
                    var res = client.Send(request);
                    var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                    foreach(dynamic ob in d)
                    {
                        repos.Add(new Repo() { name = (string)ob.name ,id = (int)ob.id});
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

        public void Issue(Repo repo,int id, string title, string descr)
        {
            try
            {
                string url = github.createupdate.Replace("LOGIN", Login).Replace("REPO", repo.name);
                if (repo.id != -1)
                {
                    url = url+ "/" + repo.id;
                }
                using (var clinet = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Headers.Add("Authorization", "Basic " + Encoded);
                    request.Headers.Add("User-Agent", "IssueMenagment");
                    request.Content = new StringContent("{\"title\":\"" + title + "\",\"body\":\"" + descr + "\"}", Encoding.UTF8, "application/json");
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
