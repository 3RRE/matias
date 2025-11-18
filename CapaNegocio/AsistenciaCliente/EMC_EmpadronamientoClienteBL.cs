using CapaDatos.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente;
using System;
using System.Collections.Generic;

namespace CapaNegocio.AsistenciaCliente
{
    public class EMC_EmpadronamientoClienteBL
    {
        private readonly EMC_EmpadronamientoClienteDAL _empadronamientoDAL = new EMC_EmpadronamientoClienteDAL();

        public List<EMC_EmpadronamientoClienteEntidad> GetListadoEmpadronamientoCliente(DateTime fechaIni, DateTime fechaFin, int SalaId)
        {
            return _empadronamientoDAL.GetListadoEmpadronamientoCliente(fechaIni, fechaFin, SalaId);
        }

        public EMC_EmpadronamientoClienteEntidad GetEmpadronamientoCliente(DateTime fechaIni,  string nro)
        {
            return _empadronamientoDAL.GetEmpadronamientoCliente(fechaIni, nro);
        }
        public bool GuardarEmpadronamientoCliente(EMC_EmpadronamientoClienteEntidad cliente)
        {
            return _empadronamientoDAL.GuardarEmpadronamientoCliente(cliente);
        }

        public bool GuardarEmpadronamientoClienteMobil(EMC_EmpadronamientoClienteEntidad cliente)
        {
            return _empadronamientoDAL.GuardarEmpadronamientoClienteMobil(cliente);
        }

        public bool EliminarEmpadronamientoCliente(Int64 id)
        {
            return _empadronamientoDAL.EliminarEmpadronamientoCliente(id);
        }

        // Empradonamiento Cliente
        public List<EMC_EmpadronamientoClienteEntidad> ListarEmpadronamientoCliente(int roomId, DateTime fromDate, DateTime toDate)
        {
            return _empadronamientoDAL.ListarEmpadronamientoCliente(roomId, fromDate, toDate);
        }

        public EMC_EmpadronamientoClienteEntidad ObtenerEmpadronamientoCliente(int customerId, DateTime todayDate)
        {
            return _empadronamientoDAL.ObtenerEmpadronamientoCliente(customerId, todayDate);
        }

        public EMC_EmpadronamientoClienteEntidad ObtenerEmpadronamientoCliente(long empadronamientoId)
        {
            return _empadronamientoDAL.ObtenerEmpadronamientoCliente(empadronamientoId);
        }

        public long GuardarEmpadronamientoClienteV2(EMC_EmpadronamientoClienteEntidad empadronamiento)
        {
            return _empadronamientoDAL.GuardarEmpadronamientoClienteV2(empadronamiento);
        }

        public bool RegistrarFechaHoraSalida(EMC_EmpadronamientoClienteEntidad empadronamiento)
        {
            return _empadronamientoDAL.RegistrarFechaHoraSalida(empadronamiento);
        }
    }
}
