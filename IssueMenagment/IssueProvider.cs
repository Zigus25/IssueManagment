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
        void updateIssue(string login, string repo, int number);
        void createIssue(string login, string repo, string title, string descr);
    }
}
