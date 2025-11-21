namespace Aventour.Application.Interfaces.Services;

/// <summary>
/// Contrato para la generación de reportes en diferentes formatos.
/// </summary>
public interface IReporteService
{
    /// <summary>
    /// Genera un array de bytes que representa un archivo Excel 
    /// con la lista de destinos turísticos.
    /// </summary>
    /// <returns>Array de bytes del archivo Excel (xlsx).</returns>
    Task<byte[]> ExportDestinosToExcelAsync();
}