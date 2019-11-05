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
    public class EvidenciasHallazgosController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Evidencias
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hall = db.Hallazgos.Where(h => h.IdHallazgo == id).SingleOrDefault();
            ViewBag.idHallazgo = id;
            ViewBag.nombreHallazgo = hall.Hallazgo;
            ViewBag.idActividad = hall.IdActividad;
            var evidencias = db.Evidencias.Where(e => e.Eliminado!=true && e.IdHallazgo==id).Include(e => e.Actividad).Include(e => e.Hallazgo).Include(e => e.PlanAccion);
            return View(evidencias.ToList());
        }

        // GET: Evidencias/Details/5
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

        // GET: Evidencias/Create
        public ActionResult Create(int? idHallazgo)
        {
            if (idHallazgo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evidencias ev = new Evidencias();
            ev.IdHallazgo = idHallazgo;
            return View(ev);
        }

        // POST: Evidencias/Create
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
                string path = Server.MapPath("~/Content/Evidencias/Hallazgos");
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

                if (System.IO.File.Exists(path + "\\" + evidencias2.IdHallazgo + "-" + fileUpload.FileName))
                {
                    System.IO.File.Move(path + "\\" + evidencias2.IdActividad + "-" + fileUpload.FileName, path + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + evidencias2.IdHallazgo + "-" + fileUpload.FileName);
                }
                fileUpload.SaveAs(path + "\\" + evidencias2.IdHallazgo + "-" + fileUpload.FileName);
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
            return Json(new { Value = exito, Message = msj, Ret = "index/" + evidencias2.IdHallazgo }, JsonRequestBehavior.AllowGet);
        }

        // GET: Evidencias/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.IdActividad = new SelectList(db.Actividades, "IdActividad", "Actividad", evidencias.IdActividad);
            ViewBag.IdHallazgo = new SelectList(db.Hallazgos, "IdHallazgo", "Hallazgo", evidencias.IdHallazgo);
            ViewBag.IdPlanAccion = new SelectList(db.PlanesDeAccions, "IdPlanAccion", "NombrePlanAccion", evidencias.IdPlanAccion);
            return View(evidencias);
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
            string path = Server.MapPath("~/Content/Evidencias/Hallazgos");
            string archivo = path + "\\" + evidencias.IdHallazgo + "-" + evidencias.NombreDoc;
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


        // POST: Evidencias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEvidencia,Evidencia,DescripcionEvidencia,NombreDoc,Documento,IdHallazgo,IdActividad,IdPlanAccion,Eliminado,UsuarioCrea,UsuarioModifica,FechaCrea,FechaModifica")] Evidencias evidencias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evidencias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdActividad = new SelectList(db.Actividades, "IdActividad", "Actividad", evidencias.IdActividad);
            ViewBag.IdHallazgo = new SelectList(db.Hallazgos, "IdHallazgo", "Hallazgo", evidencias.IdHallazgo);
            ViewBag.IdPlanAccion = new SelectList(db.PlanesDeAccions, "IdPlanAccion", "NombrePlanAccion", evidencias.IdPlanAccion);
            return View(evidencias);
        }

        // GET: Evidencias/Delete/5
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

        // POST: Evidencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evidencias evidencias = db.Evidencias.Find(id);
            evidencias.Eliminado = true;
            evidencias.FechaModifica = DateTime.Now;
            evidencias.UsuarioModifica = GetUserId(User);
            db.Entry(evidencias).State=EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index",new { id=evidencias.IdHallazgo});
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
