using CapaEntidad.ControlAcceso;
using S3k.Utilitario;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.ControlAcceso {
    public class CAL_BusquedaDAL {
        private readonly string conexion;

        public CAL_BusquedaDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public CAL_LudopataEntidad GetLudopata(string buscar) {
            CAL_LudopataEntidad item = new CAL_LudopataEntidad();
            string consulta = @"SELECT lud.[LudopataID]
                                      ,lud.[Nombre]
                                      ,lud.[ApellidoPaterno]
                                      ,lud.[ApellidoMaterno]
                                      ,lud.[FechaInscripcion]
                                      ,lud.[TipoExclusion]
                                      ,lud.[DNI]
                                      ,lud.[Foto]
                                      ,lud.[ContactoID]
                                      ,lud.[Telefono]
                                      ,lud.[CodRegistro]
                                      ,lud.[Estado]
                                      ,lud.[Imagen]
                                      ,lud.[TipoDoiID]
                                      ,lud.[CodUbigeo]
                                      ,doi.[Nombre] AS [DOINombre]
                                FROM [dbo].[CAL_Ludopata] lud
                                left JOIN [dbo].[AST_TipoDocumento] doi ON lud.TipoDoiID = doi.Id 
                                WHERE TRIM(lud.[DNI]) = TRIM(@buscar) and lud.Estado=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@buscar", buscar);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.LudopataID = ManejoNulos.ManageNullInteger(dr["LudopataID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]);
                                item.FechaInscripcion = ManejoNulos.ManageNullDate(dr["FechaInscripcion"]);
                                item.TipoExclusion = ManejoNulos.ManageNullInteger(dr["TipoExclusion"]);
                                item.DNI = ManejoNulos.ManageNullStr(dr["DNI"]);
                                item.Foto = ManejoNulos.ManageNullStr(dr["Foto"]);
                                item.ContactoID = ManejoNulos.ManageNullInteger(dr["ContactoID"]);
                                item.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]);
                                item.CodRegistro = ManejoNulos.ManageNullStr(dr["CodRegistro"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]);
                                item.TipoDoiID = ManejoNulos.ManageNullInteger(dr["TipoDoiID"]);
                                item.CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]);
                                item.DOINombre = ManejoNulos.ManageNullStr(dr["DOINombre"]);
                            }
                        }
                    };
                    SetContacto(item, con);

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return new CAL_LudopataEntidad();
            }
            return item;
        }

        public CAL_PersonaProhibidoIngresoEntidad GetTimador(string buscar) {
            CAL_PersonaProhibidoIngresoEntidad item = new CAL_PersonaProhibidoIngresoEntidad();
            string consulta = @"SELECT tim.[TimadorID]
,tim.[Nombre]
,tim.[ApellidoPaterno]
,tim.[ApellidoMaterno]
,tim.[FechaInscripcion]
,tim.[DNI]
,tim.[Foto]
,tim.[Telefono]
,tim.[Estado]
,tim.[Imagen]
,tim.[FechaRegistro]
,tim.[TipoTimadorID]
,tim.[CantidadIncidencias]
,tim.[BandaID]
,tim.[EmpleadoID]
,tim.[CodSala]
,tim.[Observacion]
,tim.[Prohibir]
,tim.[SustentoLegal]
,tim.[ConAtenuante]
,tim.[DescripcionAtenuante]
,sala.Nombre as SalaNombre
FROM [dbo].[CAL_Timador] as tim
left join Sala as sala 
on tim.CodSala=sala.CodSala
                                WHERE TRIM(tim.[DNI]) = TRIM(@buscar) and tim.estado=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@buscar", buscar);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.TimadorID = ManejoNulos.ManageNullInteger(dr["TimadorID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]);
                                item.FechaInscripcion = ManejoNulos.ManageNullDate(dr["FechaInscripcion"]);
                                item.DNI = ManejoNulos.ManageNullStr(dr["DNI"]);
                                item.Foto = ManejoNulos.ManageNullStr(dr["Foto"]);
                                item.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.TipoTimadorID = ManejoNulos.ManageNullInteger(dr["TipoTimadorID"]);
                                item.CantidadIncidencias = ManejoNulos.ManageNullInteger(dr["CantidadIncidencias"]);
                                item.BandaID = ManejoNulos.ManageNullInteger(dr["BandaID"]);
                                item.EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]);
                                item.SustentoLegal = ManejoNulos.ManageNullInteger(dr["SustentoLegal"]);
                                item.Prohibir = ManejoNulos.ManageNullInteger(dr["Prohibir"]);
                                item.SalaNombre = ManejoNulos.ManageNullStr(dr["SalaNombre"]);
                                item.ConAtenuante = ManejoNulos.ManegeNullBool(dr["ConAtenuante"]);
                                item.DescripcionAtenuante = ManejoNulos.ManageNullStr(dr["DescripcionAtenuante"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return new CAL_PersonaProhibidoIngresoEntidad();
            }
            return item;
        }

        public CAL_PoliticoEntidad GetPolitico(string buscar) {
            CAL_PoliticoEntidad item = new CAL_PoliticoEntidad();
            string consulta = @"SELECT pol.[PoliticoID]
                                      ,pol.[Nombres]
                                      ,pol.[Apellidos]
                                      ,pol.[Estado]
                                      ,pol.[CargoPoliticoID]
                                      ,pol.[Dni]
                                      ,pol.[EntidadEstatal]
                                      ,pol.[Meses]
                                      ,pol.[FechaRegistro]
                                      ,car.[Nombre] as cargoPoliticoNombre
                                      ,car.[Descripcion] as descripcionPoliticoNombre
                                FROM [dbo].[CAL_Politico] pol
                                left JOIN [dbo].[CAL_CargoPolitico] car ON pol.CargoPoliticoID = car.CargoPoliticoID 
                                WHERE TRIM([DNI]) = TRIM(@buscar) and pol.Estado=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@buscar", buscar);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.PoliticoID = ManejoNulos.ManageNullInteger(dr["PoliticoID"]);
                                item.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]);
                                item.Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]);
                                item.Estado = ManejoNulos.ManegeNullBool(dr["Estado"]);
                                item.CargoPoliticoID = ManejoNulos.ManageNullInteger(dr["CargoPoliticoID"]);
                                item.cargoPoliticoNombre = ManejoNulos.ManageNullStr(dr["cargoPoliticoNombre"]);
                                item.descripcionPoliticoNombre = ManejoNulos.ManageNullStr(dr["descripcionPoliticoNombre"]);
                                item.Dni = ManejoNulos.ManageNullStr(dr["Dni"]);
                                item.EntidadEstatal = ManejoNulos.ManageNullStr(dr["EntidadEstatal"]);
                                item.Meses = ManejoNulos.ManageNullInteger(dr["Meses"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                            }
                        }
                    };


                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return new CAL_PoliticoEntidad();
            }
            return item;
        }

        public CAL_PersonaEntidadPublicaEntidad GetPersonaEntidadPublica(string buscar) {


            CAL_PersonaEntidadPublicaEntidad item = new CAL_PersonaEntidadPublicaEntidad();
            string consulta = @"SELECT pep.[PersonaEntidadPublicaID]
                                      ,pep.[Nombres]
                                      ,pep.[Apellidos]
                                      ,pep.[Estado]
                                      ,pep.[EntidadPublicaID]
                                      ,pep.[Dni]
                                      ,pep.[CargoEntidadID]
                                      ,pep.[Meses]
                                      ,pep.[FechaRegistro]
                                      ,car.[Nombre] as EntidadPublicaNombre
                                      ,car.[Nombre] as CargoEntidadNombre
                                FROM [dbo].[CAL_PersonaEntidadPublica] pep
                                left JOIN [dbo].[CAL_EntidadPublica] ent ON pep.EntidadPublicaID = ent.EntidadPublicaID 
                                left JOIN [dbo].[CAL_CargoEntidad] car ON pep.CargoEntidadID = car.CargoEntidadID 
                                WHERE TRIM([DNI]) = TRIM(@buscar)  and pep.estado=1";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@buscar", buscar);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.PersonaEntidadPublicaID = ManejoNulos.ManageNullInteger(dr["PersonaEntidadPublicaID"]);
                                item.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]);
                                item.Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.EntidadPublicaID = ManejoNulos.ManageNullInteger(dr["EntidadPublicaID"]);
                                item.EntidadPublicaNombre = ManejoNulos.ManageNullStr(dr["EntidadPublicaNombre"]);
                                item.Dni = ManejoNulos.ManageNullStr(dr["Dni"]);
                                item.CargoEntidadID = ManejoNulos.ManageNullInteger(dr["CargoEntidadID"]);
                                item.CargoEntidadNombre = ManejoNulos.ManageNullStr(dr["CargoEntidadNombre"]);
                                item.Meses = ManejoNulos.ManageNullInteger(dr["Meses"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                            }
                        }
                    };


                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return new CAL_PersonaEntidadPublicaEntidad();
            }
            return item;
        }

        private void SetContacto(CAL_LudopataEntidad ludopata, SqlConnection context) {
            var command = new SqlCommand(@"SELECT [ContactoID]
      ,[Nombre]
      ,[ApellidoPaterno]
      ,[ApellidoMaterno]
      ,[Telefono]
      ,[Celular]
  FROM [dbo].[CAL_Contacto] where[ContactoID] = @p0", context);
            command.Parameters.AddWithValue("@p0", ludopata.ContactoID);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    reader.Read();
                    ludopata.NombreContacto=ManejoNulos.ManageNullStr(reader["Nombre"]);
                    ludopata.ApellidoPaternoContacto=ManejoNulos.ManageNullStr(reader["ApellidoPaterno"]);
                    ludopata.ApellidoMaternoContacto=ManejoNulos.ManageNullStr(reader["ApellidoMaterno"]);
                    ludopata.TelefonoContacto=ManejoNulos.ManageNullStr(reader["Telefono"]);
                    ludopata.CelularContacto=ManejoNulos.ManageNullStr(reader["Celular"]);
                }
            };
        }

        public CAL_RobaStackersBilleteroEntidad GetRobaStackersBilletero(string buscar) {
            CAL_RobaStackersBilleteroEntidad item = new CAL_RobaStackersBilleteroEntidad();
            string consulta = @"SELECT tim.[RobaStackersBilleteroID]
,tim.[Nombre]
,tim.[ApellidoPaterno]
,tim.[ApellidoMaterno]
,tim.[FechaInscripcion]
,tim.[DNI]
,tim.[Foto]
,tim.[Telefono]
,tim.[Estado]
,tim.[Imagen]
,tim.[FechaRegistro]
,tim.[CantidadIncidencias]
,tim.[EmpleadoID]
,tim.[CodSala]
,tim.[Observacion],sala.Nombre as SalaNombre
FROM [dbo].[CAL_RobaStackersBilletero] as tim
left join Sala as sala 
on tim.CodSala=sala.CodSala
                                WHERE TRIM(tim.[DNI]) = TRIM(@buscar) and tim.estado=1 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@buscar", buscar);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.RobaStackersBilleteroID = ManejoNulos.ManageNullInteger(dr["RobaStackersBilleteroID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]);
                                item.FechaInscripcion = ManejoNulos.ManageNullDate(dr["FechaInscripcion"]);
                                item.DNI = ManejoNulos.ManageNullStr(dr["DNI"]);
                                item.Foto = ManejoNulos.ManageNullStr(dr["Foto"]);
                                item.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.Imagen = ManejoNulos.ManageNullStr(dr["Imagen"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.CantidadIncidencias = ManejoNulos.ManageNullInteger(dr["CantidadIncidencias"]);
                                item.EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]);
                                item.SalaNombre = ManejoNulos.ManageNullStr(dr["SalaNombre"]);
                            }
                        }
                    };


                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return new CAL_RobaStackersBilleteroEntidad();
            }
            return item;
        }

        public CAL_MenorDeEdadEntidad GetMenorDeEdad(string dni) {
            CAL_MenorDeEdadEntidad item = new CAL_MenorDeEdadEntidad();
            string consulta = @"
        SELECT tim.[idMenor]
         ,tim.[nombre]
         ,tim.[apellido_paterno]
         ,tim.[apellido_materno]
         ,tim.[fecha_registro]
         ,tim.[doi]
         ,tim.[estado]
         ,tim.[empleado_id] 
         ,tim.[sala]
         ,tim.[tipoDoi]
           FROM [dbo].[CAL_MenorDeEdad] tim   WHERE trim(doi) = trim(@doi) and tim.estado = 1";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@doi", dni);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.IdMenor = ManejoNulos.ManageNullInteger(dr["idMenor"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["apellido_paterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["apellido_materno"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["fecha_registro"]);
                                item.Doi = ManejoNulos.ManageNullStr(dr["doi"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["estado"]);
                                item.EmpleadoID = ManejoNulos.ManageNullInteger(dr["empleado_id"]);
                                item.Sala = ManejoNulos.ManageNullInteger(dr["sala"]);
                                item.TipoDoi = ManejoNulos.ManageNullInteger(dr["tipoDoi"]);
                            }
                        }
                    };


                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                item = new CAL_MenorDeEdadEntidad();
            }
            return item;
        }
    }
}
