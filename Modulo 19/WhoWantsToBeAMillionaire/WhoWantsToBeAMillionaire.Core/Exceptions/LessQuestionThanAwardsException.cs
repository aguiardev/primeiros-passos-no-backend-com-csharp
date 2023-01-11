namespace WhoWantsToBeAMillionaire.Core.Exceptions;

public class LessQuestionThanAwardsException : Exception
{
	public LessQuestionThanAwardsException(string message) : base(message)
	{

	}
}