using IssueManagment.DataClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;

namespace IssueManagment.Providers
{
    public class GithubLogic : IssueProvider
    {
        public string Login, Token, Encoded;
        HttpClient client = new HttpClient();
        Provider github = new Provider
        {
            Name = "GitHub",
            Aout = "https://api.github.com/user",
            GetRepo = "https://api.github.com/users/LOGIN/repos",
            GetIssue = "https://api.github.com/repos/LOGIN/REPO/issues",
            CreateUpdate = "https://api.github.com/repos/LOGIN/REPO/issues"
        };

        public bool authentication(string login, string token)
        {
            Login = login;
            Token = token;
            try
            {
                Encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(login + ":" + token));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {Encoded}");
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("IssueManagment", "1.1"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var request = new HttpRequestMessage(HttpMethod.Get, github.Aout); 
                var res = client.Send(request);
                dynamic d = JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result);
                return d.login == login;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public List<Issue> getIssues(Repo repo)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, github.GetIssue.Replace("LOGIN", Login).Replace("REPO", repo.Name));

                var res = client.Send(request);
                var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                List<Issue> issues = new List<Issue>();
                foreach (dynamic ob in d)
                {
                    issues.Add(new Issue { Number = (int)ob.number, Title = (string)ob.title, Body = (string)ob.body });
                }
                return issues;
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
                var request = new HttpRequestMessage(HttpMethod.Get, github.GetRepo.Replace("LOGIN", Login));
                List<Repo> repos = new List<Repo>();
                var res = client.Send(request);
                var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                foreach (dynamic ob in d)
                {
                    repos.Add(new Repo() { Name = (string)ob.name, ID = (int)ob.id });
                }

                return repos;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void issueCreateUpdate(Repo repo, int id, string title, string descr)
        {
            IssueRequest iss = new IssueRequest { title = title, body = descr };
            try
            {
                string url = github.CreateUpdate.Replace("LOGIN", Login).Replace("REPO", repo.Name);
                if (id != -1)
                {
                    url = url + "/" + id;
                }
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(JsonConvert.SerializeObject(iss), Encoding.UTF8, "application/json");
                client.Send(request);
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
