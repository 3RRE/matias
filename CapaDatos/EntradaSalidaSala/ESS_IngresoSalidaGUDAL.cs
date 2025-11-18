using CapaEntidad;
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
    public class ESS_IngresoSalidaGUDAL {
        string _conexion = string.Empty;
        public ESS_IngresoSalidaGUDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ESS_IngresoSalidaGUEntidad> ListadoIngresoSalida(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            List<ESS_IngresoSalidaGUEntidad> lista = new List<ESS_IngresoSalidaGUEntidad>();
            string strSala = string.Empty;
            strSala = $" codsala in ({String.Join(",", codSala)}) and ";
            string strTipo = string.Empty;

            string consulta = $@"SELECT [IdIngresoSalidaGU], [CodSala], [NombreSala], [Fecha], [Descripcion],
                                      [IdMotivo], [NombreMotivo], [DescripcionMotivo], [HoraIngreso], [HoraSalida], [Observaciones], 
                                      [UsuarioRegistro], [UsuarioModificacion], [FechaRegistro], [FechaModificacion],
                                        CASE 
                                                WHEN HoraIngreso IS NOT NULL AND HoraSalida IS NULL THEN 'En Espera'
                                                WHEN HoraIngreso IS NOT NULL AND HoraSalida IS NOT NULL THEN 'Finalizado'
                                                ELSE 'Sin Estado'
                                            END AS Estado,
                                        FechaRegistro
                                FROM [ESS_IngresoSalidaGU]
                                WHERE {strSala} CONVERT(date, HoraIngreso) BETWEEN CONVERT(date, @p1) AND CONVERT(date, @p2)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin); 

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_IngresoSalidaGUEntidad {
                                IdIngresoSalidaGU = ManejoNulos.ManageNullInteger(dr["IdIngresoSalidaGU"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]), 
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]), 
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                IdMotivo = ManejoNulos.ManageNullInteger(dr["IdMotivo"]),
                                NombreMotivo = ManejoNulos.ManageNullStr(dr["NombreMotivo"]),
                                DescripcionMotivo = ManejoNulos.ManageNullStr(dr["DescripcionMotivo"]),
                                HoraIngreso = ManejoNulos.ManageNullDate(dr["HoraIngreso"]),
                                HoraSalida = ManejoNulos.ManageNullDate(dr["HoraSalida"]),
                                Observaciones = ManejoNulos.ManageNullStr(dr["Observaciones"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                Estado = ManejoNulos.ManageNullStr(dr["Estado"]),
                            };
                            using(var conEmpleados = new SqlConnection(_conexion)) {
                                conEmpleados.Open();
                                item.Empleados = ObtenerEmpleadosPorIdIngresoSalidaGU(item.IdIngresoSalidaGU, conEmpleados);
                            }
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_IngresoSalidaGUEntidad>();
            }

            return lista;
        }
        private List<ESS_IngresoSalidaGUEmpleadoEntidad> ObtenerEmpleadosPorIdIngresoSalidaGU(int idIngresoSalidaGU, SqlConnection con) {
            List<ESS_IngresoSalidaGUEmpleadoEntidad> empleados = new List<ESS_IngresoSalidaGUEmpleadoEntidad>();

            string consultaEmpleados = @"
           SELECT 
       e.IdBuk as IdEmpleado,
       e.Nombres as Nombre,
       e.ApellidoPaterno,
       e.ApellidoMaterno,
       e.NombreCompleto,
       e.IdCargo,
       e.Cargo as NombreCargo,
       e.TipoDocumento,
       e.NumeroDocumento as DocumentoRegistro, 
       ca.Nombre as NombreCargo
   FROM ESS_IngresoSalidaGUEmpleado bme
   JOIN BUK_Empleado e ON bme.IdEmpleado = e.IdBuk 
   LEFT JOIN BUK_Cargo ca ON e.IdCargo = ca.CodEmpresa
            WHERE bme.IdIngresoSalidaGU = @IdIngresoSalidaGU";

            using(var cmd = new SqlCommand(consultaEmpleados, con)) {
                cmd.Parameters.AddWithValue("@IdIngresoSalidaGU", idIngresoSalidaGU);

                using(var dr = cmd.ExecuteReader()) {
                    while(dr.Read()) {
                        var empleado = new ESS_IngresoSalidaGUEmpleadoEntidad {
                            IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                            Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                            ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                            ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                            DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]),
                            IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                            TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                            NombreCargo = ManejoNulos.ManageNullStr(dr["NombreCargo"]),
                        };

                        empleados.Add(empleado);
                    }
                }
            }
            return empleados;
        }

        public int GuardarIngresoSalidaGU(ESS_IngresoSalidaGUEntidad registro) {
            int idInsertado = 0;
            string queryIngresoSalidaGU = @"INSERT INTO [ESS_IngresoSalidaGU]
                                                ([CodSala], [NombreSala], [Descripcion], [IdMotivo], [NombreMotivo], [DescripcionMotivo], 
                                                 [HoraIngreso],HoraSalida, [Observaciones], [UsuarioRegistro], [FechaRegistro])
                                            OUTPUT inserted.IdIngresoSalidaGU
                                            VALUES (@CodSala, @NombreSala, @Descripcion, @IdMotivo, @NombreMotivo, @DescripcionMotivo, 
                                                    @HoraIngreso, @HoraSalida, @Observaciones, @UsuarioRegistro, @FechaRegistro)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var transaction = con.BeginTransaction()) { 
                        var query = new SqlCommand(queryIngresoSalidaGU, con, transaction);
                        query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullStr(registro.CodSala));
                        query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                        //query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(registro.Fecha));
                      
                        query.Parameters.AddWithValue("@Descripcion", ManejoNulos.ManageNullStr(registro.Descripcion));
                        query.Parameters.AddWithValue("@IdMotivo", ManejoNulos.ManageNullStr(registro.IdMotivo ));
                        query.Parameters.AddWithValue("@NombreMotivo", ManejoNulos.ManageNullStr(registro.NombreMotivo));
                        query.Parameters.AddWithValue("@DescripcionMotivo", ManejoNulos.ManageNullStr(registro.DescripcionMotivo));
                        query.Parameters.AddWithValue("@HoraIngreso", ManejoNulos.ManageNullDate(registro.HoraIngreso));
                        //query.Parameters.AddWithValue("@HoraSalida", ManejoNulos.ManageNullDate(registro.HoraSalida));
                        query.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(registro.Observaciones));
                        query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                        query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));
                        if(registro.HoraSalida == DateTime.MinValue) {
                            query.Parameters.AddWithValue("@HoraSalida", DBNull.Value);
                        } else {
                            query.Parameters.AddWithValue("@HoraSalida", ManejoNulos.ManageNullDate(registro.HoraSalida));
                        }
                        idInsertado = Convert.ToInt32(query.ExecuteScalar());
                        foreach(ESS_IngresoSalidaGUEmpleadoEntidad empleado in registro.Empleados) {
                            string queryDetalle = @"INSERT INTO [ESS_IngresoSalidaGUEmpleado]
                               ([IdIngresoSalidaGU]
                               ,[IdEmpleado]
                               ,[Nombre]
                               ,[ApellidoPaterno]
                               ,[ApellidoMaterno]
                               ,[IdTipoDocumentoRegistro]
                               ,[DocumentoRegistro]
                               ,[IdCargo],Cargo,NombreDocumentoRegistro,IdEmpresa,Empresa)
                         VALUES
                               (@IdIngresoSalidaGU
                               ,@IdEmpleado
                               ,@Nombre
                               ,@ApellidoPaterno
                               ,@ApellidoMaterno
                               ,@IdTipoDocumentoRegistro
                               ,@DocumentoRegistro
                               ,@IdCargo,@Cargo,@NombreDocumentoRegistro,@IdEmpresa,@Empresa)";
                            var queryDet = new SqlCommand(queryDetalle, con, transaction);
                            queryDet.Parameters.AddWithValue("@IdIngresoSalidaGU", ManejoNulos.ManageNullInteger(idInsertado));
                            queryDet.Parameters.AddWithValue("@IdEmpleado", ManejoNulos.ManageNullInteger(empleado.IdEmpleado));
                            queryDet.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(empleado.Nombre));
                            queryDet.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(empleado.ApellidoPaterno));
                            queryDet.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(empleado.ApellidoMaterno));
                            queryDet.Parameters.AddWithValue("@IdTipoDocumentoRegistro", ManejoNulos.ManageNullInteger(empleado.IdTipoDocumentoRegistro));
                            queryDet.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(empleado.DocumentoRegistro));
                            queryDet.Parameters.AddWithValue("@IdCargo", ManejoNulos.ManageNullInteger(empleado.IdCargo));
                            queryDet.Parameters.AddWithValue("@Cargo", ManejoNulos.ManageNullStr(empleado.Cargo));
                            queryDet.Parameters.AddWithValue("@NombreDocumentoRegistro", ManejoNulos.ManageNullStr(empleado.NombreDocumentoRegistro));
                            queryDet.Parameters.AddWithValue("@IdEmpresa", ManejoNulos.ManageNullInteger(empleado.IdEmpresa));
                            queryDet.Parameters.AddWithValue("@Empresa", ManejoNulos.ManageNullStr(empleado.Empresa));
                            queryDet.ExecuteNonQuery();
 
                        }
                        transaction.Commit();
                    }
                }
            } catch(Exception ex) {
                idInsertado = 0;
                Console.WriteLine(ex.Message);
            }

            return idInsertado;
        }

       
        public bool ActualizarIngresoSalidaGU(ESS_IngresoSalidaGUEntidad registro) {
            bool respuesta = false;
            string consulta = @"UPDATE [ESS_IngresoSalidaGU]
                        SET [CodSala] = @CodSala, [NombreSala] = @NombreSala,
                            [Descripcion] = @Descripcion, [IdMotivo] = @IdMotivo, [NombreMotivo] = @NombreMotivo, 
                            [DescripcionMotivo] = @DescripcionMotivo, [HoraIngreso] = @HoraIngreso, HoraSalida = @HoraSalida,
                            [Observaciones] = @Observaciones, 
                            [UsuarioModificacion] = @UsuarioModificacion, [FechaModificacion] = @FechaModificacion
                        WHERE [IdIngresoSalidaGU] = @IdIngresoSalidaGU";

            string consultaEliminarEmpleados = @"
            DELETE FROM ESS_IngresoSalidaGUEmpleado
            WHERE IdIngresoSalidaGU = @IdIngresoSalidaGU";

            string consultaInsertarEmpleado = @"
            INSERT INTO ESS_IngresoSalidaGUEmpleado
            (IdIngresoSalidaGU, IdEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, IdTipoDocumentoRegistro, DocumentoRegistro, IdCargo,Cargo,NombreDocumentoRegistro,IdEmpresa,Empresa)
            VALUES
            (@IdIngresoSalidaGU, @IdEmpleado, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @IdTipoDocumentoRegistro, @DocumentoRegistro, @IdCargo,@Cargo,@NombreDocumentoRegistro,@IdEmpresa,@Empresa)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdIngresoSalidaGU", ManejoNulos.ManageNullInteger(registro.IdIngresoSalidaGU));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registro.CodSala));
                    query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                    //query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(registro.Fecha));
                 
                    query.Parameters.AddWithValue("@Descripcion", ManejoNulos.ManageNullStr(registro.Descripcion));
                    query.Parameters.AddWithValue("@IdMotivo", ManejoNulos.ManageNullInteger(registro.IdMotivo));
                    query.Parameters.AddWithValue("@NombreMotivo", ManejoNulos.ManageNullStr(registro.NombreMotivo)); 
                    query.Parameters.AddWithValue("@DescripcionMotivo", ManejoNulos.ManageNullStr(registro.DescripcionMotivo)); 
                    query.Parameters.AddWithValue("@HoraIngreso", ManejoNulos.ManageNullDate(registro.HoraIngreso));
                    //query.Parameters.AddWithValue("@HoraSalida", ManejoNulos.ManageNullDate(registro.HoraSalida));
                    query.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(registro.Observaciones)); 
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(registro.UsuarioModificacion)); 
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(registro.FechaModificacion));
                    if(registro.HoraSalida == DateTime.MinValue) {
                        query.Parameters.AddWithValue("@HoraSalida", DBNull.Value);
                    } else {
                        query.Parameters.AddWithValue("@HoraSalida", ManejoNulos.ManageNullDate(registro.HoraSalida));
                    }
                    int filasAfectadas = query.ExecuteNonQuery();
                    using(var queryEliminar = new SqlCommand(consultaEliminarEmpleados, con)) {
                        queryEliminar.Parameters.AddWithValue("@IdIngresoSalidaGU", registro.IdIngresoSalidaGU);
                        queryEliminar.ExecuteNonQuery();
                    }
                    foreach(var empleado in registro.Empleados) {
                        using(var queryInsertarEmpleado = new SqlCommand(consultaInsertarEmpleado, con)) {
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdIngresoSalidaGU", registro.IdIngresoSalidaGU);
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdEmpleado", ManejoNulos.ManageNullInteger(empleado.IdEmpleado));
                            queryInsertarEmpleado.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(empleado.Nombre));
                            queryInsertarEmpleado.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(empleado.ApellidoPaterno));
                            queryInsertarEmpleado.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(empleado.ApellidoMaterno));
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdTipoDocumentoRegistro", ManejoNulos.ManageNullInteger(empleado.IdTipoDocumentoRegistro));

                            queryInsertarEmpleado.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(empleado.DocumentoRegistro));
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdCargo", ManejoNulos.ManageNullInteger(empleado.IdCargo));
                            queryInsertarEmpleado.Parameters.AddWithValue("@Cargo", ManejoNulos.ManageNullStr(empleado.Cargo));
                            queryInsertarEmpleado.Parameters.AddWithValue("@NombreDocumentoRegistro", ManejoNulos.ManageNullStr(empleado.NombreDocumentoRegistro));
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdEmpresa", ManejoNulos.ManageNullInteger(empleado.IdEmpresa));
                            queryInsertarEmpleado.Parameters.AddWithValue("@Empresa", ManejoNulos.ManageNullStr(empleado.Empresa));

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
         
        public bool EliminarIngresoSalidaGU(int idIngresoSalida) {
            bool respuesta = false;
            string consulta = "DELETE FROM [ESS_IngresoSalidaGU] WHERE [IdIngresoSalidaGU] = @IdIngresoSalidaGU";
            string consultaEmpleados = "DELETE FROM [ESS_IngresoSalidaGUEmpleado] WHERE [IdIngresoSalidaGU] = @IdIngresoSalidaGU";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var transaccion = con.BeginTransaction()) 
                    {
                        try {
                            
                            using(var cmdEmpleados = new SqlCommand(consultaEmpleados, con, transaccion)) {
                                cmdEmpleados.Parameters.AddWithValue("@IdIngresoSalidaGU", idIngresoSalida);
                                cmdEmpleados.ExecuteNonQuery();
                            }

                            
                            using(var cmdPrincipal = new SqlCommand(consulta, con, transaccion)) {
                                cmdPrincipal.Parameters.AddWithValue("@IdIngresoSalidaGU", idIngresoSalida);
                                cmdPrincipal.ExecuteNonQuery();
                            }

                            
                            transaccion.Commit();
                            respuesta = true;
                        } catch(Exception ex) {
                            
                            transaccion.Rollback();
                            Console.WriteLine("Error al eliminar: " + ex.Message);
                            respuesta = false;
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
                respuesta = false;
            }

            return respuesta;
        }

        public bool FinalizarHoraRegistroIngresoSalidaGU(int idingresosalidagu) {
            bool respuesta = false;

            string consulta = @"
                                UPDATE [ESS_IngresoSalidaGU]
                                SET HoraSalida = @HoraSalida
                                WHERE IdIngresoSalidaGU = @IdIngresoSalidaGU";


            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var queryRegistro = new SqlCommand(consulta, con);
                    queryRegistro.Parameters.AddWithValue("@HoraSalida", DateTime.Now);
                    queryRegistro.Parameters.AddWithValue("@IdIngresoSalidaGU", idingresosalidagu);
                    queryRegistro.ExecuteNonQuery();
                    respuesta = true;

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public List<ESS_MotivoGUEntidad> ListarMotivoGUPorEstado(int estado) {
            List<ESS_MotivoGUEntidad> lista = new List<ESS_MotivoGUEntidad>();
            string consulta = @"SELECT [IdMotivo]
                       ,[Nombre]
                       ,[Estado]
                       ,[FechaRegistro]
                       ,[FechaModificacion]
                       ,[UsuarioModificacion]
                       ,[UsuarioRegistro]
                   FROM ESS_IngresoSalidaGUMotivo where Estado = @Estado";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_MotivoGUEntidad {
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
                lista = new List<ESS_MotivoGUEntidad>();
            }
            return lista;
        }


        public List<ESS_MotivoGUEntidad> ListarMotivo() {
            List<ESS_MotivoGUEntidad> lista = new List<ESS_MotivoGUEntidad>();
            string consulta = @"SELECT [IdMotivo]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioRegistro]
                              ,[UsuarioModificacion]
                          FROM [ESS_IngresoSalidaGUMotivo]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_MotivoGUEntidad {
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
                lista = new List<ESS_MotivoGUEntidad>();
            }
            return lista;
        }

        public int InsertarMotivo(ESS_MotivoGUEntidad model) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [ESS_IngresoSalidaGUMotivo]
           ([Nombre]
           ,[Estado]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
OUTPUT Inserted.IdMotivo
     VALUES
           (@Nombre
            ,@Estado
            ,@FechaRegistro
            ,@UsuarioRegistro)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(model.FechaRegistro));
                    query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(model.UsuarioRegistro).ToUpper().Trim());
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool EditarMotivo(ESS_MotivoGUEntidad model) {
            bool respuesta = false;
            string consulta = @"UPDATE [ESS_IngresoSalidaGUMotivo]
                       SET [Nombre] = @Nombre
                          ,[Estado] = @Estado
                          ,[FechaModificacion] = @FechaModificacion
                          ,[UsuarioModificacion] = @UsuarioModificacion
                     WHERE IdMotivo = @IdMotivo";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(model.FechaModificacion));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(model.UsuarioModificacion));
                    query.Parameters.AddWithValue("@IdMotivo", ManejoNulos.ManageNullInteger(model.IdMotivo));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                respuesta = false;
            }
            return respuesta;
        }

    }

}
