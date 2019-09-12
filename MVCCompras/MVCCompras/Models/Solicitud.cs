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
    
    public partial class Solicitud
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Solicitud()
        {
            this.Concepto = new HashSet<Concepto>();
            this.ArchivoXML = new HashSet<ArchivoXML>();
            this.Comprobante = new HashSet<Comprobante>();
            this.DetalleSolicitud = new HashSet<DetalleSolicitud>();
            this.DistribucionGasto = new HashSet<DistribucionGasto>();
            this.Factura = new HashSet<Factura>();
        }
    
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Concepto> Concepto { get; set; }
        public virtual FormaPago FormaPago { get; set; }
        public virtual Periocidad Periocidad { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public virtual TipoGasto TipoGasto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArchivoXML> ArchivoXML { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comprobante> Comprobante { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleSolicitud> DetalleSolicitud { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DistribucionGasto> DistribucionGasto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Factura> Factura { get; set; }
    }
}
