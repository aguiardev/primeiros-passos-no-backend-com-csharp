namespace WhoWantsToBeAMillionaire.App
{
    public class Option
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool IsCorrect { get; set; }

        public Option(string alias, string description)
        {
            Alias = alias;
            Description = description;
        }

        public Option(string alias, string description, bool isCorrect)
        {
            Alias = alias;
            Description = description;
            IsCorrect = isCorrect;
        }
    }
}