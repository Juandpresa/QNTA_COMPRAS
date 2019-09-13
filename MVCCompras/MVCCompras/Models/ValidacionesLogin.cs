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

    [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", ErrorMessage = "El formato de correo es: ejemplo@gmail.com")]
    [Required(ErrorMessage = "Correo Requerido")]
    [Display(Name = "Correo")]
    [MaxLength(50)]
    public string Correo { get; set; }

    [Required(ErrorMessage = "Contraseña Requerida")]
    [Display(Name = "Contraseña")]
    public string Pass { get; set; }
    public string Nombre { get; set; }
    public string APaterno { get; set; }
    public string AMaterno { get; set; }
    public bool UActivo { get; set; }

    public virtual TipoUsuario TipoUsuario { get; set; }
  }
}