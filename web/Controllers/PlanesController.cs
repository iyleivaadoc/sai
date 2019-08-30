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
    public class PlanesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Planes
        public ActionResult Index()
        {
            return View(db.Planes.ToList());
        }

        // GET: Planes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Planes planes = db.Planes.Find(id);
            if (planes == null)
            {
                return HttpNotFound();
            }
            return View(planes);
        }

        // GET: Planes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Planes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPlan,NombrePlan,DescripcionPlan,FechaInicio,anio,Eliminado,FechaCrea,FechaMod,UsuarioCrea,UsuarioMod")] Planes planes)
        {
            if (ModelState.IsValid)
            {
                db.Planes.Add(planes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(planes);
        }

        // GET: Planes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Planes planes = db.Planes.Find(id);
            if (planes == null)
            {
                return HttpNotFound();
            }
            return View(planes);
        }

        // POST: Planes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPlan,NombrePlan,DescripcionPlan,FechaInicio,anio,Eliminado,FechaCrea,FechaMod,UsuarioCrea,UsuarioMod")] Planes planes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(planes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(planes);
        }

        // GET: Planes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Planes planes = db.Planes.Find(id);
            if (planes == null)
            {
                return HttpNotFound();
            }
            return View(planes);
        }

        // POST: Planes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Planes planes = db.Planes.Find(id);
            db.Planes.Remove(planes);
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
