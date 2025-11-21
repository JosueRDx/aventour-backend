using Aventour.Application.Interfaces.Services;
using Aventour.Application.Interfaces.Utilities;
using Aventour.Domain.Interfaces;
 

namespace Aventour.Application.Services;

public class ReporteService : IReporteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelExporter _excelExporter; // Abstracción para ClosedXML

    public ReporteService(IUnitOfWork unitOfWork, IExcelExporter excelExporter)
    {
        _unitOfWork = unitOfWork;
        _excelExporter = excelExporter;
    }

    public async Task<byte[]> ExportDestinosToExcelAsync()
    {
        // 1. Obtener los datos de la base de datos (Usando el Repositorio)
        var destinos = await _unitOfWork.DestinosTuristicos.GetAllAsync();
        
        // 2. Mapear las entidades a un DTO de reporte simple (para controlar los campos)
        var dataToExport = destinos.Select(d => new DestinoReporteDto
        {
            IdDestino = d.IdDestino,
            Nombre = d.Nombre,
            Tipo = d.Tipo,
            Latitud = d.Latitud,
            Longitud = d.Longitud,
            CostoEntrada = d.CostoEntrada ?? 0,
            PuntuacionMedia = d.PuntuacionMedia ?? 0
        }).ToList();

        // 3. Llamar al adaptador de Infraestructura para generar el archivo
        return _excelExporter.ExportToExcel(dataToExport, "DestinosTuristicos");
    }
}

// DTO Necesario para el reporte (Aventour.Application/DTOs/Destino/DestinoReporteDto.cs)
public class DestinoReporteDto
{
    public int IdDestino { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public decimal CostoEntrada { get; set; }
    public decimal PuntuacionMedia { get; set; }
}