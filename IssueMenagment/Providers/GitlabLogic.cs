using IssueManagment.DataClass;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace IssueManagment.Providers
{
    public class GitlabLogic : IssueProvider
    {
        public string Login, Token, id;
        HttpClient client = new HttpClient();
        Provider gitlab = new Provider
        {
            Name = "GitLab",
            Aout = "https://gitlab.com/api/v4/user",
            GetRepo = "https://gitlab.com/api/v4/users/ID/projects",
            GetIssue = "https://gitlab.com/api/v4/projects/ProjectID/issues",
            CreateUpdate = "https://gitlab.com/api/v4/projects/ID/issues"
        };
        public bool authentication(string login, string token)
        {
            Login = login;
            Token = token;
            try
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",$"Bearer {Token}");
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("IssueManagment","1.1"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, gitlab.Aout);
                var res = client.Send(request);
                dynamic d = JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result);
                id = d.id;
                return d.d.username == login;
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
                var request = new HttpRequestMessage(HttpMethod.Get, gitlab.GetIssue.Replace("ProjectID", repo.ID.ToString()));

                var res = client.Send(request);
                var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                List<Issue> issues = new List<Issue>();
                foreach (dynamic ob in d)
                {
                    issues.Add(new Issue { Number = (int)ob.iid, Title = (string)ob.title, Body = (string)ob.description });
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
                var request = new HttpRequestMessage(HttpMethod.Get, gitlab.GetRepo.Replace("ID", id));
                List<Repo> repos = new List<Repo>();
                var res = client.Send(request);
                var d = JsonConvert.DeserializeObject<List<dynamic>>(res.Content.ReadAsStringAsync().Result);
                foreach (dynamic ob in d)
                {
                    repos.Add(new Repo { Name = (string)ob.name, ID = ob.id });
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
            try
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
            id = "";
        }
    }
}
