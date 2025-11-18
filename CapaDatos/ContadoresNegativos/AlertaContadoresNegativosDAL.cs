using CapaEntidad.ContadoresNegativos;
using CapaEntidad.Disco;
using S3k.Utilitario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ContadoresNegativos
{
    public class AlertaContadoresNegativosDAL
    {
        string _conexion = string.Empty;
        public AlertaContadoresNegativosDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<string> AlertaContadorCorreosListado(int codsala)
        {
            List<string> lista =new List<string>();
            string consulta = @"SELECT
                                e.MailJob
                            from SEG_Empleado e
                            join ContadorNegativoConfig cargoContador on cargoContador.cargo_id = e.CargoID
                            join SEG_Usuario usu on usu.EmpleadoID = e.EmpleadoID
                            join UsuarioSala ususala on ususala.UsuarioId = usu.UsuarioID
                            where cargoContador.sala_id = @p0 and ususala.SalaId = @p1";

         
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codsala);
                    query.Parameters.AddWithValue("@p1", codsala);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var campania = new AlertaContadorNegativoEntidad
                            {
                               
                                mailJob = ManejoNulos.ManageNullStr(dr["MailJob"]),
                                
                            };

                            lista.Add(campania.mailJob);
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
        public List<AlertaContadorNegativoEntidad> AlertaContadoresNegativos(int codsala)
        {
            List<AlertaContadorNegativoEntidad> lista = new List<AlertaContadorNegativoEntidad>();
            string consulta = @"SELECT
	                            [emd_id]
                                ,[emd_imei]
                                ,[emp_id]
                                ,[emd_estado]
	                            ,emd_firebaseid
	                            ,e.CargoID
	                            ,cargoContador.sala_id
                                ,e.MailJob
                            FROM [EmpleadoDispositivo] ed
                            join SEG_Empleado e on e.EmpleadoID=ed.emp_id
                            join ContadorNegativoConfig cargoContador on cargoContador.cargo_id=e.CargoID
                            join SEG_Usuario usu on usu.EmpleadoID=ed.emp_id
                            join UsuarioSala ususala on ususala.UsuarioId=usu.UsuarioID
                            where cargoContador.sala_id =@p0 and ususala.SalaId=@p1 and ed.emd_firebaseid IS not NUll order by emd_id desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codsala);
                    query.Parameters.AddWithValue("@p1", codsala);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var campania = new AlertaContadorNegativoEntidad
                            {
                                emd_id = ManejoNulos.ManageNullInteger64(dr["emd_id"]),
                                emd_imei = ManejoNulos.ManageNullStr(dr["emd_imei"]),
                                emp_id = ManejoNulos.ManageNullInteger(dr["emp_id"]),
                                id = ManejoNulos.ManageNullStr(dr["emd_firebaseid"]),
                                mailJob = ManejoNulos.ManageNullStr(dr["MailJob"]),
                                CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                            };

                            lista.Add(campania);
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
