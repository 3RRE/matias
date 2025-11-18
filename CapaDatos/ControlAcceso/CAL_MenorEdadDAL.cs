using CapaEntidad.ControlAcceso;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ControlAcceso
{
    public  class CAL_MenorEdadDAL
    {
        string conexion = string.Empty;
        public CAL_MenorEdadDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public CAL_MenorDeEdadEntidad GetMenorEdadPorDNI(string dni)
        {
            CAL_MenorDeEdadEntidad item = new CAL_MenorDeEdadEntidad();
            string consulta = @"SELECT  [idMenor],[doi]
                              FROM [CAL_MenorDeEdad](nolock) 
                              WHERE doi = @pDNI ";
            try
            {
                using(var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDNI", dni);

                    using(var dr = query.ExecuteReader())
                    {
                        if(dr.HasRows)
                        {
                            while(dr.Read())
                            {
                                item.IdMenor = ManejoNulos.ManageNullInteger(dr["idMenor"]);
                                item.Doi = ManejoNulos.ManageNullStr(dr["doi"]);
                            }
                        }
                    };


                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;

        }

        public int InsertarMenorEdad(CAL_MenorDeEdadEntidad Entidad)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_MenorDeEdad (
                                  [nombre]
                                  ,[apellido_paterno]
                                  ,[doi]
                                  ,[fecha_registro]
                                  ,[sala]
                                  ,[empleado_id]
                                  ,[apellido_materno]
                                  ,[estado]
                                  ,[tipoDoi])
                                OUTPUT Inserted.idMenor  
                                VALUES(@pNombre 
                               ,@pApellidoPaterno 
                               ,@pDNI 
                               ,@pFechaRegistro 
                               ,@pCodSala
                               ,@pEmpleadoID
                               ,@pApellidoMaterno 
                               ,@pEstado
                               ,@pTipoDOI)";
            try
            {
                using(var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoPaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoPaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoMaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoMaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pDNI", ManejoNulos.ManageNullStr(Entidad.Doi));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.Sala));
                    query.Parameters.AddWithValue("@pEmpleadoID", ManejoNulos.ManageNullInteger(Entidad.EmpleadoID));
                    query.Parameters.AddWithValue("@pTipoDOI", ManejoNulos.ManageNullInteger(Entidad.TipoDoi));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                  
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }


        public List<CAL_MenorDeEdadEntidad> ListarMenorEdad()
        {
            List<CAL_MenorDeEdadEntidad> lista = new List<CAL_MenorDeEdadEntidad>();
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
                                ,emp.[Nombres] +' '+ emp.[ApellidosPaterno] +' '+ emp.[ApellidosMaterno] AS [EmpleadoNombres]
                                ,sal.[Nombre] AS [SalaNombre]
                                ,tp.[Nombre] AS [TipoNombre]
                                FROM [dbo].[CAL_MenorDeEdad] tim
                                INNER JOIN [dbo].[SEG_Empleado] emp ON tim.empleado_id = emp.EmpleadoID
                                INNER JOIN [dbo].[Sala] sal ON tim.sala = sal.CodSala 
                                INNER JOIN [dbo].[AST_TipoDocumento] tp ON tim.tipoDoi = tp.Id  ";
            try
            {
                using(var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            var item = new CAL_MenorDeEdadEntidad
                            {
                                IdMenor = ManejoNulos.ManageNullInteger(dr["idMenor"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["apellido_paterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["apellido_materno"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["fecha_registro"]),
                                Doi = ManejoNulos.ManageNullStr(dr["doi"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["empleado_id"]),
                                Sala = ManejoNulos.ManageNullInteger(dr["sala"]),
                                NombreEmpleado = ManejoNulos.ManageNullStr(dr["EmpleadoNombres"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["SalaNombre"]),
                                TipoDoi = ManejoNulos.ManageNullInteger(dr["tipoDOI"]),
                                NombreTipoDoi = ManejoNulos.ManageNullStr(dr["TipoNombre"])
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }


        public CAL_MenorDeEdadEntidad GetMenorEdadPorId(int id)
        {
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
                                ,emp.[Nombres] +' '+ emp.[ApellidosPaterno] +' '+ emp.[ApellidosMaterno] AS [EmpleadoNombres]
                                ,sal.[Nombre] AS [SalaNombre]
                                FROM [dbo].[CAL_MenorDeEdad] tim
                                INNER JOIN [dbo].[SEG_Empleado] emp ON tim.empleado_id = emp.EmpleadoID
                                INNER JOIN [dbo].[Sala] sal ON tim.sala = sal.CodSala   WHERE idMenor = @id";
            try
            {
                using(var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@id", id);

                    using(var dr = query.ExecuteReader())
                    {
                        if(dr.HasRows)
                        {
                            while(dr.Read())
                            {
                                item.IdMenor = ManejoNulos.ManageNullInteger(dr["idMenor"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["apellido_paterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["apellido_materno"]);
                                item.Doi = ManejoNulos.ManageNullStr(dr["doi"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["estado"]);
                                item.Sala = ManejoNulos.ManageNullInteger(dr["sala"]);
                                item.TipoDoi = ManejoNulos.ManageNullInteger(dr["tipoDoi"]);
                                item.NombreSala = ManejoNulos.ManageNullStr(dr["SalaNombre"]);
                            }
                        }
                    };


                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;

        }

        public bool EditarMenorEdad(CAL_MenorDeEdadEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CAL_MenorDeEdad]
                               SET [nombre] = @pNombre
                                  ,[apellido_paterno] = @pApellidoPaterno
                                  ,[apellido_materno] = @pApellidoMaterno             
                                  ,[doi] = @pDNI
                                  ,[estado] = @pEstado 
                                  ,[sala] = @pCodSala
                                  ,[empleado_id] = @pEmpleadoID  
                                  ,[TipoDOI] = @pTipoDOI
                                   WHERE idMenor = @id";

            try
            {
                using(var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@id", ManejoNulos.ManageNullInteger(Entidad.IdMenor));
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoPaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoPaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidoMaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoMaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDNI", ManejoNulos.ManageNullStr(Entidad.Doi));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.Sala));
                    query.Parameters.AddWithValue("@pEmpleadoID", ManejoNulos.ManageNullInteger(Entidad.EmpleadoID));
                    query.Parameters.AddWithValue("@pTipoDOI", ManejoNulos.ManageNullInteger(Entidad.TipoDoi));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
    }
}
