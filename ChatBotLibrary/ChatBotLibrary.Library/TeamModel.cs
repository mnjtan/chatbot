namespace ChatBotLibrary.Library
{
    public class TeamModel
    {
        //public int Id { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        

        public override string ToString()
        {
            return $"City:{City}, Name:{Name}, Abbreviation:{Abbreviation}";
        }
    }
}