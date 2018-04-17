namespace ChatBotLibrary.Library
{
    public class GameModel
    {
        //public int Id { get; set; }
        //public string ScheduleStatus { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public TeamModel AwayTeam { get; set; }
        public TeamModel HomeTeam { get; set; }

        public override string ToString()
        {
            return $"Date:{Date}, Time:{Time}, Location:{Location}" +
                $"\n\t Away Team: {AwayTeam} \n\t Home Team:{HomeTeam}";
        }
    }
}