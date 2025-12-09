using Domain.Interfaces;
using MediatR;

namespace Application.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IEmployeeRepository _repository;

    public DeleteEmployeeHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
    }
}
