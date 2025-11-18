using CapaDatos;
using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Campaña
{
    public class CMP_Campaña_ParametrosBL
    {
        private CMP_Campania_ParametrosDAL campa_dal = new CMP_Campania_ParametrosDAL();

        public CMP_Campania_ParametrosEntidad CampañaParametrosIdObtenerJson(Int64 id)
        {
            return campa_dal.GetCampañaParametrosID(id);
        }

        public int CampañaParametrosInsertarJson(CMP_Campania_ParametrosEntidad campaña)
        {
            return campa_dal.GuardarCampañaParametros(campaña);
        }
        public bool CampañaParametrosEditarJson(CMP_Campania_ParametrosEntidad campaña)
        {
            return campa_dal.EditarCampañaParametros(campaña);
        }
    }
}
