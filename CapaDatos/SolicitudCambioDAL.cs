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
    public class SolicitudCambioDAL
    {
        string _conexion = string.Empty;
        public SolicitudCambioDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<SolicitudCambioEntidad> ListaSolicitudCambioJson()
        {
            List<SolicitudCambioEntidad> lista = new List<SolicitudCambioEntidad>();
            string consulta = @"select
                                s.SolicitudId,m.Descripcion Modulo,t.Descripcion TipoCambio,i.Descripcion Impacto,(se.ApellidosPaterno+' '+se.ApellidosMaterno+' '+se.Nombres) NombreCompleto,
                                e.Descripcion EstadoSolicitud,sl.Nombre Sala,s.Descripcion,s.FechaRegistro
                                from SolicitudCambio s
                                join Modulo m on m.ModuloId = s.ModuloId  
                                join TipoCambio t on t.TipoCambioId = s.TipoCambioId
                                join Impacto i on i.ImpactoId = s.ImpactoId
                                join EstadoSolicitudCambio e on e.EstadoSolicitudCambioId = s.EstadoSolicitudCambioId
                                join Sala sl on sl.CodSala = s.SalaId
                                join SEG_Empleado se on se.EmpleadoID = s.SolicitanteId
                                where s.SolicitanteId";
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
                            var sistema = new SolicitudCambioEntidad
                            {
                                SolicitudId = ManejoNulos.ManageNullInteger(dr["SolicitudId"]),
                                ModuloDescripcion = ManejoNulos.ManageNullStr(dr["Modulo"].Trim()),
                                TipoCambioDescripcion = ManejoNulos.ManageNullStr(dr["TipoCambio"]),
                                ImpactoDescripcion = ManejoNulos.ManageNullStr(dr["Impacto"]),
                                SolicitanteDescripcion = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                EstadoSolicitudCambioDescripcion = ManejoNulos.ManageNullStr(dr["EstadoSolicitud"]),
                                SalaDescripcion = ManejoNulos.ManageNullStr(dr["Sala"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"])

                            };

                            lista.Add(sistema);
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

        public SolicitudCambioEntidad ObtenerUltimaSolicitudUsuarioLogeado(int EmpleadoId)
        {
            SolicitudCambioEntidad solicitudentidad = new SolicitudCambioEntidad();
            string consulta = @"select top 1 * 
                                from SolicitudCambio s
                                where s.SolicitanteId = @p0
                                order by s.SolicitudId desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", EmpleadoId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                solicitudentidad.SolicitudId = ManejoNulos.ManageNullInteger(dr["SolicitudId"]);
                                solicitudentidad.ModuloId = ManejoNulos.ManageNullInteger(dr["ModuloId"]);
                                solicitudentidad.TipoCambioId = ManejoNulos.ManageNullInteger(dr["TipoCambioId"]);
                                solicitudentidad.ImpactoId = ManejoNulos.ManageNullInteger(dr["ImpactoId"]);
                                solicitudentidad.SolicitanteId = ManejoNulos.ManageNullInteger(dr["SolicitanteId"]);
                                solicitudentidad.EstadoSolicitudCambioId = ManejoNulos.ManageNullInteger(dr["EstadoSolicitudCambioId"]);
                                solicitudentidad.SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]);
                                solicitudentidad.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                solicitudentidad.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return solicitudentidad;
        }

        public bool SolicitudCambioEliminarJson(int id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM SolicitudCambio
                                where SolicitudId = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
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

        public bool SolicitudCambioActualizarJson(SolicitudCambioEntidad solicitudcambio)
        {
            bool respuesta = false;
            string consulta = @"update SolicitudCambio set
                                EstadoSolicitudCambioId = @p1,
                                Descripcion = @p2
                                WHERE SolicitudId = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", solicitudcambio.SolicitudId);
                    query.Parameters.AddWithValue("@p1", solicitudcambio.EstadoSolicitudCambioId);
                    query.Parameters.AddWithValue("@p2", solicitudcambio.Descripcion);
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

        public SolicitudCambioEntidad ObtenerSolicitudIdJson(int id)
        {
            SolicitudCambioEntidad solicitudentidad = new SolicitudCambioEntidad();
            string consulta = @"select s.SolicitudId,s.EstadoSolicitudCambioId,s.Descripcion
                                from SolicitudCambio s
                                where s.SolicitudId = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                solicitudentidad.SolicitudId = ManejoNulos.ManageNullInteger(dr["SolicitudId"]);
                                solicitudentidad.EstadoSolicitudCambioId = ManejoNulos.ManageNullInteger(dr["EstadoSolicitudCambioId"]);
                                solicitudentidad.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return solicitudentidad;
        }

        public List<SolicitudCambioEntidad> BuscarSolicitudCambioNoComite(string id_sala, DateTime fechaInicio, DateTime fechaFin,int id_empleado, string tipoCambio,string estadoSolicitudCambioId)
        {
            List<SolicitudCambioEntidad> lista = new List<SolicitudCambioEntidad>();
            //string consulta = @"select
            //                    s.SolicitudId,sm.Descripcion Sistema,m.Descripcion Modulo,t.Descripcion TipoCambio,i.Descripcion Impacto,(se.ApellidosPaterno+' '+se.ApellidosMaterno+' '+se.Nombres) NombreCompleto,
            //                    e.Descripcion EstadoSolicitud,sl.Nombre Sala,s.Descripcion,s.FechaRegistro,s.EstadoSolicitudCambioId
            //                    from SolicitudCambio s
            //                    join Modulo m on m.ModuloId = s.ModuloId  
            //                    join Sistema sm on sm.SistemaId = m.SistemaId
            //                    join TipoCambio t on t.TipoCambioId = s.TipoCambioId
            //                    join Impacto i on i.ImpactoId = s.ImpactoId
            //                    join EstadoSolicitudCambio e on e.EstadoSolicitudCambioId = s.EstadoSolicitudCambioId
            //                    join Sala sl on sl.CodSala = s.SalaId
            //                    join SEG_Empleado se on se.EmpleadoID = s.SolicitanteId
            //                    WHERE
	           //                 s.FechaRegistro BETWEEN '{3}' AND '{4}' AND s.SolicitanteId="+id_empleado+
            //                    "{0} {1} {2} ORDER BY s.EstadoSolicitudCambioId ASC	";
            //estadoSolicitudCambioId = estadoSolicitudCambioId == "" ? "" : " AND s.EstadoSolicitudCambioId =" + estadoSolicitudCambioId;
            //tipoCambio = tipoCambio == "" ? "" : " AND s.TipoCambioId =" + tipoCambio;
            //id_sala = id_sala == "" ? "" : " AND s.SalaId =" + id_sala;
            //consulta = String.Format(consulta,estadoSolicitudCambioId,tipoCambio,id_sala,fechaInicio,fechaFin);

            string QuerySQL = @"select s.SolicitudId,sm.Descripcion Sistema,m.Descripcion Modulo,t.Descripcion TipoCambio,i.Descripcion Impacto,
                (se.ApellidosPaterno+' '+se.ApellidosMaterno+' '+se.Nombres) NombreCompleto, e.Descripcion EstadoSolicitud,
                sl.Nombre Sala,s.Descripcion,s.FechaRegistro,s.EstadoSolicitudCambioId 
                from SolicitudCambio s join Modulo m on m.ModuloId = s.ModuloId 
                join Sistema sm on sm.SistemaId = m.SistemaId join TipoCambio t on t.TipoCambioId = s.TipoCambioId 
                join Impacto i on i.ImpactoId = s.ImpactoId join EstadoSolicitudCambio e on e.EstadoSolicitudCambioId = s.EstadoSolicitudCambioId 
                join Sala sl on sl.CodSala = s.SalaId join SEG_Empleado se on se.EmpleadoID = s.SolicitanteId
                WHERE s.FechaRegistro BETWEEN @fechaIni AND @fechaFin 
                 AND s.TipoCambioId=(case when @TC= '' then  s.TipoCambioId else  @TC end )
                AND s.EstadoSolicitudCambioId=(case when @EstSol= '' then  s.EstadoSolicitudCambioId else  @EstSol end )
                AND s.SolicitanteId= @IdSolicitante AND s.SalaId = @IdSala ORDER BY s.EstadoSolicitudCambioId ASC";
            //procc_Nro_Maquina = procc_Nro_Maquina == null ? "" : " and PROCC_Nro_Maquina='" + procc_Nro_Maquina + "'";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(QuerySQL, con);
                    query.Parameters.AddWithValue("@fechaIni", fechaInicio);
                    query.Parameters.AddWithValue("@fechaFin", fechaFin);
                    query.Parameters.AddWithValue("@IdSolicitante", id_empleado);
                    query.Parameters.AddWithValue("@IdSala", id_sala);
                    query.Parameters.AddWithValue("@TC", tipoCambio);
                    query.Parameters.AddWithValue("@EstSol", estadoSolicitudCambioId);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var sistema = new SolicitudCambioEntidad
                            {
                                SolicitudId = ManejoNulos.ManageNullInteger(dr["SolicitudId"]),
                                SistemaDescripcion = ManejoNulos.ManageNullStr(dr["Sistema"]),
                                ModuloDescripcion = ManejoNulos.ManageNullStr(dr["Modulo"].Trim()),
                                TipoCambioDescripcion = ManejoNulos.ManageNullStr(dr["TipoCambio"]),
                                ImpactoDescripcion = ManejoNulos.ManageNullStr(dr["Impacto"]),
                                SolicitanteDescripcion = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                EstadoSolicitudCambioDescripcion = ManejoNulos.ManageNullStr(dr["EstadoSolicitud"]),
                                SalaDescripcion = ManejoNulos.ManageNullStr(dr["Sala"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                EstadoSolicitudCambioId = ManejoNulos.ManageNullInteger(dr["EstadoSolicitudCambioId"])

                            };

                            lista.Add(sistema);
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

        public bool EnviarSolicitudCambioJson(SolicitudCambioEntidad solicitudCambio)
        {
            bool respuesta = false;

            string consulta = @"INSERT INTO [SolicitudCambio]
                               ([ModuloId]
                               ,[TipoCambioId]
                               ,[ImpactoId]
                               ,[SolicitanteId]
                               ,[EstadoSolicitudCambioId]
                               ,[SalaId]
                               ,[Descripcion]
                               ,[FechaRegistro])
                         VALUES
                               (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(solicitudCambio.ModuloId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(solicitudCambio.TipoCambioId));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(solicitudCambio.ImpactoId));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(solicitudCambio.SolicitanteId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(solicitudCambio.EstadoSolicitudCambioId));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(solicitudCambio.SalaId));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(solicitudCambio.Descripcion));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(solicitudCambio.FechaRegistro));
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

        public bool GuardarBorradorSolicitudCambioJson(SolicitudCambioEntidad solicitudCambio)
        {
            bool respuesta = false;

            string consulta = @"INSERT INTO [SolicitudCambio]
                               ([ModuloId]
                               ,[TipoCambioId]
                               ,[ImpactoId]
                               ,[SolicitanteId]
                               ,[EstadoSolicitudCambioId]
                               ,[SalaId]
                               ,[Descripcion]
                               ,[FechaRegistro])
                         VALUES
                               (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(solicitudCambio.ModuloId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(solicitudCambio.TipoCambioId));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(solicitudCambio.ImpactoId));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(solicitudCambio.SolicitanteId));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(solicitudCambio.EstadoSolicitudCambioId));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(solicitudCambio.SalaId));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(solicitudCambio.Descripcion));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(solicitudCambio.FechaRegistro));
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

        public List<SolicitudCambioEntidad> BuscarSolicitudCambioJson(string id_sala, DateTime fechainicio, DateTime fechafin, string tipoCambio, string estadoSolicitudCambioId)
        {
            List<SolicitudCambioEntidad> lista = new List<SolicitudCambioEntidad>();
            //string consulta = @"select
            //                    s.SolicitudId,sm.Descripcion Sistema,m.Descripcion Modulo,t.Descripcion TipoCambio,i.Descripcion Impacto,(se.ApellidosPaterno+' '+se.ApellidosMaterno+' '+se.Nombres) NombreCompleto,
            //                    e.Descripcion EstadoSolicitud,sl.Nombre Sala,s.Descripcion,s.FechaRegistro,s.EstadoSolicitudCambioId
            //                    from SolicitudCambio s
            //                    join Modulo m on m.ModuloId = s.ModuloId  
            //                    join Sistema sm on sm.SistemaId = m.SistemaId
            //                    join TipoCambio t on t.TipoCambioId = s.TipoCambioId
            //                    join Impacto i on i.ImpactoId = s.ImpactoId
            //                    join EstadoSolicitudCambio e on e.EstadoSolicitudCambioId = s.EstadoSolicitudCambioId
            //                    join Sala sl on sl.CodSala = s.SalaId
            //                    join SEG_Empleado se on se.EmpleadoID = s.SolicitanteId
            //                    WHERE
	           //                 s.FechaRegistro BETWEEN  {3} AND {4} " +
            //                    "{0} {1} {2}   ORDER BY s.EstadoSolicitudCambioId ASC	"
            //                    ;
            //estadoSolicitudCambioId = estadoSolicitudCambioId == "" ? "" : " AND s.EstadoSolicitudCambioId =" + estadoSolicitudCambioId;
            //tipoCambio = tipoCambio == "" ? "" : " AND s.TipoCambioId =" + tipoCambio;
            ////id_sala = id_sala == "" ? "" : " AND s.SalaId =" + id_sala;
            //id_sala = id_sala == "" ? "" : " AND s.SalaId =" + id_sala;
            //consulta = String.Format(consulta, id_sala,estadoSolicitudCambioId,tipoCambio, fechainicio,fechafin);

            string QuerySQL = @"select s.SolicitudId,sm.Descripcion Sistema,m.Descripcion Modulo,t.Descripcion TipoCambio,i.Descripcion Impacto,
                    (se.ApellidosPaterno+' '+se.ApellidosMaterno+' '+se.Nombres) NombreCompleto, e.Descripcion EstadoSolicitud,sl.Nombre Sala,
                    s.Descripcion,s.FechaRegistro,s.EstadoSolicitudCambioId 
                    from SolicitudCambio s join Modulo m on m.ModuloId = s.ModuloId join Sistema sm on sm.SistemaId = m.SistemaId 
                    join TipoCambio t on t.TipoCambioId = s.TipoCambioId join Impacto i on i.ImpactoId = s.ImpactoId 
                    join EstadoSolicitudCambio e on e.EstadoSolicitudCambioId = s.EstadoSolicitudCambioId 
                    join Sala sl on sl.CodSala = s.SalaId join SEG_Empleado se on se.EmpleadoID = s.SolicitanteId 
                    WHERE s.FechaRegistro BETWEEN @fechaIni AND @fechaFin  AND s.SalaId =@IdSala 
                    AND s.TipoCambioId=(case when @TC= '' then  s.TipoCambioId else  @TC end )
                    AND s.EstadoSolicitudCambioId=(case when @EstSol= '' then  s.EstadoSolicitudCambioId else  @EstSol end )
                    ORDER BY s.EstadoSolicitudCambioId ASC ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(QuerySQL, con);
                    query.Parameters.AddWithValue("@fechaIni", fechainicio);
                    query.Parameters.AddWithValue("@fechaFin", fechafin);
                    query.Parameters.AddWithValue("@IdSala", id_sala);
                    query.Parameters.AddWithValue("@TC", tipoCambio);
                    query.Parameters.AddWithValue("@EstSol", estadoSolicitudCambioId);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var sistema = new SolicitudCambioEntidad
                            {
                                SolicitudId = ManejoNulos.ManageNullInteger(dr["SolicitudId"]),
                                SistemaDescripcion = ManejoNulos.ManageNullStr(dr["Sistema"]),
                                ModuloDescripcion = ManejoNulos.ManageNullStr(dr["Modulo"].Trim()),
                                TipoCambioDescripcion = ManejoNulos.ManageNullStr(dr["TipoCambio"]),
                                ImpactoDescripcion = ManejoNulos.ManageNullStr(dr["Impacto"]),
                                SolicitanteDescripcion = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                EstadoSolicitudCambioDescripcion = ManejoNulos.ManageNullStr(dr["EstadoSolicitud"]),
                                SalaDescripcion = ManejoNulos.ManageNullStr(dr["Sala"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                EstadoSolicitudCambioId = ManejoNulos.ManageNullInteger(dr["EstadoSolicitudCambioId"])
                            };

                            lista.Add(sistema);
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
