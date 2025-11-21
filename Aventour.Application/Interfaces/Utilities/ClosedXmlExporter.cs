using Aventour.Application.Interfaces.Utilities;
using ClosedXML.Excel;

namespace Aventour.Infrastructure.Utilities;

public class ClosedXmlExporter : IExcelExporter
{
    public byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName)
    {
        using (var workbook = new XLWorkbook())
        {
            // Crear una nueva hoja con el nombre especificado
            var worksheet = workbook.Worksheets.Add(sheetName);
            
            // Insertar los datos, incluyendo los encabezados de las propiedades
            worksheet.Cell(1, 1).InsertTable(data); 

            // Estilos profesionales (Opcional, pero recomendado)
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Columns().AdjustToContents();

            // Guardar el archivo en un MemoryStream para retornar los bytes
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}