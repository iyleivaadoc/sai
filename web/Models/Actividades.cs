using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Actividades
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdActividad { get; set; }
        [StringLength(128), Required(ErrorMessage ="Debe proporcionar nombre a la actividad")]
        public string Actividad { get; set; }
        [StringLength(256), Required(ErrorMessage ="Debe proporcionar una descripción")]
        public string DescripcionActividad { get; set; }
        [Required(ErrorMessage ="Se debe proporcionar una fecha de inicio")]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "Se debe proporcionar una duración a la actividad")]
        public int Duracion { get; set; }
        [Required(ErrorMessage = "Se debe proporcionar un porcentaje a la actividad")]
        public double porcentaje { get; set; }
        public bool Eliminado { get; set; }
        [ForeignKey("Encargado")]
        public string IdEncargado { get; set; }
        [ForeignKey("Fase")]
        public int IdFase { get; set; }
        [ForeignKey("Estado")]
        public int IdEstado { get; set; }
        [StringLength(128)]
        public string UsuarioCrea { get; set; }
        [StringLength(128)]
        public string UsuarioModifica { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime FechaModifica { get; set; }
        public virtual Fases Fase { get; set; }
        public virtual ApplicationUser Encargado { get; set; }
        public virtual Estados Estado { get; set; }
    }
}