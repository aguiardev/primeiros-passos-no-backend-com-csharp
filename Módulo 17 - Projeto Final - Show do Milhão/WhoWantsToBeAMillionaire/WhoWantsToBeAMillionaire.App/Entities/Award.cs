namespace WhoWantsToBeAMillionaire.App.Entities
{
    public class Award
    {
        public int Number { get; private set; }
        public decimal Correct { get; private set; }
        public decimal Stop { get; private set; }
        public decimal Wrong { get; private set; }

        public Award(int number, decimal correct, decimal stop, decimal wrong)
        {
            Number = number;
            Correct = correct;
            Stop = stop;
            Wrong = wrong;
        }
    }
}