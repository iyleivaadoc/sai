using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web.Models;

namespace web.Controllers
{
    public class AuditoriasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Auditorias
        public async Task<ActionResult> Index(int? IdPlan,int? page)
        {
            ViewBag.page = page;
            ViewBag.plan = IdPlan;
            var auditorias = db.Auditorias.Where(a=>a.IdPlan==IdPlan).Include(a => a.Estado).Include(a => a.Plan).Include(a => a.UsuarioRealiza);
            return View(await auditorias.ToListAsync());
        }

        // GET: Auditorias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auditorias auditorias = await db.Auditorias.FindAsync(id);
            if (auditorias == null)
            {
                return HttpNotFound();
            }
            return View(auditorias);
        }

        // GET: Auditorias/Create
        public ActionResult Create()
        {
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado");
            ViewBag.IdPlan = new SelectList(db.Planes, "IdPlan", "NombrePlan");
            ViewBag.IdUsuarioRealiza = null; //new SelectList(db.ApplicationUsers, "Id", "Nombres");
            return View();
        }

        // POST: Auditorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdAuditoria,Auditoria,DescripcionAuditoria,FechaInicio,Duracion,Planificada,Elimanado,FechaCrea,FechaMod,UsuarioCrea,UsuarioMod,IdUsuarioRealiza,IdPlan,IdEstado")] Auditorias auditorias)
        {
            if (ModelState.IsValid)
            {
                db.Auditorias.Add(auditorias);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", auditorias.IdEstado);
            ViewBag.IdPlan = new SelectList(db.Planes, "IdPlan", "NombrePlan", auditorias.IdPlan);
            ViewBag.IdUsuarioRealiza = null;// new SelectList(db.ApplicationUsers, "Id", "Nombres", auditorias.IdUsuarioRealiza);
            return View(auditorias);
        }

        // GET: Auditorias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auditorias auditorias = await db.Auditorias.FindAsync(id);
            if (auditorias == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", auditorias.IdEstado);
            ViewBag.IdPlan = new SelectList(db.Planes, "IdPlan", "NombrePlan", auditorias.IdPlan);
            ViewBag.IdUsuarioRealiza = null; // new SelectList(db.ApplicationUsers, "Id", "Nombres", auditorias.IdUsuarioRealiza);
            return View(auditorias);
        }

        // POST: Auditorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdAuditoria,Auditoria,DescripcionAuditoria,FechaInicio,Duracion,Planificada,Elimanado,FechaCrea,FechaMod,UsuarioCrea,UsuarioMod,IdUsuarioRealiza,IdPlan,IdEstado")] Auditorias auditorias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auditorias).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdEstado = new SelectList(db.Estados, "IdEstado", "Estado", auditorias.IdEstado);
            ViewBag.IdPlan = new SelectList(db.Planes, "IdPlan", "NombrePlan", auditorias.IdPlan);
            ViewBag.IdUsuarioRealiza = null; // new SelectList(db.ApplicationUsers, "Id", "Nombres", auditorias.IdUsuarioRealiza);
            return View(auditorias);
        }

        // GET: Auditorias/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auditorias auditorias = await db.Auditorias.FindAsync(id);
            if (auditorias == null)
            {
                return HttpNotFound();
            }
            return View(auditorias);
        }

        // POST: Auditorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Auditorias auditorias = await db.Auditorias.FindAsync(id);
            db.Auditorias.Remove(auditorias);
            await db.SaveChangesAsync();
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
