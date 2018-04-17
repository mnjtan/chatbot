using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLibrary.Library
{
    public class ScoreModel
    {
        public GameModel game { get; set; }
        public int AwayScore { get; set; }
        public int HomeScore { get; set; }
        public string IsUnplayed { get; set; }
        public string IsInProgress { get; set; }
        public string IsCompleted { get; set; }
        //public QuarterSummary QuarterSummary { get; set; }

    }
}
