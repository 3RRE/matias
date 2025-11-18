using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class SEG_CargoDAL
    {
        string _conexion = string.Empty;
        public SEG_CargoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<SEG_CargoEntidad> CargoListarJson()
        {
            List<SEG_CargoEntidad> lista = new List<SEG_CargoEntidad>();
            string consulta = @"SELECT [CargoID]
                                  ,[Descripcion]
                                  ,[Estado]
                              FROM [dbo].[SEG_Cargo]
                              where Estado=1
	                          order by CargoID Desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var cargo = new SEG_CargoEntidad
                            {
                                CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"].Trim()),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim())
                            };

                            lista.Add(cargo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }

        public List<SEG_CargoEntidad> CargoMantenimientoListarJson()
        {
            List<SEG_CargoEntidad> lista = new List<SEG_CargoEntidad>();
            string consulta = @"SELECT [CargoID]
                                  ,[Descripcion]
                                  ,[Estado]
                              FROM [dbo].[SEG_Cargo]
	                          order by CargoID Desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var cargo = new SEG_CargoEntidad
                            {
                                CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"].Trim()),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim()),
                            };

                            lista.Add(cargo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }


        public SEG_CargoEntidad GetCargoId(int cargoid)
        {
            SEG_CargoEntidad segCargo = new SEG_CargoEntidad();
            string consulta = @"SELECT [CargoID]
                                  ,[Descripcion]
                                  ,[Estado]
                              FROM [dbo].[SEG_Cargo]
	                       where CargoID=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", cargoid);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                segCargo.CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"]);
                                segCargo.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"].Trim());
                                segCargo.Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim());
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return segCargo;
        }

        public bool CargoGuardarJson(SEG_CargoEntidad varItem)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[SEG_Cargo] ([Descripcion],[Estado]) VALUES 
                                (@p0, @p1)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(varItem.Descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(varItem.Estado));
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public bool CargoActualizarJson(SEG_CargoEntidad cargo)
        {
            bool respuesta = false;
            string consulta = @"update SEG_Cargo set [Descripcion] = @p1
                                  ,[Estado] = @p2
                                    WHERE CargoID = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", cargo.CargoID);
                    query.Parameters.AddWithValue("@p1", cargo.Descripcion);
                    query.Parameters.AddWithValue("@p2", cargo.Estado);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }
    }
}
