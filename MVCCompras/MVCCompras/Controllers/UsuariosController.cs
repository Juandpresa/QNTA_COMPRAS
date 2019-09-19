using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MVCCompras.Models;

namespace MVCCompras.Controllers
{
  public class UsuariosController : Controller
  {
    string urlDominio = "http://localhost:52772/";
    private ComprasEntities db = new ComprasEntities();

    // GET: Usuarios
    public ActionResult Index()
    {
      var usuarios = db.Usuarios.Include(u => u.TipoUsuario);
      return View(usuarios.ToList());
    }

    // GET: Usuarios/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Usuarios usuarios = db.Usuarios.Find(id);
      if (usuarios == null)
      {
        return HttpNotFound();
      }
      return View(usuarios);
    }

    // GET: Usuarios/Create
    public ActionResult Create()
    {
      ViewBag.idTipoUsuario = new SelectList(db.TipoUsuario, "idTipoUsuario", "Perfil");
      return View();
    }

    // POST: Usuarios/Create
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "idUsuario,idTipoUsuario,Correo,Pass,Nombre,APaterno,AMaterno,UActivo,Token_Recuperacion")] Usuarios usuarios)
    {
      if (ModelState.IsValid)
      {
        db.Usuarios.Add(usuarios);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      ViewBag.idTipoUsuario = new SelectList(db.TipoUsuario, "idTipoUsuario", "Perfil", usuarios.idTipoUsuario);
      return View(usuarios);
    }

    // GET: Usuarios/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Usuarios usuarios = db.Usuarios.Find(id);
      if (usuarios == null)
      {
        return HttpNotFound();
      }
      ViewBag.idTipoUsuario = new SelectList(db.TipoUsuario, "idTipoUsuario", "Perfil", usuarios.idTipoUsuario);
      return View(usuarios);
    }

    // POST: Usuarios/Edit/5
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "idUsuario,idTipoUsuario,Correo,Pass,Nombre,APaterno,AMaterno,UActivo,Token_Recuperacion")] Usuarios usuarios)
    {
      if (ModelState.IsValid)
      {
        db.Entry(usuarios).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      ViewBag.idTipoUsuario = new SelectList(db.TipoUsuario, "idTipoUsuario", "Perfil", usuarios.idTipoUsuario);
      return View(usuarios);
    }

    // GET: Usuarios/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Usuarios usuarios = db.Usuarios.Find(id);
      if (usuarios == null)
      {
        return HttpNotFound();
      }
      return View(usuarios);
    }

    // POST: Usuarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      Usuarios usuarios = db.Usuarios.Find(id);
      db.Usuarios.Remove(usuarios);
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

    [HttpGet]
    public ActionResult InicioRecuperar()
    {
      ViewBag.Message = "hola";
      return View();
    }

    [HttpPost]
    public ActionResult InicioRecuperar(ValidacionesRecuperarPassword model)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(model);
        }

        string token = CifradoSHA256(Guid.NewGuid().ToString());

        var user = db.Usuarios.Where(e => e.Correo == model.Correo).FirstOrDefault();
        if (user != null)
        {
          user.Token_Recuperacion = token;
          db.Entry(user).State = System.Data.Entity.EntityState.Modified;
          db.SaveChanges();

          //enviar mensaje
          EnviarCorreo(user.Correo, token);
        }
        return View();
      }
      catch (Exception ex)
      {

        throw new Exception(ex.Message);
      }
      
    }

    [HttpGet]
    public ActionResult Recuperar(string token)
    {
      ValidacionesPassword model = new ValidacionesPassword();

      model.token = token;
      if (model.token == null || model.token.Trim().Equals(""))
      {
        ViewBag.Message = "Token ha Expirado";
        //TempData["var"] = "Token ha Expirado";
        return View("../Home/Login");
      }
      var user = db.Usuarios.Where(e => e.Token_Recuperacion == model.token).FirstOrDefault();
      if (user == null)
      {
        TempData["var"] = "Token ha Expirado";
        //ViewBag.Error = "Token ha Expirado";
        return View("../Home/Login");
      }
      
      return View(model);
    }

    [HttpPost]
    public ActionResult Recuperar(ValidacionesPassword model)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(model);
        }

        var user = db.Usuarios.Where(e => e.Token_Recuperacion == model.token).FirstOrDefault();

        if (user !=null)
        {
          user.Pass = model.Pass; 
          user.Token_Recuperacion = null;
          db.Entry(user).State = System.Data.Entity.EntityState.Modified;
          db.SaveChanges(); 
        }
      }
      catch (Exception ex)
      {

        throw new Exception(ex.Message);
      }
      ViewBag.Message = "Contraseña modificada";
      return View("../Home/Login");
    }

    #region HELPERS
    private string CifradoSHA256( string str)
    {
      SHA256 sha256 = SHA256Managed.Create();
      ASCIIEncoding encoding = new ASCIIEncoding();
      byte[] stream = null;
      StringBuilder sb = new StringBuilder();
      stream = sha256.ComputeHash(encoding.GetBytes(str));
      for (int i = 0; i < stream.Length; i++)      
        sb.AppendFormat("{0:x2}", stream[i]);
      return sb.ToString();
    }

    private void EnviarCorreo(string EmailDestino, string token)
    {
      string EmailOrigen = "demesrmadrid@gmail.com";
      //string EmailDestino = "demesrmadrid@gmail.com";
      string pass = "MISTERPOPO45";
      string url = urlDominio+"/Usuarios/Recuperar/?token="+token;
      MailMessage msj = new MailMessage(EmailOrigen, EmailDestino, "Recuperacion de Contraseña",
        "<p>Correo para recuperacion de comtraseña</p><br><a href='" + url + "'>Click para recuperar</a>");

      msj.IsBodyHtml = true;

      SmtpClient cliente = new SmtpClient("smtp.gmail.com");
      cliente.EnableSsl = true;
      cliente.UseDefaultCredentials = false;
      //cliente.Host = "smtp.gmail.com";
      cliente.Port = 587;
      cliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, pass);

      cliente.Send(msj);

      cliente.Dispose();
    }
    #endregion
  }
}
