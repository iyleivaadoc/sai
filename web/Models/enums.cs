using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web.Models
{
    public enum ReporteFase
    {
        NA=0,
        [Display(Name="Carta de inicio")]
        CartaInicio =1,
        [Display(Name ="Solicitud de información")]
        SolicitudInformacion =2,
        [Display(Name ="Informe Draft")]
        Draft =3,
        [Display(Name ="Informe final")]
        informeFinal =4
    }
}