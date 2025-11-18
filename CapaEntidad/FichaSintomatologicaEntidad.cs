using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class FichaSintomatologicaEntidad
    {
        public Int64 FichaId { get; set; }
        public string Empresa { get; set; }
        public string RUC { get; set; }
        public string DireccionEmpresa { get; set; }
        public string DOI { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public double TemperaturaIngreso { get; set; }
        public double TemperaturaSalida  { get; set; }
        public bool Signo1Ingreso { get; set; }
        public bool Signo2Ingreso { get; set; }
        public bool Signo3Ingreso { get; set; }
        public bool Signo4Ingreso { get; set; }
        public bool Signo5Ingreso { get; set; }
        public bool Signo6Ingreso { get; set; }
        public bool Signo1Salida { get; set; }
        public bool Signo2Salida { get; set; }
        public bool Signo3Salida { get; set; }
        public bool Signo4Salida { get; set; }
        public bool Signo5Salida { get; set; }
        public bool Signo6Salida { get; set; }
        public string Firma { get; set; }
        public bool Activo { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Celular { get; set; }
        public string Direccion { get; set; }
        public string Area { get; set; }
        public string Sala { get; set; }
        public int EmpleadoId { get; set; }
        public int CodSala { get; set; }
        public string Fecha { get; set; }

    }

    public class FichaSintomatologicaEntidadReporte
    {
        public Int64 FichaId { get; set; }
        public string Empresa { get; set; }
        public string RUC { get; set; }
        public string DireccionEmpresa { get; set; }
        public string empleadoEmpresa { get; set; }
        public string empleadoRuc { get; set; }
        public string RutaArchivoLogo { get; set; }
        public string DOI { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public double TemperaturaIngreso { get; set; }
        public double TemperaturaSalida { get; set; }
        public bool Signo1Ingreso { get; set; }
        public bool Signo2Ingreso { get; set; }
        public bool Signo3Ingreso { get; set; }
        public bool Signo4Ingreso { get; set; }
        public bool Signo5Ingreso { get; set; }
        public bool Signo6Ingreso { get; set; }
        public bool Signo1Salida { get; set; }
        public bool Signo2Salida { get; set; }
        public bool Signo3Salida { get; set; }
        public bool Signo4Salida { get; set; }
        public bool Signo5Salida { get; set; }
        public bool Signo6Salida { get; set; }
        public string Firma { get; set; }
        public bool Activo { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Celular { get; set; }
        public string Direccion { get; set; }
        public string Area { get; set; }
        public string Sala { get; set; }
        public int EmpleadoId { get; set; }
        public int CodSala { get; set; }
        public string salaNombre { get; set; }
        public string Fecha { get; set; }

    }

}
