
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Discos;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace CapaDatos.Disco {
    public class DiscoDAL {
        string _conexion = string.Empty;
        public DiscoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<DiscoEntidad> ListadoDiscos(int codsala, DateTime fechaIni, DateTime fechaFin) {
            List<DiscoEntidad> lista = new List<DiscoEntidad>();
            string consulta = @"Select IdDisco,IpServidor,Nombre, Seudonimo,Tipo_Disco, Sistema_Disco, Capacidad_Total, Capacidad_EnUso,Capacidad_Libre,fechaRegistro from Disco where CodSala=" + codsala + "and convert(date,fechaRegistro) between @p1 and @p2";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new DiscoEntidad {
                                idDisco = ManejoNulos.ManageNullInteger(dr["IdDisco"]),
                                ipServidor = ManejoNulos.ManageNullStr(dr["IpServidor"]),
                                nombreDisco = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                seudonimoDisco = ManejoNulos.ManageNullStr(dr["Seudonimo"]),
                                tipoDisco = ManejoNulos.ManageNullStr(dr["Tipo_Disco"]),
                                sistemaDisco = ManejoNulos.ManageNullStr(dr["Sistema_Disco"]),
                                capacidadTotal = ManejoNulos.ManageNullStr(dr["Capacidad_Total"]),
                                capacidadEnUso = ManejoNulos.ManageNullStr(dr["Capacidad_EnUso"]),
                                capacidadLibre = ManejoNulos.ManageNullStr(dr["Capacidad_Libre"]),
                                fechaRegistro = ManejoNulos.ManageNullDate(dr["fechaRegistro"]),
                            };
                            lista.Add(item);
                        }
                    }
                }

            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }


        public List<DiscoEntidad> ListadoDiscosAll(string codsala, DateTime fechaIni, DateTime fechaFin) {
            List<DiscoEntidad> lista = new List<DiscoEntidad>();
            string consulta = @"
       SELECT 
            D.IdDisco, D.IpServidor, D.Nombre AS NombreDisco, D.Seudonimo, D.Tipo_Disco, D.Sistema_Disco, 
            D.Capacidad_Total, D.Capacidad_EnUso, D.Capacidad_Libre, D.fechaRegistro,
            S.Nombre AS NombreSala
        FROM 
            Disco D
            INNER JOIN Sala S ON D.CodSala = S.CodSala
        WHERE 
            D.CodSala IN (" + codsala + @") 
            AND CONVERT(date, D.fechaRegistro) BETWEEN @p1 AND @p2";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new DiscoEntidad {
                                idDisco = ManejoNulos.ManageNullInteger(dr["IdDisco"]),
                                ipServidor = ManejoNulos.ManageNullStr(dr["IpServidor"]),
                                nombreDisco = ManejoNulos.ManageNullStr(dr["NombreDisco"]),
                                seudonimoDisco = ManejoNulos.ManageNullStr(dr["Seudonimo"]),
                                tipoDisco = ManejoNulos.ManageNullStr(dr["Tipo_Disco"]),
                                sistemaDisco = ManejoNulos.ManageNullStr(dr["Sistema_Disco"]),
                                capacidadTotal = ManejoNulos.ManageNullStr(dr["Capacidad_Total"]),
                                capacidadEnUso = ManejoNulos.ManageNullStr(dr["Capacidad_EnUso"]),
                                capacidadLibre = ManejoNulos.ManageNullStr(dr["Capacidad_Libre"]),
                                fechaRegistro = ManejoNulos.ManageNullDate(dr["fechaRegistro"]),
                                nombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
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



        public int GuardarDiscoSala(DiscoEntidad discoLista) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO Disco (CodSala, Nombre,Seudonimo,Tipo_Disco,Sistema_Disco, Capacidad_Total, Capacidad_EnUso, Capacidad_Libre,fechaRegistro,IpServidor)
Output Inserted.IdDisco
VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8,@p9)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(discoLista.codSala));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(discoLista.nombreDisco));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(discoLista.seudonimoDisco));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(discoLista.tipoDisco));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(discoLista.sistemaDisco));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(discoLista.capacidadTotal));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(discoLista.capacidadEnUso));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(discoLista.capacidadLibre));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullDate(discoLista.fechaRegistro));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(discoLista.ipServidor));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public DiscoEntidad UltimoRegistro(int codSala)
        {
            DiscoEntidad lista = new DiscoEntidad();
            string consulta = @"SELECT TOP 1 IdDisco,Nombre,IpServidor, Seudonimo,Tipo_Disco, Sistema_Disco, Capacidad_Total, Capacidad_EnUso,Capacidad_Libre,fechaRegistro from Disco where codsala =" + codSala+"ORDER BY IdDisco Desc";
            

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

                            lista.idDisco = ManejoNulos.ManageNullInteger(dr["IdDisco"]);
                            lista.ipServidor = ManejoNulos.ManageNullStr(dr["IpServidor"]);
                            lista.nombreDisco = ManejoNulos.ManageNullStr(dr["Nombre"]);
                            lista.seudonimoDisco = ManejoNulos.ManageNullStr(dr["Seudonimo"]);
                            lista.tipoDisco = ManejoNulos.ManageNullStr(dr["Tipo_Disco"]);
                            lista.sistemaDisco = ManejoNulos.ManageNullStr(dr["Sistema_Disco"]);
                            lista.capacidadTotal = ManejoNulos.ManageNullStr(dr["Capacidad_Total"]);
                            lista.capacidadEnUso = ManejoNulos.ManageNullStr(dr["Capacidad_EnUso"]);
                            lista.capacidadLibre = ManejoNulos.ManageNullStr(dr["Capacidad_Libre"]);
                            lista.fechaRegistro = ManejoNulos.ManageNullDate(dr["fechaRegistro"]);
                          
                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
            }
            return lista;
        }

        public List<EspacioDiscoBD> ListadoBDsAzure() {
            List<EspacioDiscoBD> lista = new List<EspacioDiscoBD>();
            string consulta = @"
                                SELECT sdt.database_id as Id, 
		                        sdt.name as NombreBD, 
		                        (SELECT size * 8 / 1024 FROM sys.master_files WHERE database_id>4 AND physical_name LIKE '%.mdf' AND database_id=sdt.database_id) as EspacioBD,
		                        smf.name as NombreLog, 
		                        smf.size * 8 / 1024 AS EspacioLog, 
		                        SDT.create_date as FechaCreacion
                                FROM sys.databases sdt
		                        INNER JOIN sys.master_files smf ON sdt.database_id=smf.database_id
		                        WHERE sdt.database_id>4 AND smf.name LIKE '%log'
                                ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new EspacioDiscoBD {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                NombreBD = ManejoNulos.ManageNullStr(dr["NombreBD"]),
                                EspacioBD = ManejoNulos.ManageNullStr(dr["EspacioBD"]),
                                NombreLog = ManejoNulos.ManageNullStr(dr["NombreLog"]),
                                EspacioLog = ManejoNulos.ManageNullStr(dr["EspacioLog"]),
                                FechaCreacion = ManejoNulos.ManageNullDate(dr["FechaCreacion"]),
                            };
                            lista.Add(item);
                        }
                    }
                }

            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            finally {
            }
            return lista;
        }

        public bool LimpiarLogBDAzure(string nombreBD, string nombreLog) {

            bool respuesta = false;
            string consulta = @"
                                USE [" + nombreBD + @"];  

                                ALTER DATABASE [" + nombreBD + @"]  
                                SET RECOVERY SIMPLE;  

                                DBCC SHRINKFILE ([" + nombreLog + @"], 1);

                                ALTER DATABASE [" + nombreBD + @"]  
                                SET RECOVERY FULL;  
                                ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
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