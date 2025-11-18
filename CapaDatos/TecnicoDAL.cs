using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class TecnicoDAL
    {
        string _conexion = string.Empty;
        public TecnicoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool TecnicoGuardarJson(TecnicoEntidad tecnico)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[Tecnico]([EmpleadoId],[AreaTechId],[FechaRegistro],[NivelTecnicoId],[Estado])
            VALUES (@p0,@p1,@p2,@p3,@p4)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(tecnico.EmpleadoId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(tecnico.AreaTechId));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(tecnico.FechaRegistro));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(tecnico.NivelTecnicoId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManegeNullBool(true));
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

        public TecnicoEntidad TecnicoIdObtenerJson(int tecnicoId)
        {
            TecnicoEntidad tecnico = new TecnicoEntidad();
            string consulta = @"SELECT t.*,concat(e.Nombres ,' ', e.ApellidosPaterno, ' ', e.ApellidosMaterno) as NombreCompleto
                                FROM [dbo].[Tecnico] as t
                                join SEG_Empleado as e on e.EmpleadoID = t.EmpleadoId
                                where t.TecnicoId = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tecnicoId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                tecnico.TecnicoId = ManejoNulos.ManageNullInteger(dr["TecnicoId"]);
                                tecnico.EmpleadoId = ManejoNulos.ManageNullInteger(dr["EmpleadoId"]);
                                tecnico.AreaTechId = ManejoNulos.ManageNullInteger(dr["AreaTechId"]);
                                tecnico.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"].Trim());
                                tecnico.Estado = ManejoNulos.ManegeNullBool(dr["Estado"].Trim());
                                tecnico.NivelTecnicoId = ManejoNulos.ManageNullInteger(dr["NivelTecnicoId"]);
                                tecnico.NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return tecnico;
        }

        public bool TecnicoEditarJson(TecnicoEntidad tecnico)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[Tecnico]
                               SET [EmpleadoId] = @p0
                                  ,[AreaTechId] = @p1
                                  ,[FechaRegistro] = @p2
                                  ,[NivelTecnicoId] = @p3
                                  ,[Estado] = @p4
                             WHERE TecnicoId = @p5";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(tecnico.EmpleadoId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(tecnico.AreaTechId));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(tecnico.FechaRegistro));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(tecnico.NivelTecnicoId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManegeNullBool(tecnico.Estado));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(tecnico.TecnicoId));
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

        public List<TecnicoEntidad> TecnicoListarJson()
        {
            List<TecnicoEntidad> lista = new List<TecnicoEntidad>();
            string consulta = @"SELECT t.*,concat(e.Nombres ,' ', e.ApellidosPaterno, ' ', e.ApellidosMaterno) as NombreCompleto
                                FROM [dbo].[Tecnico] as t
                                join SEG_Empleado as e on e.EmpleadoID = t.EmpleadoId";
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
                            var tecnico = new TecnicoEntidad
                            {
                                TecnicoId = ManejoNulos.ManageNullInteger(dr["TecnicoId"]),
                                EmpleadoId = ManejoNulos.ManageNullInteger(dr["EmpleadoId"]),
                                AreaTechId = ManejoNulos.ManageNullInteger(dr["AreaTechId"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"].Trim()),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"].Trim()),
                                NivelTecnicoId = ManejoNulos.ManageNullInteger(dr["NivelTecnicoId"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"])                                
                            };
                            lista.Add(tecnico);
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


        public bool ActualizarEstadoTecnico(int TecnicoId, int TecnicoEstado)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[Tecnico]
                               SET [Estado] = @p1
                             WHERE TecnicoId = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(TecnicoId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(TecnicoEstado));
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
