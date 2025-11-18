using CapaEntidad.EntradaSalidaSala;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   
using System.Data;  
using System.Security.Cryptography;
using S3k.Utilitario.clases_especial;
using CapaEntidad.ControlAcceso;
using System.Collections;
using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaDatos.ExcelenciaOperativa;
namespace CapaDatos.EntradaSalidaSala {
    public class ESS_BienMaterialDAL {
        string _conexion = string.Empty;
        public ESS_BienMaterialDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
  
        public List<ESS_BienMaterialEntidad> ListadoBienMaterial(int[] codsala, int idtipobienmaterial, DateTime fechaIni, DateTime fechaFin) {
            List<ESS_BienMaterialEntidad> lista = new List<ESS_BienMaterialEntidad>();
            string strSala = string.Empty;
            strSala = $" codsala in ({ String.Join(",",codsala)}) and ";
            string strTipo = string.Empty;
            strTipo = idtipobienmaterial == -1 ? string.Empty : $" TipoBienMaterial = {idtipobienmaterial} and ";
            string consulta = $@"SELECT [IdBienMaterial]
              ,[CodSala]
              ,[NombreSala]
              ,[TipoBienMaterial]
              ,[IdCategoria]
              ,[NombreCategoria]
              ,[DescripcionCategoria]
              ,[Descripcion]
              ,[IdMotivo]
              ,[NombreMotivo]
              ,[IdEmpresa]
              ,[NombreEmpresa]
              ,[GRRFT]
              ,[RutaImagen]
              ,[FechaIngreso]
              ,[FechaSalida]
              ,[Observaciones]
              ,[UsuarioRegistro]
              ,[UsuarioModificacion]
              ,[FechaModificacion]
              ,[FechaRegistro]
          FROM ESS_BienMaterial
          WHERE {strSala} {strTipo} convert(date, FechaIngreso) between convert(date,@p1) and convert(date,@p2)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_BienMaterialEntidad {
                                IdBienMaterial = ManejoNulos.ManageNullInteger(dr["IdBienMaterial"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                TipoBienMaterial = ManejoNulos.ManageNullInteger(dr["TipoBienMaterial"]),
                                IdCategoria = ManejoNulos.ManageNullInteger(dr["IdCategoria"]),
                                NombreCategoria = ManejoNulos.ManageNullStr(dr["NombreCategoria"]),
                                DescripcionCategoria = ManejoNulos.ManageNullStr(dr["DescripcionCategoria"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                IdMotivo = ManejoNulos.ManageNullInteger(dr["IdMotivo"]),
                                NombreMotivo = ManejoNulos.ManageNullStr(dr["NombreMotivo"]),
                                IdEmpresa = ManejoNulos.ManageNullInteger(dr["IdEmpresa"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                GRRFT = ManejoNulos.ManageNullStr(dr["GRRFT"]),
                                RutaImagen = ManejoNulos.ManageNullStr(dr["RutaImagen"]),
                                FechaIngreso = ManejoNulos.ManageNullDate(dr["FechaIngreso"]),
                                FechaSalida = ManejoNulos.ManageNullDate(dr["FechaSalida"]),
                                Observaciones = ManejoNulos.ManageNullStr(dr["Observaciones"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]), 
                            };
                       
                            using(var conEmpleados = new SqlConnection(_conexion)) {
                                conEmpleados.Open();
                                if(item.TipoBienMaterial == 1) {
                                    item.Empleados = ObtenerEmpleadosPorIdBienMaterial(item.IdBienMaterial, conEmpleados);

                                } else {
                                    if(item.TipoBienMaterial == 2) {
                                        item.Empleados = ObtenerEmpleadosExternoPorIdBienMaterial(item.IdBienMaterial, conEmpleados); 
                                    }
                                }
                            }
                            lista.Add(item);
                        }
                    }
                }

            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_BienMaterialEntidad>();
            } finally {
            }
            return lista;
        }
        private List<ESS_BienMaterialEmpleadoEntidad> ObtenerEmpleadosPorIdBienMaterial(int idBienMaterial, SqlConnection con) {
            List<ESS_BienMaterialEmpleadoEntidad> empleados = new List<ESS_BienMaterialEmpleadoEntidad>();
 
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
             e.NumeroDocumento as DocumentoRegistro
         FROM ESS_BienMaterialEmpleado bme
         JOIN BUK_Empleado e ON bme.IdEmpleado = e.IdBuk 
         LEFT JOIN BUK_Cargo ca ON e.IdCargo = ca.CodEmpresa
         WHERE bme.IdBienMaterial = @IdBienMaterial";


            using(var cmd = new SqlCommand(consultaEmpleados, con)) {
                cmd.Parameters.AddWithValue("@IdBienMaterial", idBienMaterial);

                using(var dr = cmd.ExecuteReader()) {
                    while(dr.Read()) {
                        var empleado = new ESS_BienMaterialEmpleadoEntidad {
                            IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                            Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                            ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                            ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]), 
                            DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]),
                            IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                            TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                            Cargo = ManejoNulos.ManageNullStr(dr["NombreCargo"]),
                        };
                        empleados.Add(empleado);
                    }
                }
            }
            return empleados;
        }

        private List<ESS_BienMaterialEmpleadoEntidad> ObtenerEmpleadosExternoPorIdBienMaterial(int idBienMaterial, SqlConnection con) {
            List<ESS_BienMaterialEmpleadoEntidad> empleados = new List<ESS_BienMaterialEmpleadoEntidad>();

            string consultaEmpleados = @"
                   SELECT 
            e.IdEmpleado,
            e.Nombre,
            e.ApellidoPaterno,
            e.ApellidoMaterno,
            CONCAT(e.Nombre,' ',e.ApellidoPaterno,' ',e.ApellidoMaterno) as NombreCompleto,
            e.IdCargo,
            ca.Nombre as NombreCargo,
            td.Nombre as TipoDocumento,
            e.DocumentoRegistro
            FROM ESS_BienMaterialEmpleado bme
            JOIN ESS_EmpleadoExterno e ON bme.IdEmpleado = e.IdEmpleado
            LEFT JOIN ESS_CargoExterno ca ON e.IdCargo = ca.IdCargo
            LEFT JOIN AST_TipoDocumento td ON e.IdTipoDocumentoRegistro = td.Id
             WHERE bme.IdBienMaterial = @IdBienMaterial";


            using(var cmd = new SqlCommand(consultaEmpleados, con)) {
                cmd.Parameters.AddWithValue("@IdBienMaterial", idBienMaterial);


                using(var dr = cmd.ExecuteReader()) {
                    while(dr.Read()) {
                        var empleado = new ESS_BienMaterialEmpleadoEntidad {
                            IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                            Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                            ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                            ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                            DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]),
                            IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                            TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                            Cargo = ManejoNulos.ManageNullStr(dr["NombreCargo"]),
                        };
                        empleados.Add(empleado);
                    }
                }
            }
            return empleados;
        }


        public int GuardarRegistroBienMaterial(ESS_BienMaterialEntidad registro) {
            int idInsertado = 0; 

            string queryBienMaterial = @"INSERT INTO [ESS_BienMaterial]
           ([CodSala]
,[NombreSala]
           ,[TipoBienMaterial]
           ,[IdCategoria]
           ,[NombreCategoria]
           ,[DescripcionCategoria]
           ,[Descripcion]
           ,[IdMotivo]
           ,[NombreMotivo]
           ,[IdEmpresa]
           ,[NombreEmpresa]
           ,[GRRFT]
           ,[RutaImagen]
           ,[FechaIngreso]
           ,FechaSalida
           ,[Observaciones]
           ,[UsuarioRegistro]
           ,[FechaRegistro])
output inserted.IdBienMaterial
     VALUES
           (@CodSala
,@NombreSala
           ,@TipoBienMaterial
           ,@IdCategoria
           ,@NombreCategoria
           ,@DescripcionCategoria
           ,@Descripcion
           ,@IdMotivo
           ,@NombreMotivo
           ,@IdEmpresa
           ,@NombreEmpresa
           ,@GRRFT
           ,@RutaImagen
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
                        queryBM.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registro.CodSala));
                        queryBM.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                        queryBM.Parameters.AddWithValue("@TipoBienMaterial", ManejoNulos.ManageNullInteger(registro.TipoBienMaterial));
                        queryBM.Parameters.AddWithValue("@IdCategoria", ManejoNulos.ManageNullInteger(registro.IdCategoria));

                        queryBM.Parameters.AddWithValue("@NombreCategoria", ManejoNulos.ManageNullStr(registro.NombreCategoria));
                        queryBM.Parameters.AddWithValue("@DescripcionCategoria", ManejoNulos.ManageNullStr(registro.DescripcionCategoria));
                        queryBM.Parameters.AddWithValue("@Descripcion", ManejoNulos.ManageNullStr(registro.Descripcion));
                        queryBM.Parameters.AddWithValue("@IdMotivo", ManejoNulos.ManageNullInteger(registro.IdMotivo));
                        queryBM.Parameters.AddWithValue("@NombreMotivo", ManejoNulos.ManageNullStr(registro.NombreMotivo));
                        queryBM.Parameters.AddWithValue("@IdEmpresa", ManejoNulos.ManageNullInteger(registro.IdEmpresa));
                        queryBM.Parameters.AddWithValue("@NombreEmpresa", ManejoNulos.ManageNullStr(registro.NombreEmpresa));
                        queryBM.Parameters.AddWithValue("@GRRFT", ManejoNulos.ManageNullStr(registro.GRRFT));
                        queryBM.Parameters.AddWithValue("@RutaImagen", ManejoNulos.ManageNullStr(registro.RutaImagen));
                        queryBM.Parameters.AddWithValue("@FechaIngreso", ManejoNulos.ManageNullDate(registro.FechaIngreso));
                        queryBM.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(registro.Observaciones));
                        queryBM.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                        queryBM.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));
                         
                        if(registro.FechaSalida == DateTime.MinValue) {
                            queryBM.Parameters.AddWithValue("@FechaSalida", DBNull.Value);
                        } else {
                            queryBM.Parameters.AddWithValue("@FechaSalida", ManejoNulos.ManageNullDate(registro.FechaSalida));
                        }

                        idInsertado = Convert.ToInt32(queryBM.ExecuteScalar());

                        foreach(ESS_BienMaterialEmpleadoEntidad empleado in registro.Empleados) {
                            string queryDetalle = @"INSERT INTO ESS_BienMaterialEmpleado
                               ([IdBienMaterial]
                               ,[IdEmpleado]
                               ,[Nombre]
                               ,[ApellidoPaterno]
                               ,[ApellidoMaterno]
                               ,[IdTipoDocumentoRegistro]
                               ,[NombreDocumentoRegistro]
                               ,[DocumentoRegistro]
                               ,[Cargo]
                               ,[IdCargo]
                               ,[Empresa]
                               ,[IdEmpresa])
                         VALUES
                               (@IdBienMaterial
                               ,@IdEmpleado
                               ,@Nombre
                               ,@ApellidoPaterno
                               ,@ApellidoMaterno
                               ,@IdTipoDocumentoRegistro
                               ,@NombreDocumentoRegistro
                               ,@DocumentoRegistro
                               ,@Cargo
                               ,@IdCargo
                               ,@Empresa
                               ,@IdEmpresa)";
                            var queryDet = new SqlCommand(queryDetalle, con, transaction);
                            queryDet.Parameters.AddWithValue("@IdBienMaterial", ManejoNulos.ManageNullInteger(idInsertado));
                            queryDet.Parameters.AddWithValue("@IdEmpleado", ManejoNulos.ManageNullInteger(empleado.IdEmpleado));
                            queryDet.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(empleado.Nombre));
                            queryDet.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(empleado.ApellidoPaterno));
                            queryDet.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(empleado.ApellidoMaterno));
                            queryDet.Parameters.AddWithValue("@IdTipoDocumentoRegistro", ManejoNulos.ManageNullInteger(empleado.IdTipoDocumentoRegistro));
                            queryDet.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(empleado.DocumentoRegistro));
                            queryDet.Parameters.AddWithValue("@Cargo", ManejoNulos.ManageNullStr(empleado.Cargo));
                            queryDet.Parameters.AddWithValue("@IdCargo", ManejoNulos.ManageNullInteger(empleado.IdCargo));
                            queryDet.Parameters.AddWithValue("@NombreDocumentoRegistro", ManejoNulos.ManageNullStr(empleado.NombreDocumentoRegistro));
                            queryDet.Parameters.AddWithValue("@Empresa", ManejoNulos.ManageNullStr(empleado.Empresa));
                            queryDet.Parameters.AddWithValue("@IdEmpresa", ManejoNulos.ManageNullInteger(empleado.IdEmpresa));
                            queryDet.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
            } catch(Exception ex) {
                idInsertado = 0;
            }

            return idInsertado;
        }
        public bool ActualizarRutaImagen(int idBienMaterial, string rutaImagen) {
            bool respuesta = false;
            string consulta = @"UPDATE ESS_BienMaterial
                       SET [RutaImagen] = @RutaImagen
                     WHERE IdBienMaterial = @IdBienMaterial";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@RutaImagen", ManejoNulos.ManageNullStr(rutaImagen));
                    query.Parameters.AddWithValue("@IdBienMaterial", ManejoNulos.ManageNullInteger(idBienMaterial));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                respuesta = false;
            }
            return respuesta;
        }
         
        public bool EditarBienesMateriales(ESS_BienMaterialEntidad bienMaterial) {
            bool respuesta = false;

            string consultaBienMaterial = @"
        UPDATE ESS_BienMaterial
        SET 
            CodSala = @CodSala,
            NombreSala = @NombreSala,
            TipoBienMaterial = @TipoBienMaterial,
            IdCategoria = @IdCategoria,
            NombreCategoria = @NombreCategoria,
            DescripcionCategoria = @DescripcionCategoria,
            Descripcion = @Descripcion,
            IdMotivo = @IdMotivo,
            NombreMotivo = @NombreMotivo,
            IdEmpresa = @IdEmpresa,
            NombreEmpresa = @NombreEmpresa,
            GRRFT = @GRRFT,
            FechaIngreso = @FechaIngreso,
            FechaSalida = @FechaSalida,
            Observaciones = @Observaciones,
            UsuarioModificacion = @UsuarioModificacion,
            FechaModificacion = @FechaModificacion
        WHERE IdBienMaterial = @IdBienMaterial";

            string consultaEliminarEmpleados = @"
        DELETE FROM ESS_BienMaterialEmpleado
        WHERE IdBienMaterial = @IdBienMaterial";

            string consultaInsertarEmpleado = @"
        INSERT INTO ESS_BienMaterialEmpleado
        (IdBienMaterial, IdEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, IdTipoDocumentoRegistro, DocumentoRegistro, IdCargo,Cargo,NombreDocumentoRegistro,IdEmpresa,Empresa)
        VALUES
        (@IdBienMaterial, @IdEmpleado, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @IdTipoDocumentoRegistro, @DocumentoRegistro, @IdCargo,@Cargo,@NombreDocumentoRegistro,@IdEmpresa,@Empresa)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();

                    // Actualizar BienMaterial
                    using(var queryBienMaterial = new SqlCommand(consultaBienMaterial, con)) {
                        queryBienMaterial.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(bienMaterial.CodSala));
                        queryBienMaterial.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(bienMaterial.NombreSala));
                        queryBienMaterial.Parameters.AddWithValue("@TipoBienMaterial", ManejoNulos.ManageNullInteger(bienMaterial.TipoBienMaterial));
                        queryBienMaterial.Parameters.AddWithValue("@IdCategoria", ManejoNulos.ManageNullInteger(bienMaterial.IdCategoria));
                        queryBienMaterial.Parameters.AddWithValue("@NombreCategoria", ManejoNulos.ManageNullStr(bienMaterial.NombreCategoria));
                        queryBienMaterial.Parameters.AddWithValue("@DescripcionCategoria", ManejoNulos.ManageNullStr(bienMaterial.DescripcionCategoria));
                        queryBienMaterial.Parameters.AddWithValue("@Descripcion", ManejoNulos.ManageNullStr(bienMaterial.Descripcion));
                        queryBienMaterial.Parameters.AddWithValue("@IdMotivo", ManejoNulos.ManageNullInteger(bienMaterial.IdMotivo));
                        queryBienMaterial.Parameters.AddWithValue("@NombreMotivo", ManejoNulos.ManageNullStr(bienMaterial.NombreMotivo));
                        queryBienMaterial.Parameters.AddWithValue("@IdEmpresa", ManejoNulos.ManageNullInteger(bienMaterial.IdEmpresa));
                        queryBienMaterial.Parameters.AddWithValue("@NombreEmpresa", ManejoNulos.ManageNullStr(bienMaterial.NombreEmpresa));
                        queryBienMaterial.Parameters.AddWithValue("@GRRFT", ManejoNulos.ManageNullStr(bienMaterial.GRRFT));
                        queryBienMaterial.Parameters.AddWithValue("@FechaIngreso", ManejoNulos.ManageNullDate(bienMaterial.FechaIngreso));
                     

                        queryBienMaterial.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(bienMaterial.Observaciones));
                        queryBienMaterial.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(bienMaterial.UsuarioModificacion));
                        queryBienMaterial.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(bienMaterial.FechaModificacion));
                        queryBienMaterial.Parameters.AddWithValue("@IdBienMaterial", ManejoNulos.ManageNullInteger(bienMaterial.IdBienMaterial));
                        
                        if(bienMaterial.FechaSalida == DateTime.MinValue) {
                            queryBienMaterial.Parameters.AddWithValue("@FechaSalida", DBNull.Value);
                        } else {
                            queryBienMaterial.Parameters.AddWithValue("@FechaSalida", ManejoNulos.ManageNullDate(bienMaterial.FechaSalida));
                        }

                        queryBienMaterial.ExecuteNonQuery();
                    }
                     
                    using(var queryEliminar = new SqlCommand(consultaEliminarEmpleados, con)) {
                        queryEliminar.Parameters.AddWithValue("@IdBienMaterial", bienMaterial.IdBienMaterial);
                        queryEliminar.ExecuteNonQuery();
                    }
                     
                    foreach(var empleado in bienMaterial.Empleados) {
                        using(var queryInsertarEmpleado = new SqlCommand(consultaInsertarEmpleado, con)) {
                            queryInsertarEmpleado.Parameters.AddWithValue("@IdBienMaterial", bienMaterial.IdBienMaterial);
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

                             
                            queryInsertarEmpleado.ExecuteNonQuery();
                        }
                    }

                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }

            return respuesta;
        }

        public bool EliminarRegistroBienMaterial(int IdBienMaterial) {
            bool respuesta = false;
            string consulta = "DELETE FROM ESS_BienMaterial WHERE IdBienMaterial = @IdBienMaterial";
            string consultadetalle = "DELETE FROM ESS_BienMaterialEmpleado WHERE IdBienMaterial = @IdBienMaterial";


            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var transaccion = con.BeginTransaction()) 
                    {
                        try {
                            
                            using(var cmdDependientes = new SqlCommand(consultadetalle, con, transaccion)) {
                                cmdDependientes.Parameters.AddWithValue("@IdBienMaterial", IdBienMaterial);
                                cmdDependientes.ExecuteNonQuery();
                            }

                            
                            using(var cmdPrincipal = new SqlCommand(consulta, con, transaccion)) {
                                cmdPrincipal.Parameters.AddWithValue("@IdBienMaterial", IdBienMaterial);
                                cmdPrincipal.ExecuteNonQuery();
                            }

                            
                            transaccion.Commit();
                            respuesta = true;
                        } catch(Exception ex) {
                            
                            transaccion.Rollback();
                            Console.WriteLine("Error al eliminar: " + ex.Message);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
            }

            return respuesta;
        }

        public bool FinalizarHoraRegistroBienMaterial(int idbienMaterial) {
            bool respuesta = false;

            string consulta = @"
                                UPDATE [ESS_BienMaterial]
                                SET FechaSalida = @FechaSalida
                                WHERE IdBienMaterial = @IdBienMaterial"; 

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var queryRegistro = new SqlCommand(consulta, con);
                    queryRegistro.Parameters.AddWithValue("@FechaSalida", DateTime.Now);
                    queryRegistro.Parameters.AddWithValue("@IdBienMaterial", idbienMaterial);
                    queryRegistro.ExecuteNonQuery();  
                    respuesta = true;

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public ESS_BienMaterialEntidad GetBienMaterialPorId(int id) {
            ESS_BienMaterialEntidad item = new ESS_BienMaterialEntidad();
            string consulta = @"SELECT [IdBienMaterial]
                              ,[CodSala]
                              ,[NombreSala]
                              ,[TipoBienMaterial]
                              ,[IdCategoria]
                              ,[NombreCategoria]
                              ,[DescripcionCategoria]
                              ,[Descripcion]
                              ,[IdMotivo]
                              ,[NombreMotivo]
                              ,[IdEmpresa]
                              ,[NombreEmpresa]
                              ,[GRRFT]
                              ,[RutaImagen]
                              ,[FechaIngreso]
                              ,[FechaSalida]
                              ,[Observaciones]
                              ,[UsuarioRegistro]
                              ,[UsuarioModificacion]
                              ,[FechaModificacion]
                              ,[FechaRegistro]
                              ,[Fecha]
                          FROM [ESS_BienMaterial] where IdBienMaterial = @IdBienMaterial";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdBienMaterial", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.IdBienMaterial = ManejoNulos.ManageNullInteger(dr["IdBienMaterial"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                                item.TipoBienMaterial = ManejoNulos.ManageNullInteger(dr["TipoBienMaterial"]);
                                item.IdCategoria = ManejoNulos.ManageNullInteger(dr["IdCategoria"]);
                                item.NombreCategoria = ManejoNulos.ManageNullStr(dr["NombreCategoria"]);
                                item.DescripcionCategoria = ManejoNulos.ManageNullStr(dr["DescripcionCategoria"]);
                                item.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                item.IdMotivo = ManejoNulos.ManageNullInteger(dr["IdMotivo"]);
                                item.NombreMotivo = ManejoNulos.ManageNullStr(dr["NombreMotivo"]);
                                item.IdEmpresa = ManejoNulos.ManageNullInteger(dr["IdEmpresa"]);
                                item.NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]);
                                item.GRRFT = ManejoNulos.ManageNullStr(dr["GRRFT"]);
                                item.RutaImagen = ManejoNulos.ManageNullStr(dr["RutaImagen"]);
                                item.FechaIngreso = ManejoNulos.ManageNullDate(dr["FechaIngreso"]);
                                item.FechaSalida = ManejoNulos.ManageNullDate(dr["FechaSalida"]);
                                item.Observaciones = ManejoNulos.ManageNullStr(dr["Observaciones"]);
                                item.UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]);
                                item.UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                            }
                        }
                    };
                    SetEmpleados(item, con);
                }
            } catch(Exception ex) {
                item = new ESS_BienMaterialEntidad();
            }
            return item;

        }
        private void SetEmpleados(ESS_BienMaterialEntidad bienMaterial, SqlConnection context) {
            var empleados = new List<ESS_BienMaterialEmpleadoEntidad>();
            var command = new SqlCommand(@"SELECT [IdBienMaterialEmpleado]
      ,[IdBienMaterial]
      ,[IdEmpleado]
      ,[Nombre]
      ,[ApellidoPaterno]
      ,[ApellidoMaterno]
      ,[IdTipoDocumentoRegistro]
      ,[DocumentoRegistro]
      ,[IdCargo]
      ,[NombreDocumentoRegistro]
      ,[Cargo]
      ,[IdEmpresa]
      ,[Empresa]
  FROM [ESS_BienMaterialEmpleado] where [IdBienMaterial] = @p0", context);
            command.Parameters.AddWithValue("@p0", bienMaterial.IdBienMaterial);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    while(reader.Read()) {
                        empleados.Add(new ESS_BienMaterialEmpleadoEntidad() {
                            IdBienMaterialEmpleado = ManejoNulos.ManageNullInteger(reader["IdBienMaterialEmpleado"]),
                            IdBienMaterial = ManejoNulos.ManageNullInteger(reader["IdBienMaterial"]),
                            IdEmpleado = ManejoNulos.ManageNullInteger(reader["IdEmpleado"]),
                            Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                            ApellidoPaterno = ManejoNulos.ManageNullStr(reader["ApellidoPaterno"]),
                            ApellidoMaterno = ManejoNulos.ManageNullStr(reader["ApellidoMaterno"]),
                            IdTipoDocumentoRegistro = ManejoNulos.ManageNullInteger(reader["IdTipoDocumentoRegistro"]),
                            DocumentoRegistro = ManejoNulos.ManageNullStr(reader["DocumentoRegistro"]),
                            IdCargo = ManejoNulos.ManageNullInteger(reader["IdCargo"]),
                            NombreDocumentoRegistro = ManejoNulos.ManageNullStr(reader["NombreDocumentoRegistro"]),
                            Cargo = ManejoNulos.ManageNullStr(reader["Cargo"]),
                            IdEmpresa = ManejoNulos.ManageNullInteger(reader["IdEmpresa"]),
                            Empresa = ManejoNulos.ManageNullStr(reader["Empresa"]),
                        });
                    }
                }
            };
            bienMaterial.Empleados = empleados;
        }
    }

}
