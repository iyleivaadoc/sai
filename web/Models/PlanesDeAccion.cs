using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class PlanesDeAccion
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlanAccion { get; set; }
        [StringLength(128),Required(ErrorMessage ="Debe proporcionar el nombre del hallazgo"),Display(Name ="Hallazgo")]
        public string NombrePlanAccion { get; set; }
        [StringLength(256), Required(ErrorMessage = "Debe proporcionar un plan de acción"), Display(Name = "Plan de acción")]
        public string DescripcionPlanAccion { get; set; }
        [Required(ErrorMessage = "Debe proporcionar una fecha de cumplimiento"), Display(Name = "Fecha de límite"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaCierre { get; set; }
        public bool Eliminado { get; set; }
        [ForeignKey("Hallazgo")]
        public int IdHallazgo { get; set; }
        [ForeignKey("Encargado"),Display(Name ="Responsable")]
        public string IdEncargado { get; set; }
        [ForeignKey("Validador"), Display(Name = "Director Valida")]
        public string IdDirectorValidador { get; set; }
        [ForeignKey("Estado")]
        public int IdEstado { get; set; }
        //[ForeignKey("Evidencia")]
        //public int IdEvidencia { get; set; }
        [StringLength(128)]
        public string UsuarioCrea { get; set; }
        [StringLength(128)]
        public string UsuarioModifica { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaModifica { get; set; }
        public virtual Hallazgos Hallazgo { get; set; }
        public virtual ApplicationUser Encargado { get; set; }
        public virtual ApplicationUser Validador { get; set; }
        public virtual Estados Estado { get; set; }
        //public virtual Evidencias Evidencia { get; set; }
    }
}