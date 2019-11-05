using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Direccion : HuellaAuditoria
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDireccion { get; set; }
        [Display(Name = "Dirección")]
        public string DireccionNombre { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Persona a cargo"), ForeignKey("PersonaCargo")]
        public String IdPersonaACargo { get; set; }
        public ApplicationUser PersonaCargo { get; set; }
        [NotMapped]
        public ApplicationUser PersonaACargo
        {
            get
            {
                return db.Users.Find(IdPersonaACargo);
            }
        }
        public virtual ICollection<Departamentos> DepartamentosDependientes { get; set; }
    }
}