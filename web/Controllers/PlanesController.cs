using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using web.Models;
using PagedList;
using Quartz;

namespace web.Controllers
{
    public class PlanesController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async System.Threading.Tasks.Task<ActionResult> notificar()
        {
            JobNotificacionDirectores notificacion = new JobNotificacionDirectores();
            await notificacion.ExecuteNow();
            JobNotificacionEtapas notificacion2 = new JobNotificacionEtapas();
            await notificacion2.ExecuteNow();
            Session["MyAlert"] = "<script type='text/javascript'>alertify.success('las notificaciones se han enviado.');</script>";
            return RedirectToAction("index");
        }


        // GET: Planes
        public ActionResult Index(int? page, string searchString)
        {
            //page = !ViewBag.CurrentFilter==null && ViewBag.CurrentFilter.equals(searchString) ? 1 : page;

            var planes = db.Planes.Where(n => n.Eliminado != true);
            if (!String.IsNullOrEmpty(searchString))
            {
                planes = planes.Where(s => s.NombrePlan.Contains(searchString)
                                       || s.DescripcionPlan.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;
            var a = DateTime.Now.ToString();
            planes = planes.OrderByDescending(p => p.anio);
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return View(planes.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Activar(int id)
        {
            var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.IdPlan == id).ToList();
            if (auditorias.Count > 0)
            {
                Planes plan = db.Planes.Find(id);
                plan.IdEstado = 3;
                plan.FechaMod = DateTime.Now;
                plan.UsuarioMod = GetUserId();
                db.Entry(plan).State = EntityState.Modified;
                db.SaveChanges();
                Session["MyAlert"] = "<script type='text/javascript'>alertify.success('El plan ha sido activado con éxito.');</script>";
            }
            else
            {
                Session["MyAlert"] = "<script type='text/javascript'>alertify.error('El plan no se puede iniciar porque no hay auditorías registradas.');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Finalizar(int id)
        {
            var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.IdPlan == id && a.IdEstado != 2).ToList();
            if (auditorias.Count > 0)
            {
                Session["MyAlert"] = "<script type='text/javascript'>alertify.error('El plan no se puede finalizar porque hay auditorías activas.');</script>";
            }
            else
            {
                Planes plan = db.Planes.Find(id);
                plan.IdEstado = 2;
                plan.FechaMod = DateTime.Now;
                plan.UsuarioMod = GetUserId();

                db.Entry(plan).State = EntityState.Modified;
                db.SaveChanges();
                Session["MyAlert"] = "<script type='text/javascript'>alertify.success('El plan ha sido finalizado  con éxito.');</script>";
            }
            return RedirectToAction("Index");
        }


        public ActionResult Desactivar(int id)
        {

            Planes plan = db.Planes.Find(id);
            plan.IdEstado = 1;
            plan.FechaMod = DateTime.Now;
            plan.UsuarioMod = GetUserId();

            db.Entry(plan).State = EntityState.Modified;
            db.SaveChanges();
            Session["MyAlert"] = "<script type='text/javascript'>alertify.success('El plan ha sido desactivado  con éxito.');</script>";

            return RedirectToAction("Index");
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
            Planes plan = new Planes();
            plan.FechaInicio = new DateTime(DateTime.Now.Year + 1, 1, 2);
            plan.anio = DateTime.Now.Year + 1;
            return View(plan);
        }

        // POST: Planes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPlan,NombrePlan,DescripcionPlan,FechaInicio,anio,Eliminado,FechaCrea,FechaMod,UsuarioCrea,UsuarioMod")] Planes planes)
        {
            planes.FechaCrea = DateTime.Now;
            planes.Eliminado = false;
            planes.UsuarioCrea = this.GetUserId(User);
            planes.IdEstado = 1;
            ModelState.Clear();
            TryValidateModel(planes);
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
        public ActionResult Edit(Planes planes)
        {
            planes.UsuarioMod = this.GetUserId(User);
            planes.FechaMod = DateTime.Now;
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
            planes.Eliminado = true;
            planes.UsuarioMod = this.GetUserId(User);
            planes.FechaMod = DateTime.Now;
            db.Entry(planes).State = EntityState.Modified;
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
