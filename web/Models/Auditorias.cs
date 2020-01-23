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
        ApplicationDbContext db = new ApplicationDbContext();

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
        public int Duracion
        {
            get
            {
                TimeSpan dias = FechaFin - FechaInicio;
                AsuetosController asueto = new AsuetosController();
                var diasAsuetos = asueto.DaysInTimeSpan(FechaInicio, FechaFin);
                var ret= (dias.Days + 1) - diasAsuetos;
                return ret < 0 ? 1 : ret;
            }
        }
        [Required(ErrorMessage = "La Fecha de Finalización es requerida."), Display(Name = "Finalización"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaFin { get; set; }
        public DateTime? FechaCierre { get; set; }
        public bool? Planificada { get; set; }
        public bool Elimanado { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaMod { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioMod { get; set; }
        [ForeignKey("UsuarioRealiza"), Display(Name = "Auditor")]
        public string IdUsuarioRealiza { get; set; }
        [ForeignKey("Plan")]
        public int IdPlan { get; set; }
        [ForeignKey("Estado")]
        public int IdEstado { get; set; }
        [ForeignKey("DepartamentoRealizar"), Display(Name = "Área a auditar")]
        public int IdDepartamentoRealizar { get; set; }
        public virtual Departamentos DepartamentoRealizar { get; set; }
        public virtual ApplicationUser UsuarioRealiza { get; set; }
        public virtual Planes Plan { get; set; }
        public virtual Estados Estado { get; set; }
        public ICollection<Fases> Fases { get; set; }
        [NotMapped]
        public double PorcentajeAvance
        {
            get
            {
                double ret = 0.0;
                var fases = db.Fases.Where(f => f.IdAuditoria == IdAuditoria && f.Eliminado != true);
                foreach (var fase in fases)
                {
                    ret += (fase.Porcentaje * fase.PorcentajeAvance);
                }
                return ret;
            }
        }

        [NotMapped]
        public string[,] CorreosNotificar
        {
            get
            {
                string[,] list=new string[3,2];
                if (IdDepartamentoRealizar != null)
                {
                    Departamentos dept = db.Departamentos.Find(IdDepartamentoRealizar);
                    list[0,0] = dept.PersonaACargo.Email;
                    list[0, 1] = dept.PersonaACargo.NombreCompleto;

                    if (dept.IdDireccion != null)
                    {
                        Departamentos dept2 = db.Departamentos.Find(dept.IdDireccion);
                        list[1, 0] = dept2.PersonaACargo.Email;
                        list[1, 1] = dept2.PersonaACargo.NombreCompleto;
                        if (dept2.IdDireccion != null)
                        {
                            Departamentos dept3 = db.Departamentos.Find(dept2.IdDireccion);
                            list[2, 0] = dept3.PersonaACargo.Email;
                            list[2, 1] = dept3.PersonaACargo.NombreCompleto;
                        }
                    }
                }
                return list;
            }
        }
    }
}