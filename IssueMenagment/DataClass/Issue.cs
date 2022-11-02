using LiteDB;

namespace IssueManagment.DataClass
{
    public class Issue
    {
        [BsonId]
        public ObjectId? Id { get; set; }
        public int? Number { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

    }
}
