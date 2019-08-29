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
        [Display(Name = "Auditoría"), StringLength(256)]
        public string Auditoria { get; set; }
        [Display(Name = "Descripción"), StringLength(256)]
        public string DescripcionAuditoria { get; set; }
        [Display(Name = "Fecha inicio")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Duracion de auditoría")]
        public int Duracion { get; set; }
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
        public int IdEstado { get; set; }
        public virtual ApplicationUser UsuarioRealiza { get; set; }
        [ForeignKey("Estado")]
        public virtual Planes Plan { get; set; }
        public virtual Estados Estado { get; set; }
    }
}