namespace Aventour.Application.Interfaces.Utilities;

/// <summary>
/// Abstracción para la librería de exportación a Excel. 
/// Su implementación va en Infraestructura.
/// </summary>
public interface IExcelExporter
{
    byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName);
}