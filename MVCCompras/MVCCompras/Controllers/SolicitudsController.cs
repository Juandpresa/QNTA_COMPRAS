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
using System.IO;
using System.Data.Entity.Validation;

namespace MVCCompras.Controllers
{
  public class SolicitudsController : Controller
  {
    private ComprasEntities db = new ComprasEntities();

    // GET: Solicituds

    public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
    {
      ViewBag.CurrentSort = sortOrder;
      ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Solicitante_desc" : "";
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
        estatus = estatus.Where(s => s.Solicitante.Contains(searchString) || s.Observacion.Contains(searchString));
      }
      switch (sortOrder)
      {
        case "Solicitante_desc":
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
      //ViewBag.PeriocidadID = new SelectList(db.Periocidad, "PeriocidadID", "Nombre");      
      //ViewBag.ReferenciaBancariaID = new SelectList(db.ReferenciaBancaria, "CuentaID", "Cuenta");
      //ViewBag.ReferenciaBancariaID = new SelectList(db.ReferenciaBancaria, "ClabeID", "CLABE");      
      //ViewBag.ConceptoID = new SelectList(db.Concepto, "ConceptoID", "Nombre");
      //ViewBag.CentroCostosID = new SelectList(db.CentroCostos, "CentroCostosID", "Nombre");

      ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre");
      ViewBag.CentroCostosID = new SelectList(db.CentroCostos, "CentroCostosID", "Nombre");


      ViewBag.PagadoraID = new SelectList(db.Pagadora, "PagadoraID", "Alias");
      ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias");
      ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre");


      ViewBag.MonedaID = new SelectList(db.Moneda, "MonedaID", "Nombre");
      ViewBag.BancoID = new SelectList(db.Bancos, "BancoId", "Alias");
      ViewBag.TipoPAgoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre");
      ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "RazonSocial");

      return View();
    }

    // POST: Solicituds/Create
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "MonedaID,PagadoraID,BancoID,CuentaID,ClabeID,TipoPagoID,ConceptoID,CentroCostosID,ClienteID,SolicitudID,ProveedorID,FormaPagoID,TipoGastoID,PeriocidadID,CantidadPagos,ImporteTotal,ImporteLetra,Observacion,FechaRegistro,FechaInicioPagos,FechaModificacion,CuentaIDModificacion,PagadoraID,ObservacionesOtroFormaP,ObsOtroTipoGasto,Solicitante")] Solicitud solicitud, ReferenciaBancaria referencia)
    {
      if (ModelState.IsValid)
      {
        ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", solicitud.ProveedorID);
        ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre", solicitud.FormaPagoID);
        ViewBag.TipoPAgoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre", solicitud.Concepto);

        ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre", solicitud.TipoGastoID);
        ViewBag.CentroCostosID = new SelectList(db.CentroCostos, "CentroCostosID", "Nombre", solicitud.TipoGasto);

        solicitud.TipoGastoID = 1;
        //ViewBag.PeriocidadID = new SelectList(db.Periocidad, "PeriocidadID", "Nombre", solicitud.PeriocidadID);
        solicitud.PeriocidadID = 1;
        solicitud.CantidadPagos = 3;
        solicitud.ImporteTotal = 2365;
        solicitud.ImporteLetra = "zzzz";
        solicitud.FechaRegistro = DateTime.Now;
        solicitud.FechaInicioPagos = DateTime.Now;
        solicitud.FechaModificacion = DateTime.Now;
        solicitud.CuentaIDModificacion = "asdf125dfg";        
        ViewBag.Pagadora = new SelectList(db.Pagadora, "PagadoraID", "Alias", solicitud.PagadoraID);
        solicitud.Solicitante = solicitud.solicitantes.GetDescripcion().ToString();


        //ViewBag.MonedaID = new SelectList(db.Moneda, "MonedaID", "Nombre", referencia.MonedaID);
        //ViewBag.BancoID = new SelectList(db.Bancos, "BancoId", "Alias", referencia.BancoID);
        //ViewBag.ReferenciaID = new SelectList(db.ReferenciaBancaria, "CuentaID", "Cuenta", referencia.ReferenciaBancariaID);
        //ViewBag.ReferenciaID = new SelectList(db.ReferenciaBancaria, "ClabeID", "CLABE", referencia.ReferenciaBancariaID);
        //ViewBag.ConceptoID = new SelectList(db.Concepto, "ConceptoID", "Nombre", solicitud.Concepto);
        //ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "RazonSocial", solicitud.TipoGasto);
        db.Solicitud.Add(solicitud);
        //db.ReferenciaBancaria.Add(referencia);
        db.SaveChanges();
        return RedirectToAction("Index");
      }


      //ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre", solicitud.FormaPagoID);
      //ViewBag.PeriocidadID = new SelectList(db.Periocidad, "PeriocidadID", "Nombre", solicitud.PeriocidadID);
      //ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", solicitud.ProveedorID);
      //ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre", solicitud.TipoGastoID);
      //ViewBag.Pagadora = new SelectList(db.Pagadora, "PagadoraID", "Alias", solicitud.PagadoraID);
      //ViewBag.MonedaID = new SelectList(db.Moneda, "MonedaID", "Nombre", referencia.MonedaID);
      //ViewBag.BancoID = new SelectList(db.Bancos, "BancoID", "Alias", referencia.BancoID);
      //ViewBag.ReferenciaID = new SelectList(db.ReferenciaBancaria, "CuentaID", "Cuenta", referencia.ReferenciaBancariaID);
      //ViewBag.ReferenciaID = new SelectList(db.ReferenciaBancaria, "ClabeID", "CLABE", referencia.ReferenciaBancariaID);
      //ViewBag.TipoPAgoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre", solicitud.Concepto);
      //ViewBag.ConceptoID = new SelectList(db.Concepto, "ConceptoID", "Nombre", solicitud.Concepto);
      //ViewBag.CentroCostosID = new SelectList(db.CentroCostos, "CentroCostosID", "Nombre", solicitud.TipoGasto);
      //ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "RazonSocial", solicitud.TipoGasto);





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
    public ActionResult Edit([Bind(Include = "SolicitudID,ProveedorID,FormaPagoID,TipoGastoID,PeriocidadID,CantidadPagos,ImporteTotal,ImporteLetra,Observacion,FechaRegistro,FechaInicioPagos,FechaModificacion,CuentaIDModificacion,PagadoraID,ObservacionesOtroFormaP,ObsOtroTipoGasto,Solicitante")] Solicitud solicitud)
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
