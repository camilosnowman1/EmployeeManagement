using MediatR;

namespace Application.Employees.Commands.ImportEmployees;

public class ImportEmployeesCommand : IRequest<int>
{
    public Stream FileStream { get; set; }

    public ImportEmployeesCommand(Stream fileStream)
    {
        FileStream = fileStream;
    }
}
