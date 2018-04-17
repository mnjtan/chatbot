namespace ChatBotLibrary.Library
{
    public class TeamStatsModel
    {
        public TeamModel Team { get; set; }
        public int Rank { get; set; }
        public StatsModel Stats { get; set; }

        public override string ToString()
        {
            return $"Team:{Team}, Rank:{Rank}";//, \n\t Stats:{Stats}";
        }

    }
}