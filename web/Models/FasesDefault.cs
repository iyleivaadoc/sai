using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class FasesDefault
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFaseDefault { get; set; }
        public string NombreFase { get; set; }
        public double Porcentaje { get; set; }
        public int Duracion { get; set; }
        public int orden { get; set; }
    }
}