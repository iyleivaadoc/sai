using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using web.ViewModels;

namespace web.Models
{
    public class ReportesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Reportes
        public ActionResult EstadoPlan()
        {
            var model = new ReportsVM();
            var planes= db.Planes.Where(p => p.Eliminado != true).OrderByDescending(p => p.anio).ToList();
            List<SelectListItem> PlanesList = new List<SelectListItem>
            {
                new SelectListItem() {Value = "0",Text = "Todos"},
            };
            foreach(Planes plan in planes)
            {
                PlanesList.Add(new SelectListItem() { Value = plan.IdPlan.ToString(), Text = plan.NombrePlan });
            }
            var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.IdPlan == model.idPlan).OrderBy(a => a.FechaInicio).ToList();
            List<SelectListItem> auditoriasList = new List<SelectListItem>
            {
                new SelectListItem() {Value = "0",Text = "Todas"},
            };
            foreach(Auditorias auditoria in auditorias)
            {
                auditoriasList.Add(new SelectListItem() { Value = auditoria.IdAuditoria.ToString(), Text = auditoria.Auditoria });
            }
            ViewBag.TypesList = PlanesList;
            ViewBag.SubTypesList = auditoriasList;
            return View(model);
        }


        public ActionResult EstadoPlanEjecutivo()
        {
            var model = new ReportsVM();
            var planes = db.Planes.Where(p => p.Eliminado != true).OrderByDescending(p => p.anio).ToList();
            List<SelectListItem> PlanesList = new List<SelectListItem>
            {
                new SelectListItem() {Value = "0",Text = "Todos"},
            };
            foreach (Planes plan in planes)
            {
                PlanesList.Add(new SelectListItem() { Value = plan.IdPlan.ToString(), Text = plan.NombrePlan });
            }
            ViewBag.TypesList = PlanesList;
            return View(model);
        }


        public ActionResult PlanesdeAccion()
        {
            var model = new ReportsVM();
            var planes = db.Planes.Where(p => p.Eliminado != true).OrderByDescending(p => p.anio).ToList();
            List<SelectListItem> PlanesList = new List<SelectListItem>
            {
                new SelectListItem() {Value = "0",Text = "Todos"},
            };
            foreach (Planes plan in planes)
            {
                PlanesList.Add(new SelectListItem() { Value = plan.IdPlan.ToString(), Text = plan.NombrePlan });
            }
            var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.IdPlan == model.idPlan).OrderBy(a => a.FechaInicio).ToList();
            List<SelectListItem> auditoriasList = new List<SelectListItem>
            {
                new SelectListItem() {Value = "0",Text = "Todas"},
            };
            foreach (Auditorias auditoria in auditorias)
            {
                auditoriasList.Add(new SelectListItem() { Value = auditoria.IdAuditoria.ToString(), Text = auditoria.Auditoria });
            }
            ViewBag.TypesList = PlanesList;
            ViewBag.SubTypesList = auditoriasList;
            return View(model);
        }


        public ActionResult GetSubTypes(int? id)
        {
            var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.IdPlan == id).OrderBy(a => a.FechaInicio).ToList();
            List<SelectListItem> auditoriasList = new List<SelectListItem>
            {
                new SelectListItem() {Value = "0",Text = "Todas"},
            };
            foreach (Auditorias auditoria in auditorias)
            {
                auditoriasList.Add(new SelectListItem() { Value = auditoria.IdAuditoria.ToString(), Text = auditoria.Auditoria });
            }
            return Json(auditoriasList, JsonRequestBehavior.AllowGet);
        }
    }
}