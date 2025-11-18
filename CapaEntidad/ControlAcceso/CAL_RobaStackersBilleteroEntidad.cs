using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ControlAcceso {
    public class CAL_RobaStackersBilleteroEntidad {

        public int RobaStackersBilleteroID { get; set; }
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
        public int CantidadIncidencias { get; set; }

        public int EmpleadoID { get; set; }
        public string EmpleadoNombres { get; set; }
        public string EmpleadoApellidoPaterno { get; set; }
        public string EmpleadoNombreCompleto { get; set; }

        public int CodSala { get; set; }
        public string SalaNombre { get; set; }

        public string Observacion { get; set; }

        //Metodo
        public string Iniciales {
            get {
                var inicial = string.Empty;
                var objCadena = Nombre.Split(" ".ToArray());
                if(objCadena.Length > 0) {
                    inicial = objCadena.Where(objeto => !string.IsNullOrWhiteSpace(objeto)).Aggregate(inicial, (current, objeto) => current + objeto[0].ToString());
                }
                objCadena = ApellidoPaterno.Split(" ".ToArray());
                if(objCadena.Length > 0) {
                    inicial = objCadena.Where(objeto => !string.IsNullOrWhiteSpace(objeto)).Aggregate(inicial, (current, objeto) => current + objeto[0].ToString());
                }
                return inicial;
            }
        }

        public int TipoDOI { get; set; }
    }
}
