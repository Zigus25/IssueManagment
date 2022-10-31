using LiteDB;
using System.Collections.Generic;

namespace IssueMenagment
{
    public class DBL
    {
        public void createDB(string path,List<Issue> issues)
        {
            using(var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<Issue>("Issues");
                foreach (var issue in issues)
                {
                    col.Insert(issue);
                }
            }
        }
        public List<Issue> importDB(string path)
        {
            List<Issue> issues = new List<Issue>();
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<Issue>("Issues");
                var res = col.FindAll();
                foreach (var issue in res)
                {
                    issues.Add(issue);
                }
            }
            return issues;
        }
    }
}
