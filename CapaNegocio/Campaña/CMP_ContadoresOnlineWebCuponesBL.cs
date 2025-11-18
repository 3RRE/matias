using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Campaña
{
    public class CMP_ContadoresOnlineWebCuponesBL
    {
        private CMP_ContadoresOnlineWebCuponesDAL cmpcontadores_dal = new CMP_ContadoresOnlineWebCuponesDAL();

        public List<CMP_ContadoresOnlineWebCuponesEntidad> GetListadoCMP_ContadoresOnlineWebCupones()
        {
            return cmpcontadores_dal.GetListadoCMP_ContadoresOnlineWebCupones();
        }
        public CMP_ContadoresOnlineWebCuponesEntidad GetCMP_ContadoresOnlineWebCuponesId(Int64 id)
        {
            return cmpcontadores_dal.GetCMP_ContadoresOnlineWebCuponesId(id);
        }
        public Int64 GuardarCMP_ContadoresOnlineWebCupones(CMP_ContadoresOnlineWebCuponesEntidad contador)
        {
            return cmpcontadores_dal.GuardarCMP_ContadoresOnlineWebCupones(contador);
        }
    }
}
