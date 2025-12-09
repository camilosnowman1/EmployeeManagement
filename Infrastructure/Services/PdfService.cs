using Application.DTOs;
using Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services;

public class PdfService : IPdfService
{
    public PdfService()
    {
        // License configuration for QuestPDF (Community License)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateEmployeePdf(EmployeeDto employee)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Hoja de Vida - TalentoPlus S.A.S.")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Text($"Nombre Completo: {employee.FirstName} {employee.LastName}").Bold().FontSize(16);
                        
                        x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        x.Item().Text("Información Personal").SemiBold().FontSize(14);
                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Documento:");
                            table.Cell().Text(employee.DocumentNumber.ToString());

                            table.Cell().Text("Fecha de Nacimiento:");
                            table.Cell().Text(employee.DateOfBirth.ToShortDateString());

                            table.Cell().Text("Email:");
                            table.Cell().Text(employee.Email);

                            table.Cell().Text("Teléfono:");
                            table.Cell().Text(employee.PhoneNumber.ToString());

                            table.Cell().Text("Dirección:");
                            table.Cell().Text(employee.Address);
                        });

                        x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        x.Item().Text("Información Laboral").SemiBold().FontSize(14);
                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Cargo:");
                            table.Cell().Text(employee.JobTitle);

                            table.Cell().Text("Departamento:");
                            table.Cell().Text(employee.Department);

                            table.Cell().Text("Salario:");
                            table.Cell().Text($"{employee.Salary:C}");

                            table.Cell().Text("Fecha de Ingreso:");
                            table.Cell().Text(employee.HireDate.ToShortDateString());

                            table.Cell().Text("Estado:");
                            table.Cell().Text(employee.Status);
                        });

                        x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        x.Item().Text("Perfil Profesional").SemiBold().FontSize(14);
                        x.Item().Text(employee.ProfessionalProfile);

                        x.Item().Text("Educación").SemiBold().FontSize(14);
                        x.Item().Text(employee.EducationLevel);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        });

        return document.GeneratePdf();
    }
}
