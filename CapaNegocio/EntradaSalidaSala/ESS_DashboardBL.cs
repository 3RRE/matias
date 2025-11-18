using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala.CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala
{
    public class ESS_DashboardBL
    {
        private ESS_DashboardDAL _dashboardDal = new ESS_DashboardDAL();


        public List<ESS_DashboardLudopatasEntidad> GetListDashboardLudopatasEntidad(int[] codSala, DateTime fechaIni, DateTime fechaFin)
        {
            return _dashboardDal.GetListDashboardLudopatasEntidad(codSala, fechaIni, fechaFin);
        }
        public List<ESS_DashboardReacudacionEntidad> GetListDashboardReacudacionEntidad(int[] codSala, DateTime fechaIni, DateTime fechaFin)
        {
            return _dashboardDal.GetListDashboardReacudacionEntidad(codSala, fechaIni, fechaFin);
        }
        public List<ESS_DashboardCajasTemporizadasEntidad> GetListDashboardCajasTemporizadasEntidad(int[] codSala, DateTime fechaIni, DateTime fechaFin)
        {
            return _dashboardDal.GetListDashboardCajasTemporizadasEntidad(codSala, fechaIni, fechaFin);
        }
        public List<ESS_DashboardEnteReguladoraEntidad> GetListDashboardEnteReguladoraEntidad(int[] codSala, DateTime fechaIni, DateTime fechaFin)
        {
            return _dashboardDal.GetListDashboardEnteReguladoraEntidad(codSala, fechaIni, fechaFin);
        }
        public List<ESS_DashboardOcurrenciasLogEntidad> GetListDashboardOcurrenciasLogEntidad(int[] codSala, DateTime fechaIni, DateTime fechaFin)
        {
            return _dashboardDal.GetListDashboardOcurrenciasLogEntidad(codSala, fechaIni, fechaFin);
        }
    }
}