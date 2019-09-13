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
    private ComprasEntities db = new ComprasEntities();
    
    public ActionResult Index()
    {
      if (Session["idUsuario"] != null)
      {
        return View();
      }
      else
      {
        return RedirectToAction("Login");
      }

    }

    public ActionResult Login()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    //Metodo POST
    [HttpPost]
    [AllowAnonymous]
    public ActionResult Login(Login modelo)
    {
      //Verificar que Correo y contraseña no esten vacio
      if (modelo.Correo != null && modelo.Pass != null)
      {
        var user = db.Usuarios.FirstOrDefault(e => e.Correo == modelo.Correo && e.Pass == modelo.Pass);
        if (user != null)
        {
          //Encontro usuario con los datos
          Session["idUsuario"] = user.idUsuario.ToString();
          Session["Correo"] = user.Correo.ToString();
          return RedirectToAction("Index");

        }
        else
        {
          return View("Login");
        }
      }
      return View(modelo);
    }
    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}