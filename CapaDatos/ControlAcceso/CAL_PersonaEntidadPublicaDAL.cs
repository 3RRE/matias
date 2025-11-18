using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.Utilitarios;
using CapaEntidad.ControlAcceso;
using S3k.Utilitario;

namespace CapaDatos.ControlAcceso
{
    public class CAL_PersonaEntidadPublicaDAL
    {
        string conexion = string.Empty;
        public CAL_PersonaEntidadPublicaDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_PersonaEntidadPublicaEntidad> GetAllPersonaEntidadPublica()
        {
            List<CAL_PersonaEntidadPublicaEntidad> lista = new List<CAL_PersonaEntidadPublicaEntidad>();
            string consulta = @"SELECT [PersonaEntidadPublicaID]
                                      ,pe.[Nombres]
                                      ,pe.[Apellidos]
                                      ,pe.[Estado]
                                      ,pe.[EntidadPublicaID]
									  ,ep.Nombre EntidadPublicaNombre
                                      ,pe.[Dni]
                                      ,pe.[CargoEntidadID]
									  ,ce.Nombre CargoEntidadNombre
                                      ,pe.[Meses]
                                      ,pe.[FechaRegistro]
                                      ,pe.[TipoDOI]
                                  FROM [dbo].[CAL_PersonaEntidadPublica](nolock) pe
								  join CAL_CargoEntidad (nolock) ce on ce.CargoEntidadID=pe.CargoEntidadID
								  join CAL_EntidadPublica (nolock) ep on ep.EntidadPublicaID=pe.EntidadPublicaID ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new CAL_PersonaEntidadPublicaEntidad
                            {
                                PersonaEntidadPublicaID = ManejoNulos.ManageNullInteger(dr["PersonaEntidadPublicaID"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                EntidadPublicaID = ManejoNulos.ManageNullInteger(dr["EntidadPublicaID"]),
                                EntidadPublicaNombre = ManejoNulos.ManageNullStr(dr["EntidadPublicaNombre"]),
                                Dni = ManejoNulos.ManageNullStr(dr["Dni"]),
                                CargoEntidadID = ManejoNulos.ManageNullInteger(dr["CargoEntidadID"]),
                                CargoEntidadNombre = ManejoNulos.ManageNullStr(dr["CargoEntidadNombre"]),
                                Meses = ManejoNulos.ManageNullDecimal(dr["Meses"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]),
                            };

                            lista.Add(item);
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
        public CAL_PersonaEntidadPublicaEntidad GetIDPersonaEntidadPublica(int id)
        {
            CAL_PersonaEntidadPublicaEntidad item = new CAL_PersonaEntidadPublicaEntidad();
            string consulta = @"SELECT [PersonaEntidadPublicaID]
                                      ,pe.[Nombres]
                                      ,pe.[Apellidos]
                                      ,pe.[Estado]
                                      ,pe.[EntidadPublicaID]
									  ,ep.Nombre EntidadPublicaNombre
                                      ,pe.[Dni]
                                      ,pe.[CargoEntidadID]
									  ,ce.Nombre CargoEntidadNombre
                                      ,pe.[Meses]
                                      ,pe.[FechaRegistro]
                                      ,pe.[TipoDOI]
                                  FROM [dbo].[CAL_PersonaEntidadPublica](nolock) pe
								  join CAL_CargoEntidad (nolock) ce on ce.CargoEntidadID=pe.CargoEntidadID
								  join CAL_EntidadPublica (nolock) ep on ep.EntidadPublicaID=pe.EntidadPublicaID
                                WHERE PersonaEntidadPublicaID = @pPersonaEntidadPublicaID ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pPersonaEntidadPublicaID", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.PersonaEntidadPublicaID = ManejoNulos.ManageNullInteger(dr["PersonaEntidadPublicaID"]);
                                item.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]);
                                item.Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.EntidadPublicaID = ManejoNulos.ManageNullInteger(dr["EntidadPublicaID"]);
                                item.EntidadPublicaNombre = ManejoNulos.ManageNullStr(dr["EntidadPublicaNombre"]);
                                item.Dni = ManejoNulos.ManageNullStr(dr["Dni"]);
                                item.CargoEntidadID = ManejoNulos.ManageNullInteger(dr["CargoEntidadID"]);
                                item.CargoEntidadNombre = ManejoNulos.ManageNullStr(dr["CargoEntidadNombre"]);
                                item.Meses = ManejoNulos.ManageNullDecimal(dr["Meses"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarPersonaEntidadPublica(CAL_PersonaEntidadPublicaEntidad Entidad)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_PersonaEntidadPublica ([Nombres],[Apellidos],[Estado],[EntidadPublicaID],[Dni],[CargoEntidadID],[Meses],[FechaRegistro],[TipoDOI])
                                OUTPUT Inserted.PersonaEntidadPublicaID  
                                VALUES(@pNombres,@pApellidos,@pEstado,@pEntidadPublicaID,@pDni,@pCargoEntidadID,@pMeses,@pFechaRegistro,@pTipoDOI)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombres", ManejoNulos.ManageNullStr( Entidad.Nombres).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidos", ManejoNulos.ManageNullStr(Entidad.Apellidos).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pEntidadPublicaID", ManejoNulos.ManageNullInteger(Entidad.EntidadPublicaID));
                    query.Parameters.AddWithValue("@pDni", ManejoNulos.ManageNullStr(Entidad.Dni));
                    query.Parameters.AddWithValue("@pCargoEntidadID", ManejoNulos.ManageNullInteger(Entidad.CargoEntidadID));
                    query.Parameters.AddWithValue("@pMeses", ManejoNulos.ManageNullDecimal(Entidad.Meses));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pTipoDOI", ManejoNulos.ManageNullInteger(Entidad.TipoDOI));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarPersonaEntidadPublica(CAL_PersonaEntidadPublicaEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CAL_PersonaEntidadPublica]
                               SET [Nombres] = @pNombres
                                  ,[Apellidos] = @pApellidos
                                  ,[Estado] = @pEstado
                                  ,[EntidadPublicaID] = @pEntidadPublicaID
                                  ,[Dni] = @pDni
                                  ,[CargoEntidadID] = @pCargoEntidadID
                                  ,[Meses] = @pMeses
                                  ,[TipoDOI] = @pTipoDOI
                                         where PersonaEntidadPublicaID = @pPersonaEntidadPublicaID";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pPersonaEntidadPublicaID", ManejoNulos.ManageNullInteger( Entidad.PersonaEntidadPublicaID));
                    query.Parameters.AddWithValue("@pNombres", ManejoNulos.ManageNullStr(Entidad.Nombres).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidos", ManejoNulos.ManageNullStr(Entidad.Apellidos).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pEntidadPublicaID", ManejoNulos.ManageNullInteger(Entidad.EntidadPublicaID));
                    query.Parameters.AddWithValue("@pDni", ManejoNulos.ManageNullStr(Entidad.Dni));
                    query.Parameters.AddWithValue("@pCargoEntidadID", ManejoNulos.ManageNullInteger(Entidad.CargoEntidadID));
                    query.Parameters.AddWithValue("@pMeses", ManejoNulos.ManageNullDecimal(Entidad.Meses));
                    query.Parameters.AddWithValue("@pTipoDOI", ManejoNulos.ManageNullInteger(Entidad.TipoDOI));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarPersonaEntidadPublica(int id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_PersonaEntidadPublica 
                                WHERE PersonaEntidadPublicaID  = @pPersonaEntidadPublicaID";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pPersonaEntidadPublicaID", ManejoNulos.ManageNullInteger(id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public CAL_PersonaEntidadPublicaEntidad GetPersonaEntidadPublicaPorDNI(string dni)
        {
            CAL_PersonaEntidadPublicaEntidad item = new CAL_PersonaEntidadPublicaEntidad();
            string consulta = @"SELECT [PersonaEntidadPublicaID]
                                      ,pe.[Nombres]
                                      ,pe.[Apellidos]
                                      ,pe.[Estado]
                                      ,pe.[EntidadPublicaID]
									  ,ep.Nombre EntidadPublicaNombre
                                      ,pe.[Dni]
                                      ,pe.[CargoEntidadID]
									  ,ce.Nombre CargoEntidadNombre
                                      ,pe.[Meses]
                                      ,pe.[FechaRegistro]
                                      ,pe.[TipoDOI]
                                  FROM [dbo].[CAL_PersonaEntidadPublica](nolock) pe
								  join CAL_CargoEntidad (nolock) ce on ce.CargoEntidadID=pe.CargoEntidadID
								  join CAL_EntidadPublica (nolock) ep on ep.EntidadPublicaID=pe.EntidadPublicaID
                                WHERE pe.[Dni] = @pDni ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDni", dni);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.PersonaEntidadPublicaID = ManejoNulos.ManageNullInteger(dr["PersonaEntidadPublicaID"]);
                                item.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]);
                                item.Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.EntidadPublicaID = ManejoNulos.ManageNullInteger(dr["EntidadPublicaID"]);
                                item.EntidadPublicaNombre = ManejoNulos.ManageNullStr(dr["EntidadPublicaNombre"]);
                                item.Dni = ManejoNulos.ManageNullStr(dr["Dni"]);
                                item.CargoEntidadID = ManejoNulos.ManageNullInteger(dr["CargoEntidadID"]);
                                item.CargoEntidadNombre = ManejoNulos.ManageNullStr(dr["CargoEntidadNombre"]);
                                item.Meses = ManejoNulos.ManageNullDecimal(dr["Meses"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.TipoDOI = ManejoNulos.ManageNullInteger(dr["TipoDOI"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
    }
}
