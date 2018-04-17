using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotLibrary.Library
{
    public class ConferenceModel
    {
        public string @Name { get; set; }
        public List<TeamStatsModel> TeamEntry { get; set; }

        public override string ToString()
        {
            return $"Name:{Name} \n\t Teams:{PrintEntries()}";
        }

        public string PrintEntries()
        {
            string str = "";
            foreach(var team in TeamEntry)
            {
                str += $"{team.ToString()}\n";
            }
            return str;
        }

    }
}
