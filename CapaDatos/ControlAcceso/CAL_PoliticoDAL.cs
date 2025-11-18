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
    public class CAL_PoliticoDAL
    {
        string conexion = string.Empty;
        public CAL_PoliticoDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_PoliticoEntidad> GetAllPolitico()
        {
            List<CAL_PoliticoEntidad> lista = new List<CAL_PoliticoEntidad>();
            string consulta = @"SELECT pol.PoliticoID, 
                                pol.Nombres, 
                                pol.Apellidos, 
                                pol.Dni, 
                                pol.EntidadEstatal, 
                                pol.Meses, 
                                pol.Estado, 
                                pol.CargoPoliticoID, 
                                pol.FechaRegistro, 
                                pol.TipoDOI, 
                                car.Nombre AS cargoPoliticoNombre,
                                car.Descripcion AS descripcionPoliticoNombre
                                FROM dbo.CAL_Politico pol
                                INNER JOIN dbo.CAL_CargoPolitico car ON pol.CargoPoliticoID = car.CargoPoliticoID";
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
                            var item = new CAL_PoliticoEntidad
                            {
                                PoliticoID = ManejoNulos.ManageNullInteger(dr["PoliticoID"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]),
                                Dni = ManejoNulos.ManageNullStr(dr["Dni"]),
                                EntidadEstatal = ManejoNulos.ManageNullStr(dr["EntidadEstatal"]),
                                Meses = ManejoNulos.ManageNullDecimal(dr["Meses"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                CargoPoliticoID = ManejoNulos.ManageNullInteger(dr["CargoPoliticoID"]),
                                cargoPoliticoNombre = ManejoNulos.ManageNullStr(dr["cargoPoliticoNombre"]),
                                descripcionPoliticoNombre = ManejoNulos.ManageNullStr(dr["descripcionPoliticoNombre"]),
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
        public CAL_PoliticoEntidad GetIDPolitico(int id)
        {
            CAL_PoliticoEntidad item = new CAL_PoliticoEntidad();
            string consulta = @"SELECT pol.PoliticoID, 
                                pol.Nombres, 
                                pol.Apellidos, 
                                pol.Dni, 
                                pol.EntidadEstatal, 
                                pol.Meses, 
                                pol.Estado, 
                                pol.CargoPoliticoID, 
                                pol.TipoDOI, 
                                car.Nombre AS cargoPoliticoNombre,
                                car.Descripcion AS descripcionPoliticoNombre
                                FROM dbo.CAL_Politico pol
                                INNER JOIN dbo.CAL_CargoPolitico car ON pol.CargoPoliticoID = car.CargoPoliticoID 
                                WHERE pol.PoliticoID = @pPoliticoID ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pPoliticoID", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.PoliticoID = ManejoNulos.ManageNullInteger(dr["PoliticoID"]);
                                item.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]);
                                item.Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]);
                                item.Dni = ManejoNulos.ManageNullStr(dr["Dni"]);
                                item.EntidadEstatal = ManejoNulos.ManageNullStr(dr["EntidadEstatal"]);
                                item.Meses = ManejoNulos.ManageNullDecimal(dr["Meses"]);
                                item.Estado = ManejoNulos.ManegeNullBool(dr["Estado"]);
                                item.CargoPoliticoID = ManejoNulos.ManageNullInteger(dr["CargoPoliticoID"]);
                                item.cargoPoliticoNombre = ManejoNulos.ManageNullStr(dr["cargoPoliticoNombre"]);
                                item.descripcionPoliticoNombre = ManejoNulos.ManageNullStr(dr["descripcionPoliticoNombre"]);
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
        public int InsertarPolitico(CAL_PoliticoEntidad Entidad)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_Politico (Nombres, Apellidos, Dni, EntidadEstatal, Meses, FechaRegistro, Estado, CargoPoliticoID,TipoDOI)
                                OUTPUT Inserted.PoliticoID  
                                VALUES(@pNombres, @pApellidos, @pDni, @pEntidadEstatal, @pMeses, @pFechaRegistro, @pEstado, @pCargoPoliticoID,@pTipoDOI)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombres", ManejoNulos.ManageNullStr(Entidad.Nombres).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidos", ManejoNulos.ManageNullStr(Entidad.Apellidos).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDni", ManejoNulos.ManageNullStr(Entidad.Dni));
                    query.Parameters.AddWithValue("@pEntidadEstatal", ManejoNulos.ManageNullStr(Entidad.EntidadEstatal));
                    query.Parameters.AddWithValue("@pMeses", ManejoNulos.ManageNullDecimal(Entidad.Meses));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManegeNullBool(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCargoPoliticoID", ManejoNulos.ManageNullInteger(Entidad.CargoPoliticoID));
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
        public bool EditarPolitico(CAL_PoliticoEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CAL_Politico]
                               SET Nombres = @pNombres, 
                                    Apellidos = @pApellidos, 
                                    Dni = @pDni, 
                                    EntidadEstatal = @pEntidadEstatal, 
                                    Meses = @pMeses, 
                                    Estado = @pEstado, 
                                    CargoPoliticoID = @pCargoPoliticoID
                                    TipoDOI = @pTipoDOI
                                    WHERE PoliticoID = @pPoliticoID";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pPoliticoID", ManejoNulos.ManageNullInteger(Entidad.PoliticoID));
                    query.Parameters.AddWithValue("@pNombres", ManejoNulos.ManageNullStr(Entidad.Nombres).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pApellidos", ManejoNulos.ManageNullStr(Entidad.Apellidos).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDni", ManejoNulos.ManageNullStr(Entidad.Dni));
                    query.Parameters.AddWithValue("@pEntidadEstatal", ManejoNulos.ManageNullStr(Entidad.EntidadEstatal));
                    query.Parameters.AddWithValue("@pMeses", ManejoNulos.ManageNullDecimal(Entidad.Meses));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManegeNullBool(Entidad.Estado));
                    query.Parameters.AddWithValue("@pCargoPoliticoID", ManejoNulos.ManageNullInteger(Entidad.CargoPoliticoID));
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
        public bool EliminarPolitico(int id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_Politico 
                                WHERE PoliticoID  = @pPoliticoID";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pPoliticoID", ManejoNulos.ManageNullInteger(id));
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

        public CAL_PoliticoEntidad GetPoliticoPorDNI(string dni)
        {
            CAL_PoliticoEntidad item = new CAL_PoliticoEntidad();
            string consulta = @"SELECT pol.PoliticoID, 
                                pol.Nombres, 
                                pol.Apellidos, 
                                pol.Dni, 
                                pol.EntidadEstatal, 
                                pol.Meses, 
                                pol.Estado, 
                                pol.CargoPoliticoID, 
                                pol.TipoDOI, 
                                car.Nombre AS cargoPoliticoNombre,
                                car.Descripcion AS descripcionPoliticoNombre
                                FROM dbo.CAL_Politico pol
                                INNER JOIN dbo.CAL_CargoPolitico car ON pol.CargoPoliticoID = car.CargoPoliticoID 
                                WHERE pol.Dni = @pDni ";
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
                                item.PoliticoID = ManejoNulos.ManageNullInteger(dr["PoliticoID"]);
                                item.Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]);
                                item.Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]);
                                item.Dni = ManejoNulos.ManageNullStr(dr["Dni"]);
                                item.EntidadEstatal = ManejoNulos.ManageNullStr(dr["EntidadEstatal"]);
                                item.Meses = ManejoNulos.ManageNullDecimal(dr["Meses"]);
                                item.Estado = ManejoNulos.ManegeNullBool(dr["Estado"]);
                                item.CargoPoliticoID = ManejoNulos.ManageNullInteger(dr["CargoPoliticoID"]);
                                item.cargoPoliticoNombre = ManejoNulos.ManageNullStr(dr["cargoPoliticoNombre"]);
                                item.descripcionPoliticoNombre = ManejoNulos.ManageNullStr(dr["descripcionPoliticoNombre"]);
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
