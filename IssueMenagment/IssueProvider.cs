using System.Collections.Generic;

namespace IssueMenagment
{
    public interface IssueProvider
    {
        public string authentication(string login, string token);
        List<Repo> getRepos();
        List<Issue> getIssues(Repo repo);
        void Issue(Repo repo, int id, string title, string descr);
    }
}
