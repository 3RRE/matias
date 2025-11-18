using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class ProyectadoProgresivoDAL
    {
        readonly string _conexion;

        public ProyectadoProgresivoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ProyectadoProgresivoEntidad> ProyectadoProgresivoListadoJson()
        {
            List<ProyectadoProgresivoEntidad> lista = new List<ProyectadoProgresivoEntidad>();
            string consulta = @"SELECT 
                               [IdProyectadoProgresivo]
                              ,[NroMaquina]
                              ,[Descripcion]
                              ,[TotalJugMes]
                              ,[TipoCambio]
                              ,[Retencion]
                              ,[PremioBasePozoInferior]
                              ,[PremioBasePozoMedio]
                              ,[PremioBasePozoSuperior]
                              ,[PremioMinimoPozoInferior]
                              ,[PremioMinimoPozoMedio]
                              ,[PremioMinimoPozoSuperior]
                              ,[PremioMaximoPozoInferior]
                              ,[PremioMaximoPozoMedio]
                              ,[PremioMaximoPozoSuperior]
                              ,[IncrementoPozoInferior]
                              ,[IncrementoPozoMedio]
                              ,[IncrementoPozoSuperior]
                              ,[FechaRegistro]
                          FROM [dbo].[ProyectadoProgresivo]";

            using (var con = new SqlConnection(_conexion))
            {
                con.Open();
                var query = new SqlCommand(consulta, con);
                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var item = new ProyectadoProgresivoEntidad
                        {
                            IdProyectadoProgresivo = ManejoNulos.ManageNullInteger(dr["IdProyectadoProgresivo"]),
                            NroMaquina = ManejoNulos.ManageNullInteger(dr["NroMaquina"]),
                            Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                            TotalJugMes = ManejoNulos.ManageNullDecimal(dr["TotalJugMes"]),
                            TipoCambio = ManejoNulos.ManageNullDecimal(dr["TipoCambio"]),
                            Retencion = ManejoNulos.ManageNullDecimal(dr["Retencion"]),
                            PremioBasePozoInferior = ManejoNulos.ManageNullDecimal(dr["PremioBasePozoInferior"]),
                            PremioBasePozoMedio = ManejoNulos.ManageNullDecimal(dr["PremioBasePozoMedio"]),
                            PremioBasePozoSuperior = ManejoNulos.ManageNullDecimal(dr["PremioBasePozoSuperior"]),
                            PremioMinimoPozoInferior = ManejoNulos.ManageNullDecimal(dr["PremioMinimoPozoInferior"]),
                            PremioMinimoPozoMedio = ManejoNulos.ManageNullDecimal(dr["PremioMinimoPozoMedio"]),
                            PremioMinimoPozoSuperior = ManejoNulos.ManageNullDecimal(dr["PremioMinimoPozoSuperior"]),
                            PremioMaximoPozoInferior = ManejoNulos.ManageNullDecimal(dr["PremioMaximoPozoInferior"]),
                            PremioMaximoPozoMedio = ManejoNulos.ManageNullDecimal(dr["PremioMaximoPozoMedio"]),
                            PremioMaximoPozoSuperior = ManejoNulos.ManageNullDecimal(dr["PremioMaximoPozoSuperior"]),
                            IncrementoPozoInferior = ManejoNulos.ManageNullDecimal(dr["IncrementoPozoInferior"]),
                            IncrementoPozoMedio = ManejoNulos.ManageNullDecimal(dr["IncrementoPozoMedio"]),
                            IncrementoPozoSuperior = ManejoNulos.ManageNullDecimal(dr["IncrementoPozoSuperior"]),
                            FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"])
                        };
                        lista.Add(item);
                    }
                }
            }

            return lista;
        }

        public bool ProyectadoProgresivoEditarJson(ProyectadoProgresivoEntidad entidad)
        {
            string consulta = @"UPDATE [dbo].[ProyectadoProgresivo]
                           SET
                               [NroMaquina] = @p1
                              ,[Descripcion] = @p2
                              ,[TotalJugMes] = @p3
                              ,[TipoCambio] = @p4
                              ,[Retencion] = @p5
                              ,[PremioBasePozoInferior] = @p6
                              ,[PremioBasePozoMedio] = @p7
                              ,[PremioBasePozoSuperior] =@p8
                              ,[PremioMinimoPozoInferior] = @p9
                              ,[PremioMinimoPozoMedio] = @p10
                              ,[PremioMinimoPozoSuperior] = @p11
                              ,[PremioMaximoPozoInferior] = @p12
                              ,[PremioMaximoPozoMedio] = @p13
                              ,[PremioMaximoPozoSuperior] = @p14
                              ,[IncrementoPozoInferior] = @p15
                              ,[IncrementoPozoMedio] = @p16
                              ,[IncrementoPozoSuperior] = @p17
                         WHERE [IdProyectadoProgresivo] = @p0";

            using (var con = new SqlConnection(_conexion))
            {
                con.Open();
                var query = new SqlCommand(consulta, con);
                query.Parameters.AddWithValue("@p0", entidad.IdProyectadoProgresivo);
                query.Parameters.AddWithValue("@p1", entidad.NroMaquina);
                query.Parameters.AddWithValue("@p2", entidad.Descripcion);
                query.Parameters.AddWithValue("@p3", entidad.TotalJugMes);
                query.Parameters.AddWithValue("@p4", entidad.TipoCambio);
                query.Parameters.AddWithValue("@p5", entidad.Retencion);
                query.Parameters.AddWithValue("@p6", entidad.PremioBasePozoInferior);
                query.Parameters.AddWithValue("@p7", entidad.PremioBasePozoMedio);
                query.Parameters.AddWithValue("@p8", entidad.PremioBasePozoSuperior);
                query.Parameters.AddWithValue("@p9", entidad.PremioMinimoPozoInferior);
                query.Parameters.AddWithValue("@p10", entidad.PremioMinimoPozoMedio);
                query.Parameters.AddWithValue("@p11", entidad.PremioMinimoPozoSuperior);
                query.Parameters.AddWithValue("@p12", entidad.PremioMaximoPozoInferior);
                query.Parameters.AddWithValue("@p13", entidad.PremioMaximoPozoMedio);
                query.Parameters.AddWithValue("@p14", entidad.PremioMaximoPozoSuperior);
                query.Parameters.AddWithValue("@p15", entidad.IncrementoPozoInferior);
                query.Parameters.AddWithValue("@p16", entidad.IncrementoPozoMedio);
                query.Parameters.AddWithValue("@p17", entidad.IncrementoPozoSuperior);
                return query.ExecuteNonQuery() > 0;
            }
        }

        public ProyectadoProgresivoEntidad ProyectadoProgresivoObtenerIdJson(int idProyectadoProgresivo)
        {
            ProyectadoProgresivoEntidad entidad = null;
            string consulta = @"SELECT 
                               [IdProyectadoProgresivo]
                              ,[NroMaquina]
                              ,[Descripcion]
                              ,[TotalJugMes]
                              ,[TipoCambio]
                              ,[Retencion]
                              ,[PremioBasePozoInferior]
                              ,[PremioBasePozoMedio]
                              ,[PremioBasePozoSuperior]
                              ,[PremioMinimoPozoInferior]
                              ,[PremioMinimoPozoMedio]
                              ,[PremioMinimoPozoSuperior]
                              ,[PremioMaximoPozoInferior]
                              ,[PremioMaximoPozoMedio]
                              ,[PremioMaximoPozoSuperior]
                              ,[IncrementoPozoInferior]
                              ,[IncrementoPozoMedio]
                              ,[IncrementoPozoSuperior]
                              ,[FechaRegistro]
                          FROM [dbo].[ProyectadoProgresivo]
                            WHERE [IdProyectadoProgresivo] = @p0 ";

            using (var con = new SqlConnection(_conexion))
            {
                con.Open();
                var query = new SqlCommand(consulta, con);
                query.Parameters.AddWithValue("@p0", idProyectadoProgresivo);
                using (var dr = query.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            entidad = new ProyectadoProgresivoEntidad
                            {
                                IdProyectadoProgresivo =
                                    ManejoNulos.ManageNullInteger(dr["IdProyectadoProgresivo"]),
                                NroMaquina = ManejoNulos.ManageNullInteger(dr["NroMaquina"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                TotalJugMes = ManejoNulos.ManageNullDecimal(dr["TotalJugMes"]),
                                TipoCambio = ManejoNulos.ManageNullDecimal(dr["TipoCambio"]),
                                Retencion = ManejoNulos.ManageNullDecimal(dr["Retencion"]),
                                PremioBasePozoInferior =
                                    ManejoNulos.ManageNullDecimal(dr["PremioBasePozoInferior"]),
                                PremioBasePozoMedio = ManejoNulos.ManageNullDecimal(dr["PremioBasePozoMedio"]),
                                PremioBasePozoSuperior =
                                    ManejoNulos.ManageNullDecimal(dr["PremioBasePozoSuperior"]),
                                PremioMinimoPozoInferior =
                                    ManejoNulos.ManageNullDecimal(dr["PremioMinimoPozoInferior"]),
                                PremioMinimoPozoMedio =
                                    ManejoNulos.ManageNullDecimal(dr["PremioMinimoPozoMedio"]),
                                PremioMinimoPozoSuperior =
                                    ManejoNulos.ManageNullDecimal(dr["PremioMinimoPozoSuperior"]),
                                PremioMaximoPozoInferior =
                                    ManejoNulos.ManageNullDecimal(dr["PremioMaximoPozoInferior"]),
                                PremioMaximoPozoMedio =
                                    ManejoNulos.ManageNullDecimal(dr["PremioMaximoPozoMedio"]),
                                PremioMaximoPozoSuperior =
                                    ManejoNulos.ManageNullDecimal(dr["PremioMaximoPozoSuperior"]),
                                IncrementoPozoInferior =
                                    ManejoNulos.ManageNullDecimal(dr["IncrementoPozoInferior"]),
                                IncrementoPozoMedio = ManejoNulos.ManageNullDecimal(dr["IncrementoPozoMedio"]),
                                IncrementoPozoSuperior =
                                    ManejoNulos.ManageNullDecimal(dr["IncrementoPozoSuperior"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"])
                            };
                            break;
                        }
                    }
                }
            }

            return entidad;
        }


        public bool ProyectadoProgresivoInsertarJson(ProyectadoProgresivoEntidad entidad)
        {

            string consulta = @"INSERT INTO [dbo].[ProyectadoProgresivo]
           ([NroMaquina]
           ,[Descripcion]
           ,[TotalJugMes]
           ,[TipoCambio]
           ,[Retencion]
           ,[PremioBasePozoInferior]
           ,[PremioBasePozoMedio]
           ,[PremioBasePozoSuperior]
           ,[PremioMinimoPozoInferior]
           ,[PremioMinimoPozoMedio]
           ,[PremioMinimoPozoSuperior]
           ,[PremioMaximoPozoInferior]
           ,[PremioMaximoPozoMedio]
           ,[PremioMaximoPozoSuperior]
           ,[IncrementoPozoInferior]
           ,[IncrementoPozoMedio]
           ,[IncrementoPozoSuperior]
           ,[FechaRegistro])
     VALUES
           (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17)";

            using (var con = new SqlConnection(_conexion))
            {
                con.Open();
                var query = new SqlCommand(consulta, con);
                query.Parameters.AddWithValue("@p0", entidad.NroMaquina);
                query.Parameters.AddWithValue("@p1", entidad.Descripcion);
                query.Parameters.AddWithValue("@p2", entidad.TotalJugMes);
                query.Parameters.AddWithValue("@p3", entidad.TipoCambio);
                query.Parameters.AddWithValue("@p4", entidad.Retencion);
                query.Parameters.AddWithValue("@p5", entidad.PremioBasePozoInferior);
                query.Parameters.AddWithValue("@p6", entidad.PremioBasePozoMedio);
                query.Parameters.AddWithValue("@p7", entidad.PremioBasePozoSuperior);
                query.Parameters.AddWithValue("@p8", entidad.PremioMinimoPozoInferior);
                query.Parameters.AddWithValue("@p9", entidad.PremioMinimoPozoMedio);
                query.Parameters.AddWithValue("@p10", entidad.PremioMinimoPozoSuperior);
                query.Parameters.AddWithValue("@p11", entidad.PremioMaximoPozoInferior);
                query.Parameters.AddWithValue("@p12", entidad.PremioMaximoPozoMedio);
                query.Parameters.AddWithValue("@p13", entidad.PremioMaximoPozoSuperior);
                query.Parameters.AddWithValue("@p14", entidad.IncrementoPozoInferior);
                query.Parameters.AddWithValue("@p15", entidad.IncrementoPozoMedio);
                query.Parameters.AddWithValue("@p16", entidad.IncrementoPozoSuperior);
                query.Parameters.AddWithValue("@p17", entidad.FechaRegistro);
                return query.ExecuteNonQuery() > 0;

            }
        }
    }
}

