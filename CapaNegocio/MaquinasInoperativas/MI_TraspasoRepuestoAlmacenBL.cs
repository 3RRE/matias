using CapaDatos.MaquinasInoperativas;
using CapaEntidad.MaquinasInoperativas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MaquinasInoperativas
{
    public class MI_TraspasoRepuestoAlmacenBL
    {

        MI_TraspasoRepuestoAlmacenDAL capaDatos = new MI_TraspasoRepuestoAlmacenDAL();

        public List<MI_TraspasoRepuestoAlmacenEntidad> TraspasoRepuestoAlmacenListadoCompletoJson()
        {
            return capaDatos.GetAllTraspasoRepuestoAlmacen();
        }
        public List<MI_TraspasoRepuestoAlmacenEntidad> TraspasoRepuestoAlmacenListadoCompletoxMaquinaInoperativaJson(int cod)
        {
            return capaDatos.GetAllTraspasoRepuestoAlmacenxMaquinaInoperativa(cod);
        }
        public List<MI_TraspasoRepuestoAlmacenEntidad> TraspasoRepuestoAlmacenListadoCompletoxMaquinaInoperativaJsonSinAlmacenes(int cod)
        {
            return capaDatos.GetAllTraspasoRepuestoAlmacenxMaquinaInoperativaSinAlmacenes(cod);
        }
        public List<MI_TraspasoRepuestoAlmacenEntidad> TraspasoRepuestoAlmacenListadoActiveJson()
        {
            return capaDatos.GetAllActiveTraspasoRepuestoAlmacen();
        }
        public List<MI_TraspasoRepuestoAlmacenEntidad> TraspasoRepuestoAlmacenListadoInactiveJson()
        {
            return capaDatos.GetAllInactiveTraspasoRepuestoAlmacen();
        }
        public MI_TraspasoRepuestoAlmacenEntidad TraspasoRepuestoAlmacenCodObtenerJson(int cod)
        {
            return capaDatos.GetCodTraspasoRepuestoAlmacen(cod);
        }
        public int TraspasoRepuestoAlmacenInsertarJson(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            var cod = capaDatos.InsertarTraspasoRepuestoAlmacen(Entidad);

            return cod;
        }
        public bool TraspasoRepuestoAlmacenEditarJson(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            var status = capaDatos.EditarTraspasoRepuestoAlmacen(Entidad);

            return status;
        }
        public bool TraspasoRepuestoAlmacenEliminarJson(int cod)
        {
            var status = capaDatos.EliminarTraspasoRepuestoAlmacen(cod);

            return status;
        }


        public bool TraspasoRepuestoAlmacenAceptarJson(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            var status = capaDatos.AceptarTraspasoRepuestoAlmacen(Entidad);

            return status;
        }

        public bool TraspasoRepuestoAlmacenRechazarJson(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            var status = capaDatos.RechazarTraspasoRepuestoAlmacen(Entidad);

            return status;
        }

        public List<MI_TraspasoRepuestoAlmacenEntidad> GetAllTraspasoRepuestoAlmacenxCodMaquinaInoperativa(int cod, int estado)
        {
            return capaDatos.GetAllTraspasoRepuestoAlmacenxCodMaquinaInoperativa(cod, estado);
        }
        
        public List<MI_TraspasoRepuestoAlmacenEntidad> GetAllTraspasoRepuestoAlmacenxCodMaquinaInoperativaSinAlmacenes(int cod, int estado)
        {
            return capaDatos.GetAllTraspasoRepuestoAlmacenxCodMaquinaInoperativaSinAlmacenes(cod, estado);
        }

    }
}
