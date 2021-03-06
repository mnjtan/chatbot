﻿namespace ChatBotLibrary.Library
{
    public class StatsModel
    {
        public Details GamesPlayed { get; set; }
        public Details Wins { get; set; }
        public Details Losses { get; set; }
        public Details WinPct { get; set; }
        public Details FgPct { get; set; }
        public Details PtsPerGame { get; set; }



        public override string ToString()
        {
            return $"Games Played:{GamesPlayed}, Wins:{Wins}, Losses{Losses}";
        }

    }

    public class Details
    {
        public string Abbreviation { get; set; }
        public string Text { get; set; }
    }

}