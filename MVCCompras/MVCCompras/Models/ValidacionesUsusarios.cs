using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCCompras.Models
{
  [MetadataType(typeof(UsuariosMetaData))]
  public partial class Usuarios{

  }
  public class UsuariosMetaData
  {
    public int idUsuario { get; set; }
    [Required(ErrorMessage = "Tipo de Usuario Rquerido")]
    [Display(Name = "Tipo de Usuario")]
    public int idTipoUsuario { get; set; }

    [Required(ErrorMessage = "Correo Requerido")]
    [Display(Name = "Correo")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "Contraseña Requerida")]
    [Display(Name = "Contraseña")]
    [DataType(DataType.Password)]
    public string Pass { get; set; }

    [Required(ErrorMessage = "Nombre Requerido")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; }

    public string APaterno { get; set; }
    public string AMaterno { get; set; }
    [Display(Name = "Usuario Activo")]
    public bool UActivo { get; set; }
    public string Token_Recuperacion { get; set; }
  }
}