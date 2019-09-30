using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCCompras.Models;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace MVCCompras.Controllers
{
  public class HomeController : Controller
  {
    //crear objeto que tendra los valores de nuestro modelo de datos (MVCCompras.Models)
    private ComprasEntities db = new ComprasEntities();
    
    public ActionResult Index()
    {
      try
      {
        if (Session["idUsuario"] != null)
        {
          //Si sesion trae datos permite el acceso a la vista
          return View();
        }
        else
        {
          //Si sesion es null redirecciona a la vista de login
          return RedirectToAction("Login");
        }
      }
      catch (Exception)
      {

        throw;
      }
      //Validar que la sesion no este vacia
      

    }

    public ActionResult Login()
    {
      try
      {
        //Si sesion trae datos permite el acceso a la vista
        if (TempData["var"] == null)
          {
            return View();
          }
          else
          {
            ViewBag.Message = TempData["var"].ToString();
            return View();
          }        
      }
      catch (Exception)
      {

        throw;
      }
    }

    //Metodo POST
    [HttpPost]
    [AllowAnonymous]
    public ActionResult Login(Login modelo)
    {
      try
      {
        //Verificar que Correo y contraseña no esten vacio
        if (modelo.Correo != null && modelo.Pass != null)
        {
          //Consulta a la base de datos para obtener los datos correspondientes y almacenarlos en el objeto user
          var user = db.Usuarios.FirstOrDefault(e => e.Correo == modelo.Correo && e.Pass == modelo.Pass);
          //Verificar que user no sea null
          if (user != null)
          {
            //Guardar los datos en variables de sesion

            //En sesion idUsuario guardar lo que tenga objeto user en el campo IdUsuario y convertirlo a string   
            Session["idUsuario"] = user.idUsuario.ToString();
            //En sesion Correo guardar lo que tenga objeto user en el campo Correo y convertirlo a string
            Session["Correo"] = user.Correo.ToString();
            //En sesion idTipoUsuario guardar lo que tenga objeto user en el campo IdTipoUsuario y convertirlo a string
            Session["idTipoUsuario"] = user.idTipoUsuario.ToString();
            //Redirecciona a la vista Index
            return RedirectToAction("../Solicituds/Index");

          }
          else
          {
            //Si user es null regresar a la vista Login
            //ViewBag.Message = "Correo o Contraseña incorrectos";
            return View("Login");
          }
        }
      }
      catch (Exception ex)
      {
        string error = ex.Message;
        ViewBag.Message = "Usuario o contraseña incorrectos";
        return View();
      }
      
      return View(modelo);
    }

    public ActionResult CerrarSesion()
    {
      try
      {
        //Eliminar la sesion actual
        Session.Contents.RemoveAll();
        //Redirecciona a la vista de Login
        return RedirectToAction("Login");
      }
      catch (Exception)
      {
        ViewBag.Message = "Se ha producido un error";
        return RedirectToAction("Login");
      }
     
    }
    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}