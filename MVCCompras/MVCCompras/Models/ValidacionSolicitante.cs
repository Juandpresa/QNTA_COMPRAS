using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCCompras.Models
{
  [MetadataType(typeof(SolicitudMetaData))]
  public partial class Solicitud
  {
    public USolicitante Solicitantes { get; set; }
  }
  public class SolicitudMetaData
  {    
    public int SolicitudID { get; set; }
    
    [Required(ErrorMessage = "Selecciona una Opcion")]
    [Display(Name = "Beneficiario")]
    public int ProveedorID { get; set; }

    
    [Required(ErrorMessage = "Selecciona una Opcion")]
    [Display(Name = "Forma Pago")]
    [MaxLength(50)]
    public int FormaPagoID { get; set; }


    [Required(ErrorMessage = "Requerido")]
    public int TipoGastoID { get; set; }

    [Required(ErrorMessage = "Requerido")]
    public int PeriocidadID { get; set; }

    [Required(ErrorMessage = "Requerido")]
    public int CantidadPagos { get; set; }

    [Required(ErrorMessage = "Requerido")]
    public decimal ImporteTotal { get; set; }

    [Required(ErrorMessage = "Requerido")]
    public string ImporteLetra { get; set; }

    [Required(ErrorMessage = "Requerido")]
    [Display(Name = "Observaciones")]
    public string Observacion { get; set; }


    public System.DateTime FechaRegistro { get; set; }
    public System.DateTime FechaInicioPagos { get; set; }
    public System.DateTime FechaModificacion { get; set; }
    public string CuentaIDModificacion { get; set; }

    [Required(ErrorMessage = "Requerido")]
    public Nullable<int> PagadoraID { get; set; }
    public string ObservacionesOtroFormaP { get; set; }
    public string ObsOtroTipoGasto { get; set; }

    [Required(ErrorMessage = "Requerido")]
    public string Solicitante { get; set; }
  }
}