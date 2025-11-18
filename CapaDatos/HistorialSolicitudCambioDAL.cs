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
    public class HistorialSolicitudCambioDAL
    {
        string _conexion = string.Empty;
        public HistorialSolicitudCambioDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<HistorialSolicitudCambioEntidad> BusquedaHistorialSolicitudCambioJson(int id)
        {
            List<HistorialSolicitudCambioEntidad> lista = new List<HistorialSolicitudCambioEntidad>();
            string consulta = @"select h.HistorialId, e.Descripcion EstadoNuevo,h.FechaRespuesta,h.Observacion
                                from HistorialSolicitud h
                                join SolicitudCambio s on s.SolicitudId = h.SolicitudId
                                join EstadoSolicitudCambio e on e.EstadoSolicitudCambioId = h.EstadoSolicitudId
                                where h.SolicitudId ="+id;
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
                            var historial = new HistorialSolicitudCambioEntidad
                            {
                                HistorialId = ManejoNulos.ManageNullInteger(dr["HistorialId"]),
                                EstadoNuevo = ManejoNulos.ManageNullStr(dr["EstadoNuevo"]),
                                FechaRespuesta = ManejoNulos.ManageNullDate(dr["FechaRespuesta"]),
                                Observacion = ManejoNulos.ManageNullStr(dr["Observacion"])
                            };

                            lista.Add(historial);
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

        public bool RegistrarHistorialSolicitudCambio(HistorialSolicitudCambioEntidad historial)
        {
            bool respuesta = false;

            string consulta = @"INSERT INTO [dbo].[HistorialSolicitud]
                               ([SolicitudId]
                               ,[EstadoSolicitudId]
                               ,[FechaRegistro]
                               ,[AprobadoPor]
                               ,[FechaRespuesta]
                               ,[Observacion])
                                VALUES
                                (@p0,@p1,@p2,@p3,@p4,@p5)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(historial.SolicitudId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(historial.EstadoSolicitudId));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(historial.FechaRegistro));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(historial.AprobadorPor));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(historial.FechaRespuesta));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(historial.Observacion));
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
