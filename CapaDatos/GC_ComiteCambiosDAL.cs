using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using S3k.Utilitario;

namespace CapaDatos
{
    public class GC_ComiteCambiosDAL

    {
        string _conexion = string.Empty;
        public GC_ComiteCambiosDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public bool ComiteCambiosGuardarJson(ComiteCambios comite)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[ComiteCambios]
           ([EmpleadoID],[Estado])
                VALUES(@p0,@p1) SELECT DISTINCT B.EmpleadoID
            FROM ComiteCambios B
            WHERE B.EmpleadoID IS NULL";
           
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(comite.EmpleadoID));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(comite.Estado));
                    
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public bool ComiteCambioActualizarJson()
        {
            bool respuesta = false;
            string consulta = @"TRUNCATE table [dbo].[ComiteCambios]";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public List<ComiteCambios> ComiteCambiosListadoJson()
        {
            List<ComiteCambios> lista = new List<ComiteCambios>();
            string consulta = @"select  cc.[EmpleadoID]
                          ,(emp.[ApellidosPaterno]+' '+emp.[ApellidosMaterno]+', '+emp.[Nombres]) NombreCompleto
                          ,[Nombres]
                          ,[ApellidosPaterno]
                          ,[ApellidosMaterno] from  [dbo].[ComiteCambios] cc
                            INNER JOIN SEG_Empleado emp on cc.EmpleadoID=emp.EmpleadoID";
            
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@p0", 1);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var comiteCambios = new ComiteCambios
                            {
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"].Trim()),
                                ApellidosPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"].Trim()),
                                ApellidosMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"].Trim()),
                                

                            };

                            lista.Add(comiteCambios);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }

        public bool VerificarEmpleadoComiteCambio(int id)
        {
            bool respuesta = false;
            List<ComiteCambios> lista = new List<ComiteCambios>();
            string consulta = @"select c.EmpleadoComiteId,c.EmpleadoID,c.Estado
                                from ComiteCambios c
                                where c.EmpleadoID ="+id;
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
                            var comiteCambios = new ComiteCambios
                            {
                                EmpleadoComiteId = ManejoNulos.ManageNullInteger(dr["EmpleadoComiteId"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"])
                            };

                            lista.Add(comiteCambios);
                        }
                        if(lista.Count != 0)
                        {
                            respuesta = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

    }
}
