using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class GC_ComiteCambiosBL
    {
        private GC_ComiteCambiosDAL gcComiteCambiosDal = new GC_ComiteCambiosDAL();
        public bool ComiteCambioGuardarJson(ComiteCambios comite)
        {
            return gcComiteCambiosDal.ComiteCambiosGuardarJson(comite);
        }

        public List<ComiteCambios> ComiteCambiosListadoJson()
        {
            return gcComiteCambiosDal.ComiteCambiosListadoJson();
        }

        public bool ComiteCambioActualizarJson()
        {
            return gcComiteCambiosDal.ComiteCambioActualizarJson();
        }

        public bool VerificarEmpleadoComiteJson(int id)
        {
            return gcComiteCambiosDal.VerificarEmpleadoComiteCambio(id);
        }
    }
}
