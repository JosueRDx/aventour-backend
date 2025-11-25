namespace Aventour.Application.DTOs.Guias;

public class GuiaResponseDto
{
    public int IdAgencia { get; set; } // En la BD es el ID de la tabla Agencias_Guias
    public string NombreComercial { get; set; } = string.Empty;
    public string WhatsappContacto { get; set; } = string.Empty;
    public string? EmailContacto { get; set; }
    public string? Descripcion { get; set; }
    public decimal? PuntuacionMedia { get; set; }
    // No devolvemos 'Validado' a menos que sea para un panel de admin, 
    // pero para uso general estos datos suelen bastar.
}