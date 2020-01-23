using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Periodos
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPeriodo { get; set; }
        [Display(Name ="Periodo"),Required(ErrorMessage ="Nombre requerido")]
        [StringLength(256)]
        public string NombrePeriodo { get; set; }
        [Display(Name ="Descripción"),Required(ErrorMessage ="Descripción requerida")]
        [StringLength(256)]
        public string DescripcionPeriodo { get; set; }
        [Display(Name ="Fecha inicio"),DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; }
        [Display(Name ="Fecha fin"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaFin { get; set; }  
        public bool Eliminado { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaMod { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioMod { get; set; }
        [NotMapped]
        public bool betweenDateInicio
        {
            get
            {
                var per = db.Periodos.Where(p => p.FechaInicio <= FechaInicio && p.FechaFin >= FechaInicio && p.Eliminado != true).ToList();
                if (per.Count > 0)
                    return true;
                return false;
            }
        }

        [NotMapped]
        public bool betweenDateFin
        {
            get
            {
                var per = db.Periodos.Where(p => p.FechaInicio <= FechaFin && p.FechaFin >= FechaFin && p.Eliminado != true).ToList();
                if (per.Count > 0)
                    return true;
                return false;
            }
        }

        [NotMapped]
        public bool betweenDateInicioEdit
        {
            get
            {
                var per = db.Periodos.Where(p => p.FechaInicio <= FechaInicio && p.FechaFin >= FechaInicio && p.IdPeriodo != IdPeriodo && p.Eliminado!=true).ToList();
                if (per.Count > 0)
                    return true;
                return false;
            }
        }

        [NotMapped]
        public bool betweenDateFinEdit
        {
            get
            {
                var per = db.Periodos.Where(p => p.FechaInicio <= FechaFin && p.FechaFin >= FechaFin && p.IdPeriodo!= IdPeriodo && p.Eliminado != true).ToList();
                if (per.Count > 0)
                    return true;
                return false;
            }
        }
    }
}