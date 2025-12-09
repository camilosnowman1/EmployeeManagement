using FluentValidation;

namespace Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.DocumentNumber).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Salary).GreaterThan(0);
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.Now);
        RuleFor(x => x.HireDate).NotEmpty();
    }
}
