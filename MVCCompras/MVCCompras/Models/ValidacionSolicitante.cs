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
    //public int SolicitudID { get; set; }
    [Required]
    [Display(Name = "Proveedor")]
    public int ProveedorID { get; set; }

    [Required]
    [Display(Name = "Forma de Pago")]
    public int FormaPagoID { get; set; }

    [Required]
    [Display(Name = "Tipo de Gasto")]
    public int TipoGastoID { get; set; }
    //public int PeriocidadID { get; set; }
    //public int CantidadPagos { get; set; }

    [Required]
    [Display(Name = "Importe Total")]
    public decimal ImporteTotal { get; set; }

    [Required]
    [Display(Name = "Importe Letra")]
    public string ImporteLetra { get; set; }

    
    public string Observacion { get; set; }
    //public System.DateTime FechaRegistro { get; set; }
    //public System.DateTime FechaInicioPagos { get; set; }
    //public System.DateTime FechaModificacion { get; set; }
    //public string CuentaIDModificacion { get; set; }

    [Required]
    [Display(Name = "Razón Social Pagadora")]
    public Nullable<int> PagadoraID { get; set; }
    //public string ObservacionesOtroFormaP { get; set; }
    //public string ObsOtroTipoGasto { get; set; }

    //[Required]
    //[Display(Name = "Solicitante")]
    public string Solicitante { get; set; }
  }
}