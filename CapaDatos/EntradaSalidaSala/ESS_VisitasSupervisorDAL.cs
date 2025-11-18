using CapaEntidad.EntradaSalidaSala;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaDatos.EntradaSalidaSala {
    public class ESS_VisitasSupervisorDAL {
        string _conexion = string.Empty;
        public ESS_VisitasSupervisorDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ESS_VisitasSupervisorEntidad> ListadoVisitasSupervisor(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            List<ESS_VisitasSupervisorEntidad> lista = new List<ESS_VisitasSupervisorEntidad>();
            string strSala = string.Empty;
            strSala = $" vs.CodSala in ({String.Join(",", codSala)}) and ";

            string consulta = $@"
        SELECT 
            vs.[IdVisitaSupervisor],
            vs.[CodSala],
            s.[Nombre] AS NombreSala,
            vs.[IdMotivo],
            m.[Nombre] AS NombreMotivo,
            vs.[OtroMotivo],
            vs.[FechaIngreso],
            vs.[FechaSalida],
            vs.[Observaciones],
            vs.[UsuarioRegistro],
            vs.[FechaRegistro]
        FROM [ESS_VisitaSupervisor] vs
        LEFT JOIN [Sala] s ON vs.CodSala = s.CodSala
        LEFT JOIN [ESS_VisitaSupervisorMotivo] m ON vs.IdMotivo = m.IdMotivo
        WHERE {strSala} convert(date, vs.FechaRegistro) between convert(date, @p1) and convert(date, @p2)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_VisitasSupervisorEntidad {
                                IdVisitaSupervisor = ManejoNulos.ManageNullInteger(dr["IdVisitaSupervisor"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                IdMotivo = ManejoNulos.ManageNullInteger(dr["IdMotivo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["NombreMotivo"]),
                                OtroMotivo = ManejoNulos.ManageNullStr(dr["OtroMotivo"]),
                                FechaIngreso = ManejoNulos.ManageNullDate(dr["FechaIngreso"]),
                                FechaSalida = ManejoNulos.ManageNullDate(dr["FechaSalida"]),
                                Observaciones = ManejoNulos.ManageNullStr(dr["Observaciones"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                            };
                            using(var conEmpleados = new SqlConnection(_conexion)) {
                                conEmpleados.Open();
                                item.Empleados = ObtenerEmpleadosPorIdVisitaSupervisor(item.IdVisitaSupervisor, conEmpleados);
                            }
                            lista.Add(item);
                        }
                    }
                }
      
      
                    
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_VisitasSupervisorEntidad>();
            }
            return lista;
        }

        private List<ESS_VisitaSupervisorDetalleEntidad> ObtenerEmpleadosPorIdVisitaSupervisor(int IdVisitaSupervisor, SqlConnection con) {
            List<ESS_VisitaSupervisorDetalleEntidad> empleados = new List<ESS_VisitaSupervisorDetalleEntidad>();

            string consultaEmpleados = @"
                SELECT
                e.IdBuk as IdEmpleado,
                e.Nombres as Nombre,
                e.ApellidoPaterno,
                e.ApellidoMaterno,
                e.NombreCompleto,
                e.IdCargo,
                e.Cargo,
                e.TipoDocumento,
                e.NumeroDocumento as DocumentoRegistro
                FROM ESS_VisitaSupervisorDetalle vsd
                JOIN BUK_Empleado e ON vsd.IdSupervisor = e.IdBuk
                WHERE vsd.IdVisitaSupervisor = @IdVisitaSupervisor";

            using(var cmd = new SqlCommand(consultaEmpleados, con)) {
                cmd.Parameters.AddWithValue("@IdVisitaSupervisor", IdVisitaSupervisor);

                using(var dr = cmd.ExecuteReader()) {
                    while(dr.Read()) {
                        var empleado = new ESS_VisitaSupervisorDetalleEntidad {
                            IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                            Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                            ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                            ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                            NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                            DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]),
                            IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                            TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                            Cargo = ManejoNulos.ManageNullStr(dr["Cargo"]),
                        };
                        empleados.Add(empleado);
                    }
                }
            }
            return empleados;
        }

      

        public bool EliminarVisitasSupervisor(int IdVisitaSupervisor) {
            bool respuesta = false;
            string consulta = "DELETE FROM ESS_VisitaSupervisor WHERE IdVisitaSupervisor = @IdVisitaSupervisor";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@IdVisitaSupervisor", IdVisitaSupervisor);
                        cmd.ExecuteNonQuery();
                    }
                }
                respuesta = true;
            } catch(Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
            }

            return respuesta;
        }
        public int GuardarVisitasSupervisor(ESS_VisitasSupervisorEntidad visitasSupervisor) {
            int idInsertado = 0;

            string queryBienMaterial = @"INSERT INTO [ESS_VisitaSupervisor]
           ([CodSala]
           ,[IdMotivo]
           ,[OtroMotivo]
           ,[FechaIngreso]
           ,FechaSalida
           ,[Observaciones]
           ,[UsuarioRegistro]
           ,[FechaRegistro])
        output inserted.IdVisitaSupervisor
             VALUES
           (@CodSala
           ,@IdMotivo
           ,@OtroMotivo
           ,@FechaIngreso
           ,@FechaSalida
           ,@Observaciones
           ,@UsuarioRegistro
           ,@FechaRegistro)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var transaction = con.BeginTransaction()) {

                        var queryBM = new SqlCommand(queryBienMaterial, con, transaction);
                        queryBM.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(visitasSupervisor.CodSala));
                        queryBM.Parameters.AddWithValue("@IdMotivo", ManejoNulos.ManageNullInteger(visitasSupervisor.IdMotivo));
                        queryBM.Parameters.AddWithValue("@OtroMotivo", ManejoNulos.ManageNullStr(visitasSupervisor.OtroMotivo));
                        queryBM.Parameters.AddWithValue("@FechaIngreso", ManejoNulos.ManageNullDate(visitasSupervisor.FechaIngreso));
                        queryBM.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(visitasSupervisor.Observaciones));
                        queryBM.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(visitasSupervisor.UsuarioRegistro));
                        queryBM.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(visitasSupervisor.FechaRegistro));

                        if(visitasSupervisor.FechaSalida == DateTime.MinValue) {
                            queryBM.Parameters.AddWithValue("@FechaSalida", DBNull.Value);
                        } else {
                            queryBM.Parameters.AddWithValue("@FechaSalida", ManejoNulos.ManageNullDate(visitasSupervisor.FechaSalida));
                        }

                        idInsertado = Convert.ToInt32(queryBM.ExecuteScalar());
                        GuardarDetalleVisitasSupervisor(visitasSupervisor.Empleados, idInsertado, visitasSupervisor.UsuarioRegistro, visitasSupervisor.FechaRegistro, con, transaction);
                        transaction.Commit();
                    }
                }
            } catch(Exception ex) {
                idInsertado = 0;
            }
            return idInsertado;
        }


        private void GuardarDetalleVisitasSupervisor(List<ESS_VisitaSupervisorDetalleEntidad> empleados, int idVisitaSupervisor, string usuarioRegistro, DateTime fechaRegistro, SqlConnection con, SqlTransaction transaction) {
            string queryDetalle = @"INSERT INTO ESS_VisitaSupervisorDetalle
        ([IdVisitaSupervisor]
        ,[IdSupervisor]
        ,[UsuarioRegistro]
        ,[FechaRegistro])
        VALUES
        (@IdVisitaSupervisor
        ,@IdSupervisor
        ,@UsuarioRegistro
        ,@FechaRegistro)";
            foreach(var empleado in empleados) {
                var queryDet = new SqlCommand(queryDetalle, con, transaction);
                queryDet.Parameters.AddWithValue("@IdVisitaSupervisor", ManejoNulos.ManageNullInteger(idVisitaSupervisor));
                queryDet.Parameters.AddWithValue("@IdSupervisor", ManejoNulos.ManageNullInteger(empleado.IdEmpleado));
                queryDet.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(usuarioRegistro));
                queryDet.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(fechaRegistro));
                queryDet.ExecuteNonQuery();
            }
        }

        public bool EditarVisitasSupervisor(ESS_VisitasSupervisorEntidad registro) {
            bool respuesta = false;
            string consulta = @"UPDATE [ESS_VisitaSupervisor]
                        SET [CodSala] = @CodSala,
                            [IdMotivo] = @IdMotivo,
                            [OtroMotivo] = @OtroMotivo,
                            [FechaIngreso] = @FechaIngreso,
                             FechaSalida = @FechaSalida,
                            [Observaciones] = @Observaciones, 
                            [UsuarioModificacion] = @UsuarioModificacion,
                            [FechaModificacion] = @FechaModificacion
                        WHERE [IdVisitaSupervisor] = @IdVisitaSupervisor";

            string consultaEliminarEmpleados = @"
            DELETE FROM ESS_VisitaSupervisorDetalle
            WHERE IdVisitaSupervisor = @IdVisitaSupervisor";

            string consultaInsertarEmpleado = @"
            INSERT INTO ESS_VisitaSupervisorDetalle
            ([IdVisitaSupervisor]
            ,[IdSupervisor]
            ,[UsuarioRegistro]
            ,[FechaRegistro])
            VALUES
            (@IdVisitaSupervisor
            ,@IdSupervisor
            ,@UsuarioRegistro
            ,@FechaRegistro)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdVisitaSupervisor", ManejoNulos.ManageNullInteger(registro.IdVisitaSupervisor));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registro.CodSala));
                    query.Parameters.AddWithValue("@IdMotivo", ManejoNulos.ManageNullInteger(registro.IdMotivo));
                    query.Parameters.AddWithValue("@OtroMotivo", ManejoNulos.ManageNullStr(registro.OtroMotivo));
                    query.Parameters.AddWithValue("@FechaIngreso", ManejoNulos.ManageNullDate(registro.FechaIngreso));
                    //query.Parameters.AddWithValue("@FechaSalida", ManejoNulos.ManageNullDate(registro.FechaSalida));
                    query.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(registro.Observaciones));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(registro.UsuarioModificacion));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(registro.FechaModificacion));
                    if(registro.FechaSalida == DateTime.MinValue) {
                        query.Parameters.AddWithValue("@FechaSalida", DBNull.Value);
                    } else {
                        query.Parameters.AddWithValue("@FechaSalida", ManejoNulos.ManageNullDate(registro.FechaSalida));
                    }
                    int filasAfectadas = query.ExecuteNonQuery();
                    using(var queryEliminar = new SqlCommand(consultaEliminarEmpleados, con)) {
                        queryEliminar.Parameters.AddWithValue("@IdVisitaSupervisor", registro.IdVisitaSupervisor);
                        queryEliminar.ExecuteNonQuery();
                    }
                    foreach(var empleado in registro.Empleados) {
                        using(var queryInsertarEmpleado = new SqlCommand(consultaInsertarEmpleado, con)) {
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdVisitaSupervisor", registro.IdVisitaSupervisor);
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdSupervisor", ManejoNulos.ManageNullInteger(empleado.IdEmpleado));
                            queryInsertarEmpleado.Parameters.AddWithValue("@UsuarioRegistro", registro.UsuarioModificacion);
                            queryInsertarEmpleado.Parameters.AddWithValue("@FechaRegistro", registro.FechaModificacion);



                            int filasEmpleadoAfectadas = queryInsertarEmpleado.ExecuteNonQuery();
                        }
                    }

                    respuesta = (filasAfectadas > 0);
                }
            } catch(Exception ex) {
                respuesta = false;
                Console.WriteLine(ex.Message);
            }

            return respuesta;
        }

        public List<ESS_VisitaSupervisorMotivoEntidad> ListarMotivoPorEstado(int estado) {
            List<ESS_VisitaSupervisorMotivoEntidad> lista = new List<ESS_VisitaSupervisorMotivoEntidad>();
            string consulta = @"SELECT [IdMotivo]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [ESS_VisitaSupervisorMotivo]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_VisitaSupervisorMotivoEntidad {
                                IdMotivo = ManejoNulos.ManageNullInteger(dr["IdMotivo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                            };
                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                lista = new List<ESS_VisitaSupervisorMotivoEntidad>();
            }
            return lista;
        }
    }
}


