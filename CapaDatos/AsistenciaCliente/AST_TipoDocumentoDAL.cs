using CapaEntidad.AsistenciaCliente;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.AsistenciaCliente
{
    public class AST_TipoDocumentoDAL
    {
        string _conexion = string.Empty;
        public AST_TipoDocumentoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<AST_TipoDocumentoEntidad> GetListadoTipoDocumento()
        {
            List<AST_TipoDocumentoEntidad> lista = new List<AST_TipoDocumentoEntidad>();
            string consulta = @"SELECT [Id]
                              ,[Nombre]
                          FROM [dbo].[AST_TipoDocumento]";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var tipoDocumento = new AST_TipoDocumentoEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Nombre= ManejoNulos.ManageNullStr(dr["Nombre"]),
                            };

                            lista.Add(tipoDocumento);
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
        public AST_TipoDocumentoEntidad GetTipoDocumentoID(int TipoDocID)
        {
            AST_TipoDocumentoEntidad tipodoc = new AST_TipoDocumentoEntidad();
            string consulta = @"SELECT [Id]
                                  ,[Nombre]
                              FROM [dbo].[AST_TipoDocumento] where [Id]=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", TipoDocID);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                tipodoc.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                tipodoc.Nombre= ManejoNulos.ManageNullStr(dr["Nombre"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                tipodoc.Id = 0;
            }
            return tipodoc;
        }
        public int GuardarTipoDocumento(AST_TipoDocumentoEntidad tipoDocumento)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[AST_TipoDocumento]
                       ([Nombre])
                        Output Inserted.Id
                        VALUES
                       (@p0);";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(tipoDocumento.Nombre));
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
    }
}
