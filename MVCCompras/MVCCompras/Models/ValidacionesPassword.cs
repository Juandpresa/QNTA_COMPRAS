using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCCompras.Models
{
  public class ValidacionesPassword
  {
    [Required(ErrorMessage = "Contraseña Requerida")]
    [Display(Name = "Contraseña")]
    public string Pass { get; set; }

    [Required(ErrorMessage = "Contraseña Requerida")]
    [Compare("Pass")]
    public string Pass2 { get; set; }

    public string token { get; set; }
  }
}