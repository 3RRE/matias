using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using S3k.Utilitario;
namespace CapaDatos
{
    public class UbigeoDAL
    {
        string _conexion = string.Empty;
        public UbigeoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<UbigeoEntidad> CAL_ListadoPais()
        {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            string consulta = @"select DepartamentoId,Nombre as Nombre from UBIGEO DepartamentoId = 0 and ProvinciaId=0 and DistritoId=0 order by [Nombre] asc";
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
                            var Departamento = new UbigeoEntidad
                            {
                                DepartamentoId = ManejoNulos.ManageNullInteger(dr["DepartamentoId"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"])
                            };
                            lista.Add(Departamento);
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

        public List<UbigeoEntidad> ListaPaises()
        {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();

            string query = @"
            SELECT 
                CodUbigeo,
                PaisId,
                Nombre
            FROM Ubigeo
            WHERE DepartamentoId = 0 AND ProvinciaId = 0 AND DistritoId = 0 AND Activo = 1 AND Estado = 1
            ORDER BY Nombre ASC
            ";

            try
            {
                using(SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            UbigeoEntidad ubigeo = new UbigeoEntidad
                            {
                                CodUbigeo = ManejoNulos.ManageNullInteger(reader["CodUbigeo"]),
                                PaisId = ManejoNulos.ManageNullStr(reader["PaisId"]),
                                Nombre = ManejoNulos.ManageNullStr(reader["Nombre"])
                            };

                            lista.Add(ubigeo);
                        }
                    }

                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return lista;
        }

        public List<UbigeoEntidad> ListadoDepartamento()
        {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            string consulta = @"select DepartamentoId,REPLACE([Nombre],'DEPARTAMENTO ','') as Nombre from UBIGEO where PaisId='PE' and DepartamentoId != 0 and ProvinciaId=0 and DistritoId=0 order by [Nombre] asc";
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
                            var Departamento = new UbigeoEntidad
                            {
                                DepartamentoId = ManejoNulos.ManageNullInteger(dr["DepartamentoId"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"])
                            };
                            lista.Add(Departamento);
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
        public List<UbigeoEntidad> GetListadoProvincia(int DepartamentoID)
        {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            string consulta = @"select ProvinciaId,Nombre from UBIGEO where PaisId='PE' and DepartamentoId = @pDepartamentoId and ProvinciaId!=0 and DistritoId=0 order by [Nombre] asc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDepartamentoId", ManejoNulos.ManageNullInteger(DepartamentoID));
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var Departamento = new UbigeoEntidad
                            {
                                ProvinciaId = ManejoNulos.ManageNullInteger(dr["ProvinciaId"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"])
                            };
                            lista.Add(Departamento);
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
        public List<UbigeoEntidad> GetListadoDistrito(int ProvinciaID, int DepartamentoID)
        {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            string consulta = @"select CodUbigeo,Nombre from UBIGEO where PaisId='PE' and DepartamentoId = @pDepartamentoId and ProvinciaId=@pProvinciaId and DistritoId!=0 order by [Nombre] asc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pProvinciaId", ManejoNulos.ManageNullInteger(ProvinciaID));
                    query.Parameters.AddWithValue("@pDepartamentoId", ManejoNulos.ManageNullInteger(DepartamentoID));
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var Departamento = new UbigeoEntidad
                            {
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"])
                            };
                            lista.Add(Departamento);
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
        public UbigeoEntidad GetDatosUbigeo(int CodUbigeo)
        {
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            string consulta = @"select CodUbigeo,DepartamentoId,ProvinciaId,Nombre from UBIGEO where PaisId='PE' and CodUbigeo=@pCodUbigeo";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodUbigeo", ManejoNulos.ManageNullInteger(CodUbigeo));
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ubigeo.CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]);
                            ubigeo.DepartamentoId = ManejoNulos.ManageNullInteger(dr["DepartamentoId"]);
                            ubigeo.ProvinciaId = ManejoNulos.ManageNullInteger(dr["ProvinciaId"]);
                            ubigeo.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ubigeo.CodUbigeo = 0;
            }
            return ubigeo;
        }

        public List<UbigeoEntidad> ListaPaisesConCodigoTelefonico() {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();

            string query = @"
                SELECT
	                CodUbigeo,
	                PaisId,
	                Nombre,
	                CodigoTelefonico,
	                Bandera
                FROM
	                Ubigeo
                WHERE
	                DepartamentoId = 0 AND
	                ProvinciaId = 0 AND
	                DistritoId = 0 AND
	                PaisId NOT LIKE'1%' AND
	                Estado = 1 AND
	                Activo = 1
                ORDER BY
	                Nombre ASC
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    using(SqlDataReader reader = command.ExecuteReader()) {
                        while(reader.Read()) {
                            UbigeoEntidad ubigeo = new UbigeoEntidad {
                                CodUbigeo = ManejoNulos.ManageNullInteger(reader["CodUbigeo"]),
                                PaisId = ManejoNulos.ManageNullStr(reader["PaisId"]),
                                Nombre = ManejoNulos.ManageNullStr(reader["Nombre"]),
                                CodigoTelefonico = ManejoNulos.ManageNullStr(reader["CodigoTelefonico"]),
                                Bandera = ManejoNulos.ManageNullStr(reader["Bandera"])
                            };
                            lista.Add(ubigeo);
                        }
                    }
                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);
            }

            return lista;
        }
    }
}
