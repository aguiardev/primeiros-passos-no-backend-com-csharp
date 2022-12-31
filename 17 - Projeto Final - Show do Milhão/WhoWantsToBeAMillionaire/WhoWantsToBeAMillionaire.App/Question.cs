namespace WhoWantsToBeAMillionaire.App
{
    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Option> Options { get; set; }
    }
}