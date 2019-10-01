using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using web.Models;

namespace web.ViewModels
{
    public class FasesDefaultViewModel
    {

        public FasesDefault FasesD { get; set; } // Si no necesitas esto solo la lista, quitarlo
        public class FasesDefaultVMList
        {
            public int Id_fase { get; set; }
            [Display(Name = "Fase")]
            public string NombreFase { get; set; }
            public double Porcentaje { get; set; }
            [Display(Name = "Duración aproximada")]
            public int Duracion { get; set; }
            public int orden { get; set; }
            [Display(Name = "Seleccionar")]
            public bool Selected { get; set; }
        }



        //public List<FasesDefaultViewModel> list { get; set; }
        public List<FasesDefaultVMList> list { get; set; }

        //[DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //public DateTime fecha { get; set; }

        public FasesDefaultViewModel()
        {
            FasesD = new FasesDefault(); // Si no necesitas esto solo la lista, quitarlo
            list = new List<FasesDefaultVMList>();
            //AccesosSelect = new List<AccesosVMList>();

        }

    }
}