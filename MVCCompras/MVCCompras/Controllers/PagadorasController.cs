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
    public class PagadorasController : Controller
    {
        private ComprasEntities db = new ComprasEntities();

        // GET: Pagadoras
        public ActionResult Index()
        {
            return View(db.Pagadora.ToList());
        }

        // GET: Pagadoras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagadora pagadora = db.Pagadora.Find(id);
            if (pagadora == null)
            {
                return HttpNotFound();
            }
            return View(pagadora);
        }

        // GET: Pagadoras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pagadoras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PagadoraID,Alias,RazonSocial,EstaActivo")] Pagadora pagadora)
        {
            if (ModelState.IsValid)
            {
                db.Pagadora.Add(pagadora);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pagadora);
        }

        // GET: Pagadoras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagadora pagadora = db.Pagadora.Find(id);
            if (pagadora == null)
            {
                return HttpNotFound();
            }
            return View(pagadora);
        }

        // POST: Pagadoras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PagadoraID,Alias,RazonSocial,EstaActivo")] Pagadora pagadora)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pagadora).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pagadora);
        }

        // GET: Pagadoras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagadora pagadora = db.Pagadora.Find(id);
            if (pagadora == null)
            {
                return HttpNotFound();
            }
            return View(pagadora);
        }

        // POST: Pagadoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pagadora pagadora = db.Pagadora.Find(id);
            db.Pagadora.Remove(pagadora);
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
