//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCCompras.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuarios
    {
        public int idUsuario { get; set; }
        public int idTipoUsuario { get; set; }
        public string Correo { get; set; }
        public string Pass { get; set; }
        public string Nombre { get; set; }
        public string APaterno { get; set; }
        public string AMaterno { get; set; }
        public bool UActivo { get; set; }
        public string Token_Recuperacion { get; set; }
    
        public virtual TipoUsuario TipoUsuario { get; set; }
    }
}
