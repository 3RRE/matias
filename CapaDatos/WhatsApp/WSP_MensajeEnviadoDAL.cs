using CapaEntidad.WhatsApp;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.WhatsApp {
    public class WSP_MensajeEnviadoDAL {
        string _conexion = string.Empty;

        public WSP_MensajeEnviadoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerTodosLosMensajesEnviados() {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosCorrectamente() {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.estado = 1
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosIncorrectamente() {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.estado = 0
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerTodosLosMensajesEnviadosPorCodigoDePais(string codigoPais) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                c.codigoPais = @pCodigoPais
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPais", codigoPais);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosCorrectamentePorCodigoDePais(string codigoPais) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                c.codigoPais = @pCodigoPais AND
	                me.estado = 1
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPais", codigoPais);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosIncorrectamentePorCodigoDePais(string codigoPais) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                c.codigoPais = @pCodigoPais AND
	                me.estado = 1
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPais", codigoPais);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerTodosLosMensajesEnviadosPorIdContacto(int idContacto) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.idContacto = @pIdContacto
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdContacto", idContacto);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosCorrectamentePorIdContacto(int idContacto) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.idContacto = @pIdContacto AND
	                me.estado = 1
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdContacto", idContacto);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosIncorrectamentePorIdContacto(int idContacto) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.idContacto = @pIdContacto AND
	                me.estado = 0
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdContacto", idContacto);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerTodosLosMensajesEnviadosDesdeUnNumero(string phoneNumber) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.desde LIKE CONCAT('%', @pDesde, '%')
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDesde", phoneNumber);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosCorrectamenteDesdeUnNumero(string phoneNumber) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.desde LIKE CONCAT('%', @pDesde, '%') AND
	                me.estado = 1
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDesde", phoneNumber);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosIncorrectamenteDesdeUnNumero(string phoneNumber) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.desde LIKE CONCAT('%', @pDesde, '%') AND
	                me.estado = 0
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDesde", phoneNumber);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerTodosLosMensajesEnviadosHaciaUnNumero(string phoneNumber) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.hacia LIKE CONCAT('%', @pHacia, '%')
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pHacia", phoneNumber);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosCorrectamenteHaciaUnNumero(string phoneNumber) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.hacia LIKE CONCAT('%', @pHacia, '%') AND
	                me.estado = 1
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pHacia", phoneNumber);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosIncorrectamenteHaciaUnNumero(string phoneNumber) {
            List<WSP_MensajeEnviadoEntidad> lista = new List<WSP_MensajeEnviadoEntidad>();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.hacia LIKE CONCAT('%', @pHacia, '%') AND
	                me.estado = 0
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pHacia", phoneNumber);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
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

        public WSP_MensajeEnviadoEntidad ObtenerMensajeEnviadoPorIdMensajeEnviado(int idMensajeEnviado) {
            WSP_MensajeEnviadoEntidad mensajeEnviado = new WSP_MensajeEnviadoEntidad();
            string consulta = @"
                SELECT
	                me.idMensajeEnviado AS IdMensajeEnviado,
	                me.idContacto AS IdContacto,
	                c.nombre AS Nombre,
	                me.codigoMensaje AS CodigoMensaje,
	                c.codigoPais AS CodigoPais,
	                c.numero AS NumeroInicial,
	                me.desde AS Desde,
	                me.hacia AS Hacia,
	                me.mensaje AS Mensaje,
	                me.fechaEnvio AS FechaEnvio,
	                me.estado AS Estado
                FROM
	                WSP_MensajeEnviado AS me
                INNER JOIN 
	                WSP_Contacto AS c ON c.idContacto = me.idContacto
                WHERE
	                me.idMensajeEnviado = @pIdMensajeEnviado
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdMensajeEnviado", idMensajeEnviado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_MensajeEnviadoEntidad {
                                IdMensajeEnviado = ManejoNulos.ManageNullInteger(dr["IdMensajeEnviado"]),
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                CodigoMensaje = ManejoNulos.ManageNullStr(dr["CodigoMensaje"]),
                                Desde = ManejoNulos.ManageNullStr(dr["Desde"]),
                                Hacia = ManejoNulos.ManageNullStr(dr["Hacia"]),
                                Mensaje = ManejoNulos.ManageNullStr(dr["Mensaje"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                NombreDestinatarioContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NumeroContacto = ManejoNulos.ManageNullStr(dr["NumeroInicial"]),
                                CodigoPaisContacto = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                            };
                            mensajeEnviado = item;
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return mensajeEnviado;
        }

        public bool InsertarMensaje(WSP_MensajeEnviadoEntidad mensajeEnviado) {
            bool response = false;
            string consulta = @"
                INSERT INTO
	                WSP_MensajeEnviado(idContacto, codigoMensaje, desde, hacia, mensaje, fechaEnvio, estado)
                VALUES
	                (@pIdContacto, @pCodigoMensaje, @pDesde, @pHacia, @pMensaje, @pFechaEnvio, @pEstadoEnvioMensaje)
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdContacto", mensajeEnviado.IdContacto);
                    query.Parameters.AddWithValue("@pCodigoMensaje", mensajeEnviado.CodigoMensaje);
                    query.Parameters.AddWithValue("@pDesde", mensajeEnviado.Desde);
                    query.Parameters.AddWithValue("@pHacia", mensajeEnviado.Hacia);
                    query.Parameters.AddWithValue("@pMensaje", mensajeEnviado.Mensaje);
                    query.Parameters.AddWithValue("@pFechaEnvio", mensajeEnviado.FechaEnvio);
                    query.Parameters.AddWithValue("@pEstadoEnvioMensaje", mensajeEnviado.Estado);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
    }
}
