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
    public class PlanesDeAccionController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PlanesDeAccion
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hall = db.Hallazgos.Where(h => h.IdHallazgo == id).SingleOrDefault();
            ViewBag.idHallazgo = id;
            ViewBag.nombreHallazgo = hall.Hallazgo;
            ViewBag.idActividad = hall.IdActividad;
            var planesDeAccions = db.PlanesDeAccions.Where(p => p.Eliminado != true && p.IdHallazgo == id).Include(p => p.Encargado).Include(p => p.Estado).Include(p => p.Hallazgo);
            return View(planesDeAccions.ToList());
        }

        // GET: PlanesDeAccion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanesDeAccion planesDeAccion = db.PlanesDeAccions.Find(id);
            if (planesDeAccion == null)
            {
                return HttpNotFound();
            }
            return View(planesDeAccion);
        }

        // GET: PlanesDeAccion/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.IdEncargado = new SelectList(db.Users.Where(u=>u.Eliminado!=true && u.UserName!="Admin" && u.Nombres!="Administrador" && u.Apellidos!="Administrador").OrderBy(u=>u.Nombres).ThenBy(u=>u.Apellidos), "Id", "NombreCompleto",GetUserId(User));
            ViewBag.IdDirectorValidador = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Roles.Any(r => r.RoleId == "99f5f9a5-5998-412f-9c06-0a3459809316")).OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto");
            PlanesDeAccion plan = new PlanesDeAccion();
            plan.IdHallazgo = (int)id;
            plan.FechaCrea = DateTime.Now;
            plan.FechaVencimiento = DateTime.Now;
            plan.IdDirectorValidador = "";
            return View(plan);
        }

        // POST: PlanesDeAccion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPlanAccion,NombrePlanAccion,DescripcionPlanAccion,FechaVencimiento,Eliminado,IdHallazgo,IdEncargado,IdEstado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica,IdDirectorValidador")] PlanesDeAccion planesDeAccion)
        {
            if (ModelState.IsValid)
            {
                planesDeAccion.IdEstado = 1;
                planesDeAccion.UsuarioCrea = GetUserId(User);
                planesDeAccion.FechaCrea = DateTime.Now;
                db.PlanesDeAccions.Add(planesDeAccion);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = planesDeAccion.IdHallazgo });
            }

            ViewBag.IdEncargado = new SelectList(db.Users.Where(u => u.Eliminado != true && u.UserName != "Admin" && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", planesDeAccion.IdEncargado);
            ViewBag.IdDirectorValidador = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Roles.Any(r => r.RoleId == "99f5f9a5-5998-412f-9c06-0a3459809316")).OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto");
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", planesDeAccion.IdEstado);
            ViewBag.IdHallazgo = new SelectList(db.Hallazgos, "IdHallazgo", "Hallazgo", planesDeAccion.IdHallazgo);
            return View(planesDeAccion);
        }

        // GET: PlanesDeAccion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanesDeAccion planesDeAccion = db.PlanesDeAccions.Find(id);
            if (planesDeAccion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEncargado = new SelectList(db.Users.Where(u => u.Eliminado != true && u.UserName != "Admin" && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", planesDeAccion.IdEncargado);
            ViewBag.IdDirectorValidador = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Roles.Any(r => r.RoleId == "99f5f9a5-5998-412f-9c06-0a3459809316")).OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto",planesDeAccion.IdDirectorValidador);
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", planesDeAccion.IdEstado);
            ViewBag.IdHallazgo = new SelectList(db.Hallazgos, "IdHallazgo", "Hallazgo", planesDeAccion.IdHallazgo);
            return View(planesDeAccion);
        }

        // POST: PlanesDeAccion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPlanAccion,NombrePlanAccion,DescripcionPlanAccion,FechaVencimiento,Eliminado,IdHallazgo,IdEncargado,IdEstado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica,IdDirectorValidador")] PlanesDeAccion planesDeAccion)
        {
            if (ModelState.IsValid)
            {
                planesDeAccion.UsuarioModifica = GetUserId(User);
                planesDeAccion.FechaModifica = DateTime.Now;
                db.Entry(planesDeAccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { id=planesDeAccion.IdHallazgo});
            }
            ViewBag.IdEncargado = new SelectList(db.Users.Where(u => u.Eliminado != true && u.UserName != "Admin" && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", planesDeAccion.IdEncargado);
            ViewBag.IdDirectorValidador = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Roles.Any(r => r.RoleId == "99f5f9a5-5998-412f-9c06-0a3459809316")).OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto",planesDeAccion.IdDirectorValidador);
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", planesDeAccion.IdEstado);
            ViewBag.IdHallazgo = new SelectList(db.Hallazgos, "IdHallazgo", "Hallazgo", planesDeAccion.IdHallazgo);
            return View(planesDeAccion);
        }

        // GET: PlanesDeAccion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanesDeAccion planesDeAccion = db.PlanesDeAccions.Find(id);
            if (planesDeAccion == null)
            {
                return HttpNotFound();
            }
            return View(planesDeAccion);
        }

        // POST: PlanesDeAccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlanesDeAccion planesDeAccion = db.PlanesDeAccions.Find(id);
            planesDeAccion.Eliminado = true;
            planesDeAccion.UsuarioModifica = GetUserId();
            planesDeAccion.FechaModifica = DateTime.Now;
            db.Entry(planesDeAccion).State=EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index",new { id=planesDeAccion.IdHallazgo});
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
