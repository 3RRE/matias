using CapaDatos;
using CapaDatos.WhatsApp;
using CapaEntidad;
using CapaEntidad.WhatsApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.WhatsApp {
    public class WSP_ContactoBL {
        private WSP_ContactoDAL _contactoDAL = new WSP_ContactoDAL();
        
        public List<WSP_ContactoEntidad> ObtenerTodosLosContactos() {
            return _contactoDAL.ObtenerTodosLosContactos();
        }

        public List<WSP_ContactoEntidad> ObtenerContactosActivos() {
            return _contactoDAL.ObtenerContactosActivos();
        }

        public List<WSP_ContactoEntidad> ObtenerContactosInactivos() {
            return _contactoDAL.ObtenerContactosInactivos();
        }

        public List<WSP_ContactoEntidad> ObtenerTodosLosContactosPorCodigoDePais(string codigoPais) {
            return _contactoDAL.ObtenerTodosLosContactosPorCodigoDePais(codigoPais);
        }

        public List<WSP_ContactoEntidad> ObtenerContactosActivosPorCodigoDePais(string codigoPais) {
            return _contactoDAL.ObtenerContactosActivosPorCodigoDePais(codigoPais);
        }

        public List<WSP_ContactoEntidad> ObtenerContactosInactivosPorCodigoDePais(string codigoPais) {
            return _contactoDAL.ObtenerContactosInactivosPorCodigoDePais(codigoPais);
        }

        public List<WSP_ContactoEntidad> ObtenerContactosPorIdsContacto(List<int> ids) {
            string include = $"({String.Join(",", ids)})";
            return _contactoDAL.ObtenerContactosPorIdsContacto(include);
        }
        
        public WSP_ContactoEntidad ObtenerContactoPorIdContacto(int idContacto) {
            return _contactoDAL.ObtenerContactoPorIdContacto(idContacto);
        }

        public bool InsertarContacto(WSP_ContactoEntidad contacto) {
            return _contactoDAL.InsertarContacto(contacto);
        }

        public bool ActualizarContactoPorIdContacto(WSP_ContactoEntidad contacto) {
            return _contactoDAL.ActualizarContactoPorIdContacto(contacto);
        }

        public bool ActualizarEstadoDeSalaMaestraPorIdContacto(int idContacto, bool estado) {
            return estado ? _contactoDAL.ActivarContactoPorIdContacto(idContacto) : _contactoDAL.DesactivarContactoPorIdContacto(idContacto);
        }

        public bool ActivarContactoPorIdContacto(int idContacto) {
            return _contactoDAL.ActivarContactoPorIdContacto(idContacto);
        }

        public bool DesactivarContactoPorIdContacto(int idContacto) {
            return _contactoDAL.DesactivarContactoPorIdContacto(idContacto);
        }

        public bool ActualizarEstadoDeSalaMaestraPorCodigoDePais(string codigoPais, bool estado) {
            return estado ? _contactoDAL.ActivarContactosPorCodigoDePais(codigoPais) : _contactoDAL.DesactivarContactosPorCodigoDePais(codigoPais);
        }

        public bool ActivarContactosPorCodigoDePais(string codigoPais) {
            return _contactoDAL.ActivarContactosPorCodigoDePais(codigoPais);
        }

        public bool DesactivarContactosPorCodigoDePais(string codigoPais) {
            return _contactoDAL.DesactivarContactosPorCodigoDePais(codigoPais);
        }

        public bool EliminarContactoPorIdContacto(int idContacto) {
            return _contactoDAL.EliminarContactoPorIdContacto(idContacto);
        }
    }
}
