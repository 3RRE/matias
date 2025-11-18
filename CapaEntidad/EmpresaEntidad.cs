using System;

namespace CapaEntidad
{
    public class EmpresaEntidad
    {
        public Int32 CodEmpresa { get; set; }
        public Int32 CodConsorcio { get; set; }
        public Int32 CodUbigeo { get; set; }
        public String RazonSocial { get; set; }
        public String Ruc { get; set; }
        public String Direccion { get; set; }
        public String Telefono { get; set; }
        public String ColorHexa { get; set; }
        public String Sigla { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool Activo { get; set; }
        public Int32 Estado { get; set; }
        public String RutaArchivoFirma { get; set; }
        public Int32 CodRD { get; set; }
        public Int32 TipoEmpresa { get; set; }
        public String CodUsuario { get; set; }
        public Int32 CodRRHH { get; set; }
        public Int32 CodRRHHTecnicos { get; set; }
        public String RutaArchivoLogo { get; set; }
        public Int32 SportBar { get; set; }
        public String RutaArchivoMembrete { get; set; } 
        public String NombreRepresentanteLegal { get; set; } 
        public String DniRepresentanteLegal { get; set; }
        public String RutaArchivoLogoAnt { get; set; }

        public bool Existe() {
            return CodEmpresa > 0;
        }
    }
}
