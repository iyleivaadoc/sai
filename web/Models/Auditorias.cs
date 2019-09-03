using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Auditorias
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAuditoria { get; set; }
        [Required(ErrorMessage = "El nombre de la auditoría es requerida."), Display(Name = "Auditoría"), StringLength(256)]
        public string Auditoria { get; set; }
        [Required(ErrorMessage = "La Descripción es requerida."), Display(Name = "Descripción"), StringLength(256)]
        public string DescripcionAuditoria { get; set; }
        [Required(ErrorMessage = "La fecha de inicio es requerida."), Display(Name = "Fecha de inicio"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Duración de auditoría")]
        [NotMapped]
        public int Duracion { get; set; }
        [Required(ErrorMessage = "La Fecha de Finalización es requerida.") Display(Name = "Fecha Finalización"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaFin { get; set; }
        public bool? Planificada { get; set; }
        public bool Elimanado { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaMod { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioMod { get; set; }
        [ForeignKey("UsuarioRealiza")]
        public string IdUsuarioRealiza { get; set; }
        [ForeignKey("Plan")]
        public int IdPlan { get; set; }
        [ForeignKey("Estado")]
        public int IdEstado { get; set; }
        public virtual ApplicationUser UsuarioRealiza { get; set; }
        public virtual Planes Plan { get; set; }
        public virtual Estados Estado { get; set; }
    }
}