using CapaEntidad.MaquinasInoperativas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_PiezaRepuestoAlmacenDAL {

        string conexion = string.Empty;
        public MI_PiezaRepuestoAlmacenDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacen() {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @" SELECT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Cantidad]
                                  ,pra.[FechaRegistro]
                                  ,pra.[FechaModificacion]
                                  ,pra.[Estado]
                                  ,al.Nombre as NombreAlmacen
                                  ,sa.Nombre as NombreSala
                                  , CASE pra.CodTipo
									  WHEN  1
									  THEN
											pie.Nombre
									  ELSE 
											rep.Nombre
									  END AS NombrePiezaRepuesto
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              INNER JOIN MI_Almacen al ON  pra.[CodAlmacen]=al.CodAlmacen
                              INNER JOIN Sala sa ON  sa.CodSala=al.CodSala
                              INNER JOIN MI_Pieza pie ON  pie.CodPieza=pra.CodpiezaRepuesto
                              INNER JOIN MI_Repuesto rep ON  rep.CodRepuesto=pra.CodpiezaRepuesto";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_PiezaRepuestoAlmacenEntidad {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreAlmacen = ManejoNulos.ManageNullStr(dr["NombreAlmacen"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombrePiezaRepuesto = ManejoNulos.ManageNullStr(dr["NombrePiezaRepuesto"]),
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
        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenActive() {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @" SELECT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Cantidad]
                                  ,pra.[FechaRegistro]
                                  ,pra.[FechaModificacion]
                                  ,pra.[Estado]
                                  ,al.Nombre as NombreAlmacen
                                  ,sa.Nombre as NombreSala
                                  ,rep.Nombre as NombrePiezaRepuesto
                                  ,sa.[CodSala] as CodSala
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              INNER JOIN MI_Almacen al ON  pra.[CodAlmacen]=al.CodAlmacen
                              INNER JOIN Sala sa ON  sa.CodSala=al.CodSala
                              INNER JOIN MI_Repuesto rep ON  rep.CodRepuesto=pra.CodpiezaRepuesto
                              WHERE pra.Estado=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_PiezaRepuestoAlmacenEntidad {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreAlmacen = ManejoNulos.ManageNullStr(dr["NombreAlmacen"]),
                                NombrePiezaRepuesto = ManejoNulos.ManageNullStr(dr["NombrePiezaRepuesto"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreTipo = "Repuesto"
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


        public List<MI_PiezaRepuestoAlmacenEntidad> GetPiezaRepuestoAlmacenPropioxCodPiezaRepuesto(int codPiezaRepuesto, int codUsuario)
        {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @" SELECT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Cantidad]
                                  ,pra.[FechaRegistro]
                                  ,pra.[FechaModificacion]
                                  ,pra.[Estado]
                                  ,al.Nombre as NombreAlmacen
                                  ,sa.Nombre as NombreSala
                                  ,rep.Nombre as NombrePiezaRepuesto
                                  ,sa.[CodSala] as CodSala
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              INNER JOIN MI_Almacen al ON  pra.[CodAlmacen]=al.CodAlmacen
                              INNER JOIN Sala sa ON  sa.CodSala=al.CodSala
                              INNER JOIN MI_Repuesto rep ON  rep.CodRepuesto=pra.CodpiezaRepuesto
                              INNER JOIN MI_AlmacenUsuario au  ON pra.CodAlmacen = au.CodAlmacen
                              WHERE pra.Estado=1 AND pra.CodPiezaRepuesto=@pCodPiezaRepuesto AND au.CodUsuario=@pCodUsuario";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuesto", codPiezaRepuesto);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new MI_PiezaRepuestoAlmacenEntidad
                            {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreAlmacen = ManejoNulos.ManageNullStr(dr["NombreAlmacen"]),
                                NombrePiezaRepuesto = ManejoNulos.ManageNullStr(dr["NombrePiezaRepuesto"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreTipo = "Repuesto"
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
        public List<MI_PiezaRepuestoAlmacenEntidad> GetPiezaRepuestoAlmacenAjenoxCodPiezaRepuesto(int codPiezaRepuesto,int codUsuario)
        {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @" SELECT DISTINCT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Cantidad]
                                  ,pra.[FechaRegistro]
                                  ,pra.[FechaModificacion]
                                  ,pra.[Estado]
                                  ,al.Nombre as NombreAlmacen
                                  ,sa.Nombre as NombreSala
                                  ,rep.Nombre as NombrePiezaRepuesto
                                  ,sa.[CodSala] as CodSala
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              INNER JOIN MI_Almacen al ON  pra.[CodAlmacen]=al.CodAlmacen
                              INNER JOIN Sala sa ON  sa.CodSala=al.CodSala
                              INNER JOIN MI_Repuesto rep ON  rep.CodRepuesto=pra.CodpiezaRepuesto
                              LEFT JOIN MI_AlmacenUsuario au  ON pra.CodAlmacen = au.CodAlmacen
                              WHERE pra.Estado=1 AND pra.CodPiezaRepuesto=@pCodPiezaRepuesto AND au.CodUsuario=@pCodUsuario";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuesto", codPiezaRepuesto);
                    query.Parameters.AddWithValue("@pCodUsuario", codUsuario);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new MI_PiezaRepuestoAlmacenEntidad
                            {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreAlmacen = ManejoNulos.ManageNullStr(dr["NombreAlmacen"]),
                                NombrePiezaRepuesto = ManejoNulos.ManageNullStr(dr["NombrePiezaRepuesto"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreTipo = "Repuesto"
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


        public List<MI_PiezaRepuestoAlmacenEntidad> GetPiezaRepuestoAlmacenTodoxCodPiezaRepuesto(int codPiezaRepuesto)
        {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @"  SELECT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Cantidad]
                                  ,pra.[FechaRegistro]
                                  ,pra.[FechaModificacion]
                                  ,pra.[Estado]
                                  ,al.Nombre as NombreAlmacen
                                  ,sa.Nombre as NombreSala
                                  ,rep.Nombre as NombrePiezaRepuesto
                                  ,sa.[CodSala] as CodSala
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              INNER JOIN MI_Almacen al ON  pra.[CodAlmacen]=al.CodAlmacen
                              INNER JOIN Sala sa ON  sa.CodSala=al.CodSala
                              INNER JOIN MI_Repuesto rep ON  rep.CodRepuesto=pra.CodpiezaRepuesto
                              WHERE pra.Estado=1 AND pra.CodPiezaRepuesto=@pCodPiezaRepuesto";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuesto", codPiezaRepuesto);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new MI_PiezaRepuestoAlmacenEntidad
                            {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreAlmacen = ManejoNulos.ManageNullStr(dr["NombreAlmacen"]),
                                NombrePiezaRepuesto = ManejoNulos.ManageNullStr(dr["NombrePiezaRepuesto"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreTipo = "Repuesto"
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

        public MI_PiezaRepuestoAlmacenEntidad GetCodPiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen) {
            MI_PiezaRepuestoAlmacenEntidad item = new MI_PiezaRepuestoAlmacenEntidad();
            string consulta = @" SELECT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Cantidad]
                                  ,pra.[FechaRegistro]
                                  ,pra.[FechaModificacion]
                                  ,pra.[Estado]
                                  ,al.CodSala as CodSala
                                  ,al.Nombre as NombreAlmacen
                                  ,sa.Nombre as NombreSala
                                  ,rep.Nombre as NombrePiezaRepuesto
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              INNER JOIN MI_Almacen al ON  pra.[CodAlmacen]=al.CodAlmacen
                              INNER JOIN Sala sa ON  sa.CodSala=al.CodSala
                              INNER JOIN MI_Repuesto rep ON  rep.CodRepuesto=pra.CodpiezaRepuesto
                              WHERE pra.[CodPiezaRepuestoAlmacen]=@pCodPiezaRepuestoAlmacen ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuestoAlmacen", codPiezaRepuestoAlmacen);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]);
                                item.CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]);
                                item.CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]);
                                item.CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]);
                                item.Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.NombreAlmacen = ManejoNulos.ManageNullStr(dr["NombreAlmacen"]);
                                item.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                                item.NombrePiezaRepuesto = ManejoNulos.ManageNullStr(dr["NombrePiezaRepuesto"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarPiezaRepuestoAlmacen(MI_PiezaRepuestoAlmacenEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_PiezaRepuestoAlmacen] ([CodPiezaRepuesto],[CodTipo],[CodAlmacen],[Cantidad],CantidadPendiente,[FechaRegistro],[FechaModificacion],[Estado])
                                OUTPUT Inserted.CodPiezaRepuestoAlmacen   
	                            values (@pCodPiezaRepuesto, @pCodTipo, @pCodAlmacen, @pCantidad,@pCantidadPendiente, @pFechaRegistro, @pFechaModificacion, @pEstado)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodPiezaRepuesto));
                    query.Parameters.AddWithValue("@pCodTipo", ManejoNulos.ManageNullInteger(Entidad.CodTipo));
                    query.Parameters.AddWithValue("@pCodAlmacen", ManejoNulos.ManageNullInteger(Entidad.CodAlmacen));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(Entidad.Cantidad));
                    query.Parameters.AddWithValue("@pCantidadPendiente", ManejoNulos.ManageNullInteger(0));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarPiezaRepuestoAlmacen(MI_PiezaRepuestoAlmacenEntidad Entidad) {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_PiezaRepuestoAlmacen] SET 
                                CodPiezaRepuesto = @pCodPiezaRepuesto, 
                                CodTipo = @pCodTipo, 
                                CodAlmacen = @pCodAlmacen, 
                                Cantidad = @pCantidad, 
                                FechaModificacion  = @pFechaModificacion,
                                Estado  = @pEstado 
                                WHERE  CodPiezaRepuestoAlmacen = @pCodPiezaRepuestoAlmacen";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodPiezaRepuesto));
                    query.Parameters.AddWithValue("@pCodTipo", ManejoNulos.ManageNullInteger(Entidad.CodTipo));
                    query.Parameters.AddWithValue("@pCodAlmacen", ManejoNulos.ManageNullInteger(Entidad.CodAlmacen));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullDouble(Entidad.Cantidad));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodPiezaRepuestoAlmacen", ManejoNulos.ManageNullInteger(Entidad.CodPiezaRepuestoAlmacen));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarPiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen) {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_PiezaRepuestoAlmacen] 
                                WHERE CodPiezaRepuestoAlmacen  = @pCodPiezaRepuestoAlmacen";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuestoAlmacen", ManejoNulos.ManageNullInteger(codPiezaRepuestoAlmacen));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool RevisarExistenciaPiezaRepuestoAlmacen(MI_PiezaRepuestoAlmacenEntidad Entidad)
        {
            bool existe = false;

            string consulta = @" SELECT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Estado]
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              WHERE pra.CodPiezaRepuesto=@pCodPiezaRepuesto AND pra.CodTipo=@pCodTipo AND pra.CodAlmacen=@pCodAlmacen ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuesto", Entidad.CodPiezaRepuesto);
                    query.Parameters.AddWithValue("@pCodTipo", Entidad.CodTipo);
                    query.Parameters.AddWithValue("@pCodAlmacen", Entidad.CodAlmacen);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                existe = true;
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return existe;

        }

        public bool EditarCantidadPiezaRepuestoAlmacen(MI_PiezaRepuestoAlmacenEntidad Entidad) {
            bool respuesta = false;

            //if(Entidad.Cantidad <= 0) { Entidad.Estado = 0; } else { Entidad.Estado = 1; };

            string consulta = @" UPDATE [MI_PiezaRepuestoAlmacen] SET 
                                Cantidad = @pCantidad, 
                                FechaModificacion  = @pFechaModificacion,
                                Estado  = @pEstado 
                                WHERE  CodPiezaRepuestoAlmacen = @pCodPiezaRepuestoAlmacen";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(Entidad.Cantidad));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodPiezaRepuestoAlmacen", ManejoNulos.ManageNullInteger(Entidad.CodPiezaRepuestoAlmacen));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxTipo(int codTipo) {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @" SELECT [CodPiezaRepuestoAlmacen]
                                  ,[CodPiezaRepuesto]
                                  ,[CodTipo]
                                  ,[CodAlmacen]
                                  ,[Cantidad]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[Estado]
                              FROM [MI_PiezaRepuestoAlmacen] WHERE CodTipo=@pCodTipo";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodTipo", codTipo);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_PiezaRepuestoAlmacenEntidad {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxTipoxAlmacen(string codTipo,int codAlmacen) {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @" SELECT [CodPiezaRepuestoAlmacen]
                                  ,[CodPiezaRepuesto]
                                  ,[CodTipo]
                                  ,[CodAlmacen]
                                  ,[Cantidad]
                                  ,[FechaRegistro]
                                  ,[FechaModificacion]
                                  ,[Estado]
                              FROM [MI_PiezaRepuestoAlmacen] WHERE CodTipo IN ("+codTipo+") AND CodAlmacen=@pCodAlmacen";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@pCodTipo", codTipo);
                    query.Parameters.AddWithValue("@pCodAlmacen", codAlmacen);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_PiezaRepuestoAlmacenEntidad {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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


        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxPiezaxAlmacen(int codAlmacen) {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @" SELECT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Cantidad]
                                  ,pra.[FechaRegistro]
                                  ,pra.[FechaModificacion]
                                  ,pra.[Estado]
                                  ,al.Nombre as NombreAlmacen
                                  ,sa.Nombre as NombreSala
                                  ,pie.Nombre as NombrePiezaRepuesto
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              INNER JOIN MI_Almacen al ON  pra.[CodAlmacen]=al.CodAlmacen
                              INNER JOIN Sala sa ON  sa.CodSala=al.CodSala
                              INNER JOIN MI_Pieza pie ON  pie.CodPieza=pra.CodpiezaRepuesto
                              WHERE pra.CodTipo=1 AND pra.CodAlmacen=@pCodAlmacen";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@pCodTipo", codTipo);
                    query.Parameters.AddWithValue("@pCodAlmacen", codAlmacen);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_PiezaRepuestoAlmacenEntidad {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreAlmacen = ManejoNulos.ManageNullStr(dr["NombreAlmacen"]),
                                NombrePiezaRepuesto = ManejoNulos.ManageNullStr(dr["NombrePiezaRepuesto"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreTipo = "Pieza"
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



        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxRepuestoxAlmacen(int codAlmacen) {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @" SELECT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Cantidad]
                                  ,pra.[FechaRegistro]
                                  ,pra.[FechaModificacion]
                                  ,pra.[Estado]
                                  ,al.Nombre as NombreAlmacen
                                  ,sa.Nombre as NombreSala
                                  ,rep.Nombre as NombrePiezaRepuesto
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              INNER JOIN MI_Almacen al ON  pra.[CodAlmacen]=al.CodAlmacen
                              INNER JOIN Sala sa ON  sa.CodSala=al.CodSala
                              INNER JOIN MI_Repuesto rep ON  rep.CodRepuesto=pra.CodpiezaRepuesto
                              WHERE pra.CodTipo=2 AND pra.CodAlmacen=@pCodAlmacen";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@pCodTipo", codTipo);
                    query.Parameters.AddWithValue("@pCodAlmacen", codAlmacen);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_PiezaRepuestoAlmacenEntidad {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreAlmacen = ManejoNulos.ManageNullStr(dr["NombreAlmacen"]),
                                NombrePiezaRepuesto = ManejoNulos.ManageNullStr(dr["NombrePiezaRepuesto"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreTipo = "Repuesto"
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

        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxAlmacen(int codAlmacen) {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @" SELECT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Cantidad]
                                  ,pra.[FechaRegistro]
                                  ,pra.[FechaModificacion]
                                  ,pra.[Estado]
                                  ,al.Nombre as NombreAlmacen
                                  ,sa.Nombre as NombreSala
                                  , CASE pra.CodTipo
									  WHEN  1
									  THEN
											pie.Nombre
									  ELSE 
											rep.Nombre
									  END AS NombrePiezaRepuesto
                                  , CASE pra.CodTipo
									  WHEN  1
									  THEN
											'Pieza'
									  ELSE 
											'Repuesto'
									  END AS NombreTipo
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              INNER JOIN MI_Almacen al ON  pra.[CodAlmacen]=al.CodAlmacen
                              INNER JOIN Sala sa ON  sa.CodSala=al.CodSala
                              INNER JOIN MI_Pieza pie ON  pie.CodPieza=pra.CodpiezaRepuesto
                              INNER JOIN MI_Repuesto rep ON  rep.CodRepuesto=pra.CodpiezaRepuesto
                              WHERE pra.CodAlmacen=@pCodAlmacen";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@pCodTipo", codTipo);
                    query.Parameters.AddWithValue("@pCodAlmacen", codAlmacen);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_PiezaRepuestoAlmacenEntidad {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreAlmacen = ManejoNulos.ManageNullStr(dr["NombreAlmacen"]),
                                NombrePiezaRepuesto = ManejoNulos.ManageNullStr(dr["NombrePiezaRepuesto"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreTipo = ManejoNulos.ManageNullStr(dr["NombreTipo"]),
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


        public List<MI_PiezaRepuestoAlmacenEntidad> GetAllPiezaRepuestoAlmacenxRepuestoActive()
        {
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            string consulta = @" SELECT pra.[CodPiezaRepuestoAlmacen]
                                  ,pra.[CodPiezaRepuesto]
                                  ,pra.[CodTipo]
                                  ,pra.[CodAlmacen]
                                  ,pra.[Cantidad]
                                  ,pra.[FechaRegistro]
                                  ,pra.[FechaModificacion]
                                  ,pra.[Estado]
                                  ,al.Nombre as NombreAlmacen
                                  ,sa.Nombre as NombreSala
                                  ,rep.Nombre as NombrePiezaRepuesto
                              FROM [MI_PiezaRepuestoAlmacen]  pra
                              INNER JOIN MI_Almacen al ON  pra.[CodAlmacen]=al.CodAlmacen
                              INNER JOIN Sala sa ON  sa.CodSala=al.CodSala
                              INNER JOIN MI_Repuesto rep ON  rep.CodRepuesto=pra.CodpiezaRepuesto
                              WHERE pra.Estado=1 ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new MI_PiezaRepuestoAlmacenEntidad
                            {
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                CodPiezaRepuesto = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuesto"]),
                                CodTipo = ManejoNulos.ManageNullInteger(dr["CodTipo"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreAlmacen = ManejoNulos.ManageNullStr(dr["NombreAlmacen"]),
                                NombrePiezaRepuesto = ManejoNulos.ManageNullStr(dr["NombrePiezaRepuesto"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreTipo = "Repuesto"
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



        public bool AgregarCantidadPendientePiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen,int cantPendiente)
        {

            bool respuesta = false;
            string consulta = @"  UPDATE [MI_PiezaRepuestoAlmacen] 
                                  SET [CantidadPendiente]=@pCantidadPendiente,
                                  [Cantidad]=[Cantidad]-@pCantidadPendiente
                                  WHERE [CodPiezaRepuestoAlmacen]=@pCodPiezaRepuestoAlmacen";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuestoAlmacen", ManejoNulos.ManageNullInteger(codPiezaRepuestoAlmacen));
                    query.Parameters.AddWithValue("@pCantidadPendiente", ManejoNulos.ManageNullInteger(cantPendiente));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }


        public bool AceptarDescontarCantidadPendientePiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen, int cantPendiente)
        {

            bool respuesta = false;
            string consulta = @"  UPDATE [MI_PiezaRepuestoAlmacen] 
                                  SET [CantidadPendiente]=CantidadPendiente-@pCantidadPendiente
                                  WHERE [CodPiezaRepuestoAlmacen]=@pCodPiezaRepuestoAlmacen";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuestoAlmacen", ManejoNulos.ManageNullInteger(codPiezaRepuestoAlmacen));
                    query.Parameters.AddWithValue("@pCantidadPendiente", ManejoNulos.ManageNullInteger(cantPendiente));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }


        public bool AceptarAgregarCantidadPendientePiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen, int cantPendiente)
        {

            bool respuesta = false;
            string consulta = @"  UPDATE [MI_PiezaRepuestoAlmacen] 
                                  SET [Cantidad]=[Cantidad]+@pCantidadPendiente
                                  WHERE [CodPiezaRepuestoAlmacen]=@pCodPiezaRepuestoAlmacen";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuestoAlmacen", ManejoNulos.ManageNullInteger(codPiezaRepuestoAlmacen));
                    query.Parameters.AddWithValue("@pCantidadPendiente", ManejoNulos.ManageNullInteger(cantPendiente));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool RechazarCantidadPendientePiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen, int cantPendiente)
        {

            bool respuesta = false;
            string consulta = @"  UPDATE [MI_PiezaRepuestoAlmacen] 
                                  SET [CantidadPendiente]=CantidadPendiente-@pCantidadPendiente,
                                  [Cantidad]=[Cantidad]+@pCantidadPendiente
                                  WHERE [CodPiezaRepuestoAlmacen]=@pCodPiezaRepuestoAlmacen";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuestoAlmacen", ManejoNulos.ManageNullInteger(codPiezaRepuestoAlmacen));
                    query.Parameters.AddWithValue("@pCantidadPendiente", ManejoNulos.ManageNullInteger(cantPendiente));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool DescontarCantidadPiezaRepuestoAlmacen(int codPiezaRepuestoAlmacen, int cant)
        {

            bool respuesta = false;
            string consulta = @"  UPDATE [MI_PiezaRepuestoAlmacen] 
                                  SET [Cantidad]=Cantidad-@pCantidad
                                  WHERE [CodPiezaRepuestoAlmacen]=@pCodPiezaRepuestoAlmacen";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodPiezaRepuestoAlmacen", ManejoNulos.ManageNullInteger(codPiezaRepuestoAlmacen));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(cant));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

    }
}
