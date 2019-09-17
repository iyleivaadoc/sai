using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web.ViewModels
{
    public class FasesDefaultViewModel
    {
        [Display(Name = "Fase")]
        public string NombreFase { get; set; }
        public double Porcentaje { get; set; }
        [Display(Name ="Duración aproximada")]
        public int Duracion { get; set; }
        public int orden { get; set; }
        [Display(Name ="Seleccionar")]
        public bool Selected { get; set; }
        public List<FasesDefaultViewModel> list { get; set; }
    }
}