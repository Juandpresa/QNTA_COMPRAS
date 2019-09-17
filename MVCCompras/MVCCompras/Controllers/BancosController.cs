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
  public class BancosController : Controller
  {
    private ComprasEntities db = new ComprasEntities();

    // GET: Bancos
    public ActionResult Index()
    {
      try
      {
        if (Session["idUsuario"] != null)
        {
          //Si sesion trae datos permite el acceso a la vista
          return View(db.Bancos.ToList());
        }
        else
        {
          //Si sesion es null redirecciona a la vista de login
          return RedirectToAction("../Home/Login");
        }
      }
      catch (Exception)
      {

        throw;
      }

    }

    // GET: Bancos/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Bancos bancos = db.Bancos.Find(id);
      if (bancos == null)
      {
        return HttpNotFound();
      }
      return View(bancos);
    }

    // GET: Bancos/Create
    public ActionResult Create()
    {
      if (Session["idUsuario"] != null)
      {
        return View();
      }
      else
      {
        //Si sesion es null redirecciona a la vista de login
        return RedirectToAction("../Home/Login");
      }

    }

    // POST: Bancos/Create
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "BancoId,Alias,Nombre,EstaActivo")] Bancos bancos)
    {
      try
      {
        if (Session["idUsuario"] != null)
        {
          if (ModelState.IsValid)
          {
            db.Bancos.Add(bancos);
            db.SaveChanges();
            return RedirectToAction("Index");
          }
        }
        else
        {
          //Si sesion es null redirecciona a la vista de login
          return RedirectToAction("../Home/Login");
        }
      }
      catch (Exception)
      {

        throw;
      }

      return View(bancos);
    }

    // GET: Bancos/Edit/5
    public ActionResult Edit(int? id)
    {
      try
      {
        if (Session["idUsuario"] != null)
        {
          return View();
        }
        else
        {
          //Si sesion es null redirecciona a la vista de login
          return RedirectToAction("../Home/Login");
        }
      }
      catch (Exception)
      {

        throw;
      }
      
     
    }

    // POST: Bancos/Edit/5
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "BancoId,Alias,Nombre,EstaActivo")] Bancos bancos)
    {
      if (ModelState.IsValid)
      {
        db.Entry(bancos).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(bancos);
    }

    // GET: Bancos/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Bancos bancos = db.Bancos.Find(id);
      if (bancos == null)
      {
        return HttpNotFound();
      }
      return View(bancos);
    }

    // POST: Bancos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      Bancos bancos = db.Bancos.Find(id);
      db.Bancos.Remove(bancos);
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
