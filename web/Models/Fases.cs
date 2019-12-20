using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using web.Controllers;

namespace web.Models
{
    [Validator(typeof(ValidatorDate))]
    public class Fases
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFase { get; set; }
        [Display(Name = "Fase"), StringLength(256), Required(ErrorMessage = "Debe proporcionar un nombre a la fase")]
        public string Fase { get; set; }
        [Display(Name = "Porcentaje"), Required(ErrorMessage = "Debe Proporcionar el porcentaje"), DataType(DataType.Currency, ErrorMessage = "El valor '{0}' no es valido para {1}"), DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P2}"), Range(0.001, 100.0, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        public double Porcentaje { get; set; }
        [Display(Name = "Inicio"), Required(ErrorMessage = "Debe Proporcionar la fecha de inicio"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Finalización"), Required(ErrorMessage = "Debe Proporcionar la fecha de Finalización"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaFin { get; set; }
        public DateTime? FechaCierre { get; set; }
        [NotMapped, Display(Name = "Duración (Días)")]
        public int Duracion
        {
            get
            {
                TimeSpan dias = FechaFin - FechaInicio;
                AsuetosController asueto = new AsuetosController();
                var diasAsuetos = asueto.DaysInTimeSpan(FechaInicio, FechaFin);
                return dias.Days - diasAsuetos;
            }
        }
        [ForeignKey("Auditoria")]
        public int IdAuditoria { get; set; }
        [ForeignKey("Estado")]
        public int IdEstado { get; set; }
        [Display(Name ="Reporte")]
        public ReporteFase reporte { get; set; }
        public bool Eliminado { get; set; }
        [StringLength(128)]
        public string UsuarioCrea { get; set; }
        [StringLength(128)]
        public string UsuarioModifica { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime FechaModifica { get; set; }
        public virtual Auditorias Auditoria { get; set; }
        public virtual Estados Estado { get; set; }
        [NotMapped]
        public double PorcentajeAvance
        {
            get
            {
                var actividades = db.Actividades.Where(a => a.IdFase == IdFase && a.Eliminado!=true);
                double ret = 0.0;
                foreach (var actividad in actividades.Where(a=>a.IdEstado==2))
                {
                    ret += actividad.Porcentaje;
                }
                return ret;
            }
        }
        
    }
}