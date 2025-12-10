using FluentValidation;

namespace Application.Employees.Commands.RegisterEmployee;

public class RegisterEmployeeValidator : AbstractValidator<RegisterEmployeeCommand>
{
    public RegisterEmployeeValidator()
    {
        RuleFor(x => x.DocumentNumber).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");
        RuleFor(x => x.Salary).GreaterThan(0);
        RuleFor(x => x.HireDate).NotEmpty();
    }
}
