using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web.ViewModels
{
    public class ReportsVM
    {
        [Display(Name="Plan")]
        public int? idPlan { get; set; }
        [Display(Name = "Auditoría")]
        public int? idAuditoria { get; set; }
    }
}