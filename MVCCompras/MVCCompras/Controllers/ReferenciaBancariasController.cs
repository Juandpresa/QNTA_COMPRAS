using MVCCompras.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MVCCompras.Controllers
{
  public class ReferenciaBancariasController : Controller
  {
    private ComprasEntities db = new ComprasEntities();

    // GET: ReferenciaBancarias
    public ActionResult Index()
    {
      var referenciaBancaria = db.ReferenciaBancaria.Include(r => r.Bancos).Include(r => r.Moneda).Include(r => r.Proveedor);
      return View(referenciaBancaria.ToList());
    }

    // GET: ReferenciaBancarias/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ReferenciaBancaria referenciaBancaria = db.ReferenciaBancaria.Find(id);
      if (referenciaBancaria == null)
      {
        return HttpNotFound();
      }
      return View(referenciaBancaria);
    }

    // GET: ReferenciaBancarias/Create
    public ActionResult Create()
    {
      ViewBag.BancoID = new SelectList(db.Bancos, "BancoId", "Alias");
      ViewBag.MonedaID = new SelectList(db.Moneda, "MonedaID", "Nombre");
      ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias");
      return View();
    }

    // POST: ReferenciaBancarias/Create
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Exclude = "Nombre")] ReferenciaBancaria referenciaBancaria)
    {
      if (ModelState.IsValid)
      {
        referenciaBancaria.Nombre = "REFERENCIA BANCARIA";
        db.ReferenciaBancaria.Add(referenciaBancaria);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      ViewBag.BancoID = new SelectList(db.Bancos, "BancoId", "Alias", referenciaBancaria.BancoID);
      ViewBag.MonedaID = new SelectList(db.Moneda, "MonedaID", "Nombre", referenciaBancaria.MonedaID);
      ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", referenciaBancaria.ProveedorID);
      return View(referenciaBancaria);
    }

    // GET: ReferenciaBancarias/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ReferenciaBancaria referenciaBancaria = db.ReferenciaBancaria.Find(id);
      if (referenciaBancaria == null)
      {
        return HttpNotFound();
      }
      ViewBag.BancoID = new SelectList(db.Bancos, "BancoId", "Alias", referenciaBancaria.BancoID);
      ViewBag.MonedaID = new SelectList(db.Moneda, "MonedaID", "Nombre", referenciaBancaria.MonedaID);
      ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", referenciaBancaria.ProveedorID);
      return View(referenciaBancaria);
    }

    // POST: ReferenciaBancarias/Edit/5
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "ReferenciaBancariaID,ProveedorID,BancoID,MonedaID,Nombre,Cuenta,Clabe,EstaActivo")] ReferenciaBancaria referenciaBancaria)
    {
      if (ModelState.IsValid)
      {
        db.Entry(referenciaBancaria).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      ViewBag.BancoID = new SelectList(db.Bancos, "BancoId", "Alias", referenciaBancaria.BancoID);
      ViewBag.MonedaID = new SelectList(db.Moneda, "MonedaID", "Nombre", referenciaBancaria.MonedaID);
      ViewBag.ProveedorID = new SelectList(db.Proveedor, "ProveedorID", "Alias", referenciaBancaria.ProveedorID);
      return View(referenciaBancaria);
    }

    // GET: ReferenciaBancarias/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ReferenciaBancaria referenciaBancaria = db.ReferenciaBancaria.Find(id);
      if (referenciaBancaria == null)
      {
        return HttpNotFound();
      }
      return View(referenciaBancaria);
    }

    // POST: ReferenciaBancarias/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      ReferenciaBancaria referenciaBancaria = db.ReferenciaBancaria.Find(id);
      db.ReferenciaBancaria.Remove(referenciaBancaria);
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
