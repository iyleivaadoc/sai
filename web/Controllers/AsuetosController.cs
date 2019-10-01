using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web.Models;
using PagedList;

namespace web.Controllers
{
    public class AsuetosController : OwnController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Asuetos
        public ActionResult Index(string searchString, int? page)
        {
            ViewBag.page = page;
            ViewBag.CurrentFilter = searchString;
            var asuetos = db.Asuetos.Where(a => a.Eliminado != true);
            if (!String.IsNullOrEmpty(searchString))
            {
                asuetos = asuetos.Where(s => s.DescripcionAsueto.Contains(searchString));
            }
            asuetos = asuetos.OrderBy(a => a.Inicio);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(asuetos.ToPagedList(pageNumber, pageSize));
        }

        // GET: Asuetos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asuetos asuetos = db.Asuetos.Find(id);
            if (asuetos == null)
            {
                return HttpNotFound();
            }
            return View(asuetos);
        }

        // GET: Asuetos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Asuetos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Asuetos asuetos)
        {
            if (ModelState.IsValid)
            {
                asuetos.FechaCrea = DateTime.Now;
                asuetos.UsuarioCrea = GetUserId(User);
                asuetos.FechaModifica = asuetos.FechaCrea;
                db.Asuetos.Add(asuetos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(asuetos);
        }

        // GET: Asuetos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asuetos asuetos = db.Asuetos.Find(id);
            if (asuetos == null)
            {
                return HttpNotFound();
            }
            return View(asuetos);
        }

        // POST: Asuetos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Asuetos asuetos)
        {
            if (ModelState.IsValid)
            {
                asuetos.FechaModifica = DateTime.Now;
                asuetos.UsuarioModifica = GetUserId(User);
                db.Entry(asuetos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asuetos);
        }

        // GET: Asuetos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asuetos asuetos = db.Asuetos.Find(id);
            if (asuetos == null)
            {
                return HttpNotFound();
            }
            return View(asuetos);
        }

        // POST: Asuetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asuetos asuetos = db.Asuetos.Find(id);
            asuetos.Eliminado = true;
            asuetos.FechaModifica = DateTime.Now;
            asuetos.UsuarioModifica = GetUserId(User);
            db.Entry(asuetos).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public JsonResult BetweenAsueto(DateTime fecha)
        {
            bool retorno = false;
            string msjret = "";
            foreach (var item in db.Asuetos.Where(a => a.Eliminado != true && a.Inicio.Year <= fecha.Year && a.Fin.Year >= fecha.Year))
            {
                if (fecha >= item.Inicio && fecha <= item.Fin)
                {
                    retorno = true;
                    msjret = item.DescripcionAsueto;
                }
            }
            return Json(new { existe = retorno, msj = msjret }, JsonRequestBehavior.AllowGet);
        }


        public int DaysInTimeSpan(DateTime inicio, DateTime fin)
        {
            if (inicio > fin)
            {
                return 0;
            }
            int dias = 0;
            var fechas = db.Asuetos.Where(f => f.Eliminado != true && ((f.Inicio >= inicio && f.Inicio <= fin) || (f.Fin >= inicio && f.Fin <= fin) || (f.Inicio < inicio && f.Fin > fin))).OrderBy(f => f.Inicio).ToList();
            foreach (var item in fechas)
            {
                if (item.Inicio >= inicio)
                {
                    if (item.Fin <= fin)
                    {
                        dias += (item.Fin - item.Inicio).Days + 1;
                    }
                    else
                    {
                        dias += (fin - item.Inicio).Days + 1;
                    }
                }
                else if (item.Fin <= fin)
                {
                    dias += (item.Fin - inicio).Days + 1;
                }
                else
                {
                    dias += (fin - inicio).Days + 1;
                }
            }
            return dias;
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
