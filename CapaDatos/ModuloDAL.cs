using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class ModuloDAL
    {
        string _conexion = string.Empty;
        public ModuloDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ModuloEntidad> ModuloListarJson()
        {
            List<ModuloEntidad> lista = new List<ModuloEntidad>();

          string consulta = @"SELECT m.ModuloId,m.Descripcion,m.SistemaId,s.Descripcion NombreSistema,m.FechaRegistro,m.FechaModificacion,m.Estado
            from Modulo m
            join Sistema s on s.SistemaId = m.SistemaId";
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
                            var modulo = new ModuloEntidad
                            {
                                ModuloId = ManejoNulos.ManageNullInteger(dr["ModuloId"]),
                                SistemaID = ManejoNulos.ManageNullInteger(dr["SistemaID"]),
                                SistemaDescripcion = ManejoNulos.ManageNullStr(dr["NombreSistema"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"])
                            };

                            lista.Add(modulo);
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

        public bool ModuloActualizarJson(ModuloEntidad moduloEntidad)
        {
            bool respuesta = false;
            string consulta = @"update [Modulo] set 
                                   [SistemaId] = @p1
                                  ,[Descripcion] = @p2
                                  ,[FechaModificacion] = @p3
                                  ,[Estado] = @p4
                                    WHERE ModuloId = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", moduloEntidad.ModuloId);
                    query.Parameters.AddWithValue("@p1", moduloEntidad.SistemaID);
                    query.Parameters.AddWithValue("@p2", moduloEntidad.Descripcion);
                    query.Parameters.AddWithValue("@p3", moduloEntidad.FechaModificacion);
                    query.Parameters.AddWithValue("@p4", moduloEntidad.Estado);
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

        public ModuloEntidad ModuloIdObtenerJson(int moduloid)
        {
            ModuloEntidad modulo = new ModuloEntidad();
            string consulta = @"select  [ModuloId]
                              ,[SistemaId]
                              ,[Descripcion]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[Estado] from [Modulo]
	                            where ModuloId =@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", moduloid);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                modulo.ModuloId = ManejoNulos.ManageNullInteger(dr["ModuloId"]);
                                modulo.SistemaID = ManejoNulos.ManageNullInteger(dr["SistemaID"]);
                                modulo.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                modulo.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return modulo;
        }

        public bool ModuloGuardarJson(ModuloEntidad moduloEntidad)
        {
            bool respuesta = false;
            
            string consulta = @"INSERT INTO [Modulo]
           ([SistemaId],[Descripcion],[FechaRegistro],[FechaModificacion],[Estado]) VALUES(@p0, @p1, @p2, @p3, @p4)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(moduloEntidad.SistemaID));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(moduloEntidad.Descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(moduloEntidad.FechaRegistro.Date));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(moduloEntidad.FechaModificacion.Date));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(moduloEntidad.Estado));
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

        public List<ModuloEntidad> BuscarModuloSistema(int id)
        {
            List<ModuloEntidad> lista = new List<ModuloEntidad>();

            string consulta = @"select m.ModuloId,m.Descripcion,m.Estado
                                from modulo m
                                where m.SistemaId ="+id;
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
                            var modulo = new ModuloEntidad
                            {
                                ModuloId = ManejoNulos.ManageNullInteger(dr["ModuloId"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                            };

                            lista.Add(modulo);
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

    }
}
