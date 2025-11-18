using CapaEntidad.Alertas;
using CapaEntidad.ContadoresNegativos;
using CapaEntidad.Discos;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ContadoresNegativos
{
    public class ContadoresNegativosDAL
    {
        string _conexion = string.Empty;
        public ContadoresNegativosDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int GuardarContadorNegativo(ContadoresNegativosEntidad contador)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO contadoresNegativos (codEmpresa, nombreEmpresa, codSala, nombreSala,codMaquina,fechaRegistroSala,descripcion, fechaRegistro,codigoId)
Output Inserted.idContadorNegativo
VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(contador.CodEmpresa));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(contador.NombreEmpresa));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(contador.CodSala));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(contador.NombreSala));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(contador.CodMaquina));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(contador.FechaRegistroSala));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(contador.Descripcion));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(contador.FechaRegistro));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger64(contador.CodigoId));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }


        public List<ContadoresNegativosEntidad> ListadoContadoresNegativosSala(string CodSala, DateTime fechaini, DateTime fechafin)
        {
            List<ContadoresNegativosEntidad> lista = new List<ContadoresNegativosEntidad>();
            string consulta = @"SELECT 
                               [idContadorNegativo]
                              ,[codEmpresa]
                              ,[nombreEmpresa]
                              ,[codSala]
                              ,[nombreSala]
                              ,[codMaquina]
                              ,[fechaRegistroSala]
                              ,[descripcion]
                              ,[fechaRegistro]
                             
                          FROM contadoresNegativos (nolock) where " + CodSala + " CONVERT(date, fechaRegistro) between @p0 and @p1  order by idContadorNegativo desc ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaini);
                    query.Parameters.AddWithValue("@p1", fechafin);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var contadorNegativo = new ContadoresNegativosEntidad
                            {
                                IdContadorNegativo = ManejoNulos.ManageNullInteger(dr["idContadorNegativo"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["codEmpresa"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["nombreEmpresa"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["codSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["nombreSala"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["codMaquina"]),
                                FechaRegistroSala = ManejoNulos.ManageNullDate(dr["fechaRegistroSala"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["fechaRegistro"]),

                            };

                            lista.Add(contadorNegativo);
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




    }
}
