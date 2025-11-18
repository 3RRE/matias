using CapaEntidad.BUK;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.BUK {
    public class BUK_CreditoDAL {
        string _conexion = string.Empty;

        public BUK_CreditoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<BUK_CreditoEntidad> ObtenerCreditos() {
            List<BUK_CreditoEntidad> lista = new List<BUK_CreditoEntidad>();
            string consulta = @"
                SELECT 
	                IdCredito,
	                IdCreditoBuk,
	                NumeroDocumento,
	                Anio,
	                Periodo,
                    CodigoEmpresa,
	                CodigoTipoVale,
	                CuotaMensual,
	                CantidadCoutas,
	                MontoTotal,
	                DescripcionTipo,
	                Estado,
	                FechaRegistro
                FROM 
	                BUK_Credito
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            BUK_CreditoEntidad item = new BUK_CreditoEntidad {
                                IdCredito = ManejoNulos.ManageNullInteger(dr["IdCredito"]),
                                IdCreditoBuk = ManejoNulos.ManageNullInteger(dr["IdCreditoBuk"]),
                                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                Anio = ManejoNulos.ManageNullInteger(dr["Anio"]),
                                Periodo = ManejoNulos.ManageNullInteger(dr["Periodo"]),
                                CodigoEmpresa = ManejoNulos.ManageNullStr(dr["CodigoEmpresa"]),
                                CodigoTipoVale = ManejoNulos.ManageNullStr(dr["CodigoTipoVale"]),
                                CuotaMensual = ManejoNulos.ManageNullDecimal(dr["CuotaMensual"]),
                                CantidadCoutas = ManejoNulos.ManageNullInteger(dr["CantidadCoutas"]),
                                MontoTotal = ManejoNulos.ManageNullDecimal(dr["MontoTotal"]),
                                DescripcionTipo = ManejoNulos.ManageNullStr(dr["DescripcionTipo"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
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

        public List<BUK_CreditoEntidad> ObtenerCreditosDeEmpleadoByEmpresa(BUK_CreditoEntidad credito) {
            List<BUK_CreditoEntidad> lista = new List<BUK_CreditoEntidad>();
            string consulta = @"
                SELECT 
	                IdCredito,
	                IdCreditoBuk,
	                NumeroDocumento,
	                Anio,
	                Periodo,
                    CodigoEmpresa,
	                CodigoTipoVale,
	                CuotaMensual,
	                CantidadCoutas,
	                MontoTotal,
	                DescripcionTipo,
	                Estado,
	                FechaRegistro
                FROM 
	                BUK_Credito
                WHERE NumeroDocumento = @NumeroDocumento AND
                      Anio = @Anio AND
	                  Periodo = @Periodo AND
	                  CodigoEmpresa = @CodigoEmpresa AND
	                  CodigoTipoVale = @CodigoTipoVale
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@NumeroDocumento", credito.NumeroDocumento);
                    query.Parameters.AddWithValue("@Anio", credito.Anio);
                    query.Parameters.AddWithValue("@Periodo", credito.Periodo);
                    query.Parameters.AddWithValue("@CodigoEmpresa", credito.CodigoEmpresa);
                    query.Parameters.AddWithValue("@CodigoTipoVale", credito.CodigoTipoVale);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            BUK_CreditoEntidad item = new BUK_CreditoEntidad {
                                IdCredito = ManejoNulos.ManageNullInteger(dr["IdCredito"]),
                                IdCreditoBuk = ManejoNulos.ManageNullInteger(dr["IdCreditoBuk"]),
                                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                Anio = ManejoNulos.ManageNullInteger(dr["Anio"]),
                                Periodo = ManejoNulos.ManageNullInteger(dr["Periodo"]),
                                CodigoEmpresa = ManejoNulos.ManageNullStr(dr["CodigoEmpresa"]),
                                CodigoTipoVale = ManejoNulos.ManageNullStr(dr["CodigoTipoVale"]),
                                CuotaMensual = ManejoNulos.ManageNullDecimal(dr["CuotaMensual"]),
                                CantidadCoutas = ManejoNulos.ManageNullInteger(dr["CantidadCoutas"]),
                                MontoTotal = ManejoNulos.ManageNullDecimal(dr["MontoTotal"]),
                                DescripcionTipo = ManejoNulos.ManageNullStr(dr["DescripcionTipo"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
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

        public BUK_CreditoEntidad ObtenerCreditoPorId(int idCredito) {
            BUK_CreditoEntidad credito = new BUK_CreditoEntidad();
            string consulta = @"
                SELECT 
	                IdCredito,
	                IdCreditoBuk,
	                NumeroDocumento,
	                Anio,
	                Periodo,
                    CodigoEmpresa,
	                CodigoTipoVale,
	                CuotaMensual,
	                CantidadCoutas,
	                MontoTotal,
	                DescripcionTipo,
	                Estado,
	                FechaRegistro
                FROM 
	                BUK_Credito
                WHERE
	                IdCredito = @IdCredito
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdCredito", idCredito);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            credito = new BUK_CreditoEntidad {
                                IdCredito = ManejoNulos.ManageNullInteger(dr["IdCredito"]),
                                IdCreditoBuk = ManejoNulos.ManageNullInteger(dr["IdCreditoBuk"]),
                                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                Anio = ManejoNulos.ManageNullInteger(dr["Anio"]),
                                Periodo = ManejoNulos.ManageNullInteger(dr["Periodo"]),
                                CodigoEmpresa = ManejoNulos.ManageNullStr(dr["CodigoEmpresa"]),
                                CodigoTipoVale = ManejoNulos.ManageNullStr(dr["CodigoTipoVale"]),
                                CuotaMensual = ManejoNulos.ManageNullDecimal(dr["CuotaMensual"]),
                                CantidadCoutas = ManejoNulos.ManageNullInteger(dr["CantidadCoutas"]),
                                MontoTotal = ManejoNulos.ManageNullDecimal(dr["MontoTotal"]),
                                DescripcionTipo = ManejoNulos.ManageNullStr(dr["DescripcionTipo"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return credito;
        }

        public int InsertarCredito(BUK_CreditoEntidad credito) {
            int idInsertado = 0;
            string consulta = @"
                INSERT INTO BUK_Credito (IdCreditoBuk, NumeroDocumento, Anio, Periodo, CodigoEmpresa, CodigoTipoVale, CuotaMensual, CantidadCoutas, MontoTotal, DescripcionTipo, Estado, FechaRegistro)
                OUTPUT INSERTED.IdCredito
                VALUES (@IdCreditoBuk, @NumeroDocumento, @Anio, @Periodo, @CodigoEmpresa, @CodigoTipoVale, @CuotaMensual, @CantidadCoutas, @MontoTotal, @DescripcionTipo, @Estado, @FechaRegistro)
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdCreditoBuk", credito.IdCreditoBuk);
                    query.Parameters.AddWithValue("@NumeroDocumento", credito.NumeroDocumento);
                    query.Parameters.AddWithValue("@Anio", credito.Anio);
                    query.Parameters.AddWithValue("@Periodo", credito.Periodo);
                    query.Parameters.AddWithValue("@CodigoEmpresa", credito.CodigoEmpresa);
                    query.Parameters.AddWithValue("@CodigoTipoVale", credito.CodigoTipoVale);
                    query.Parameters.AddWithValue("@CuotaMensual", credito.CuotaMensual);
                    query.Parameters.AddWithValue("@CantidadCoutas", credito.CantidadCoutas);
                    query.Parameters.AddWithValue("@MontoTotal", credito.MontoTotal);
                    query.Parameters.AddWithValue("@DescripcionTipo", credito.DescripcionTipo);
                    query.Parameters.AddWithValue("@Estado", credito.Estado);
                    query.Parameters.AddWithValue("@FechaRegistro", credito.FechaRegistro);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idInsertado = 0;
            }

            return idInsertado;
        }

        public int AnularCredito(BUK_CreditoEntidad credito) {
            int idInsertado = 0;
            string consulta = @"
                UPDATE BUK_Credito
                SET Estado = 0
                OUTPUT INSERTED.IdCredito
                WHERE NumeroDocumento = @NumeroDocumento AND
                      Anio = @Anio AND
	                  Periodo = @Periodo AND
	                  CodigoEmpresa = @CodigoEmpresa AND
	                  CodigoTipoVale = @CodigoTipoVale
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@NumeroDocumento", credito.NumeroDocumento);
                    query.Parameters.AddWithValue("@Anio", credito.Anio);
                    query.Parameters.AddWithValue("@Periodo", credito.Periodo);
                    query.Parameters.AddWithValue("@CodigoEmpresa", credito.CodigoEmpresa);
                    query.Parameters.AddWithValue("@CodigoTipoVale", credito.CodigoTipoVale);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idInsertado = 0;
            }

            return idInsertado;
        }
    }
}
