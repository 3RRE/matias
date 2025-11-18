using CapaEntidad.MaquinasInoperativas;
using S3k.Utilitario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_AlmacenDAL {

        string conexion = string.Empty;
        public MI_AlmacenDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_AlmacenEntidad> GetAllAlmacen() {
            List<MI_AlmacenEntidad> lista = new List<MI_AlmacenEntidad>();
            string consulta = @" SELECT al.[CodAlmacen]
                                  ,al.[Nombre]
                                  ,al.[Descripcion]
                                  ,al.[FechaRegistro]
                                  ,al.[FechaModificacion]
                                  ,al.[CodSala]
                                  ,al.[Estado]
                                  ,sa.[NombreCorto] as NombreSala
                              FROM [MI_Almacen] al
							  INNER JOIN Sala sa
							  ON al.CodSala = sa.CodSala  ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_AlmacenEntidad {
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
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
        public List<MI_AlmacenEntidad> GetAllAlmacenxSalasUsuario(int codUsuario)
        {
            List<MI_AlmacenEntidad> lista = new List<MI_AlmacenEntidad>();
            string consulta = @" SELECT al.[CodAlmacen]
                                  ,al.[Nombre]
                                  ,al.[Descripcion]
                                  ,al.[FechaRegistro]
                                  ,al.[FechaModificacion]
                                  ,al.[CodSala]
                                  ,al.[Estado]
                                  ,sa.[NombreCorto] as NombreSala
                              FROM [MI_Almacen] al
							  INNER JOIN Sala sa
							  ON al.CodSala = sa.CodSala 
                              INNER JOIN UsuarioSala usa ON usa.SalaId = al.CodSala 
                              WHERE usa.UsuarioId=@pCodUsuario ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new MI_AlmacenEntidad
                            {
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                            };

                            lista.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<MI_AlmacenEntidad> GetAllAlmacenActive(int codUsuario) {
            List<MI_AlmacenEntidad> lista = new List<MI_AlmacenEntidad>();
            string consulta = @" SELECT al.[CodAlmacen]
                                  ,al.[Nombre]
                                  ,al.[Descripcion]
                                  ,al.[FechaRegistro]
                                  ,al.[FechaModificacion]
                                  ,al.[CodSala]
                                  ,al.[Estado]
                                  ,sa.[NombreCorto] as NombreSala
                              FROM [MI_Almacen] al
							  INNER JOIN Sala sa
							  ON al.CodSala = sa.CodSala
                              INNER JOIN UsuarioSala usa ON usa.SalaId = al.CodSala 
                              WHERE usa.UsuarioId=@pCodUsuario AND al.[Estado]=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_AlmacenEntidad {
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
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
        public MI_AlmacenEntidad GetCodAlmacen(int codAlmacen) {
            MI_AlmacenEntidad item = new MI_AlmacenEntidad();
            string consulta = @" SELECT al.[CodAlmacen]
                                  ,al.[Nombre]
                                  ,al.[Descripcion]
                                  ,al.[FechaRegistro]
                                  ,al.[FechaModificacion]
                                  ,al.[CodSala]
                                  ,al.[Estado]
                                  ,sa.[NombreCorto] as NombreSala
                              FROM [MI_Almacen] al
							  INNER JOIN Sala sa
							  ON al.CodSala = sa.CodSala WHERE al.[CodAlmacen]=@pCodAlmacen ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodAlmacen", codAlmacen);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarAlmacen(MI_AlmacenEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_Almacen] ([Nombre],[Descripcion],[FechaRegistro],[FechaModificacion],[CodSala],[Estado])
                                OUTPUT Inserted.CodAlmacen   
	                            values (@pNombre, @pDescripcion, @pFechaRegistro, @pFechaModificacion, @pCodSala, @pEstado)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.CodSala));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarAlmacen(MI_AlmacenEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_Almacen] SET 
                                Nombre = @pNombre, 
                                Descripcion = @pDescripcion, 
                                FechaModificacion  = @pFechaModificacion,
                                CodSala  = @pCodSala,
                                Estado  = @pEstado 
                                WHERE  CodAlmacen = @pCodAlmacen";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.CodSala));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodAlmacen", ManejoNulos.ManageNullInteger(Entidad.CodAlmacen));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarAlmacen(int codAlmacen) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_Almacen] 
                                WHERE CodAlmacen  = @pCodAlmacen";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodAlmacen", ManejoNulos.ManageNullInteger(codAlmacen));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool AsignarUsuarioAlmacen(int codAlmacen,int codUsuario) {
            bool respuesta = false;
            string consulta = @"INSERT INTO [MI_AlmacenUsuario]   ([CodAlmacen],[CodUsuario])
                                VALUES (@pCodAlmacen,@pCodUsuario)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodAlmacen", ManejoNulos.ManageNullInteger(codAlmacen));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullInteger(codUsuario));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool QuitarUsuarioAlmacen(int codAlmacen, int codUsuario) {
            bool respuesta = false;
            string consulta = @"DELETE FROM MI_AlmacenUsuario WHERE CodAlmacen = @pCodAlmacen AND CodUsuario = @pCodUsuario ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodAlmacen", ManejoNulos.ManageNullInteger(codAlmacen));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullInteger(codUsuario));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public MI_AlmacenEntidad GetCodAlmacenCodUsuario(int codAlmacen, int codUsuario) {
            MI_AlmacenEntidad item = new MI_AlmacenEntidad();
            string consulta = @" SELECT al.[CodAlmacen]
                                  ,al.[Nombre]
                                  ,al.[Descripcion]
                                  ,al.[FechaRegistro]
                                  ,al.[FechaModificacion]
                                  ,al.[CodSala]
                                  ,al.[Estado]
                                  ,sa.[NombreCorto] as NombreSala
                              FROM [MI_Almacen] al
							  INNER JOIN Sala sa
							  ON al.CodSala = sa.CodSala 
                              INNER JOIN [MI_AlmacenUsuario] au
                              ON al.CodAlmacen = au.CodAlmacen
                              WHERE au.[CodAlmacen]=@pCodAlmacen AND au.[CodUsuario]=@pCodUsuario";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodAlmacen", codAlmacen);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }

        public List<MI_AlmacenEntidad> GetAllAlmacenxUsuario(int codUsuario) {
            List<MI_AlmacenEntidad> lista = new List<MI_AlmacenEntidad>();
            string consulta = @" SELECT al.[CodAlmacen]
                                  ,al.[Nombre]
                                  ,al.[Descripcion]
                                  ,al.[FechaRegistro]
                                  ,al.[FechaModificacion]
                                  ,al.[CodSala]
                                  ,al.[Estado]
                                  ,sa.[NombreCorto] as NombreSala
                              FROM [MI_Almacen] al
							  INNER JOIN Sala sa
							  ON al.CodSala = sa.CodSala 
                              INNER JOIN [MI_AlmacenUsuario] au
                              ON al.CodAlmacen = au.CodAlmacen
                              WHERE au.[CodUsuario]=@pCodUsuario ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_AlmacenEntidad {
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
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
        public List<MI_AlmacenEntidad> GetAllAlmacenxSala(int codSala) {
            List<MI_AlmacenEntidad> lista = new List<MI_AlmacenEntidad>();
            string consulta = @" SELECT al.[CodAlmacen]
                                  ,al.[Nombre]
                                  ,al.[Descripcion]
                                  ,al.[FechaRegistro]
                                  ,al.[FechaModificacion]
                                  ,al.[CodSala]
                                  ,al.[Estado]
                                  ,sa.[NombreCorto] as NombreSala
                              FROM [MI_Almacen] al
							  INNER JOIN Sala sa
							  ON al.CodSala = sa.CodSala 
                              WHERE al.[CodSala] IN (@pCodSala) ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_AlmacenEntidad {
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
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


        public List<MI_AlmacenEntidad> GetAllUsuarioxAlmacen(int codAlmacen) {
            List<MI_AlmacenEntidad> lista = new List<MI_AlmacenEntidad>();
            string consulta = @" SELECT al.[CodAlmacen]
                                  ,al.[Nombre]
                                  ,al.[Descripcion]
                                  ,al.[FechaRegistro]
                                  ,al.[FechaModificacion]
                                  ,al.[CodSala]
                                  ,al.[Estado]
                                  ,sa.[NombreCorto] as NombreSala
                              FROM [MI_Almacen] al
							  INNER JOIN Sala sa
							  ON al.CodSala = sa.CodSala 
                              INNER JOIN [MI_AlmacenUsuario] au
                              ON al.CodAlmacen = au.CodAlmacen
                              WHERE au.[CodAlmacen]=@pCodAlmacen ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodAlmacen", codAlmacen);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_AlmacenEntidad {
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
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
