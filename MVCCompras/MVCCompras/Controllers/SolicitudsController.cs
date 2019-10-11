using MVCCompras.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MVCCompras.Controllers
{
  public class SolicitudsController : Controller
  {
    string[] conceptos;
    string smtpOff = "office365.com", qdomi = "qnta.com.mx";
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
      var paga = (from p in db.Pagadora
                  join s in db.Solicitud
on p.PagadoraID equals s.PagadoraID
                  select new { p.Alias });
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
      //asignar a ViewData el valor del arreglo
      ViewData["status"] = status;
      foreach (var item in estat)
      {
        string test = item.Nombre;
        ViewBag.estatus = test;
      }

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
              estatus = estatus.Where(s => s.Solicitante.Contains(searchString) || s.ImporteTotal.ToString().Contains(searchString));
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
                estatus = estatus.OrderBy(s => s.SolicitudID);
                break;
            }


            return View(estatus.ToPagedList(pageNumber, pageSize));
          }
          else
          {
            ViewBag.Message = TempData["var"].ToString();
            ViewBag.CurrentSort = sortOrder;

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
                estatus = estatus.OrderBy(s => s.SolicitudID);
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

    [HttpPost]
    public ActionResult Details(Solicitud solicitud, FormCollection collection)
    {
      ////SEGUIMIENTO
      string s = collection.Get("valida");
      string imp = collection.Get("ImporteT");
      string solicitante = collection.Get("Solicitante");
      if (s == "on")
      {
        string est = "";
        int S = int.Parse(collection.Get("NumeroSolicitud"));
        //Obtenemos la solicitus con la cual trabajaremos
        var referencia = db.Seguimiento.FirstOrDefault(e => e.SolicitudID == S);
        //obtenemos de la tabla seguimiento el Id de la solicitus que se editara
        var segui = new Seguimiento { SolicitudID = S };
        //instancimos el contexto
        using (var context = new ComprasEntities())
        {
          context.Seguimiento.Attach(segui);
          //si referencia trae datos, asignamos a la variable est el valor del estatus que trae esa solicitud
          if (referencia != null)
          {
            est = referencia.EstatusID.ToString();
          }
          //depende el estatud de la solicitus se modificara 
          if (int.Parse(est) == 2)
          {
            segui.EstatusID = 3;
          }
          if (int.Parse(est) == 3)
          {
            segui.EstatusID = 4;
          }
          if (int.Parse(est) == 4)
          {
            segui.EstatusID = 5;
          }
          segui.CuentaID = Session["idUsuario"].ToString();
          segui.FechaMovimiento = DateTime.Now;
          context.SaveChanges();
        }

        var con = (from so in db.Solicitud
                   join c in db.Concepto
                   on so.SolicitudID equals c.SolicitudId
                   where c.SolicitudId == S
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
        var emailD = from u in db.Usuarios
                     where u.idTipoUsuario == 4
                     select new { u.Correo };
        int dir = 0;
        string[] destino = new string[1];
        foreach (var item in emailD)
        {
          destino[dir] = item.Correo;
          dir++;
        }

        if (emailO != null)
        {
          string pass = emailO.Pass.ToString();
          string autoriza = emailO.Nombre.ToString();
          EnviarCorreoAU(correoOrigen, pass, S, imp, solicitante, cons, smtpOff, qdomi, destino, autoriza);
          TempData["var"] = "Solicitud Autorizada";

        }
      }

      return RedirectToAction("Index");

    }

    // GET: Solicituds/Create
    public ActionResult Create()
    {
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
            ViewBag.Solicita = new SelectList(db.Usuarios, "idUsuario", "Nombre", solicitud.Solicitante);
            //obtiene el idUsuario del solicitante
            string SOLID = CrearConcepto.Get("Solicita");
            int SOLID2 = int.Parse(SOLID);
            //Obtener el nombre del solicitante en base al idUsuario
            var ssoo = db.Usuarios.FirstOrDefault(e => e.idUsuario == SOLID2);
            //asignamos el nombre solicitante al campo solicitante
            solicitud.Solicitante = ssoo.Nombre;

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
        catch (Exception ex)
        {

          throw ex;
        }
      }
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

      //Consulta join para obtenerla factura  
      var fac = (from f in db.Factura
                 where f.SolicitudID == solicitud.SolicitudID
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
      ViewData["factura"] = fa;


      //MOSTRAR CONCEPTOS
      //Consulta join para obtener conceptos  
      var conc = (from s in db.Solicitud
                  join c in db.Concepto
                  on s.SolicitudID equals c.SolicitudId
                  where c.SolicitudId == solicitud.SolicitudID
                  //join t in db.TipoPago on c.TipoPagoID equals t.TipoPagoID
                  select new
                  {
                    c.Nombre,
                    c.ImporteParcial
                  });
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
                   where c.SolicitudId == solicitud.SolicitudID
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


      //Consulta join para obtener conceptos  
      var idconcep = (from idc in db.Concepto
                      where idc.SolicitudId == solicitud.SolicitudID
                      select new
                      {
                        idc.ConceptoID
                      });
      //ciclo que asigna a la variable "cont" el tamaño de un arreglo que sera similar al tamaño de las tuplas que trae paga
      int contidc = 0;
      foreach (var item in idconcep)
      {
        contidc++;
      }
      //foreach que asigna los valores de la consulta y los guarda en el arreglo pag
      int idcon = 0;
      string[] idcp = new string[contidc];
      foreach (var item in idconcep)
      {
        idcp[idcon] = item.ConceptoID.ToString();
        idcon = idcon + 1;
      }
      //asignar a ViewData el valor del arreglo

      ViewData["idc"] = idcp;


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
      //ViewBag.PagadoraID = new SelectList(db.Pagadora, "PagadoraID", "Alias", solicitud.PagadoraID);
      return View(solicitud);
    }

    // POST: Solicituds/Edit/5
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Exclude = "Solicitante, EstatusID, CuentaID, FechaMovimiento,ImporteTotal,ImporteLetra")] Solicitud solicitud, FormCollection collection, Seguimiento seg, FormCollection CrearConcepto)
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
        //AGREGA CONCEPTOS NUEVOS 
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

        //db.Entry(solicitud).State = EntityState.Modified;
        //db.SaveChanges();

        ////SEGUIMIENTO
        string s = collection.Get("valida");
        if (s == "on")
        {
          string est = "";
          //Obtenemos la solicitus con la cual trabajaremos
          var referencia = db.Seguimiento.FirstOrDefault(e => e.SolicitudID == solicitud.SolicitudID);
          //obtenemos de la tabla seguimiento el Id de la solicitus que se editara
          var segui = new Seguimiento { SolicitudID = solicitud.SolicitudID };
          //instancimos el contexto
          using (var context = new ComprasEntities())
          {
            context.Seguimiento.Attach(segui);
            //si referencia trae datos, asignamos a la variable est el valor del estatus que trae esa solicitud
            if (referencia != null)
            {
              est = referencia.EstatusID.ToString();
            }
            //depende el estatud de la solicitus se modificara 
            if (int.Parse(est) == 1)
            {
              segui.EstatusID = 2;
            }
            if (int.Parse(est) == 2)
            {
              segui.EstatusID = 3;
            }
            if (int.Parse(est) == 3)
            {
              segui.EstatusID = 4;
            }
            if (int.Parse(est) == 4)
            {
              segui.EstatusID = 5;
            }
            segui.CuentaID = Session["idUsuario"].ToString();
            segui.FechaMovimiento = DateTime.Now;
            context.SaveChanges();
          }

          




          var con = (from so in db.Solicitud
                     join c in db.Concepto
                     on so.SolicitudID equals c.SolicitudId
                     where c.SolicitudId == solicitud.SolicitudID
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
          var emailD = from u in db.Usuarios
                       where u.idTipoUsuario == 3
                       select new { u.Correo };
          int dir = 0;
          string[] destino = new string[2];
          foreach (var item in emailD)
          {
            destino[dir] = item.Correo;
            dir++;
          }

          if (emailO != null)
          {
            string pass = emailO.Pass.ToString();
            string aproba = emailO.Nombre.ToString();
            //EnviarCorreoA(correoOrigen, pass, solicitud.SolicitudID, solicitud.ImporteTotal, solicitud.Solicitante, cons, smtpOff, qdomi, destino, aproba);
            db.Entry(solicitud).State = EntityState.Modified;
            db.SaveChanges();
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
    public JsonResult EliminaConcepto(int idSol, string idcon, decimal impT, string impL)
    {
      int concep = int.Parse(idcon);
      var eliminaCon = from econ in db.Concepto
                       where econ.SolicitudId == idSol
                       select econ;

      db.Concepto.RemoveRange(db.Concepto.Where(x => x.ConceptoID == concep));
      db.SaveChanges();

      var modificaSoli = (from s in db.Solicitud
                          where s.SolicitudID == idSol
                          select s).FirstOrDefault();

      modificaSoli.ImporteTotal = impT;
      modificaSoli.ImporteLetra = impL;
      db.SaveChanges();


      return ViewBag.message;
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
      ViewBag.Solicita = new SelectList(db.Usuarios, "idUsuario", "Nombre");
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
      string dominio = EmailOrigen.Split('@').Last();
      string result = string.Join(",", conceptos);
      string url = urlDominio + "Home/Login/";
      MailMessage msj = new MailMessage(EmailOrigen, EmailDestino, "Nueva Solicitud de Compra",
        "<h1 align=center><b>DATOS DE LA SOLICITUD:</b></h>" +
        "<h2 align=center>No.Solicitud: " + idsol + "</h2>" +
        "<h2 align=center>Conceptos: " + result + "</h2>" +
        "<h2 align=center>Importe Total de Compra: $" + impT + "</h2>" +
        "<h2 align=center>Solicitado por: " + solicitante + "</h2><br><br><h4 align=center><a href='" + url + "'>Click para Acceder</a></h4>");

      msj.IsBodyHtml = true;

      SmtpClient cliente = new SmtpClient("mail." + dominio);
      cliente.EnableSsl = false;
      cliente.UseDefaultCredentials = false;
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

    private void EnviarCorreoA(string EmailOrigen, string pass, int idsol, decimal impT, string solicitante, string[] conceptos, string domi, string dom, string[] dest, string aprobado)
    {
      //obtener el dominio y guardarlo en varioable
      string dominio = EmailOrigen.Split('@').Last();
      string result = string.Join(",", conceptos);

      string EmailDestino = dest[0];

      string url = urlDominio + "Home/Login/";
      MailMessage msj = new MailMessage(EmailOrigen, EmailDestino, "Nueva Solicitud de Compra",
        "<h1 align=center><b>DATOS DE LA SOLICITUD APROBADA:</b></h>" +
        "<h2 align=center>No.Solicitud: " + idsol + "</h2>" +
        "<h2 align=center>Conceptos: " + result + "</h2>" +
        "<h2 align=center>Importe Total de Compra: $" + impT + "</h2>" +
        "<h2 align=center>Solicitado por: " + solicitante + "</h2>" +
        "<h2 align=center> Aprobado por: " + aprobado + "</h2>" +
        "<br><br><h4 align=center><a href='" + url + "'>Click para Acceder</a></h4>");

      msj.IsBodyHtml = true;
      MailAddress copy = new MailAddress(dest[1]);
      msj.CC.Add(copy);

      SmtpClient cliente = new SmtpClient();
      if (dominio == dom)
      {
        cliente.Host = "smtp." + domi;
        cliente.EnableSsl = true;
      }
      else
      {
        cliente.Host = "mail." + dominio;
        cliente.EnableSsl = false;
      }
      //SmtpClient cliente = new SmtpClient("mail." + dominio);


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

    private void EnviarCorreoAU(string EmailOrigen, string pass, int idsol, string impT, string solicitante, string[] conceptos, string domi, string dom, string[] dest, string autorizado)
    {
      //obtener el dominio y guardarlo en varioable
      string dominio = EmailOrigen.Split('@').Last();
      string result = string.Join(",", conceptos);

      string EmailDestino = dest[0];

      string url = urlDominio + "Home/Login/";
      MailMessage msj = new MailMessage(EmailOrigen, EmailDestino, "Nueva Solicitud de Compra",
        "<h1 align=center><b>DATOS DE LA SOLICITUD AUTORIZADA:</b></h>" +
        "<h2 align=center>No.Solicitud: " + idsol + "</h2>" +
        "<h2 align=center>Conceptos: " + result + "</h2>" +
        "<h2 align=center>Importe Total de Compra: $" + impT + "</h2>" +
        "<h2 align=center>Solicitado por: " + solicitante + "</h2>" +
        "<h2 align=center> Autorizado por: " + autorizado + "</h2>" +
        "<br><br><h4 align=center><a href='" + url + "'>Click para Acceder</a></h4>");

      msj.IsBodyHtml = true;
      //MailAddress copy = new MailAddress(dest[1]);
      //msj.CC.Add(copy);

      SmtpClient cliente = new SmtpClient();
      if (dominio == dom)
      {
        cliente.Host = "smtp." + domi;
        cliente.EnableSsl = true;
      }
      else
      {
        cliente.Host = "mail." + dominio;
        cliente.EnableSsl = false;
      }
      //SmtpClient cliente = new SmtpClient("mail." + dominio);


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
