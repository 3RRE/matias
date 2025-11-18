using System;

namespace IASServiceServer.Domain.Administrativo
{
    public class Sala : Entity
    {
        public int CodSala { get; set; }

        public int CodEmpresa { get; set; }

        public int CodUbigeo { get; set; }

        public int? CodRegion { get; set; }

        public string Nombre { get; set; }

        public string NombreCorto { get; set; }

        public string Direccion { get; set; }

        public DateTime FechaAniversario { get; set; }

        public string UrlSistemaOnline { get; set; }

        public int NroMaquinasRD { get; set; }

        public int CodRD { get; set; }

        public int CodRRHH { get; set; }

        public int CodRRHHTecnicos { get; set; }

        public int NroActasSorteos { get; set; }

        public string RutaArchivoLogo { get; set; }

        public string CodOB { get; set; }

        public string UrlCuadre { get; set; }

        public string UrlProgresivo { get; set; }

        public string UrlPlayerTracking { get; set; }

        public string IpPublica { get; set; }

        public string NombresAdministrador { get; set; }

        public string ApellidosAdministrador { get; set; }

        public string DniAdministrador { get; set; }

        public string FirmaAdministrador { get; set; }

        public string CodigoEstablecimiento { get; set; }
    }
}