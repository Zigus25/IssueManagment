using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueMenagment
{
    internal interface IssueProvider
    {
        List<String> getRepos();
        List<String> getIssues();
    }
}
