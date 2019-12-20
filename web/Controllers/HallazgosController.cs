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
    public class HallazgosController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Hallazgos
        public ActionResult Index(int? id)
        {
            if (id==null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var act = db.Actividades.Where(a => a.IdActividad == id && a.Eliminado != true).Include(a => a.Fase.Auditoria).SingleOrDefault();
            ViewBag.nombreActividad = act.Fase.Auditoria.Auditoria+"/"+act.Fase.Fase+"/"+act.Actividad+"/"+act.Fase.Auditoria.UsuarioRealiza.UserName;
            ViewBag.idActividad = id;
            ViewBag.active = act.IdEstado;
            ViewBag.idFase = act.Fase.IdFase;
            ViewBag.nombreAuditoria = act.Fase.Auditoria.Auditoria;

            var hallazgos = db.Hallazgos.Where(h=>h.Eliminado!=true && h.IdActividad==id).Include(h => h.Actividad);
            return View(hallazgos.ToList());
        }

        // GET: Hallazgos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hallazgos hallazgos = db.Hallazgos.Find(id);
            if (hallazgos == null)
            {
                return HttpNotFound();
            }
            var act = db.Actividades.Where(a => a.IdActividad == hallazgos.IdActividad && a.Eliminado != true).Include(a => a.Fase.Auditoria).SingleOrDefault();
            ViewBag.nombreActividad = act.Fase.Auditoria.Auditoria + "/" + act.Fase.Fase + "/" + act.Actividad + "/" + act.Fase.Auditoria.UsuarioRealiza.UserName;
            return View(hallazgos);
        }

        // GET: Hallazgos/Create
        public ActionResult Create(int? idActividad)
        {
            if (idActividad == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.IdActividad = new SelectList(db.Actividades, "IdActividad", "Actividad");
            var act = db.Actividades.Where(a => a.IdActividad == idActividad && a.Eliminado != true).Include(a => a.Fase.Auditoria).SingleOrDefault();
            ViewBag.nombreActividad = act.Fase.Auditoria.Auditoria + "/" + act.Fase.Fase + "/" + act.Actividad + "/" + act.Fase.Auditoria.UsuarioRealiza.UserName;
            Hallazgos h = new Hallazgos();
            h.IdActividad = (int)idActividad;
            h.FechaHallazgo = DateTime.Now;
            return View(h);
        }

        // POST: Hallazgos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdHallazgo,Hallazgo,DescripcionHallazgo,FechaHallazgo,IdActividad,Eliminado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica")] Hallazgos hallazgos)
        {
            var act = db.Actividades.Where(a => a.IdActividad == hallazgos.IdActividad && a.Eliminado != true).Include(a => a.Fase.Auditoria).SingleOrDefault();
            ViewBag.nombreActividad = act.Fase.Auditoria.Auditoria + "/" + act.Fase.Fase + "/" + act.Actividad + "/" + act.Fase.Auditoria.UsuarioRealiza.UserName;
            if (ModelState.IsValid)
            {
                hallazgos.FechaCrea = DateTime.Now;
                hallazgos.FechaModifica = hallazgos.FechaCrea;
                hallazgos.Eliminado = false;
                hallazgos.UsuarioCrea = GetUserId(User);
                db.Hallazgos.Add(hallazgos);
                db.SaveChanges();
                Session["MyAlert"] = "<script>alertify.warning('Favor no olvidar subir la evidencia correspondiente al hallazgo.')</script>";
                return RedirectToAction("Index",new { id=hallazgos.IdActividad});
            }

            ViewBag.IdActividad = new SelectList(db.Actividades, "IdActividad", "Actividad", hallazgos.IdActividad);
            return View(hallazgos);
        }

        // GET: Hallazgos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hallazgos hallazgos = db.Hallazgos.Find(id);
            if (hallazgos == null)
            {
                return HttpNotFound();
            }
            var act = db.Actividades.Where(a => a.IdActividad == hallazgos.IdActividad && a.Eliminado != true).Include(a => a.Fase.Auditoria).SingleOrDefault();
            ViewBag.nombreActividad = act.Fase.Auditoria.Auditoria + "/" + act.Fase.Fase + "/" + act.Actividad + "/" + act.Fase.Auditoria.UsuarioRealiza.UserName;
            return View(hallazgos);
        }

        // POST: Hallazgos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdHallazgo,Hallazgo,DescripcionHallazgo,FechaHallazgo,IdActividad,Eliminado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica")] Hallazgos hallazgos)
        {
            var act = db.Actividades.Where(a => a.IdActividad == hallazgos.IdActividad && a.Eliminado != true).Include(a => a.Fase.Auditoria).SingleOrDefault();
            ViewBag.nombreActividad = act.Fase.Auditoria.Auditoria + "/" + act.Fase.Fase + "/" + act.Actividad + "/" + act.Fase.Auditoria.UsuarioRealiza.UserName;
            if (ModelState.IsValid)
            {
                hallazgos.FechaModifica = DateTime.Now;
                hallazgos.UsuarioModifica = GetUserId(User);
                db.Entry(hallazgos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { id=hallazgos.IdActividad});
            }
            ViewBag.IdActividad = new SelectList(db.Actividades, "IdActividad", "Actividad", hallazgos.IdActividad);
            return View(hallazgos);
        }

        // GET: Hallazgos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hallazgos hallazgos = db.Hallazgos.Find(id);
            if (hallazgos == null)
            {
                return HttpNotFound();
            }
            return View(hallazgos);
        }

        // POST: Hallazgos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hallazgos hallazgos = db.Hallazgos.Find(id);
            hallazgos.Eliminado = true;
            hallazgos.FechaModifica = DateTime.Now;
            hallazgos.UsuarioModifica = GetUserId(User);
            db.Entry(hallazgos);
            db.SaveChanges();
            return RedirectToAction("Index",new { id=hallazgos.IdActividad});
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
