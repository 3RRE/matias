using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using CapaEntidad.EntradaSalidaSala;
using S3k.Utilitario;

namespace CapaDatos.EntradaSalidaSala {
    public class ESS_AperturaCierreSalaDAL {
        string _conexion = string.Empty;

        public ESS_AperturaCierreSalaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }


        public List<ESS_AperturaCierreSalaEntidad> ListarRegistroAperturaCierreSala(int[] codsala, DateTime fechaIni, DateTime fechaFin) {
            List<ESS_AperturaCierreSalaEntidad> lista = new List<ESS_AperturaCierreSalaEntidad>();
            string strSala = string.Empty;

            //if(codsala != null && codsala.Length > 0 && !codsala.Contains(-1)) {
            strSala = $" codsala in ({String.Join(",", codsala)}) and ";
            //}


            string consulta = $@"SELECT [IdAperturaCierreSala],
                                [CodSala],
                                [NombreSala],
                                [Fecha],
                                [HoraApertura],
                                [PrevencionistaApertura],
                                [JefeSalaApertura],
                                [IdPrevencionistaApertura],
                                [IdJefeSalaApertura],
                                [ObservacionesApertura],
                                [HoraCierre],
                                [PrevencionistaCierre],
                                [JefeSalaCierre],
                                 [IdPrevencionistaCierre],
                                [IdJefeSalaCierre],
                                [ObservacionesCierre],
                                [UsuarioRegistro],
                                [FechaRegistro],
                                [UsuarioModificacion],
                                [FechaModificacion],
                                [Estado]
                         FROM [ESS_AperturaCierreSala]
                         WHERE {strSala} CONVERT(DATE, Fecha) BETWEEN CONVERT(DATE, @p1) AND CONVERT(DATE, @p2)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_AperturaCierreSalaEntidad {
                                IdAperturaCierreSala = ManejoNulos.ManageNullInteger(dr["IdAperturaCierreSala"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Sala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                HoraApertura = ManejoNulos.ManageNullTimespan(dr["HoraApertura"]),
                                PrevencionistaApertura = ManejoNulos.ManageNullStr(dr["PrevencionistaApertura"]),
                                JefeSalaApertura = ManejoNulos.ManageNullStr(dr["JefeSalaApertura"]),
                                IdPrevencionistaApertura = ManejoNulos.ManageNullInteger(dr["IdPrevencionistaApertura"]),
                                IdJefeSalaApertura = ManejoNulos.ManageNullInteger(dr["IdJefeSalaApertura"]),
                                ObservacionesApertura = ManejoNulos.ManageNullStr(dr["ObservacionesApertura"]),
                                HoraCierre = ManejoNulos.ManageNullTimespan(dr["HoraCierre"]),
                                PrevencionistaCierre = ManejoNulos.ManageNullStr(dr["PrevencionistaCierre"]),
                                JefeSalaCierre = ManejoNulos.ManageNullStr(dr["JefeSalaCierre"]),
                                IdPrevencionistaCierre = ManejoNulos.ManageNullInteger(dr["IdPrevencionistaCierre"]),
                                IdJefeSalaCierre = ManejoNulos.ManageNullInteger(dr["IdJefeSalaCierre"]),
                                ObservacionesCierre = ManejoNulos.ManageNullStr(dr["ObservacionesCierre"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"])
                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_AperturaCierreSalaEntidad>();
            }

            return lista;
        }
        public DateTime? ObtenerFechaHoraCierrePorId(int IdAperturaCierreSala) {
            DateTime? fechaCierre = null;

            string consulta = @"SELECT Fecha, HoraCierre
                        FROM ESS_AperturaCierreSala
                        WHERE IdAperturaCierreSala = @IdAperturaCierreSala"; 
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdAperturaCierreSala", IdAperturaCierreSala);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.Read()) {
                            DateTime fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                            TimeSpan horaCierre = ManejoNulos.ManageNullTimespan(dr["HoraCierre"]);

                            if(horaCierre == TimeSpan.Zero) {
                                return null;
                            }

                            fechaCierre = fecha.Add(horaCierre);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return fechaCierre;
        }


        public int GuardarRegistroAperturaCierreSala(ESS_AperturaCierreSalaEntidad registro) {
            int idInsertado = 0;

            string query = @"INSERT INTO [ESS_AperturaCierreSala]
           ([CodSala]
           ,[NombreSala]
           ,[Fecha]
           ,[HoraApertura]
           ,[PrevencionistaApertura]
           ,[IdPrevencionistaApertura]
           ,[JefeSalaApertura]
           ,[IdJefeSalaApertura]
           ,[ObservacionesApertura]

           ,[IdEmpleadoSEG]
           ,[UsuarioRegistro]
           ,[FechaRegistro]
           ,[Estado])
        OUTPUT INSERTED.IdAperturaCierreSala
        VALUES
           (@CodSala
           ,@NombreSala
           ,@Fecha
           ,@HoraApertura
           ,@PrevencionistaApertura
           ,@IdPrevencionistaApertura
           ,@JefeSalaApertura
           ,@IdJefeSalaApertura
           ,@ObservacionesApertura

           ,@IdEmpleadoSEG
           ,@UsuarioRegistro
           ,@FechaRegistro
           ,@Estado)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();

                    using(var cmd = new SqlCommand(query, con)) {
    
                        cmd.Parameters.AddWithValue("@CodSala", registro.CodSala);
                        cmd.Parameters.AddWithValue("@NombreSala", registro.Sala);
                        cmd.Parameters.AddWithValue("@Fecha", registro.Fecha);
                        cmd.Parameters.AddWithValue("@HoraApertura", ManejoNulos.ManageNullTimespan(registro.HoraApertura));
                        cmd.Parameters.AddWithValue("@PrevencionistaApertura", ManejoNulos.ManageNullStr(registro.PrevencionistaApertura));
                        cmd.Parameters.AddWithValue("@JefeSalaApertura", ManejoNulos.ManageNullStr(registro.JefeSalaApertura));
                        cmd.Parameters.AddWithValue("@ObservacionesApertura", ManejoNulos.ManageNullStr(registro.ObservacionesApertura));
                        cmd.Parameters.AddWithValue("@IdEmpleadoSEG", ManejoNulos.ManageNullInteger(registro.IdEmpleadoSEG));
                        cmd.Parameters.AddWithValue("@IdPrevencionistaApertura", ManejoNulos.ManageNullInteger(registro.IdPrevencionistaApertura));
                        cmd.Parameters.AddWithValue("@IdJefeSalaApertura", ManejoNulos.ManageNullInteger(registro.IdJefeSalaApertura));
                        cmd.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                        cmd.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));
                        cmd.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(registro.Estado));

                        idInsertado = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error al guardar: {ex.Message}");
                idInsertado = 0;
            }

            return idInsertado;
        }
        public int GuardarRegistroAperturaCierreSala_Importar(ESS_AperturaCierreSalaEntidad registro)
        {
            int idInsertado = 0;

            string query = @"INSERT INTO [ESS_AperturaCierreSala]
           ([CodSala]
           ,[NombreSala]
           ,[Fecha]

		    ,[HoraApertura]
		    ,[PrevencionistaApertura]
		    ,[IdPrevencionistaApertura] 
		    ,[JefeSalaApertura]
		    ,[IdJefeSalaApertura]
		    ,[ObservacionesApertura]
 
		    ,HoraCierre
		    ,PrevencionistaCierre
		    ,IdPrevencionistaCierre 
		    ,JefeSalaCierre
		    ,IdJefeSalaCierre
		    ,ObservacionesCierre

           ,[UsuarioRegistro]
           ,[FechaRegistro] )
        OUTPUT INSERTED.IdAperturaCierreSala
        VALUES
           (@CodSala
           ,@NombreSala
           ,@Fecha

           ,@HoraApertura
           ,@PrevencionistaApertura
           ,@IdPrevencionistaApertura 
           ,@JefeSalaApertura
           ,@IdJefeSalaApertura
           ,@ObservacionesApertura
  
		   ,@HoraCierre
		   ,@PrevencionistaCierre
		   ,@IdPrevencionistaCierre 
		   ,@JefeSalaCierre
		   ,@IdJefeSalaCierre
		   ,@ObservacionesCierre

           ,@UsuarioRegistro
           ,@FechaRegistro )";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();

                    using (var cmd = new SqlCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("@CodSala", registro.CodSala);
                        cmd.Parameters.AddWithValue("@NombreSala", registro.Sala);
                        cmd.Parameters.AddWithValue("@Fecha", registro.Fecha);

                        cmd.Parameters.AddWithValue("@HoraApertura", ManejoNulos.ManageNullTimespan(registro.HoraApertura));
                        cmd.Parameters.AddWithValue("@IdPrevencionistaApertura", ManejoNulos.ManageNullInteger(registro.IdPrevencionistaApertura));
                        cmd.Parameters.AddWithValue("@PrevencionistaApertura", ManejoNulos.ManageNullStr(registro.PrevencionistaApertura));
                        cmd.Parameters.AddWithValue("@IdJefeSalaApertura", ManejoNulos.ManageNullInteger(registro.IdJefeSalaApertura));
                        cmd.Parameters.AddWithValue("@JefeSalaApertura", ManejoNulos.ManageNullStr(registro.JefeSalaApertura));
                        cmd.Parameters.AddWithValue("@ObservacionesApertura", ManejoNulos.ManageNullStr(registro.ObservacionesApertura));

                        cmd.Parameters.AddWithValue("@HoraCierre", ManejoNulos.ManageNullTimespan(registro.HoraCierre));
                        cmd.Parameters.AddWithValue("@IdPrevencionistaCierre", ManejoNulos.ManageNullInteger(registro.IdPrevencionistaCierre));
                        cmd.Parameters.AddWithValue("@PrevencionistaCierre", ManejoNulos.ManageNullStr(registro.PrevencionistaCierre));
                        cmd.Parameters.AddWithValue("@IdJefeSalaCierre", ManejoNulos.ManageNullInteger(registro.IdJefeSalaCierre));
                        cmd.Parameters.AddWithValue("@JefeSalaCierre", ManejoNulos.ManageNullStr(registro.JefeSalaCierre));
                        cmd.Parameters.AddWithValue("@ObservacionesCierre", ManejoNulos.ManageNullStr(registro.ObservacionesCierre));
                         
                        cmd.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                        cmd.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));

                        idInsertado = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar: {ex.Message}");
                idInsertado = 0;
            }

            return idInsertado;
        }


        public bool ActualizarAperturaCierreSala(ESS_AperturaCierreSalaEntidad registro) {
            string query = @"UPDATE [ESS_AperturaCierreSala]
                     SET [CodSala] = @CodSala,
                         [NombreSala] = @NombreSala,
                         [Fecha] = @Fecha,
                         [HoraApertura] = @HoraApertura,
                         [PrevencionistaApertura] = @PrevencionistaApertura,
                         [JefeSalaApertura] = @JefeSalaApertura, 
                         [IdPrevencionistaApertura] = @IdPrevencionistaApertura,
                         [IdJefeSalaApertura] = @IdJefeSalaApertura,
                         [ObservacionesApertura] = @ObservacionesApertura,
                
                         
                         [UsuarioModificacion] = @UsuarioModificacion,
                         [FechaModificacion] = @FechaModificacion
                     WHERE [IdAperturaCierreSala] = @IdAperturaCierreSala";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(query, con)) {
          
                        cmd.Parameters.AddWithValue("@IdAperturaCierreSala", registro.IdAperturaCierreSala);
                        cmd.Parameters.AddWithValue("@CodSala", registro.CodSala);
                        cmd.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.Sala));
                        cmd.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(registro.Fecha)); 
                        cmd.Parameters.AddWithValue("@HoraApertura", ManejoNulos.ManageNullTimespan(registro.HoraApertura)); 
                        cmd.Parameters.AddWithValue("@PrevencionistaApertura", ManejoNulos.ManageNullStr(registro.PrevencionistaApertura));
                        cmd.Parameters.AddWithValue("@JefeSalaApertura", ManejoNulos.ManageNullStr(registro.JefeSalaApertura));
                        cmd.Parameters.AddWithValue("@IdPrevencionistaApertura", ManejoNulos.ManageNullInteger(registro.IdPrevencionistaApertura));
                        cmd.Parameters.AddWithValue("@IdJefeSalaApertura", ManejoNulos.ManageNullInteger(registro.IdJefeSalaApertura));
                        cmd.Parameters.AddWithValue("@ObservacionesApertura", ManejoNulos.ManageNullStr(registro.ObservacionesApertura));
                        //cmd.Parameters.AddWithValue("@HoraCierre", ManejoNulos.ManageNullTimespan(registro.HoraCierre)); 
                        //cmd.Parameters.AddWithValue("@PrevencionistaCierre", ManejoNulos.ManageNullStr(registro.PrevencionistaCierre));
                        //cmd.Parameters.AddWithValue("@JefeSalaCierre", ManejoNulos.ManageNullStr(registro.JefeSalaCierre));
                        //cmd.Parameters.AddWithValue("@ObservacionesCierre", ManejoNulos.ManageNullStr(registro.ObservacionesCierre));
                        cmd.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(registro.UsuarioModificacion));
                        cmd.Parameters.AddWithValue("@FechaModificacion", DateTime.Now); 

                      
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            } catch(Exception ex) {
                Console.WriteLine(ex.Message); 
                return false;
            }
        }

        public bool FinalizarRegistroAperturaCierreSala(ESS_AperturaCierreSalaEntidad registro) {
 
               
                string query = @"UPDATE [ESS_AperturaCierreSala]
                     SET [CodSala] = @CodSala,
                         [NombreSala] = @NombreSala,
                         [HoraCierre] = @HoraCierre,
                         [PrevencionistaCierre] = @PrevencionistaCierre,
                         [JefeSalaCierre] = @JefeSalaCierre,
                         [IdPrevencionistaCierre] = @IdPrevencionistaCierre,
                         [IdJefeSalaCierre] = @IdJefeSalaCierre,
                         [ObservacionesCierre] = @ObservacionesCierre,
                         [UsuarioModificacion] = @UsuarioModificacion,
                         [FechaModificacion] = @FechaModificacion
                     WHERE [IdAperturaCierreSala] = @IdAperturaCierreSala";

                try {
                    using(var con = new SqlConnection(_conexion)) {
                        con.Open();
                        using(var cmd = new SqlCommand(query, con)) {

                            cmd.Parameters.AddWithValue("@IdAperturaCierreSala", registro.IdAperturaCierreSala);
                            cmd.Parameters.AddWithValue("@CodSala", registro.CodSala);
                            cmd.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.Sala));
                            cmd.Parameters.AddWithValue("@HoraCierre", ManejoNulos.ManageNullTimespan(registro.HoraCierre));
                            cmd.Parameters.AddWithValue("@PrevencionistaCierre", ManejoNulos.ManageNullStr(registro.PrevencionistaCierre));
                            cmd.Parameters.AddWithValue("@JefeSalaCierre", ManejoNulos.ManageNullStr(registro.JefeSalaCierre));

                        cmd.Parameters.AddWithValue("@IdPrevencionistaCierre", ManejoNulos.ManageNullStr(registro.IdPrevencionistaCierre));
                        cmd.Parameters.AddWithValue("@IdJefeSalaCierre", ManejoNulos.ManageNullStr(registro.IdJefeSalaCierre));

                        cmd.Parameters.AddWithValue("@ObservacionesCierre", ManejoNulos.ManageNullStr(registro.ObservacionesCierre));
                            cmd.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(registro.UsuarioModificacion));
                            cmd.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);


                            cmd.ExecuteNonQuery();
                        }
                    }
                    return true;
                } catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }


        public bool EliminarRegistroAperturaCierreSala(int IdAperturaCierreSala) {
            string query = @"DELETE FROM [ESS_AperturaCierreSala]
                             WHERE [IdAperturaCierreSala] = @IdAperturaCierreSala";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(query, con)) {
                        cmd.Parameters.AddWithValue("@IdAperturaCierreSala", IdAperturaCierreSala);

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        return filasAfectadas > 0; 
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error al eliminar el registro: {ex.Message}");
                return false;
            }
        }

        public List<ESS_AperturaCierreSalaPersonaEntidad> ListarEmpleadoBUKcargo() {
            List<ESS_AperturaCierreSalaPersonaEntidad> lista = new List<ESS_AperturaCierreSalaPersonaEntidad>();
            string consulta = @"SELECT 
                            [IdBuk]
                            ,[NombreCompleto]
                            ,[Cargo]
                            ,IdCargo
                            ,NumeroDocumento
                        FROM [BUK_Empleado]";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_AperturaCierreSalaPersonaEntidad {
                                IdBuk = ManejoNulos.ManageNullInteger(dr["IdBuk"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Cargo = ManejoNulos.ManageNullStr(dr["Cargo"]),
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                lista = new List<ESS_AperturaCierreSalaPersonaEntidad>();
            }
            return lista;
        }


    }
}
