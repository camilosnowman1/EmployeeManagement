using Application.Employees.Commands.CreateEmployee;
using FluentValidation.TestHelper;
using Xunit;

namespace EmployeeManagement.Tests.UnitTests;

public class CreateEmployeeValidatorTests
{
    private readonly CreateEmployeeValidator _validator;

    public CreateEmployeeValidatorTests()
    {
        _validator = new CreateEmployeeValidator();
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        // Arrange
        var command = new CreateEmployeeCommand
        {
            FirstName = "",
            LastName = "Doe",
            DocumentNumber = 123456789,
            Email = "test@example.com",
            PhoneNumber = 1234567890,
            JobTitle = "Developer",
            Department = "IT",
            Status = "Active",
            EducationLevel = "Bachelor",
            ProfessionalProfile = "Software Developer",
            Address = "123 Main St",
            Salary = 5000000,
            HireDate = DateTime.Now,
            DateOfBirth = DateTime.Now.AddYears(-25)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateEmployeeCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DocumentNumber = 123456789,
            Email = "john.doe@example.com",
            PhoneNumber = 1234567890,
            JobTitle = "Developer",
            Department = "IT",
            Status = "Active",
            EducationLevel = "Bachelor",
            ProfessionalProfile = "Experienced Software Developer",
            Address = "123 Main St",
            Salary = 5000000,
            HireDate = DateTime.Now,
            DateOfBirth = DateTime.Now.AddYears(-25)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
