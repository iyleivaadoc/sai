using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using web.Controllers;

namespace web.Models
{
    public class Auditorias
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAuditoria { get; set; }
        [Required(ErrorMessage = "El nombre de la auditoría es requerida."), Display(Name = "Auditoría"), StringLength(256)]
        public string Auditoria { get; set; }
        [Required(ErrorMessage = "La Descripción es requerida."), Display(Name = "Descripción"), StringLength(256)]
        public string DescripcionAuditoria { get; set; }
        [Required(ErrorMessage = "La fecha de inicio es requerida."), Display(Name = "Inicio"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; }
        [StringLength(1024)]
        public string Objetivo { get; set; }
        [StringLength(1024)]
        public string Alcances { get; set; }
        [StringLength(1024)]
        public string Procedimiento { get; set; }
        [StringLength(1024)]
        public string Entregables { get; set; }
        [Display(Name = "Duración (Días)")]
        [NotMapped]
        public int Duracion { get {
                TimeSpan dias = FechaFin - FechaInicio;
                AsuetosController asueto = new AsuetosController();
                var diasAsuetos = asueto.DaysInTimeSpan(FechaInicio, FechaFin);
                return dias.Days-diasAsuetos+1;
            } }
        [Required(ErrorMessage = "La Fecha de Finalización es requerida."), Display(Name = "Finalización"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaFin { get; set; }
        public bool? Planificada { get; set; }
        public bool Elimanado { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaMod { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioMod { get; set; }
        [ForeignKey("UsuarioRealiza"),Display(Name ="Asignada")]
        public string IdUsuarioRealiza { get; set; }
        [ForeignKey("Plan")]
        public int IdPlan { get; set; }
        [ForeignKey("Estado")]
        public int IdEstado { get; set; }
        [ForeignKey("DepartamentoRealizar"), Display(Name ="Lugar")]
        public int IdDepartamentoRealizar { get; set; }
        public virtual Departamentos DepartamentoRealizar { get; set; }
        public virtual ApplicationUser UsuarioRealiza { get; set; }
        public virtual Planes Plan { get; set; }
        public virtual Estados Estado { get; set; }
        public ICollection<Fases> Fases { get; set; }
    }
}