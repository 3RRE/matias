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
    public class ESS_RecaudacionPersonalDAL {
        string _conexion = string.Empty;
        public ESS_RecaudacionPersonalDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ESS_RecaudacionPersonalEntidad> ListadoRecaudacionPersonal(int[] codSala,DateTime fechaIni, DateTime fechaFin) {
            List<ESS_RecaudacionPersonalEntidad> lista = new List<ESS_RecaudacionPersonalEntidad>();
            string strSala = string.Empty; 
            strSala = $" codsala in ({String.Join(",", codSala)}) and ";
            string strTipo = string.Empty;
            string consulta = $@"
        SELECT [IdRecaudacionPersonal], [CodSala], [NombreSala], [RecaudacionInicio], [RecaudacionFin], 
               [EmpadronamientoInicio], [EmpadronamientoFin], [NumeroClientes], [Observaciones], 
               [UsuarioRegistro], [UsuarioModificacion], [FechaRegistro], [FechaModificacion]
        FROM [ESS_RecaudacionPersonal]
        WHERE {strSala} CONVERT(date, RecaudacionInicio) BETWEEN CONVERT(date, @p1) AND CONVERT(date, @p2)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_RecaudacionPersonalEntidad {
                                IdRecaudacionPersonal = ManejoNulos.ManageNullInteger(dr["IdRecaudacionPersonal"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                RecaudacionInicio = ManejoNulos.ManageNullDate(dr["RecaudacionInicio"]),
                                RecaudacionFin = ManejoNulos.ManageNullDate(dr["RecaudacionFin"]),
                                EmpadronamientoInicio = ManejoNulos.ManageNullDate(dr["EmpadronamientoInicio"]),
                                EmpadronamientoFin = ManejoNulos.ManageNullDate(dr["EmpadronamientoFin"]),
                                NumeroClientes = ManejoNulos.ManageNullInteger(dr["NumeroClientes"]),
                                Observaciones = ManejoNulos.ManageNullStr(dr["Observaciones"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"])
                            };
                            using(var conEmpleados = new SqlConnection(_conexion)) {
                                conEmpleados.Open();
                                item.Empleados = ObtenerEmpleadosPorIdRecaudacionPersonal(item.IdRecaudacionPersonal, conEmpleados);
                            }
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_RecaudacionPersonalEntidad>();
            }

            return lista;
        }
        private List<ESS_RecaudacionPersonalEmpleadoEntidad> ObtenerEmpleadosPorIdRecaudacionPersonal(int idIngresoSalidaGU, SqlConnection con) {
            List<ESS_RecaudacionPersonalEmpleadoEntidad> empleados = new List<ESS_RecaudacionPersonalEmpleadoEntidad>();
 

            
            string consultaEmpleados = @"
         SELECT 
            e.IdBuk as IdEmpleado,
            e.Nombres as Nombre,
            e.ApellidoPaterno,
            e.ApellidoMaterno,
			e.NombreCompleto,
            bme.IdTipoDocumentoRegistro,
            bme.IdEstadoParticipante,
            bme.EstadoParticipante,
            bme.IdFuncion,
            bme.NombreFuncion,
            bme.DescripcionFuncion,
            bme.IdEmpresa,
            bme.Empresa,
            bme.Cargo,
            e.TipoDocumento,
            e.NumeroDocumento as DocumentoRegistro,
            e.IdCargo,
            e.Cargo as NombreCargo
            FROM ESS_RecaudacionPersonalEmpleado bme   
            JOIN BUK_Empleado e ON bme.IdEmpleado = e.IdBuk
              LEFT JOIN BUK_Cargo ca ON e.IdCargo = ca.CodEmpresa
            WHERE bme.IdRecaudacionPersonal = @IdRecaudacionPersonal";
             

            using(var cmd = new SqlCommand(consultaEmpleados, con)) {
                cmd.Parameters.AddWithValue("@IdRecaudacionPersonal", idIngresoSalidaGU);

                using(var dr = cmd.ExecuteReader()) {
                    while(dr.Read()) {
                        var empleado = new ESS_RecaudacionPersonalEmpleadoEntidad {
                            IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                            Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                            ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                            ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                            IdEstadoParticipante = ManejoNulos.ManageNullInteger(dr["IdEstadoParticipante"]),
                            EstadoParticipante = ManejoNulos.ManageNullStr(dr["EstadoParticipante"]),
                            NombreFuncion = ManejoNulos.ManageNullStr(dr["NombreFuncion"]),
                            IdFuncion = ManejoNulos.ManageNullInteger(dr["IdFuncion"]),
                            Empresa = ManejoNulos.ManageNullStr(dr["Empresa"]),
                            IdEmpresa = ManejoNulos.ManageNullInteger(dr["IdEmpresa"]),
                            DescripcionFuncion = ManejoNulos.ManageNullStr(dr["DescripcionFuncion"]),
                            IdTipoDocumentoRegistro = ManejoNulos.ManageNullInteger(dr["IdTipoDocumentoRegistro"]),
                            DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]),
                            IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                            TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                            NombreCargo = ManejoNulos.ManageNullStr(dr["NombreCargo"]),
                            Cargo = ManejoNulos.ManageNullStr(dr["Cargo"]),
                        };
                        empleados.Add(empleado);
                    }
                }
            }

            return empleados;
        }
  
        public int GuardarRecaudacionPersonal(ESS_RecaudacionPersonalEntidad registro) {
            int idInsertado = 0;
            string queryRecaudacionPersonal = @"
        INSERT INTO [ESS_RecaudacionPersonal]
        ([CodSala], [NombreSala], [RecaudacionInicio],RecaudacionFin, [EmpadronamientoInicio], EmpadronamientoFin,
         [NumeroClientes], [Observaciones], [UsuarioRegistro], [FechaRegistro])
        OUTPUT inserted.IdRecaudacionPersonal
        VALUES (@CodSala, @NombreSala, @RecaudacionInicio, @RecaudacionFin, @EmpadronamientoInicio, @EmpadronamientoFin,
               @NumeroClientes, @Observaciones, @UsuarioRegistro, @FechaRegistro)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();

                    using(var transaction = con.BeginTransaction()) {

                        var query = new SqlCommand(queryRecaudacionPersonal, con, transaction);
                        query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullStr(registro.CodSala));
                        query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                        query.Parameters.AddWithValue("@RecaudacionInicio", ManejoNulos.ManageNullDate(registro.RecaudacionInicio)); 
                        //query.Parameters.AddWithValue("@RecaudacionFin", ManejoNulos.ManageNullDate(registro.RecaudacionFin)); 
                        //query.Parameters.AddWithValue("@EmpadronamientoFin", ManejoNulos.ManageNullDate(registro.EmpadronamientoFin));
                        query.Parameters.AddWithValue("@NumeroClientes", ManejoNulos.ManageNullInteger(registro.NumeroClientes));
                        query.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(registro.Observaciones));
                        query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                        query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));
                        if(registro.EmpadronamientoInicio == DateTime.MinValue) {
                            query.Parameters.AddWithValue("@EmpadronamientoInicio", DBNull.Value);
                        } else {
                            query.Parameters.AddWithValue("@EmpadronamientoInicio", ManejoNulos.ManageNullDate(registro.EmpadronamientoInicio)); 
                        }
                        if(registro.RecaudacionFin == DateTime.MinValue) {
                            query.Parameters.AddWithValue("@RecaudacionFin", DBNull.Value);
                        } else {
                            query.Parameters.AddWithValue("@RecaudacionFin", ManejoNulos.ManageNullDate(registro.RecaudacionFin));
                        }
                        if(registro.EmpadronamientoFin == DateTime.MinValue) {
                            query.Parameters.AddWithValue("@EmpadronamientoFin", DBNull.Value);
                        } else {
                            query.Parameters.AddWithValue("@EmpadronamientoFin", ManejoNulos.ManageNullDate(registro.EmpadronamientoFin));
                        }
                        idInsertado = Convert.ToInt32(query.ExecuteScalar());
                        foreach(ESS_RecaudacionPersonalEmpleadoEntidad empleado in registro.Empleados) {
                            string queryDetalle = @"
                        INSERT INTO [ESS_RecaudacionPersonalEmpleado]
                        ([IdRecaudacionPersonal], [IdEmpleado], [IdEstadoParticipante], [EstadoParticipante], 
                         [IdFuncion], [NombreFuncion], [DescripcionFuncion], [Nombre], [ApellidoPaterno], 
                         [ApellidoMaterno], [IdTipoDocumentoRegistro], [DocumentoRegistro], [IdCargo],[Cargo],[NombreDocumentoRegistro],[IdEmpresa],[Empresa])
                        VALUES
                        (@IdRecaudacionPersonal, @IdEmpleado, @IdEstadoParticipante, @EstadoParticipante, 
                         @IdFuncion, @NombreFuncion, @DescripcionFuncion, @Nombre, @ApellidoPaterno, 
                         @ApellidoMaterno, @IdTipoDocumentoRegistro, @DocumentoRegistro, @IdCargo,@Cargo,@NombreDocumentoRegistro,@IdEmpresa,@Empresa)";

                            var queryDet = new SqlCommand(queryDetalle, con, transaction);
                            queryDet.Parameters.AddWithValue("@IdRecaudacionPersonal", ManejoNulos.ManageNullInteger(idInsertado));
                            queryDet.Parameters.AddWithValue("@IdEmpleado", ManejoNulos.ManageNullInteger(empleado.IdEmpleado));
                            queryDet.Parameters.AddWithValue("@IdEstadoParticipante", ManejoNulos.ManageNullInteger(empleado.IdEstadoParticipante));
                            queryDet.Parameters.AddWithValue("@EstadoParticipante", ManejoNulos.ManageNullStr(empleado.EstadoParticipante));
                            queryDet.Parameters.AddWithValue("@IdFuncion", ManejoNulos.ManageNullInteger(empleado.IdFuncion));
                            queryDet.Parameters.AddWithValue("@NombreFuncion", ManejoNulos.ManageNullStr(empleado.NombreFuncion));
                            queryDet.Parameters.AddWithValue("@DescripcionFuncion", ManejoNulos.ManageNullStr(empleado.DescripcionFuncion));
                            queryDet.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(empleado.Nombre));
                            queryDet.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(empleado.ApellidoPaterno));
                            queryDet.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(empleado.ApellidoMaterno));
                            queryDet.Parameters.AddWithValue("@IdTipoDocumentoRegistro", ManejoNulos.ManageNullInteger(empleado.IdTipoDocumentoRegistro));
                            queryDet.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(empleado.DocumentoRegistro));
                            queryDet.Parameters.AddWithValue("@Cargo", ManejoNulos.ManageNullStr(empleado.Cargo)); 
                            queryDet.Parameters.AddWithValue("@IdCargo", ManejoNulos.ManageNullInteger(empleado.IdCargo));
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

        public bool ActualizarRecaudacionPersonal(ESS_RecaudacionPersonalEntidad registro) {
            bool actualizado = false;
            string queryRecaudacionPersonal = @"
        UPDATE [ESS_RecaudacionPersonal]
        SET [CodSala] = @CodSala, [NombreSala] = @NombreSala, [RecaudacionInicio] = @RecaudacionInicio, RecaudacionFin = @RecaudacionFin,
            [EmpadronamientoInicio] = @EmpadronamientoInicio, EmpadronamientoFin = @EmpadronamientoFin,
            [NumeroClientes] = @NumeroClientes, 
            [Observaciones] = @Observaciones, [UsuarioModificacion] = @UsuarioModificacion, 
            [FechaModificacion] = @FechaModificacion
        WHERE [IdRecaudacionPersonal] = @IdRecaudacionPersonal";
            string consultaEliminarEmpleados = @"
        DELETE FROM ESS_RecaudacionPersonalEmpleado
        WHERE IdRecaudacionPersonal = @IdRecaudacionPersonal";
             
            string consultaInsertarEmpleado = @"
        INSERT INTO ESS_RecaudacionPersonalEmpleado
        (IdRecaudacionPersonal, IdEmpleado, IdEstadoParticipante, EstadoParticipante, 
        IdFuncion, NombreFuncion, DescripcionFuncion, Nombre, ApellidoPaterno, 
        ApellidoMaterno, IdTipoDocumentoRegistro, DocumentoRegistro, IdCargo,[Cargo],[NombreDocumentoRegistro],[IdEmpresa],[Empresa])
        VALUES
        (@IdRecaudacionPersonal, @IdEmpleado, @IdEstadoParticipante, @EstadoParticipante,
        @IdFuncion, @NombreFuncion, @DescripcionFuncion, @Nombre, @ApellidoPaterno, 
        @ApellidoMaterno, @IdTipoDocumentoRegistro, @DocumentoRegistro, @IdCargo,@Cargo,@NombreDocumentoRegistro,@IdEmpresa,@Empresa)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(queryRecaudacionPersonal, con);
                    query.Parameters.AddWithValue("@IdRecaudacionPersonal", registro.IdRecaudacionPersonal);
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullStr(registro.CodSala));
                    query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                    query.Parameters.AddWithValue("@RecaudacionInicio", ManejoNulos.ManageNullDate(registro.RecaudacionInicio));
                    //query.Parameters.AddWithValue("@RecaudacionFin", ManejoNulos.ManageNullDate(registro.RecaudacionFin));
                    //query.Parameters.AddWithValue("@EmpadronamientoInicio", ManejoNulos.ManageNullDate(registro.EmpadronamientoInicio));
                    //query.Parameters.AddWithValue("@EmpadronamientoFin", ManejoNulos.ManageNullDate(registro.EmpadronamientoFin));
                    query.Parameters.AddWithValue("@NumeroClientes", ManejoNulos.ManageNullInteger(registro.NumeroClientes));
                    query.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(registro.Observaciones));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(registro.UsuarioModificacion));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(registro.FechaModificacion));
                    if(registro.EmpadronamientoInicio == DateTime.MinValue) {
                        query.Parameters.AddWithValue("@EmpadronamientoInicio", DBNull.Value);
                    } else {
                        query.Parameters.AddWithValue("@EmpadronamientoInicio", ManejoNulos.ManageNullDate(registro.EmpadronamientoInicio));
                    }
                    if(registro.RecaudacionFin == DateTime.MinValue) {
                        query.Parameters.AddWithValue("@RecaudacionFin", DBNull.Value);
                    } else {
                        query.Parameters.AddWithValue("@RecaudacionFin", ManejoNulos.ManageNullDate(registro.RecaudacionFin));
                    }
                    if(registro.EmpadronamientoFin == DateTime.MinValue) {
                        query.Parameters.AddWithValue("@EmpadronamientoFin", DBNull.Value);
                    } else {
                        query.Parameters.AddWithValue("@EmpadronamientoFin", ManejoNulos.ManageNullDate(registro.EmpadronamientoFin));
                    }
                    int filasAfectadas = query.ExecuteNonQuery(); 
                    using(var queryEliminar = new SqlCommand(consultaEliminarEmpleados, con)) {
                        queryEliminar.Parameters.AddWithValue("@IdRecaudacionPersonal", registro.IdRecaudacionPersonal);
                        queryEliminar.ExecuteNonQuery();
                    }
                    foreach(ESS_RecaudacionPersonalEmpleadoEntidad empleado in registro.Empleados) {
                        using(var queryInsertarEmpleado = new SqlCommand(consultaInsertarEmpleado, con)) {
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdRecaudacionPersonal", registro.IdRecaudacionPersonal);
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdEmpleado", ManejoNulos.ManageNullInteger(empleado.IdEmpleado));
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdEstadoParticipante", ManejoNulos.ManageNullInteger(empleado.IdEstadoParticipante));
                            queryInsertarEmpleado.Parameters.AddWithValue("@EstadoParticipante", ManejoNulos.ManageNullStr(empleado.EstadoParticipante));
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdFuncion", ManejoNulos.ManageNullInteger(empleado.IdFuncion));
                            queryInsertarEmpleado.Parameters.AddWithValue("@NombreFuncion", ManejoNulos.ManageNullStr(empleado.NombreFuncion));
                            queryInsertarEmpleado.Parameters.AddWithValue("@DescripcionFuncion", ManejoNulos.ManageNullStr(empleado.DescripcionFuncion));
                            queryInsertarEmpleado.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(empleado.Nombre));
                            queryInsertarEmpleado.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(empleado.ApellidoPaterno));
                            queryInsertarEmpleado.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(empleado.ApellidoMaterno));
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdTipoDocumentoRegistro", ManejoNulos.ManageNullInteger(empleado.IdTipoDocumentoRegistro));
                            queryInsertarEmpleado.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(empleado.DocumentoRegistro)); 
                            queryInsertarEmpleado.Parameters.AddWithValue("@Cargo", ManejoNulos.ManageNullStr(empleado.Cargo)); 
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdCargo", ManejoNulos.ManageNullInteger(empleado.IdCargo));
                            queryInsertarEmpleado.Parameters.AddWithValue("@NombreDocumentoRegistro", ManejoNulos.ManageNullStr(empleado.NombreDocumentoRegistro));
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdEmpresa", ManejoNulos.ManageNullInteger(empleado.IdEmpresa));
                            queryInsertarEmpleado.Parameters.AddWithValue("@Empresa", ManejoNulos.ManageNullStr(empleado.Empresa));

                            int filasEmpleadoAfectadas = queryInsertarEmpleado.ExecuteNonQuery();
                        }
                    }

                    actualizado = filasAfectadas > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                actualizado = false;
            }

            return actualizado;
        }
   public bool EliminarRecaudacionPersonal(int idRecaudacionPersonal) {
            bool respuesta = false;
            string consulta = "DELETE FROM [ESS_RecaudacionPersonal] WHERE [IdRecaudacionPersonal] = @IdRecaudacionPersonal";
            string consultaDetalle = "DELETE FROM [ESS_RecaudacionPersonalEmpleado] WHERE [IdRecaudacionPersonal] = @IdRecaudacionPersonal";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var transaction = con.BeginTransaction();  

                    try {
                        
                        var queryEliminarDetalle = new SqlCommand(consultaDetalle, con, transaction);
                        queryEliminarDetalle.Parameters.AddWithValue("@IdRecaudacionPersonal", idRecaudacionPersonal);
                        queryEliminarDetalle.ExecuteNonQuery(); 
                        var queryEliminarCabecera = new SqlCommand(consulta, con, transaction);
                        queryEliminarCabecera.Parameters.AddWithValue("@IdRecaudacionPersonal", idRecaudacionPersonal);
                        queryEliminarCabecera.ExecuteNonQuery(); 
                        transaction.Commit();
                        respuesta = true;
                    } catch(Exception ex) {
                        
                        transaction.Rollback();
                        Console.WriteLine("Error: " + ex.Message);
                        respuesta = false;
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
                respuesta = false;
            }

            return respuesta;
        }

        public List<ESS_FuncionEntidad> ListarFuncionPorEstado(int estado) {
            List<ESS_FuncionEntidad> lista = new List<ESS_FuncionEntidad>();
            string consulta = @"SELECT [IdFuncion]
                       ,[Nombre]
                       ,[Estado]
                       ,[FechaRegistro]
                       ,[FechaModificacion]
                       ,[UsuarioModificacion]
                       ,[UsuarioRegistro]
                   FROM [ESS_Funcion] where Estado = @Estado";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_FuncionEntidad {
                                IdFuncion = ManejoNulos.ManageNullInteger(dr["IdFuncion"]),
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
                lista = new List<ESS_FuncionEntidad>();
            }
            return lista;
        }
        public List<ESS_CargoRPEntidad> ListarCargoRPPorEstado(int estado) {
            List<ESS_CargoRPEntidad> lista = new List<ESS_CargoRPEntidad>();
            string consulta = @"SELECT [IdCargo]
                       ,[Nombre]
                       ,[Estado]
                       ,[FechaRegistro]
                       ,[FechaModificacion]
                       ,[UsuarioModificacion]
                       ,[UsuarioRegistro]
                   FROM [ESS_CargoRecaudacionPersonal] where Estado = @Estado";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_CargoRPEntidad {
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
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
                lista = new List<ESS_CargoRPEntidad>();
            }
            return lista;
        }

        public List<ESS_FuncionEntidad> ListarFuncion() {
            List<ESS_FuncionEntidad> lista = new List<ESS_FuncionEntidad>();
            string consulta = @"SELECT [IdFuncion]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioRegistro]
                              ,[UsuarioModificacion]
                          FROM [ESS_Funcion]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_FuncionEntidad {
                                IdFuncion = ManejoNulos.ManageNullInteger(dr["IdFuncion"]),
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
                lista = new List<ESS_FuncionEntidad>();
            }
            return lista;
        }

        public int InsertarFuncion(ESS_FuncionEntidad model) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [ESS_Funcion]
           ([Nombre]
           ,[Estado]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
OUTPUT Inserted.IdFuncion
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

        public bool EditarFuncion(ESS_FuncionEntidad model) {
            bool respuesta = false;
            string consulta = @"UPDATE [ESS_Funcion]
                       SET [Nombre] = @Nombre
                          ,[Estado] = @Estado
                          ,[FechaModificacion] = @FechaModificacion
                          ,[UsuarioModificacion] = @UsuarioModificacion
                     WHERE IdFuncion = @IdFuncion";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(model.FechaModificacion));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(model.UsuarioModificacion));
                    query.Parameters.AddWithValue("@IdFuncion", ManejoNulos.ManageNullInteger(model.IdFuncion));
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
