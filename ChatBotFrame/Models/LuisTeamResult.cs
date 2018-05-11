namespace Microsoft.Bot.Sample.LuisBot.Models
{
    public class TopScoringIntent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }
    
    public class Intent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }
    
    public class Resolution
    {
        public List<string> values { get; set; }
    }
    
    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public Resolution resolution { get; set; }
    }
    
    public class LuisTeamResult
    {
        public string query { get; set; }
        public TopScoringIntent topScoringIntent { get; set; }
        public List<Intent> intents { get; set; }
        public List<Entity> entities { get; set; }
    }
}