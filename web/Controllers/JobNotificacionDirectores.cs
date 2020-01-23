using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using web.Models;

namespace web.Controllers
{
    public class JobNotificacionDirectores : IJob
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                //var a = 0;
                //var b=1 / a;
                var ahora = DateTime.Now;
                var hoy = new DateTime(ahora.Year, ahora.Month, ahora.Day);
                var quincena = hoy.AddDays(15);
                var ochoDias = quincena.AddDays(-7);
                var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.Plan.Eliminado != true && (a.FechaInicio == quincena || a.FechaInicio == ochoDias) && a.Plan.IdEstado == 3).ToList();
                foreach (Auditorias audit in auditorias)
                {
                    var Correos = audit.CorreosNotificar;
                    //enviar correo al auditado y al auditor aquí
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionAuditoria.html");
                    readText = readText.Replace("$$nombre$$", Correos[0, 1]).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaInicio.ToShortDateString()).Replace("$$auditor$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$correoAuditor$$", audit.UsuarioRealiza.Email);
                    EnviarCorreo(Correos[0, 0], "Notificación de auditoría a realizar", readText);

                    string readTextAuditor = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionAuditoriaAuditor.html");
                    readTextAuditor = readTextAuditor.Replace("$$nombre$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaInicio.ToShortDateString()).Replace("$$colaborador$$", Correos[0, 1]).Replace("$$area$$", audit.DepartamentoRealizar.NombreDepartamento);
                    EnviarCorreo(audit.UsuarioRealiza.Email, "Notificación de auditoría próxima a realizar", readTextAuditor);
                    //enviar correo al jefe del auditado aquí
                    if (Correos[1, 0] != null)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionJefes.html");
                        readText2 = readText2.Replace("$$nombre$$", Correos[1, 1]).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaInicio.ToShortDateString()).Replace("$$area$$", audit.DepartamentoRealizar.NombreDepartamento).Replace("$$auditor$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$correoAuditor$$", audit.UsuarioRealiza.Email);
                        EnviarCorreo(Correos[1, 0], "Notificación de auditoría a realizar", readText2);
                        //enviar correo al jefe del jefe del auditado aquí
                        if (Correos[2, 0] != null)
                        {
                            string readText3 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionJefes.html");
                            readText3 = readText3.Replace("$$nombre$$", Correos[2, 1]).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaInicio.ToShortDateString()).Replace("$$area$$", audit.DepartamentoRealizar.NombreDepartamento).Replace("$$auditor$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$correoAuditor$$", audit.UsuarioRealiza.Email);
                            EnviarCorreo(Correos[2, 0], "Notificación de auditoría a realizar", readText3);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }


        public async Task<bool> ExecuteNow()
        {
            var ret = true;
            try
            {
                //var a = 0;
                //var b=1 / a;
                
                var ahora = DateTime.Now;
                var hoy = new DateTime(ahora.Year, ahora.Month, ahora.Day);
                var quincena = hoy.AddDays(15);
                var ochoDias = quincena.AddDays(-7);
                //var mesAdelante = quincena.AddDays(15);
                var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.Plan.Eliminado != true && (a.FechaInicio == quincena || a.FechaInicio == ochoDias) && a.Plan.IdEstado == 3).ToList();
                foreach (Auditorias audit in auditorias)
                {
                    var Correos = audit.CorreosNotificar;
                    //enviar correo al auditado y al auditor aquí
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionAuditoria.html");
                    readText = readText.Replace("$$nombre$$", Correos[0, 1]).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaInicio.ToShortDateString()).Replace("$$auditor$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$correoAuditor$$", audit.UsuarioRealiza.Email);
                    EnviarCorreo(Correos[0, 0], "Notificación de auditoría a realizar", readText);

                    string readTextAuditor = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionAuditoriaAuditor.html");
                    readTextAuditor = readTextAuditor.Replace("$$nombre$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaInicio.ToShortDateString()).Replace("$$colaborador$$", Correos[0, 1]).Replace("$$area$$", audit.DepartamentoRealizar.NombreDepartamento);
                    EnviarCorreo(audit.UsuarioRealiza.Email, "Notificación de auditoría próxima a realizar", readTextAuditor);
                    //enviar correo al jefe del auditado aquí
                    if (Correos[1, 0] != null)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionJefes.html");
                        readText2 = readText2.Replace("$$nombre$$", Correos[1, 1]).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaInicio.ToShortDateString()).Replace("$$area$$", audit.DepartamentoRealizar.NombreDepartamento).Replace("$$auditor$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$correoAuditor$$", audit.UsuarioRealiza.Email);
                        EnviarCorreo(Correos[1, 0], "Notificación de auditoría a realizar", readText2);
                        //enviar correo al jefe del jefe del auditado aquí
                        if (Correos[2, 0] != null)
                        {
                            string readText3 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionJefes.html");
                            readText3 = readText3.Replace("$$nombre$$", Correos[2, 1]).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaInicio.ToShortDateString()).Replace("$$area$$", audit.DepartamentoRealizar.NombreDepartamento).Replace("$$auditor$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$correoAuditor$$", audit.UsuarioRealiza.Email);
                            EnviarCorreo(Correos[2, 0], "Notificación de auditoría a realizar", readText3);
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                ret = false;
            }
            return ret;
        }


        public bool EnviarCorreo(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("notificaciones@empresasadoc.com");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                //contenedor alrernativo para el visor html
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, MediaTypeNames.Text.Html);

                //se crea recurso vinculado con la imagen a incustar estos seran fijos(los mismos) en todos los correos
                LinkedResource img = new LinkedResource(@"C:\FormatosCorreo\SAI\images\adoc.png", MediaTypeNames.Image.Jpeg);
                img.ContentId = "logo";

                //añadiendo los recursos vinculados.
                htmlView.LinkedResources.Add(img);

                //se añade el contenedor alternativo al correo
                mail.AlternateViews.Add(htmlView);

                //Se configura el cliente smtp
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "192.168.16.30";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("notificaciones@empresasadoc.com", "");
                smtp.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }

        }

    }
}