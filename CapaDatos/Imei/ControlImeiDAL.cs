using CapaEntidad.Discos;
using CapaEntidad.Imei;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Imei {
    public class ControlImeiDAL {

        string _conexion = string.Empty;
        public ControlImeiDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ControlImeiEntidad> ListadoControlImei() {
            List<ControlImeiEntidad> lista = new List<ControlImeiEntidad>();
            string consulta = @"SELECT 
                                ci.Imei,
                                ci.[Estado],
                                ci.[FechaRegistro],
								su.[UsuarioNombre],
                                se.[Nombres],
                                se.[ApellidosPaterno],
                                se.[ApellidosMaterno],
                                sc.[Descripcion],
								ci.[IdEmpleado],
                                ci.[IdControlImei]
                                FROM ControlImei ci
                                INNER JOIN SEG_Empleado se ON ci.IdEmpleado = se.EmpleadoID
								INNER JOIN SEG_Usuario su ON  ci.IdEmpleado=su.EmpleadoID
                                INNER JOIN SEG_Cargo sc ON se.CargoID = sc.CargoID WHERE ci.[Estado] =0 order by FechaRegistro desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ControlImeiEntidad {
                                IdControlImei = ManejoNulos.ManageNullInteger(dr["IdControlImei"]),
                                IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                                NombreEmpleado = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"]),
                                Cargo = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                Imei = ManejoNulos.ManageNullStr(dr["Imei"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreUsuario = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"])

                            };
                            lista.Add(item);
                        }
                    }
                }

            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }


        public int GuardarControlImei(ControlImeiEntidad controlImei) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO ControlImei (IdEmpleado, Imei)
                            Output Inserted.IdControlImei
                            VALUES(@p0, @p1)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(controlImei.IdEmpleado));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(controlImei.Imei));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        

        public bool RechazarControlImei(int idControlImei) {


            bool response = false;
            string consulta = @"UPDATE ControlImei
	                                   SET Estado=2
	                                   WHERE IdControlImei=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(idControlImei));
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }
        public bool AceptarControlImei(int idControlImei) {


            bool response = false;
            string consulta = @"UPDATE ControlImei
	                                   SET Estado=1
	                                   WHERE IdControlImei=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(idControlImei));
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }


        public int RegistrarNuevoImei(ControlImeiEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[ControlImei]
                               ([IdEmpleado]
                               ,[Imei]
                               ,[Estado]
                               ,[FechaRegistro])
                                OUTPUT Inserted.IdControlImei  
                                VALUES(@pIdEmpleado,@pImei,@pEstado,@pFechaRegistro)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdEmpleado", ManejoNulos.ManageNullInteger(Entidad.IdEmpleado));
                    query.Parameters.AddWithValue("@pImei", ManejoNulos.ManageNullStr(Entidad.Imei));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool EditarNuevoImei(ControlImeiEntidad Entidad) {
            bool respuesta = false;
            string consulta = @"  UPDATE [dbo].[ControlImei]
                                  SET Imei=@pImei,FechaRegistro=@pFechaRegistro
                                  WHERE IdControlImei=@pIdControlImei";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdControlImei", ManejoNulos.ManageNullInteger(Entidad.IdControlImei));
                    query.Parameters.AddWithValue("@pImei", ManejoNulos.ManageNullStr(Entidad.Imei));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public ControlImeiEntidad ObtenerRegistroPendienteImei(int IdEmpleado) {
            ControlImeiEntidad entidad = new ControlImeiEntidad();
            string consulta = @"SELECT [IdControlImei]
                                    ,[IdEmpleado]
                                    ,[Imei]
                                    ,[Estado]
                                    ,[FechaRegistro]
                                FROM [dbo].[ControlImei] WHERE IdEmpleado = @pIdEmpleado AND Estado=0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pIdEmpleado", ManejoNulos.ManageNullInteger(IdEmpleado));

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ControlImeiEntidad {
                                IdControlImei = ManejoNulos.ManageNullInteger(dr["IdControlImei"]),
                                IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                                Imei = ManejoNulos.ManageNullStr(dr["Imei"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                            };
                            entidad = item;
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                entidad = new ControlImeiEntidad();
            }
            return entidad;
        }

    }
}
