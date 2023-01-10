using System.Diagnostics.CodeAnalysis;

namespace WhoWantsToBeAMillionaire.Data.Entities;

[ExcludeFromCodeCoverage]
public class Question
{
    public int Id { get; set; }
    public string Description { get; set; }
}