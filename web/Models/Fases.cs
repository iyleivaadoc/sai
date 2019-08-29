using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Fases
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFase { get; set; }
        [StringLength(256),Required(ErrorMessage ="Debe proporcionar un nombre a la fase")]
        public string Fase { get; set; }
        [Required(ErrorMessage ="Debe Proporcionar el porcentaje")]
        public double Porcentaje { get; set; }
        [Required(ErrorMessage ="Debe Proporcionar la fecha de inicio")]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "Debe Proporcionar la duración de la fase")]
        public int Duracion { get; set; }
        [ForeignKey("Auditoria")]
        public int IdAuditoria { get; set; }
        [ForeignKey("Estado")]
        public int IdEstado { get; set; }
        public bool Eliminado { get; set; }
        [StringLength(128)]
        public string UsuarioCrea { get; set; }
        [StringLength(128)]
        public string UsuarioModifica { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime FechaModifica { get; set; }
        public virtual Auditorias Auditoria { get; set; }
        public virtual Estados Estado { get; set; }
    }
}