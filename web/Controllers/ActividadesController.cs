using FluentValidation.Results;
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
            ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase+"/"+fa.Auditoria.UsuarioRealiza.UserName;
            ViewBag.enable = fa.IdEstado == 2 ? false : true;
            var actividades = db.Actividades.Where(a=>a.Eliminado!=true && a.IdFase==idFase).Include(a => a.Encargado).Include(a => a.Estado).Include(a => a.Fase.Auditoria);
            return View(actividades.ToList());
        }

        public ActionResult Index2(int? idFase)
        {
            if (idFase == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.idFase = idFase;
            var fa = db.Fases.Where(f => f.IdFase == idFase).Include(f => f.Auditoria).SingleOrDefault();
            ViewBag.idAuditoria = fa.Auditoria.IdAuditoria;
            ViewBag.nombreAuditoria = fa.Auditoria.Auditoria;
            ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
            ViewBag.enable = fa.IdEstado == 2 ? false : true;
            var actividades = db.Actividades.Where(a => a.Eliminado != true && a.IdFase == idFase).Include(a => a.Encargado).Include(a => a.Estado).Include(a => a.Fase.Auditoria);
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
            var fa = db.Fases.Where(f => f.IdFase == actividades.IdFase).Include(f => f.Auditoria).SingleOrDefault();
            ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
            return View(actividades);
        }

        // GET: Actividades/Details/5
        public ActionResult Details2(int? id)
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
            var fa = db.Fases.Where(f => f.IdFase == actividades.IdFase).Include(f => f.Auditoria).SingleOrDefault();
            ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
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
            var fa = db.Fases.Where(f => f.IdFase == idFase).Include(f => f.Auditoria).SingleOrDefault();
            ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
            act.IdFase = (int)idFase;
            act.FechaInicio = fa.FechaInicio;
            act.FechaFin = fa.FechaFin;
            ViewBag.idFace = idFase;
            var users = db.Users.Where(u =>u.Eliminado!=true && u.Nombres != "Administrador" && u.Apellidos!= "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos);
            ViewBag.IdEncargado = new SelectList(users, "Id", "NombreCompleto",act.IdEncargado);
            return View(act);
        }


        public ActionResult Create2(int? idFase)
        {
            if (idFase == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades act = new Actividades();
            act.IdEncargado = GetUserId(User);
            var fa = db.Fases.Where(f => f.IdFase == idFase).Include(f => f.Auditoria).SingleOrDefault();
            ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
            act.IdFase = (int)idFase;
            act.FechaInicio = fa.FechaInicio;
            act.FechaFin = fa.FechaFin;
            ViewBag.idFace = idFase;
            var users = db.Users.Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos);
            ViewBag.IdEncargado = new SelectList(users, "Id", "NombreCompleto", act.IdEncargado);
            return View(act);
        }

        // POST: Actividades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Actividades actividades)
        {
            var fa = db.Fases.Where(f => f.IdFase == actividades.IdFase).Include(f => f.Auditoria).SingleOrDefault();
            actividades.Fase = fa;
            ValidadorActividades val = new ValidadorActividades();
            ValidationResult mod = val.Validate(actividades);
            if (ModelState.IsValid && mod.IsValid)
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
                    
                    ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
                    Session["MyAlert"] = "<script type='text/javascript'>alertify.error('El Porcentaje acumulado sobrepasa el 100%, sumatoria actual: " + (porcent + actividades.Porcentaje) + "%, porcentaje faltante: " + (100 - porcent) + "%.');</script>";
                    ViewBag.IdEncargado = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", actividades.IdEncargado);
                    return View(actividades);
                }
                if ((porcent + actividades.Porcentaje) < 100) {
                    Session["MyAlert"] = "<script type='text/javascript'>alertify.warning('El Porcentaje acumulado no alcanza el 100%, sumatoria actual: " + (porcent + actividades.Porcentaje) + "%.');</script>";
                }
                db.Actividades.Add(actividades);
                db.SaveChanges();
                return RedirectToAction("Index",new { IdFase =actividades.IdFase});
            }
            foreach (ValidationFailure _error in mod.Errors)
            {
                ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
            }
            ViewBag.IdEncargado = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", actividades.IdEncargado);
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", actividades.IdEstado);
            ViewBag.IdFase = new SelectList(db.Fases, "IdFase", "Fase", actividades.IdFase);
            ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
            return View(actividades);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(Actividades actividades)
        {
            var fa = db.Fases.Where(f => f.IdFase == actividades.IdFase).Include(f => f.Auditoria).SingleOrDefault();
            actividades.Fase = fa;
            ValidadorActividades val = new ValidadorActividades();
            ValidationResult mod = val.Validate(actividades);
            if (ModelState.IsValid && mod.IsValid)
            {
                actividades.UsuarioCrea = GetUserId(User);
                actividades.FechaCrea = DateTime.Now;
                actividades.IdEstado = 1;
                actividades.Eliminado = false;
                var lis = db.Actividades.Where(a => a.IdFase == actividades.IdFase && a.Eliminado != true).ToList().AsReadOnly();
                var porcent = 0.0;
                foreach (var item in lis)
                {
                    porcent += item.Porcentaje;
                }
                if ((porcent + actividades.Porcentaje) > 100)
                {

                    ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
                    Session["MyAlert"] = "<script type='text/javascript'>alertify.error('El Porcentaje acumulado sobrepasa el 100%, sumatoria actual: " + (porcent + actividades.Porcentaje) + "%, porcentaje faltante: " + (100 - porcent) + "%.');</script>";
                    ViewBag.IdEncargado = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", actividades.IdEncargado);
                    return View(actividades);
                }
                if ((porcent + actividades.Porcentaje) < 100)
                {
                    Session["MyAlert"] = "<script type='text/javascript'>alertify.warning('El Porcentaje acumulado no alcanza el 100%, sumatoria actual: " + (porcent + actividades.Porcentaje) + "%.');</script>";
                }
                db.Actividades.Add(actividades);
                db.SaveChanges();
                return RedirectToAction("Index2", new { IdFase = actividades.IdFase });
            }
            foreach (ValidationFailure _error in mod.Errors)
            {
                ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
            }
            ViewBag.IdEncargado = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", actividades.IdEncargado);
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", actividades.IdEstado);
            ViewBag.IdFase = new SelectList(db.Fases, "IdFase", "Fase", actividades.IdFase);
            ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
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
            var fa = db.Fases.Where(f => f.IdFase == actividades.IdFase).Include(f => f.Auditoria).SingleOrDefault();
            if (actividades == null)
            {
                return HttpNotFound();
            }
            ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
            ViewBag.IdEncargado = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", actividades.IdEncargado);
            return View(actividades);
        }

        // POST: Actividades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Actividades actividades)
        {
            var fa = db.Fases.Where(f => f.IdFase == actividades.IdFase).Include(f => f.Auditoria).SingleOrDefault();
            actividades.Fase = fa;
            ValidadorActividades val = new ValidadorActividades();
            ValidationResult mod = val.Validate(actividades);
            if (ModelState.IsValid && mod.IsValid)
            {
                var lis = db.Actividades.Where(a => a.IdFase == actividades.IdFase && a.Eliminado != true && a.IdActividad!=actividades.IdActividad).ToList().AsReadOnly();
                var porcent = 0.0;
                foreach (var item in lis)
                {
                    if (item.IdActividad != actividades.IdActividad)
                    {
                        porcent += item.Porcentaje;
                    }
                }
                if ((porcent + actividades.Porcentaje) > 100)
                {
                    Session["MyAlert"] = "<script type='text/javascript'>alertify.error('El Porcentaje acumulado sobrepasa el 100%, sumatoria actual: " + (porcent + actividades.Porcentaje) + "%.');</script>";
                    ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
                    ViewBag.IdEncargado = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", actividades.IdEncargado);
                    return View(actividades);
                }
                else
                {
                    actividades.UsuarioModifica = GetUserId(User);
                    actividades.FechaModifica = DateTime.Now;
                    db.Entry(actividades).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idFase = actividades.IdFase });
                }
            }
            foreach (ValidationFailure _error in mod.Errors)
            {
                ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
            }
            ViewBag.fase = fa.Auditoria.Auditoria + "/" + fa.Fase + "/" + fa.Auditoria.UsuarioRealiza.UserName;
            ViewBag.IdEncargado = new SelectList(db.Users.Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos), "Id", "NombreCompleto", actividades.IdEncargado);
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

        public ActionResult FinalizarActividad(int IdActividad)
        {
            Actividades actividad = db.Actividades.Find(IdActividad);
            if (actividad == null) {
                return HttpNotFound();
            }
            actividad.IdEstado = 2;
            actividad.UsuarioModifica = GetUserId();
            actividad.FechaCierre = DateTime.Now;
            actividad.FechaModifica = DateTime.Now;
            db.Entry(actividad).State = EntityState.Modified;
            db.SaveChanges();
            Session["MyAlert"] = "<script type='text/javascript'>alertify.success('La actividad se finalizo con éxito.');</script>";
            return RedirectToAction("Index",new { idFase =actividad.IdFase});
        }

        public ActionResult FinalizarActividad2(int IdActividad)
        {
            Actividades actividad = db.Actividades.Find(IdActividad);
            if (actividad == null)
            {
                return HttpNotFound();
            }
            actividad.IdEstado = 2;
            actividad.UsuarioModifica = GetUserId();
            actividad.FechaCierre = DateTime.Now;
            actividad.FechaModifica = DateTime.Now;
            db.Entry(actividad).State = EntityState.Modified;
            db.SaveChanges();
            Session["MyAlert"] = "<script type='text/javascript'>alertify.success('La actividad se finalizo con éxito.');</script>";
            return RedirectToAction("Index2", new { idFase = actividad.IdFase });
        }

        public ActionResult ReactivarActividad(int IdActividad)
        {
            Actividades actividad = db.Actividades.Find(IdActividad);
            if (actividad == null)
            {
                return HttpNotFound();
            }
            actividad.IdEstado = 1;
            actividad.UsuarioModifica = GetUserId();
            actividad.FechaModifica = DateTime.Now;
            db.Entry(actividad).State = EntityState.Modified;
            db.SaveChanges();
            Session["MyAlert"] = "<script type='text/javascript'>alertify.success('La actividad se reactivó con éxito.');</script>";
            return RedirectToAction("Index", new { idFase = actividad.IdFase });
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
