using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WhoWantsToBeAMillionaire.Data.Entities;

[ExcludeFromCodeCoverage]
public class Options
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool Correct { get; set; }
    public int QuestionId { get; set; }
}