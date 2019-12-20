using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class BitacoraLogueo
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdBitacoraLogueo { get; set; }
        public string User { get; set; }
        public DateTime FechaLogueo { get; set; }
        public string Resultado { get; set; }
        public string Tipo { get; set; }
        public BitacoraLogueo() { }

    }
}