using FluentValidation;

namespace trifork.Models.Input;

public record CreatePlayerModel
{
    public required string Name { get; init; }
    public required string Initials { get; init; }
    public int Handicap { get; init; } = 10;
}

public class CreatePlayerModelValidator: AbstractValidator<CreatePlayerModel>
{
    public CreatePlayerModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(128).WithMessage("Name cannot be longer than 128 characters");

        RuleFor(x => x.Initials)
            .NotEmpty().WithMessage("Initials are required")
            .MaximumLength(64).WithMessage("Initials cannot be longer than 64 characters");

        RuleFor(x => x.Handicap)
            .GreaterThanOrEqualTo(0).WithMessage("Handicap cannot be negative");
    }
}