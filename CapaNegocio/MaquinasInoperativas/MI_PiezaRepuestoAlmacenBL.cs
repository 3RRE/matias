using CapaDatos.MaquinasInoperativas;
using CapaEntidad.MaquinasInoperativas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MaquinasInoperativas {
    public class MI_PiezaRepuestoAlmacenBL {


        MI_PiezaRepuestoAlmacenDAL capaDatos = new MI_PiezaRepuestoAlmacenDAL();

        public List<MI_PiezaRepuestoAlmacenEntidad> PiezaRepuestoAlmacenListadoCompletoJson() {
            return capaDatos.GetAllPiezaRepuestoAlmacen();
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> PiezaRepuestoAlmacenListadoActiveJson() {
            return capaDatos.GetAllPiezaRepuestoAlmacenActive();
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> GetPiezaRepuestoAlmacenPropioxCodPiezaRepuesto(int codPiezaRepuesto, int codUsuario)
        {
            return capaDatos.GetPiezaRepuestoAlmacenPropioxCodPiezaRepuesto(codPiezaRepuesto,codUsuario);
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> GetPiezaRepuestoAlmacenAjenoxCodPiezaRepuesto(int codPiezaRepuesto, int codUsuario)
        {
            return capaDatos.GetPiezaRepuestoAlmacenAjenoxCodPiezaRepuesto(codPiezaRepuesto,codUsuario);
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> GetPiezaRepuestoAlmacenTodoxCodPiezaRepuesto(int codPiezaRepuesto)
        {
            return capaDatos.GetPiezaRepuestoAlmacenTodoxCodPiezaRepuesto(codPiezaRepuesto);
        }
        
        public MI_PiezaRepuestoAlmacenEntidad PiezaRepuestoAlmacenCodObtenerJson(int cod) {
            return capaDatos.GetCodPiezaRepuestoAlmacen(cod);
        }
        public int PiezaRepuestoAlmacenInsertarJson(MI_PiezaRepuestoAlmacenEntidad Entidad) {
            var cod = capaDatos.InsertarPiezaRepuestoAlmacen(Entidad);

            return cod;
        }
        public bool PiezaRepuestoAlmacenEditarJson(MI_PiezaRepuestoAlmacenEntidad Entidad)
        {
            var status = capaDatos.EditarPiezaRepuestoAlmacen(Entidad);

            return status;
        }
        public bool RevisarExistenciaPiezaRepuestoAlmacen(MI_PiezaRepuestoAlmacenEntidad Entidad) {
            var status = capaDatos.RevisarExistenciaPiezaRepuestoAlmacen(Entidad);

            return status;
        }
        public bool PiezaRepuestoAlmacenEliminarJson(int cod) {
            var status = capaDatos.EliminarPiezaRepuestoAlmacen(cod);

            return status;
        }


        public bool EditarCantidadPiezaRepuestoAlmacen(MI_PiezaRepuestoAlmacenEntidad Entidad) {
            var status = capaDatos.EditarCantidadPiezaRepuestoAlmacen(Entidad);

            return status;
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxTipo(int codTipo) {
            return capaDatos.GetAllPiezaRepuestoAlmacenxTipo(codTipo);
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxTipoxAlmacen(string codTipo,int codAlmacen) {
            return capaDatos.GetAllPiezaRepuestoAlmacenxTipoxAlmacen(codTipo, codAlmacen);
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxPiezaxAlmacen( int codAlmacen) {
            return capaDatos.GetAllPiezaRepuestoAlmacenxPiezaxAlmacen( codAlmacen);
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxRepuestoxAlmacen( int codAlmacen) {
            return capaDatos.GetAllPiezaRepuestoAlmacenxRepuestoxAlmacen( codAlmacen);
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxAlmacen(int codAlmacen) {
            return capaDatos.GetAllPiezaRepuestoAlmacenxAlmacen(codAlmacen);
        }



        public bool AgregarCantidadPendientePiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen,int cantPendiente)
        {
            var status = capaDatos.AgregarCantidadPendientePiezaRepuestoAlmacen(codPiezaRepuestoAlmacen, cantPendiente);

            return status;
        }

        public bool AceptarDescontarCantidadPendientePiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen, int cantPendiente)
        {
            var status = capaDatos.AceptarDescontarCantidadPendientePiezaRepuestoAlmacen(codPiezaRepuestoAlmacen, cantPendiente);

            return status;
        }
        public bool AceptarAgregarCantidadPendientePiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen, int cantPendiente)
        {
            var status = capaDatos.AceptarAgregarCantidadPendientePiezaRepuestoAlmacen(codPiezaRepuestoAlmacen, cantPendiente);

            return status;
        }
        public bool RechazarCantidadPendientePiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen, int cantPendiente)
        {
            var status = capaDatos.RechazarCantidadPendientePiezaRepuestoAlmacen(codPiezaRepuestoAlmacen, cantPendiente);

            return status;
        }

        public bool DescontarCantidadPiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen, int cantPendiente)
        {
            var status = capaDatos.DescontarCantidadPiezaRepuestoAlmacen(codPiezaRepuestoAlmacen, cantPendiente);

            return status;
        }
        
    }
}
