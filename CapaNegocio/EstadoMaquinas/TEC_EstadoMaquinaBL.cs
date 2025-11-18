using CapaDatos.EntradaSalidaSala;
using CapaDatos.MaquinaEstado;
using CapaDatos.Sala;
using CapaEntidad.EntradaSalidaSala;
using CapaEntidad.MaquinaEstado;
using CapaEntidad.Reclamaciones;
using CapaEntidad.Sala;
using S3k.Utilitario.clases_especial;
using System;
using System.Collections.Generic;

namespace CapaNegocio.EstadoMaquinas {

    public class TEC_EstadoMaquinaBL {
        private readonly TEC_EstadoMaquinaDAL estadoMaquinaDAL = new TEC_EstadoMaquinaDAL();
        public (List<TEC_EstadoMaquinaEntidad> maquinaestadoLista, ClaseError error) maquianestadoListarJson() {
            return estadoMaquinaDAL.maquianestadoListarJson();
        }

        public (List<TEC_EstadoMaquinaEntidad> maquinaestadoLista, ClaseError error) maquinaestadoListarxSalaFechaJson(int [] salas, DateTime fechaini, DateTime fechafin) {
            return estadoMaquinaDAL.maquinaestadoListarxSalaFechaJson(salas, fechaini, fechafin);
        }
        public List<(int CodSala, DateTime FechaOperacion)> ObtenerFechaOperacionUltimaxSala(int [] salas) {
            return estadoMaquinaDAL.ObtenerFechaOperacionUltimaxSala(salas);
        }
        public (List<TEC_EstadoMaquinaEntidad> maquinaestadoLista, ClaseError error) maquinaestadoListarxSalaUltimaFechaOperacionJson(int [] salas) {
            return estadoMaquinaDAL.maquinaestadoListarxSalaUltimaFechaOperacionJson(salas);
        }
         
        public (Int64 total, ClaseError error) maquinaestadoTotalSalaJson(Int64 id) {
            return estadoMaquinaDAL.maquinaestadoTotalSalaJson(id);
        }
        public int GuardarMaquinaEstado(TEC_EstadoMaquinaEntidad maquinaestado) {
            return estadoMaquinaDAL.GuardarMaquinaEstado(maquinaestado);
        }
        public void GuardarHistorialMaquina(List<TEC_HistorialMaquinaEntidad> listadomaquinas) {
            estadoMaquinaDAL.GuardarHistorialMaquina(listadomaquinas);
        }

        public (List<TEC_ConsolidadoMaquinaEntidad> consolidadoLista, ClaseError error) ObtenerConsolidado(string strElementos, DateTime fechaini, DateTime fechafin) {
            TEC_EstadoMaquinaDAL maquinaEstadoDAL = new TEC_EstadoMaquinaDAL();
            return estadoMaquinaDAL.ObtenerConsolidado(strElementos, fechaini, fechafin);
        }
        
          public (List<TEC_EstadoMaquinaEntidad> lista, ClaseError error) TEC_EstadoMaquinaListarporIdsJson(string ids) {
            return estadoMaquinaDAL.TEC_EstadoMaquinaListarporIdsJson(ids);
                }
         
        public string NombreId(int usuarioId) {
            TEC_EstadoMaquinaDAL estadoMaquinaDAL = new TEC_EstadoMaquinaDAL();
            return estadoMaquinaDAL.NombreId(usuarioId);
        }
        public TEC_EstadoMaquinaEntidad GetEstadoMaquinaPorId(int IdEstadoMaquina) => estadoMaquinaDAL.GetMaquinaEstadoPorId(IdEstadoMaquina);


        public List<TEC_EstadoMaquinaDetalleEntidad> ListarRetiroTemporal(int Id) => estadoMaquinaDAL.ListarRetiroTemporal(Id);
        public int InsertarRetiroTemporal(TEC_EstadoMaquinaDetalleEntidad model) => estadoMaquinaDAL.InsertarRetiroTemporal(model);

        public bool EliminarRetiroTemporal(int idEstadoMaquinaDetalle) => estadoMaquinaDAL.EliminarRetiroTemporal(idEstadoMaquinaDetalle);
        public string BuscarEstadoMaquinaxCodMaquina(string CodMaquina, DateTime FechaOperacion, int CodSala) => estadoMaquinaDAL.BuscarEstadoMaquinaxCodMaquina(CodMaquina, FechaOperacion, CodSala);
        public bool ActualizarCantidadRetiroTemporal(int idEstadoMaquina) {
            return estadoMaquinaDAL.ActualizarCantidadRetiroTemporal(idEstadoMaquina);
        }

        public int InsertarRegistroMaquina(TEC_RegistroMaquinaEntidad registro) => estadoMaquinaDAL.InsertarRegistroMaquina(registro);

        public int ObtenerTotalMaquinaPorCodSala(int codSala) => estadoMaquinaDAL.ObtenerTotalMaquinaPorCodSala(codSala);

        public (List<TEC_RegistroMaquinaEntidad> maquinaestadoLista, ClaseError error) ListaReporteRegistroMaquinaxSalaJson(string salas) {
            return estadoMaquinaDAL.ListaReporteRegistroMaquinaxSalaJson(salas);
        }
        public TEC_RegistroMaquinaEntidad ReporteRegistroMaquinaxSalaJson(int  sala) {
            return estadoMaquinaDAL.ReporteRegistroMaquinaxSalaJson(sala);
        }

        public int ExisteRegistro(DateTime FechaOperacion, int sala_id) {
            return estadoMaquinaDAL.ExisteRegistro(FechaOperacion, sala_id);
        }
        public bool ActualizarMaquinaEstado(TEC_EstadoMaquinaEntidad maquinaestado,int idEstadoMaquina) {
            return estadoMaquinaDAL.ActualizarMaquinaEstado(maquinaestado, idEstadoMaquina);
        }
        public List<TEC_EstadoMaquinaEntidad> ListadoEstadoMaquina(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            return estadoMaquinaDAL.ListadoEstadoMaquina(codSala, fechaIni, fechaFin);
        }

        public bool ActualizarRegistroMaquina(TEC_RegistroMaquinaEntidad registroMaquina) {
            if(registroMaquina.TipoRegistroMaquina == "INDECI") {
                return estadoMaquinaDAL.ActualizarRegistroMaquinaINDECI(registroMaquina);

            }else if(registroMaquina.TipoRegistroMaquina == "RD") {
                return estadoMaquinaDAL.ActualizarRegistroMaquinaRD(registroMaquina);
            }
            throw new ArgumentException("El Tipo Registro Maquina debe ser válido.");
            
        }
        public int CrearSalaRegistroMaquina(TEC_RegistroMaquinaEntidad registro) {
            return estadoMaquinaDAL.CrearSalaRegistroMaquina(registro);
        }
        public bool AgregarRetiroTemporal(int idEstadoMaquina, string EstadoMaquina) {
            return estadoMaquinaDAL.AgregarRetiroTemporal(idEstadoMaquina, EstadoMaquina);
        }
        public bool QuitarRetiroTemporal(int idEstadoMaquina, string EstadoMaquina) {
            return estadoMaquinaDAL.QuitarRetiroTemporal(idEstadoMaquina, EstadoMaquina);
        }
        public bool ExisteRegistroHistorialMaquinaxFechaOperacion(DateTime FechaOperacion, int CodSala) {
            return estadoMaquinaDAL.ExisteRegistroHistorialMaquinaxFechaOperacion(FechaOperacion, CodSala);
        } 
        public int EliminarRegistrosHistorialMaquinaAntiguos(DateTime FechaOperacion,int CodSala) {
            return estadoMaquinaDAL.EliminarRegistrosHistorialMaquinaAntiguos(FechaOperacion, CodSala);
        }public TEC_EstadoMaquinaEntidad ObtenerEstadoMaquinaporId(int IdEstadoMaquina) {
            return estadoMaquinaDAL.ObtenerEstadoMaquinaporId(IdEstadoMaquina);
        }
        
    }
}
