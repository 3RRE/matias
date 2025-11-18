using CapaDatos.Progresivo;
using CapaEntidad.Progresivo;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Progresivo
{
    public class RegistroProgresivoBL
    {
        private readonly RegistroProgresivoDAL _registroProgresivoDAL = new RegistroProgresivoDAL();

        public bool HRPGuardarDetallePozo(RegistroProgresivoEntidad detalle)
        {
            bool response = false;

            detalle.FechaRegistro = DateTime.Now;

            long detalleId = _registroProgresivoDAL.HRPGuardarDetalle(detalle);

            if (detalleId > 0)
            {
                foreach (RegistroProgresivoPozoEntidad pozo in detalle.Pozos)
                {
                    pozo.DetalleId = detalleId;

                    _registroProgresivoDAL.HRPGuardarPozo(pozo);
                }

                response = true;
            }

            return response;
        }

        public List<RegistroProgresivoEntidad> HRPListarDetalle(int salaId, int progresivoIdOnline, int rows)
        {
            List<RegistroProgresivoEntidad> registros = _registroProgresivoDAL.HRPListarDetalle(salaId, progresivoIdOnline, rows);

            foreach(RegistroProgresivoEntidad registro in registros)
            {
                registro.Pozos = HRPListarPozo(registro.Id);
            }

            return registros;
        }

        public List<RegistroProgresivoPozoEntidad> HRPListarPozo(long detalleId)
        {
            return _registroProgresivoDAL.HRPListarPozo(detalleId);
        }

        public RegistroProgresivoEntidad HRPObtenerDetalle(int salaId, long detalleId)
        {
            RegistroProgresivoEntidad registro = _registroProgresivoDAL.HRPObtenerDetalle(salaId, detalleId);

            if(registro.Id > 0)
            {
                registro.Pozos = HRPListarPozo(registro.Id);
            }

            return registro;
        }

        public RegistroProgresivoEntidad HRPObtenerUltimoDetalle(int salaId, int progresivoIdOnline)
        {
            RegistroProgresivoEntidad registro = _registroProgresivoDAL.HRPObtenerUltimoDetalle(salaId, progresivoIdOnline);

            if (registro.Id > 0)
            {
                registro.Pozos = HRPListarPozo(registro.Id);
            }

            return registro;
        }
    }
}
