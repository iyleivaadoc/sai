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
        [StringLength(256),Display(Name ="Plan")]
        public string NombrePlan { get; set; }
        [StringLength(256),Display(Name ="Nombre")]
        public string DescripcionPlan { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; }
        public int anio { get; set; }
        public bool Eliminado { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaMod { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioMod { get; set; }
    }
}