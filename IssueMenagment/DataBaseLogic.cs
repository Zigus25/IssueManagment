using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace IssueMenagment
{
    public class DataBaseLogic: IssueProvider
    {
        LiteDatabase db;
        public string authentication(string login, string token)
        {
            try
            {
                db = new LiteDatabase(login);
                var col = db.GetCollectionNames();
                var res = col.Count();
                if(res > 0)
                {
                    return "Istnieje";
                }
                else
                {
                    return "Nie istnieje";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void createDB(string path,List<Issue> issues, Repo repo)
        {
            try
            {
                using (var db = new LiteDatabase(path))
                {
                    var col = db.GetCollection<Issue>(repo.Name);
                    foreach (var issue in issues)
                    {
                        col.Insert(issue);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<Issue> getIssues(Repo repo)
        {
            try
            {
                List<Issue> issues = new List<Issue>();
                var col = db.GetCollection<Issue>(repo.Name);
                var res = col.FindAll();
                foreach (var issue in res)
                {
                    issues.Add(issue);
                }
                return issues;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public List<Repo> getRepos()
        {
            try
            {
                List<Repo> repos = new List<Repo>();
                var col = db.GetCollectionNames();
                foreach (var colName in col)
                {
                    repos.Add(new Repo { Name = (string)colName, ID = 1 });
                }
                return repos;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void issue(Repo repo, int id, string title, string descr)
        {
            try
            {
                var col = db.GetCollection<Issue>(repo.Name);
                if (id == -1)
                {
                    col.Insert(new Issue { Number = col.Count() + 1, Title = title, body = descr });
                }
                else
                {
                    Issue issue = col.Query().Where(x => x.Number == id).ToList()[0];
                    issue.Title = title;
                    issue.body = descr;
                    col.Update(issue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
