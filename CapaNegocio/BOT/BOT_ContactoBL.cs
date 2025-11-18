using CapaDatos.Cortesias;
using CapaEntidad.BOT.Entities;
using System.Collections.Generic;

namespace CapaNegocio.Cortesias {
    public class BOT_ContactoBL {
        private readonly BOT_ContactoDAL contactoDAL;

        public BOT_ContactoBL() {
            contactoDAL = new BOT_ContactoDAL();
        }

        public List<BOT_ContactoEntidad> ObtenerContactos() {
            return contactoDAL.ObtenerContactos();
        }

        public BOT_ContactoEntidad ObtenerContactoPorId(int id) {
            return contactoDAL.ObtenerContactoPorId(id);
        }

        public bool InsertarContacto(BOT_ContactoEntidad contacto) {
            return contactoDAL.InsertarContacto(contacto) != 0;
        }

        public bool ActualizarContacto(BOT_ContactoEntidad contacto) {
            return contactoDAL.ActualizarContacto(contacto) != 0;
        }

        public bool EliminarContacto(int id) {
            return contactoDAL.EliminarContacto(id) != 0;
        }
    }
}
