using CapaDatos.Sala;
using CapaEntidad.Sala;
using System.Collections.Generic;

namespace CapaNegocio.Sala
{
    public class SL_ZonaBL
    {
        private readonly SL_ZonaDAL _zonaDAL = new SL_ZonaDAL();

        public int GuardarZona(SL_ZonaEntidad zona)
        {
            return _zonaDAL.GuardarZona(zona);
        }

        public bool ActualizarZona(SL_ZonaEntidad zona)
        {
            return _zonaDAL.ActualizarZona(zona);
        }

        public bool ActualizarEstadoZona(byte estado, int zonaId)
        {
            return _zonaDAL.ActualizarEstadoZona(estado, zonaId);
        }

        public SL_ZonaEntidad ObtenerZona(int zonaId)
        {
            return _zonaDAL.ObtenerZona(zonaId);
        }

        public List<SL_ZonaEntidad> ListarZona(List<int> salaIds)
        {
            return _zonaDAL.ListarZona(salaIds);
        }

        public List<SL_ZonaEntidad> ListarZonasPorSala(int salaId)
        {
            return _zonaDAL.ListarZonasPorSala(salaId);
        }
    }
}
