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
    public class DepartamentosController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Departamentos
        public ActionResult Index()
        {
            var departamentos = db.Departamentos.Where(d => d.Eliminado != true).Include(d => d.Direccion);
            return View(departamentos.ToList());
        }

        // GET: Departamentos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamentos departamentos = db.Departamentos.Find(id);
            if (departamentos == null)
            {
                return HttpNotFound();
            }
            return View(departamentos);
        }

        // GET: Departamentos/Create
        public ActionResult Create()
        {
            ViewBag.IdDireccion = new SelectList(db.Direccions.Where(di=>di.Eliminado!=true), "IdDireccion", "DireccionNombre");
            ViewBag.IdPersonaACargo = new SelectList(db.Users.Where(u=>u.Eliminado!=true && u.UserName!="Admin" && u.Nombres!="Administrador" && u.Apellidos!="Administrador").OrderBy(u=>u.Nombres).ThenBy(u=>u.Apellidos), "Id", "NombreCompleto");
            Departamentos d = new Departamentos();
            d.FechaCrea = DateTime.Now;
            return View(d);
        }

        // POST: Departamentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDepartamento,NombreDepartamento,DescripcionDepartamento,IdPersonaACargo,IdDireccion,Eliminado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica")] Departamentos departamentos)
        {
            if (ModelState.IsValid)
            {
                departamentos.FechaCrea = DateTime.Now;
                departamentos.UsuarioCrea = GetUserId(User);
                db.Departamentos.Add(departamentos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdDireccion = new SelectList(db.Direccions.Where(di => di.Eliminado != true), "IdDireccion", "DireccionNombre", departamentos.IdDireccion);
            ViewBag.IdPersonaACargo = new SelectList(db.Users.Where(u => u.Eliminado != true && u.UserName != "Admin" && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", departamentos.IdPersonaACargo);
            return View(departamentos);
        }

        // GET: Departamentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamentos departamentos = db.Departamentos.Find(id);
            if (departamentos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDireccion = new SelectList(db.Direccions.Where(di => di.Eliminado != true), "IdDireccion", "DireccionNombre", departamentos.IdDireccion);
            ViewBag.IdPersonaACargo = new SelectList(db.Users.Where(u => u.Eliminado != true && u.UserName != "Admin" && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", departamentos.IdPersonaACargo);
            return View(departamentos);
        }

        // POST: Departamentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDepartamento,NombreDepartamento,DescripcionDepartamento,IdPersonaACargo,IdDireccion,Eliminado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica")] Departamentos departamentos)
        {
            if (ModelState.IsValid)
            {
                departamentos.UsuarioModifica = GetUserId(User);
                departamentos.FechaModifica = DateTime.Now;
                db.Entry(departamentos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdDireccion = new SelectList(db.Direccions.Where(di => di.Eliminado != true), "IdDireccion", "DireccionNombre", departamentos.IdDireccion);
            ViewBag.IdPersonaACargo = new SelectList(db.Users.Where(u => u.Eliminado != true && u.UserName != "Admin" && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", departamentos.IdPersonaACargo);
            return View(departamentos);
        }

        // GET: Departamentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamentos departamentos = db.Departamentos.Find(id);
            if (departamentos == null)
            {
                return HttpNotFound();
            }
            return View(departamentos);
        }

        // POST: Departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Departamentos departamentos = db.Departamentos.Find(id);
            departamentos.Eliminado = true;
            departamentos.FechaModifica = DateTime.Now;
            departamentos.UsuarioModifica = GetUserId(User);
            db.Entry(departamentos).State=EntityState.Modified;
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
