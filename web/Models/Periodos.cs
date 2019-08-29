using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Periodos
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPeriodo { get; set; }
        [Display(Name ="Periodo")]
        [StringLength(256)]
        public string NombrePeriodo { get; set; }
        [Display(Name ="Descripción")]
        [StringLength(256)]
        public string DescripcionPeriodo { get; set; }
        [Display(Name ="Fecha inicio")]
        public DateTime FechaInicio { get; set; }
        [Display(Name ="Fecha fin")]
        public DateTime FechaFin { get; set; }
        [ForeignKey("Plan")]
        public int IdPlan { get; set; }     
        public bool Eliminado { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaMod { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioMod { get; set; }

        public virtual Planes Plan { get; set; }
    }
}