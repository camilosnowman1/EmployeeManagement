using MediatR;

namespace Application.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommand : IRequest
{
    public Guid Id { get; set; }

    public DeleteEmployeeCommand(Guid id)
    {
        Id = id;
    }
}
