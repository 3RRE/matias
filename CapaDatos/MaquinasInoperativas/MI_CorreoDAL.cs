using CapaEntidad.MaquinasInoperativas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_CorreoDAL {

        string conexion = string.Empty;
        public MI_CorreoDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<MI_CorreoEntidad> GetCorreosxMaquina(int codMaquinaInoperativa) {
            List<MI_CorreoEntidad> lista = new List<MI_CorreoEntidad>();
            string consulta = @"  SELECT
                                   cor.CodCorreo,
                                   cor.CodMaquinaInoperativa,
                                   cor.CodUsuario,
                                   cor.CodEstadoProceso,
                                   cor.CantEnvios,
                                   emp.MailJob as UsuarioMail,
                                   usu.UsuarioNombre as UsuarioNombre
                                   FROM MI_Correos cor
                                   INNER JOIN SEG_Usuario usu ON usu.UsuarioID=cor.CodUsuario
                                   INNER JOIN SEG_Empleado emp ON emp.EmpleadoID= usu.EmpleadoID WHERE cor.CodMaquinaInoperativa=@pCodMaquinaInoperativa";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(codMaquinaInoperativa));

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new MI_CorreoEntidad {
                                CodCorreo = ManejoNulos.ManageNullInteger(dr["CodCorreo"]),
                                CodMaquinaInoperativa = ManejoNulos.ManageNullInteger(dr["CodMaquinaInoperativa"]),
                                CodUsuario = ManejoNulos.ManageNullInteger(dr["CodUsuario"]),
                                CodEstadoProceso = ManejoNulos.ManageNullInteger(dr["CodEstadoProceso"]),
                                CantEnvios = ManejoNulos.ManageNullInteger(dr["CantEnvios"]),
                                UsuarioMail = ManejoNulos.ManageNullStr(dr["UsuarioMail"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"])
                            };

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public bool AgregarCorreo(MI_CorreoEntidad correo) {
            bool respuesta = false;
            string consulta = @"INSERT INTO [MI_Correos]   ([CodMaquinaInoperativa],[CodUsuario],[CodEstadoProceso])
                                VALUES (@pCodMaquinaInoperativa,@pCodUsuario,@pCodEstadoProceso)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaquinaInoperativa", ManejoNulos.ManageNullInteger(correo.CodMaquinaInoperativa));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullInteger(correo.CodUsuario));
                    query.Parameters.AddWithValue("@pCodEstadoProceso", ManejoNulos.ManageNullInteger(correo.CodEstadoProceso));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool ActulizarCantEnviosCorreo(int codCorreo) {
            bool respuesta = false;
            string consulta = @"UPDATE MI_Correos SET [CantEnvios] = [CantEnvios] +1 WHERE CodCorreo=@pCodCorreo";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCorreo", ManejoNulos.ManageNullInteger(codCorreo));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }



    }
}
