﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ComprasEntities : DbContext
    {
        public ComprasEntities()
            : base("name=ComprasEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bancos> Bancos { get; set; }
        public virtual DbSet<CentroCostos> CentroCostos { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<ClienteCentroCostos> ClienteCentroCostos { get; set; }
        public virtual DbSet<Concepto> Concepto { get; set; }
        public virtual DbSet<Contacto> Contacto { get; set; }
        public virtual DbSet<Estatus> Estatus { get; set; }
        public virtual DbSet<FormaPago> FormaPago { get; set; }
        public virtual DbSet<Moneda> Moneda { get; set; }
        public virtual DbSet<Pagadora> Pagadora { get; set; }
        public virtual DbSet<Periocidad> Periocidad { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<ReferenciaBancaria> ReferenciaBancaria { get; set; }
        public virtual DbSet<TipoGasto> TipoGasto { get; set; }
        public virtual DbSet<TipoPago> TipoPago { get; set; }
        public virtual DbSet<ArchivoXML> ArchivoXML { get; set; }
        public virtual DbSet<Comprobante> Comprobante { get; set; }
        public virtual DbSet<DetalleSolicitud> DetalleSolicitud { get; set; }
        public virtual DbSet<DistribucionGasto> DistribucionGasto { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<Pago> Pago { get; set; }
        public virtual DbSet<Solicitud> Solicitud { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Autorizacion> Autorizacion { get; set; }
        public virtual DbSet<Seguimiento> Seguimiento { get; set; }
    }
}
