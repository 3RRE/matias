using CapaEntidad.Cortesias;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Cortesias {
    public class CRT_ProductoDAL {
        private readonly string _conexion;

        public CRT_ProductoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarProducto(CRT_Producto producto) {
            int idActualizado;
            string consulta = @"
                UPDATE CRT_Producto
                SET
                    IdSubTipo = @IdSubTipo,
                    IdMarca = @IdMarca,
                    Nombre = @Nombre,
                    ImagenUrl = @ImagenUrl,
                    Estado = @Estado,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", producto.Id);
                    query.Parameters.AddWithValue("@IdSubTipo", producto.IdSubTipo);
                    query.Parameters.AddWithValue("@IdMarca", producto.IdMarca);
                    query.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    query.Parameters.AddWithValue("@ImagenUrl", producto.ImagenUrl);
                    query.Parameters.AddWithValue("@Estado", producto.Estado);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarProducto(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM CRT_Producto
                OUTPUT DELETED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    idEliminado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idEliminado = 0;
            }
            return idEliminado;
        }

        public int InsertarProducto(CRT_Producto producto) {
            int idInsertado;
            string consulta = @"
                INSERT INTO CRT_Producto(IdSubTipo, IdMarca, Nombre, ImagenUrl, IdUsuario, Estado)
                OUTPUT INSERTED.Id
                VALUES (@IdSubTipo, @IdMarca, @Nombre, @ImagenUrl, @IdUsuario, @Estado)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdSubTipo", producto.IdSubTipo);
                    query.Parameters.AddWithValue("@IdMarca", producto.IdMarca);
                    query.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    query.Parameters.AddWithValue("@ImagenUrl", producto.ImagenUrl);
                    query.Parameters.AddWithValue("@IdUsuario", producto.IdUsuario);
                    query.Parameters.AddWithValue("@Estado", producto.Estado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public CRT_Producto ObtenerProductoPorId(int id) {
            CRT_Producto item = new CRT_Producto();
            string consulta = @"
                SELECT
                    p.Id,
                    p.IdSubTipo,
                    p.IdMarca,
                    p.Nombre,
                    p.ImagenUrl,
                    p.IdUsuario,
                    p.Estado,
                    p.FechaRegistro,
                    p.FechaModificacion,
                    st.Nombre AS NombreSubTipo,
                    t.Nombre AS NombreTipo,
                    m.Nombre AS NombreMarca,
                    t.Id AS IdTipo
                FROM
                    CRT_Producto AS p
                LEFT JOIN CRT_SubTipo AS st ON st.Id = p.IdSubTipo
                LEFT JOIN CRT_Tipo AS t ON t.Id = st.IdTipo
                LEFT JOIN CRT_Marca AS m ON m.Id = p.IdMarca
                WHERE
                    p.Id = @Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = new CRT_Producto {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                IdSubTipo = ManejoNulos.ManageNullInteger(dr["IdSubTipo"]),
                                IdMarca = ManejoNulos.ManageNullInteger(dr["IdMarca"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ImagenUrl = ManejoNulos.ManageNullStr(dr["ImagenUrl"]),
                                IdUsuario = ManejoNulos.ManageNullInteger(dr["IdUsuario"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                NombreSubTipo = ManejoNulos.ManageNullStr(dr["NombreSubTipo"]),
                                NombreTipo = ManejoNulos.ManageNullStr(dr["NombreTipo"]),
                                NombreMarca = ManejoNulos.ManageNullStr(dr["NombreMarca"]),
                                IdTipo = ManejoNulos.ManageNullInteger(dr["IdTipo"]),
                            };
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<CRT_Producto> ObtenerProductos() {
            List<CRT_Producto> items = new List<CRT_Producto>();
            string consulta = @"
                SELECT
                    p.Id,
                    p.IdSubTipo,
                    p.IdMarca,
                    p.Nombre,
                    p.ImagenUrl,
                    p.IdUsuario,
                    p.Estado,
                    p.FechaRegistro,
                    p.FechaModificacion,
                    st.Nombre AS NombreSubTipo,
                    t.Nombre AS NombreTipo,
                    m.Nombre AS NombreMarca,
                    t.Id AS IdTipo
                FROM
                    CRT_Producto AS p
                LEFT JOIN CRT_SubTipo AS st ON st.Id = p.IdSubTipo
                LEFT JOIN CRT_Tipo AS t ON t.Id = st.IdTipo
                LEFT JOIN CRT_Marca AS m ON m.Id = p.IdMarca
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CRT_Producto item = new CRT_Producto {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                IdSubTipo = ManejoNulos.ManageNullInteger(dr["IdSubTipo"]),
                                IdMarca = ManejoNulos.ManageNullInteger(dr["IdMarca"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ImagenUrl = ManejoNulos.ManageNullStr(dr["ImagenUrl"]),
                                IdUsuario = ManejoNulos.ManageNullInteger(dr["IdUsuario"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                NombreSubTipo = ManejoNulos.ManageNullStr(dr["NombreSubTipo"]),
                                NombreTipo = ManejoNulos.ManageNullStr(dr["NombreTipo"]),
                                NombreMarca = ManejoNulos.ManageNullStr(dr["NombreMarca"]),
                                IdTipo = ManejoNulos.ManageNullInteger(dr["IdTipo"]),
                            };
                            items.Add(item);
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<CRT_Producto> ObtenerProductosFiltrados(string whereFilter) {
            List<CRT_Producto> items = new List<CRT_Producto>();
            string consulta = $@"
                SELECT
                    p.Id,
                    p.IdSubTipo,
                    p.IdMarca,
                    p.Nombre,
                    p.ImagenUrl,
                    p.IdUsuario,
                    p.Estado,
                    p.FechaRegistro,
                    p.FechaModificacion,
                    st.Nombre AS NombreSubTipo,
                    t.Nombre AS NombreTipo,
                    m.Nombre AS NombreMarca,
                    t.Id AS IdTipo
                FROM
                    CRT_Producto AS p
                LEFT JOIN CRT_SubTipo AS st ON st.Id = p.IdSubTipo
                LEFT JOIN CRT_Tipo AS t ON t.Id = st.IdTipo
                LEFT JOIN CRT_Marca AS m ON m.Id = p.IdMarca
                {whereFilter}
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CRT_Producto item = new CRT_Producto {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                IdSubTipo = ManejoNulos.ManageNullInteger(dr["IdSubTipo"]),
                                IdMarca = ManejoNulos.ManageNullInteger(dr["IdMarca"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ImagenUrl = ManejoNulos.ManageNullStr(dr["ImagenUrl"]),
                                IdUsuario = ManejoNulos.ManageNullInteger(dr["IdUsuario"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                NombreSubTipo = ManejoNulos.ManageNullStr(dr["NombreSubTipo"]),
                                NombreTipo = ManejoNulos.ManageNullStr(dr["NombreTipo"]),
                                NombreMarca = ManejoNulos.ManageNullStr(dr["NombreMarca"]),
                                IdTipo = ManejoNulos.ManageNullInteger(dr["IdTipo"]),
                            };
                            items.Add(item);
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<CRT_Producto> ObtenerProductosPorIdsSubTipo(string idsSubTipoStr) {
            List<CRT_Producto> items = new List<CRT_Producto>();
            string consulta = $@"
                SELECT
                    p.Id,
                    p.IdSubTipo,
                    p.IdMarca,
                    p.Nombre,
                    p.ImagenUrl,
                    p.IdUsuario,
                    p.Estado,
                    p.FechaRegistro,
                    p.FechaModificacion,
                    st.Nombre AS NombreSubTipo,
                    t.Nombre AS NombreTipo,
                    m.Nombre AS NombreMarca,
                    t.Id AS IdTipo
                FROM
                    CRT_Producto AS p
                LEFT JOIN CRT_SubTipo AS st ON st.Id = p.IdSubTipo
                LEFT JOIN CRT_Tipo AS t ON t.Id = st.IdTipo
                LEFT JOIN CRT_Marca AS m ON m.Id = p.IdMarca
                WHERE p.IdSubTipo IN({idsSubTipoStr})
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CRT_Producto item = new CRT_Producto {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                IdSubTipo = ManejoNulos.ManageNullInteger(dr["IdSubTipo"]),
                                IdMarca = ManejoNulos.ManageNullInteger(dr["IdMarca"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ImagenUrl = ManejoNulos.ManageNullStr(dr["ImagenUrl"]),
                                IdUsuario = ManejoNulos.ManageNullInteger(dr["IdUsuario"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                NombreSubTipo = ManejoNulos.ManageNullStr(dr["NombreSubTipo"]),
                                NombreTipo = ManejoNulos.ManageNullStr(dr["NombreTipo"]),
                                NombreMarca = ManejoNulos.ManageNullStr(dr["NombreMarca"]),
                                IdTipo = ManejoNulos.ManageNullInteger(dr["IdTipo"]),
                            };
                            items.Add(item);
                        }
                    }
                }
            } catch { }
            return items;
        }


        public List<CRT_ProductoSala> ObtenerProductosSala() {
            List<CRT_ProductoSala> items = new List<CRT_ProductoSala>();
            string consulta = @"
                SELECT
	                p.Id,
                    p.IdSubTipo,
                    p.IdMarca,
	                p.Nombre,
                    p.ImagenUrl,
	                p.IdUsuario,
	                p.Estado,
                    p.FechaRegistro,
                    p.FechaModificacion,
	                st.Nombre AS NombreSubTipo,
	                t.Nombre AS NombreTipo,
	                m.Nombre AS NombreMarca
                FROM
	                CRT_Producto AS p
                LEFT JOIN CRT_SubTipo AS st ON st.Id = p.IdSubTipo
                LEFT JOIN CRT_Tipo AS t ON t.Id = st.IdTipo
                LEFT JOIN CRT_Marca AS m ON m.Id = p.IdMarca
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CRT_ProductoSala item = new CRT_ProductoSala {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                IdSubTipo = ManejoNulos.ManageNullInteger(dr["IdSubTipo"]),
                                IdMarca = ManejoNulos.ManageNullInteger(dr["IdMarca"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ImagenUrl = ManejoNulos.ManageNullStr(dr["ImagenUrl"]),
                                IdUsuario = ManejoNulos.ManageNullInteger(dr["IdUsuario"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                NombreSubTipo = ManejoNulos.ManageNullStr(dr["NombreSubTipo"]),
                                NombreTipo = ManejoNulos.ManageNullStr(dr["NombreTipo"]),
                                NombreMarca = ManejoNulos.ManageNullStr(dr["NombreMarca"]),
                                isChecked = false,
                            };
                            items.Add(item);
                        }
                    }
                }
            } catch { }
            return items;
        }

    }
}
