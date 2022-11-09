using System.Collections.Generic;
using IssueManagment.DataClass;

namespace IssueManagment
{
    public interface IssueProvider
    {
        public bool authentication(string login, string token);
        List<Repo> getRepos();
        List<Issue> getIssues(Repo repo);
        void issueCreateUpdate(Repo repo, int id, string title, string descr);

        void endConnection();
    }
}
