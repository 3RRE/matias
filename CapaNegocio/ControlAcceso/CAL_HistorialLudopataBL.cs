using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso.HistorialLudopata;
using CapaEntidad.ControlAcceso.HistorialLudopata.Dto;
using System;
using System.Collections.Generic;

namespace CapaNegocio.ControlAcceso {
    public class CAL_HistorialLudopataBL {
        private readonly CAL_HistorialLudopataDAL historialLudopataDAL;

        public CAL_HistorialLudopataBL() {
            historialLudopataDAL = new CAL_HistorialLudopataDAL();
        }

        public bool InsertarHistorialLudopata(CAL_HistorialLudopata historialLudopata) {
            return historialLudopataDAL.InsertarHistorialLudopata(historialLudopata) > 0;
        }

        public List<CAL_HistorialLudopataDto> ObtenerHistorialLudopata() {
            return historialLudopataDAL.ObtenerHistorialLudopata();
        }

        public List<CAL_HistorialLudopataDto> ObtenerHistorialLudopataPorFechas(DateTime fechaInicio, DateTime fechaFin) {
            return historialLudopataDAL.ObtenerHistorialLudopataPorFechas(fechaInicio, fechaFin);
        }
    }
}
