namespace ChatBotLibrary.Library
{
    public class GameViewModel
    {
        //public int Id { get; set; }
        //public string ScheduleStatus { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public Team AwayTeam { get; set; }
        public Team HomeTeam { get; set; }

        public override string ToString()
        {
            return $"Date:{Date}, Time:{Time}, Location:{Location}" +
                $"\n\t Away Team: {AwayTeam} \n\t Home Team:{HomeTeam}";
        }
    }
}