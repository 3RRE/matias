using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad.AsistenciaEmpleado;
using S3k.Utilitario;

namespace CapaDatos.AsistenciaEmpleado
{
    public class EmpleadoDispositivoDAL
    {
        string _conexion = string.Empty;
        public EmpleadoDispositivoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public EmpleadoDispositivo EmpleadoDispositivoemp_IdObtenerJson(Int64 emp_id)
        {
            EmpleadoDispositivo empleadoDispositivo = new EmpleadoDispositivo();
            string consulta = @"SELECT 
                                emd_id , 
                                emd_imei , 
                                emp_id , 
                                emd_estado
	                            FROM EmpleadoDispositivo
                                where emp_id=@p0;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", emp_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                empleadoDispositivo.emd_id = ManejoNulos.ManageNullInteger(dr["emd_id"]);
                                empleadoDispositivo.emd_imei = ManejoNulos.ManageNullStr(dr["emd_imei"]);
                                empleadoDispositivo.emp_id = ManejoNulos.ManageNullInteger(dr["emp_id"]);
                                empleadoDispositivo.emd_estado = ManejoNulos.ManageNullInteger(dr["emd_estado"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return empleadoDispositivo;
        }

        public int EmpleadoDispositivoInsertarJson(EmpleadoDispositivo empleadoDispositivo)
        {
            //bool response = false;
            int idempleadoDispositivoInsertado = 0;
            string consulta = @"
            INSERT INTO EmpleadoDispositivo(emd_imei, emp_id, emd_estado)
	            VALUES (@p0, @p1, @p2) 
                SELECT SCOPE_IDENTITY()";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(empleadoDispositivo.emd_imei));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(empleadoDispositivo.emp_id));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(empleadoDispositivo.emd_estado));
                    idempleadoDispositivoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return idempleadoDispositivoInsertado;
        }
        public bool EmpleadoDispositivoEditarJson(EmpleadoDispositivo empleadoDispositivoEditado)
        {
           
            bool response = false;
            string consulta = @"UPDATE EmpleadoDispositivo
	                            SET emd_imei=@p1, emp_id=@p2, emd_estado=@p3
	                            WHERE emd_id=@p4;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(empleadoDispositivoEditado.emd_imei));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(empleadoDispositivoEditado.emp_id));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(empleadoDispositivoEditado.emd_estado));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(empleadoDispositivoEditado.emd_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        public bool EmpleadoDispositivoEditarFirebaseJson(EmpleadoDispositivo empleadoDispositivoEditado)
        {

            bool response = false;
            //string consulta = @"UPDATE EmpleadoDispositivo
            //                 SET emd_firebaseid=@p1
            //                 WHERE emp_id=@p2;";
            string consulta = @"declare @dispositivo int    
set @dispositivo=(SELECT count(emd_id) FROM [EmpleadoDispositivo] (NOLOCK) WHERE emp_id=@p2)  
if (@dispositivo=0)
	INSERT INTO EmpleadoDispositivo(emp_id,emd_firebaseid, emd_estado)
	            VALUES (@p2,@p1, 1)
else
    UPDATE EmpleadoDispositivo
	                            SET emd_firebaseid=@p1
	                            WHERE emp_id=@p2
	 ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(empleadoDispositivoEditado.emd_firebaseid));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(empleadoDispositivoEditado.emp_id));

                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool EditarFirebaseJson(EmpleadoDispositivo empleadoDispositivoEditado)
        {

            bool response = false;
            string consulta = @" UPDATE EmpleadoDispositivo
	                            SET emd_firebaseid=@p1,emd_imei=@p3
	                            WHERE emp_id=@p2";
            try
            {
                using(var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(empleadoDispositivoEditado.emd_firebaseid));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(empleadoDispositivoEditado.emp_id));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(empleadoDispositivoEditado.emd_imei));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
    }
}
