using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class Prueba
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPrueba { get; set; }
        public string Campo1 { get; set; }
        public int Campo2 { get; set; }
    }
}