using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CapaDatos {
    public class ModeloMaquinaDAL {
        string _conexion_s3k_ = string.Empty;
        public ModeloMaquinaDAL() {
            _conexion_s3k_ = ConfigurationManager.ConnectionStrings["conexion_s3k_administrativo"].ConnectionString;
        }

        public List<ModeloMaquinaEntidad> ListaModeloMaquinaJson(int id) {
            List<ModeloMaquinaEntidad> lista = new List<ModeloMaquinaEntidad>();
            string consulta = @"select * from ModeloMaquina m
                                where m.CodMarcaMaquina =" + id;
            try {
                using(var con = new SqlConnection(_conexion_s3k_)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var modelomaquina = new ModeloMaquinaEntidad {
                                CodModeloMaquina = ManejoNulos.ManageNullInteger(dr["CodModeloMaquina"]),
                                CodMarcaMaquina = ManejoNulos.ManageNullInteger(dr["CodMarcaMaquina"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Ncmod = ManejoNulos.ManageNullStr(dr["Ncmod"]),
                                Cimod = ManejoNulos.ManageNullStr(dr["Cimod"]),
                                ColorHexa = ManejoNulos.ManageNullStr(dr["ColorHexa"]),
                                Sigla = ManejoNulos.ManageNullStr(dr["Sigla"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManageNullInteger(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRd"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"])
                            };

                            lista.Add(modelomaquina);
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }
    }
}
