using CapaEntidad.AsistenciaCliente;
using System;
using System.Linq;

namespace CapaEntidad.ControlAcceso {
    public class CAL_LudopataEntidad {
        public int LudopataID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public int TipoExclusion { get; set; }
        public string DNI { get; set; }
        public string Foto { get; set; }
        public int ContactoID { get; set; }
        public string Telefono { get; set; }
        public string CodRegistro { get; set; }
        public int Estado { get; set; }
        public string Imagen { get; set; }
        public int TipoDoiID { get; set; }
        public string DOINombre { get; set; }
        public int CodUbigeo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaEnvioCorreo { get; set; }
        //relaciones

        public string NombreContacto { get; set; }
        public string ApellidoPaternoContacto { get; set; }
        public string ApellidoMaternoContacto { get; set; }
        public string TelefonoContacto { get; set; }
        public string CelularContacto { get; set; }
        public int UsuarioRegistro { get; set; }
        public string SalasUsuario { get; set; }
        public string NombreUsuarioRegistro { get; set; }
        public UbigeoEntidad Ubigeo { get; set; }
        public AST_ClienteEntidad Cliente { get; set; }
        public CAL_LudopataEntidad() {
            Ubigeo = new UbigeoEntidad();
        }
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

    }
}
