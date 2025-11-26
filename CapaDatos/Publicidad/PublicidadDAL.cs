

using CapaEntidad.Publicidad;
using S3k.Utilitario;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System;

namespace CapaDatos.Publicidad {
    public class PublicidadDAL {
        string _conexion = string.Empty;

        public PublicidadDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<PublicidadEntidad> ListarPublicidadActivaPorSala(int codSala) {
            List<PublicidadEntidad> lista = new List<PublicidadEntidad>();
            string consulta = @"
                SELECT IdPublicidad, CodSala, Titulo, RutaImagen, UrlEnlace, Orden, FechaInicio, FechaFin, Estado, FechaRegistro
                FROM PUB_Publicidad (NOLOCK)
                WHERE CodSala = @pCodSala 
                  AND Estado = 1 
                  AND GETDATE() BETWEEN FechaInicio AND FechaFin
                ORDER BY Orden ASC";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    using (var dr = query.ExecuteReader()) {
                        while (dr.Read()) {
                            var item = new PublicidadEntidad {
                                IdPublicidad = ManejoNulos.ManageNullInteger(dr["IdPublicidad"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Titulo = ManejoNulos.ManageNullStr(dr["Titulo"]),
                                RutaImagen = ManejoNulos.ManageNullStr(dr["RutaImagen"]),
                                UrlEnlace = ManejoNulos.ManageNullStr(dr["UrlEnlace"]),
                                Orden = ManejoNulos.ManageNullInteger(dr["Orden"]),
                                FechaInicio = ManejoNulos.ManageNullDate(dr["FechaInicio"]),
                                FechaFin = ManejoNulos.ManageNullDate(dr["FechaFin"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"])
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public List<EventoEntidad> ListarEventosActivosPorSala(int codSala) {
            List<EventoEntidad> lista = new List<EventoEntidad>();
            string consulta = @"
                SELECT IdEvento, CodSala, Titulo, Descripcion, RutaImagen, FechaEvento,UrlDireccion, Estado, FechaRegistro
                FROM PUB_Evento (NOLOCK)
                WHERE CodSala = @pCodSala 
                  AND Estado = 1 
                  AND FechaEvento >= GETDATE()
                ORDER BY FechaEvento ASC";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    using (var dr = query.ExecuteReader()) {
                        while (dr.Read()) {
                            var item = new EventoEntidad {
                                IdEvento = ManejoNulos.ManageNullInteger(dr["IdEvento"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Titulo = ManejoNulos.ManageNullStr(dr["Titulo"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                RutaImagen = ManejoNulos.ManageNullStr(dr["RutaImagen"]),
                                UrlDireccion = ManejoNulos.ManageNullStr(dr["UrlDireccion"]),
                                FechaEvento = ManejoNulos.ManageNullDate(dr["FechaEvento"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"])
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        #region CRUD Publicidad

        public List<PublicidadEntidad> ListadoPublicidadAdmin(int codSala) {
            List<PublicidadEntidad> lista = new List<PublicidadEntidad>();
            string consulta = @"
                SELECT p.IdPublicidad, p.CodSala, s.Nombre as NombreSala, p.Titulo, p.RutaImagen, p.UrlEnlace, p.Orden, p.FechaInicio, p.FechaFin, p.Estado, p.FechaRegistro
                FROM PUB_Publicidad p (NOLOCK)
                JOIN Sala s (NOLOCK) ON p.CodSala = s.CodSala
where p.CodSala = @pCodSala 
                ORDER BY p.FechaRegistro asc";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    using (var dr = query.ExecuteReader()) {
                        while (dr.Read()) {
                            var item = new PublicidadEntidad {
                                IdPublicidad = ManejoNulos.ManageNullInteger(dr["IdPublicidad"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Titulo = ManejoNulos.ManageNullStr(dr["Titulo"]),
                                RutaImagen = ManejoNulos.ManageNullStr(dr["RutaImagen"]),
                                UrlEnlace = ManejoNulos.ManageNullStr(dr["UrlEnlace"]),
                                Orden = ManejoNulos.ManageNullInteger(dr["Orden"]),
                                FechaInicio = ManejoNulos.ManageNullDate(dr["FechaInicio"]),
                                FechaFin = ManejoNulos.ManageNullDate(dr["FechaFin"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"])
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch (Exception ex) { Console.WriteLine(ex.Message); }
            return lista;
        }

        public PublicidadEntidad PublicidadListaIdJson(int idPublicidad) {
            PublicidadEntidad item = null;
            string consulta = @"
                SELECT IdPublicidad, CodSala, Titulo, RutaImagen, UrlEnlace, Orden, FechaInicio, FechaFin, Estado
                FROM PUB_Publicidad (NOLOCK)
                WHERE IdPublicidad = @pIdPublicidad";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdPublicidad", idPublicidad);
                    using (var dr = query.ExecuteReader()) {
                        if (dr.Read()) {
                            item = new PublicidadEntidad {
                                IdPublicidad = ManejoNulos.ManageNullInteger(dr["IdPublicidad"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Titulo = ManejoNulos.ManageNullStr(dr["Titulo"]),
                                RutaImagen = ManejoNulos.ManageNullStr(dr["RutaImagen"]),
                                UrlEnlace = ManejoNulos.ManageNullStr(dr["UrlEnlace"]),
                                Orden = ManejoNulos.ManageNullInteger(dr["Orden"]),
                                FechaInicio = ManejoNulos.ManageNullDate(dr["FechaInicio"]),
                                FechaFin = ManejoNulos.ManageNullDate(dr["FechaFin"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"])
                            };
                        }
                    }
                }
            } catch (Exception ex) { Console.WriteLine(ex.Message); }
            return item;
        }

        public bool InsertarPublicidadJson(PublicidadEntidad publicidad) {
            string consulta = @"
                INSERT INTO PUB_Publicidad (CodSala, Titulo, RutaImagen, UrlEnlace, Orden, FechaInicio, FechaFin, Estado, FechaRegistro)
                VALUES (@pCodSala, @pTitulo, @pRutaImagen, @pUrlEnlace, @pOrden, @pFechaInicio, @pFechaFin, @pEstado, GETDATE())";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(publicidad.CodSala));
                    query.Parameters.AddWithValue("@pTitulo", ManejoNulos.ManageNullStr(publicidad.Titulo));
                    query.Parameters.AddWithValue("@pRutaImagen", ManejoNulos.ManageNullStr(publicidad.RutaImagen));
                    query.Parameters.AddWithValue("@pUrlEnlace", ManejoNulos.ManageNullStr(publicidad.UrlEnlace));
                    query.Parameters.AddWithValue("@pOrden", ManejoNulos.ManageNullInteger(publicidad.Orden));
                    query.Parameters.AddWithValue("@pFechaInicio", ManejoNulos.ManageNullDate(publicidad.FechaInicio));
                    query.Parameters.AddWithValue("@pFechaFin", ManejoNulos.ManageNullDate(publicidad.FechaFin));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(publicidad.Estado));
                    query.ExecuteNonQuery();
                    return true;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool ModificarPublicidadJson(PublicidadEntidad publicidad) {
            string consulta = @"
                UPDATE PUB_Publicidad SET
                    CodSala = @pCodSala,
                    Titulo = @pTitulo,
                    RutaImagen = @pRutaImagen,
                    UrlEnlace = @pUrlEnlace,
                    Orden = @pOrden,
                    FechaInicio = @pFechaInicio,
                    FechaFin = @pFechaFin,
                    Estado = @pEstado
                WHERE IdPublicidad = @pIdPublicidad";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdPublicidad", ManejoNulos.ManageNullInteger(publicidad.IdPublicidad));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(publicidad.CodSala));
                    query.Parameters.AddWithValue("@pTitulo", ManejoNulos.ManageNullStr(publicidad.Titulo));
                    query.Parameters.AddWithValue("@pRutaImagen", ManejoNulos.ManageNullStr(publicidad.RutaImagen));
                    query.Parameters.AddWithValue("@pUrlEnlace", ManejoNulos.ManageNullStr(publicidad.UrlEnlace));
                    query.Parameters.AddWithValue("@pOrden", ManejoNulos.ManageNullInteger(publicidad.Orden));
                    query.Parameters.AddWithValue("@pFechaInicio", ManejoNulos.ManageNullDate(publicidad.FechaInicio));
                    query.Parameters.AddWithValue("@pFechaFin", ManejoNulos.ManageNullDate(publicidad.FechaFin));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(publicidad.Estado));
                    query.ExecuteNonQuery();
                    return true;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool ModificarEstadoPublicidadJson(int idPublicidad, int estado) {
            string consulta = @"UPDATE PUB_Publicidad SET Estado = @pEstado WHERE IdPublicidad = @pIdPublicidad";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdPublicidad", idPublicidad);
                    query.Parameters.AddWithValue("@pEstado", estado);
                    query.ExecuteNonQuery();
                    return true;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region CRUD Evento

        public List<EventoEntidad> ListadoEventoAdmin(int codSala) { 
            List<EventoEntidad> lista = new List<EventoEntidad>();
            string consulta = @"
                SELECT e.IdEvento, e.CodSala, s.Nombre as NombreSala, e.Titulo, e.Descripcion, e.RutaImagen,e.UrlDireccion ,e.FechaEvento, e.Estado, e.FechaRegistro
                FROM PUB_Evento e (NOLOCK)
                JOIN Sala s (NOLOCK) ON e.CodSala = s.CodSala
                WHERE e.CodSala = @pCodSala 
                ORDER BY e.FechaRegistro DESC";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala); 
                    using (var dr = query.ExecuteReader()) {
                        while (dr.Read()) {
                            var item = new EventoEntidad {
                                IdEvento = ManejoNulos.ManageNullInteger(dr["IdEvento"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Titulo = ManejoNulos.ManageNullStr(dr["Titulo"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                RutaImagen = ManejoNulos.ManageNullStr(dr["RutaImagen"]),
                                UrlDireccion = ManejoNulos.ManageNullStr(dr["UrlDireccion"]),
                                FechaEvento = ManejoNulos.ManageNullDate(dr["FechaEvento"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"])
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch (Exception ex) { Console.WriteLine(ex.Message); }
            return lista;
        }

        public EventoEntidad EventoListaIdJson(int idEvento) {
            EventoEntidad item = null;
            string consulta = @"
                SELECT IdEvento, CodSala, Titulo, Descripcion, RutaImagen,UrlDireccion, FechaEvento, Estado
                FROM PUB_Evento (NOLOCK)
                WHERE IdEvento = @pIdEvento";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdEvento", idEvento);
                    using (var dr = query.ExecuteReader()) {
                        if (dr.Read()) {
                            item = new EventoEntidad {
                                IdEvento = ManejoNulos.ManageNullInteger(dr["IdEvento"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Titulo = ManejoNulos.ManageNullStr(dr["Titulo"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                RutaImagen = ManejoNulos.ManageNullStr(dr["RutaImagen"]),
                                UrlDireccion = ManejoNulos.ManageNullStr(dr["UrlDireccion"]),
                                FechaEvento = ManejoNulos.ManageNullDate(dr["FechaEvento"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"])
                            };
                        }
                    }
                }
            } catch (Exception ex) { Console.WriteLine(ex.Message); }
            return item;
        }

        public bool InsertarEventoJson(EventoEntidad evento) {
            string consulta = @"
                INSERT INTO PUB_Evento (CodSala, Titulo, Descripcion, RutaImagen,UrlDireccion, FechaEvento, Estado, FechaRegistro)
                VALUES (@pCodSala, @pTitulo, @pDescripcion, @pRutaImagen,@pUrlDireccion, @pFechaEvento, @pEstado, GETDATE())";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(evento.CodSala));
                    query.Parameters.AddWithValue("@pTitulo", ManejoNulos.ManageNullStr(evento.Titulo));
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(evento.Descripcion));
                    query.Parameters.AddWithValue("@pUrlDireccion", ManejoNulos.ManageNullStr(evento.UrlDireccion));
                    query.Parameters.AddWithValue("@pRutaImagen", ManejoNulos.ManageNullStr(evento.RutaImagen));
                    query.Parameters.AddWithValue("@pFechaEvento", ManejoNulos.ManageNullDate(evento.FechaEvento));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(evento.Estado));
                    query.ExecuteNonQuery();
                    return true;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool ModificarEventoJson(EventoEntidad evento) {
            string consulta = @"
                UPDATE PUB_Evento SET
                    CodSala = @pCodSala,
                    Titulo = @pTitulo,
                    Descripcion = @pDescripcion,
                    RutaImagen = @pRutaImagen,
                    UrlDireccion = @pUrlDireccion,
                    FechaEvento = @pFechaEvento,
                    Estado = @pEstado
                WHERE IdEvento = @pIdEvento";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdEvento", ManejoNulos.ManageNullInteger(evento.IdEvento));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(evento.CodSala));
                    query.Parameters.AddWithValue("@pTitulo", ManejoNulos.ManageNullStr(evento.Titulo));
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(evento.Descripcion));
                    query.Parameters.AddWithValue("@pUrlDireccion", ManejoNulos.ManageNullStr(evento.UrlDireccion));
                    query.Parameters.AddWithValue("@pRutaImagen", ManejoNulos.ManageNullStr(evento.RutaImagen));
                    query.Parameters.AddWithValue("@pFechaEvento", ManejoNulos.ManageNullDate(evento.FechaEvento));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(evento.Estado));
                    query.ExecuteNonQuery();
                    return true;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool ModificarEstadoEventoJson(int idEvento, int estado) {
            string consulta = @"UPDATE PUB_Evento SET Estado = @pEstado WHERE IdEvento = @pIdEvento";
            try {
                using (var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdEvento", idEvento);
                    query.Parameters.AddWithValue("@pEstado", estado);
                    query.ExecuteNonQuery();
                    return true;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion
    }
}
