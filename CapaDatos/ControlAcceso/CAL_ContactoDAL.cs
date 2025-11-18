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
    public class CAL_ContactoDAL
    {
        string conexion = string.Empty;
        public CAL_ContactoDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int InsertarContacto(CAL_ContactoEntidad Entidad)
        {
            int idInsertado = 0;
            SqlConnection cnn = null;
     
            string Consulta = @"INSERT INTO [dbo].[CAL_Contacto]
           ([Nombre]
           ,[ApellidoPaterno]
           ,[ApellidoMaterno]
           ,[Telefono]
           ,[Celular])
   OUTPUT Inserted.ContactoID
     VALUES
           (@pNombre 
           ,@pApellidoPaterno 
           ,@pApellidoMaterno 
           ,@pTelefono 
           ,@pCelular )
            ";
            try
            {
                using (cnn = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(Consulta, cnn))
                    {
                        cnn.Open();
                        cmd.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.NombreContacto).ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@pApellidoPaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoPaternoContacto).ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@pApellidoMaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoMaternoContacto).ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@pTelefono", ManejoNulos.ManageNullStr(Entidad.TelefonoContacto));
                        cmd.Parameters.AddWithValue("@pCelular", ManejoNulos.ManageNullStr(Entidad.CelularContacto));
                        idInsertado = (int)(cmd.ExecuteScalar());
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
        public bool UpdateContacto(CAL_ContactoEntidad Entidad)
        {
            bool estatus = false;
            SqlConnection cnn = null;
            string Consulta = @"UPDATE [dbo].[CAL_Contacto]
   SET [Nombre] = @pNombre
      ,[ApellidoPaterno] = @pApellidoPaterno
      ,[ApellidoMaterno] = @pApellidoMaterno
      ,[Telefono] = @pTelefono
      ,[Celular] = @pCelular
 WHERE   ContactoID = @pContactoID";
            try
            {
                using (cnn = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(Consulta, cnn))
                    {
                        cnn.Open();
                        cmd.Parameters.AddWithValue("@pContactoID", ManejoNulos.ManageNullInteger(Entidad.ContactoID));
                        cmd.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.NombreContacto).ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@pApellidoPaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoPaternoContacto).ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@pApellidoMaterno", ManejoNulos.ManageNullStr(Entidad.ApellidoMaternoContacto).ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@pTelefono", ManejoNulos.ManageNullStr(Entidad.TelefonoContacto));
                        cmd.Parameters.AddWithValue("@pCelular", ManejoNulos.ManageNullStr(Entidad.CelularContacto));
                        cmd.ExecuteNonQuery();
                        //cmd.ExecuteScalar();
                        //Entidad.ContactoID = Conversiones.Valor<int>(cmd.ExecuteScalar());
                        estatus = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                estatus =false;
            }
            return estatus;
        }

        public bool UpdateContactoInLudopatas(int idcontacto, int idludopata)
        {
            bool estatus = false;
            SqlConnection cnn = null;
            SqlDataReader dr = null;
            string Consulta = @"UPDATE [dbo].[CAL_Contacto]
   SET 
        [ContactoID] = @pContactoID
    
 WHERE LudopataID=@pLudopataID";
            try
            {
                using (cnn = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(Consulta, cnn))
                    {
                        cnn.Open();
                        cmd.Parameters.AddWithValue("@pContactoID",ManejoNulos.ManageNullInteger(idcontacto));
                        cmd.Parameters.AddWithValue("@pLudopataID", ManejoNulos.ManageNullInteger(idludopata));
                        cmd.ExecuteNonQuery();
                        estatus = true;
                    }
                }

            }
        
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                estatus = false;
            }
        
            return estatus;
        }

        public CAL_ContactoEntidad GetContactoByID(int id)
        {
            CAL_ContactoEntidad contacto = new CAL_ContactoEntidad();
            SqlConnection cnn = null;
            SqlDataReader dr = null;

  //          string Consulta = @"SELECT ct.[ContactoID]
  //    , ct.[Nombre] as NombreContacto
  //    , ct.[ApellidoPaterno]  as ApellidoPaternoContacto
  //    , ct.[ApellidoMaterno] as ApellidoMaternoContacto
  //    , ct.[Telefono] as TelefonoContacto
  //    , ct.[Celular] as CelularContacto
	 // ,lp.LudopataID
  //     ,lp.Nombre
	 // ,lp.ApellidoPaterno
	 // ,lp.ApellidoMaterno
	 // ,lp.CodRegistro
	 // ,lp.FechaInscripcion
	 // ,lp.TipoExclusion
  //    ,lp.DNI
  //FROM Contacto ct
  // inner join Ludopata lp on  ct.[ContactoID] = lp.ContactoID
  // where ct.[ContactoID]=@pContactoID ";
            string Consulta = @"SELECT [ContactoID]
      ,[Nombre]
      ,[ApellidoPaterno]
      ,[ApellidoMaterno]
      ,[Telefono]
      ,[Celular]
  FROM [dbo].[CAL_Contacto]
   where [ContactoID]=@pContactoID ";
            try
            {
                using (cnn = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(Consulta, cnn))
                    {

                        cnn.Open();
                        cmd.Parameters.AddWithValue("@pContactoID", id);
                        using (dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                contacto = new CAL_ContactoEntidad
                                {
                                    ContactoID = ManejoNulos.ManageNullInteger(dr["ContactoID"]),
                                    ApellidoPaternoContacto = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                    ApellidoMaternoContacto = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                    NombreContacto = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                    CelularContacto = ManejoNulos.ManageNullStr(dr["Celular"]),
                                    TelefonoContacto = ManejoNulos.ManageNullStr(dr["Telefono"]),
                                };
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                contacto = new CAL_ContactoEntidad();
            }
          
            return contacto;
        }
        public bool EliminarContacto(int id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_Contacto 
                                WHERE ContactoID  = @pContactoID";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pContactoID", ManejoNulos.ManageNullInteger(id));
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
    }
}
