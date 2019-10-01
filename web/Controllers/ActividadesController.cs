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
    public class ActividadesController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Actividades
        public ActionResult Index(int?idFase)
        {
            if (idFase == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.idFase = idFase;
            var fa = db.Fases.Where(f => f.IdFase == idFase).Include(f=>f.Auditoria).SingleOrDefault();
            ViewBag.idAuditoria = fa.Auditoria.IdAuditoria;
            ViewBag.nombreAuditoria = fa.Auditoria.Auditoria;
            var actividades = db.Actividades.Where(a=>a.Eliminado!=true && a.IdFase==idFase).Include(a => a.Encargado).Include(a => a.Estado).Include(a => a.Fase.Auditoria);
            return View(actividades.ToList());
        }

        // GET: Actividades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                return HttpNotFound();
            }
            return View(actividades);
        }

        // GET: Actividades/Create
        public ActionResult Create(int? idFase)
        {
            if (idFase == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades act = new Actividades();
            act.IdEncargado = GetUserId(User);
            var fase = db.Fases.Find(idFase);
            act.IdFase = (int)idFase;
            act.FechaInicio = fase.FechaInicio;
            act.FechaFin = act.FechaInicio.AddDays(2);
            ViewBag.idFace = idFase;
            var users = db.Users.Where(u => u.Nombres != "Administrador" && u.Apellidos!= "Administrador");
            ViewBag.IdEncargado = new SelectList(users, "Id", "NombreCompleto",act.IdEncargado);
            return View(act);
        }

        // POST: Actividades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Actividades actividades)
        {
            if (ModelState.IsValid)
            {
                actividades.UsuarioCrea = GetUserId(User);
                actividades.FechaCrea = DateTime.Now;
                actividades.IdEstado = 1;
                actividades.Eliminado = false;
                var lis = db.Actividades.Where(a => a.IdFase == actividades.IdFase && a.Eliminado!=true).ToList().AsReadOnly();
                var porcent = 0.0;
                foreach(var item in lis)
                {
                    porcent += item.Porcentaje;
                }
                if ((porcent + actividades.Porcentaje) > 100)
                {
                    Session["MyAlert"] = "<script type='text/javascript'>alertify.error('El Porcentaje acumulado sobrepasa el 100%, sumatoria actual: "+(porcent+actividades.Porcentaje)+"%.');</script>";
                    ViewBag.IdEncargado = new SelectList(db.Users, "Id", "NombreCompleto", actividades.IdEncargado);
                    return View(actividades);
                }
                db.Actividades.Add(actividades);
                db.SaveChanges();
                return RedirectToAction("Index",new { IdFase =actividades.IdFase});
            }

            ViewBag.IdEncargado = new SelectList(db.Users, "Id", "NombreCompleto", actividades.IdEncargado);
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", actividades.IdEstado);
            ViewBag.IdFase = new SelectList(db.Fases, "IdFase", "Fase", actividades.IdFase);
            return View(actividades);
        }

        // GET: Actividades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEncargado = new SelectList(db.Users, "Id", "NombreCompleto", actividades.IdEncargado);
            return View(actividades);
        }

        // POST: Actividades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Actividades actividades)
        {
            if (ModelState.IsValid)
            {
                actividades.UsuarioModifica = GetUserId(User);
                actividades.FechaModifica = DateTime.Now;
                db.Entry(actividades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { idFase=actividades.IdFase});
            }
            ViewBag.IdEncargado = new SelectList(db.Users, "Id", "NombreCompleto", actividades.IdEncargado);
            return View(actividades);
        }

        // GET: Actividades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                return HttpNotFound();
            }
            return View(actividades);
        }

        // POST: Actividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Actividades actividades = db.Actividades.Find(id);
            actividades.Eliminado = true;
            actividades.UsuarioModifica = GetUserId(User);
            actividades.FechaModifica = DateTime.Now;
            db.Entry(actividades).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { idFase=actividades.IdFase});
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
