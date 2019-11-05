using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web.Models;

namespace web.Controllers
{
    public class EvidenciasPlanesController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EvidenciasPlanes
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var plan = db.PlanesDeAccions.Where(p => p.IdPlanAccion == id).SingleOrDefault();
            ViewBag.idPlanAccion = id;
            ViewBag.nombrePlanAccion = plan.NombrePlanAccion;
            ViewBag.idHallazgo = plan.IdHallazgo;
            var evidencias = db.Evidencias.Where(e => e.Eliminado != true && e.IdPlanAccion == id).Include(e => e.Actividad).Include(e => e.Hallazgo).Include(e => e.PlanAccion);
            return View(evidencias.ToList());
        }

        // GET: EvidenciasPlanes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evidencias evidencias = db.Evidencias.Find(id);
            if (evidencias == null)
            {
                return HttpNotFound();
            }
            return View(evidencias);
        }

        // GET: EvidenciasPlanes/Create
        public ActionResult Create(int? idPlanAccion)
        {
            if (idPlanAccion == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evidencias ev = new Evidencias();
            ev.IdPlanAccion = idPlanAccion;
            return View(ev);
        }

        // POST: EvidenciasPlanes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase fileUpload, string evidencias)
        {
            Evidencias evidencias2 = JsonConvert.DeserializeObject<Evidencias>(evidencias);
            string msj = "";
            bool exito = true;
            try
            {
                string path = Server.MapPath("~/Content/Evidencias/PlanesAccion");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (fileUpload == null)
                {
                    exito = false;
                    msj = "Debe seleccionar un archivo";
                    goto retorno;
                }

                if (System.IO.File.Exists(path + "\\" + evidencias2.IdPlanAccion + "-" + fileUpload.FileName))
                {
                    System.IO.File.Move(path + "\\" + evidencias2.IdActividad + "-" + fileUpload.FileName, path + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + evidencias2.IdHallazgo + "-" + fileUpload.FileName);
                }
                fileUpload.SaveAs(path + "\\" + evidencias2.IdPlanAccion + "-" + fileUpload.FileName);
                msj = "Evidencia Cargada";
                evidencias2.FechaCrea = DateTime.Now;
                evidencias2.UsuarioCrea = GetUserId(User);
                evidencias2.NombreDoc = fileUpload.FileName;
                db.Entry(evidencias2).State = EntityState.Added;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                msj = "Error: " + e.Message;
            }

            retorno:
            return Json(new { Value = exito, Message = msj, Ret = "index/" + evidencias2.IdPlanAccion }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Download(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evidencias evidencias = db.Evidencias.Find(id);
            if (evidencias == null)
            {
                return HttpNotFound(); ;
            }
            string path = Server.MapPath("~/Content/Evidencias/PlanesAccion");
            string archivo = path + "\\" + evidencias.IdPlanAccion + "-" + evidencias.NombreDoc;
            if (!System.IO.File.Exists(archivo))
            {
                return View("ArchiveNotFound");
            }
            return File(archivo, "application/octet-stream", evidencias.NombreDoc);
        }

        public ActionResult ArchiveNotFound()
        {
            return View();
        }

        

        // GET: EvidenciasPlanes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evidencias evidencias = db.Evidencias.Find(id);
            if (evidencias == null)
            {
                return HttpNotFound();
            }
            return View(evidencias);
        }

        // POST: EvidenciasPlanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evidencias evidencias = db.Evidencias.Find(id);
            evidencias.Eliminado = true;
            evidencias.FechaModifica = DateTime.Now;
            evidencias.UsuarioModifica = GetUserId(User);
            db.Entry(evidencias).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = evidencias.IdPlanAccion });
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
