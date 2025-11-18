using CapaDatos.Notificaciones.Mantenedores;
using CapaEntidad.Notificaciones.DTO.Mantenedores;
using CapaEntidad.Notificaciones.Entity.Mantenedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Notificaciones.Mantenedores {
    public class SistemaBL {
        private readonly SistemaDAL sistemaDAL;
            public SistemaBL() {
            sistemaDAL = new SistemaDAL();
        }

        public List<SistemaDto> ObtenerSistemas() {
            return sistemaDAL.ObtenerSistema();
        }

        public bool InsertarSistema(Sistema sistema) {
            return sistemaDAL.InsertarSistema(sistema) > 0;
        }

        public bool ActualizarSistema(Sistema sistema) {
            return sistemaDAL.ActualizarSistema(sistema) > 0;
        }

        public SistemaDto ObtenerSistemaPorId(int id) {
            return sistemaDAL.ObtenerSistemaPorId(id);
        }

        public bool EliminarSistema(int id) {
            return sistemaDAL.EliminarSistema(id) > 0;
        }
    }
}
