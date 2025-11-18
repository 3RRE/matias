using CapaEntidad.MaquinasInoperativas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas
{
    public class MI_TraspasoRepuestoAlmacenDAL
    {

        string conexion = string.Empty;
        public MI_TraspasoRepuestoAlmacenDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MI_TraspasoRepuestoAlmacenEntidad> GetAllTraspasoRepuestoAlmacen()
        {
            List<MI_TraspasoRepuestoAlmacenEntidad> lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();
            string consulta = @" SELECT [CodTraspasoRepuestoAlmacen]
                                  ,p.[CodMaquinaInoperativa]
                                  ,p.[CodAlmacenOrigen]
                                  ,p.[CodAlmacenDestino]
                                  ,p.[CodRepuesto]
                                  ,p.[CodPiezaRepuestoAlmacen]
                                  ,p.[Cantidad]
                                  ,p.[Estado]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuarioRemitente]
                                  ,p.[CodUsuarioDestinatario]
                                  ,a.[Nombre] as NombreAlmacenOrigen
                                  ,b.[Nombre] as NombreAlmacenDestino
                                  ,c.[Nombre] as NombreSala
                                  ,d.[Nombre] as NombreRepuesto
                              FROM [MI_TraspasoRepuestoAlmacen] p
							  INNER JOIN MI_Almacen a
							  ON a.CodAlmacen = p.CodAlmacenOrigen
							  INNER JOIN MI_Almacen b
							  ON b.CodAlmacen = p.CodAlmacenDestino
							  INNER JOIN Sala c
							  ON c.CodSala = b.CodSala
							  INNER JOIN MI_Repuesto d
							  ON d.CodRepuesto = p.CodRepuesto ";
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
                            var item = new MI_TraspasoRepuestoAlmacenEntidad
                            {
                                CodTraspasoRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodTraspasoRepuestoAlmacen"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodAlmacenOrigen = ManejoNulos.ManageNullInteger(dr["CodAlmacenOrigen"]),
                                CodAlmacenDestino = ManejoNulos.ManageNullInteger(dr["CodAlmacenDestino"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuarioRemitente = ManejoNulos.ManageNullInteger(dr["CodUsuarioRemitente"]),
                                CodUsuarioDestinatario = ManejoNulos.ManageNullInteger(dr["CodUsuarioDestinatario"]),
                                NombreAlmacenOrigen = ManejoNulos.ManageNullStr(dr["NombreAlmacenOrigen"]),
                                NombreAlmacenDestino = ManejoNulos.ManageNullStr(dr["NombreAlmacenDestino"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreRepuesto = ManejoNulos.ManageNullStr(dr["NombreRepuesto"]),
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
        public List<MI_TraspasoRepuestoAlmacenEntidad> GetAllTraspasoRepuestoAlmacenxMaquinaInoperativa(int cod)
        {
            List<MI_TraspasoRepuestoAlmacenEntidad> lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();
            string consulta = @" SELECT [CodTraspasoRepuestoAlmacen]
                                  ,p.[CodMaquinaInoperativa]
                                  ,p.[CodAlmacenOrigen]
                                  ,p.[CodAlmacenDestino]
                                  ,p.[CodRepuesto]
                                  ,p.[CodPiezaRepuestoAlmacen]
                                  ,p.[Cantidad]
                                  ,p.[Estado]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuarioRemitente]
                                  ,p.[CodUsuarioDestinatario]
                                  ,a.[Nombre] as NombreAlmacenOrigen
                                  ,b.[Nombre] as NombreAlmacenDestino
                                  ,c.[Nombre] as NombreSala
                                  ,d.[Nombre] as NombreRepuesto
                              FROM [MI_TraspasoRepuestoAlmacen] p
							  INNER JOIN MI_Almacen a
							  ON a.CodAlmacen = p.CodAlmacenOrigen
							  INNER JOIN MI_Almacen b
							  ON b.CodAlmacen = p.CodAlmacenDestino
							  INNER JOIN Sala c
							  ON c.CodSala = b.CodSala
							  INNER JOIN MI_Repuesto d
							  ON d.CodRepuesto = p.CodRepuesto 
                              WHERE p.CodMaquinaInoperativa = @pCodMaquinaInoperativa  ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa",cod);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new MI_TraspasoRepuestoAlmacenEntidad
                            {
                                CodTraspasoRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodTraspasoRepuestoAlmacen"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodAlmacenOrigen = ManejoNulos.ManageNullInteger(dr["CodAlmacenOrigen"]),
                                CodAlmacenDestino = ManejoNulos.ManageNullInteger(dr["CodAlmacenDestino"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuarioRemitente = ManejoNulos.ManageNullInteger(dr["CodUsuarioRemitente"]),
                                CodUsuarioDestinatario = ManejoNulos.ManageNullInteger(dr["CodUsuarioDestinatario"]),
                                NombreAlmacenOrigen = ManejoNulos.ManageNullStr(dr["NombreAlmacenOrigen"]),
                                NombreAlmacenDestino = ManejoNulos.ManageNullStr(dr["NombreAlmacenDestino"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreRepuesto = ManejoNulos.ManageNullStr(dr["NombreRepuesto"]),
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

        public List<MI_TraspasoRepuestoAlmacenEntidad> GetAllTraspasoRepuestoAlmacenxMaquinaInoperativaSinAlmacenes(int cod) {
            List<MI_TraspasoRepuestoAlmacenEntidad> lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();
            string consulta = @" SELECT [CodTraspasoRepuestoAlmacen]
                                  ,p.[CodMaquinaInoperativa]
                                  ,p.[CodAlmacenOrigen]
                                  ,p.[CodAlmacenDestino]
                                  ,p.[CodRepuesto]
                                  ,p.[CodPiezaRepuestoAlmacen]
                                  ,p.[Cantidad]
                                  ,p.[Estado]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuarioRemitente]
                                  ,p.[CodUsuarioDestinatario]
                                  ,' - ' as NombreAlmacenOrigen
                                  ,' - ' as NombreAlmacenDestino
                                  ,' - ' as NombreSala
                                  ,d.[Nombre] as NombreRepuesto
                              FROM [MI_TraspasoRepuestoAlmacen] p
							  INNER JOIN MI_Repuesto d
							  ON d.CodRepuesto = p.CodRepuesto 
                              WHERE p.CodMaquinaInoperativa = @pCodMaquinaInoperativa   ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", cod);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_TraspasoRepuestoAlmacenEntidad {
                                CodTraspasoRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodTraspasoRepuestoAlmacen"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodAlmacenOrigen = ManejoNulos.ManageNullInteger(dr["CodAlmacenOrigen"]),
                                CodAlmacenDestino = ManejoNulos.ManageNullInteger(dr["CodAlmacenDestino"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuarioRemitente = ManejoNulos.ManageNullInteger(dr["CodUsuarioRemitente"]),
                                CodUsuarioDestinatario = ManejoNulos.ManageNullInteger(dr["CodUsuarioDestinatario"]),
                                NombreAlmacenOrigen = ManejoNulos.ManageNullStr(dr["NombreAlmacenOrigen"]),
                                NombreAlmacenDestino = ManejoNulos.ManageNullStr(dr["NombreAlmacenDestino"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreRepuesto = ManejoNulos.ManageNullStr(dr["NombreRepuesto"]),
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

        public List<MI_TraspasoRepuestoAlmacenEntidad> GetAllActiveTraspasoRepuestoAlmacen()
        {
            List<MI_TraspasoRepuestoAlmacenEntidad> lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();
            string consulta = @"  SELECT [CodTraspasoRepuestoAlmacen]
                                  ,p.[CodMaquinaInoperativa]
                                  ,p.[CodAlmacenOrigen]
                                  ,p.[CodAlmacenDestino]
                                  ,p.[CodRepuesto]
                                  ,p.[CodPiezaRepuestoAlmacen]
                                  ,p.[Cantidad]
                                  ,p.[Estado]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuarioRemitente]
                                  ,p.[CodUsuarioDestinatario]
                                  ,a.[Nombre] as NombreAlmacenOrigen
                                  ,b.[Nombre] as NombreAlmacenDestino
                                  ,c.[Nombre] as NombreSala
                              FROM [MI_TraspasoRepuestoAlmacen] p
							  INNER JOIN MI_Almacen a
							  ON a.CodAlmacen = p.CodAlmacenOrigen
							  INNER JOIN MI_Almacen b
							  ON b.CodAlmacen = p.CodAlmacenDestino
							  INNER JOIN Sala c
							  ON c.CodSala = b.CodSala 
							  WHERE p.[Estado]=0 ";
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
                            var item = new MI_TraspasoRepuestoAlmacenEntidad
                            {
                                CodTraspasoRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodTraspasoRepuestoAlmacen"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodAlmacenOrigen = ManejoNulos.ManageNullInteger(dr["CodAlmacenOrigen"]),
                                CodAlmacenDestino = ManejoNulos.ManageNullInteger(dr["CodAlmacenDestino"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuarioRemitente = ManejoNulos.ManageNullInteger(dr["CodUsuarioRemitente"]),
                                CodUsuarioDestinatario = ManejoNulos.ManageNullInteger(dr["CodUsuarioDestinatario"]),
                                NombreAlmacenOrigen = ManejoNulos.ManageNullStr(dr["NombreAlmacenOrigen"]),
                                NombreAlmacenDestino = ManejoNulos.ManageNullStr(dr["NombreAlmacenDestino"]),
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
        public List<MI_TraspasoRepuestoAlmacenEntidad> GetAllInactiveTraspasoRepuestoAlmacen()
        {
            List<MI_TraspasoRepuestoAlmacenEntidad> lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();
            string consulta = @" SELECT [CodTraspasoRepuestoAlmacen]
                                  ,p.[CodMaquinaInoperativa]
                                  ,p.[CodAlmacenOrigen]
                                  ,p.[CodAlmacenDestino]
                                  ,p.[CodRepuesto]
                                  ,p.[CodPiezaRepuestoAlmacen]
                                  ,p.[Cantidad]
                                  ,p.[Estado]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuarioRemitente]
                                  ,p.[CodUsuarioDestinatario]
                                  ,a.[Nombre] as NombreAlmacenOrigen
                                  ,b.[Nombre] as NombreAlmacenDestino
                                  ,c.[Nombre] as NombreSala
                              FROM [MI_TraspasoRepuestoAlmacen] p
							  INNER JOIN MI_Almacen a
							  ON a.CodAlmacen = p.CodAlmacenOrigen
							  INNER JOIN MI_Almacen b
							  ON b.CodAlmacen = p.CodAlmacenDestino
							  INNER JOIN Sala c
							  ON c.CodSala = b.CodSala 
							  WHERE p.[Estado]!=0 ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@pCodCategoriaRepuesto", cod);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new MI_TraspasoRepuestoAlmacenEntidad
                            {
                                CodTraspasoRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodTraspasoRepuestoAlmacen"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodAlmacenOrigen = ManejoNulos.ManageNullInteger(dr["CodAlmacenOrigen"]),
                                CodAlmacenDestino = ManejoNulos.ManageNullInteger(dr["CodAlmacenDestino"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuarioRemitente = ManejoNulos.ManageNullInteger(dr["CodUsuarioRemitente"]),
                                CodUsuarioDestinatario = ManejoNulos.ManageNullInteger(dr["CodUsuarioDestinatario"]),
                                NombreAlmacenOrigen = ManejoNulos.ManageNullStr(dr["NombreAlmacenOrigen"]),
                                NombreAlmacenDestino = ManejoNulos.ManageNullStr(dr["NombreAlmacenDestino"]),
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
        public MI_TraspasoRepuestoAlmacenEntidad GetCodTraspasoRepuestoAlmacen(int cod)
        {
            MI_TraspasoRepuestoAlmacenEntidad item = new MI_TraspasoRepuestoAlmacenEntidad();
            string consulta = @"  SELECT [CodTraspasoRepuestoAlmacen]
                                  ,p.[CodMaquinaInoperativa]
                                  ,p.[CodAlmacenOrigen]
                                  ,p.[CodAlmacenDestino]
                                  ,p.[CodRepuesto]
                                  ,p.[CodPiezaRepuestoAlmacen]
                                  ,p.[Cantidad]
                                  ,p.[Estado]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuarioRemitente]
                                  ,p.[CodUsuarioDestinatario]
                                  ,a.[Nombre] as NombreAlmacenOrigen
                                  ,b.[Nombre] as NombreAlmacenDestino
                                  ,c.[Nombre] as NombreSala
                              FROM [MI_TraspasoRepuestoAlmacen] p
							  INNER JOIN MI_Almacen a
							  ON a.CodAlmacen = p.CodAlmacenOrigen
							  INNER JOIN MI_Almacen b
							  ON b.CodAlmacen = p.CodAlmacenDestino
							  INNER JOIN Sala c
							  ON c.CodSala = b.CodSala 
							  WHERE [CodTraspasoRepuestoAlmacen]=@pCodTraspasoRepuestoAlmacen";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodTraspasoRepuestoAlmacen", cod);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.CodTraspasoRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodTraspasoRepuestoAlmacen"]);
                                item.CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]);
                                item.CodAlmacenOrigen = ManejoNulos.ManageNullInteger(dr["CodAlmacenOrigen"]);
                                item.CodAlmacenDestino = ManejoNulos.ManageNullInteger(dr["CodAlmacenDestino"]);
                                item.CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]);
                                item.CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]);
                                item.Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.CodUsuarioRemitente = ManejoNulos.ManageNullInteger(dr["CodUsuarioRemitente"]);
                                item.CodUsuarioDestinatario = ManejoNulos.ManageNullInteger(dr["CodUsuarioDestinatario"]);
                                item.NombreAlmacenOrigen = ManejoNulos.ManageNullStr(dr["NombreAlmacenOrigen"]);
                                item.NombreAlmacenDestino = ManejoNulos.ManageNullStr(dr["NombreAlmacenDestino"]);
                                item.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarTraspasoRepuestoAlmacen(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            int IdInsertado = 0;
            string consulta = @" INSERT INTO [MI_TraspasoRepuestoAlmacen]
                                ([CodMaquinaInoperativa]
                                           ,[CodAlmacenOrigen]
                                           ,[CodAlmacenDestino]
                                           ,[CodRepuesto]
                                           ,[CodPiezaRepuestoAlmacen]
                                           ,[Cantidad]
                                           ,[Estado]
                                           ,[FechaRegistro]
                                           ,[FechaModificacion]
                                           ,[CodUsuarioRemitente]
                                           ,[CodUsuarioDestinatario])
                                OUTPUT Inserted.CodTraspasoRepuestoAlmacen
	                            values (@pCodMaquinaInoperativa, @pCodAlmacenOrigen, @pCodAlmacenDestino,@pCodRepuesto,@pCodPiezaRepuestoAlmacen,@pCantidad,@pEstado, @pFechaRegistro ,@pFechaModificacion ,@pCodUsuarioRemitente ,@pCodUsuarioDestinatario)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pCodAlmacenOrigen", ManejoNulos.ManageNullInteger(Entidad.CodAlmacenOrigen));
                    query.Parameters.AddWithValue("@pCodAlmacenDestino", ManejoNulos.ManageNullInteger(Entidad.CodAlmacenDestino));
                    query.Parameters.AddWithValue("@pCodRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodRepuesto));
                    query.Parameters.AddWithValue("@pCodPiezaRepuestoAlmacen", ManejoNulos.ManageNullInteger(Entidad.CodPiezaRepuestoAlmacen));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(Entidad.Cantidad));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuarioRemitente", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioRemitente));
                    query.Parameters.AddWithValue("@pCodUsuarioDestinatario", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioDestinatario));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarTraspasoRepuestoAlmacen(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_TraspasoRepuestoAlmacen] SET 
                                CodMaquinaInoperativa = @pCodMaquinaInoperativa, 
                                CodAlmacenOrigen = @pCodAlmacenOrigen, 
                                CodAlmacenDestino = @pCodAlmacenDestino, 
                                CodRepuesto  = @pCodRepuesto,
                                CodPiezaRepuestoAlmacen  = @pCodPiezaRepuestoAlmacen,
                                Cantidad  = @pCantidad, 
                                Estado  = @pEstado, 
                                FechaRegistro  = @pFechaRegistro, 
                                FechaModificacion  = @pFechaModificacion, 
                                CodUsuarioRemitente  = @pCodUsuarioRemitente, 
                                CodUsuarioDestinatario  = @pCodUsuarioDestinatario
                                WHERE  CodTraspasoRepuestoAlmacen = @pCodTraspasoRepuestoAlmacen";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(Entidad.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pCodAlmacenOrigen", ManejoNulos.ManageNullInteger(Entidad.CodAlmacenOrigen));
                    query.Parameters.AddWithValue("@pCodAlmacenDestino", ManejoNulos.ManageNullInteger(Entidad.CodAlmacenDestino));
                    query.Parameters.AddWithValue("@pCodRepuesto", ManejoNulos.ManageNullInteger(Entidad.CodRepuesto));
                    query.Parameters.AddWithValue("@PCodPiezaRepuestoAlmacen", ManejoNulos.ManageNullInteger(Entidad.CodPiezaRepuestoAlmacen));
                    query.Parameters.AddWithValue("@pCantidad", ManejoNulos.ManageNullInteger(Entidad.Cantidad));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuarioRemitente", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioRemitente));
                    query.Parameters.AddWithValue("@pCodUsuarioDestinatario", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioDestinatario));
                    query.Parameters.AddWithValue("@pCodTraspasoRepuestoAlmacen", ManejoNulos.ManageNullInteger(Entidad.CodRepuesto));
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
        public bool EliminarTraspasoRepuestoAlmacen(int cod)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM [MI_TraspasoRepuestoAlmacen] 
                                WHERE CodTraspasoRepuestoAlmacen  = @pCodTraspasoRepuestoAlmacen";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodTraspasoRepuestoAlmacen", ManejoNulos.ManageNullInteger(cod));
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


        public bool AceptarTraspasoRepuestoAlmacen(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_TraspasoRepuestoAlmacen] SET 
                                Estado  = 1,
                                FechaModificacion  = @pFechaModificacion,
                                CodUsuarioDestinatario  = @pCodUsuarioDestinatario
                                WHERE  CodTraspasoRepuestoAlmacen = @pCodTraspasoRepuestoAlmacen";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodTraspasoRepuestoAlmacen", ManejoNulos.ManageNullInteger(Entidad.CodTraspasoRepuestoAlmacen));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuarioDestinatario", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioDestinatario));
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


        public bool RechazarTraspasoRepuestoAlmacen(MI_TraspasoRepuestoAlmacenEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @" UPDATE [MI_TraspasoRepuestoAlmacen] SET 
                                Estado  = 2,
                                FechaModificacion  = @pFechaModificacion,
                                CodUsuarioDestinatario  = @pCodUsuarioDestinatario
                                WHERE  CodTraspasoRepuestoAlmacen = @pCodTraspasoRepuestoAlmacen";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodTraspasoRepuestoAlmacen", ManejoNulos.ManageNullInteger(Entidad.CodTraspasoRepuestoAlmacen));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pCodUsuarioDestinatario", ManejoNulos.ManageNullInteger(Entidad.CodUsuarioDestinatario));
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


        public List<MI_TraspasoRepuestoAlmacenEntidad> GetAllTraspasoRepuestoAlmacenxCodMaquinaInoperativa(int CodMaquinaInoperativa, int Estado)
        {
            List<MI_TraspasoRepuestoAlmacenEntidad> lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();
            string consulta = @" SELECT [CodTraspasoRepuestoAlmacen]
                                  ,p.[CodMaquinaInoperativa]
                                  ,p.[CodAlmacenOrigen]
                                  ,p.[CodAlmacenDestino]
                                  ,p.[CodRepuesto]
                                  ,p.[CodPiezaRepuestoAlmacen]
                                  ,p.[Cantidad]
                                  ,p.[Estado]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuarioRemitente]
                                  ,p.[CodUsuarioDestinatario]
                                  ,a.[Nombre] as NombreAlmacenOrigen
                                  ,b.[Nombre] as NombreAlmacenDestino
                                  ,c.[Nombre] as NombreSala
                                  ,d.[Nombre] as NombreRepuesto
                              FROM [MI_TraspasoRepuestoAlmacen] p
							  INNER JOIN MI_Almacen a
							  ON a.CodAlmacen = p.CodAlmacenOrigen
							  INNER JOIN MI_Almacen b
							  ON b.CodAlmacen = p.CodAlmacenDestino
							  INNER JOIN Sala c
							  ON c.CodSala = b.CodSala
							  INNER JOIN MI_Repuesto d
							  ON d.CodRepuesto = p.CodRepuesto WHERE p.[CodMaquinaInoperativa]=@pCodMaquinaInoperativa AND  p.[Estado] =@pEstado ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Estado));


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new MI_TraspasoRepuestoAlmacenEntidad
                            {
                                CodTraspasoRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodTraspasoRepuestoAlmacen"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodAlmacenOrigen = ManejoNulos.ManageNullInteger(dr["CodAlmacenOrigen"]),
                                CodAlmacenDestino = ManejoNulos.ManageNullInteger(dr["CodAlmacenDestino"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuarioRemitente = ManejoNulos.ManageNullInteger(dr["CodUsuarioRemitente"]),
                                CodUsuarioDestinatario = ManejoNulos.ManageNullInteger(dr["CodUsuarioDestinatario"]),
                                NombreAlmacenOrigen = ManejoNulos.ManageNullStr(dr["NombreAlmacenOrigen"]),
                                NombreAlmacenDestino = ManejoNulos.ManageNullStr(dr["NombreAlmacenDestino"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreRepuesto = ManejoNulos.ManageNullStr(dr["NombreRepuesto"]),
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

        
        public List<MI_TraspasoRepuestoAlmacenEntidad> GetAllTraspasoRepuestoAlmacenxCodMaquinaInoperativaSinAlmacenes(int CodMaquinaInoperativa, int Estado)
        {
            List<MI_TraspasoRepuestoAlmacenEntidad> lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();
            string consulta = @"  SELECT [CodTraspasoRepuestoAlmacen]
                                  ,p.[CodMaquinaInoperativa]
                                  ,p.[CodAlmacenOrigen]
                                  ,p.[CodAlmacenDestino]
                                  ,p.[CodRepuesto]
                                  ,p.[CodPiezaRepuestoAlmacen]
                                  ,p.[Cantidad]
                                  ,p.[Estado]
                                  ,p.[FechaRegistro]
                                  ,p.[FechaModificacion]
                                  ,p.[CodUsuarioRemitente]
                                  ,p.[CodUsuarioDestinatario]
                              FROM [MI_TraspasoRepuestoAlmacen] p WHERE p.[CodMaquinaInoperativa]=@pCodMaquinaInoperativa AND  p.[Estado] =@pEstado ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Estado));


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new MI_TraspasoRepuestoAlmacenEntidad
                            {
                                CodTraspasoRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodTraspasoRepuestoAlmacen"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodAlmacenOrigen = ManejoNulos.ManageNullInteger(dr["CodAlmacenOrigen"]),
                                CodAlmacenDestino = ManejoNulos.ManageNullInteger(dr["CodAlmacenDestino"]),
                                CodRepuesto = ManejoNulos.ManageNullInteger(dr["CodRepuesto"]),
                                CodPiezaRepuestoAlmacen = ManejoNulos.ManageNullInteger(dr["CodPiezaRepuestoAlmacen"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                CodUsuarioRemitente = ManejoNulos.ManageNullInteger(dr["CodUsuarioRemitente"]),
                                CodUsuarioDestinatario = ManejoNulos.ManageNullInteger(dr["CodUsuarioDestinatario"]),
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


    }
}
