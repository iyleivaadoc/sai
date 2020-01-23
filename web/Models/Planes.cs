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
        ApplicationDbContext db = new ApplicationDbContext();
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlan { get; set; }
        [Required(ErrorMessage = "El nombre del plan es Requerido."), StringLength(256), Display(Name = "Plan")]
        public string NombrePlan { get; set; }
        [Required(ErrorMessage = "La descripcion del plan es requerida."), StringLength(256), Display(Name = "Descripción")]
        public string DescripcionPlan { get; set; }
        [Required(ErrorMessage = "La fecha de inicio es requerida."), Display(Name = "Fecha de inicio"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Año"), Required(ErrorMessage = "El año al que pertenece el plan es requerido.")]
        public int anio { get; set; }
        [ForeignKey("Estado")]
        public int IdEstado { get; set; }
        public bool Aprobado { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaCierre { get; set; }
        [NotMapped, Display(Name = "Porcentaje de avance")]
        public double Porcentaje
        {
            get
            {
                var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.IdPlan == IdPlan);
                double ret = 0.0;
                if (auditorias.Count() > 0)
                {
                    foreach (var auditoria in auditorias)
                    {
                        ret += auditoria.PorcentajeAvance;
                    }
                    ret = ret / auditorias.Count();
                }
                return ret;
            }
        }
        [NotMapped]
        public bool AuditoriasAbiertas
        {
            get
            {
                var ret= false;
                var audits = db.Auditorias.Where(a=>a.IdPlan==IdPlan && a.Elimanado!=true && a.IdEstado!=2).ToList();
                if (audits.Count > 0)
                {
                    ret = true;
                }
                return ret;
            }
        }

        public bool Eliminado { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaMod { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioMod { get; set; }
        public virtual Estados Estado { get; set; }
    }
}