using CapaDatos.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.AsistenciaCliente
{
    public class AST_AsistenciaClienteSalaBL
    {
        private AST_AsistenciaClienteSalaDAL asistenciaDal = new AST_AsistenciaClienteSalaDAL();
        public List<AST_AsistenciaClienteSalaEntidad> GetListadoAsistenciaClienteSala(int ClienteId, int SalaId)
        {
            return asistenciaDal.GetListadoAsistenciaClienteSala(ClienteId,SalaId);
        }
        public AST_AsistenciaClienteSalaEntidad GetAsistenciaClienteSalaID(int asistenciaId)
        {
            return asistenciaDal.GetAsistenciaClienteSalaID(asistenciaId);
        }
        public int GuardarAsistenciaClienteSala(AST_AsistenciaClienteSalaEntidad asistencia)
        {
            return asistenciaDal.GuardarAsistenciaClienteSala(asistencia);
        }
        public List<AST_AsistenciaClienteSalaEntidad> GetListadoAsistenciaCliente(int ClienteId)
        {
            return asistenciaDal.GetListadoAsistenciaCliente(ClienteId);
        }
        public List<AST_AsistenciaClienteSalaEntidad> GetListadoAsistenciaClienteFiltros(int ClienteId,DateTime fechaIni,DateTime fechaFin,int SalaId)
        {
            return asistenciaDal.GetListadoAsistenciaClienteFiltros(ClienteId, fechaIni, fechaFin,SalaId);
        }
        public List<AST_AsistenciaClienteSalaEntidad> GetListadoAsistenciaSalaFiltros(string ArraySalaId,DateTime fechaIni,DateTime fechaFin)
        {
            return asistenciaDal.GetListadoAsistenciaSalaFiltros(ArraySalaId, fechaIni, fechaFin);
        }
        public bool EliminarAsistenciaClienteSala(int AsistenciaId)
        {
            return asistenciaDal.EliminarAsistenciaClienteSala(AsistenciaId);
        }
        public AST_AsistenciaClienteSalaEntidad GetUltimaAsistenciaClienteSalaID(int ClienteId, int SalaId) {
            return asistenciaDal.GetUltimaAsistenciaClienteSalaID(ClienteId, SalaId);
        }

        public List<AST_ClienteSala> GetListadoClienteSala(string ArraySalaId, DateTime fechaIni, DateTime fechaFin)
        {
            return asistenciaDal.GetListadoClienteSala(ArraySalaId, fechaIni, fechaFin);
        }

        public List<AST_ClienteSala> GetReporteListaClienteSala(string array_salas, DateTime fechaIni, DateTime fechaFin, bool verInfoContacto) {
            return asistenciaDal.GetReporteListaClienteSala(array_salas, fechaIni, fechaFin, verInfoContacto);
        }

        public List<AST_ClienteSalaGlobal> GetAllClientesSala(int salaId)
        {
            return asistenciaDal.GetAllClientesSala(salaId);
        }

    }
}
