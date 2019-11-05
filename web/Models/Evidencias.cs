using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Evidencias
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEvidencia { get; set; }
        [StringLength(128), Display(Name = "Evidencia"), Required(ErrorMessage = "Debe proporcionar nombre a la evidencia")]
        public string Evidencia { get; set; }
        [StringLength(128), Display(Name = "Descripción"), Required(ErrorMessage = "Debe proporcionar una descripción a la evidencia")]
        public string DescripcionEvidencia { get; set; }
        [StringLength(128), Display(Name ="Nombre Documento")]
        public string NombreDoc { get; set; }
        public byte[] Documento { get; set; }
        [ForeignKey("Hallazgo")]
        public int? IdHallazgo { get; set; }
        [ForeignKey("Actividad")]
        public int? IdActividad { get; set; }
        [ForeignKey("PlanAccion")]
        public int? IdPlanAccion { get; set; }
        public bool Eliminado { get; set; }
        [StringLength(128)]
        public string UsuarioCrea { get; set; }
        [StringLength(128)]
        public string UsuarioModifica { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaModifica { get; set; }
        public virtual Hallazgos Hallazgo { get; set; }
        public virtual Actividades Actividad { get; set; }
        public virtual PlanesDeAccion PlanAccion { get; set; }
    }
}