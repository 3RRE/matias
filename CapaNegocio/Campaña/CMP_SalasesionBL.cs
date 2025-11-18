using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;
namespace CapaNegocio.Campaña
{
    public class CMP_SalasesionBL
    {
        private CMP_SalasesionDAL salasesion_dal = new CMP_SalasesionDAL();

        public List<CMP_SalasesionEntidad> CMPsalalibrexsala(int codsala)
        {
            return salasesion_dal.GetSalassesionxCodsala(codsala);
        }

        public int salasesionInsertarJson(CMP_SalasesionEntidad sala)
        {
            return salasesion_dal.GuardarSalasesion(sala);
        }
        public bool salasesionEliminarJson(Int64 id)
        {
            return salasesion_dal.eliminarSalasesion(id);
        }
    }
}
