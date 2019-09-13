using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCCompras.Models
{
  [MetadataType(typeof(Login))]
  public class Login
  {
    public int idUsuario { get; set; }
    public int idTipoUsuario { get; set; }


    [Required(ErrorMessage = "Correo Requerido")]
    [Display(Name = "Correo")]
    [MaxLength(50)]
    [RegularExpression(@"^[A-Za-z]{3}-[0-9]{4}$", ErrorMessage = "El formato de matricula es XXX-0000")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "Contraseña Requerida")]
    public string Pass { get; set; }
    public string Nombre { get; set; }
    public string APaterno { get; set; }
    public string AMaterno { get; set; }
    public bool UActivo { get; set; }

    public virtual TipoUsuario TipoUsuario { get; set; }
  }
}