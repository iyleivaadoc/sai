using Quartz;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using web.Models;

namespace web.Controllers
{
    public class JobNotificacionEtapas : IJob
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Execute(IJobExecutionContext context)
        {
            try {
                var hoy= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var FechaAviso = hoy.AddDays(3);
                //Notificacion de fases
                var fases = db.Fases.Where(f => f.Eliminado != true && f.Auditoria.Elimanado!=true && f.Auditoria.Plan.Eliminado!=true && f.Auditoria.Plan.IdEstado == 3 && f.FechaInicio == FechaAviso).Include(f=>f.Auditoria.Plan).Include(f => f.Auditoria.UsuarioRealiza).ToList();
                foreach (Fases fase in fases)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionFase.html");
                    readText = readText.Replace("$$nombre$$", fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", fase.Auditoria.Auditoria).Replace("$$fecha$$", fase.FechaInicio.ToShortDateString()).Replace("$$fase$$", fase.Fase);
                    EnviarCorreo(fase.Auditoria.UsuarioRealiza.Email, "Fase de auditoría a realizarse pronto", readText);
                }

                var fasesFinalizan = db.Fases.Where(f => f.Eliminado != true && f.Auditoria.Elimanado != true && f.Auditoria.Plan.Eliminado != true && f.Auditoria.Plan.IdEstado == 3 && f.FechaFin == FechaAviso && f.IdEstado!=2).Include(f => f.Auditoria.Plan).Include(f => f.Auditoria.UsuarioRealiza).ToList();
                foreach (Fases fase in fasesFinalizan)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionFaseFinaliza.html");
                    readText = readText.Replace("$$nombre$$", fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", fase.Auditoria.Auditoria).Replace("$$fecha$$", fase.FechaFin.ToShortDateString()).Replace("$$fase$$", fase.Fase);
                    EnviarCorreo(fase.Auditoria.UsuarioRealiza.Email, "Fase a punto de finalizar.", readText);
                }

                var fasesVencidas = db.Fases.Where(f => f.Eliminado != true && f.Auditoria.Elimanado != true && f.Auditoria.Plan.Eliminado != true && f.Auditoria.Plan.IdEstado == 3 && f.FechaFin < hoy && f.IdEstado != 2).Include(f => f.Auditoria.Plan).Include(f => f.Auditoria.UsuarioRealiza).ToList();
                foreach (Fases fase in fasesVencidas)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionFaseVencida.html");
                    readText = readText.Replace("$$nombre$$", fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", fase.Auditoria.Auditoria).Replace("$$fecha$$", fase.FechaFin.ToShortDateString()).Replace("$$fase$$", fase.Fase);
                    EnviarCorreo(fase.Auditoria.UsuarioRealiza.Email, "Fase fuera de tiempo.", readText);
                }

                //notificación de actividades
                var actividades = db.Actividades.Where(a => a.Eliminado != true && a.Fase.Eliminado != true && a.Fase.Auditoria.Elimanado != true && a.Fase.Auditoria.Plan.Eliminado != true && a.Fase.Auditoria.Plan.IdEstado == 3 && a.FechaInicio == FechaAviso).Include(a=>a.Fase.Auditoria.Plan).Include(a=>a.Fase.Auditoria.UsuarioRealiza).Include(a=>a.Encargado).ToList();
                foreach(Actividades actividad in actividades)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividad.html");
                    readText = readText.Replace("$$nombre$$", actividad.Encargado.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaInicio.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad);
                    EnviarCorreo(actividad.Encargado.Email, "actividad de auditoría a realizarse pronto", readText);
                    if (actividad.Encargado.Id != actividad.Fase.Auditoria.UsuarioRealiza.Id)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividadAuditorPrincipal.html");
                        readText2 = readText2.Replace("$$nombre$$", actividad.Fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaInicio.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad).Replace("$$colaborador$$", actividad.Encargado.NombreCompleto);
                        EnviarCorreo(actividad.Fase.Auditoria.UsuarioRealiza.Email, "actividad de auditoría a realizarse pronto", readText2);
                    }
                }

                var actividadesFinalizar = db.Actividades.Where(a => a.Eliminado != true && a.Fase.Eliminado != true && a.Fase.Auditoria.Elimanado != true && a.Fase.Auditoria.Plan.Eliminado != true && a.Fase.Auditoria.Plan.IdEstado == 3 && a.FechaFin == FechaAviso && a.IdEstado!=2).Include(a => a.Fase.Auditoria.Plan).Include(a => a.Fase.Auditoria.UsuarioRealiza).ToList();
                foreach (Actividades actividad in actividadesFinalizar)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividadFinaliza.html");
                    readText = readText.Replace("$$nombre$$", actividad.Encargado.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaFin.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad);
                    EnviarCorreo(actividad.Encargado.Email, "Actividad a punto de finalizar", readText);
                    if (actividad.Encargado.Id != actividad.Fase.Auditoria.UsuarioRealiza.Id)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividadFinalizaAuditorPrincipal.html");
                        readText2 = readText2.Replace("$$nombre$$", actividad.Fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaFin.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad).Replace("$$colaborador$$", actividad.Encargado.NombreCompleto);
                        EnviarCorreo(actividad.Fase.Auditoria.UsuarioRealiza.Email, "Actividad a punto de finalizar", readText2);
                    }
                }

                var actividadesVencidas = db.Actividades.Where(a => a.Eliminado != true && a.Fase.Eliminado != true && a.Fase.Auditoria.Elimanado != true && a.Fase.Auditoria.Plan.Eliminado != true && a.Fase.Auditoria.Plan.IdEstado == 3 && a.FechaFin <hoy && a.IdEstado != 2).Include(a => a.Fase.Auditoria.Plan).Include(a => a.Fase.Auditoria.UsuarioRealiza).ToList();
                foreach (Actividades actividad in actividadesVencidas)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividadVencida.html");
                    readText = readText.Replace("$$nombre$$", actividad.Encargado.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaFin.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad);
                    EnviarCorreo(actividad.Encargado.Email, "Actividad fuera de tiempo", readText);
                    if (actividad.Encargado.Id != actividad.Fase.Auditoria.UsuarioRealiza.Id)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividadVencidaAuditorPrincipal.html");
                        readText2 = readText2.Replace("$$nombre$$", actividad.Fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaFin.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad).Replace("$$colaborador$$", actividad.Encargado.NombreCompleto);
                        EnviarCorreo(actividad.Fase.Auditoria.UsuarioRealiza.Email, "Actividad fuera de tiempo", readText2);
                    }
                }

                //notificación de planes de acción
                var planes = db.PlanesDeAccions.Where(p => p.Eliminado != true && p.Hallazgo.Eliminado != true && p.Hallazgo.Actividad.Eliminado != true && p.Hallazgo.Actividad.Fase.Eliminado != true && p.Hallazgo.Actividad.Fase.Auditoria.Elimanado != true && p.Hallazgo.Actividad.Fase.Auditoria.Plan.Eliminado != true && p.Hallazgo.Actividad.Fase.Auditoria.Plan.IdEstado == 3 && p.IdEstado != 2 && p.FechaVencimiento == FechaAviso).Include(p => p.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza).ToList();
                foreach (PlanesDeAccion plan in planes)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionPlanAccion.html");
                    readText = readText.Replace("$$nombre$$", plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", plan.Hallazgo.Actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", plan.FechaVencimiento.ToShortDateString()).Replace("$$plan$$", plan.NombrePlanAccion).Replace("$$descripcion$$", plan.DescripcionPlanAccion);
                    EnviarCorreo(plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.Email, "Plan de acción a punto de vencer.", readText);
                    if (plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.Email!=plan.Encargado.Email) {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionPlanAccion.html");
                        readText2 = readText2.Replace("$$nombre$$", plan.Encargado.NombreCompleto).Replace("$$auditoria$$", plan.Hallazgo.Actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", plan.FechaVencimiento.ToShortDateString()).Replace("$$plan$$", plan.NombrePlanAccion).Replace("$$descripcion$$", plan.DescripcionPlanAccion);
                        EnviarCorreo(plan.Encargado.Email, "Plan de acción a punto de vencer.", readText2);
                    }
                }

                var planesVencidos = db.PlanesDeAccions.Where(p => p.Eliminado != true && p.Hallazgo.Eliminado != true && p.Hallazgo.Actividad.Eliminado != true && p.Hallazgo.Actividad.Fase.Eliminado != true && p.Hallazgo.Actividad.Fase.Auditoria.Elimanado != true && p.Hallazgo.Actividad.Fase.Auditoria.Plan.Eliminado != true && p.Hallazgo.Actividad.Fase.Auditoria.Plan.IdEstado == 3 && p.IdEstado != 2 && p.FechaVencimiento < hoy).Include(p => p.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza).ToList();
                foreach (PlanesDeAccion plan in planesVencidos)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionPlanAccionVencido.html");
                    readText = readText.Replace("$$nombre$$", plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", plan.Hallazgo.Actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", plan.FechaVencimiento.ToShortDateString()).Replace("$$plan$$", plan.NombrePlanAccion).Replace("$$descripcion$$", plan.DescripcionPlanAccion);
                    EnviarCorreo(plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.Email, "Plan de acción fuera de tiempo.", readText);
                    if (plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.Id != plan.Encargado.Id)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionPlanAccionVencido.html");
                        readText2 = readText2.Replace("$$nombre$$", plan.Encargado.NombreCompleto).Replace("$$auditoria$$", plan.Hallazgo.Actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", plan.FechaVencimiento.ToShortDateString()).Replace("$$plan$$", plan.NombrePlanAccion).Replace("$$descripcion$$", plan.DescripcionPlanAccion);
                        EnviarCorreo(plan.Encargado.Email, "Plan de acción fuera de tiempo.", readText2);
                    }
                }

                //auditorías
                var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.Plan.Eliminado != true && a.FechaInicio == FechaAviso && a.Plan.IdEstado == 3 && a.IdEstado == 1).ToList();
                foreach(Auditorias audit in auditorias)
                {
                    string readTextAuditor = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionAuditoriaAuditor.html");
                    readTextAuditor = readTextAuditor.Replace("$$nombre$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaInicio.ToShortDateString()).Replace("$$colaborador$$", audit.DepartamentoRealizar.PersonaACargo.NombreCompleto).Replace("$$area$$", audit.DepartamentoRealizar.NombreDepartamento);
                    EnviarCorreo(audit.UsuarioRealiza.Email, "Notificación de inicio de auditoría.", readTextAuditor);
                }

                var auditoriasFinalizan = db.Auditorias.Where(a => a.Elimanado != true && a.Plan.Eliminado != true && a.FechaFin == FechaAviso && a.Plan.IdEstado == 3 && a.IdEstado == 1).ToList();
                foreach (Auditorias audit in auditoriasFinalizan)
                {
                    string readTextAuditor = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionAuditoriaAuditor3d.html");
                    readTextAuditor = readTextAuditor.Replace("$$nombre$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaFin.ToShortDateString());
                    EnviarCorreo(audit.UsuarioRealiza.Email, "Notificación de fin de auditoría.", readTextAuditor);
                }

                var auditoriasFin = db.Auditorias.Where(a => a.Elimanado != true && a.Plan.Eliminado != true && a.FechaFin < hoy && a.Plan.IdEstado == 3 && a.IdEstado == 1).ToList();
                foreach (Auditorias audit in auditoriasFin)
                {
                    string readTextAuditor = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionAuditoriaVencida.html");
                    readTextAuditor = readTextAuditor.Replace("$$nombre$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaFin.ToShortDateString());
                    EnviarCorreo(audit.UsuarioRealiza.Email, "Auditoría fuera de tiempo.", readTextAuditor);
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
                var hoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var FechaAviso = hoy.AddDays(3);
                //Notificacion de fases
                var fases = db.Fases.Where(f => f.Eliminado != true && f.Auditoria.Elimanado != true && f.Auditoria.Plan.Eliminado != true && f.Auditoria.Plan.IdEstado == 3 && f.FechaInicio == FechaAviso).Include(f => f.Auditoria.Plan).Include(f => f.Auditoria.UsuarioRealiza).ToList();
                foreach (Fases fase in fases)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionFase.html");
                    readText = readText.Replace("$$nombre$$", fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", fase.Auditoria.Auditoria).Replace("$$fecha$$", fase.FechaInicio.ToShortDateString()).Replace("$$fase$$", fase.Fase);
                    EnviarCorreo(fase.Auditoria.UsuarioRealiza.Email, "Fase de auditoría a realizarse pronto", readText);
                }

                var fasesFinalizan = db.Fases.Where(f => f.Eliminado != true && f.Auditoria.Elimanado != true && f.Auditoria.Plan.Eliminado != true && f.Auditoria.Plan.IdEstado == 3 && f.FechaFin == FechaAviso && f.IdEstado != 2).Include(f => f.Auditoria.Plan).Include(f => f.Auditoria.UsuarioRealiza).ToList();
                foreach (Fases fase in fasesFinalizan)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionFaseFinaliza.html");
                    readText = readText.Replace("$$nombre$$", fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", fase.Auditoria.Auditoria).Replace("$$fecha$$", fase.FechaFin.ToShortDateString()).Replace("$$fase$$", fase.Fase);
                    EnviarCorreo(fase.Auditoria.UsuarioRealiza.Email, "Fase a punto de finalizar.", readText);
                }

                var fasesVencidas = db.Fases.Where(f => f.Eliminado != true && f.Auditoria.Elimanado != true && f.Auditoria.Plan.Eliminado != true && f.Auditoria.Plan.IdEstado == 3 && f.FechaFin < hoy && f.IdEstado != 2).Include(f => f.Auditoria.Plan).Include(f => f.Auditoria.UsuarioRealiza).ToList();
                foreach (Fases fase in fasesVencidas)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionFaseVencida.html");
                    readText = readText.Replace("$$nombre$$", fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", fase.Auditoria.Auditoria).Replace("$$fecha$$", fase.FechaFin.ToShortDateString()).Replace("$$fase$$", fase.Fase);
                    EnviarCorreo(fase.Auditoria.UsuarioRealiza.Email, "Fase fuera de tiempo.", readText);
                }

                //notificación de actividades
                var actividades = db.Actividades.Where(a => a.Eliminado != true && a.Fase.Eliminado != true && a.Fase.Auditoria.Elimanado != true && a.Fase.Auditoria.Plan.Eliminado != true && a.Fase.Auditoria.Plan.IdEstado == 3 && a.FechaInicio == FechaAviso).Include(a => a.Fase.Auditoria.Plan).Include(a => a.Fase.Auditoria.UsuarioRealiza).Include(a => a.Encargado).ToList();
                foreach (Actividades actividad in actividades)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividad.html");
                    readText = readText.Replace("$$nombre$$", actividad.Encargado.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaInicio.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad);
                    EnviarCorreo(actividad.Encargado.Email, "actividad de auditoría a realizarse pronto", readText);
                    if (actividad.Encargado.Id != actividad.Fase.Auditoria.UsuarioRealiza.Id)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividadAuditorPrincipal.html");
                        readText2 = readText2.Replace("$$nombre$$", actividad.Fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaInicio.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad).Replace("$$colaborador$$", actividad.Encargado.NombreCompleto);
                        EnviarCorreo(actividad.Fase.Auditoria.UsuarioRealiza.Email, "actividad de auditoría a realizarse pronto", readText2);
                    }
                }

                var actividadesFinalizar = db.Actividades.Where(a => a.Eliminado != true && a.Fase.Eliminado != true && a.Fase.Auditoria.Elimanado != true && a.Fase.Auditoria.Plan.Eliminado != true && a.Fase.Auditoria.Plan.IdEstado == 3 && a.FechaFin == FechaAviso && a.IdEstado != 2).Include(a => a.Fase.Auditoria.Plan).Include(a => a.Fase.Auditoria.UsuarioRealiza).ToList();
                foreach (Actividades actividad in actividadesFinalizar)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividadFinaliza.html");
                    readText = readText.Replace("$$nombre$$", actividad.Encargado.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaFin.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad);
                    EnviarCorreo(actividad.Encargado.Email, "Actividad a punto de finalizar", readText);
                    if (actividad.Encargado.Id != actividad.Fase.Auditoria.UsuarioRealiza.Id)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividadFinalizaAuditorPrincipal.html");
                        readText2 = readText2.Replace("$$nombre$$", actividad.Fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaFin.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad).Replace("$$colaborador$$", actividad.Encargado.NombreCompleto);
                        EnviarCorreo(actividad.Fase.Auditoria.UsuarioRealiza.Email, "Actividad a punto de finalizar", readText2);
                    }
                }

                var actividadesVencidas = db.Actividades.Where(a => a.Eliminado != true && a.Fase.Eliminado != true && a.Fase.Auditoria.Elimanado != true && a.Fase.Auditoria.Plan.Eliminado != true && a.Fase.Auditoria.Plan.IdEstado == 3 && a.FechaFin < hoy && a.IdEstado != 2).Include(a => a.Fase.Auditoria.Plan).Include(a => a.Fase.Auditoria.UsuarioRealiza).ToList();
                foreach (Actividades actividad in actividadesVencidas)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividadVencida.html");
                    readText = readText.Replace("$$nombre$$", actividad.Encargado.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaFin.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad);
                    EnviarCorreo(actividad.Encargado.Email, "Actividad fuera de tiempo", readText);
                    if (actividad.Encargado.Id != actividad.Fase.Auditoria.UsuarioRealiza.Id)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionActividadVencidaAuditorPrincipal.html");
                        readText2 = readText2.Replace("$$nombre$$", actividad.Fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", actividad.FechaFin.ToShortDateString()).Replace("$$fase$$", actividad.Fase.Fase).Replace("$$actividad$$", actividad.Actividad).Replace("$$colaborador$$", actividad.Encargado.NombreCompleto);
                        EnviarCorreo(actividad.Fase.Auditoria.UsuarioRealiza.Email, "Actividad fuera de tiempo", readText2);
                    }
                }

                //notificación de planes de acción
                var planes = db.PlanesDeAccions.Where(p => p.Eliminado != true && p.Hallazgo.Eliminado != true && p.Hallazgo.Actividad.Eliminado != true && p.Hallazgo.Actividad.Fase.Eliminado != true && p.Hallazgo.Actividad.Fase.Auditoria.Elimanado != true && p.Hallazgo.Actividad.Fase.Auditoria.Plan.Eliminado != true && p.Hallazgo.Actividad.Fase.Auditoria.Plan.IdEstado == 3 && p.IdEstado != 2 && p.FechaVencimiento == FechaAviso).Include(p => p.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza).ToList();
                foreach (PlanesDeAccion plan in planes)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionPlanAccion.html");
                    readText = readText.Replace("$$nombre$$", plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", plan.Hallazgo.Actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", plan.FechaVencimiento.ToShortDateString()).Replace("$$plan$$", plan.NombrePlanAccion).Replace("$$descripcion$$", plan.DescripcionPlanAccion);
                    EnviarCorreo(plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.Email, "Plan de acción a punto de vencer.", readText);
                    if (plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.Email != plan.Encargado.Email)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionPlanAccion.html");
                        readText2 = readText2.Replace("$$nombre$$", plan.Encargado.NombreCompleto).Replace("$$auditoria$$", plan.Hallazgo.Actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", plan.FechaVencimiento.ToShortDateString()).Replace("$$plan$$", plan.NombrePlanAccion).Replace("$$descripcion$$", plan.DescripcionPlanAccion);
                        EnviarCorreo(plan.Encargado.Email, "Plan de acción a punto de vencer.", readText2);
                    }
                }

                var planesVencidos = db.PlanesDeAccions.Where(p => p.Eliminado != true && p.Hallazgo.Eliminado != true && p.Hallazgo.Actividad.Eliminado != true && p.Hallazgo.Actividad.Fase.Eliminado != true && p.Hallazgo.Actividad.Fase.Auditoria.Elimanado != true && p.Hallazgo.Actividad.Fase.Auditoria.Plan.Eliminado != true && p.Hallazgo.Actividad.Fase.Auditoria.Plan.IdEstado == 3 && p.IdEstado != 2 && p.FechaVencimiento < hoy).Include(p => p.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza).ToList();
                foreach (PlanesDeAccion plan in planesVencidos)
                {
                    string readText = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionPlanAccionVencido.html");
                    readText = readText.Replace("$$nombre$$", plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", plan.Hallazgo.Actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", plan.FechaVencimiento.ToShortDateString()).Replace("$$plan$$", plan.NombrePlanAccion).Replace("$$descripcion$$", plan.DescripcionPlanAccion);
                    EnviarCorreo(plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.Email, "Plan de acción fuera de tiempo.", readText);
                    if (plan.Hallazgo.Actividad.Fase.Auditoria.UsuarioRealiza.Id != plan.Encargado.Id)
                    {
                        string readText2 = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionPlanAccionVencido.html");
                        readText2 = readText2.Replace("$$nombre$$", plan.Encargado.NombreCompleto).Replace("$$auditoria$$", plan.Hallazgo.Actividad.Fase.Auditoria.Auditoria).Replace("$$fecha$$", plan.FechaVencimiento.ToShortDateString()).Replace("$$plan$$", plan.NombrePlanAccion).Replace("$$descripcion$$", plan.DescripcionPlanAccion);
                        EnviarCorreo(plan.Encargado.Email, "Plan de acción fuera de tiempo.", readText2);
                    }
                }

                //auditorías
                var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.Plan.Eliminado != true && a.FechaInicio == FechaAviso && a.Plan.IdEstado == 3 && a.IdEstado == 1).ToList();
                foreach (Auditorias audit in auditorias)
                {
                    string readTextAuditor = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionAuditoriaAuditor.html");
                    readTextAuditor = readTextAuditor.Replace("$$nombre$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaInicio.ToShortDateString()).Replace("$$colaborador$$", audit.DepartamentoRealizar.PersonaACargo.NombreCompleto).Replace("$$area$$", audit.DepartamentoRealizar.NombreDepartamento);
                    EnviarCorreo(audit.UsuarioRealiza.Email, "Notificación de inicio de auditoría.", readTextAuditor);
                }

                var auditoriasFinalizan = db.Auditorias.Where(a => a.Elimanado != true && a.Plan.Eliminado != true && a.FechaFin == FechaAviso && a.Plan.IdEstado == 3 && a.IdEstado == 1).ToList();
                foreach (Auditorias audit in auditoriasFinalizan)
                {
                    string readTextAuditor = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionAuditoriaAuditor3d.html");
                    readTextAuditor = readTextAuditor.Replace("$$nombre$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaFin.ToShortDateString());
                    EnviarCorreo(audit.UsuarioRealiza.Email, "Notificación de fin de auditoría.", readTextAuditor);
                }

                var auditoriasFin = db.Auditorias.Where(a => a.Elimanado != true && a.Plan.Eliminado != true && a.FechaFin < hoy && a.Plan.IdEstado == 3 && a.IdEstado == 1).ToList();
                foreach (Auditorias audit in auditoriasFin)
                {
                    string readTextAuditor = System.IO.File.ReadAllText(@"C:\FormatosCorreo\SAI\notificacionAuditoriaVencida.html");
                    readTextAuditor = readTextAuditor.Replace("$$nombre$$", audit.UsuarioRealiza.NombreCompleto).Replace("$$auditoria$$", audit.Auditoria).Replace("$$fecha$$", audit.FechaFin.ToShortDateString());
                    EnviarCorreo(audit.UsuarioRealiza.Email, "Auditoría fuera de tiempo.", readTextAuditor);
                }
            }
            catch (Exception e)
            {
                ret = false;
                Console.Error.WriteLine(e.Message);
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