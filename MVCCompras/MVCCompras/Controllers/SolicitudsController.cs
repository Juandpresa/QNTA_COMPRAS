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
using System.Net.Mail;

namespace MVCCompras.Controllers
{
  public class SolicitudsController : Controller
  {
    Conversion c = new Conversion();
    string urlDominio = "http://localhost:52772/";
    private ComprasEntities db = new ComprasEntities();

    // GET: Solicituds

    public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
    {
      ViewBag.CurrentSort = sortOrder;
      ViewBag.DateSortParm = sortOrder == "Date" ? "date_asc" : "Date";

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
        case "Solicitante_asc":
          estatus = estatus.OrderByDescending(s => s.FechaRegistro);
          break;
        case "Date":
          estatus = estatus.OrderByDescending(s => s.FechaRegistro.Day);
          break;
        case "date_asc":
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
      ViewBag.Message = "Hola";
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
      //ViewBag.BancoID = new SelectList(db.Bancos, "BancoId", "Alias");
      ViewBag.TipoPAgoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre");
      ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "RazonSocial");


      return View();
    }

    // POST: Solicituds/Create
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Exclude = "Solicitante")] Solicitud solicitud, ReferenciaBancaria referencia, Usuarios usr)
    {
      string pass = "";
      //string correo = Session["Correo"].ToString();
      if (ModelState.IsValid)
      {
        ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", solicitud.ProveedorID);
        ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre", solicitud.FormaPagoID);
        //ViewBag.TipoPAgoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre", solicitud.Concepto);

        ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre", solicitud.TipoGastoID);
        //ViewBag.CentroCostosID = new SelectList(db.CentroCostos, "CentroCostosID", "Nombre", solicitud.TipoGasto);

        //solicitud.TipoGastoID = 1;
        //ViewBag.PeriocidadID = new SelectList(db.Periocidad, "PeriocidadID", "Nombre", solicitud.PeriocidadID);
        solicitud.PeriocidadID = 1;
        solicitud.CantidadPagos = 1;
        //solicitud.ImporteTotal = 2365;
        //solicitud.ImporteLetra = "aaaa";
        solicitud.FechaRegistro = DateTime.Now;
        solicitud.FechaInicioPagos = DateTime.Now;
        solicitud.FechaModificacion = DateTime.Now;
        solicitud.CuentaIDModificacion = "asdf125dfg";
        ViewBag.Pagadora = new SelectList(db.Pagadora, "PagadoraID", "Alias", solicitud.PagadoraID);
        solicitud.Solicitante = solicitud.Solicitantes.GetDescripcion().ToString();


        //ViewBag.MonedaID = new SelectList(db.Moneda, "MonedaID", "Nombre", referencia.MonedaID);
        //ViewBag.BancoID = new SelectList(db.Bancos, "BancoId", "Alias", referencia.BancoID);
        //ViewBag.ReferenciaID = new SelectList(db.ReferenciaBancaria, "CuentaID", "Cuenta", referencia.ReferenciaBancariaID);
        //ViewBag.ReferenciaID = new SelectList(db.ReferenciaBancaria, "ClabeID", "CLABE", referencia.ReferenciaBancariaID);
        //ViewBag.ConceptoID = new SelectList(db.Concepto, "ConceptoID", "Nombre", solicitud.Concepto);
        //ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "RazonSocial", solicitud.TipoGasto);
        db.Solicitud.Add(solicitud);
        //db.ReferenciaBancaria.Add(referencia);
        db.SaveChanges();
        string correoOrigen = Session["Correo"].ToString();
        var emailO = db.Usuarios.FirstOrDefault(e => e.Correo == correoOrigen);
        if (emailO != null)
        {
          pass = emailO.Pass.ToString();
        }
        var user = db.Usuarios.FirstOrDefault(e => e.Nombre == solicitud.Solicitante);
        if (user != null)
        {
          string correoDestino = user.Correo.ToString();

          EnviarCorreo(correoOrigen, correoDestino, pass);
        }

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

    public JsonResult DatoBanco(int idProv, ReferenciaBancaria refe)
    {
      string banco = "";
      //var user = db.Usuarios.FirstOrDefault(e => e.Nombre == solicitud.Solicitante);
      var query = (from b in db.Bancos
                   join r in db.ReferenciaBancaria on b.BancoId equals r.BancoID
                   where r.ProveedorID == idProv
                   select new { b.Alias });

      foreach (var item in query)
      {
        banco = item.Alias.ToString();
      }

      return Json(banco, JsonRequestBehavior.AllowGet);
    }

    public JsonResult DatoCuenta(int idProv, ReferenciaBancaria refe)
    {
      string cuenta = "";
      //var user = db.Usuarios.FirstOrDefault(e => e.Nombre == solicitud.Solicitante);
      var referencia = db.ReferenciaBancaria.FirstOrDefault(e => e.ProveedorID == idProv);
      if (referencia != null)
      {
        cuenta = referencia.Cuenta.ToString();
        //clabe = referencia.Clabe.ToString();
      }
      return Json(cuenta, JsonRequestBehavior.AllowGet);
    }
    public JsonResult DatoClabe(int idProv, ReferenciaBancaria refe)
    {

      string clabe = "";
      //var user = db.Usuarios.FirstOrDefault(e => e.Nombre == solicitud.Solicitante);
      var referencia = db.ReferenciaBancaria.FirstOrDefault(e => e.ProveedorID == idProv);
      if (referencia != null)
      {
        //cuenta = referencia.Cuenta.ToString();
        clabe = referencia.Clabe.ToString();
      }
      return Json(clabe, JsonRequestBehavior.AllowGet);
    }

    #region HELPERS
    private void EnviarCorreo(string EmailOrigen, string EmailDestino, string pass)
    {
      //string EmailOrigen = "demesrmadrid@gmail.com";
      //string EmailDestino = "demesrmadrid@gmail.com";
      //string pass = "/04Demetr.";
      string url = urlDominio + "/Home/Login";
      MailMessage msj = new MailMessage(EmailOrigen, EmailDestino, "Nueva Solicitud de Compra",
        "<p>DATOS DE LA SOLICITUD:</p><br><a href='" + url + "'>Click para Acceder</a>");

      msj.IsBodyHtml = true;

      //SmtpClient cliente = new SmtpClient("smtp.gmail.com");
      SmtpClient cliente = new SmtpClient("mail.qnta.mx");
      cliente.EnableSsl = false;
      cliente.UseDefaultCredentials = false;
      //cliente.Host = "smtp.gmail.com";
      //cliente.Host = "mail.qnta.mx";
      cliente.Port = 587;
      cliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, pass);
      try
      {
        cliente.Send(msj);

        cliente.Dispose();
      }
      catch (Exception ex)
      {

        throw;
      }

    }
    #endregion
  }
}
