using FluentValidation;

namespace trifork.Models.Input;

public record CreateMatchModel
{
    public required Guid Team1Player1 { get; init; }
    public required Guid Team1Player2 { get; init; }
    public required Guid Team2Player1 { get; init; }
    public required Guid Team2Player2 { get; init; }
}

public class CreateMatchModelValidator: AbstractValidator<CreateMatchModel>
{
    public CreateMatchModelValidator()
    {
        RuleFor(x => x.Team1Player1)
            .NotEmpty().WithMessage("Team1Player1 is required");
        
        RuleFor(x => x.Team1Player2)
            .NotEmpty().WithMessage("Team1Player2 is required");
        
        RuleFor(x => x.Team2Player1)
            .NotEmpty().WithMessage("Team2Player1 is required");
        
        RuleFor(x => x.Team2Player2)
            .NotEmpty().WithMessage("Team2Player2 is required");

        RuleFor(x => new { x.Team1Player1, x.Team1Player2 })
            .Must(x => !x.Team1Player1.Equals(x.Team1Player2))
            .WithMessage("Duplicate player on Team 1");

        RuleFor(x => new { x.Team2Player1, x.Team2Player2 })
            .Must(x => !x.Team2Player1.Equals(x.Team2Player2))
            .WithMessage("Duplicate player on Team 2");

        RuleFor(x => new { x.Team1Player1, x.Team1Player2, x.Team2Player1, x.Team2Player2 })
            .Must(x =>
                !x.Team1Player1.Equals(x.Team2Player1) && !x.Team1Player1.Equals(x.Team2Player2) &&
                !x.Team1Player2.Equals(x.Team2Player1) && !x.Team1Player2.Equals(x.Team2Player2)
            )
            .WithMessage("Players cannot be on both teams");
    }
}