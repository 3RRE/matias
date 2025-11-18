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
using Microsoft.Win32;
namespace CapaDatos.EntradaSalidaSala {
    public class ESS_EnteReguladorDAL {
        string _conexion = string.Empty;
        public ESS_EnteReguladorDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ESS_EnteReguladorEntidad> ListadoEnteRegulador(int[] codsala, DateTime fechaIni, DateTime fechaFin) {
            List<ESS_EnteReguladorEntidad> lista = new List<ESS_EnteReguladorEntidad>();
            string strSala = string.Empty;
            //if(!codsala.Contains(-1) && codsala.Length > 0) {
                strSala = $" codsala in ({String.Join(",", codsala)}) and ";
            //}

            // Modificación de la consulta para incluir los datos de ESS_Empleado
            string consulta = $@"
        SELECT 
            er.[IdEnteRegulador],
            er.[CodSala],
            er.[NombreSala],
            er.[Descripcion],
            er.[IdMotivo],
            er.[NombreMotivo],
            er.[DescripcionMotivo],
            er.[IdEmpresa],
            er.[NombreEmpresa],
            er.[DocReferencia],
            er.[RutaImagen],
            er.[FechaIngreso],
            er.[FechaSalida],
            er.[Observaciones],
            er.[UsuarioRegistro],
            er.[UsuarioModificacion],
            er.[FechaModificacion],
            er.[FechaRegistro],
            er.[NombreAutoriza],
            er.IdAutoriza
         
        FROM [ESS_EnteRegulador] er
        
        WHERE {strSala} convert(date, er.FechaIngreso) between convert(date, @p1) and convert(date, @p2)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_EnteReguladorEntidad {
                                IdEnteRegulador = ManejoNulos.ManageNullInteger(dr["IdEnteRegulador"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                IdMotivo = ManejoNulos.ManageNullInteger(dr["IdMotivo"]),
                                NombreMotivo = ManejoNulos.ManageNullStr(dr["NombreMotivo"]),
                                DescripcionMotivo = ManejoNulos.ManageNullStr(dr["DescripcionMotivo"]),
                                IdEmpresa = ManejoNulos.ManageNullInteger(dr["IdEmpresa"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                DocReferencia = ManejoNulos.ManageNullStr(dr["DocReferencia"]),
                                RutaImagen = ManejoNulos.ManageNullStr(dr["RutaImagen"]),
                                FechaIngreso = ManejoNulos.ManageNullDate(dr["FechaIngreso"]),
                                FechaSalida = ManejoNulos.ManageNullDate(dr["FechaSalida"]),
                                //FechaSalida = ManejoNulos.ManageNullDate(dr["FechaSalida"]) == new DateTime(1753, 1, 1) ? null : ManejoNulos.ManageNullDate(dr["FechaSalida"]),
                                
                                Observaciones = ManejoNulos.ManageNullStr(dr["Observaciones"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                NombreAutoriza = ManejoNulos.ManageNullStr(dr["NombreAutoriza"]),
                                IdAutoriza = ManejoNulos.ManageNullInteger(dr["IdAutoriza"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                              
                            };
                            using(var conEmpleados = new SqlConnection(_conexion)) {
                                conEmpleados.Open();
                                item.PersonasEntidadPublica = ObtenerEmpleadosPorIdEnteRegulador(item.IdEnteRegulador, conEmpleados);
                            }
                            lista.Add(item);
                        }
                    }
                }

            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_EnteReguladorEntidad>();
            }
            return lista;
        }




private List<ESS_EntidadRegularPersonaEntidadPublica> ObtenerEmpleadosPorIdEnteRegulador(int idIngresoSalidaGU, SqlConnection con) {
    List<ESS_EntidadRegularPersonaEntidadPublica> empleados = new List<ESS_EntidadRegularPersonaEntidadPublica>();

    // Consulta SQL con JOIN entre las tablas ESS_EnteRegulador y ESS_EntidadRegularPersonaEntidadPublica
    string consultaEmpleados = @"
    SELECT 
        erp.IdEntidadRegularPersonaEntidadPublica,
        erp.PersonaEntidadPublicaID,
        erp.IdEnteRegulador,
        erp.Nombres,
        erp.Apellidos,
        erp.Estado,
        erp.IdEntidadPublica,
        erp.EntidadPublicaNombre,
        erp.Dni,
        erp.IdCargoEntidad,
        erp.CargoEntidadNombre,
        erp.FechaRegistro,
        erp.TipoDOI
    FROM ESS_EnteRegulador er
    LEFT JOIN ESS_EntidadRegularPersonaEntidadPublica erp ON er.IdEnteRegulador = erp.IdEnteRegulador
    WHERE erp.IdEnteRegulador = @IdEnteRegulador";

    using (var cmd = new SqlCommand(consultaEmpleados, con)) {
        cmd.Parameters.AddWithValue("@IdEnteRegulador", idIngresoSalidaGU);

        using (var dr = cmd.ExecuteReader()) {
            while (dr.Read()) {
                var empleado = new ESS_EntidadRegularPersonaEntidadPublica {
                    IdEntidadRegularPersonaEntidadPublica = ManejoNulos.ManageNullInteger(dr["IdEntidadRegularPersonaEntidadPublica"]),
                    PersonaEntidadPublicaID = ManejoNulos.ManageNullInteger(dr["PersonaEntidadPublicaID"]),
                    IdEnteRegulador = ManejoNulos.ManageNullInteger(dr["IdEnteRegulador"]),
                    Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                    Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]),
                    Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                    IdEntidadPublica = ManejoNulos.ManageNullInteger(dr["IdEntidadPublica"]),
                    EntidadPublicaNombre = ManejoNulos.ManageNullStr(dr["EntidadPublicaNombre"]),
                    Dni = ManejoNulos.ManageNullStr(dr["Dni"]),
                    IdCargoEntidad = ManejoNulos.ManageNullInteger(dr["IdCargoEntidad"]),
                    CargoEntidadNombre = ManejoNulos.ManageNullStr(dr["CargoEntidadNombre"]),
                    //FechaRegistro = ManejoNulos.ManageNullDateTime(dr["FechaRegistro"]),
                    TipoDOI = ManejoNulos.ManageNullStr(dr["TipoDOI"])
                };
                empleados.Add(empleado);
            }
        }
    }
    return empleados;
}


            public int GuardarRegistroEnteRegulador(ESS_EnteReguladorEntidad registro) {
            int idInsertado = 0;
            string queryEnteRegulador = @"INSERT INTO [ESS_EnteRegulador]
                                       ([CodSala]
                                       ,[NombreSala]
                                       ,[Descripcion]
                                       ,[IdMotivo]
                                       ,[NombreMotivo]
                                       ,[DescripcionMotivo]
                                       ,[IdEmpresa]
                                       ,[NombreEmpresa]
                                       ,[DocReferencia]
                                       ,[RutaImagen]
                                       ,[FechaIngreso]
                                       ,[Observaciones]
                                       ,[UsuarioRegistro]
                                       ,[NombreAutoriza]
                                        ,IdAutoriza
                                       ,[FechaRegistro])
                                    output inserted.IdEnteRegulador
                                    VALUES
                                       (@CodSala
                                       ,@NombreSala
                                       ,@Descripcion
                                       ,@IdMotivo
                                       ,@NombreMotivo
                                       ,@DescripcionMotivo
                                       ,@IdEmpresa
                                       ,@NombreEmpresa
                                       ,@DocReferencia
                                       ,@RutaImagen
                                       ,@FechaIngreso
                                       ,@Observaciones
                                       ,@UsuarioRegistro
                                       ,@NombreAutoriza
                                        ,@IdAutoriza
                                       ,@FechaRegistro)";
            
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var transaction = con.BeginTransaction()) {
                        // Crear el comando SQL para insertar el Ente Regulador
                        var queryBM = new SqlCommand(queryEnteRegulador, con, transaction);
                        queryBM.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registro.CodSala));
                        queryBM.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                        queryBM.Parameters.AddWithValue("@Descripcion", ManejoNulos.ManageNullStr(registro.Descripcion));
                        queryBM.Parameters.AddWithValue("@IdMotivo", ManejoNulos.ManageNullInteger(registro.IdMotivo));
                        queryBM.Parameters.AddWithValue("@NombreMotivo", ManejoNulos.ManageNullStr(registro.NombreMotivo));
                        queryBM.Parameters.AddWithValue("@DescripcionMotivo", ManejoNulos.ManageNullStr(registro.DescripcionMotivo));
                        queryBM.Parameters.AddWithValue("@IdEmpresa", ManejoNulos.ManageNullInteger(registro.IdEmpresa));
                        queryBM.Parameters.AddWithValue("@NombreEmpresa", ManejoNulos.ManageNullStr(registro.NombreEmpresa));
                        queryBM.Parameters.AddWithValue("@DocReferencia", ManejoNulos.ManageNullStr(registro.DocReferencia));
                        queryBM.Parameters.AddWithValue("@RutaImagen", ManejoNulos.ManageNullStr(registro.RutaImagen));
                        queryBM.Parameters.AddWithValue("@FechaIngreso", ManejoNulos.ManageNullDate(registro.FechaIngreso));
                        queryBM.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(registro.Observaciones));
                        queryBM.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                        queryBM.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));
                        queryBM.Parameters.AddWithValue("@NombreAutoriza", ManejoNulos.ManageNullStr(registro.NombreAutoriza));
                        queryBM.Parameters.AddWithValue("@IdAutoriza", ManejoNulos.ManageNullInteger(registro.IdAutoriza));

                        // Ejecutar el comando para obtener el ID insertado
                        idInsertado = Convert.ToInt32(queryBM.ExecuteScalar());

                        // Insertar los registros relacionados en ESS_EntidadRegularPersonaEntidadPublica
                        foreach(var persona in registro.PersonasEntidadPublica) {
                            string queryPersona = @"INSERT INTO [ESS_EntidadRegularPersonaEntidadPublica]
                   ([PersonaEntidadPublicaID]
                   ,[IdEnteRegulador]
                   ,[Nombres]
                   ,[Apellidos]
                   ,[Estado]
                   ,[IdEntidadPublica]
                   ,[EntidadPublicaNombre]
                   ,[Dni]
                   ,[IdCargoEntidad]
                   ,[CargoEntidadNombre]
                   ,[FechaRegistro]
                   ,[TipoDOI])
                VALUES
                   (@PersonaEntidadPublicaID
                   ,@IdEnteRegulador
                   ,@Nombres
                   ,@Apellidos
                   ,@Estado
                   ,@IdEntidadPublica
                   ,@EntidadPublicaNombre
                   ,@Dni
                   ,@IdCargoEntidad
                   ,@CargoEntidadNombre
                   ,@FechaRegistro
                   ,@TipoDOI)";

                            var queryDet = new SqlCommand(queryPersona, con, transaction);
                            queryDet.Parameters.AddWithValue("@PersonaEntidadPublicaID", ManejoNulos.ManageNullInteger(persona.PersonaEntidadPublicaID));
                            queryDet.Parameters.AddWithValue("@IdEnteRegulador", ManejoNulos.ManageNullInteger(idInsertado));
                            queryDet.Parameters.AddWithValue("@Nombres", ManejoNulos.ManageNullStr(persona.Nombres));
                            queryDet.Parameters.AddWithValue("@Apellidos", ManejoNulos.ManageNullStr(persona.Apellidos));
                            queryDet.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(persona.Estado));
                            queryDet.Parameters.AddWithValue("@IdEntidadPublica", ManejoNulos.ManageNullInteger(persona.IdEntidadPublica));
                            queryDet.Parameters.AddWithValue("@EntidadPublicaNombre", ManejoNulos.ManageNullStr(persona.EntidadPublicaNombre));
                            queryDet.Parameters.AddWithValue("@Dni", ManejoNulos.ManageNullStr(persona.Dni));
                            queryDet.Parameters.AddWithValue("@IdCargoEntidad", ManejoNulos.ManageNullInteger(persona.IdCargoEntidad));
                            queryDet.Parameters.AddWithValue("@CargoEntidadNombre", ManejoNulos.ManageNullStr(persona.CargoEntidadNombre));
                            queryDet.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(persona.FechaRegistro));
                            queryDet.Parameters.AddWithValue("@TipoDOI", ManejoNulos.ManageNullStr(persona.TipoDOI));
                            queryDet.ExecuteNonQuery();
                        }

                        // Confirmar la transacción
                        transaction.Commit();
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idInsertado = 0;
            }

            return idInsertado;
        }



        public bool ActualizarRutaImagen(int idEnteRegulador, string rutaImagen) {
            bool respuesta = false;
            string consulta = @"UPDATE [ESS_EnteRegulador]
                       SET [RutaImagen] = @RutaImagen
                     WHERE IdEnteRegulador = @IdEnteRegulador";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@RutaImagen", ManejoNulos.ManageNullStr(rutaImagen));
                    query.Parameters.AddWithValue("@IdEnteRegulador", ManejoNulos.ManageNullInteger(idEnteRegulador));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                respuesta = false;
            }
            return respuesta;
        }


      


        public bool ActualizarRegistroEnteRegulador(ESS_EnteReguladorEntidad registro) {
            bool respuesta = false;

            string queryEnteRegulador = @"UPDATE [ESS_EnteRegulador]
       SET [CodSala] = @CodSala,
           [NombreSala] = @NombreSala,
           [Descripcion] = @Descripcion,
           [IdMotivo] = @IdMotivo,
           [NombreMotivo] = @NombreMotivo,
           [DescripcionMotivo] = @DescripcionMotivo,
           [IdEmpresa] = @IdEmpresa,
           [NombreEmpresa] = @NombreEmpresa,
           [DocReferencia] = @DocReferencia,
           [FechaIngreso] = @FechaIngreso,
           [Observaciones] = @Observaciones,
           [UsuarioModificacion] = @UsuarioModificacion,
           [NombreAutoriza] = @NombreAutoriza,
            IdAutoriza = @IdAutoriza,
           [FechaModificacion] = @FechaModificacion
     WHERE [IdEnteRegulador] = @IdEnteRegulador";

            // Eliminamos empleados solo si es necesario, es decir, si la lógica lo requiere
            string consultaEliminarEmpleados = @"
    DELETE FROM ESS_EntidadRegularPersonaEntidadPublica
    WHERE IdEnteRegulador = @IdEnteRegulador";

            // Consulta para insertar empleados nuevos
            string consultaInsertarEmpleado = @"
    INSERT INTO ESS_EntidadRegularPersonaEntidadPublica
    (PersonaEntidadPublicaID, IdEnteRegulador, Nombres, Apellidos, Estado, IdEntidadPublica, 
    EntidadPublicaNombre, Dni, IdCargoEntidad, CargoEntidadNombre, TipoDOI)
    VALUES
    (@PersonaEntidadPublicaID, @IdEnteRegulador, @Nombres, @Apellidos, @Estado, @IdEntidadPublica, 
    @EntidadPublicaNombre, @Dni, @IdCargoEntidad, @CargoEntidadNombre, @TipoDOI)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var transaction = con.BeginTransaction()) {
                        // Crear el comando SQL para actualizar el Ente Regulador
                        var queryBM = new SqlCommand(queryEnteRegulador, con, transaction);
                        queryBM.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullStr(registro.CodSala));
                        queryBM.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                        queryBM.Parameters.AddWithValue("@Descripcion", ManejoNulos.ManageNullStr(registro.Descripcion));
                        queryBM.Parameters.AddWithValue("@IdMotivo", ManejoNulos.ManageNullInteger(registro.IdMotivo));
                        queryBM.Parameters.AddWithValue("@NombreMotivo", ManejoNulos.ManageNullStr(registro.NombreMotivo));
                        queryBM.Parameters.AddWithValue("@DescripcionMotivo", ManejoNulos.ManageNullStr(registro.DescripcionMotivo));
                        queryBM.Parameters.AddWithValue("@IdEmpresa", ManejoNulos.ManageNullInteger(registro.IdEmpresa));
                        queryBM.Parameters.AddWithValue("@NombreEmpresa", ManejoNulos.ManageNullStr(registro.NombreEmpresa));
                        queryBM.Parameters.AddWithValue("@DocReferencia", ManejoNulos.ManageNullStr(registro.DocReferencia));
                        queryBM.Parameters.AddWithValue("@FechaIngreso", ManejoNulos.ManageNullDate(registro.FechaIngreso));
                        queryBM.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(registro.Observaciones));
                        queryBM.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(registro.UsuarioModificacion));
                        queryBM.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(DateTime.Now));
                        queryBM.Parameters.AddWithValue("@NombreAutoriza", ManejoNulos.ManageNullStr(registro.NombreAutoriza));
                        queryBM.Parameters.AddWithValue("@IdAutoriza", ManejoNulos.ManageNullInteger(registro.IdAutoriza));
                        queryBM.Parameters.AddWithValue("@IdEnteRegulador", ManejoNulos.ManageNullInteger(registro.IdEnteRegulador));

                        // Ejecutar el comando para actualizar el Ente Regulador
                        queryBM.ExecuteNonQuery();

                        // Eliminar empleados si es necesario (solo si la lógica lo requiere, no siempre)
                        using(var queryEliminar = new SqlCommand(consultaEliminarEmpleados, con, transaction)) {
                            queryEliminar.Parameters.AddWithValue("@IdEnteRegulador", registro.IdEnteRegulador);
                            queryEliminar.ExecuteNonQuery();
                        }

                        // Insertar los nuevos empleados
                        foreach(var persona in registro.PersonasEntidadPublica) {
                            using(var queryInsertarEmpleado = new SqlCommand(consultaInsertarEmpleado, con, transaction)) {
                                queryInsertarEmpleado.Parameters.AddWithValue("@PersonaEntidadPublicaID", ManejoNulos.ManageNullInteger(persona.PersonaEntidadPublicaID));
                                queryInsertarEmpleado.Parameters.AddWithValue("@IdEnteRegulador", ManejoNulos.ManageNullInteger(registro.IdEnteRegulador));
                                queryInsertarEmpleado.Parameters.AddWithValue("@Nombres", ManejoNulos.ManageNullStr(persona.Nombres));
                                queryInsertarEmpleado.Parameters.AddWithValue("@Apellidos", ManejoNulos.ManageNullStr(persona.Apellidos));
                                queryInsertarEmpleado.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(persona.Estado));
                                queryInsertarEmpleado.Parameters.AddWithValue("@IdEntidadPublica", ManejoNulos.ManageNullInteger(persona.IdEntidadPublica));
                                queryInsertarEmpleado.Parameters.AddWithValue("@EntidadPublicaNombre", ManejoNulos.ManageNullStr(persona.EntidadPublicaNombre));
                                queryInsertarEmpleado.Parameters.AddWithValue("@Dni", ManejoNulos.ManageNullStr(persona.Dni));
                                queryInsertarEmpleado.Parameters.AddWithValue("@IdCargoEntidad", ManejoNulos.ManageNullInteger(persona.IdCargoEntidad));
                                queryInsertarEmpleado.Parameters.AddWithValue("@CargoEntidadNombre", ManejoNulos.ManageNullStr(persona.CargoEntidadNombre));
                                //queryInsertarEmpleado.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(persona.FechaRegistro));
                                queryInsertarEmpleado.Parameters.AddWithValue("@TipoDOI", ManejoNulos.ManageNullStr(persona.TipoDOI));
                                queryInsertarEmpleado.ExecuteNonQuery();
                            }
                        }

                        // Confirmar la transacción
                        transaction.Commit();
                        respuesta = true;
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }

            return respuesta;
        }



        public bool EliminarRegistroEnteRegulador(int IdEnteRegulador) {
            bool respuesta = false;
            string consulta = "DELETE FROM ESS_EnteRegulador WHERE IdEnteRegulador = @IdEnteRegulador";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@IdEnteRegulador", IdEnteRegulador);
                        cmd.ExecuteNonQuery();
                    }
                }
                respuesta = true;
            } catch(Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
            }

            return respuesta;
        }





        public bool FinalizarHoraRegistroEnteRegulador(int identeRegulador, DateTime horaSalida) {
            bool respuesta = false;

            string consulta = @"
                         UPDATE [ESS_EnteRegulador]
                         SET FechaSalida = @FechaSalida
                         WHERE IdEnteRegulador = @IdEnteRegulador";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var queryRegistro = new SqlCommand(consulta, con);
                    queryRegistro.Parameters.AddWithValue("@FechaSalida", horaSalida);
                    queryRegistro.Parameters.AddWithValue("@IdEnteRegulador", identeRegulador);
                    queryRegistro.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }


        //    public List<ESS_EnteReguladorPersonaEntidadPublicaEntidad> ObtenerPersonasActivasPorTermino(int entidadPublicaID, string term) {
        //        List<ESS_EnteReguladorPersonaEntidadPublicaEntidad> lista = new List<ESS_EnteReguladorPersonaEntidadPublicaEntidad>();
        //        string consulta = $@"
        //    SELECT top 10 [PersonaEntidadPublicaID]
        //        ,[Nombres]
        //        ,[Apellidos]
        //        ,[Estado]
        //        ,[EntidadPublicaID]
        //        ,[Dni]
        //        ,[CargoEntidadID]
        //        ,[Meses]
        //        ,[FechaRegistro]
        //        ,[TipoDOI]
        //    FROM [CAL_PersonaEntidadPublica] 
        //    WHERE (Estado = 1 OR Estado IS NULL) 
        //    AND EntidadPublicaID = @EntidadPublicaID
        //    AND (Nombres LIKE '%{term}%' OR Apellidos LIKE '%{term}%' OR Dni LIKE '%{term}%')
        //";
        //        try {
        //            using(var con = new SqlConnection(_conexion)) {
        //                con.Open();
        //                var query = new SqlCommand(consulta, con);
        //                query.Parameters.AddWithValue("@EntidadPublicaID", entidadPublicaID);
        //                using(var dr = query.ExecuteReader()) {
        //                    while(dr.Read()) {
        //                        var item = new ESS_EnteReguladorPersonaEntidadPublicaEntidad {
        //                            PersonaEntidadPublicaID = ManejoNulos.ManageNullInteger(dr["PersonaEntidadPublicaID"]),
        //                            Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
        //                            Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]),
        //                            Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
        //                            EntidadPublicaID = ManejoNulos.ManageNullInteger(dr["EntidadPublicaID"]),
        //                            Dni = ManejoNulos.ManageNullStr(dr["Dni"]),
        //                            CargoEntidadID = ManejoNulos.ManageNullInteger(dr["CargoEntidadID"]),
        //                            Meses = ManejoNulos.ManageNullDecimal(dr["Meses"]),
        //                            FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
        //                            TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"])
        //                        };
        //                        lista.Add(item);
        //                    }
        //                }
        //            }
        //        } catch(Exception ex) {
        //            return new List<ESS_EnteReguladorPersonaEntidadPublicaEntidad>();
        //        }
        //        return lista;
        //    }

        public List<ESS_EnteReguladorPersonaEntidadPublicaEntidad> ObtenerPersonasActivasPorTermino(int entidadPublicaID, string term) {
            List<ESS_EnteReguladorPersonaEntidadPublicaEntidad> lista = new List<ESS_EnteReguladorPersonaEntidadPublicaEntidad>();
            string consulta = $@"
        SELECT TOP 10 
            p.[PersonaEntidadPublicaID],
            p.[Nombres],
            p.[Apellidos],
            p.[Estado],
            p.[EntidadPublicaID],
            p.[Dni],
            p.[CargoEntidadID],
            p.[Meses],
            p.[FechaRegistro],
            p.[TipoDOI],
            c.[Nombre] AS CargoEntidadNombre,  -- Añadido nombre del cargo
            e.[Nombre] AS EntidadPublicaNombre  -- Añadido nombre de la entidad pública
        FROM [CAL_PersonaEntidadPublica] p
        LEFT JOIN [CAL_CargoEntidad] c
            ON p.CargoEntidadID = c.CargoEntidadID
        LEFT JOIN [CAL_EntidadPublica] e
            ON p.EntidadPublicaID = e.EntidadPublicaID
        WHERE (p.Estado = 1 OR p.Estado IS NULL)
        AND p.EntidadPublicaID = @EntidadPublicaID
        AND (p.Nombres LIKE '%{term}%' OR p.Apellidos LIKE '%{term}%' OR p.Dni LIKE '%{term}%')
    ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@EntidadPublicaID", entidadPublicaID);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_EnteReguladorPersonaEntidadPublicaEntidad {
                                PersonaEntidadPublicaID = ManejoNulos.ManageNullInteger(dr["PersonaEntidadPublicaID"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                EntidadPublicaID = ManejoNulos.ManageNullInteger(dr["EntidadPublicaID"]),
                                Dni = ManejoNulos.ManageNullStr(dr["Dni"]),
                                CargoEntidadID = ManejoNulos.ManageNullInteger(dr["CargoEntidadID"]),
                                Meses = ManejoNulos.ManageNullDecimal(dr["Meses"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]),
                                CargoEntidadNombre = ManejoNulos.ManageNullStr(dr["CargoEntidadNombre"]), 
                                EntidadPublicaNombre = ManejoNulos.ManageNullStr(dr["EntidadPublicaNombre"]) 
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                // Manejar la excepción de acuerdo a tu lógica
                return new List<ESS_EnteReguladorPersonaEntidadPublicaEntidad>();
            }
            return lista;
        }


        public List<ESS_EnteReguladorPersonaEntidadPublicaEntidad> ObtenerPersonasActivasPorEntidadPublica(int entidadPublicaID)
        {
            List<ESS_EnteReguladorPersonaEntidadPublicaEntidad> lista = new List<ESS_EnteReguladorPersonaEntidadPublicaEntidad>();
            string consulta = $@"
        SELECT 
            p.[PersonaEntidadPublicaID],
            p.[Nombres],
            p.[Apellidos],
            p.[Estado],
            p.[EntidadPublicaID],
            p.[Dni],
            p.[CargoEntidadID],
            p.[Meses],
            p.[FechaRegistro],
            p.[TipoDOI],
            c.[Nombre] AS CargoEntidadNombre,
            e.[Nombre] AS EntidadPublicaNombre
        FROM [CAL_PersonaEntidadPublica] p
        LEFT JOIN [CAL_CargoEntidad] c
            ON p.CargoEntidadID = c.CargoEntidadID
        LEFT JOIN [CAL_EntidadPublica] e
            ON p.EntidadPublicaID = e.EntidadPublicaID
        WHERE (p.Estado = 1)
        AND p.EntidadPublicaID = @EntidadPublicaID   
        ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@EntidadPublicaID", entidadPublicaID);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new ESS_EnteReguladorPersonaEntidadPublicaEntidad
                            {
                                PersonaEntidadPublicaID = ManejoNulos.ManageNullInteger(dr["PersonaEntidadPublicaID"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                EntidadPublicaID = ManejoNulos.ManageNullInteger(dr["EntidadPublicaID"]),
                                Dni = ManejoNulos.ManageNullStr(dr["Dni"]),
                                CargoEntidadID = ManejoNulos.ManageNullInteger(dr["CargoEntidadID"]),
                                Meses = ManejoNulos.ManageNullDecimal(dr["Meses"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]),
                                CargoEntidadNombre = ManejoNulos.ManageNullStr(dr["CargoEntidadNombre"]),
                                EntidadPublicaNombre = ManejoNulos.ManageNullStr(dr["EntidadPublicaNombre"])
                            };
                            lista.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<ESS_EnteReguladorPersonaEntidadPublicaEntidad>();
            }
            return lista;
        }
        public int GuardarRegistroEnteRegulador_ImportarExcel(ESS_EnteReguladorEntidad registro)
        {
            int idInsertado = 0;
            string queryEnteRegulador = @"INSERT INTO [ESS_EnteRegulador]
                                       ([CodSala]
                                       ,[NombreSala]
                                       ,[Descripcion]
                                       ,[IdMotivo]
                                       ,[NombreMotivo]
                                       ,[DescripcionMotivo]
                                       ,[IdEmpresa]
                                       ,[NombreEmpresa]
                                       ,[DocReferencia]
                                       ,[RutaImagen]
                                       ,[FechaIngreso]
                                       ,[Observaciones]
                                       ,[UsuarioRegistro]
                                       ,[NombreAutoriza]
                                       ,IdAutoriza
                                       ,FechaSalida
                                       ,[FechaRegistro])
                                    output inserted.IdEnteRegulador
                                    VALUES
                                       (@CodSala
                                       ,@NombreSala
                                       ,@Descripcion
                                       ,@IdMotivo
                                       ,@NombreMotivo
                                       ,@DescripcionMotivo
                                       ,@IdEmpresa
                                       ,@NombreEmpresa
                                       ,@DocReferencia
                                       ,@RutaImagen
                                       ,@FechaIngreso
                                       ,@Observaciones
                                       ,@UsuarioRegistro
                                       ,@NombreAutoriza
                                       ,@IdAutoriza
                                       ,@FechaSalida       
                                       ,@FechaRegistro)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    { 
                        var queryBM = new SqlCommand(queryEnteRegulador, con, transaction);
                        queryBM.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registro.CodSala));
                        queryBM.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                        queryBM.Parameters.AddWithValue("@Descripcion", ManejoNulos.ManageNullStr(registro.Descripcion));
                        queryBM.Parameters.AddWithValue("@IdMotivo", ManejoNulos.ManageNullInteger(registro.IdMotivo));
                        queryBM.Parameters.AddWithValue("@NombreMotivo", ManejoNulos.ManageNullStr(registro.NombreMotivo));
                        queryBM.Parameters.AddWithValue("@DescripcionMotivo", ManejoNulos.ManageNullStr(registro.DescripcionMotivo));
                        queryBM.Parameters.AddWithValue("@IdEmpresa", ManejoNulos.ManageNullInteger(registro.IdEmpresa));
                        queryBM.Parameters.AddWithValue("@NombreEmpresa", ManejoNulos.ManageNullStr(registro.NombreEmpresa));
                        queryBM.Parameters.AddWithValue("@DocReferencia", ManejoNulos.ManageNullStr(registro.DocReferencia));
                        queryBM.Parameters.AddWithValue("@RutaImagen", ManejoNulos.ManageNullStr(registro.RutaImagen));
                        queryBM.Parameters.AddWithValue("@FechaIngreso", ManejoNulos.ManageNullDate(registro.FechaIngreso));
                        queryBM.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(registro.Observaciones));
                        queryBM.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                        queryBM.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));
                        queryBM.Parameters.AddWithValue("@NombreAutoriza", ManejoNulos.ManageNullStr(registro.NombreAutoriza));
                        queryBM.Parameters.AddWithValue("@IdAutoriza", ManejoNulos.ManageNullInteger(registro.IdAutoriza));
                        queryBM.Parameters.AddWithValue("@FechaSalida", ManejoNulos.ManageNullDate(registro.FechaSalida));
                         
                        idInsertado = Convert.ToInt32(queryBM.ExecuteScalar()); 
                        foreach (var persona in registro.PersonasEntidadPublica)
                        {
                            string queryPersona = @"INSERT INTO [ESS_EntidadRegularPersonaEntidadPublica]
                                                   ([PersonaEntidadPublicaID]
                                                   ,[IdEnteRegulador]
                                                   ,[Nombres]
                                                   ,[Apellidos]
                                                   ,[Estado]
                                                   ,[IdEntidadPublica]
                                                   ,[EntidadPublicaNombre]
                                                   ,[Dni]
                                                   ,[IdCargoEntidad]
                                                   ,[CargoEntidadNombre]
                                                   ,[FechaRegistro]
                                                   ,[TipoDOI])
                                                VALUES
                                                   (@PersonaEntidadPublicaID
                                                   ,@IdEnteRegulador
                                                   ,@Nombres
                                                   ,@Apellidos
                                                   ,@Estado
                                                   ,@IdEntidadPublica
                                                   ,@EntidadPublicaNombre
                                                   ,@Dni
                                                   ,@IdCargoEntidad
                                                   ,@CargoEntidadNombre
                                                   ,@FechaRegistro
                                                   ,@TipoDOI)";

                            var queryDet = new SqlCommand(queryPersona, con, transaction);
                            queryDet.Parameters.AddWithValue("@PersonaEntidadPublicaID", ManejoNulos.ManageNullInteger(persona.PersonaEntidadPublicaID));
                            queryDet.Parameters.AddWithValue("@IdEnteRegulador", ManejoNulos.ManageNullInteger(idInsertado));
                            queryDet.Parameters.AddWithValue("@Nombres", ManejoNulos.ManageNullStr(persona.Nombres));
                            queryDet.Parameters.AddWithValue("@Apellidos", ManejoNulos.ManageNullStr(persona.Apellidos));
                            queryDet.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(persona.Estado));
                            queryDet.Parameters.AddWithValue("@IdEntidadPublica", ManejoNulos.ManageNullInteger(persona.IdEntidadPublica));
                            queryDet.Parameters.AddWithValue("@EntidadPublicaNombre", ManejoNulos.ManageNullStr(persona.EntidadPublicaNombre));
                            queryDet.Parameters.AddWithValue("@Dni", ManejoNulos.ManageNullStr(persona.Dni));
                            queryDet.Parameters.AddWithValue("@IdCargoEntidad", ManejoNulos.ManageNullInteger(persona.IdCargoEntidad));
                            queryDet.Parameters.AddWithValue("@CargoEntidadNombre", ManejoNulos.ManageNullStr(persona.CargoEntidadNombre));
                            queryDet.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(persona.FechaRegistro));
                            queryDet.Parameters.AddWithValue("@TipoDOI", ManejoNulos.ManageNullStr(persona.TipoDOI));
                            queryDet.ExecuteNonQuery();
                        } 
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                idInsertado = 0;
            }

            return idInsertado;
        }

    }
}