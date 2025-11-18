namespace IASServiceServer.Domain.Administrativo
{
    public class Empresa : Etiqueta
    {
        public int CodEmpresa { get; set; }

        public int CodConsorcio { get; set; }

        public int CodUbigeo { get; set; }

        public string RazonSocial { get; set; }

        public string Ruc { get; set; }

        public string Direccion { get; set; }

        public string NombreRepresentanteLegal { get; set; }

        public string DniRepresentanteLegal { get; set; }

        public string Telefono { get; set; }

        public string RutaArchivoFirma { get; set; }

        public string ArchivoBase64Firma { get; set; }

        public string NombreArchivoFirma { get; set; }

        public int TipoEmpresa { get; set; }

        public int CodRD { get; set; }

        public int CodRRHH { get; set; }

        public int CodRRHHTecnicos { get; set; }

        public string RutaArchivoLogo { get; set; }

        public string RutaArchivoMembrete { get; set; }

        public int SportBar { get; set; }
    }
}