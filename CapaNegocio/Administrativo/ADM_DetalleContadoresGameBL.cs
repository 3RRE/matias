using CapaDatos.Administrativo;
using CapaEntidad.Administrativo;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Administrativo {
    public class ADM_DetalleContadoresGameBL
    {
        private readonly ADM_DetalleContadoresGameDAL _detalleContadoresDAL = new ADM_DetalleContadoresGameDAL();
        public List<ADM_DetalleContadoresGameEntidad> GetListado_DetalleContadoresGamePorFechaOperacion(int CodSala, DateTime FechaOperacion)
        {
            return _detalleContadoresDAL.GetListado_DetalleContadoresGamePorFechaOperacion(CodSala, FechaOperacion);
        }
        public int Guardar_DetalleContadoresGame(ADM_DetalleContadoresGameEntidad contador)
        {
            return _detalleContadoresDAL.Guardar_DetalleContadoresGame(contador);
        }
        public List<ADM_DetalleContadoresGameEntidad> GetListado_DetalleContadoresGamePorQuery(string whereQuery, IDictionary<string, DateTime> fechaParametros)
        {
            return _detalleContadoresDAL.GetListado_DetalleContadoresGamePorQuery(whereQuery, fechaParametros);
        }
        public bool Eliminar_DetalleContadoresGamePorFecha(int CodSala, DateTime fechaOperacion)
        {
            return _detalleContadoresDAL.Eliminar_DetalleContadoresGamePorFecha(CodSala, fechaOperacion);
        }
        public bool EditarDetalleContadoresGamePorMaquina(ADM_DetalleContadoresGameEntidad item) {
            return _detalleContadoresDAL.EditarDetalleContadoresGamePorMaquina(item);
        }
    }
}
