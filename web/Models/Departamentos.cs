﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Departamentos
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDepartamento { get; set; }

        [Required]
        [Display(Name = "Nombre Departamento")]
        [StringLength(256)]
        public String NombreDepartamento { get; set; }

        [Display(Name = "Descripción Departamento")]
        [StringLength(256)]
        public String DescripcionDepartamento { get; set; }

        [Display(Name = "Persona a cargo")]
        [StringLength(256)]
        public String PersonaACargo { get; set; }

        [Display(Name = "Email notificación")]
        [StringLength(256)]
        public String EmailNotificacion { get; set; }
        [StringLength(128)]
        public string UsuarioCrea { get; set; }
        [StringLength(128)]
        public string UsuarioModifica { get; set; }

        public DateTime FechaCrea { get; set; }

        public DateTime FechaModifica { get; set; }

        public bool Eliminado { get; set; }

        [ForeignKey("DepartamentoDepende")]
        public int? IdDepartamentoDepende { get; set; }

        public virtual Departamentos DepartamentoDepende { get; set; }
        public virtual ICollection<Departamentos> DepartamentosDependientes { get; set; }
    }
}