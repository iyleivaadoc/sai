using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Hallazgos
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdHallazgo { get; set; }
        [StringLength(128),Required(ErrorMessage ="Debe proporcionar un nombre al hallazgo")]
        public string Hallazgo { get; set; }
        [StringLength(128), Required(ErrorMessage = "Debe proporcionar una descripción al hallazgo")]
        public string DescripcionHallazgo { get; set; }
        [Required(ErrorMessage = "Debe proporcionar una fecha al hallazgo")]
        public DateTime FechaHallazgo { get; set; }
        [ForeignKey("Actividad")]
        public int IdActividad { get; set; }
        public bool Eliminado { get; set; }
        [StringLength(128)]
        public string UsuarioCrea { get; set; }
        [StringLength(128)]
        public string UsuarioModifica { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime FechaModifica { get; set; }
        public virtual Actividades Actividad { get; set; }
    }
}