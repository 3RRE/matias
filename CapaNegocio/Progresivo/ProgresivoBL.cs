using CapaDatos.Progresivo;
using CapaEntidad;
using CapaEntidad.ProgresivoOffline;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Progresivo
{
    public class ProgresivoBL
    {
        private ProgresivoDAL _progresivoDal = new ProgresivoDAL();


        

        public List<ProgresivoEntidad> ListarProgresivos()
        {
            return _progresivoDal.GetProgresivos();
        }
        public bool ProgresivoHistoricoInsertarJson(HistorialProgresivoEntidad obj)
        {
            return _progresivoDal.ProgresivoHistoricoInsertarJson(obj);
        }

        public string NombreProgresivo(int codProgresivo, int codSala) {
            return _progresivoDal.ProgresivoNombre(codProgresivo, codSala);
        }

        #region Progresivo Offline


        public CabeceraOfflineEntidad GetUltimaFechaProgresivoxSala(int codSala, int codProgresivo)
        {
            return _progresivoDal.GetUltimaFechaProgresivoxSala(codSala, codProgresivo);
        }

        public List<DetalleOfflineEntidad> DetallesContadoresPremio(string id, string id2, int id3, int id4)
        {
            return _progresivoDal.DetallesContadoresPremio(id,id2,id3,id4);
        }
        public List<ProgresivoOfflineEntidad> GetProgresivoOfflinexSalaxProgresivo(int codSala)
        {
            return _progresivoDal.GetProgresivoOfflinexSalaxProgresivo(codSala);
        }
        public bool ProgresivoOfflineInsertarJson(ProgresivoOfflineEntidad obj)
        {
            return _progresivoDal.ProgresivoOfflineInsertarJson(obj);
        }
        public bool ProgresivoOfflineActualizarJson(ProgresivoOfflineEntidad obj)
        {
            return _progresivoDal.ProgresivoOfflineActualizarJson(obj);
        }
        public List<ProgresivoOfflineEntidad> GetProgresivoOffline()
        {
            return _progresivoDal.GetProgresivoOffline();
        }
        public List<CabeceraOfflineEntidad> GetCabeceraOfflinexSalaxProgresivo(int codSala, int codProgresivo, DateTime fechaini, DateTime fechafin)
        {
            return _progresivoDal.GetCabeceraOfflinexSalaxProgresivo(codSala, codProgresivo, fechaini, fechafin);
        }
        public int CabeceraOfflineInsertarJson(CabeceraOfflineEntidad obj)
        {
            return _progresivoDal.CabeceraOfflineInsertarJson(obj);
        }
        public List<CabeceraOfflineEntidad> GetCabeceraOffline()
        {
            return _progresivoDal.GetCabeceraOffline();
        }

        public List<DetalleOfflineEntidad> GetDetalleOfflinexSalaxProgresivo(int codSala, int codProgresivo, int codCabecera)
        {
            return _progresivoDal.GetDetalleOfflinexSalaxProgresivo(codSala, codProgresivo, codCabecera);
        }
        public bool DetalleOfflineInsertarJson(DetalleOfflineEntidad obj)
        {
            return _progresivoDal.DetalleOfflineInsertarJson(obj);
        }
        public List<DetalleOfflineEntidad> GetDetalleOfflinexCabecera(int codCabecera)
        {
            return _progresivoDal.GetDetalleOfflinexCabecera(codCabecera);
        }
        public List<DetalleOfflineEntidad> GetDetalleOffline()
        {
            return _progresivoDal.GetDetalleOffline();
        }
        public List<CabeceraOfflineEntidad> ListarCabecerasPorFechaYSala(DateTime fechaInicio, DateTime fechaFin, string listaSalas) {
            return _progresivoDal.ListarCabecerasPorFechaYSala(fechaInicio,fechaFin,listaSalas);
        }



        public List<Asignacion_M_T> GetAsignacion_M_T()
        {
            return _progresivoDal.GetAsignacion_M_T();
        }
        public int Asignacion_M_TInsertarJson(Asignacion_M_T obj)
        {
            return _progresivoDal.Asignacion_M_TInsertarJson(obj);
        }

        public List<DetalleContadoresGame> GetDetalleContadoresGame()
        {
            return _progresivoDal.GetDetalleContadoresGame();
        }
        public int DetalleContadoresGameInsertarJson(DetalleContadoresGame obj)
        {
            return _progresivoDal.DetalleContadoresGameInsertarJson(obj);
        }
        #endregion 

    }
}
