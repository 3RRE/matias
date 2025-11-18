using CapaEntidad.WhatsApp;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.WhatsApp {
    public class WSP_ContactoDAL {
        string _conexion = string.Empty;

        public WSP_ContactoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<WSP_ContactoEntidad> ObtenerTodosLosContactos() {
            List<WSP_ContactoEntidad> lista = new List<WSP_ContactoEntidad>();
            string consulta = @"
                SELECT
	                idContacto AS IdContacto,
	                nombre AS Nombre,
	                codigoPais AS CodigoPais,
	                numero AS Numero,
	                CONCAT(codigoPais, numero) AS NumeroCompleto,
	                estado AS Estado
                FROM
	                WSP_Contacto
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_ContactoEntidad {
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                                Numero = ManejoNulos.ManageNullStr(dr["Numero"]),
                                NumeroCompleto = ManejoNulos.ManageNullStr(dr["NumeroCompleto"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public List<WSP_ContactoEntidad> ObtenerContactosActivos() {
            List<WSP_ContactoEntidad> lista = new List<WSP_ContactoEntidad>();
            string consulta = @"
                SELECT
	                idContacto AS IdContacto,
	                nombre AS Nombre,
	                codigoPais AS CodigoPais,
	                numero AS Numero,
	                CONCAT(codigoPais, numero) AS NumeroCompleto,
	                estado AS Estado
                FROM
	                WSP_Contacto
                WHERE
	                estado = 1
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_ContactoEntidad {
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                                Numero = ManejoNulos.ManageNullStr(dr["Numero"]),
                                NumeroCompleto = ManejoNulos.ManageNullStr(dr["NumeroCompleto"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public List<WSP_ContactoEntidad> ObtenerContactosInactivos() {
            List<WSP_ContactoEntidad> lista = new List<WSP_ContactoEntidad>();
            string consulta = @"
                SELECT
	                idContacto AS IdContacto,
	                nombre AS Nombre,
	                codigoPais AS CodigoPais,
	                numero AS Numero,
	                CONCAT(codigoPais, numero) AS NumeroCompleto,
	                estado AS Estado
                FROM
	                WSP_Contacto
                WHERE
	                estado = 0
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_ContactoEntidad {
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                                Numero = ManejoNulos.ManageNullStr(dr["Numero"]),
                                NumeroCompleto = ManejoNulos.ManageNullStr(dr["NumeroCompleto"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public List<WSP_ContactoEntidad> ObtenerTodosLosContactosPorCodigoDePais(string codigoPais) {
            List<WSP_ContactoEntidad> lista = new List<WSP_ContactoEntidad>();
            string consulta = @"
                SELECT
	                idContacto AS IdContacto,
	                nombre AS Nombre,
	                codigoPais AS CodigoPais,
	                numero AS Numero,
	                CONCAT(codigoPais, numero) AS NumeroCompleto,
	                estado AS Estado
                FROM
	                WSP_Contacto
                WHERE
	                codigoPais = @pCodigoPais
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPais", codigoPais);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_ContactoEntidad {
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                                Numero = ManejoNulos.ManageNullStr(dr["Numero"]),
                                NumeroCompleto = ManejoNulos.ManageNullStr(dr["NumeroCompleto"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public List<WSP_ContactoEntidad> ObtenerContactosActivosPorCodigoDePais(string codigoPais) {
            List<WSP_ContactoEntidad> lista = new List<WSP_ContactoEntidad>();
            string consulta = @"
                SELECT
	                idContacto AS IdContacto,
	                nombre AS Nombre,
	                codigoPais AS CodigoPais,
	                numero AS Numero,
	                CONCAT(codigoPais, numero) AS NumeroCompleto,
	                estado AS Estado
                FROM
	                WSP_Contacto
                WHERE
	                codigoPais = @pCodigoPais AND
	                estado = 1
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPais", codigoPais);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_ContactoEntidad {
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                                Numero = ManejoNulos.ManageNullStr(dr["Numero"]),
                                NumeroCompleto = ManejoNulos.ManageNullStr(dr["NumeroCompleto"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public List<WSP_ContactoEntidad> ObtenerContactosInactivosPorCodigoDePais(string codigoPais) {
            List<WSP_ContactoEntidad> lista = new List<WSP_ContactoEntidad>();
            string consulta = @"
                SELECT
	                idContacto AS IdContacto,
	                nombre AS Nombre,
	                codigoPais AS CodigoPais,
	                numero AS Numero,
	                CONCAT(codigoPais, numero) AS NumeroCompleto,
	                estado AS Estado
                FROM
	                WSP_Contacto
                WHERE
	                codigoPais = @pCodigoPais AND
	                estado = 0
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPais", codigoPais);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_ContactoEntidad {
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                                Numero = ManejoNulos.ManageNullStr(dr["Numero"]),
                                NumeroCompleto = ManejoNulos.ManageNullStr(dr["NumeroCompleto"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public List<WSP_ContactoEntidad> ObtenerContactosPorIdsContacto(string ids) {
            List<WSP_ContactoEntidad> lista = new List<WSP_ContactoEntidad>();
            string consulta = $@"
                SELECT
	                idContacto AS IdContacto,
	                nombre AS Nombre,
	                codigoPais AS CodigoPais,
	                numero AS Numero,
	                CONCAT(codigoPais, numero) AS NumeroCompleto,
	                estado AS Estado
                FROM
	                WSP_Contacto
                WHERE
	                idContacto IN {ids}
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_ContactoEntidad {
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                                Numero = ManejoNulos.ManageNullStr(dr["Numero"]),
                                NumeroCompleto = ManejoNulos.ManageNullStr(dr["NumeroCompleto"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }

        public WSP_ContactoEntidad ObtenerContactoPorIdContacto(int idContacto) {
            WSP_ContactoEntidad contacto = new WSP_ContactoEntidad();
            string consulta = @"
                SELECT
	                idContacto AS IdContacto,
	                nombre AS Nombre,
	                codigoPais AS CodigoPais,
	                numero AS Numero,
	                CONCAT(codigoPais, numero) AS NumeroCompleto,
	                estado AS Estado
                FROM
	                WSP_Contacto
                WHERE
	                idContacto = @pIdContacto
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdContacto", idContacto);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_ContactoEntidad {
                                IdContacto = ManejoNulos.ManageNullInteger(dr["IdContacto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodigoPais = ManejoNulos.ManageNullStr(dr["CodigoPais"]),
                                Numero = ManejoNulos.ManageNullStr(dr["Numero"]),
                                NumeroCompleto = ManejoNulos.ManageNullStr(dr["NumeroCompleto"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                            };
                            contacto = item;
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return contacto;
        }

        public bool InsertarContacto(WSP_ContactoEntidad contacto) {
            bool response = false;
            string consulta = @"
                INSERT INTO
	                WSP_Contacto(nombre, codigoPais, numero, estado)
                VALUES
	                (@pNombre,@pCodigoPais,@pNumero, 1)
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", contacto.Nombre);
                    query.Parameters.AddWithValue("@pCodigoPais", contacto.CodigoPais);
                    query.Parameters.AddWithValue("@pNumero", contacto.Numero);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool ActualizarContactoPorIdContacto(WSP_ContactoEntidad contacto) {
            bool response = false;
            string consulta = @"
                UPDATE
	                WSP_Contacto
                SET
	                nombre = @pNombre,
	                codigoPais = @pCodigoPais,
	                numero = @pNumero
                WHERE
	                idContacto = @pIdContacto
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdContacto", contacto.IdContacto);
                    query.Parameters.AddWithValue("@pNombre", contacto.Nombre);
                    query.Parameters.AddWithValue("@pCodigoPais", contacto.CodigoPais);
                    query.Parameters.AddWithValue("@pNumero", contacto.Numero);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool ActivarContactoPorIdContacto(int idContacto) {
            bool response = false;
            string consulta = @"
                UPDATE
	                WSP_Contacto
                SET
	                estado = 1
                WHERE
	                idContacto = @pIdContacto
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdContacto", idContacto);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool DesactivarContactoPorIdContacto(int idContacto) {
            bool response = false;
            string consulta = @"
                UPDATE
	                WSP_Contacto
                SET
	                estado = 0
                WHERE
	                idContacto = @pIdContacto
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdContacto", idContacto);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool ActivarContactosPorCodigoDePais(string codigoPais) {
            bool response = false;
            string consulta = @"
                UPDATE
	                WSP_Contacto
                SET
	                estado = 1
                WHERE
	                codigoPais = @pCodigoPais
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPais", codigoPais);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool DesactivarContactosPorCodigoDePais(string codigoPais) {
            bool response = false;
            string consulta = @"
                UPDATE
	                WSP_Contacto
                SET
	                estado = 0
                WHERE
	                codigoPais = @pCodigoPais
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPais", codigoPais);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool EliminarContactoPorIdContacto(int idContacto) {
            bool response = false;
            string consulta = @"
                DELETE FROM
	                WSP_Contacto
                WHERE
	                idContacto = @pIdContacto
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdContacto", idContacto);
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
