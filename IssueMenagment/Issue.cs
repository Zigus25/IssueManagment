using LiteDB;

namespace IssueMenagment
{
    public class Issue
    {
        [BsonId]
        public ObjectId? Id { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public string body { get; set; }

    }
}
