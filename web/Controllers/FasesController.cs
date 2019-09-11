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
    [Authorize]
    public class FasesController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Fases
        public ActionResult Index(int? idAuditoria, string nombreAuditoria)
        {
            if (idAuditoria == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.nombreAuditoria = nombreAuditoria;
            ViewBag.idAuditoria = idAuditoria;
            var fases = db.Fases.Where(f=>f.IdAuditoria==idAuditoria && f.Eliminado!=true).Include(f => f.Auditoria).Include(f => f.Estado).OrderBy(f=>f.FechaInicio);
            return View(fases.ToList());
        }

        // GET: Fases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fases fases = db.Fases.Find(id);
            if (fases == null)
            {
                return HttpNotFound();
            }
            return View(fases);
        }

        // GET: Fases/Create
        public ActionResult Create(int idAuditoria, string nombreAuditoria)
        {
            ViewBag.IdAuditoria = new SelectList(db.Auditorias, "IdAuditoria", "Auditoria");
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado");
            Fases fase = new Fases();
            fase.FechaInicio = DateTime.Now;
            fase.FechaFin = fase.FechaInicio.AddDays(11);
            fase.IdAuditoria = idAuditoria;
            fase.Porcentaje = 20.0;
            ViewBag.idAuditoriaRet = idAuditoria;
            ViewBag.nombreAuditoria = nombreAuditoria;
            return View(fase);
        }

        // POST: Fases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fases fases)
        {
            var aud = db.Auditorias.Where(a => a.IdAuditoria == fases.IdAuditoria).Include(a => a.Fases).SingleOrDefault();
            if (ModelState.IsValid)
            {
                fases.FechaCrea = DateTime.Now;
                fases.FechaModifica = fases.FechaCrea;
                fases.UsuarioCrea = GetUserId(User);
                fases.IdEstado = 1;
                fases.Eliminado = false;
                fases.Porcentaje = fases.Porcentaje / 100;
                var porcent = 0.0;
                foreach(var item in aud.Fases.Where(f=>f.Eliminado!=true))
                {
                    porcent += item.Porcentaje;
                }
                porcent += fases.Porcentaje;
                if (porcent <= 1.0)
                {
                    db.Fases.Add(fases);
                    db.SaveChanges();
                    return RedirectToAction("Index",new { idAuditoria=aud.IdAuditoria, nombreAuditoria=aud.Auditoria});
                }else
                {
                    ModelState.AddModelError("", "La sumatoria de porcentajes sobrepasa el 100%.");
                    ViewBag.IdAuditoria = new SelectList(db.Auditorias, "IdAuditoria", "Auditoria", fases.IdAuditoria);
                    ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", fases.IdEstado);
                    ViewBag.idAuditoriaRet = aud.IdAuditoria;
                    ViewBag.nombreAuditoria = aud.Auditoria;
                    fases.Porcentaje = fases.Porcentaje * 100;
                    return View(fases);
                }
            }

            ViewBag.IdAuditoria = new SelectList(db.Auditorias, "IdAuditoria", "Auditoria", fases.IdAuditoria);
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", fases.IdEstado);
            ViewBag.idAuditoriaRet = aud.IdAuditoria;
            ViewBag.nombreAuditoria = aud.Auditoria;
            //fases.Porcentaje = fases.Porcentaje * 100;
            return View(fases);
        }

        // GET: Fases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fases fases = db.Fases.Find(id);
            if (fases == null)
            {
                return HttpNotFound();
            }
            ViewBag.idAuditoriaRet = fases.IdAuditoria;
            ViewBag.nombreAuditoria = fases.Auditoria.Auditoria;
            ViewBag.IdAuditoria = new SelectList(db.Auditorias, "IdAuditoria", "Auditoria", fases.IdAuditoria);
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", fases.IdEstado);
            fases.Porcentaje = fases.Porcentaje * 100;
            return View(fases);
        }

        // POST: Fases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fases fases)
        {
            var audaux = db.Auditorias.Where(a => a.IdAuditoria == fases.IdAuditoria).SingleOrDefault();
            if (ModelState.IsValid)
            {
                fases.Porcentaje = fases.Porcentaje / 100;
                var porcent = 0.0;
                var fasesAux = db.Fases.Where(f => f.Eliminado != true && f.IdAuditoria== fases.IdAuditoria && f.IdFase!=fases.IdFase).ToList().AsReadOnly();
                //var fasesAux = audaux.Fases.Where(f => f.Eliminado != true).ToList().AsReadOnly();
                foreach (var item in fasesAux)
                {
                    if (item.IdFase != fases.IdFase)
                    {
                        porcent += item.Porcentaje;
                    }
                }
                porcent += fases.Porcentaje;
                if (porcent <= 1.0)
                {
                    db.Entry(fases).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idAuditoria = fases.IdAuditoria, nombreAuditoria = audaux.Auditoria });
                }
                ModelState.AddModelError("", "La sumatoria de porcentajes sobrepasa el 100%.");
                ViewBag.IdAuditoria = new SelectList(db.Auditorias, "IdAuditoria", "Auditoria", fases.IdAuditoria);
                ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", fases.IdEstado);
                ViewBag.idAuditoriaRet = fases.IdAuditoria;
                ViewBag.nombreAuditoria = audaux.Auditoria;
                return View(fases);
            }
            ViewBag.IdAuditoria = new SelectList(db.Auditorias, "IdAuditoria", "Auditoria", fases.IdAuditoria);
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", fases.IdEstado);
            ViewBag.idAuditoriaRet = fases.IdAuditoria;
            ViewBag.nombreAuditoria = audaux.Auditoria;
            return View(fases);
        }

        // GET: Fases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fases fases = db.Fases.Find(id);
            if (fases == null)
            {
                return HttpNotFound();
            }
            return View(fases);
        }

        // POST: Fases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fases fases = db.Fases.Include(f=>f.Auditoria).Where(f=>f.IdFase==id).SingleOrDefault();
            var aud = db.Auditorias.Where(a => a.IdAuditoria == fases.IdAuditoria).SingleOrDefault();
            fases.Eliminado = true;
            fases.UsuarioModifica = GetUserId(User);
            db.Entry(fases).State=EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index",new { idAuditoria=fases.IdAuditoria,nombreAuditoria=aud.Auditoria});
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
