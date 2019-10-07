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
    string[] conceptos;
    public ActionResult GetPagadora(string PagadoraID)
    {
      Pagadora pagadora = db.Pagadora.Find(int.Parse(PagadoraID));
      return Content(pagadora.ToString());
    }
    Conversion c = new Conversion();
    string urlDominio = "http://localhost:52772/";
    private ComprasEntities db = new ComprasEntities();

    // GET: Solicituds

    public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
    {
      //Consulta join para obtener el ALIAS  de la pagadora 
      var paga = (from p in db.Pagadora join s in db.Solicitud 
                  on p.PagadoraID equals s.PagadoraID
                  select new{ p.Alias});
      //ciclo que asigna a la variable "cont" el tamaño de un arreglo que sera similar al tamaño de las tuplas que trae paga
      int cont = 0;
      foreach (var item in paga)
      {
        cont++;
      }
      //foreach que asigna los valores de la consulta y los guarda en el arreglo pag
      int pagadora = 0;
      string[] pag = new string[cont];
      foreach (var item in paga)
      {
        pag[pagadora] = item.Alias;
        pagadora = pagadora + 1;
      }
      //asignar a ViewData el valor del arreglo
      ViewData["pagadora"] = pag;

      //Consulta join para obtener el Status de solicitud
      var estat = (from s in db.Solicitud
                  join e in db.Seguimiento
                  on s.SolicitudID equals e.SolicitudID
                  join d in db.Estatus
                  on e.EstatusID equals d.EstatusId
                  select new { d.Nombre });

      //ciclo que asigna a la variable "cont" el tamaño de un arreglo que sera similar al tamaño de las tuplas que trae estat
      int conta = 0;
      foreach (var item in estat)
      {
        conta++;
      }
      //foreach que asigna los valores de la consulta y los guarda en el arreglo pag
      int statu = 0;
      string[] status = new string[conta];
      foreach (var item in estat)
      {
        status[statu] = item.Nombre;
        statu = statu + 1;
      }
      //asignar a ViewData el cvalor del arreglo
      ViewData["status"] = status;





      foreach (var item in estat)
      {
        string test = item.Nombre;
        ViewBag.estatus = test;
      }

      var con = (from s in db.Solicitud
                   join c in db.Concepto
                   on s.SolicitudID equals c.SolicitudId
                   select new { c.Nombre });
      int contador = 0;
      foreach (var item in con)
      {
        contador++;
      }
      int cs = 0;
      string[] cons = new string[contador];
      foreach (var item in con)
      {
        cons[cs] = item.Nombre;
        cs=cs+1;
      }
      string result = string.Join(",", cons);
      ViewBag.concepto = result;




      //TempData["var"] = "Hola";
      var estatus = from s in db.Solicitud
                    select s;
      int pageSize = 8;
      int pageNumber = (page ?? 1);
      try
      {
        //Si sesion trae datos permite el acceso a la vista
        if (Session["idUsuario"] != null)
        {
          if (TempData["var"] == null)
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


            return View(estatus.ToPagedList(pageNumber, pageSize));
          }
          else
          {
            ViewBag.Message = TempData["var"].ToString();
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
            return View(estatus.ToPagedList(pageNumber, pageSize));
          }


        }
        else
        {
          //return RedirectToActionPermanent("../Home/Login");
          return View(estatus.ToPagedList(pageNumber, pageSize));
        }
      }
      catch (Exception)
      {

        throw;
      }



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

      viewbags();


      return View();
    }

    // POST: Solicituds/Create
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Exclude = "Solicitante, Factura")] Solicitud solicitud, ReferenciaBancaria referencia, Usuarios usr, FormCollection CrearConcepto, Factura fac, IEnumerable<HttpPostedFileBase> Factura)
    {

      string pass = "";
      int idSol = 0;
      //string correo = Session["Correo"].ToString();
      if (ModelState.IsValid)
      {
        try
        {
          //Validamos que el usuario selecciono un archivo
          if (Factura != null)
          {
            foreach (var file in Factura)
            {
              if (file.ContentLength > 0)
              {
                var fileName = Path.GetFileName(file.FileName);
                //validamos que sea un archivo pdf o xml
                if (Path.GetExtension(fileName).ToLower() == ".pdf" || Path.GetExtension(fileName).ToLower() == ".xml")
                {

                }
                else
                {
                  ViewBag.Message = "Alguno de los archivos no tiene extension (.pdf) o (.xml)";
                  viewbags();
                  return View();
                }
              }
            }

            ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", solicitud.ProveedorID);
            ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre", solicitud.FormaPagoID);
            //ViewBag.TipoPAgoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre", solicitud.Concepto);

            ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre", solicitud.TipoGastoID);

            //VERIFICAR DATOS !!//RECIEN AGREGADO
            //ViewBag.CentroCostosID = new SelectList(db.CentroCostos, "CentroCostosID", "Nombre", solicitud.TipoGasto);
            //ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "RazonSocial",solicitud.TipoGasto);

            //solicitud.TipoGastoID = 1;
            //ViewBag.PeriocidadID = new SelectList(db.Periocidad, "PeriocidadID", "Nombre", solicitud.PeriocidadID);
            solicitud.PeriocidadID = 1;
            solicitud.CantidadPagos = 1;
            //solicitud.ImporteTotal = 2365;
            //solicitud.ImporteLetra = "aaaa";
            solicitud.FechaRegistro = DateTime.Now;
            solicitud.FechaInicioPagos = DateTime.Now;
            solicitud.FechaModificacion = DateTime.Now;
            solicitud.CuentaIDModificacion = Session["idUsuario"].ToString();
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
            idSol = db.Solicitud.Max(item => item.SolicitudID);
            //guardar los conceptos
            int NumConcepto = int.Parse(CrearConcepto["NumConcepto"].ToString());
            conceptos = new string[NumConcepto];

            for (int item = 1; item <= NumConcepto; item++)
            {
              //Crear un objeto que permita guardar el cargamento 
              Concepto NewConcepto = new Concepto();
              //Agregamos registro x registro al la bd
              NewConcepto.SolicitudId = solicitud.SolicitudID;
              NewConcepto.TipoPagoID = int.Parse(CrearConcepto["idTipoPago" + item]);
              NewConcepto.Nombre = CrearConcepto["descid" + item].ToString();
              NewConcepto.ImporteParcial = decimal.Parse(CrearConcepto["importeid" + item].ToString());

              db.Concepto.Add(NewConcepto);
              conceptos[item - 1] = NewConcepto.Nombre;
            }
            db.SaveChanges();
            //guarda el archivo
            foreach (var file2 in Factura)
            {
              if (file2.ContentLength > 0)
              {
                var fileName2 = Path.GetFileName(file2.FileName);
                var path = Path.Combine(Server.MapPath("~/Archivos/Facturas"), fileName2);
                string ruta = "/Archivos/Facturas/" + fileName2;

                fac.Archivo = ruta;
                fac.Nombre = fileName2;
                fac.FechaAlmacenamiento = DateTime.Now;
                fac.SeCargoFactura = true;
                fac.SolicitudID = idSol;
                file2.SaveAs(path);
                db.Factura.Add(fac);
                db.SaveChanges();
              }
            }

            Seguimiento Nseguimiento = new Seguimiento();
            Nseguimiento.CuentaID = Session["idUsuario"].ToString();
            Nseguimiento.SolicitudID = idSol;
            Nseguimiento.EstatusID = 1;
            Nseguimiento.FechaMovimiento = DateTime.Now;
            db.Seguimiento.Add(Nseguimiento);
            db.SaveChanges();

          }

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

            EnviarCorreo(correoOrigen, correoDestino, pass, idSol, solicitud.ImporteTotal, solicitud.Solicitante, conceptos);
          }
          TempData["var"] = "Solicitud Creada";
          return RedirectToAction("Index");
        }
        catch (Exception)
        {

          throw;
        }
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




      viewbags();
      return View();
    }   



    //EDITAR//
    // GET: Solicituds/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Solicitud solicitud = db.Solicitud.Find(id);


      //CHECAR PARA EDIT
      var esta = (from s in db.Seguimiento
                  where id == solicitud.SolicitudID
                  select new { s.EstatusID });
      foreach (var item in esta)
      {
        int st = item.EstatusID;
        ViewBag.estatus = st;
      }


      //Consulta join para obtenerla factura  
      var fac = (from f in db.Factura
                 
                  select new { f.Nombre });
      //ciclo que asigna a la variable "cont" el tamaño de un arreglo que sera similar al tamaño de las tuplas que trae paga
      int cont = 0;
      foreach (var item in fac)
      {
        cont++;
      }
      //foreach que asigna los valores de la consulta y los guarda en el arreglo pag
      int factura = 0;
      string[] fa = new string[cont];
      foreach (var item in fac)
      {
        fa[factura] = item.Nombre;
        factura = factura + 1;
      }
      //asignar a ViewData el valor del arreglo
      ViewBag.conf = factura;
      ViewData["factura"] =fa;


      //MOSTRAR CONCEPTOS
      //Consulta join para obtener conceptos  
      var conc = (from s in db.Solicitud
                   join c in db.Concepto
                   on s.SolicitudID equals c.SolicitudId
                   //join t in db.TipoPago on c.TipoPagoID equals t.TipoPagoID
                   select new {
                                c.Nombre,
                                c.ImporteParcial});
      //ciclo que asigna a la variable "cont" el tamaño de un arreglo que sera similar al tamaño de las tuplas que trae paga
      int con = 0;
      foreach (var item in conc)
      {
        con++;
      }
      //foreach que asigna los valores de la consulta y los guarda en el arreglo pag
      int concepto = 0;
      string[] co = new string[con];
      string[] ip = new string[con];
      foreach (var item in conc)
      {
        co[concepto] = item.Nombre;
        ip[concepto] = item.ImporteParcial.ToString();
        concepto = concepto + 1;
      }
      //asignar a ViewData el valor del arreglo
      ViewBag.concepto = con;
      ViewData["concepto"] = co;
      ViewData["importe"] = ip;

      //MOSTRAR TIPO DE PAGO DE CONCEPTO
      //Consulta join para obtener conceptos  
      var tpago = (from t in db.TipoPago
                  join c in db.Concepto
                  on t.TipoPagoID equals c.TipoPagoID
                  join s in db.Solicitud on c.SolicitudId equals s.SolicitudID
                  select new
                  {
                    t.Nombre
                  });
      //ciclo que asigna a la variable "cont" el tamaño de un arreglo que sera similar al tamaño de las tuplas que trae paga
      int contp = 0;
      foreach (var item in tpago)
      {
        contp++;
      }
      //foreach que asigna los valores de la consulta y los guarda en el arreglo pag
      int tipopago = 0;
      string[] tp = new string[contp];
      foreach (var item in tpago)
      {
        tp[tipopago] = item.Nombre;
        tipopago = tipopago + 1;
      }
      //asignar a ViewData el valor del arreglo
      ViewBag.pago = contp;
      ViewData["tpago"] = tp;


      //VIEWBAGS PARA SOLICITAR DDL
      ViewBag.SeguimientoID = new SelectList(db.Seguimiento, "SolicitudID", "EstatusID");
      ViewBag.CentroCostosID = new SelectList(db.CentroCostos, "CentroCostosID", "Nombre");
      ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre");
      ViewBag.CentroCostosID = new SelectList(db.CentroCostos, "CentroCostosID", "Nombre");
      ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "RazonSocial");
      ViewBag.PagadoraID = new SelectList(db.Pagadora, "PagadoraID", "Alias");
      ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias");
      ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre");
      ViewBag.MonedaID = new SelectList(db.Moneda, "MonedaID", "Nombre");
      ViewBag.TipoPAgoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre");
      ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "RazonSocial");

      if (solicitud == null)
      {
        return HttpNotFound();
      }
      ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre", solicitud.FormaPagoID);
      ViewBag.PeriocidadID = new SelectList(db.Periocidad, "PeriocidadID", "Nombre", solicitud.PeriocidadID);
      ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", solicitud.ProveedorID);
      ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre", solicitud.TipoGastoID);
      ViewBag.PagadoraID = new SelectList(db.Pagadora, "PagadoraID", "Alias", solicitud.PagadoraID);
      return View(solicitud);
    }

    // POST: Solicituds/Edit/5
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Exclude = "Solicitante")] Solicitud solicitud, FormCollection collection)
    {
     
      if (ModelState.IsValid)
      {
        ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", solicitud.ProveedorID);
        ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre", solicitud.FormaPagoID);
        ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre", solicitud.TipoGastoID);
        solicitud.PeriocidadID = 1;
        solicitud.CantidadPagos = 1;
        solicitud.FechaRegistro = DateTime.Now;
        solicitud.FechaInicioPagos = DateTime.Now;
        solicitud.FechaModificacion = DateTime.Now;
        solicitud.CuentaIDModificacion = Session["idUsuario"].ToString();
        ViewBag.Pagadora = new SelectList(db.Pagadora, "PagadoraID", "Alias", solicitud.PagadoraID);
        solicitud.Solicitante = collection.Get("Solicitante");


        db.Entry(solicitud).State = EntityState.Modified;
        db.SaveChanges();

        ////SEGUIMIENTO
        string cuenta = "";
        var referencia = db.Seguimiento.FirstOrDefault(e => e.SolicitudID == solicitud.SolicitudID);
        var query = (from q in db.Seguimiento
                     where q.SolicitudID == solicitud.SolicitudID
                     select q).FirstOrDefault();

        if (referencia != null)
        {
          cuenta = query.EstatusID.ToString();
        }
        if (int.Parse(cuenta) == 1)
        {
          query.EstatusID = int.Parse(cuenta)+ 1;
        }
        if (int.Parse(cuenta) == 2)
        {
          query.EstatusID = 3;
        }
        if (int.Parse(cuenta) == 3)
        {
          query.EstatusID = 4;
        }
        if (int.Parse(cuenta) == 4)
        {
          query.EstatusID = 5;
        }

        

        query.CuentaID = Session["idUsuario"].ToString();
        query.FechaMovimiento = DateTime.Now;

        //db.SubmitChanges();

        //Nseguimiento.CuentaID = Session["idUsuario"].ToString();
        ////Nseguimiento.SolicitudID = solicitud.SolicitudID;
        //Nseguimiento.FechaMovimiento = DateTime.Now;
        db.Entry(query).State = EntityState.Modified;
        db.SaveChanges();

        string s = collection.Get("valida");
        if (s == "on")
        {
          var con = (from so in db.Solicitud
                     join c in db.Concepto
                     on so.SolicitudID equals c.SolicitudId
                     select new { c.Nombre });
          int cont = 0;
          foreach (var item in con)
          {
            cont++;
          }
          int cs = 0;
          string[] cons = new string[cont];
          foreach (var item in con)
          {
            cons[cs] = item.Nombre;
            cs = cs + 1;
          }
          string correoOrigen = Session["Correo"].ToString();
          var emailO = db.Usuarios.FirstOrDefault(e => e.Correo == correoOrigen);
          if (emailO != null)
          {
            string pass = emailO.Pass.ToString();
            EnviarCorreoA(correoOrigen, pass, solicitud.SolicitudID, solicitud.ImporteTotal, solicitud.Solicitante, cons);
            TempData["var"] = "Solicitud Aprobada";
          }
        }
        return RedirectToAction("Index");
      }
     // }

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
    public JsonResult DatoSol(int idProv, ReferenciaBancaria refe)
    {

      string solis = "";
      //var user = db.Usuarios.FirstOrDefault(e => e.Nombre == solicitud.Solicitante);
      var solicitan = db.Solicitud.FirstOrDefault(e => e.SolicitudID == idProv);
      if (solicitan != null)
      {
        //cuenta = referencia.Cuenta.ToString();
        solis = solicitan.Solicitante.ToString();
      }
      return Json(solis, JsonRequestBehavior.AllowGet);
    }

    public void viewbags()
    {
      ViewBag.CentroCostosID = new SelectList(db.CentroCostos, "CentroCostosID", "Nombre");
      ViewBag.TipoGastoID = new SelectList(db.TipoGasto, "TipoGastoID", "Nombre");
      ViewBag.CentroCostosID = new SelectList(db.CentroCostos, "CentroCostosID", "Nombre");
      ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "RazonSocial");
      ViewBag.PagadoraID = new SelectList(db.Pagadora, "PagadoraID", "Alias");
      ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias");
      ViewBag.FormaPagoID = new SelectList(db.FormaPago, "FormaPagoID", "Nombre");
      ViewBag.MonedaID = new SelectList(db.Moneda, "MonedaID", "Nombre");
      ViewBag.TipoPAgoID = new SelectList(db.TipoPago, "TipoPagoID", "Nombre");
      ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "RazonSocial");
    }

    #region HELPERS
    private void EnviarCorreo(string EmailOrigen, string EmailDestino, string pass, int idsol, decimal impT, string solicitante, string[] conceptos)
    {
      string result = string.Join(",", conceptos);
      //string result = String.Concat(""+conceptos);
      //string EmailOrigen = "demesrmadrid@gmail.com";
      //string EmailDestino = "demesrmadrid@gmail.com";
      //string pass = "/04Demetr.";
      string url = urlDominio + "Home/Login/";
      MailMessage msj = new MailMessage(EmailOrigen, EmailDestino, "Nueva Solicitud de Compra",
        "<h1 align=center><b>DATOS DE LA SOLICITUD:</b></h>" +
        "<h2 align=center>No.Solicitud: " + idsol + "</h2>" +
        "<h2 align=center>Conceptos: " + result + "</h2>" +
        "<h2 align=center>Importe Total de Compra: $" + impT + "</h2>" +
        "<h2 align=center>Solicitado por: " + solicitante + "</h2><br><br><h4 align=center><a href='" + url + "'>Click para Acceder</a></h4>");

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

    private void EnviarCorreoA(string EmailOrigen, string pass, int idsol, decimal impT, string solicitante, string[] conceptos)
    {
      string result = string.Join(",", conceptos);
      //string result = String.Concat(""+conceptos);
      //string EmailOrigen = "demesrmadrid@gmail.com";
      string EmailDestino = "demesrmadrid@gmail.com";
      //string pass = "/04Demetr.";
      string url = urlDominio + "Home/Login/";
      MailMessage msj = new MailMessage(EmailOrigen, EmailDestino, "Nueva Solicitud de Compra",
        "<h1 align=center><b>DATOS DE LA SOLICITUD APROBADA:</b></h>" +
        "<h2 align=center>No.Solicitud: " + idsol + "</h2>" +
        "<h2 align=center>Conceptos: " + result + "</h2>" +
        "<h2 align=center>Importe Total de Compra: $" + impT + "</h2>" +
        "<h2 align=center>Solicitado por: " + solicitante + "</h2>" +
        "< h2 align = center > Aprobado por: " + solicitante + " </ h2 >"+
        "<br><br><h4 align=center><a href='" + url + "'>Click para Acceder</a></h4>");

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
