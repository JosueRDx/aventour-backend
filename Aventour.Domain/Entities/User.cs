using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aventour.Domain.Entities
{
    [Table("Usuarios")]
    public class User : BaseEntity
    {
        [MaxLength(100)]
        public string Nombres { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Apellidos { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        
        public int? Edad { get; set; }
        
        [MaxLength(50)]
        public string? EstadoCivil { get; set; }
        
        public bool EsAdministrador { get; set; }
        
        [MaxLength(255)]
        public string? TokenConfirmacion { get; set; }
        
        public bool SesionActiva { get; set; }
        
        public ICollection<Resena> Resenas { get; set; } = new List<Resena>();
        public ICollection<RutaPersonalizada> RutasPersonalizadas { get; set; } = new List<RutaPersonalizada>();
    }
    public class Resena : BaseEntity {}
    public class RutaPersonalizada : BaseEntity {}
}