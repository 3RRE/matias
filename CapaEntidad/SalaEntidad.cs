using System;
using System.Collections.Generic;

namespace CapaEntidad {

    public class SalaEntidad {
        public Int32 CodSalaMaestra { get; set; }
        public Int32 CodSala { get; set; }
        public Int32 CodEmpresa { get; set; }
        public Int32 CodUbigeo { get; set; }
        public String Nombre { get; set; }
        public String NombreCorto { get; set; }
        public String Direccion { get; set; }
        public DateTime FechaAniversario { get; set; }
        public String UrlSistemaOnline { get; set; }
        public Int32 NroMaquinasRD { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool Activo { get; set; }
        public Int32 Estado { get; set; }
        public Int32 CodRD { get; set; }
        public String CodUsuario { get; set; }
        public Int32 CodRRHH { get; set; }
        public Int32 NroActasSorteos { get; set; }
        public Int32 CodRRHHTecnicos { get; set; }
        public String RutaArchivoLogo { get; set; }
        public String CodOB { get; set; }
        public String UrlProgresivo { get; set; }
        public String IpPublica { get; set; }
        public String UrlCuadre { get; set; }
        public String UrlPlayerTracking { get; set; }
        public String NombresAdministrador { get; set; }
        public String ApellidosAdministrador { get; set; }
        public String DniAdministrador { get; set; }
        public String FirmaAdministrador { get; set; }
        public String CodigoEstablecimiento { get; set; }
        public Int32 CodRegion { get; set; }
        public String UrlBoveda { get; set; }

        public String UrlSalaOnline { get; set; }

        public String latitud { get; set; }
        public String longitud { get; set; }
        public double distancia { get; set; }

        public String correo { get; set; }
        public int tipo { get; set; }
        public EmpresaEntidad Empresa { get; set; }
        public string RutaArchivoLogoAnt { get; set; }
        public int CantidadClientes { get; set; }
        public int PuertoSignalr { get; set; }
        public String CodEmpresaOfisis { get; set; }
        public String CodSalaOfisis { get; set; }
        public String CodOfisis { get; set; }

        public TimeSpan HoraApertura { get; set; }
        public SalaEntidad() {
            this.Empresa = new EmpresaEntidad();
        }
        public string IpPrivada { get; set; }
        public int PuertoServicioWebOnline { get; set; }
        public int PuertoWebOnline { get; set; }
        public string CarpetaOnline { get; set; }
        public bool EsPrincipal { get; set; }

        public class PingIpPublica {
            public int CodSalaMaestra { get; set; }
            public int CodSala { get; set; }
            public string Nombre { get; set; }
            public string IpPublica { get; set; }
            public bool Puerto9895 { get; set; }
            public bool Puerto8081 { get; set; }
            public bool Puerto2020 { get; set; }
        }

        public class PingIpPrivada {
            public int CodSalaMaestra { get; set; }
            public int CodSala { get; set; }
            public string Nombre { get; set; }
            public string IpPrivada { get; set; }
            public bool Puerto9895 { get; set; }
            public bool Puerto8081 { get; set; }
            public bool Puerto2020 { get; set; }
        }

        public bool Existe() {
            return CodSala > 0;
        }
    }

    public class SalaVpnEntidad {
        public int CodSalaMaestra { get; set; }
        public int CodSala { get; set; }
        public int CodEmpresa { get; set; }
        public string Nombre { get; set; }
        public string NombreEmpresa { get; set; }
        public string UrlProgresivo { get; set; }
        public string IpPrivada { get; set; }
        public int PuertoServicioWebOnline { get; set; }
        public int PuertoWebOnline { get; set; }
        public string CarpetaOnline { get; set; }
    }

    public class SalaRegasist {
        public int sal_id { get; set; }
        public Int64 empresa_id { get; set; }
        public string epr_codigo { get; set; }
        public string epr_razonsocial { get; set; }
        public string sal_codigo { get; set; }
        public string sal_nombre { get; set; }
        public string sal_direccion { get; set; }
        public string sal_correo { get; set; }
        public string sal_latitud { get; set; }
        public string sal_longitud { get; set; }
        public string sal_usuario { get; set; }
        public DateTime? sal_created { get; set; }
        public bool sal_estado { get; set; }

        public double distancia { get; set; }

    }


    public class SalaRegasistResponse {
        public bool success { get; set; }
        public string message { get; set; }
        public List<SalaRegasist> data { get; set; }

    }

    public class CorreoSala {
        public int IdCorreoSala { get; set; }
        public string Correo { get; set; }
        public int SalaId { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public string Contrasenia { get; set; }
    }

}
