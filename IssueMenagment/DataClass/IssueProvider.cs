using System.Collections.Generic;

namespace IssueManagment.DataClass
{
    public interface IssueProvider
    {
        public string authentication(string login, string token);
        List<Repo> getRepos();
        List<Issue> getIssues(Repo repo);
        void issue(Repo repo, int id, string title, string descr);

        void endConnection();
    }
}
