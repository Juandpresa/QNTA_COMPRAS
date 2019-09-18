using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCCompras.Models;
using PagedList;

namespace MVCCompras.Controllers
{
    public class SolicitudsController : Controller
    {
        private ComprasEntities db = new ComprasEntities();

    // GET: Solicituds

    public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
    {
      ViewBag.CurrentSort = sortOrder;
      ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "usuario_desc" : "";
      ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

      if (searchString != null)
      {
        page = 1;
      }
      else
      {
        searchString = currentFilter;
      }

      ViewBag.CurrentFilter = searchString;

      var estatus = from s in db.Solicitud
                     select s;
      if (!String.IsNullOrEmpty(searchString))
      {
        estatus = estatus.Where(s => s.Observacion.Contains(searchString)|| s.Observacion.Contains(searchString));
      }
      switch (sortOrder)
      {
        case "usuario_desc":
          estatus = estatus.OrderByDescending(s => s.Concepto);
          break;
        case "Date":
          estatus = estatus.OrderBy(s => s.FechaRegistro);
          break;
        case "date_desc":
          estatus = estatus.OrderByDescending(s => s.FechaRegistro);
          break;
        default:  // Name ascending 
          estatus = estatus.OrderBy(s => s.FechaRegistro);
          break;
      }

      int pageSize = 8;
      int pageNumber = (page ?? 1);
      return View(estatus.ToPagedList(pageNumber, pageSize));
    }

    //public ActionResult Index()
    //{
    //    var solicitud = db.Solicitud.Include(s => s.FormaPago).Include(s => s.Periocidad).Include(s => s.Proveedor).Include(s => s.TipoGasto);
    //    return View(solicitud.ToList());
    //}

    // GET: Solicituds/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicitud.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            return View(solicitud);
        }

        // GET: Solicituds/Create
        public ActionResult Create()
        {
            ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre");
            ViewBag.PeriocidadID = new SelectList(db.Periocidad, "PeriocidadID", "Nombre");
            ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias");
            ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre");
            ViewBag.MonedaID = new SelectList(db.Moneda, "Moneda", "Nombre");
            return View();
        }

        // POST: Solicituds/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SolicitudID,ProveedorID,FormaPagoID,TipoGastoID,PeriocidadID,CantidadPagos,ImporteTotal,ImporteLetra,Observacion,FechaRegistro,FechaInicioPagos,FechaModificacion,CuentaIDModificacion,PagadoraID,ObservacionesOtroFormaP,ObsOtroTipoGasto")] Solicitud solicitud, Moneda moneda)
        {
            if (ModelState.IsValid)
            {
                db.Solicitud.Add(solicitud);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MonedaID = new SelectList(db.Moneda, "Moneda", "Nombre", moneda.MonedaID);
            ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre", solicitud.FormaPagoID);
            ViewBag.PeriocidadID = new SelectList(db.Periocidad, "PeriocidadID", "Nombre", solicitud.PeriocidadID);
            ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", solicitud.ProveedorID);
            ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre", solicitud.TipoGastoID);
            return View(solicitud);
        }

        // GET: Solicituds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicitud.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre", solicitud.FormaPagoID);
            ViewBag.PeriocidadID = new SelectList(db.Periocidad, "PeriocidadID", "Nombre", solicitud.PeriocidadID);
            ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", solicitud.ProveedorID);
            ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre", solicitud.TipoGastoID);
            return View(solicitud);
        }

        // POST: Solicituds/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SolicitudID,ProveedorID,FormaPagoID,TipoGastoID,PeriocidadID,CantidadPagos,ImporteTotal,ImporteLetra,Observacion,FechaRegistro,FechaInicioPagos,FechaModificacion,CuentaIDModificacion,PagadoraID,ObservacionesOtroFormaP,ObsOtroTipoGasto")] Solicitud solicitud)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solicitud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre", solicitud.FormaPagoID);
            ViewBag.PeriocidadID = new SelectList(db.Periocidad, "PeriocidadID", "Nombre", solicitud.PeriocidadID);
            ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", solicitud.ProveedorID);
            ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre", solicitud.TipoGastoID);
            return View(solicitud);
        }

        // GET: Solicituds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicitud.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            return View(solicitud);
        }

        // POST: Solicituds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Solicitud solicitud = db.Solicitud.Find(id);
            db.Solicitud.Remove(solicitud);
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
