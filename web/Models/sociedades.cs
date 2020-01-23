using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Sociedades
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSociedad { get; set; }
        [StringLength(256)]
        public string NombreSociedad { get; set; }
        public bool Eliminado { get; set; }
    }
}