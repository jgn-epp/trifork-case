using FluentValidation;

namespace trifork.Models.Input;

public record UpdateMatchResultModel
{
    public required Guid MatchId { get; init; }
    public required WinningTeamEnum WinningTeam { get; init; }
}

public class UpdateMatchResultModelValidator: AbstractValidator<UpdateMatchResultModel>
{
    public UpdateMatchResultModelValidator()
    {
        RuleFor(x => x.WinningTeam).IsInEnum().WithMessage("WinningTeam is not a valid WinningTeamEnum");
    }
}