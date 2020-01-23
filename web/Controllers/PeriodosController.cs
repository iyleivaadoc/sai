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
    public class PeriodosController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Periodos
        public ActionResult Index()
        {
            return View(db.Periodos.Where(p=>p.Eliminado!=true).OrderByDescending(p => p.FechaInicio.Year).ThenBy(p => p.FechaInicio).ToList());
        }

        // GET: Periodos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodos periodos = db.Periodos.Find(id);
            if (periodos == null)
            {
                return HttpNotFound();
            }
            return View(periodos);
        }

        // GET: Periodos/Create
        public ActionResult Create()
        {
            Periodos p = new Periodos();
            p.FechaCrea = DateTime.Now;
            p.FechaInicio = DateTime.Now;
            p.FechaFin = DateTime.Now;
            return View(p);
        }

        // POST: Periodos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPeriodo,NombrePeriodo,DescripcionPeriodo,FechaInicio,FechaFin,Eliminado,FechaCrea,FechaMod,UsuarioCrea,UsuarioMod")] Periodos periodos)
        {
            if (ModelState.IsValid)
            {
                if (!periodos.betweenDateInicio)
                {
                    if (!periodos.betweenDateFin)
                    {
                        periodos.FechaCrea = DateTime.Now;
                        periodos.UsuarioCrea = GetUserId();
                        db.Periodos.Add(periodos);
                        db.SaveChanges();
                    }
                    else
                    {
                        Session["MyAlert"] = "<script>alertify.error('La fecha de finalización se encuentra dentro de otro periodo.')</script>";
                        return View(periodos);
                    }
                }
                else
                {
                    Session["MyAlert"] = "<script>alertify.error('La fecha de inicio se encuentra dentro de otro periodo.')</script>";
                    return View(periodos);
                }
                return RedirectToAction("Index");
            }

            return View(periodos);
        }

        // GET: Periodos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodos periodos = db.Periodos.Find(id);
            if (periodos == null)
            {
                return HttpNotFound();
            }
            return View(periodos);
        }

        // POST: Periodos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPeriodo,NombrePeriodo,DescripcionPeriodo,FechaInicio,FechaFin,Eliminado,FechaCrea,FechaMod,UsuarioCrea,UsuarioMod")] Periodos periodos)
        {
            if (ModelState.IsValid)
            {
                if (!periodos.betweenDateInicioEdit)
                {
                    if (!periodos.betweenDateFinEdit)
                    {
                        periodos.FechaMod = DateTime.Now;
                        periodos.UsuarioMod = GetUserId();
                        db.Entry(periodos).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        Session["MyAlert"] = "<script>alertify.error('La fecha de finalización se encuentra dentro de otro periodo.')</script>";
                        return View(periodos);
                    }
                }
                else
                {
                    Session["MyAlert"] = "<script>alertify.error('La fecha de inicio se encuentra dentro de otro periodo.')</script>";
                    return View(periodos);
                }
                return RedirectToAction("Index");
            }
            return View(periodos);
        }

        // GET: Periodos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodos periodos = db.Periodos.Find(id);
            if (periodos == null)
            {
                return HttpNotFound();
            }
            return View(periodos);
        }

        // POST: Periodos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Periodos periodos = db.Periodos.Find(id);
            periodos.FechaMod = DateTime.Now;
            periodos.UsuarioMod = GetUserId();
            periodos.Eliminado = true;
            db.Entry(periodos).State = EntityState.Modified;
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
