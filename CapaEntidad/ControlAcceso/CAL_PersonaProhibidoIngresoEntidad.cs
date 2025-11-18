using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ControlAcceso
{
    public class CAL_PersonaProhibidoIngresoEntidad
    {
        public int TimadorID { get; set; }
        public string NombreCompleto { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string DNI { get; set; }
        public string Foto { get; set; }
        public string Telefono { get; set; }
        public int Estado { get; set; }
        public string Imagen { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int TipoTimadorID { get; set; }
        public int CantidadIncidencias { get; set; }
        public int BandaID { get; set; }

        public int EmpleadoID { get; set; }
        public string EmpleadoNombres { get; set; }
        public string EmpleadoApellidoPaterno { get; set; }
        public string EmpleadoNombreCompleto { get; set; }

        public int CodSala { get; set; }
        public string SalaNombre { get; set; }
        public string SalaNombreCompuesto { get; set; }

        public string Observacion { get; set; }
        public int SustentoLegal { get; set; }
        public bool ConAtenuante { get; set; }
        public string DescripcionAtenuante { get; set; }
        public int TipoDOI { get; set; }
		public int Prohibir { get; set; }
		/*
        public int bandaUbigeo { get; set; }
        public string bandaNombre { get; set; }
        public string salaNombre { get; set; }
        public string salaNombreCorto { get; set; }
        public string salaDireccion { get; set; }
        public int salaubigeo { get; set; }
        public int empresaID { get; set; }
        public string empresaNombre { get; set; }
        public string empresaruc { get; set; }
        public string empresadireccion { get; set; }
        public string empresarepresentante { get; set; }
        public string empresatelefono { get; set; }
        public int empresaubigeo { get; set; }
        public int empresacodConsorcio { get; set; }
        public string empresanombreConsorcio { get; set; }
        */

		//Metodo
		public string Iniciales
        {
            get
            {
                var inicial = string.Empty;
                var objCadena = Nombre.Split(" ".ToArray());
                if (objCadena.Length > 0)
                {
                    inicial = objCadena.Where(objeto => !string.IsNullOrWhiteSpace(objeto)).Aggregate(inicial, (current, objeto) => current + objeto[0].ToString());
                }
                objCadena = ApellidoPaterno.Split(" ".ToArray());
                if (objCadena.Length > 0)
                {
                    inicial = objCadena.Where(objeto => !string.IsNullOrWhiteSpace(objeto)).Aggregate(inicial, (current, objeto) => current + objeto[0].ToString());
                }
                return inicial;
            }
        }

    }
}
