using CapaEntidad.Alertas;
using CapaEntidad.ContadoresNegativos;
using CapaEntidad.Disco;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Disco {
    public class AlertaDiscoDAL {
        string _conexion = string.Empty;
        public AlertaDiscoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<string> AlertaDiscosCorreosListado(int codsala)
        {
            List<string> lista = new List<string>();
            string consulta = @"SELECT
	                            e.MailJob
                            from SEG_Empleado e 
                            join DiscoCargoConfiguracion cargoDisco on cargoDisco.cargo_id=e.CargoID
                            join SEG_Usuario usu on usu.EmpleadoID=e.EmpleadoID
                            join UsuarioSala ususala on ususala.UsuarioId=usu.UsuarioID
                            where cargoDisco.sala_id =@p0 and ususala.SalaId=@p1 ";
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

        public List<AlertaDiscoEntidad> AlertaDisco_xdevicesListado(int codsala) {
            List<AlertaDiscoEntidad> lista = new List<AlertaDiscoEntidad>();
            string consulta = @"SELECT
	                            [emd_id]
                                ,[emd_imei]
                                ,[emp_id]
                                ,[emd_estado]
	                            ,emd_firebaseid
	                            ,e.CargoID
	                            ,cargoDisco.sala_id
                            FROM [EmpleadoDispositivo] ed
                            join SEG_Empleado e on e.EmpleadoID=ed.emp_id
                            join DiscoCargoConfiguracion cargoDisco on cargoDisco.cargo_id=e.CargoID
                            join SEG_Usuario usu on usu.EmpleadoID=ed.emp_id
                            join UsuarioSala ususala on ususala.UsuarioId=usu.UsuarioID
                            where cargoDisco.sala_id =@p0 and ususala.SalaId=@p1 and ed.emd_firebaseid IS not NUll order by emd_id desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codsala);
                    query.Parameters.AddWithValue("@p1", codsala);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campania = new AlertaDiscoEntidad {
                                emd_id = ManejoNulos.ManageNullInteger64(dr["emd_id"]),
                                emd_imei = ManejoNulos.ManageNullStr(dr["emd_imei"]),
                                emp_id = ManejoNulos.ManageNullInteger(dr["emp_id"]),
                                id = ManejoNulos.ManageNullStr(dr["emd_firebaseid"]),
                                CargoID = ManejoNulos.ManageNullInteger(dr["CargoID"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                            };

                            lista.Add(campania);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }



        public ALT_AlertaSalaEntidad ALT_AlertasalaAlertaIdObtenerJson(Int64 alertaid, int CodSala) {
            ALT_AlertaSalaEntidad alertaSalaEntidad = new ALT_AlertaSalaEntidad();
            string consulta = @"SELECT 
                                alts_id ,
                                AlertaID,
                                CodEmpresa , 
                                NombreEmpresa , 
                                CodSala , 
                                NombreSala , 
                                CodMaquina , 
                                CodMarcaMaquina , 
                                Juego , 
                                fecha_registro , 
                                fecha_termino , 
                                cod_tipo_alerta , 
                                descripcion_alerta , 
                                ColorAlerta , 
                                contador_bill_parcial , 
                                contador_bill_billetero , 
                                estado , 
                                alts_fechareg
	                            FROM ALT_AlertaSala
                                where AlertaID=@p0 and CodSala=@CodSala;";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", alertaid);
                    query.Parameters.AddWithValue("@CodSala", CodSala);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                alertaSalaEntidad.alts_id = ManejoNulos.ManageNullInteger64(dr["alts_id"]);
                                alertaSalaEntidad.AlertaID = ManejoNulos.ManageNullInteger64(dr["AlertaID"]);
                                alertaSalaEntidad.CodEmpresa = ManejoNulos.ManageNullStr(dr["CodEmpresa"]);
                                alertaSalaEntidad.NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]);
                                alertaSalaEntidad.CodSala = ManejoNulos.ManageNullStr(dr["CodSala"]);
                                alertaSalaEntidad.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                                alertaSalaEntidad.CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]);
                                alertaSalaEntidad.CodMarcaMaquina = ManejoNulos.ManageNullStr(dr["CodMarcaMaquina"]);
                                alertaSalaEntidad.Juego = ManejoNulos.ManageNullStr(dr["Juego"]);
                                alertaSalaEntidad.fecha_registro = ManejoNulos.ManageNullStr(dr["fecha_registro"]);
                                alertaSalaEntidad.fecha_termino = ManejoNulos.ManageNullStr(dr["fecha_termino"]);
                                alertaSalaEntidad.cod_tipo_alerta = ManejoNulos.ManageNullInteger(dr["cod_tipo_alerta"]);
                                alertaSalaEntidad.descripcion_alerta = ManejoNulos.ManageNullStr(dr["descripcion_alerta"]);
                                alertaSalaEntidad.ColorAlerta = ManejoNulos.ManageNullStr(dr["ColorAlerta"]);
                                alertaSalaEntidad.contador_bill_parcial = ManejoNulos.ManageNullDecimal(dr["contador_bill_parcial"]);
                                alertaSalaEntidad.contador_bill_billetero = ManejoNulos.ManageNullDecimal(dr["contador_bill_billetero"]);
                                alertaSalaEntidad.estado = ManejoNulos.ManageNullInteger(dr["estado"]);
                                alertaSalaEntidad.alts_fechareg = ManejoNulos.ManageNullDate(dr["alts_fechareg"]);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return alertaSalaEntidad;
        }
    }
}
