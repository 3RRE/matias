using CapaDatos.Utilitarios;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class SEG_AuditoriaDAL
    {
        string _conexion = string.Empty;
        public SEG_AuditoriaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool Guardar(SEG_AUDITORIA auditoria)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO SEG_Auditoria
                                            (
                                             [fechaRegistro]
                                              ,[usuario]
                                              ,[proceso]
                                              ,[descripcion]
                                              ,[subsistema]
                                              ,[ip]
                                            ,[usuariodata]
                                            ,[datainicial]
                                            ,[datafinal]
                                            ,[codSala]
                                            ,sala
                                            ,formularioID
                                        )
                                            VALUES
                                            (
                                            @fechaRegistro
                                            ,@usuario
                                            ,@proceso
                                            ,@descripcion
                                            ,@subsistema
                                            ,@ip
                                            ,@usuariodata
                                            ,@datainicial
                                            ,@datafinal
                                            ,@codSala
                                            ,@sala
                                            ,@formularioID
                                            )";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@fechaRegistro", ManejoNulls.ManageNullDate(auditoria.fechaRegistro));
                    query.Parameters.AddWithValue("@usuario", ManejoNulls.ManageNullStr(auditoria.usuario));
                    query.Parameters.AddWithValue("@proceso"    , ManejoNulls.ManageNullStr(auditoria.proceso));
                    query.Parameters.AddWithValue("@descripcion", ManejoNulls.ManageNullStr(auditoria.descripcion));
                    query.Parameters.AddWithValue("@subsistema", ManejoNulls.ManageNullStr(auditoria.subsistema));
                    query.Parameters.AddWithValue("@ip", ManejoNulls.ManageNullStr(auditoria.ip));
                    query.Parameters.AddWithValue("@usuariodata", ManejoNulls.ManageNullStr(auditoria.usuariodata));
                    query.Parameters.AddWithValue("@datainicial", ManejoNulls.ManageNullStr(auditoria.datainicial));
                    query.Parameters.AddWithValue("@datafinal", ManejoNulls.ManageNullStr(auditoria.datafinal));
                    query.Parameters.AddWithValue("@codSala", ManejoNulls.ManageNullStr(auditoria.codSala));
                    query.Parameters.AddWithValue("@sala", ManejoNulls.ManageNullStr(auditoria.sala));
                    query.Parameters.AddWithValue("@formularioID", ManejoNulls.ManageNullStr(auditoria.formularioID));
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

        public List<SEG_AUDITORIA> GetAllRangoFechas(DateTime fecha, DateTime fecha2 )
        {
            var lista = new List<SEG_AUDITORIA>();
            string consulta = @"select * from SEG_Auditoria WHERE fechaRegistro between @fecha1 and @fecha2 order by codAuditoria desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@fecha1", ManejoNulls.ManageNullDate(fecha));
                    query.Parameters.AddWithValue("@fecha2", ManejoNulls.ManageNullDate(fecha2));
                    //query.Parameters.AddWithValue("@modulo", ManejoNulls.ManageNullStr(txtmodulo));

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var fila = new SEG_AUDITORIA
                            {
                                codAuditoria=   ManejoNulls.ManageNullInteger(dr["codAuditoria"]),
                                fechaRegistro = ManejoNulls.ManageNullDate(dr["fechaRegistro"]),
                                usuario = ManejoNulls.ManageNullStr(dr["usuario"]),
                                proceso = ManejoNulls.ManageNullStr(dr["proceso"]),
                                descripcion = ManejoNulls.ManageNullStr(dr["descripcion"]),
                                subsistema = ManejoNulls.ManageNullStr(dr["subsistema"]),
                                codSala = ManejoNulls.ManageNullInteger(dr["codSala"]),
                                sala = ManejoNulls.ManageNullStr(dr["sala"]),
                                ip = ManejoNulls.ManageNullStr(dr["ip"]),
                                usuariodata = ManejoNulls.ManageNullStr(dr["usuariodata"]),
                                formularioID = ManejoNulls.ManageNullInteger(dr["formularioID"]),
                            };

                            lista.Add(fila);
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

        public SEG_AUDITORIA getDataAuditoria(int id)
        {
            SEG_AUDITORIA segData = new SEG_AUDITORIA();
            string consulta = @"select * from SEG_Auditoria WHERE codAuditoria= @id";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@id", ManejoNulls.ManageNullInteger(id));

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                segData.codAuditoria = ManejoNulls.ManageNullInteger(dr["codAuditoria"]);
                                segData.fechaRegistro = ManejoNulls.ManageNullDate(dr["fechaRegistro"]);
                                segData.proceso = ManejoNulls.ManageNullStr(dr["proceso"]);
                                segData.ip = ManejoNulls.ManageNullStr(dr["ip"]);
                                segData.codSala = ManejoNulls.ManageNullInteger(dr["codSala"]);
                                segData.sala = ManejoNulls.ManageNullStr(dr["sala"]);
                                segData.datainicial = ManejoNulls.ManageNullStr(dr["datainicial"]);
                                segData.datafinal = ManejoNulls.ManageNullStr(dr["datafinal"]);
                                segData.usuario = ManejoNulls.ManageNullStr(dr["usuario"]);
                                segData.usuariodata = ManejoNulls.ManageNullStr(dr["usuariodata"]);
                                segData.formularioID = ManejoNulls.ManageNullInteger(dr["formularioID"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return segData;
        }
    }
}
