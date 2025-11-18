using CapaEntidad;
using CapaEntidad.MaquinasInoperativas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_MaquinaInoperativaDAL {

        string conexion = string.Empty;
        public MI_MaquinaInoperativaDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        //ESTADOS
        //1 CREADO
        //2 ATENDIDA OPERATIVA
        //3 ATENDIDA INOPERATIVA
        //4 ATENDIDA INOPERATIVA SOLICITUD
        //5 ATENDIDA INOPERATIVA APROBADO
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaxUsuario(int codUsuario) {
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativa]
                                  ,maq.[CodSala]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaLinea]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaFicha]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[MaquinaToken]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodUsuarioCreado]
                                  ,maq.[CodUsuarioAtendidaOperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativaSolicitado]
                                  ,maq.[CodUsuarioAtendidaInoperativaAprobado]
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[EstadoReparacion]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,sal.Nombre as NombreSala
                                  ,usu.UsuarioNombre as NombreUsuarioCreado
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              INNER JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioCreado 
                              INNER JOIN UsuarioSala usa ON usa.SalaId = maq.CodSala 
                              WHERE usa.UsuarioId=@pCodUsuario
                              ORDER BY maq.CodMaquinaInoperativa DESC";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaEntidad {
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]),
                                MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]),
                                MaquinaLinea = ManejoNulos.ManageNullStr(dr["MaquinaLinea"]),
                                MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]),
                                MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]),
                                MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]),
                                MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]),
                                MaquinaFicha = ManejoNulos.ManageNullStr(dr["MaquinaFicha"]),
                                MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]),
                                MaquinaToken = ManejoNulos.ManageNullStr(dr["MaquinaToken"]),
                                TecnicoCreado = ManejoNulos.ManageNullStr(dr["TecnicoCreado"]),
                                TecnicoAtencion = ManejoNulos.ManageNullStr(dr["TecnicoAtencion"]),
                                ObservacionCreado = ManejoNulos.ManageNullStr(dr["ObservacionCreado"]),
                                ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]),
                                CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]),
                                CodPrioridad = ManejoNulos.ManageNullInteger(dr["CodPrioridad"]),
                                FechaInoperativa = ManejoNulos.ManageNullDate(dr["FechaInoperativa"]),
                                FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]),
                                FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]),
                                FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]),
                                FechaAtendidaInoperativaSolicitado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaSolicitado"]),
                                FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]),
                                CodUsuarioCreado = ManejoNulos.ManageNullInteger(dr["CodUsuarioCreado"]),
                                CodUsuarioAtendidaOperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaOperativa"]),
                                CodUsuarioAtendidaInoperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativa"]),
                                CodUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaSolicitado"]),
                                CodUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaAprobado"]),
                                CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreUsuarioCreado = ManejoNulos.ManageNullStr(dr["NombreUsuarioCreado"]),
                                CodEstadoReparacion = ManejoNulos.ManageNullInteger(dr["EstadoReparacion"]),
                                OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]),
                                FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]),
                                //NombreUsuarioAtendidaOperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaOperativa"]),
                                //NombreUsuarioAtendidaInoperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativa"]),
                                //NombreUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaSolicitado"]),
                                //NombreUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaAprobado"]),
                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaxUsuarioxFechas(int codUsuario, DateTime fechaIni, DateTime fechaFin) {
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativa]
                                  ,maq.[CodSala]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaLinea]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaFicha]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[MaquinaToken]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodUsuarioCreado]
                                  ,maq.[CodUsuarioAtendidaOperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativaSolicitado]
                                  ,maq.[CodUsuarioAtendidaInoperativaAprobado]
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[EstadoReparacion]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,sal.Nombre as NombreSala
                                  ,usu.UsuarioNombre as NombreUsuarioCreado
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              INNER JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioCreado 
                              INNER JOIN UsuarioSala usa ON usa.SalaId = maq.CodSala 
                              WHERE usa.UsuarioId=@pCodUsuario AND CONVERT(date, maq.FechaCreado) between @pFechaIni and @pFechaFin
                              ORDER BY maq.CodMaquinaInoperativa DESC";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);
                    query.Parameters.AddWithValue("@pFechaIni", fechaIni);
                    query.Parameters.AddWithValue("@pFechaFin", fechaFin);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaEntidad {
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]),
                                MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]),
                                MaquinaLinea = ManejoNulos.ManageNullStr(dr["MaquinaLinea"]),
                                MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]),
                                MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]),
                                MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]),
                                MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]),
                                MaquinaFicha = ManejoNulos.ManageNullStr(dr["MaquinaFicha"]),
                                MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]),
                                MaquinaToken = ManejoNulos.ManageNullStr(dr["MaquinaToken"]),
                                TecnicoCreado = ManejoNulos.ManageNullStr(dr["TecnicoCreado"]),
                                TecnicoAtencion = ManejoNulos.ManageNullStr(dr["TecnicoAtencion"]),
                                ObservacionCreado = ManejoNulos.ManageNullStr(dr["ObservacionCreado"]),
                                ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]),
                                CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]),
                                CodEstadoReparacion = ManejoNulos.ManageNullInteger(dr["EstadoReparacion"]),
                                CodPrioridad = ManejoNulos.ManageNullInteger(dr["CodPrioridad"]),
                                FechaInoperativa = ManejoNulos.ManageNullDate(dr["FechaInoperativa"]),
                                FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]),
                                FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]),
                                FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]),
                                FechaAtendidaInoperativaSolicitado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaSolicitado"]),
                                FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]),
                                CodUsuarioCreado = ManejoNulos.ManageNullInteger(dr["CodUsuarioCreado"]),
                                CodUsuarioAtendidaOperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaOperativa"]),
                                CodUsuarioAtendidaInoperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativa"]),
                                CodUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaSolicitado"]),
                                CodUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaAprobado"]),
                                CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreUsuarioCreado = ManejoNulos.ManageNullStr(dr["NombreUsuarioCreado"]),
                                OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]),
                                FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]),
                                //NombreUsuarioAtendidaOperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaOperativa"]),
                                //NombreUsuarioAtendidaInoperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativa"]),
                                //NombreUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaSolicitado"]),
                                //NombreUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaAprobado"]),
                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaxUsuarioxFechasxEstado(int codUsuario, DateTime fechaIni, DateTime fechaFin, int estado) {
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativa]
                                  ,maq.[CodSala]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaLinea]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaFicha]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[MaquinaToken]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodUsuarioCreado]
                                  ,maq.[CodUsuarioAtendidaOperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativaSolicitado]
                                  ,maq.[CodUsuarioAtendidaInoperativaAprobado]
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[EstadoReparacion]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,sal.Nombre as NombreSala
                                  ,usu.UsuarioNombre as NombreUsuarioCreado
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              INNER JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioCreado 
                              INNER JOIN UsuarioSala usa ON usa.SalaId = maq.CodSala 
                              WHERE usa.UsuarioId=@pCodUsuario AND CONVERT(date, maq.FechaCreado) between @pFechaIni and @pFechaFin and maq.CodEstadoProceso = @pEstado
                              ORDER BY maq.CodMaquinaInoperativa DESC";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);
                    query.Parameters.AddWithValue("@pFechaIni", fechaIni);
                    query.Parameters.AddWithValue("@pFechaFin", fechaFin);
                    query.Parameters.AddWithValue("@pEstado", estado);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaEntidad {
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]),
                                MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]),
                                MaquinaLinea = ManejoNulos.ManageNullStr(dr["MaquinaLinea"]),
                                MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]),
                                MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]),
                                MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]),
                                MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]),
                                MaquinaFicha = ManejoNulos.ManageNullStr(dr["MaquinaFicha"]),
                                MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]),
                                MaquinaToken = ManejoNulos.ManageNullStr(dr["MaquinaToken"]),
                                TecnicoCreado = ManejoNulos.ManageNullStr(dr["TecnicoCreado"]),
                                TecnicoAtencion = ManejoNulos.ManageNullStr(dr["TecnicoAtencion"]),
                                ObservacionCreado = ManejoNulos.ManageNullStr(dr["ObservacionCreado"]),
                                ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]),
                                CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]),
                                CodEstadoReparacion = ManejoNulos.ManageNullInteger(dr["EstadoReparacion"]),
                                CodPrioridad = ManejoNulos.ManageNullInteger(dr["CodPrioridad"]),
                                FechaInoperativa = ManejoNulos.ManageNullDate(dr["FechaInoperativa"]),
                                FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]),
                                FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]),
                                FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]),
                                FechaAtendidaInoperativaSolicitado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaSolicitado"]),
                                FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]),
                                CodUsuarioCreado = ManejoNulos.ManageNullInteger(dr["CodUsuarioCreado"]),
                                CodUsuarioAtendidaOperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaOperativa"]),
                                CodUsuarioAtendidaInoperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativa"]),
                                CodUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaSolicitado"]),
                                CodUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaAprobado"]),
                                CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreUsuarioCreado = ManejoNulos.ManageNullStr(dr["NombreUsuarioCreado"]),
                                OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]),
                                FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]),
                                //NombreUsuarioAtendidaOperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaOperativa"]),
                                //NombreUsuarioAtendidaInoperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativa"]),
                                //NombreUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaSolicitado"]),
                                //NombreUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaAprobado"]),
                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaCreado(int codUsuario) {
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativa]
                                  ,maq.[CodSala]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaLinea]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaFicha]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[MaquinaToken]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodUsuarioCreado]
                                  ,maq.[CodUsuarioAtendidaOperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativaSolicitado]
                                  ,maq.[CodUsuarioAtendidaInoperativaAprobado]
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,maq.[EstadoReparacion]
                                  ,sal.Nombre as NombreSala
                                  ,usu.UsuarioNombre as NombreUsuarioCreado
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              INNER JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioCreado 
                              INNER JOIN UsuarioSala usa ON usa.SalaId = maq.CodSala 
                              WHERE usa.UsuarioId=@pCodUsuario AND CodEstadoProceso IN (1,5)
                              ORDER BY maq.CodMaquinaInoperativa DESC";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaEntidad {
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]),
                                MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]),
                                MaquinaLinea = ManejoNulos.ManageNullStr(dr["MaquinaLinea"]),
                                MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]),
                                MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]),
                                MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]),
                                MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]),
                                MaquinaFicha = ManejoNulos.ManageNullStr(dr["MaquinaFicha"]),
                                MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]),
                                MaquinaToken = ManejoNulos.ManageNullStr(dr["MaquinaToken"]),
                                TecnicoCreado = ManejoNulos.ManageNullStr(dr["TecnicoCreado"]),
                                TecnicoAtencion = ManejoNulos.ManageNullStr(dr["TecnicoAtencion"]),
                                ObservacionCreado = ManejoNulos.ManageNullStr(dr["ObservacionCreado"]),
                                ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]),
                                CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]),
                                CodPrioridad = ManejoNulos.ManageNullInteger(dr["CodPrioridad"]),
                                FechaInoperativa = ManejoNulos.ManageNullDate(dr["FechaInoperativa"]),
                                FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]),
                                FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]),
                                FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]),
                                FechaAtendidaInoperativaSolicitado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaSolicitado"]),
                                FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]),
                                CodUsuarioCreado = ManejoNulos.ManageNullInteger(dr["CodUsuarioCreado"]),
                                CodUsuarioAtendidaOperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaOperativa"]),
                                CodUsuarioAtendidaInoperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativa"]),
                                CodUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaSolicitado"]),
                                CodUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaAprobado"]),
                                CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreUsuarioCreado = ManejoNulos.ManageNullStr(dr["NombreUsuarioCreado"]),
                                OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]),
                                FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]),
                                CodEstadoReparacion = ManejoNulos.ManageNullInteger(dr["EstadoReparacion"]),
                                //NombreUsuarioAtendidaOperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaOperativa"]),
                                //NombreUsuarioAtendidaInoperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativa"]),
                                //NombreUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaSolicitado"]),
                                //NombreUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaAprobado"]),
                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaAtendidaOperativa(int codUsuario) {
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativa]
                                  ,maq.[CodSala]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaLinea]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaFicha]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[MaquinaToken]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodUsuarioCreado]
                                  ,maq.[CodUsuarioAtendidaOperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativaSolicitado]
                                  ,maq.[CodUsuarioAtendidaInoperativaAprobado]
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,maq.[EstadoReparacion]
                                  ,sal.Nombre as NombreSala
                                  ,usu.UsuarioNombre as NombreUsuarioCreado
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              INNER JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioCreado 
                              INNER JOIN UsuarioSala usa ON usa.SalaId = maq.CodSala 
                              WHERE usa.UsuarioId=@pCodUsuario AND CodEstadoProceso=2
                              ORDER BY maq.CodMaquinaInoperativa DESC";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaEntidad {
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]),
                                MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]),
                                MaquinaLinea = ManejoNulos.ManageNullStr(dr["MaquinaLinea"]),
                                MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]),
                                MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]),
                                MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]),
                                MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]),
                                MaquinaFicha = ManejoNulos.ManageNullStr(dr["MaquinaFicha"]),
                                MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]),
                                MaquinaToken = ManejoNulos.ManageNullStr(dr["MaquinaToken"]),
                                TecnicoCreado = ManejoNulos.ManageNullStr(dr["TecnicoCreado"]),
                                TecnicoAtencion = ManejoNulos.ManageNullStr(dr["TecnicoAtencion"]),
                                ObservacionCreado = ManejoNulos.ManageNullStr(dr["ObservacionCreado"]),
                                ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]),
                                CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]),
                                CodPrioridad = ManejoNulos.ManageNullInteger(dr["CodPrioridad"]),
                                FechaInoperativa = ManejoNulos.ManageNullDate(dr["FechaInoperativa"]),
                                FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]),
                                FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]),
                                FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]),
                                FechaAtendidaInoperativaSolicitado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaSolicitado"]),
                                FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]),
                                CodUsuarioCreado = ManejoNulos.ManageNullInteger(dr["CodUsuarioCreado"]),
                                CodUsuarioAtendidaOperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaOperativa"]),
                                CodUsuarioAtendidaInoperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativa"]),
                                CodUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaSolicitado"]),
                                CodUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaAprobado"]),
                                CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreUsuarioCreado = ManejoNulos.ManageNullStr(dr["NombreUsuarioCreado"]),
                                OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]),
                                FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]),
                                CodEstadoReparacion = ManejoNulos.ManageNullInteger(dr["EstadoReparacion"]),
                                //NombreUsuarioAtendidaOperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaOperativa"]),
                                //NombreUsuarioAtendidaInoperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativa"]),
                                //NombreUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaSolicitado"]),
                                //NombreUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaAprobado"]),
                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaAtendidaInoperativa(int codUsuario) {
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativa]
                                  ,maq.[CodSala]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaLinea]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaFicha]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[MaquinaToken]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodUsuarioCreado]
                                  ,maq.[CodUsuarioAtendidaOperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativaSolicitado]
                                  ,maq.[CodUsuarioAtendidaInoperativaAprobado]
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,maq.[EstadoReparacion]
                                  ,sal.Nombre as NombreSala
                                  ,usu.UsuarioNombre as NombreUsuarioAtendidaInoperativa
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              INNER JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioAtendidaInoperativa 
                              INNER JOIN UsuarioSala usa ON usa.SalaId = maq.CodSala 
                              WHERE usa.UsuarioId=@pCodUsuario AND CodEstadoProceso=3
                              ORDER BY maq.CodMaquinaInoperativa DESC";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaEntidad {
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]),
                                MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]),
                                MaquinaLinea = ManejoNulos.ManageNullStr(dr["MaquinaLinea"]),
                                MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]),
                                MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]),
                                MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]),
                                MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]),
                                MaquinaFicha = ManejoNulos.ManageNullStr(dr["MaquinaFicha"]),
                                MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]),
                                MaquinaToken = ManejoNulos.ManageNullStr(dr["MaquinaToken"]),
                                TecnicoCreado = ManejoNulos.ManageNullStr(dr["TecnicoCreado"]),
                                TecnicoAtencion = ManejoNulos.ManageNullStr(dr["TecnicoAtencion"]),
                                ObservacionCreado = ManejoNulos.ManageNullStr(dr["ObservacionCreado"]),
                                ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]),
                                CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]),
                                CodPrioridad = ManejoNulos.ManageNullInteger(dr["CodPrioridad"]),
                                FechaInoperativa = ManejoNulos.ManageNullDate(dr["FechaInoperativa"]),
                                FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]),
                                FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]),
                                FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]),
                                FechaAtendidaInoperativaSolicitado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaSolicitado"]),
                                FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]),
                                CodUsuarioCreado = ManejoNulos.ManageNullInteger(dr["CodUsuarioCreado"]),
                                CodUsuarioAtendidaOperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaOperativa"]),
                                CodUsuarioAtendidaInoperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativa"]),
                                CodUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaSolicitado"]),
                                CodUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaAprobado"]),
                                CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]),
                                FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]),
                                CodEstadoReparacion = ManejoNulos.ManageNullInteger(dr["EstadoReparacion"]),
                                //NombreUsuarioCreado = ManejoNulos.ManageNullStr(dr["NombreUsuarioCreado"]),
                                //NombreUsuarioAtendidaOperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaOperativa"]),
                                NombreUsuarioAtendidaInoperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativa"]),
                                //NombreUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaSolicitado"]),
                                //NombreUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaAprobado"]),
                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaAtendidaInoperativaSolicitud(int codUsuario) {
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            string consulta = @" SELECT maq.[CodMaquinaInoperativa]
                                  ,maq.[CodSala]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaLinea]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaFicha]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[MaquinaToken]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodUsuarioCreado]
                                  ,maq.[CodUsuarioAtendidaOperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativaSolicitado]
                                  ,maq.[CodUsuarioAtendidaInoperativaAprobado]
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,maq.[EstadoReparacion]
                                  ,sal.Nombre as NombreSala
                                  ,usu.UsuarioNombre as NombreUsuarioCreado
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              INNER JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioCreado 
                              INNER JOIN UsuarioSala usa ON usa.SalaId = maq.CodSala 
                              WHERE usa.UsuarioId=@pCodUsuario AND CodEstadoProceso=4
                              ORDER BY maq.CodMaquinaInoperativa DESC";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaEntidad {
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]),
                                MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]),
                                MaquinaLinea = ManejoNulos.ManageNullStr(dr["MaquinaLinea"]),
                                MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]),
                                MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]),
                                MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]),
                                MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]),
                                MaquinaFicha = ManejoNulos.ManageNullStr(dr["MaquinaFicha"]),
                                MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]),
                                MaquinaToken = ManejoNulos.ManageNullStr(dr["MaquinaToken"]),
                                TecnicoCreado = ManejoNulos.ManageNullStr(dr["TecnicoCreado"]),
                                TecnicoAtencion = ManejoNulos.ManageNullStr(dr["TecnicoAtencion"]),
                                ObservacionCreado = ManejoNulos.ManageNullStr(dr["ObservacionCreado"]),
                                ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]),
                                CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]),
                                CodPrioridad = ManejoNulos.ManageNullInteger(dr["CodPrioridad"]),
                                FechaInoperativa = ManejoNulos.ManageNullDate(dr["FechaInoperativa"]),
                                FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]),
                                FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]),
                                FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]),
                                FechaAtendidaInoperativaSolicitado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaSolicitado"]),
                                FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]),
                                CodUsuarioCreado = ManejoNulos.ManageNullInteger(dr["CodUsuarioCreado"]),
                                CodUsuarioAtendidaOperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaOperativa"]),
                                CodUsuarioAtendidaInoperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativa"]),
                                CodUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaSolicitado"]),
                                CodUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaAprobado"]),
                                CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreUsuarioCreado = ManejoNulos.ManageNullStr(dr["NombreUsuarioCreado"]),
                                OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]),
                                FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]),
                                CodEstadoReparacion = ManejoNulos.ManageNullInteger(dr["EstadoReparacion"]),
                                //NombreUsuarioAtendidaOperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaOperativa"]),
                                //NombreUsuarioAtendidaInoperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativa"]),
                                //NombreUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaSolicitado"]),
                                //NombreUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaAprobado"]),
                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public MI_MaquinaInoperativaEntidad GetCodMaquinaInoperativa(int codMaquinaInoperativa) {
            MI_MaquinaInoperativaEntidad item = new MI_MaquinaInoperativaEntidad();
            string consulta = @" SELECT maq.[CodMaquinaInoperativa]
                                  ,maq.[CodSala]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaLinea]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaFicha]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[MaquinaToken]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodUsuarioCreado]
                                  ,maq.[CodUsuarioAtendidaOperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativaSolicitado]
                                  ,maq.[CodUsuarioAtendidaInoperativaAprobado]
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[IST]
                                  ,maq.[EstadoReparacion]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,sal.Nombre as NombreSala
                                  ,usu.UsuarioNombre as NombreUsuarioCreado
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              INNER JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioCreado 
                              WHERE maq.CodMaquinaInoperativa=@pCodMaquinaInoperativa";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", codMaquinaInoperativa);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]);
                                item.MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]);
                                item.MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]);
                                item.MaquinaLinea = ManejoNulos.ManageNullStr(dr["MaquinaLinea"]);
                                item.MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]);
                                item.MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]);
                                item.MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]);
                                item.MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]);
                                item.MaquinaFicha = ManejoNulos.ManageNullStr(dr["MaquinaFicha"]);
                                item.MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]);
                                item.MaquinaToken = ManejoNulos.ManageNullStr(dr["MaquinaToken"]);
                                item.TecnicoCreado = ManejoNulos.ManageNullStr(dr["TecnicoCreado"]);
                                item.TecnicoAtencion = ManejoNulos.ManageNullStr(dr["TecnicoAtencion"]);
                                item.ObservacionCreado = ManejoNulos.ManageNullStr(dr["ObservacionCreado"]);
                                item.ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]);
                                item.CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]);
                                item.CodPrioridad = ManejoNulos.ManageNullInteger(dr["CodPrioridad"]);
                                item.FechaInoperativa = ManejoNulos.ManageNullDate(dr["FechaInoperativa"]);
                                item.FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]);
                                item.FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]);
                                item.FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]);
                                item.FechaAtendidaInoperativaSolicitado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaSolicitado"]);
                                item.FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]);
                                item.CodUsuarioCreado = ManejoNulos.ManageNullInteger(dr["CodUsuarioCreado"]);
                                item.CodUsuarioAtendidaOperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaOperativa"]);
                                item.CodUsuarioAtendidaInoperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativa"]);
                                item.CodUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaSolicitado"]);
                                item.CodUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaAprobado"]);
                                item.CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]);
                                item.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                                item.NombreUsuarioCreado = ManejoNulos.ManageNullStr(dr["NombreUsuarioCreado"]);
                                item.IST = ManejoNulos.ManageNullStr(dr["IST"]);
                                item.CodEstadoReparacion = ManejoNulos.ManageNullInteger(dr["EstadoReparacion"]);
                                item.OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]);
                                item.FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public MI_MaquinaInoperativaEntidad GetCodMaquinaInoperativaHistorico(int codMaquinaInoperativa) {
            MI_MaquinaInoperativaEntidad item = new MI_MaquinaInoperativaEntidad();
            string consulta = @" SELECT maq.[CodMaquinaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[CodUsuarioCreado]
                                  ,usu.UsuarioNombre as NombreUsuarioCreado
                                  ,maq.[CodSala]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaLinea]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaFicha]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[MaquinaToken]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[EstadoReparacion]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativa]
                                  ,maq.[IST]
                                  ,maq.[ObservacionAtencionNuevo]
                                  ,usu2.UsuarioNombre as NombreUsuarioAtendidaInoperativa
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[CodUsuarioAtendidaOperativa]
                                  ,usu3.UsuarioNombre as NombreUsuarioAtendidaOperativa
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[CodUsuarioAtendidaInoperativaSolicitado]
                                  ,usu4.UsuarioNombre as NombreUsuarioAtendidaInoperativaSolicitado
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodUsuarioAtendidaInoperativaAprobado]
                                  ,usu5.UsuarioNombre as NombreUsuarioAtendidaInoperativaAprobado
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,sal.Nombre as NombreSala
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              LEFT JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioCreado 
                              LEFT JOIN SEG_Usuario usu2 ON usu2.UsuarioID = maq.CodUsuarioAtendidaInoperativa
                              LEFT JOIN SEG_Usuario usu3 ON usu3.UsuarioID = maq.CodUsuarioAtendidaOperativa
                              LEFT JOIN SEG_Usuario usu4 ON usu4.UsuarioID = maq.CodUsuarioAtendidaInoperativaSolicitado
                              LEFT JOIN SEG_Usuario usu5 ON usu5.UsuarioID = maq.CodUsuarioAtendidaInoperativaAprobado
                              WHERE maq.CodMaquinaInoperativa=@pCodMaquinaInoperativa";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", codMaquinaInoperativa);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]);
                                item.FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]);
                                item.CodUsuarioCreado = ManejoNulos.ManageNullInteger(dr["CodUsuarioCreado"]);
                                item.NombreUsuarioCreado = ManejoNulos.ManageNullStr(dr["NombreUsuarioCreado"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]);
                                item.MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]);
                                item.MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]);
                                item.MaquinaLinea = ManejoNulos.ManageNullStr(dr["MaquinaLinea"]);
                                item.MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]);
                                item.MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]);
                                item.MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]);
                                item.MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]);
                                item.MaquinaFicha = ManejoNulos.ManageNullStr(dr["MaquinaFicha"]);
                                item.MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]);
                                item.MaquinaToken = ManejoNulos.ManageNullStr(dr["MaquinaToken"]);
                                item.MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]);
                                item.CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]);
                                item.CodEstadoReparacion = ManejoNulos.ManageNullInteger(dr["EstadoReparacion"]);
                                item.CodPrioridad = ManejoNulos.ManageNullInteger(dr["CodPrioridad"]);
                                item.FechaInoperativa = ManejoNulos.ManageNullDate(dr["FechaInoperativa"]);
                                item.TecnicoCreado = ManejoNulos.ManageNullStr(dr["TecnicoCreado"]);
                                item.ObservacionCreado = ManejoNulos.ManageNullStr(dr["ObservacionCreado"]);
                                item.FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]);
                                item.CodUsuarioAtendidaInoperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativa"]);
                                item.NombreUsuarioAtendidaInoperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativa"]);
                                item.TecnicoAtencion = ManejoNulos.ManageNullStr(dr["TecnicoAtencion"]);
                                item.ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]);
                                item.FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]);
                                item.CodUsuarioAtendidaOperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaOperativa"]);
                                item.NombreUsuarioAtendidaOperativa = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaOperativa"]);
                                item.FechaAtendidaInoperativaSolicitado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaSolicitado"]);
                                item.CodUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaSolicitado"]);
                                item.NombreUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaSolicitado"]);
                                item.FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]);
                                item.CodUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaAprobado"]);
                                item.NombreUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaAprobado"]);
                                item.CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]);
                                item.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                                item.IST = ManejoNulos.ManageNullStr(dr["IST"]);
                                item.ObservacionAtencionNuevo = ManejoNulos.ManageNullStr(dr["ObservacionAtencionNuevo"]);
                                item.OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]);
                                item.FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]);


                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarMaquinaInoperativaCreado(MI_MaquinaInoperativaEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_MaquinaInoperativa]
           ([CodSala]
           ,[CodMaquina]
           ,[MaquinaLey]
           ,[MaquinaModelo]
           ,[MaquinaLinea]
           ,[MaquinaSala]
           ,[MaquinaJuego]
           ,[MaquinaNumeroSerie]
           ,[MaquinaPropietario]
           ,[MaquinaFicha]
           ,[MaquinaMarca]
           ,[MaquinaToken]
           ,[TecnicoCreado]
           ,[TecnicoAtencion]
           ,[ObservacionCreado]
           ,[ObservacionAtencion]
           ,[CodEstadoInoperativa]
           ,[CodPrioridad]
           ,[FechaInoperativa]
           ,[FechaCreado]
           ,[CodUsuarioCreado]
           ,[CodEstadoProceso]
           ,[NombreZona])
     OUTPUT Inserted.CodMaquinaInoperativa   
     VALUES
           (@pCodSala,
           @pCodMaquina,
           @pMaquinaLey,
           @pMaquinaModelo,
           @pMaquinaLinea,
           @pMaquinaSala,
           @pMaquinaJuego,
           @pMaquinaNumeroSerie,
           @pMaquinaPropietario,
           @pMaquinaFicha,
           @pMaquinaMarca,
           @pMaquinaToken,
           @pTecnicoCreado,
           @pTecnicoAtencion,
           @pObservacionCreado,
           @pObservacionAtencion,
           @pCodEstadoInoperativa,
           @pCodPrioridad,
           @pFechaInoperativa,
           @pFechaCreado,
           @pCodUsuarioCreado,
           @pCodEstadoProceso,
           @pNombreZona)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.CodSala));
                    query.Parameters.AddWithValue("@pCodMaquina", ManejoNulos.ManageNullInteger(Entidad.CodMaquina));
                    query.Parameters.AddWithValue("@pMaquinaLey", ManejoNulos.ManageNullStr(Entidad.MaquinaLey));
                    query.Parameters.AddWithValue("@pMaquinaModelo", ManejoNulos.ManageNullStr(Entidad.MaquinaModelo));
                    query.Parameters.AddWithValue("@pMaquinaLinea", ManejoNulos.ManageNullStr(Entidad.MaquinaLinea));
                    query.Parameters.AddWithValue("@pMaquinaSala", ManejoNulos.ManageNullStr(Entidad.MaquinaSala));
                    query.Parameters.AddWithValue("@pMaquinaJuego", ManejoNulos.ManageNullStr(Entidad.MaquinaJuego));
                    query.Parameters.AddWithValue("@pMaquinaNumeroSerie", ManejoNulos.ManageNullStr(Entidad.MaquinaNumeroSerie));
                    query.Parameters.AddWithValue("@pMaquinaPropietario", ManejoNulos.ManageNullStr(Entidad.MaquinaPropietario));
                    query.Parameters.AddWithValue("@pMaquinaFicha", ManejoNulos.ManageNullStr(Entidad.MaquinaFicha));
                    query.Parameters.AddWithValue("@pMaquinaMarca", ManejoNulos.ManageNullStr(Entidad.MaquinaMarca));
                    query.Parameters.AddWithValue("@pMaquinaToken", ManejoNulos.ManageNullStr(Entidad.MaquinaToken));
                    query.Parameters.AddWithValue("@pTecnicoCreado", ManejoNulos.ManageNullStr(Entidad.TecnicoCreado));
                    query.Parameters.AddWithValue("@pTecnicoAtencion", ManejoNulos.ManageNullStr(Entidad.TecnicoAtencion));
                    query.Parameters.AddWithValue("@pObservacionCreado", ManejoNulos.ManageNullStr(Entidad.ObservacionCreado));
                    query.Parameters.AddWithValue("@pObservacionAtencion", ManejoNulos.ManageNullStr(Entidad.ObservacionAtencion));
                    query.Parameters.AddWithValue("@pCodEstadoInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodEstadoInoperativa));
                    query.Parameters.AddWithValue("@pCodPrioridad", ManejoNulos.ManageNullInteger(Entidad.CodPrioridad));
                    query.Parameters.AddWithValue("@pFechaInoperativa", ManejoNulos.ManageNullDate(Entidad.FechaInoperativa));
                    query.Parameters.AddWithValue("@pFechaCreado", ManejoNulos.ManageNullDate(Entidad.FechaCreado));
                    query.Parameters.AddWithValue("@pCodUsuarioCreado", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioCreado));
                    query.Parameters.AddWithValue("@pCodEstadoProceso", ManejoNulos.ManageNullInteger(Entidad.CodEstadoProceso));
                    query.Parameters.AddWithValue("@pNombreZona", ManejoNulos.ManageNullStr(Entidad.NombreZona));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool AtenderMaquinaInoperativaOperativa(MI_MaquinaInoperativaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [TecnicoAtencion] = @pTecnicoAtencion
                                  ,[FechaAtendidaOperativa] = @pFechaAtendidaOperativa
                                  ,[CodUsuarioAtendidaOperativa] = @pCodUsuarioAtendidaOperativa
                                  ,[CodEstadoProceso] = @pCodEstadoProceso
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTecnicoAtencion", ManejoNulos.ManageNullStr(Entidad.TecnicoAtencion));
                    query.Parameters.AddWithValue("@pObservacionAtencion", ManejoNulos.ManageNullStr(Entidad.ObservacionAtencion));
                    query.Parameters.AddWithValue("@pFechaAtendidaOperativa", ManejoNulos.ManageNullDate(Entidad.FechaAtendidaOperativa));
                    query.Parameters.AddWithValue("@pCodUsuarioAtendidaOperativa", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioAtendidaOperativa));
                    query.Parameters.AddWithValue("@pCodEstadoProceso", ManejoNulos.ManageNullInteger(Entidad.CodEstadoProceso));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool AtenderMaquinaInoperativaReparacion(MI_MaquinaInoperativaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [ObservacionAtencionNuevo] = @pObservacionAtencionNuevo
                                  ,[FechaAtendidaInoperativaAprobado] = @pFechaAtendidaInoperativaAprobado
                                  ,[CodUsuarioAtendidaInoperativaAprobado] = @pCodUsuarioAtendidaInoperativaAprobado
                                  ,[CodEstadoProceso] = @pCodEstadoProceso
                                  ,[EstadoReparacion] = @pCodEstadoReparacion
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pObservacionAtencionNuevo", ManejoNulos.ManageNullStr(Entidad.ObservacionAtencionNuevo));
                    query.Parameters.AddWithValue("@pFechaAtendidaInoperativaAprobado", ManejoNulos.ManageNullDate(Entidad.FechaAtendidaInoperativaAprobado));
                    query.Parameters.AddWithValue("@pCodUsuarioAtendidaInoperativaAprobado", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioAtendidaInoperativaAprobado));
                    query.Parameters.AddWithValue("@pCodEstadoProceso", ManejoNulos.ManageNullInteger(Entidad.CodEstadoProceso));
                    query.Parameters.AddWithValue("@pCodEstadoReparacion", ManejoNulos.ManageNullInteger(Entidad.CodEstadoReparacion));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool AtenderMaquinaInoperativaOperativaResuelto(MI_MaquinaInoperativaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [TecnicoAtencion] = @pTecnicoAtencion
                                  ,[ObservacionAtencion] = @pObservacionAtencion
                                  ,[FechaAtendidaOperativa] = @pFechaAtendidaOperativa
                                  ,[CodEstadoProceso] = @pCodEstadoProceso
                                  ,[CodUsuarioAtendidaOperativa] = @pCodUsuarioAtendidaOperativa
                                  ,[IST] = @pIST
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTecnicoAtencion", ManejoNulos.ManageNullStr(Entidad.TecnicoAtencion));
                    query.Parameters.AddWithValue("@pObservacionAtencion", ManejoNulos.ManageNullStr(Entidad.ObservacionAtencion));
                    query.Parameters.AddWithValue("@pIST", ManejoNulos.ManageNullStr(Entidad.IST));
                    query.Parameters.AddWithValue("@pFechaAtendidaOperativa", ManejoNulos.ManageNullDate(Entidad.FechaAtendidaOperativa));
                    query.Parameters.AddWithValue("@pCodUsuarioAtendidaOperativa", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioAtendidaOperativa));
                    query.Parameters.AddWithValue("@pCodEstadoProceso", ManejoNulos.ManageNullInteger(Entidad.CodEstadoProceso));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool AtenderMaquinaInoperativaOperativaSinResolver(MI_MaquinaInoperativaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [TecnicoAtencion] = @pTecnicoAtencion
                                  ,[ObservacionAtencion] = @pObservacionAtencion
                                  ,[FechaAtendidaInoperativa] = @pFechaAtendidaInoperativa
                                  ,[CodUsuarioAtendidaInoperativa] = @pCodUsuarioAtendidaInoperativa
                                  ,[CodEstadoProceso] = @pCodEstadoProceso
                                  ,[IST] = @pIST
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTecnicoAtencion", ManejoNulos.ManageNullStr(Entidad.TecnicoAtencion));
                    query.Parameters.AddWithValue("@pObservacionAtencion", ManejoNulos.ManageNullStr(Entidad.ObservacionAtencion));
                    query.Parameters.AddWithValue("@pIST", ManejoNulos.ManageNullStr(Entidad.IST));
                    query.Parameters.AddWithValue("@pCodUsuarioAtendidaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioAtendidaInoperativa));
                    query.Parameters.AddWithValue("@pCodEstadoProceso", ManejoNulos.ManageNullInteger(Entidad.CodEstadoProceso));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pFechaAtendidaInoperativa", ManejoNulos.ManageNullDate(Entidad.FechaAtendidaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool AtenderMaquinaInoperativaInoperativa(MI_MaquinaInoperativaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [TecnicoAtencion] = @pTecnicoAtencion
                                  ,[ObservacionAtencion] = @pObservacionAtencion
                                  ,[FechaAtendidaInoperativa] = @pFechaAtendidaInoperativa
                                  ,[CodUsuarioAtendidaInoperativa] = @pCodUsuarioAtendidaInoperativa
                                  ,[CodEstadoProceso] = @pCodEstadoProceso
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTecnicoAtencion", ManejoNulos.ManageNullStr(Entidad.TecnicoAtencion));
                    query.Parameters.AddWithValue("@pObservacionAtencion", ManejoNulos.ManageNullStr(Entidad.ObservacionAtencion));
                    query.Parameters.AddWithValue("@pFechaAtendidaInoperativa", ManejoNulos.ManageNullDate(Entidad.FechaAtendidaInoperativa));
                    query.Parameters.AddWithValue("@pCodUsuarioAtendidaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioAtendidaInoperativa));
                    query.Parameters.AddWithValue("@pCodEstadoProceso", ManejoNulos.ManageNullInteger(Entidad.CodEstadoProceso));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool AtenderSolicitudMaquinaInoperativa(MI_MaquinaInoperativaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [FechaAtendidaInoperativaSolicitado] = @pFechaAtendidaInoperativaSolicitado
                                  ,[CodUsuarioAtendidaInoperativaSolicitado] = @pCodUsuarioAtendidaInoperativaSolicitado
                                  ,[FechaAtendidaInoperativaAprobado] = @pFechaAtendidaInoperativaAprobado
                                  ,[CodUsuarioAtendidaInoperativaAprobado] = @pCodUsuarioAtendidaInoperativaAprobado
                                  ,[CodEstadoProceso] = @pCodEstadoProceso
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pFechaAtendidaInoperativaSolicitado", ManejoNulos.ManageNullDate(Entidad.FechaAtendidaInoperativaSolicitado));
                    query.Parameters.AddWithValue("@pCodUsuarioAtendidaInoperativaSolicitado", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioAtendidaInoperativaSolicitado));
                    query.Parameters.AddWithValue("@pFechaAtendidaInoperativaAprobado", ManejoNulos.ManageNullDate(Entidad.FechaAtendidaInoperativaAprobado));
                    query.Parameters.AddWithValue("@pCodUsuarioAtendidaInoperativaAprobado", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioAtendidaInoperativaAprobado));
                    query.Parameters.AddWithValue("@pCodEstadoProceso", ManejoNulos.ManageNullInteger(Entidad.CodEstadoProceso));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool AprobarSolicitudMaquinaInoperativa(MI_MaquinaInoperativaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [FechaAtendidaInoperativaAprobado] = @pFechaAtendidaInoperativaAprobado
                                  ,[CodUsuarioAtendidaInoperativaAprobado] = @pCodUsuarioAtendidaInoperativaAprobado
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pFechaAtendidaInoperativaAprobado", ManejoNulos.ManageNullDate(Entidad.FechaAtendidaInoperativaAprobado));
                    query.Parameters.AddWithValue("@pCodUsuarioAtendidaInoperativaAprobado", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioAtendidaInoperativaAprobado));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool CambiarEstadoMaquinaInoperativa(int cod, int estado) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [Estado] = @pEstado
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(estado));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(cod));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool EliminarMaquinaInoperativa(int codMaquinaInoperativa) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_MaquinaInoperativa] 
                                WHERE CodMaquinaInoperativa  = @pCodMaquinaInoperativa";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(codMaquinaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }


        public bool AtencionPendienteEditarMaquinaInoperativa(int CodMquinaInoperativa) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [CodEstadoProceso] = 5
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(CodMquinaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool AtencionPendienteRechazadasEditarMaquinaInoperativa(int CodMquinaInoperativa) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [Estado] = 4
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(CodMquinaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public List<MI_MaquinaInoperativaEntidad> ReporteCategoriaProblemasListaJsonxFechas(DateTime fechaIni, DateTime fechaFin, string strElementos) {
            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();
            string consulta = @" SELECT DISTINCT maq.[CodMaquinaInoperativa]
                                  ,maq.[CodSala]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaLinea]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaJuego]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaFicha]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[MaquinaToken]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodUsuarioCreado]
                                  ,maq.[CodUsuarioAtendidaOperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativa]
                                  ,maq.[CodUsuarioAtendidaInoperativaSolicitado]
                                  ,maq.[CodUsuarioAtendidaInoperativaAprobado]
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,maq.[EstadoReparacion]
                                  ,sal.Nombre as NombreSala
                                  ,usu.UsuarioNombre as NombreUsuarioCreado
                                  ,pro.Nombre as NombreProblema
                                  ,mcp.Nombre as NombreCategoriaProblema
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              INNER JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioCreado 
                              INNER JOIN UsuarioSala usa ON usa.SalaId = maq.CodSala 
                              INNER JOIN MI_MaquinaInoperativaProblemas mip ON mip.CodMaquinaInoperativa = maq.CodMaquinaInoperativa 
                              INNER JOIN MI_Problema pro ON pro.CodProblema = mip.CodProblema 
                              INNER JOIN MI_CategoriaProblema mcp ON mcp.CodCategoriaProblema = pro.CodCategoriaProblema 
                              WHERE CONVERT(date, maq.FechaCreado) between @pFechaIni and @pFechaFin " + strElementos +
                              "ORDER BY maq.CodMaquinaInoperativa DESC";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pFechaIni", ManejoNulos.ManageNullDate(fechaIni));
                    query.Parameters.AddWithValue("@pFechaFin", ManejoNulos.ManageNullDate(fechaFin));

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaEntidad {
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]),
                                MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]),
                                MaquinaLinea = ManejoNulos.ManageNullStr(dr["MaquinaLinea"]),
                                MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]),
                                MaquinaJuego = ManejoNulos.ManageNullStr(dr["MaquinaJuego"]),
                                MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]),
                                MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]),
                                MaquinaFicha = ManejoNulos.ManageNullStr(dr["MaquinaFicha"]),
                                MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]),
                                MaquinaToken = ManejoNulos.ManageNullStr(dr["MaquinaToken"]),
                                TecnicoCreado = ManejoNulos.ManageNullStr(dr["TecnicoCreado"]),
                                TecnicoAtencion = ManejoNulos.ManageNullStr(dr["TecnicoAtencion"]),
                                ObservacionCreado = ManejoNulos.ManageNullStr(dr["ObservacionCreado"]),
                                ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]),
                                CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]),
                                CodPrioridad = ManejoNulos.ManageNullInteger(dr["CodPrioridad"]),
                                FechaInoperativa = ManejoNulos.ManageNullDate(dr["FechaInoperativa"]),
                                FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]),
                                FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]),
                                FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]),
                                FechaAtendidaInoperativaSolicitado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaSolicitado"]),
                                FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]),
                                CodUsuarioCreado = ManejoNulos.ManageNullInteger(dr["CodUsuarioCreado"]),
                                CodUsuarioAtendidaOperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaOperativa"]),
                                CodUsuarioAtendidaInoperativa = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativa"]),
                                CodUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaSolicitado"]),
                                CodUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullInteger(dr["CodUsuarioAtendidaInoperativaAprobado"]),
                                CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreUsuarioCreado = ManejoNulos.ManageNullStr(dr["NombreUsuarioCreado"]),
                                NombreProblema = ManejoNulos.ManageNullStr(dr["NombreProblema"]),
                                NombreCategoriaProblema = ManejoNulos.ManageNullStr(dr["NombreCategoriaProblema"]),
                                OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]),
                                FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]),
                                CodEstadoReparacion = ManejoNulos.ManageNullInteger(dr["EstadoReparacion"]),
                                //NombreUsuarioAtendidaInoperativaSolicitado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaSolicitado"]),
                                //NombreUsuarioAtendidaInoperativaAprobado = ManejoNulos.ManageNullStr(dr["NombreUsuarioAtendidaInoperativaAprobado"]),
                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }


        public bool AgregarOrdenCompraMaquinaInoperativa(MI_MaquinaInoperativaEntidad maquina) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_MaquinaInoperativa] SET 
                                  [OrdenCompra] = @pOrdenCompra,
                                    [FechaOrdenCompra] = @pfechaOrdenCompra
                                WHERE  CodMaquinaInoperativa = @pCodMaquinaInoperativa";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pOrdenCompra", ManejoNulos.ManageNullStr(maquina.OrdenCompra));
                    query.Parameters.AddWithValue("@pfechaOrdenCompra", ManejoNulos.ManageNullDate(maquina.FechaOrdenCompra));
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(maquina.CodMaquinaInoperativa));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public List<MI_MaquinaInoperativaEntidad> ReporteMaquinaInoperativa(DateTime fecha_inicio,DateTime fecha_fin,int estado) {

            List<MI_MaquinaInoperativaEntidad> lista = new List<MI_MaquinaInoperativaEntidad>();

            string consulta = @" SELECT DISTINCT maq.[CodMaquinaInoperativa]
                                  ,maq.[CodMaquinaInoperativa]
                                  ,maq.[CodMaquina]
                                  ,maq.[MaquinaLey]
                                  ,maq.[MaquinaModelo]
                                  ,maq.[MaquinaSala]
                                  ,maq.[MaquinaNumeroSerie]
                                  ,maq.[MaquinaPropietario]
                                  ,maq.[MaquinaMarca]
                                  ,maq.[TecnicoCreado]
                                  ,maq.[TecnicoAtencion]
                                  ,maq.[ObservacionCreado]
                                  ,maq.[ObservacionAtencion]
                                  ,maq.[CodEstadoInoperativa]
                                  ,maq.[CodPrioridad]
                                  ,maq.[FechaInoperativa]
                                  ,maq.[FechaCreado]
                                  ,maq.[FechaAtendidaOperativa]
                                  ,maq.[FechaAtendidaInoperativa]
                                  ,maq.[FechaAtendidaInoperativaSolicitado]
                                  ,maq.[FechaAtendidaInoperativaAprobado]
                                  ,maq.[CodEstadoProceso]
                                  ,maq.[OrdenCompra]
                                  ,maq.[FechaOrdenCompra]
                                  ,maq.[EstadoReparacion]
                                  ,maq.[NombreZona]
                                  ,sal.Nombre as NombreSala
                                  ,usu.UsuarioNombre as NombreUsuarioCreado
                                  ,pro.Nombre as NombreProblema
                                  ,mcp.Nombre as NombreCategoriaProblema
								  ,DATEDIFF(DAY, maq.FechaCreado, maq.FechaAtendidaInoperativaAprobado) AS DiasInoperativos
								  ,NULL AS ResueltoCompra
								  ,NULL AS StockResuelto
								  ,
															  (
									SELECT SUM(r.CostoReferencial)
									FROM MI_MaquinaInoperativaRepuestos mr
									INNER JOIN MI_Repuesto r ON mr.CodRepuesto = r.CodRepuesto
									WHERE mr.CodMaquinaInoperativa = maq.CodMaquinaInoperativa
								) AS Gasto
								  ,NULL AS FechaOC,
							     mip.NombreRepuesto AS Repuesto
								
                              FROM [MI_MaquinaInoperativa] maq
                              INNER JOIN Sala sal ON sal.CodSala = maq.CodSala 
                              INNER JOIN SEG_Usuario usu ON usu.UsuarioID = maq.CodUsuarioCreado 
                              INNER JOIN UsuarioSala usa ON usa.SalaId = maq.CodSala 
                              INNER JOIN MI_MaquinaInoperativaProblemas mip ON mip.CodMaquinaInoperativa = maq.CodMaquinaInoperativa 
                              INNER JOIN MI_Problema pro ON pro.CodProblema = mip.CodProblema 
                              INNER JOIN MI_CategoriaProblema mcp ON mcp.CodCategoriaProblema = pro.CodCategoriaProblema 
                             
                            WHERE  CONVERT(date, maq.FechaCreado) between @fecha_inicio and @fecha_fin
                            AND (@pEstado IS NULL OR @pEstado = 0 OR maq.CodEstadoProceso = @pEstado)
                            AND mip.Estado = (
								SELECT TOP 1
									   CASE 
										 WHEN EXISTS (
										   SELECT 1 FROM MI_MaquinaInoperativaProblemas 
										   WHERE CodMaquinaInoperativa = maq.CodMaquinaInoperativa AND Estado = 2
										 ) THEN 2
										 ELSE 1
									   END
							  )
                              ORDER BY maq.CodMaquinaInoperativa DESC";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@fecha_inicio", ManejoNulos.ManageNullDate(fecha_inicio));
                    query.Parameters.AddWithValue("@fecha_fin", ManejoNulos.ManageNullDate(fecha_fin));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullStr(estado));

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_MaquinaInoperativaEntidad {
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                NombreProblema = ManejoNulos.ManageNullStr(dr["NombreProblema"]),
                                CodEstadoInoperativa = ManejoNulos.ManageNullInteger(dr["CodEstadoInoperativa"]),
                                CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                MaquinaSala = ManejoNulos.ManageNullStr(dr["MaquinaSala"]),
                                FechaCreado = ManejoNulos.ManageNullDate(dr["FechaCreado"]),
                                MaquinaNumeroSerie = ManejoNulos.ManageNullStr(dr["MaquinaNumeroSerie"]),
                                MaquinaLey = ManejoNulos.ManageNullStr(dr["MaquinaLey"]),
                                MaquinaPropietario = ManejoNulos.ManageNullStr(dr["MaquinaPropietario"]),
                                MaquinaMarca = ManejoNulos.ManageNullStr(dr["MaquinaMarca"]),
                                MaquinaModelo = ManejoNulos.ManageNullStr(dr["MaquinaModelo"]),
                                PresupuestoRepuesto = ManejoNulos.ManageNullDouble(dr["Gasto"]),
                                NombreRepuesto = ManejoNulos.ManageNullStr(dr["Repuesto"]),
                                StockResuelto = ManejoNulos.ManageNullStr(dr["StockResuelto"]),
                                DiasInoperativos = ManejoNulos.ManageNullInteger(dr["DiasInoperativos"]),
                                ObservacionAtencion = ManejoNulos.ManageNullStr(dr["ObservacionAtencion"]),
                                FechaAtendidaOperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaOperativa"]),
                                FechaAtendidaInoperativa = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativa"]),
                                FechaAtendidaInoperativaAprobado = ManejoNulos.ManageNullDate(dr["FechaAtendidaInoperativaAprobado"]),
                                OrdenCompra = ManejoNulos.ManageNullStr(dr["OrdenCompra"]),
                                FechaOrdenCompra = ManejoNulos.ManageNullDate(dr["FechaOrdenCompra"]),
                                NombreZona = ManejoNulos.ManageNullStr(dr["NombreZona"]),

                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return lista;
        }
        

    }

}
