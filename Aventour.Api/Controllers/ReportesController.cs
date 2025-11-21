using Aventour.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aventour.Api.Controllers;

[ApiController]
[Route("api/v1/reportes")]
// [Authorize(Roles = "Administrador")] // En un entorno real, solo Admins pueden acceder
public class ReportesController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReportesController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    /// <summary>
    /// Genera y descarga un reporte Excel con todos los destinos turísticos.
    /// </summary>
    [HttpGet("destinos/excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    public async Task<IActionResult> ExportDestinosExcel()
    {
        var fileBytes = await _reporteService.ExportDestinosToExcelAsync();

        // Retornar el archivo con el tipo de contenido correcto
        return File(
            fileContents: fileBytes,
            contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileDownloadName: $"ReporteDestinos_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx"
        );
    }
}