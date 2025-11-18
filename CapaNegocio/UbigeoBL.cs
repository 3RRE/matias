using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class UbigeoBL
    {
        private UbigeoDAL ubigeoDal = new UbigeoDAL();

        public List<UbigeoEntidad> ListaPaises()
        {
            return ubigeoDal.ListaPaises();
        }

        public List<UbigeoEntidad> ListadoDepartamento()
        {
            return ubigeoDal.ListadoDepartamento();
        }
        public List<UbigeoEntidad> GetListadoProvincia(int DepartamentoID)
        {
            return ubigeoDal.GetListadoProvincia(DepartamentoID);
        }
        public List<UbigeoEntidad> GetListadoDistrito(int ProvinciaID, int DepartamentoID)
        {
            return ubigeoDal.GetListadoDistrito(ProvinciaID, DepartamentoID);
        }
        public UbigeoEntidad GetDatosUbigeo(int CodUbigeo)
        {
            return ubigeoDal.GetDatosUbigeo(CodUbigeo);
        }

        public List<UbigeoEntidad> ListaPaisesConCodigoTelefonico() {
            return ubigeoDal.ListaPaisesConCodigoTelefonico();
        }
    }
}
