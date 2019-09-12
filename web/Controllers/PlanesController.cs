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

namespace web.Controllers
{
    public class PlanesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
            plan.FechaInicio = new DateTime(DateTime.Now.Year+1, 1, 2);
            plan.anio = DateTime.Now.Year+1;
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
        public string GetUserId(IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return claim.Value;
        }
    }
}
