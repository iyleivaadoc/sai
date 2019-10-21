using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web.Models;

namespace web.Controllers
{
    public class EvidenciasActividadesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EvidenciasActividades
        public ActionResult Index()
        {
            var evidencias = db.Evidencias.Include(e => e.Actividad).Include(e => e.Hallazgo).Include(e => e.PlanAccion);
            return View(evidencias.ToList());
        }

        // GET: EvidenciasActividades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evidencias evidencias = db.Evidencias.Find(id);
            if (evidencias == null)
            {
                return HttpNotFound();
            }
            return View(evidencias);
        }

        // GET: EvidenciasActividades/Create
        public ActionResult Create()
        {
            ViewBag.IdActividad = new SelectList(db.Actividades, "IdActividad", "Actividad");
            ViewBag.IdHallazgo = new SelectList(db.Hallazgos, "IdHallazgo", "Hallazgo");
            ViewBag.IdPlanAccion = new SelectList(db.PlanesDeAccions, "IdPlanAccion", "NombrePlanAccion");
            return View();
        }

        // POST: EvidenciasActividades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEvidencia,Evidencia,DescripcionEvidencia,NombreDoc,Documento,IdHallazgo,IdActividad,IdPlanAccion,Eliminado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica")] Evidencias evidencias)
        {
            if (ModelState.IsValid)
            {
                db.Evidencias.Add(evidencias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdActividad = new SelectList(db.Actividades, "IdActividad", "Actividad", evidencias.IdActividad);
            ViewBag.IdHallazgo = new SelectList(db.Hallazgos, "IdHallazgo", "Hallazgo", evidencias.IdHallazgo);
            ViewBag.IdPlanAccion = new SelectList(db.PlanesDeAccions, "IdPlanAccion", "NombrePlanAccion", evidencias.IdPlanAccion);
            return View(evidencias);
        }

        // GET: EvidenciasActividades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evidencias evidencias = db.Evidencias.Find(id);
            if (evidencias == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdActividad = new SelectList(db.Actividades, "IdActividad", "Actividad", evidencias.IdActividad);
            ViewBag.IdHallazgo = new SelectList(db.Hallazgos, "IdHallazgo", "Hallazgo", evidencias.IdHallazgo);
            ViewBag.IdPlanAccion = new SelectList(db.PlanesDeAccions, "IdPlanAccion", "NombrePlanAccion", evidencias.IdPlanAccion);
            return View(evidencias);
        }

        // POST: EvidenciasActividades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEvidencia,Evidencia,DescripcionEvidencia,NombreDoc,Documento,IdHallazgo,IdActividad,IdPlanAccion,Eliminado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica")] Evidencias evidencias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evidencias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdActividad = new SelectList(db.Actividades, "IdActividad", "Actividad", evidencias.IdActividad);
            ViewBag.IdHallazgo = new SelectList(db.Hallazgos, "IdHallazgo", "Hallazgo", evidencias.IdHallazgo);
            ViewBag.IdPlanAccion = new SelectList(db.PlanesDeAccions, "IdPlanAccion", "NombrePlanAccion", evidencias.IdPlanAccion);
            return View(evidencias);
        }

        // GET: EvidenciasActividades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evidencias evidencias = db.Evidencias.Find(id);
            if (evidencias == null)
            {
                return HttpNotFound();
            }
            return View(evidencias);
        }

        // POST: EvidenciasActividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evidencias evidencias = db.Evidencias.Find(id);
            db.Evidencias.Remove(evidencias);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
