using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueMenagment
{
    internal interface IssueProvider
    {
        List<String> getRepos(string login);
        List<Issue> getIssues(string login, string repo);
        void Issue(string login, string repo, int number, string title, string descr, string token);
    }
}
