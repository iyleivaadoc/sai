using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Departamentos : HuellaAuditoria
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDepartamento { get; set; }
        [Required]
        [Display(Name = "Departamento")]
        [StringLength(256)]
        public String NombreDepartamento { get; set; }
        [Display(Name = "Descripción")]
        [StringLength(256)]
        public String DescripcionDepartamento { get; set; }
        [Display(Name = "Persona a cargo")]
        public String IdPersonaACargo { get; set; }
        [ForeignKey("Direccion"), Display(Name = "Dirección")]
        public int? IdDireccion { get; set; }
        public virtual Direccion Direccion { get; set; }
        [NotMapped]
        public ApplicationUser PersonaACargo
        {
            get
            {
                return db.Users.Find(IdPersonaACargo);
            }
        }
        public ICollection<Auditorias> AuditoriasRealizadas { get; set; }
    }
}