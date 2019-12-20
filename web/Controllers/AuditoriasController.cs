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
using PagedList;
using System.Security.Principal;
using System.Security.Claims;

namespace web.Controllers
{
    public class AuditoriasController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Auditorias
        public ActionResult Index(int? IdPlan, string searchString, int? page, string nombreplan)
        {
            ViewBag.page = page;
            ViewBag.idPlan = IdPlan;
            ViewBag.nombrePlan = nombreplan;
            ViewBag.CurrentFilter = searchString;
            var auditorias = db.Auditorias.Where(a => a.IdPlan == IdPlan && a.Elimanado != true).Include(a => a.Estado).Include(a => a.Plan).Include(a => a.UsuarioRealiza);
            if (!String.IsNullOrEmpty(searchString))
            {
                auditorias = auditorias.Where(s => s.Auditoria.Contains(searchString)
                                       || s.DescripcionAuditoria.Contains(searchString));
            }
            auditorias = auditorias.OrderBy(a => a.FechaInicio);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(auditorias.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Index2(string searchString, int? page, string nombreplan)
        {
            ViewBag.page = page;
            ViewBag.nombrePlan = nombreplan;
            ViewBag.CurrentFilter = searchString;
            var idUsuario = GetUserId(User);
            var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.IdUsuarioRealiza == idUsuario).Include(a => a.Estado).Include(a => a.Plan).Include(a => a.UsuarioRealiza);
            if (!String.IsNullOrEmpty(searchString))
            {
                auditorias = auditorias.Where(s => s.Auditoria.Contains(searchString)
                                       || s.DescripcionAuditoria.Contains(searchString));
            }
            auditorias = auditorias.OrderByDescending(a => a.FechaInicio);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(auditorias.ToPagedList(pageNumber, pageSize));
        }

        // GET: Auditorias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Auditorias auditorias = await db.Auditorias.FindAsync(id);
            Auditorias auditorias = await db.Auditorias.Where(a => a.IdAuditoria == id).Include(a => a.Fases).FirstOrDefaultAsync();
            if (auditorias == null)
            {
                return HttpNotFound();
            }
            return View(auditorias);
        }

        // GET: Auditorias/Details/5
        public async Task<ActionResult> Details2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Auditorias auditorias = await db.Auditorias.FindAsync(id);
            Auditorias auditorias = await db.Auditorias.Where(a => a.IdAuditoria == id).Include(a => a.Fases).FirstOrDefaultAsync();
            if (auditorias == null)
            {
                return HttpNotFound();
            }
            return View(auditorias);
        }

        // GET: Auditorias/Create
        public ActionResult Create(int idplan, string nombrePlan)
        {
            ViewBag.idPlan = idplan;
            ViewBag.nombrePlan = nombrePlan;
            Auditorias auditoria = new Auditorias();
            var plan = db.Planes.Find(idplan);
            auditoria.FechaInicio = plan.FechaInicio;
            auditoria.FechaFin = auditoria.FechaInicio.AddDays(38);
            auditoria.IdPlan = idplan;
            var usuarios = db.Users.Where(u => u.Eliminado != true && u.Roles.Any(r => r.RoleId == "b41a5a37-b052-4099-a63c-8107fe061b78")).Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos);
            ViewBag.IdUsuarioRealiza = new SelectList(usuarios, "Id", "NombreCompleto", auditoria.IdUsuarioRealiza);
            ViewBag.IdDepartamentoRealizar = new SelectList(db.Departamentos.Where(d => d.Eliminado != true), "IdDepartamento", "NombreDepartamento", auditoria.IdDepartamentoRealizar);
            return View(auditoria);
        }

        // POST: Auditorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Auditorias auditorias)
        {
            auditorias.FechaCrea = DateTime.Now;
            auditorias.UsuarioCrea = this.GetUserId(User);
            auditorias.IdEstado = 1;
            var plan = db.Planes.Find(auditorias.IdPlan);
            if (plan.FechaInicio < DateTime.Now)
            {
                auditorias.Planificada = false;
            }
            else
            {
                auditorias.Planificada = true;
            }

            if (ModelState.IsValid)
            {

                db.Auditorias.Add(auditorias);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { idplan = auditorias.IdPlan, nombrePlan = plan.NombrePlan });
            }

            var usuarios = db.Users.Where(u => u.Eliminado != true && u.Roles.Any(r => r.RoleId == "b41a5a37-b052-4099-a63c-8107fe061b78")).Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos);
            ViewBag.IdUsuarioRealiza = new SelectList(usuarios, "Id", "NombreCompleto", auditorias.IdUsuarioRealiza);
            ViewBag.IdDepartamentoRealizar = new SelectList(db.Departamentos.Where(d => d.Eliminado != true), "IdDepartamento", "NombreDepartamento", auditorias.IdDepartamentoRealizar);
            var planAux = db.Planes.Where(p => p.IdPlan == auditorias.IdPlan).First();
            ViewBag.idPlan = auditorias.IdPlan;
            ViewBag.nombrePlan = planAux.NombrePlan;
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
            var usuarios = db.Users.Where(u => u.Eliminado != true && u.Roles.Any(r => r.RoleId == "b41a5a37-b052-4099-a63c-8107fe061b78")).Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos);
            ViewBag.IdUsuarioRealiza = new SelectList(usuarios, "Id", "NombreCompleto", auditorias.IdUsuarioRealiza);
            ViewBag.IdDepartamentoRealizar = new SelectList(db.Departamentos.Where(d => d.Eliminado != true), "IdDepartamento", "NombreDepartamento", auditorias.IdDepartamentoRealizar);
            return View(auditorias);
        }

        // POST: Auditorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Auditorias auditorias)
        {
            auditorias.FechaMod = DateTime.Now;
            auditorias.UsuarioMod = this.GetUserId(User);
            //ModelState.Clear();
            //TryValidateModel(auditorias);
            if (ModelState.IsValid)
            {
                db.Entry(auditorias).State = EntityState.Modified;
                await db.SaveChangesAsync();
                var aux = db.Planes.Where(p => p.IdPlan == auditorias.IdPlan).First();
                return RedirectToAction("Index", new { idPlan = auditorias.IdPlan, nombrePlan = aux.NombrePlan });
            }

            var usuarios = db.Users.Where(u => u.Eliminado != true && u.Roles.Any(r => r.RoleId == "b41a5a37-b052-4099-a63c-8107fe061b78")).Where(u => u.Eliminado != true && u.Nombres != "Administrador" && u.Apellidos != "Administrador").OrderBy(u => u.Nombres).ThenBy(u => u.Apellidos);
            ViewBag.IdUsuarioRealiza = new SelectList(usuarios, "Id", "NombreCompleto", auditorias.IdUsuarioRealiza);
            ViewBag.IdDepartamentoRealizar = new SelectList(db.Departamentos.Where(d => d.Eliminado != true), "IdDepartamento", "NombreDepartamento", auditorias.IdDepartamentoRealizar);
            var planAux = db.Planes.Where(p => p.IdPlan == auditorias.IdPlan).First();
            ViewBag.idPlan = auditorias.IdPlan;
            auditorias.Plan = planAux;
            ViewBag.nombrePlan = planAux.NombrePlan;
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
            auditorias.Elimanado = true;
            db.Entry(auditorias).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { idplan = auditorias.IdPlan, nombrePlan = auditorias.Plan.NombrePlan });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult FinalizarAuditoria(int id)
        {
            Auditorias auditoria = db.Auditorias.Find(id);
            if (auditoria == null)
            {
                return HttpNotFound();
            }
            if ((int)auditoria.PorcentajeAvance == 100)
            {
                var fases = db.Fases.Where(f => f.IdAuditoria == auditoria.IdAuditoria && f.IdEstado == 1 && f.Eliminado != true).ToList();
                if (fases.Count() != 0)
                {
                    Session["MyAlert"] = "<script type='text/javascript'>alertify.error('No se puede finalizar la auditoría ya que hay fases sin cerrar.');</script>";
                }
                else
                {
                    auditoria.IdEstado = 2;
                    auditoria.UsuarioMod = GetUserId();
                    auditoria.FechaCierre = DateTime.Now;
                    auditoria.FechaMod = DateTime.Now;
                    db.Entry(auditoria).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["MyAlert"] = "<script type='text/javascript'>alertify.success('La auditoría se finalizo con éxito.');</script>";
                }
            }
            else
            {
                Session["MyAlert"] = "<script type='text/javascript'>alertify.error('No se puede finalizar la auditoría ya que no alcanza el 100% de avance.');</script>";
            }
            return RedirectToAction("Index", new { IdPlan = auditoria.IdPlan, nombrePlan = auditoria.Plan.NombrePlan });
        }

        public ActionResult ReactivarAuditoria(int id)
        {
            Auditorias auditoria = db.Auditorias.Find(id);
            auditoria.IdEstado = 1;
            auditoria.UsuarioMod = GetUserId();
            auditoria.FechaMod = DateTime.Now;
            db.Entry(auditoria).State = EntityState.Modified;
            db.SaveChanges();
            Session["MyAlert"] = "<script type='text/javascript'>alertify.success('La auditoría se reactivó con éxito.');</script>";
            return RedirectToAction("Index", new { IdPlan = auditoria.IdPlan, nombrePlan = auditoria.Plan.NombrePlan });
        }


        public int asignado(int idPlan, string idUser)
        {
            var auditorias = db.Auditorias.Where(a => a.Elimanado != true && a.IdPlan == idPlan && a.IdUsuarioRealiza == idUser).ToList();
            return auditorias.Count;
        }
    }
}
