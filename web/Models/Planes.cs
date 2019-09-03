using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Planes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlan { get; set; }
        [Required(ErrorMessage ="El nombre del plan es Requerido."),StringLength(256),Display(Name ="Plan")]
        public string NombrePlan { get; set; }
        [Required(ErrorMessage ="La descripcion del plan es requerida."),StringLength(256),Display(Name ="Descripción")]
        public string DescripcionPlan { get; set; }
        [Required(ErrorMessage ="La fecha de inicio es requerida.") ,Display(Name ="Fecha de inicio"),DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; }
        [Display(Name ="Año"),Required(ErrorMessage ="El año al que pertenece el plan es requerido.")]
        public int anio { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed),Display(Name ="Porcentaje de avance")]
        public double Porcentaje { get; set; }
        public bool Eliminado { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaMod { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioMod { get; set; }
    }
}