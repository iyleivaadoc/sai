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
    public class DireccionsController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Direccions
        public ActionResult Index()
        {
            var direccions = db.Direccions.Where(d=>d.Eliminado!=true).Include(d => d.PersonaCargo);
            return View(direccions.ToList());
        }

        // GET: Direccions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direccion direccion = db.Direccions.Find(id);
            if (direccion == null)
            {
                return HttpNotFound();
            }
            return View(direccion);
        }

        // GET: Direccions/Create
        public ActionResult Create()
        {
            ViewBag.IdPersonaACargo = new SelectList(db.Users.Where(u=>u.Eliminado!=true && u.UserName!="Admin").OrderBy(u=>u.Nombres).ThenBy(u=>u.Apellidos), "Id", "NombreCompleto");
            Direccion d = new Direccion();
            d.FechaCrea = DateTime.Now;
            return View(d);
        }

        // POST: Direccions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDireccion,DireccionNombre,Descripcion,IdPersonaACargo,Eliminado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                direccion.FechaCrea = DateTime.Now;
                direccion.UsuarioCrea = GetUserId(User);
                db.Direccions.Add(direccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPersonaACargo = new SelectList(db.Users.Where(u => u.Eliminado != true && u.UserName != "Admin").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "Nombres", direccion.IdPersonaACargo);
            return View(direccion);
        }

        // GET: Direccions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direccion direccion = db.Direccions.Find(id);
            if (direccion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPersonaACargo = new SelectList(db.Users.Where(u => u.Eliminado != true && u.UserName != "Admin").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", direccion.IdPersonaACargo);
            return View(direccion);
        }

        // POST: Direccions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDireccion,DireccionNombre,Descripcion,IdPersonaACargo,Eliminado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                direccion.UsuarioModifica = GetUserId(User);
                direccion.FechaModifica = DateTime.Now;
                db.Entry(direccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPersonaACargo = new SelectList(db.Users.Where(u => u.Eliminado != true && u.UserName != "Admin").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", direccion.IdPersonaACargo);
            return View(direccion);
        }

        // GET: Direccions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direccion direccion = db.Direccions.Find(id);
            if (direccion == null)
            {
                return HttpNotFound();
            }
            return View(direccion);
        }

        // POST: Direccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Direccion direccion = db.Direccions.Find(id);
            direccion.Eliminado = true;
            direccion.UsuarioModifica = GetUserId(User);
            direccion.FechaModifica = DateTime.Now;
            db.Entry(direccion).State=EntityState.Modified;
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
