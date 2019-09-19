using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCCompras.Models
{
  public class ValidacionesRecuperarPassword
  {
    [Required(ErrorMessage = "Correo Requerido")]
    [EmailAddress]   
    [Display(Name = "Correo")]
    public string Correo { get; set; }
  }
}