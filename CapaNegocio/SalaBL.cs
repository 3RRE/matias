using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaNegocio {
    public class SalaBL {
        private SalaDAL _salaDal = new SalaDAL();
        public List<SalaEntidad> ListadoSala() {
            return _salaDal.ListadoSala();
        }
        public SalaEntidad SalaListaIdJson(int salaId) {
            return _salaDal.SalaListaIdJson(salaId);
        }
        public bool SalaModificarJson(SalaEntidad sala) {
            return _salaDal.SalaModificarJson(sala);
        }
        //public List<SalaEntidad> ListadoSalaPorUsuario(int usuarioId)
        //{
        //    return _salaDal.ListadoSalaPorUsuario(usuarioId);
        //}
        public List<SalaEntidad> ListadoSalaPorUsuario(int usuarioId) {
            return _salaDal.ListadoSalaPorUsuario(usuarioId);
        }
        public List<SalaEntidad> ListadoSalaPorUsuarioOfisis(int usuarioId) {
            return _salaDal.ListadoSalaPorUsuarioOfisis(usuarioId);
        }
        public bool InsertarSalaJson(SalaEntidad sala) {
            return _salaDal.InsertarSalaJson(sala);
        }
        public List<SalaEntidad> ListadoTodosSala() {
            List<SalaEntidad> salas = _salaDal.ListadoTodosSala();
            List<SalaEntidad> salasActivas = _salaDal.ListadoSala();

            IEnumerable<IGrouping<int, SalaEntidad>> salasGrouping = salas.GroupBy(x => x.CodSalaMaestra);
            foreach(IGrouping<int, SalaEntidad> salaGrouping in salasGrouping) {
                DateTime ultimaFechaDeGrupo = salaGrouping
                    .Where(x => x.Estado == 1)
                    .OrderByDescending(x => x.FechaRegistro)
                    .Select(x => x.FechaRegistro)
                    .FirstOrDefault();

                SalaEntidad salaActiva = salasActivas
                    .FirstOrDefault(x => x.FechaRegistro == ultimaFechaDeGrupo && x.CodSalaMaestra == salaGrouping.Key);

                if(salaActiva != null) {
                    var salaPrincipal = salas.FirstOrDefault(x => x.CodSala == salaActiva.CodSala);
                    if(salaPrincipal != null) {
                        salaPrincipal.EsPrincipal = true;
                    }
                }
            }

            return salas;
        }
        public List<int> ListadoIdSala(int codUsuario) {
            return _salaDal.ListadoIdSala(codUsuario);
        }

        public List<SalaEntidad> ListadoTodosSalaActivosOrderJson() {
            return _salaDal.ListadoTodosSalaActivosOrder();
        }

        public bool SalaModificarEstadoJson(int SalaId, int Estado) {
            return _salaDal.SalaModificarEstadoJson(SalaId, Estado);
        }
        public int GetTotalSalasActivas() {
            return _salaDal.GetTotalSalasActivas();
        }
        public List<SalaEntidad> GetListadoSalasYTotalClientes() {
            return _salaDal.GetListadoSalasYTotalClientes();
        }

        public SalaEntidad ObtenerSalaEmpresa(int salaId) {
            return _salaDal.ObtenerSalaEmpresa(salaId);
        }
        public int GetTotalSalas() {
            return _salaDal.GetTotalSalas();
        }


        public bool SalaModificarCamposProgresivoJson(int salaId, string nameQuery, string value) {
            return _salaDal.SalaModificarCamposProgresivoJson(salaId, nameQuery, value);
        }
        public List<SalaEntidad> ListadoCamposProgresivoSalas() {
            return _salaDal.ListadoCamposProgresivoSalas();
        }
        public List<SalaEntidad.PingIpPublica> ListadoIpPublicaSalas() {
            return _salaDal.ListadoIpPublicaSalas();
        }
        public List<SalaEntidad.PingIpPrivada> ListadoIpPrivadaSalas() {
            return _salaDal.ListadoIpPrivadaSalas();
        }
        public List<SalaEntidad> ListadoIpsSalas() {
            return _salaDal.ListadoIpsSalas();
        }

        public SalaEntidad ObtenerSalaPorCodigo(int roomCode) {
            return _salaDal.ObtenerSalaPorCodigo(roomCode);
        }

        public SalaVpnEntidad ObtenerSalaVpnPorCodigo(int roomCode) {
            return _salaDal.ObtenerSalaVpnPorCodigo(roomCode);
        }

        public List<CorreoSala> ObtenerCorreosSala() {
            return _salaDal.ObtenerCorreosSala();

        }
        public CorreoSala ObtenerDetalleCorreosSala(int salaId) {
            return _salaDal.ObtenerDetalleCorreosSala(salaId);

        }
        public bool ActualizarCorreoSala(CorreoSala data) {
            return _salaDal.ActualizarCorreoSala(data);

        }

        #region Sala Maestra
        public List<SalaEntidad> ObtenerTodasLasSalasDeSalaMaestraPorCodigoSalaMaestra(int codSalaMaestra) {
            return _salaDal.ObtenerTodasLasSalasDeSalaMaestraPorCodigoSalaMaestra(codSalaMaestra);
        }

        public List<SalaEntidad> ListadoSalaMaestraPorUsuario(int usuarioId) {
            return _salaDal.ListadoSalaMaestraPorUsuario(usuarioId);
        }
        #endregion

        public List<int> ObtenerCodsSalasDeSesion(HttpSessionStateBase session) {
            int usuarioId = Convert.ToInt32(session["UsuarioID"]);
            List<SalaEntidad> listaSalas = ListadoSalaPorUsuario(usuarioId);
            return listaSalas.Count > 0 ? listaSalas.Select(x => x.CodSala).ToList() : new List<int> { -1 };
        }
    }
}
