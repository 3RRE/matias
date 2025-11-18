using CapaEntidad.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala.CapaEntidad.EntradaSalidaSala;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.EntradaSalidaSala
{
    public class ESS_DashboardDAL
    {
        string _conexion = string.Empty;
        public ESS_DashboardDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ESS_DashboardLudopatasEntidad> GetListDashboardLudopatasEntidad(int[] codsala, DateTime fechaIni, DateTime fechaFin)
        {
            List<ESS_DashboardLudopatasEntidad> lista = new List<ESS_DashboardLudopatasEntidad>();

            string strSalaList = string.Join(",", codsala);

            string consulta = $@"
SELECT 
aud.sala as NombreSala,
aud.dni as DNI,
aud.fecha AS FechaIngreso
FROM [CAL_Auditoria] as aud
LEFT JOIN [CAL_Ludopata] lud ON lud.DNI= aud.dni
WHERE lud.Estado=1 AND CONVERT(date, aud.fecha) BETWEEN CONVERT(date, @p1) AND CONVERT(date, @p2);";


            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);
                    query.CommandTimeout = 60;

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new ESS_DashboardLudopatasEntidad
                            {
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                DNI = ManejoNulos.ManageNullStr(dr["DNI"]),
                                FechaIngreso = ManejoNulos.ManageNullDate(dr["FechaIngreso"]),
                            };

                            lista.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_DashboardLudopatasEntidad>();
            }

            return lista;
        }


        public List<ESS_DashboardReacudacionEntidad> GetListDashboardReacudacionEntidad(int[] codsala, DateTime fechaIni, DateTime fechaFin)
        {
            List<ESS_DashboardReacudacionEntidad> lista = new List<ESS_DashboardReacudacionEntidad>();

            string strSala = string.Empty;
            strSala = $" rp.CodSala in ({String.Join(",", codsala)}) and ";
            string strTipo = string.Empty;
            string consulta = $@"
		SELECT
        rp.IdRecaudacionPersonal as IdRecaudacion,
        rpe.IdRecaudacionPersonalEmpleado as IdPersonalRecaudacion,
        sa.NombreCorto as NombreSala,
        rpe.IdEstadoParticipante as Estado,
        fun.Nombre as NombreFuncion
        FROM ESS_RecaudacionPersonal rp
        JOIN Sala sa ON rp.CodSala = sa.CodSala
        JOIN ESS_RecaudacionPersonalEmpleado rpe ON rp.IdRecaudacionPersonal = rpe.IdRecaudacionPersonal
        LEFT JOIN ESS_Funcion fun ON rpe.IdFuncion = fun.IdFuncion
        WHERE rpe.IdEstadoParticipante=1 AND {strSala} CONVERT(date, rp.FechaRegistro) BETWEEN CONVERT(date, @p1) AND CONVERT(date, @p2)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new ESS_DashboardReacudacionEntidad
                            {
                                IdRecaudacion = ManejoNulos.ManageNullInteger(dr["IdRecaudacion"]),
                                IdPersonalRecaudacion = ManejoNulos.ManageNullInteger(dr["IdPersonalRecaudacion"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreFuncion = ManejoNulos.ManageNullStr(dr["NombreFuncion"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_DashboardReacudacionEntidad>();
            }

            return lista;
        }

        
        public List<ESS_DashboardCajasTemporizadasEntidad> GetListDashboardCajasTemporizadasEntidad(int[] codsala, DateTime fechaIni, DateTime fechaFin)
        {
            List<ESS_DashboardCajasTemporizadasEntidad> lista = new List<ESS_DashboardCajasTemporizadasEntidad>();

            string strSala = string.Empty;
            strSala = $" act.CodSala in ({String.Join(",", codsala)}) and ";
            string strTipo = string.Empty;
            string consulta = $@"
		        SELECT 
        sa.NombreCorto as NombreSala,
        def.Nombre as NombreDeficiencia,
        CASE 
            WHEN act.FechaSolucion IS NOT NULL THEN 'Solucionado'
            ELSE  'No Solucionado' 
        END AS Estado
        FROM [ESS_AccionCajaTemporizada] as act
        LEFT JOIN Sala sa ON act.CodSala = sa.CodSala
        LEFT JOIN ESS_Deficiencia def ON def.IdDeficiencia= act.IdDeficiencia
        WHERE {strSala} CONVERT(date, act.FechaRegistro) BETWEEN CONVERT(date, @p1) AND CONVERT(date, @p2)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new ESS_DashboardCajasTemporizadasEntidad
                            {
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreEstado = ManejoNulos.ManageNullStr(dr["Estado"]),
                                NombreDeficiencia = ManejoNulos.ManageNullStr(dr["NombreDeficiencia"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_DashboardCajasTemporizadasEntidad>();
            }

            return lista;
        }

        public List<ESS_DashboardEnteReguladoraEntidad> GetListDashboardEnteReguladoraEntidad(int[] codsala, DateTime fechaIni, DateTime fechaFin)
        {
            List<ESS_DashboardEnteReguladoraEntidad> lista = new List<ESS_DashboardEnteReguladoraEntidad>();

            string strSala = string.Empty;
            strSala = $" er.CodSala in ({String.Join(",", codsala)}) and ";
            string strTipo = string.Empty;
            string consulta = $@"
  SELECT 
sa.NombreCorto as NombreSala,
er.NombreEmpresa as EnteReguladora,
erm.Nombre as NombreMotivo
FROM [ESS_EnteRegulador] as er
LEFT JOIN Sala sa ON er.CodSala = sa.CodSala
LEFT JOIN [ESS_EnteReguladorMotivo] erm ON erm.IdMotivo= er.IdMotivo
        WHERE {strSala} CONVERT(date, er.FechaRegistro) BETWEEN CONVERT(date, @p1) AND CONVERT(date, @p2)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new ESS_DashboardEnteReguladoraEntidad
                            {
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreMotivo = ManejoNulos.ManageNullStr(dr["NombreMotivo"]),
                                EnteReguladora = ManejoNulos.ManageNullStr(dr["EnteReguladora"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_DashboardEnteReguladoraEntidad>();
            }

            return lista;
        }

        public List<ESS_DashboardOcurrenciasLogEntidad> GetListDashboardOcurrenciasLogEntidad(int[] codsala, DateTime fechaIni, DateTime fechaFin)
        {
            List<ESS_DashboardOcurrenciasLogEntidad> lista = new List<ESS_DashboardOcurrenciasLogEntidad>();

            string strSala = string.Empty;
            strSala = $" lo.CodSala in ({String.Join(",", codsala)}) and ";
            string strTipo = string.Empty;
            string consulta = $@"
		            SELECT 
sa.NombreCorto as NombreSala,
tip.Nombre as NombreTipologia,
eo.Nombre as NombreEstado
FROM [ESS_LogOcurrencia] as lo
LEFT JOIN Sala sa ON lo.CodSala = sa.CodSala
LEFT JOIN [ESS_Tipologia] tip ON tip.IdTipologia= lo.IdTipologia
LEFT JOIN [ESS_EstadoOcurrencia] eo ON eo.IdEstadoOcurrencia= lo.IdEstadoOcurrencia
        WHERE {strSala} CONVERT(date, lo.FechaRegistro) BETWEEN CONVERT(date, @p1) AND CONVERT(date, @p2)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new ESS_DashboardOcurrenciasLogEntidad
                            {
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                NombreTipologia = ManejoNulos.ManageNullStr(dr["NombreTipologia"]),
                                NombreEstado = ManejoNulos.ManageNullStr(dr["NombreEstado"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_DashboardOcurrenciasLogEntidad>();
            }

            return lista;
        }


    }
}