using FluentValidation;

namespace trifork.Models.Input;

public record UpdatePlayerModel
{
    public Guid Id { get; set; }
    public string? Name { get; init; }
    public string? Initials { get; init; }
}

public class UpdatePlayerModelValidator: AbstractValidator<UpdatePlayerModel>
{
    public UpdatePlayerModelValidator()
    {
        When(playerModel => playerModel.Name is not null, () =>
        {
            RuleFor(playerModel => playerModel.Name)
                .NotEmpty()
                .WithMessage("Name is required");
            RuleFor(playerModel => playerModel.Name)
                .MaximumLength(128)
                .WithMessage("Name cannot be longer than 128 characters");
        });

        When(playerModel => playerModel.Initials is not null, () =>
        {
            RuleFor(playerModel => playerModel.Initials)
                .NotEmpty()
                .WithMessage("Initials are required");
            RuleFor(playerModel => playerModel.Initials)
                .MaximumLength(64)
                .WithMessage("Initials cannot be longer than 64 characters");
        });
    }
}