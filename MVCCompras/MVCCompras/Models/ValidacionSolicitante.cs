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
    public int ProveedorID { get; set; }
    public int FormaPagoID { get; set; }
    public int TipoGastoID { get; set; }
    public int PeriocidadID { get; set; }
    public int CantidadPagos { get; set; }
    public decimal ImporteTotal { get; set; }
    public string ImporteLetra { get; set; }
    public string Observacion { get; set; }
    public System.DateTime FechaRegistro { get; set; }
    public System.DateTime FechaInicioPagos { get; set; }
    public System.DateTime FechaModificacion { get; set; }
    public string CuentaIDModificacion { get; set; }
    public Nullable<int> PagadoraID { get; set; }
    public string ObservacionesOtroFormaP { get; set; }
    public string ObsOtroTipoGasto { get; set; }
    public string Solicitante { get; set; }
  }
}