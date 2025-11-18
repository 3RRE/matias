using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CapaDatos {
    public class SEG_EmpleadoDAL {
        string _conexion = string.Empty;
        public SEG_EmpleadoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool EmpleadoGuardarJson(SEG_EmpleadoEntidad varItem) {
            bool respuesta = false;
            string consulta = @"INSERT INTO [SEG_Empleado]([Nombres],[ApellidosPaterno],[ApellidosMaterno],[CargoID]
           ,[FechaNacimiento],[Direccion],[EstadoEmpleado],[DOIID],[DOI],[Telefono],[Genero]
           ,[MailJob],[FechaAlta]) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8,@p9,@p10, @p11,@p12)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(varItem.Nombres));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(varItem.ApellidosPaterno));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(varItem.ApellidosMaterno));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(varItem.CargoID));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(varItem.FechaNacimiento.Date));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(varItem.Direccion));
                    query.Parameters.AddWithValue("@p6", 1);
                    query.Parameters.AddWithValue("@p7", varItem.DOIID);
                    query.Parameters.AddWithValue("@p8", varItem.DOI);
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(varItem.Telefono));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(varItem.Genero));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(varItem.MailJob));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullDate(varItem.FechaAlta.Date));
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }
        public Int32 GuardarEmpleadoGlpi(SEG_EmpleadoEntidad varItem) {
            Int32 IdInsertado = 0;

            string consulta = @"INSERT INTO [SEG_Empleado]([Nombres],[ApellidosPaterno],[ApellidosMaterno],[CargoID]
           ,[EstadoEmpleado],[DOIID],[DOI],[Telefono],[FechaAlta],[FechaNacimiento]) Output Inserted.EmpleadoID  VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(varItem.Nombres));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(varItem.ApellidosPaterno));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(varItem.ApellidosMaterno));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(varItem.CargoID));
                    query.Parameters.AddWithValue("@p4", 1);
                    query.Parameters.AddWithValue("@p5", varItem.DOIID);
                    query.Parameters.AddWithValue("@p6", varItem.DOI);
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(varItem.Telefono));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullDate(varItem.FechaAlta.Date));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullDate(varItem.FechaNacimiento.Date));

                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return IdInsertado;
        }
        public Int32 EmpleadoReconocimientoAPKGuardarJson(SEG_EmpleadoEntidad varItem) {
            Int32 IdInsertado = 0;

            string consulta = @"INSERT INTO [SEG_Empleado]([Nombres],[ApellidosPaterno],[ApellidosMaterno],[EstadoEmpleado],[DOI]) Output Inserted.EmpleadoID 
VALUES (@p0, @p1, @p2, @p3, @p4)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(varItem.Nombres));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(varItem.ApellidosPaterno));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(varItem.ApellidosMaterno));
                    query.Parameters.AddWithValue("@p3", 1);
                    query.Parameters.AddWithValue("@p4", varItem.DOI);

                    //query.ExecuteNonQuery();
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return IdInsertado;
        }

        public List<EmpleadoEncriptacion> TecnicosEncriptacionListarJson() {
            List<EmpleadoEncriptacion> lista = new List<EmpleadoEncriptacion>();
            string consulta = @"select 
                                emp.EmpleadoID,
                                emp.Nombres,
                                emp.ApellidosPaterno,
                                emp.ApellidosMaterno,
                                encript.UsuarioNombre,
                                encript.UsuarioPassword,
                                encript.Estado,
                                encript.FechaIni,
                                encript.FechaFin,
                                encript.Id as IdUsuarioEncriptacion 
                                from SEG_Empleado emp
                                left JOIN UsuarioEncriptacion encript on encript.EmpleadoId=emp.EmpleadoID";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var Empleado = new EmpleadoEncriptacion {
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"]),
                                ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                UsuarioPassword = ManejoNulos.ManageNullStr(dr["UsuarioPassword"]),
                                FechaIni = ManejoNulos.ManageNullDate(dr["FechaIni"]),
                                FechaFin = ManejoNulos.ManageNullDate(dr["FechaFin"]),
                                IdUsuarioEncriptacion = ManejoNulos.ManageNullInteger(dr["IdUsuarioEncriptacion"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                            };
                            lista.Add(Empleado);
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return lista;
        }

        public List<SEG_EmpleadoEntidad> EmpleadoListarJson() {
            List<SEG_EmpleadoEntidad> lista = new List<SEG_EmpleadoEntidad>();
            //string consulta = @"select  [EmpleadoID]
            //              ,([ApellidosPaterno]+' '+[ApellidosMaterno]+', '+[Nombres]) NombreCompleto
            //              ,[Nombres]
            //              ,[ApellidosPaterno]
            //              ,[ApellidosMaterno]
            //              ,emp.[CargoID]
            //           ,car.Descripcion NombreCargo
            //              ,[FechaNacimiento]
            //              ,[Direccion]
            //              ,[EstadoEmpleado]
            //              ,emp.[DOIID]
            //           ,doi.DESCRIPCION DOIIDNombre
            //              ,[DOI] 
            //              ,[Telefono]
            //              ,[Genero]
            //              ,[MailJob]
            //              ,[FechaAlta] from  [dbo].[SEG_Empleado] emp
            //           join SEG_Cargo car on car.CargoID = emp.CargoID
            //           join TipoDOI doi on doi.DOIID = emp.DOIID
            //            order by EmpleadoID Desc";
            string consulta = @"
                            select  emp.[EmpleadoID]
                          ,(emp.[ApellidosPaterno]+' '+emp.[ApellidosMaterno]+', '+emp.[Nombres]) NombreCompleto
                          ,emp.[Nombres]
                          ,emp.[ApellidosPaterno]
                          ,emp.[ApellidosMaterno]
                          ,emp.[CargoID]
	                      ,car.Descripcion NombreCargo
                          ,emp.[FechaNacimiento]
                          ,emp.[Direccion]
                          ,emp.[EstadoEmpleado]
                          ,emp.[DOIID]
	                      ,doi.DESCRIPCION DOIIDNombre
                          ,emp.[DOI] 
                          ,emp.[Telefono]
                          ,emp.[Genero]
                          ,emp.[MailJob]
                          ,emp.[FechaAlta],usu.UsuarioNombre
                          ,usu.UsuarioID
                          from  [dbo].[SEG_Empleado] emp
	                      join SEG_Cargo car on car.CargoID = emp.CargoID
	                      join TipoDOI doi on doi.DOIID = emp.DOIID
						  left join SEG_Usuario as usu on usu.EmpleadoID=emp.EmpleadoID
	                       order by emp.EmpleadoID Desc
";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var empleado = new SEG_EmpleadoEntidad {
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"].Trim()),
                                ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"].Trim()),
                                ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"].Trim()),
                                CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"].Trim()),
                                CargoNombre = ManejoNulos.ManageNullStr(dr["NombreCargo"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"].Trim()),
                                EstadoEmpleado = ManejoNulos.ManageNullInteger(dr["EstadoEmpleado"]),
                                DOIID = ManejoNulos.ManageNullInteger(dr["DOIID"].ToString()),
                                DOIIDNombre = ManejoNulos.ManageNullStr(dr["DOIIDNombre"]),
                                DOI = ManejoNulos.ManageNullStr(dr["DOI"]),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"].Trim()),
                                Genero = ManejoNulos.ManageNullStr(dr["Genero"].Trim()),
                                MailJob = ManejoNulos.ManageNullStr(dr["MailJob"].Trim()),
                                FechaAlta = ManejoNulos.ManageNullDate(dr["FechaAlta"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]),
                            };

                            lista.Add(empleado);
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }

        public List<SEG_EmpleadoEntidad> EmpleadoEstadoActivoListarJson() {
            List<SEG_EmpleadoEntidad> lista = new List<SEG_EmpleadoEntidad>();
            string consulta = @"select  emp.[EmpleadoID]
                          ,([ApellidosPaterno]+' '+[ApellidosMaterno]+', '+[Nombres]) NombreCompleto
                          ,[Nombres]
                          ,[ApellidosPaterno]
                          ,[ApellidosMaterno]
                          ,emp.[CargoID]
                          ,[EstadoEmpleado] from  [dbo].[SEG_Empleado] emp
                            LEFT JOIN ComiteCambios co ON co.EmpleadoID = emp.EmpleadoID
                            WHERE co.EmpleadoID IS NULL and EstadoEmpleado = @p0";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@p0", 1);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var empleado = new SEG_EmpleadoEntidad {
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"].Trim()),
                                ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"].Trim()),
                                ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"].Trim()),

                                EstadoEmpleado = ManejoNulos.ManageNullInteger(dr["EstadoEmpleado"]),

                            };

                            lista.Add(empleado);
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }

        public SEG_EmpleadoEntidad EmpleadoIdObtenerJson(int empleadoid) {
            SEG_EmpleadoEntidad segEmpleado = new SEG_EmpleadoEntidad();
            string consulta = @"select  [EmpleadoID]
                          ,[Nombres]
                          ,[ApellidosPaterno]
                          ,[ApellidosMaterno]
                          ,emp.[CargoID]
	                      ,car.Descripcion NombreCargo
                          ,[FechaNacimiento]
                          ,[Direccion]
                          ,[EstadoEmpleado]
                          ,emp.[DOIID]
	                      ,doi.DESCRIPCION DOIIDNombre
                          ,[DOI] 
                          ,[Telefono]
                          ,[Genero]
                          ,[MailJob],Movil, mailPersonal
                          ,[FechaAlta],emp_foto,[AreaTrabajo] from  [dbo].[SEG_Empleado] emp
	                      left join SEG_Cargo car on car.CargoID = emp.CargoID
	                      left join TipoDOI doi on doi.DOIID = emp.DOIID
	                       where EmpleadoID=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", empleadoid);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                segEmpleado.EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]);
                                segEmpleado.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"].Trim());
                                segEmpleado.ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"].Trim());
                                segEmpleado.ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"].Trim());
                                segEmpleado.CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"].Trim());
                                segEmpleado.CargoNombre = ManejoNulos.ManageNullStr(dr["NombreCargo"]);
                                segEmpleado.FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]);
                                segEmpleado.Direccion = ManejoNulos.ManageNullStr(dr["Direccion"].Trim());
                                segEmpleado.EstadoEmpleado = ManejoNulos.ManageNullInteger(dr["EstadoEmpleado"]);
                                segEmpleado.DOIID = ManejoNulos.ManageNullInteger(dr["DOIID"].ToString());
                                segEmpleado.DOIIDNombre = ManejoNulos.ManageNullStr(dr["DOIIDNombre"]);
                                segEmpleado.DOI = ManejoNulos.ManageNullStr(dr["DOI"]);
                                segEmpleado.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"].Trim());
                                segEmpleado.Movil = ManejoNulos.ManageNullStr(dr["Movil"].Trim());
                                segEmpleado.Genero = ManejoNulos.ManageNullStr(dr["Genero"].Trim());
                                segEmpleado.MailJob = ManejoNulos.ManageNullStr(dr["MailJob"].Trim());
                                segEmpleado.MailPersonal = ManejoNulos.ManageNullStr(dr["mailPersonal"].Trim());
                                segEmpleado.FechaAlta = ManejoNulos.ManageNullDate(dr["FechaAlta"]);
                                segEmpleado.emp_foto = ManejoNulos.ManageNullStr(dr["emp_foto"].Trim());
                                segEmpleado.AreaTrabajo = ManejoNulos.ManageNullStr(dr["AreaTrabajo"].Trim());
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return segEmpleado;
        }

        public bool EmpleadoActualizarJson(SEG_EmpleadoEntidad empleado) {
            bool respuesta = false;
            string consulta = @"update SEG_Empleado set [Nombres] = @p1
                                  ,[ApellidosPaterno] = @p2
                                  ,[ApellidosMaterno] = @p3
                                  ,[CargoID] = @p4
                                  ,[FechaNacimiento] = @p5
                                  ,[Direccion] = @p6
                                  --,[EstadoEmpleado] = @p7
                                  ,[DOIID] = @p8
                                  ,[DOI] = @p9
                                  ,[Telefono] = @p10
                                  ,[Genero] = @p11
                                  ,[MailJob] = @p12
                                    WHERE EmpleadoID = @p0";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", empleado.EmpleadoID);
                    query.Parameters.AddWithValue("@p1", empleado.Nombres);
                    query.Parameters.AddWithValue("@p2", empleado.ApellidosPaterno);
                    query.Parameters.AddWithValue("@p3", empleado.ApellidosMaterno);
                    query.Parameters.AddWithValue("@p4", empleado.CargoID);
                    query.Parameters.AddWithValue("@p5", empleado.FechaNacimiento.Date);
                    query.Parameters.AddWithValue("@p6", empleado.Direccion);
                    //query.Parameters.AddWithValue("@p7", empleado.EstadoEmpleado);
                    query.Parameters.AddWithValue("@p8", empleado.DOIID);
                    query.Parameters.AddWithValue("@p9", empleado.DOI);
                    query.Parameters.AddWithValue("@p10", empleado.Telefono);
                    query.Parameters.AddWithValue("@p11", empleado.Genero);
                    query.Parameters.AddWithValue("@p12", empleado.MailJob);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }
        public bool EstadoEmpleadoActualizarJson(int empleadoid, int estado) {
            bool respuesta = false;
            string consulta = @"update SEG_Empleado set EstadoEmpleado = @p0  WHERE EmpleadoID = @p1";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", estado);
                    query.Parameters.AddWithValue("@p1", empleadoid);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public SEG_EmpleadoEntidad EmpleadoxNroDocumentoJson(string nro_Documento) {
            SEG_EmpleadoEntidad segEmpleado = new SEG_EmpleadoEntidad();
            string consulta = @"select  [EmpleadoID]
                          ,[Nombres]
                          ,[ApellidosPaterno]
                          ,[ApellidosMaterno]
                          ,emp.[CargoID]
	                      ,car.Descripcion NombreCargo
                          ,[FechaNacimiento]
                          ,[Direccion]
                          ,[EstadoEmpleado]
                          ,emp.[DOIID]
	                      ,doi.DESCRIPCION DOIIDNombre
                          ,[DOI] 
                          ,[Telefono]
                          ,[Genero]
                          ,[MailJob],Movil, mailPersonal
                          ,[FechaAlta],[emp_foto]
      ,[emp_foto_estado] from  [dbo].[SEG_Empleado] emp
	                     left join SEG_Cargo car on car.CargoID = emp.CargoID
	                     left join TipoDOI doi on doi.DOIID = emp.DOIID
	                       where DOI=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", nro_Documento);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                segEmpleado.EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]);
                                segEmpleado.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"].Trim());
                                segEmpleado.ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"].Trim());
                                segEmpleado.ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"].Trim());
                                segEmpleado.CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"].Trim());
                                segEmpleado.CargoNombre = ManejoNulos.ManageNullStr(dr["NombreCargo"]);
                                segEmpleado.FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]);
                                segEmpleado.Direccion = ManejoNulos.ManageNullStr(dr["Direccion"].Trim());
                                segEmpleado.EstadoEmpleado = ManejoNulos.ManageNullInteger(dr["EstadoEmpleado"]);
                                segEmpleado.DOIID = ManejoNulos.ManageNullInteger(dr["DOIID"].ToString());
                                segEmpleado.DOIIDNombre = ManejoNulos.ManageNullStr(dr["DOIIDNombre"]);
                                segEmpleado.DOI = ManejoNulos.ManageNullStr(dr["DOI"]);
                                segEmpleado.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"].Trim());
                                segEmpleado.Movil = ManejoNulos.ManageNullStr(dr["Movil"].Trim());
                                segEmpleado.Genero = ManejoNulos.ManageNullStr(dr["Genero"].Trim());
                                segEmpleado.MailJob = ManejoNulos.ManageNullStr(dr["MailJob"].Trim());
                                segEmpleado.MailPersonal = ManejoNulos.ManageNullStr(dr["mailPersonal"].Trim());
                                segEmpleado.FechaAlta = ManejoNulos.ManageNullDate(dr["FechaAlta"]);
                                segEmpleado.emp_foto = ManejoNulos.ManageNullStr(dr["emp_foto"].Trim());
                                segEmpleado.emp_foto_estado = ManejoNulos.ManageNullStr(dr["emp_foto_estado"].Trim());
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return segEmpleado;
        }


        public bool EmpleadoFotoEditarJson(SEG_EmpleadoEntidad empleado) {
            bool respuesta = false;
            string consulta = @"UPDATE SEG_Empleado
	                            SET emp_foto = @p0, emp_foto_estado=1
	                            WHERE EmpleadoID=@p1;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", empleado.emp_foto);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(empleado.EmpleadoID));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }


        //Guarda datos necesarios para la creacion de la ficha de salud del empleado 
        public bool EmpleadoActualizarFichaSintomatologicaJson(SEG_EmpleadoEntidad empleado) {
            bool respuesta = false;
            string consulta = @"UPDATE [SEG_Empleado]
                                SET [AreaTrabajo]=@p0,
                                    [Nombres] = @p1
                                  ,[ApellidosPaterno] = @p2
                                  ,[ApellidosMaterno] = @p3
                                    ,[Direccion] = @p4
                                    ,[Telefono] = @p5
 ,[Empresa] = @p6
 ,[Ruc] = @p7
	                            WHERE [EmpleadoID]= @p8;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", empleado.AreaTrabajo);

                    query.Parameters.AddWithValue("@p1", empleado.Nombres);
                    query.Parameters.AddWithValue("@p2", empleado.ApellidosPaterno);
                    query.Parameters.AddWithValue("@p3", empleado.ApellidosMaterno);
                    query.Parameters.AddWithValue("@p4", empleado.Direccion);
                    query.Parameters.AddWithValue("@p5", empleado.Telefono);
                    query.Parameters.AddWithValue("@p6", empleado.Empresa);
                    query.Parameters.AddWithValue("@p7", empleado.Ruc);
                    query.Parameters.AddWithValue("@p8", empleado.EmpleadoID);
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }


        public Int32 EmpleadoGuardarFichaSintomatologicaJson(SEG_EmpleadoEntidad empleado) {
            Int32 IdInsertado = 0;
            string consulta = @"INSERT INTO [SEG_Empleado]([DOI],[Nombres],[ApellidosPaterno],[ApellidosMaterno]
           ,[Telefono],[Direccion],[AreaTrabajo],Empresa,Ruc) Output Inserted.EmpleadoID 
            VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6,@p7,@p8) ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", empleado.DOI);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(empleado.Nombres));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(empleado.ApellidosPaterno));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(empleado.ApellidosMaterno));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(empleado.Telefono));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(empleado.Direccion));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(empleado.AreaTrabajo));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(empleado.Empresa));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(empleado.Ruc));
                    //query.ExecuteNonQuery();
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return IdInsertado;
        }


        public SEG_EmpleadoEntidad EmpleadoxNroDocumentoFichaSintomatologicaJson(string nro_Documento) {
            SEG_EmpleadoEntidad segEmpleado = new SEG_EmpleadoEntidad();
            string consulta = @"select  [EmpleadoID]
                          ,[DOI]
                          ,[Nombres]
                          ,[ApellidosPaterno]
                          ,[ApellidosMaterno]
                          ,[Telefono]
                          ,[Direccion]
                          ,[AreaTrabajo],Empresa,Ruc
                           from  [dbo].[SEG_Empleado]
	                       where DOI=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", nro_Documento);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                segEmpleado.EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]);
                                segEmpleado.DOI = ManejoNulos.ManageNullStr(dr["DOI"]);
                                segEmpleado.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"].Trim());
                                segEmpleado.ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"].Trim());
                                segEmpleado.ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"].Trim());
                                segEmpleado.Telefono = ManejoNulos.ManageNullStr(dr["Telefono"].Trim());
                                segEmpleado.Direccion = ManejoNulos.ManageNullStr(dr["Direccion"]);
                                segEmpleado.AreaTrabajo = ManejoNulos.ManageNullStr(dr["AreaTrabajo"]);
                                segEmpleado.Empresa = ManejoNulos.ManageNullStr(dr["Empresa"]);
                                segEmpleado.Ruc = ManejoNulos.ManageNullStr(dr["Ruc"]);
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return segEmpleado;
        }

        public List<SEG_EmpleadoEntidad> EmpleadoListarPorNoUsadosJson() {
            List<SEG_EmpleadoEntidad> lista = new List<SEG_EmpleadoEntidad>();

            string consulta = @"
                              select  emp.[EmpleadoID]
                          ,(emp.[ApellidosPaterno]+' '+emp.[ApellidosMaterno]+', '+emp.[Nombres]) NombreCompleto
                          ,emp.[Nombres]
                          ,emp.[ApellidosPaterno]
                          ,emp.[ApellidosMaterno]
                          ,emp.[CargoID]
	                      ,car.Descripcion NombreCargo
                          ,emp.[FechaNacimiento]
                          ,emp.[Direccion]
                          ,emp.[EstadoEmpleado]
                          ,emp.[DOIID]
	                      ,doi.DESCRIPCION DOIIDNombre
                          ,emp.[DOI] 
                          ,emp.[Telefono]
                          ,emp.[Genero]
                          ,emp.[MailJob]
                          ,emp.[FechaAlta]
                          from  [dbo].[SEG_Empleado] emp
	                      join SEG_Cargo car on car.CargoID = emp.CargoID
	                      join TipoDOI doi on doi.DOIID = emp.DOIID
						  where emp.EmpleadoID not in(select EmpleadoID from SEG_Usuario)
						  and emp.EstadoEmpleado=1
	                       order by emp.EmpleadoID Desc
";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var empleado = new SEG_EmpleadoEntidad {
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"].Trim()),
                                ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"].Trim()),
                                ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"].Trim()),
                                CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"].Trim()),
                                CargoNombre = ManejoNulos.ManageNullStr(dr["NombreCargo"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"].Trim()),
                                EstadoEmpleado = ManejoNulos.ManageNullInteger(dr["EstadoEmpleado"]),
                                DOIID = ManejoNulos.ManageNullInteger(dr["DOIID"].ToString()),
                                DOIIDNombre = ManejoNulos.ManageNullStr(dr["DOIIDNombre"]),
                                DOI = ManejoNulos.ManageNullStr(dr["DOI"]),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"].Trim()),
                                Genero = ManejoNulos.ManageNullStr(dr["Genero"].Trim()),
                                MailJob = ManejoNulos.ManageNullStr(dr["MailJob"].Trim()),
                                FechaAlta = ManejoNulos.ManageNullDate(dr["FechaAlta"]),
                            };

                            lista.Add(empleado);
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }

        public List<SEG_EmpleadoEntidad> EmpleadoListarPorUsuariosJson() {
            List<SEG_EmpleadoEntidad> lista = new List<SEG_EmpleadoEntidad>();

            string consulta = @"
                                  SELECT emp.EmpleadoID
      ,TRIM(emp.Nombres) +' '+  TRIM(emp.ApellidosPaterno) +' '+TRIM(emp.ApellidosMaterno) NombreCompleto
      ,emp.Nombres
      ,emp.ApellidosPaterno
      ,emp.ApellidosMaterno
      ,emp.Telefono
      ,emp.MailJob
      ,usu.UsuarioID
	  FROM SEG_Empleado emp
	  INNER JOIN SEG_Usuario usu ON usu.EmpleadoID = emp.EmpleadoID


";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var empleado = new SEG_EmpleadoEntidad {
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"].Trim()),
                                ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"].Trim()),
                                ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"].Trim()),
                                Telefono = ManejoNulos.ManageNullStr(dr["Telefono"].Trim()),
                                MailJob = ManejoNulos.ManageNullStr(dr["MailJob"].Trim()),
                                UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]),
                            };

                            lista.Add(empleado);
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }

    }
}
