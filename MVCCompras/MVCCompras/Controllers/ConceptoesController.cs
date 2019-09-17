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
    public class ConceptoesController : Controller
    {
        private ComprasEntities db = new ComprasEntities();

        // GET: Conceptoes
        public ActionResult Index()
        {
            var concepto = db.Concepto.Include(c => c.TipoPago).Include(c => c.Solicitud);
            return View(concepto.ToList());
        }

        // GET: Conceptoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concepto concepto = db.Concepto.Find(id);
            if (concepto == null)
            {
                return HttpNotFound();
            }
            return View(concepto);
        }

        // GET: Conceptoes/Create
        public ActionResult Create()
        {
            ViewBag.TipoPagoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre");
            ViewBag.SolicitudId = new SelectList(db.Solicitud, "SolicitudID", "ImporteLetra");
            return View();
        }

        // POST: Conceptoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConceptoID,TipoPagoID,Nombre,ImporteParcial,SolicitudId")] Concepto concepto)
        {
            if (ModelState.IsValid)
            {
                db.Concepto.Add(concepto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoPagoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre", concepto.TipoPagoID);
            ViewBag.SolicitudId = new SelectList(db.Solicitud, "SolicitudID", "ImporteLetra", concepto.SolicitudId);
            return View(concepto);
        }

        // GET: Conceptoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concepto concepto = db.Concepto.Find(id);
            if (concepto == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoPagoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre", concepto.TipoPagoID);
            ViewBag.SolicitudId = new SelectList(db.Solicitud, "SolicitudID", "ImporteLetra", concepto.SolicitudId);
            return View(concepto);
        }

        // POST: Conceptoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConceptoID,TipoPagoID,Nombre,ImporteParcial,SolicitudId")] Concepto concepto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(concepto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoPagoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre", concepto.TipoPagoID);
            ViewBag.SolicitudId = new SelectList(db.Solicitud, "SolicitudID", "ImporteLetra", concepto.SolicitudId);
            return View(concepto);
        }

        // GET: Conceptoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concepto concepto = db.Concepto.Find(id);
            if (concepto == null)
            {
                return HttpNotFound();
            }
            return View(concepto);
        }

        // POST: Conceptoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Concepto concepto = db.Concepto.Find(id);
            db.Concepto.Remove(concepto);
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
