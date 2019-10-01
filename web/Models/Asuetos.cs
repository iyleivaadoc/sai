using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Asuetos
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAsueto { get; set; }
        [Required(ErrorMessage = "La Descripción es requerida."), Display(Name = "Descripción"), StringLength(256)]
        public string DescripcionAsueto { get; set; }
        [Required(ErrorMessage = "La fecha de inicio es requerida."), Display(Name = "Inicio"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Inicio { get; set; }
        [Required(ErrorMessage = "La fecha fin es requerida."), Display(Name = "Fin"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fin { get; set; }
        public bool Eliminado { get; set; }
        [StringLength(128)]
        public string UsuarioCrea { get; set; }
        [StringLength(128)]
        public string UsuarioModifica { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime FechaModifica { get; set; }
    }
}