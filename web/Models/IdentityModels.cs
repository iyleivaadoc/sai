using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string Nombres { get; set; }

        public string Apellidos { get; set; }
        [NotMapped]
        public string NombreCompleto
        {
            get
            {
                return Nombres + " " + Apellidos;
            }
        }

        public string Puesto { get; set; }

        public bool Eliminado { get; set; }
        [ForeignKey("DepartamentoPertenence")]
        public int? IdDepartamentoPertenece { get; set; }

        public virtual Departamentos DepartamentoPertenence { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public static implicit operator ApplicationUser(IdentityResult v)
        {
            throw new NotImplementedException();
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public string Descripcion { get; set; }
        [DefaultValue(false)]
        public bool Eliminado { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioModifica { get; set; }
        public DateTime FechaCrea { get; set; }

        public DateTime FechaModifica { get; set; }
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Accesos> Accesos { get; set; }

        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<Departamentos> Departamentos { get; set; }
        public DbSet<Planes> Planes { get; set; }
        public DbSet<Periodos> Periodos { get; set; }
        public DbSet<Auditorias> Auditorias { get; set; }
        public DbSet<Estados> Estados { get; set; }
        public DbSet<Fases> Fases { get; set; }
        public DbSet<Actividades> Actividades { get; set; }
        public DbSet<Hallazgos> Hallazgos { get; set; }
        public DbSet<Evidencias> Evidencias { get; set; }
        public DbSet<FasesDefault> FasesDefault { get; set; }
        public DbSet<Asuetos> Asuetos { get; set; }

        public System.Data.Entity.DbSet<web.Models.PlanesDeAccion> PlanesDeAccions { get; set; }

        public DbSet<BitacoraLogueo> BitacoraLogueo { get; set; }
        public DbSet<Sociedades> Sociedades { get; set; }
    }
}