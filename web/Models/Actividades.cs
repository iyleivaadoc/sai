using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using web.Controllers;

namespace web.Models
{
    public class Actividades
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdActividad { get; set; }
        [StringLength(128), Required(ErrorMessage = "Debe proporcionar nombre a la actividad")]
        public string Actividad { get; set; }
        [StringLength(256), Required(ErrorMessage = "Debe proporcionar una descripción"),Display(Name ="Descripción")]
        public string DescripcionActividad { get; set; }
        [Required(ErrorMessage = "Se debe proporcionar una fecha de inicio"), Display(Name ="Inicio"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "Se debe proporcionar una fecha de finalización"), Display(Name ="Fin"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaFin { get; set; }
        public DateTime? FechaCierre { get; set; }
        [NotMapped, Display(Name ="Duración (Días)")]
        public int Duracion
        {
            get
            {
                TimeSpan dias = (FechaFin - FechaInicio);
                AsuetosController asueto = new AsuetosController();
                var diasAsuetos = asueto.DaysInTimeSpan(FechaInicio, FechaFin);
                var ret = (dias.Days + 1) - diasAsuetos;
                return ret < 0 ? 1 : ret;
            }
        }
        [Required(ErrorMessage = "Se debe proporcionar un porcentaje a la actividad"),Range(0.1,100,ErrorMessage ="Debe proporcionar un porcentaje entre 0 y 100") ]
        public double Porcentaje { get; set; }
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
        public DateTime? FechaCrea { get; set; }
        public DateTime? FechaModifica { get; set; }
        public virtual Fases Fase { get; set; }
        public virtual ApplicationUser Encargado { get; set; }
        public virtual Estados Estado { get; set; }
    }
}