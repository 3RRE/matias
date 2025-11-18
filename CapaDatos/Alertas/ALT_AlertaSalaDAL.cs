using CapaEntidad;
using CapaEntidad.Alertas;
using CapaEntidad.ContadoresNegativos;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Alertas
{
    public class ALT_AlertaSalaDAL
    {
        string _conexion = string.Empty;
        public ALT_AlertaSalaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ALT_AlertaSalaEntidad> ALT_AlertaSala_Listado()
        {
            List<ALT_AlertaSalaEntidad> lista = new List<ALT_AlertaSalaEntidad>();
            string consulta = @"SELECT [alts_id]
                              ,[CodEmpresa]
                              ,[NombreEmpresa]
                              ,[CodSala]
                              ,[NombreSala]
                              ,[CodMaquina]
                              ,[CodMarcaMaquina]
                              ,[Juego]
                              ,[fecha_registro]
                              ,[fecha_termino]
                              ,[cod_tipo_alerta]
                              ,[descripcion_alerta]
                              ,[ColorAlerta]
                              ,[contador_bill_parcial]
                              ,[contador_bill_billetero]
                              ,[estado]
                              ,[alts_fechareg]
                          FROM ALT_AlertaSala order by alts_id desc";
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
                            var campaña = new ALT_AlertaSalaEntidad
                            {
                                alts_id = ManejoNulos.ManageNullInteger64(dr["alts_id"]),
                                CodEmpresa = ManejoNulos.ManageNullStr(dr["CodEmpresa"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                CodSala = ManejoNulos.ManageNullStr(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                CodMarcaMaquina = ManejoNulos.ManageNullStr(dr["CodMarcaMaquina"]),
                                Juego = ManejoNulos.ManageNullStr(dr["Juego"]),
                                fecha_registro = ManejoNulos.ManageNullStr(dr["fecha_registro"]),
                                fecha_termino = ManejoNulos.ManageNullStr(dr["fecha_termino"]),
                                cod_tipo_alerta = ManejoNulos.ManageNullInteger(dr["cod_tipo_alerta"]),
                                descripcion_alerta = ManejoNulos.ManageNullStr(dr["descripcion_alerta"]),
                                ColorAlerta = ManejoNulos.ManageNullStr(dr["ColorAlerta"]),
                                contador_bill_parcial = ManejoNulos.ManageNullDecimal(dr["contador_bill_parcial"]),
                                contador_bill_billetero = ManejoNulos.ManageNullDecimal(dr["contador_bill_billetero"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                alts_fechareg = ManejoNulos.ManageNullDate(dr["alts_fechareg"]),

                            };

                            lista.Add(campaña);
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

        public List<ALT_AlertaSalaEntidad> ALT_AlertaSala_xsala_idListado(string CodSala, DateTime fechaini, DateTime fechafin)
        {
            List<ALT_AlertaSalaEntidad> lista = new List<ALT_AlertaSalaEntidad>();
            string consulta = @"SELECT [alts_id]
                              ,[CodEmpresa]
                              ,[NombreEmpresa]
                              ,[CodSala]
                              ,[NombreSala]
                              ,[CodMaquina]
                              ,[CodMarcaMaquina]
                              ,[Juego]
                              ,[fecha_registro]
                              ,[fecha_termino]
                              ,[cod_tipo_alerta]
                              ,[descripcion_alerta]
                              ,[ColorAlerta]
                              ,[contador_bill_parcial]
                              ,[contador_bill_billetero]
                              ,[estado]
                              ,[alts_fechareg]
                          FROM ALT_AlertaSala where " + CodSala+ " CONVERT(date, alts_fechareg) between @p0 and @p1  order by alts_id desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaini);
                    query.Parameters.AddWithValue("@p1", fechafin);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var campaña = new ALT_AlertaSalaEntidad
                            {
                                alts_id = ManejoNulos.ManageNullInteger64(dr["alts_id"]),
                                CodEmpresa = ManejoNulos.ManageNullStr(dr["CodEmpresa"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                CodSala = ManejoNulos.ManageNullStr(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                CodMarcaMaquina = ManejoNulos.ManageNullStr(dr["CodMarcaMaquina"]),
                                Juego = ManejoNulos.ManageNullStr(dr["Juego"]),
                                fecha_registro = ManejoNulos.ManageNullStr(dr["fecha_registro"]),
                                fecha_termino = ManejoNulos.ManageNullStr(dr["fecha_termino"]),
                                cod_tipo_alerta = ManejoNulos.ManageNullInteger(dr["cod_tipo_alerta"]),
                                descripcion_alerta = ManejoNulos.ManageNullStr(dr["descripcion_alerta"]),
                                ColorAlerta = ManejoNulos.ManageNullStr(dr["ColorAlerta"]),
                                contador_bill_parcial = ManejoNulos.ManageNullDecimal(dr["contador_bill_parcial"]),
                                contador_bill_billetero = ManejoNulos.ManageNullDecimal(dr["contador_bill_billetero"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                alts_fechareg = ManejoNulos.ManageNullDate(dr["alts_fechareg"]),

                            };

                            lista.Add(campaña);
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

        public List<ALT_AlertaSalaEntidad> ALT_AlertaSala_xsala_idFechaListado(int CodSala, DateTime fechaini, DateTime fechafin)
        {
            List<ALT_AlertaSalaEntidad> lista = new List<ALT_AlertaSalaEntidad>();
            string consulta = @"SELECT [alts_id],
                                AlertaID,
                              [CodEmpresa]
                              ,[NombreEmpresa]
                              ,[CodSala]
                              ,[NombreSala]
                              ,[CodMaquina]
                              ,[CodMarcaMaquina]
                              ,[Juego]
                              ,[fecha_registro]
                              ,[fecha_termino]
                              ,[cod_tipo_alerta]
                              ,[descripcion_alerta]
                              ,[ColorAlerta]
                              ,[contador_bill_parcial]
                              ,[contador_bill_billetero]
                              ,[estado]
                              ,[alts_fechareg]
                          FROM ALT_AlertaSala where CodSala=@p0 and estado=1 and CONVERT(date, alts_fechareg) between  CONVERT(date, @p1)  and CONVERT(date, @p2)  order by alts_id desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", CodSala);
                    query.Parameters.AddWithValue("@p1", fechaini);
                    query.Parameters.AddWithValue("@p2", fechafin);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var campaña = new ALT_AlertaSalaEntidad
                            {
                                alts_id = ManejoNulos.ManageNullInteger64(dr["alts_id"]),
                                AlertaID = ManejoNulos.ManageNullInteger64(dr["AlertaID"]),
                                CodEmpresa = ManejoNulos.ManageNullStr(dr["CodEmpresa"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                CodSala = ManejoNulos.ManageNullStr(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                CodMarcaMaquina = ManejoNulos.ManageNullStr(dr["CodMarcaMaquina"]),
                                Juego = ManejoNulos.ManageNullStr(dr["Juego"]),
                                fecha_registro = ManejoNulos.ManageNullStr(dr["fecha_registro"]),
                                fecha_termino = ManejoNulos.ManageNullStr(dr["fecha_termino"]),
                                cod_tipo_alerta = ManejoNulos.ManageNullInteger(dr["cod_tipo_alerta"]),
                                descripcion_alerta = ManejoNulos.ManageNullStr(dr["descripcion_alerta"]),
                                ColorAlerta = ManejoNulos.ManageNullStr(dr["ColorAlerta"]),
                                contador_bill_parcial = ManejoNulos.ManageNullDecimal(dr["contador_bill_parcial"]),
                                contador_bill_billetero = ManejoNulos.ManageNullDecimal(dr["contador_bill_billetero"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                alts_fechareg = ManejoNulos.ManageNullDate(dr["alts_fechareg"]),

                            };

                            lista.Add(campaña);
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

        public List<ALT_AlertaDeviceEntidad> ALT_AlertaSala_xdevicesListado(int codsala)
        {
            List<ALT_AlertaDeviceEntidad> lista = new List<ALT_AlertaDeviceEntidad>();
            //string consulta = @"SELECT  [emd_id]
            //                      ,[emd_imei]
            //                      ,[emp_id]
            //                      ,[emd_estado]
            //                   ,emd_firebaseid
            //                   ,e.CargoID
            //                   ,cargoalerta.sala_id
            //                  FROM [EmpleadoDispositivo] ed
            //                  join SEG_Empleado e on e.EmpleadoID=ed.emp_id
            //                  join ALT_AlertaCargoConfiguracion cargoalerta on cargoalerta.cargo_id=e.CargoID
            //                  where cargoalerta.sala_id =@p0  order by emd_id desc";
            string consulta = @"SELECT  [emd_id]
                                  ,[emd_imei]
                                  ,[emp_id]
                                  ,[emd_estado]
	                              ,emd_firebaseid
	                              ,e.CargoID
	                              ,cargoalerta.sala_id,
                                  cargoalerta.tipo
                              FROM [EmpleadoDispositivo] ed
                              join SEG_Empleado e on e.EmpleadoID=ed.emp_id
                              join ALT_AlertaCargoConfiguracion cargoalerta on cargoalerta.cargo_id=e.CargoID
							  join SEG_Usuario usu on usu.EmpleadoID=ed.emp_id
							  join UsuarioSala ususala on ususala.UsuarioId=usu.UsuarioID
                              where cargoalerta.sala_id =@p0 and ususala.SalaId=@p1 and ed.emd_firebaseid IS not NUll order by emd_id desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codsala);
                    query.Parameters.AddWithValue("@p1", codsala);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var campaña = new ALT_AlertaDeviceEntidad
                            {
                                emd_id = ManejoNulos.ManageNullInteger64(dr["emd_id"]),
                                emd_imei = ManejoNulos.ManageNullStr(dr["emd_imei"]),
                                emp_id = ManejoNulos.ManageNullInteger(dr["emp_id"]),
                                id = ManejoNulos.ManageNullStr(dr["emd_firebaseid"]),
                                CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                tipo = ManejoNulos.ManageNullInteger(dr["tipo"]),

                            };

                            lista.Add(campaña);
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

        public ALT_AlertaSalaEntidad ALT_AlertasalaIdObtenerJson(Int64 alts_id)
        {
            ALT_AlertaSalaEntidad alertaSalaEntidad = new ALT_AlertaSalaEntidad();
            string consulta = @"SELECT 
                                alts_id , 
                                CodEmpresa , 
                                NombreEmpresa , 
                                CodSala , 
                                NombreSala , 
                                CodMaquina , 
                                CodMarcaMaquina , 
                                Juego , 
                                fecha_registro , 
                                fecha_termino , 
                                cod_tipo_alerta , 
                                descripcion_alerta , 
                                ColorAlerta , 
                                contador_bill_parcial , 
                                contador_bill_billetero , 
                                estado , 
                                alts_fechareg
	                            FROM ALT_AlertaSala
                                where AlertaID=@p0;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", alts_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                alertaSalaEntidad.alts_id = ManejoNulos.ManageNullInteger64(dr["alts_id"]);
                                alertaSalaEntidad.CodEmpresa = ManejoNulos.ManageNullStr(dr["CodEmpresa"]);
                                alertaSalaEntidad.NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]);
                                alertaSalaEntidad.CodSala = ManejoNulos.ManageNullStr(dr["CodSala"]);
                                alertaSalaEntidad.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                                alertaSalaEntidad.CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]);
                                alertaSalaEntidad.CodMarcaMaquina = ManejoNulos.ManageNullStr(dr["CodMarcaMaquina"]);
                                alertaSalaEntidad.Juego = ManejoNulos.ManageNullStr(dr["Juego"]);
                                alertaSalaEntidad.fecha_registro = ManejoNulos.ManageNullStr(dr["fecha_registro"]);
                                alertaSalaEntidad.fecha_termino = ManejoNulos.ManageNullStr(dr["fecha_termino"]);
                                alertaSalaEntidad.cod_tipo_alerta = ManejoNulos.ManageNullInteger(dr["cod_tipo_alerta"]);
                                alertaSalaEntidad.descripcion_alerta = ManejoNulos.ManageNullStr(dr["descripcion_alerta"]);
                                alertaSalaEntidad.ColorAlerta = ManejoNulos.ManageNullStr(dr["ColorAlerta"]);
                                alertaSalaEntidad.contador_bill_parcial = ManejoNulos.ManageNullDecimal(dr["contador_bill_parcial"]);
                                alertaSalaEntidad.contador_bill_billetero = ManejoNulos.ManageNullDecimal(dr["contador_bill_billetero"]);
                                alertaSalaEntidad.estado = ManejoNulos.ManageNullInteger(dr["estado"]);
                                alertaSalaEntidad.alts_fechareg = ManejoNulos.ManageNullDate(dr["alts_fechareg"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return alertaSalaEntidad;
        }

        public ALT_AlertaSalaEntidad ALT_AlertasalaAlertaIdObtenerJson(Int64 alertaid,int CodSala)
        {
            ALT_AlertaSalaEntidad alertaSalaEntidad = new ALT_AlertaSalaEntidad();
            string consulta = @"SELECT 
                                alts_id ,
                                AlertaID,
                                CodEmpresa , 
                                NombreEmpresa , 
                                CodSala , 
                                NombreSala , 
                                CodMaquina , 
                                CodMarcaMaquina , 
                                Juego , 
                                fecha_registro , 
                                fecha_termino , 
                                cod_tipo_alerta , 
                                descripcion_alerta , 
                                ColorAlerta , 
                                contador_bill_parcial , 
                                contador_bill_billetero , 
                                estado , 
                                alts_fechareg
	                            FROM ALT_AlertaSala
                                where AlertaID=@p0 and CodSala=@CodSala;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", alertaid);
                    query.Parameters.AddWithValue("@CodSala", CodSala);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                alertaSalaEntidad.alts_id = ManejoNulos.ManageNullInteger64(dr["alts_id"]);
                                alertaSalaEntidad.AlertaID = ManejoNulos.ManageNullInteger64(dr["AlertaID"]);
                                alertaSalaEntidad.CodEmpresa = ManejoNulos.ManageNullStr(dr["CodEmpresa"]);
                                alertaSalaEntidad.NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]);
                                alertaSalaEntidad.CodSala = ManejoNulos.ManageNullStr(dr["CodSala"]);
                                alertaSalaEntidad.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                                alertaSalaEntidad.CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]);
                                alertaSalaEntidad.CodMarcaMaquina = ManejoNulos.ManageNullStr(dr["CodMarcaMaquina"]);
                                alertaSalaEntidad.Juego = ManejoNulos.ManageNullStr(dr["Juego"]);
                                alertaSalaEntidad.fecha_registro = ManejoNulos.ManageNullStr(dr["fecha_registro"]);
                                alertaSalaEntidad.fecha_termino = ManejoNulos.ManageNullStr(dr["fecha_termino"]);
                                alertaSalaEntidad.cod_tipo_alerta = ManejoNulos.ManageNullInteger(dr["cod_tipo_alerta"]);
                                alertaSalaEntidad.descripcion_alerta = ManejoNulos.ManageNullStr(dr["descripcion_alerta"]);
                                alertaSalaEntidad.ColorAlerta = ManejoNulos.ManageNullStr(dr["ColorAlerta"]);
                                alertaSalaEntidad.contador_bill_parcial = ManejoNulos.ManageNullDecimal(dr["contador_bill_parcial"]);
                                alertaSalaEntidad.contador_bill_billetero = ManejoNulos.ManageNullDecimal(dr["contador_bill_billetero"]);
                                alertaSalaEntidad.estado = ManejoNulos.ManageNullInteger(dr["estado"]);
                                alertaSalaEntidad.alts_fechareg = ManejoNulos.ManageNullDate(dr["alts_fechareg"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return alertaSalaEntidad;
        }

        public int ALT_AlertasalaInsertarJson(ALT_AlertaSalaEntidad AlertaCargo)
        {
            //bool response = false;
            int idempleadoDispositivoInsertado = 0;
            string consulta = @"
            INSERT INTO ALT_AlertaSala(CodEmpresa, NombreEmpresa, CodSala,NombreSala,CodMaquina,CodMarcaMaquina,Juego,fecha_registro,fecha_termino,cod_tipo_alerta,descripcion_alerta,ColorAlerta,contador_bill_parcial,contador_bill_billetero,estado,alts_fechareg,AlertaID)
	            VALUES (@p0, @p1, @p2,@p3, @p4, @p5, @p6,@p7, @p8, @p9, @p10,@p11, @p12, @p13, @p14,@p15,@p16) 
                SELECT SCOPE_IDENTITY()";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(AlertaCargo.CodEmpresa));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(AlertaCargo.NombreEmpresa));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(AlertaCargo.CodSala));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(AlertaCargo.NombreSala));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(AlertaCargo.CodMaquina));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(AlertaCargo.CodMarcaMaquina));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(AlertaCargo.Juego));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(AlertaCargo.fecha_registro));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(AlertaCargo.fecha_termino));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(AlertaCargo.cod_tipo_alerta));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(AlertaCargo.descripcion_alerta));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(AlertaCargo.ColorAlerta));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullDecimal(AlertaCargo.contador_bill_parcial));
                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullDecimal(AlertaCargo.contador_bill_billetero));
                    query.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullInteger(AlertaCargo.estado));
                    query.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullDate(AlertaCargo.alts_fechareg));
                    query.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullInteger64(AlertaCargo.AlertaID));
                    idempleadoDispositivoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return idempleadoDispositivoInsertado;
        }

        public bool ALT_AlertasalaEditarJson(ALT_AlertaSalaEntidad AlertaCargo)
        {

            bool response = false;
            string consulta = @"UPDATE ALT_AlertaSala
	                            SET fecha_termino=@p0,estado=@p1
	                            WHERE alts_id=@p2;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(AlertaCargo.fecha_termino));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(AlertaCargo.estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger64(AlertaCargo.alts_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }

        public bool ALT_AlertasalaEliminarJson(Int64 alts_id)
        {
            bool response = false;
            string consulta = @"Delete from [ALT_AlertaSala]
                 WHERE alts_id=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", alts_id);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }

        public int ConsultarAlertasCargo(int codSala)
        {
            int tipo = 0;
            string consulta = "select top(1) [tipo] from ALT_AlertaCargoConfiguracion where sala_id=" + codSala;
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {

                        if (dr.Read()) // Verifica si hay resultados antes de intentar acceder a los datos
                        {
                            tipo = ManejoNulos.ManageNullInteger(dr["tipo"]);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tipo;
        }


        public bool CambiarTipoAlerta(int codSala, int tipo)
        {
            bool response = false;
            string consulta = "update  ALT_AlertaCargoConfiguracion set tipo = @p0 where sala_id =@p1" ;
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger64(codSala));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }


        public List<string> AlertaEventosCorreos(int codsala) {
            List<string> lista = new List<string>();
            string consulta = @"SELECT DISTINCT
                                    e.MailJob,
                                    cargoDisco.tipo
                                FROM 
                                    SEG_Empleado e 
                                    JOIN ALT_AlertaCargoConfiguracion cargoDisco ON cargoDisco.cargo_id = e.CargoID
                                    JOIN SEG_Usuario usu ON usu.EmpleadoID = e.EmpleadoID
                                    JOIN UsuarioSala ususala ON ususala.UsuarioId = usu.UsuarioID
                                WHERE 
                                    cargoDisco.sala_id = @p0 
                                    AND ususala.SalaId = @p1 
                                    AND (cargoDisco.tipo = 3 OR cargoDisco.tipo = 2) ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codsala);
                    query.Parameters.AddWithValue("@p1", codsala);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campania = new AlertaContadorNegativoEntidad {

                                mailJob = ManejoNulos.ManageNullStr(dr["MailJob"]),

                            };

                            lista.Add(campania.mailJob);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }


        public ContadorCorreoAlertaEntidad ObtenerValorContador() {
            ContadorCorreoAlertaEntidad contador = new ContadorCorreoAlertaEntidad();
            string consulta = @"select Id,Contador, FechaActual from ContadorCorreos;";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                contador.Id = ManejoNulos.ManageNullInteger(dr["id"]);
                                contador.Contador = ManejoNulos.ManageNullInteger(dr["Contador"]);
                                contador.Fecha = ManejoNulos.ManageNullDate(dr["FechaActual"]);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return contador;
        }
        public void ResetearContador(DateTime fecha) {
            string consulta = $"UPDATE ContadorCorreos SET Contador = 0, FechaActual = '{fecha.ToString("yyyy-MM-dd")}'";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.ExecuteNonQuery();
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
        public void AgregarContador() {
            string consulta = $"update ContadorCorreos set Contador = contador + 1 where Id = 1";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.ExecuteNonQuery();
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }


        public List<string> AlertBilleterosCorreos(int codsala) {
            List<string> lista = new List<string>();
            string consulta = @"SELECT DISTINCT
                                    e.MailJob,
                                    cargoDisco.tipo
                                FROM 
                                    SEG_Empleado e 
                                    JOIN ALT_AlertaCargoConfiguracion cargoDisco ON cargoDisco.cargo_id = e.CargoID
                                    JOIN SEG_Usuario usu ON usu.EmpleadoID = e.EmpleadoID
                                    JOIN UsuarioSala ususala ON ususala.UsuarioId = usu.UsuarioID
                                WHERE 
                                    cargoDisco.sala_id = @p0 
                                    AND ususala.SalaId = @p1 
                                    AND (cargoDisco.tipo = 3 OR cargoDisco.tipo = 1) ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codsala);
                    query.Parameters.AddWithValue("@p1", codsala);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campania = new AlertaContadorNegativoEntidad {

                                mailJob = ManejoNulos.ManageNullStr(dr["MailJob"]),

                            };

                            lista.Add(campania.mailJob);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public List<AlertBillNotificationReqEntidad> ListarAlertBillNotificationSala(string roomCode, int status, int days)
        {
            List<AlertBillNotificationReqEntidad> list = new List<AlertBillNotificationReqEntidad>();

            string query = @"
            SELECT
                altsala.AlertaID,
	            altsala.CodEmpresa,
	            altsala.NombreEmpresa,
	            altsala.CodSala,
	            altsala.NombreSala,
	            altsala.CodMaquina,
	            altsala.Juego,
	            altsala.fecha_registro,
	            altsala.fecha_termino,
                altsala.estado,
	            altsala.cod_tipo_alerta,
	            altsala.descripcion_alerta,
	            altsala.ColorAlerta,
	            altsala.contador_bill_parcial,
	            altsala.contador_bill_billetero
            FROM ALT_AlertaSala altsala WITH (NOLOCK)
            WHERE altsala.CodSala = @w1 AND altsala.estado = @w2 AND altsala.alts_fechareg >= CONVERT(DATE, (DATEADD(DAY, -@w3, GETDATE())))
            ORDER BY altsala.AlertaID DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", roomCode);
                    command.Parameters.AddWithValue("@w2", status);
                    command.Parameters.AddWithValue("@w3", days);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            AlertBillNotificationReqEntidad alerta = new AlertBillNotificationReqEntidad
                            {
                                AlertaID = ManejoNulos.ManageNullInteger(data["AlertaID"]),
                                CodEmpresa = ManejoNulos.ManageNullStr(data["CodEmpresa"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(data["NombreEmpresa"]),
                                CodSala = ManejoNulos.ManageNullStr(data["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(data["NombreSala"]),
                                CodMaquina = ManejoNulos.ManageNullStr(data["CodMaquina"]),
                                Juego = ManejoNulos.ManageNullStr(data["Juego"]),
                                fecha_registro = ManejoNulos.ManageNullStr(data["fecha_registro"]),
                                fecha_termino = ManejoNulos.ManageNullStr(data["fecha_termino"]),
                                estado = ManejoNulos.ManageNullStr(data["estado"]),
                                cod_tipo_alerta = ManejoNulos.ManageNullInteger(data["cod_tipo_alerta"]),
                                descripcion_alerta = ManejoNulos.ManageNullStr(data["descripcion_alerta"]),
                                ColorAlerta = ManejoNulos.ManageNullStr(data["ColorAlerta"]),
                                contador_bill_parcial = ManejoNulos.ManageNullDouble(data["contador_bill_parcial"]),
                                contador_bill_billetero = ManejoNulos.ManageNullDouble(data["contador_bill_billetero"])
                            };

                            list.Add(alerta);
                        }
                    }
                }
            }
            catch(Exception)
            {
                list = new List<AlertBillNotificationReqEntidad>();
            }

            return list;
        }

        #region Destinatarios Online

        public List<WEB_DestinatarioEntidad> ObtenerDestinatariosOnline(int salaId)
        {
            List<WEB_DestinatarioEntidad> list = new List<WEB_DestinatarioEntidad>();

            string query = @"
            SELECT
	            empleado.Nombres + ' ' + empleado.ApellidosPaterno + ' ' + empleado.ApellidosMaterno AS Titular,
	            empleado.MailJob AS Correo
            FROM
            SEG_Empleado empleado
            JOIN ALT_AlertaCargoConfiguracion alertacargo ON alertacargo.cargo_id = empleado.CargoID
            JOIN SEG_Usuario usuario ON usuario.EmpleadoID = empleado.EmpleadoID
            JOIN UsuarioSala usuariosala ON usuariosala.UsuarioId = usuario.UsuarioID
            WHERE alertacargo.sala_id = @w1 AND usuariosala.SalaId = @w2
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", salaId);
                    command.Parameters.AddWithValue("@w2", salaId);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            WEB_DestinatarioEntidad destinatario = new WEB_DestinatarioEntidad
                            {
                                WEB_DestTitular = ManejoNulos.ManageNullStr(data["Titular"]),
                                WEB_DestCorreo = ManejoNulos.ManageNullStr(data["Correo"])
                            };

                            list.Add(destinatario);
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = new List<WEB_DestinatarioEntidad>();
            }

            return list;
        }

        #endregion
    }
}
