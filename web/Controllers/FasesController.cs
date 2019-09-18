using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web.Models;
using web.ViewModels;

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

        public ActionResult indexDefault(int idAuditoria, string nombreAuditoria)
        {
            ViewBag.idAuditoriaRet = idAuditoria;
            ViewBag.nombreAuditoria = nombreAuditoria;
            //List<FasesDefaultViewModel> list = new List<FasesDefaultViewModel>();
            //foreach (var a in db.FasesDefault.OrderBy(f => f.orden).ToList()){
            //    FasesDefaultViewModel fase = new FasesDefaultViewModel() {NombreFase =a.NombreFase, Porcentaje=a.Porcentaje*100,Duracion=a.Duracion,orden=a.orden};
            //    list.Add(fase);
            //}

            FasesDefaultViewModel fasesD = new FasesDefaultViewModel();
           // fasesD.list = db.FasesDefault.OrderBy(f => f.orden).ToList();
            foreach (var a in db.FasesDefault.OrderBy(f => f.orden).ToList())
            {

                FasesDefaultViewModel.FasesDefaultVMList faseList = new FasesDefaultViewModel.FasesDefaultVMList() {
                    Id_fase=a.IdFaseDefault,
                    NombreFase = a.NombreFase,
                    Porcentaje = a.Porcentaje * 100,
                    Duracion = a.Duracion, orden = a.orden };
                //fasesD.FasesD = { fasesD.FasesD.NombreFase = a.NombreFase, fasesD.FasesD.Porcentaje = a.Porcentaje * 100, fasesD.FasesD.Duracion = a.Duracion, fasesD.FasesD.orden = a.orden };
                fasesD.list.Add(faseList);
            }
            //FasesDefaultViewModel fases = new FasesDefaultViewModel() { fasesD.list= list.OrderBy(f => f.orden).ToList() };
            return View(fasesD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult indexDefault(FasesDefaultViewModel lista, Fases fases) //Aquí recibe el listado de las fases de la vista
        {
            var co = lista.list.Count;
            var aud = db.Auditorias.Where(a => a.IdAuditoria == fases.IdAuditoria).Include(a => a.Fases).SingleOrDefault();
            int duracion = 0, cont = 0;

            foreach (var item in lista.list)
            {
                if (item.Selected == true)
                {
                    fases.Fase = item.NombreFase;
                    fases.Porcentaje = item.Porcentaje;

                    if (cont == 0)
                    {
                        fases.FechaInicio = aud.FechaInicio;
                        fases.FechaFin = fases.FechaInicio.AddDays(item.Duracion);
                    }
                    else
                    {
                        fases.FechaInicio = aud.FechaInicio.AddDays(duracion + 1);
                        fases.FechaFin = fases.FechaInicio.AddDays(item.Duracion);
                    }
                                 
                    fases.FechaCrea = DateTime.Now;
                    fases.FechaModifica = fases.FechaCrea;
                    fases.UsuarioCrea = GetUserId(User);
                    fases.IdEstado = 1;
                    fases.Eliminado = false;
                    fases.Porcentaje = fases.Porcentaje / 100;
                    var porcent = 0.0;
                    foreach (var item1 in aud.Fases.Where(f => f.Eliminado != true))
                    {
                        porcent += item1.Porcentaje;
                    }
                    porcent += fases.Porcentaje;
                    duracion += item.Duracion;
                    cont += 1;
                    if (porcent <= 1.0)
                    {
                        db.Fases.Add(fases);
                        db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("", "La sumatoria de porcentajes sobrepasa el 100%, sumatoria actual: " + (porcent * 100) + " % ");
                        ViewBag.IdAuditoria = new SelectList(db.Auditorias, "IdAuditoria", "Auditoria", fases.IdAuditoria);
                        ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", fases.IdEstado);
                        ViewBag.idAuditoriaRet = aud.IdAuditoria;
                        ViewBag.nombreAuditoria = aud.Auditoria;
                        fases.Porcentaje = fases.Porcentaje * 100;
                        return View(lista);
                    }

                }       

            }
            return RedirectToAction("Index", new { idAuditoria = aud.IdAuditoria, nombreAuditoria = aud.Auditoria });

            //return View(lista);

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
            var audit = db.Auditorias.Find(idAuditoria);
            Fases fase = new Fases();
            fase.FechaInicio = audit.FechaInicio;
            fase.FechaFin = fase.FechaInicio.AddDays(8);
            fase.IdAuditoria = idAuditoria;
            fase.Porcentaje = 14.0;
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
                    ModelState.AddModelError("", "La sumatoria de porcentajes sobrepasa el 100%, sumatoria actual: "+(porcent*100)+" % ");
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
            fases.Porcentaje = fases.Porcentaje * 100;
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
                ModelState.AddModelError("", "La sumatoria de porcentajes sobrepasa el 100%, sumatoria actual: "+(porcent*100)+"%");
                fases.Porcentaje = fases.Porcentaje * 100;
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
