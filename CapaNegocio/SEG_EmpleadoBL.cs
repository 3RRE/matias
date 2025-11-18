using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class SEG_EmpleadoBL
    {
        private SEG_EmpleadoDAL segEmpleadoDAL = new SEG_EmpleadoDAL();
        public List<SEG_EmpleadoEntidad> EmpleadoListarJson()
        {
            return segEmpleadoDAL.EmpleadoListarJson();
        }
        
        public List<SEG_EmpleadoEntidad> EmpleadoEstadoActivoListarJson()
        {
            return segEmpleadoDAL.EmpleadoEstadoActivoListarJson();
        }
        public bool EmpleadoGuardarJson(SEG_EmpleadoEntidad empleado)
        {
            return segEmpleadoDAL.EmpleadoGuardarJson(empleado);
        }
        public Int32 GuardarEmpleadoGlpi(SEG_EmpleadoEntidad empleado) {
            return segEmpleadoDAL.GuardarEmpleadoGlpi(empleado);
        }
        public Int32 EmpleadoReconocimientoAPKGuardarJson(SEG_EmpleadoEntidad empleado)
        {
            return segEmpleadoDAL.EmpleadoReconocimientoAPKGuardarJson(empleado);
        }

        public SEG_EmpleadoEntidad EmpleadoIdObtenerJson(int empleadoId)
        {
            return segEmpleadoDAL.EmpleadoIdObtenerJson(empleadoId);
        }

        public bool EmpleadoActualizarJson(SEG_EmpleadoEntidad empleado)
        {
            return segEmpleadoDAL.EmpleadoActualizarJson(empleado);
        }

        public bool EstadoEmpleadoActualizarJson(int empleadoId, int estado)
        {
            return segEmpleadoDAL.EstadoEmpleadoActualizarJson(empleadoId, estado);
        }

        public List<EmpleadoEncriptacion> TecnicosEncriptacionListarJson()
        {
            //throw new NotImplementedException();
            return segEmpleadoDAL.TecnicosEncriptacionListarJson();
        }

        public SEG_EmpleadoEntidad EmpleadoxNroDocumentoJson(string nro_Documento)
        {
            return segEmpleadoDAL.EmpleadoxNroDocumentoJson(nro_Documento);
        }

        public bool EmpleadoFotoEditarJson(SEG_EmpleadoEntidad empleado)
        {
            return segEmpleadoDAL.EmpleadoFotoEditarJson(empleado);
        }

        //Ficha Sintomatologica 
        public bool EmpleadoActualizarFichaSintomatologicaJson(SEG_EmpleadoEntidad empleado)
        {
            return segEmpleadoDAL.EmpleadoActualizarFichaSintomatologicaJson(empleado);
        }

        public Int32 EmpleadoGuardarFichaSintomatologicaJson(SEG_EmpleadoEntidad empleado)
        {
            return segEmpleadoDAL.EmpleadoGuardarFichaSintomatologicaJson(empleado);
        }

        public SEG_EmpleadoEntidad EmpleadoxNroDocumentoFichaSintomatologicaJson(string DOI)
        {
            return segEmpleadoDAL.EmpleadoxNroDocumentoFichaSintomatologicaJson(DOI);
        }
        public List<SEG_EmpleadoEntidad> EmpleadoListarPorNoUsadosJson()
        {
            return segEmpleadoDAL.EmpleadoListarPorNoUsadosJson();
        }
        public List<SEG_EmpleadoEntidad> EmpleadoListarPorUsuariosJson()
        {
            return segEmpleadoDAL.EmpleadoListarPorUsuariosJson();
        }



    }
}
