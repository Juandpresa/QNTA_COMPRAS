using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCCompras.Models;

namespace MVCCompras.Controllers
{
    public class CentroCostosController : Controller
    {
        private ComprasEntities db = new ComprasEntities();

        // GET: CentroCostos
        public ActionResult Index()
        {
            return View(db.CentroCostos.ToList());
        }

        // GET: CentroCostos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CentroCostos centroCostos = db.CentroCostos.Find(id);
            if (centroCostos == null)
            {
                return HttpNotFound();
            }
            return View(centroCostos);
        }

        // GET: CentroCostos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CentroCostos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CentroCostosID,Nombre,EstaActivo")] CentroCostos centroCostos)
        {
            if (ModelState.IsValid)
            {
                db.CentroCostos.Add(centroCostos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(centroCostos);
        }

        // GET: CentroCostos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CentroCostos centroCostos = db.CentroCostos.Find(id);
            if (centroCostos == null)
            {
                return HttpNotFound();
            }
            return View(centroCostos);
        }

        // POST: CentroCostos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CentroCostosID,Nombre,EstaActivo")] CentroCostos centroCostos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(centroCostos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(centroCostos);
        }

        // GET: CentroCostos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CentroCostos centroCostos = db.CentroCostos.Find(id);
            if (centroCostos == null)
            {
                return HttpNotFound();
            }
            return View(centroCostos);
        }

        // POST: CentroCostos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CentroCostos centroCostos = db.CentroCostos.Find(id);
            db.CentroCostos.Remove(centroCostos);
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
