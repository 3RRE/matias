using CapaEntidad.Discos;
using CapaEntidad.Equipo;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Equipo
{
    public class EquipoDAL
    {
        string _conexion = string.Empty;
        public EquipoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<EquipoEntidad> ListadoEquipoInfo(int codsala, DateTime fechaIni, DateTime fechaFin)
        {
            List<EquipoEntidad> lista = new List<EquipoEntidad>();
            string consulta = @"SELECT idEquipoInfo,memoriaTotal,memoriaDisponible,memoriaUsada,porcentajeUsoRam,porcentajeCpu,velocidadCpu,procesosCpu,fechaRegistro,ipEquipo FROM equipoInfo WHERE codSala=" + codsala + " and convert(date,fechaRegistro) between @p1 and @p2";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new EquipoEntidad
                            {
                                IdEquipoInfo = ManejoNulos.ManageNullInteger(dr["idEquipoInfo"]),
                                MemoriaTotal = ManejoNulos.ManageNullStr(dr["memoriaTotal"]),
                                MemoriaDisponible = ManejoNulos.ManageNullStr(dr["memoriaDisponible"]),
                                MemoriaUsada = ManejoNulos.ManageNullStr(dr["memoriaUsada"]),
                                PorcentajeUsoRam = ManejoNulos.ManageNullStr(dr["porcentajeUsoRam"]),
                                PorcentajeCpu = ManejoNulos.ManageNullStr(dr["porcentajeCpu"]),
                                VelocidadCpu = ManejoNulos.ManageNullStr(dr["velocidadCpu"]),
                                ProcesosCpu = ManejoNulos.ManageNullInteger(dr["procesosCpu"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["fechaRegistro"]),
                                IpEquipo = ManejoNulos.ManageNullStr(dr["ipEquipo"]),
                            };
                            lista.Add(item);
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
        public int GuardarEquipoInfo(EquipoEntidad equipo)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO equipoInfo( memoriaTotal,memoriaDisponible,memoriaUsada,porcentajeUsoRam,porcentajeCpu, velocidadCpu, procesosCpu, codSala,fechaRegistro,ipEquipo)
Output Inserted.idEquipoInfo
VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7,@p8,@p9)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(equipo.MemoriaTotal));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(equipo.MemoriaDisponible));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(equipo.MemoriaUsada));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(equipo.PorcentajeUsoRam));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(equipo.PorcentajeCpu));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(equipo.VelocidadCpu));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(equipo.ProcesosCpu));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(equipo.CodSala));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullDate(equipo.FechaRegistro));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(equipo.IpEquipo));
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

        public EquipoEntidad UltimoRegistroEquipo(int codSala)
        {
            EquipoEntidad equipo = new EquipoEntidad();
            string consulta = @"SELECT TOP 1 idEquipoInfo,memoriaTotal,memoriaDisponible,memoriaUsada,porcentajeUsoRam,porcentajeCpu,velocidadCpu,procesosCpu,fechaRegistro from equipoInfo where codsala =" + codSala + "ORDER BY idEquipoInfo Desc";


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

                            equipo.IdEquipoInfo = ManejoNulos.ManageNullInteger(dr["idEquipoInfo"]);
                            equipo.MemoriaTotal = ManejoNulos.ManageNullStr(dr["memoriaTotal"]);
                            equipo.MemoriaDisponible = ManejoNulos.ManageNullStr(dr["memoriaDisponible"]);
                            equipo.MemoriaUsada = ManejoNulos.ManageNullStr(dr["memoriaUsada"]);
                            equipo.PorcentajeUsoRam = ManejoNulos.ManageNullStr(dr["porcentajeUsoRam"]);
                            equipo.PorcentajeCpu = ManejoNulos.ManageNullStr(dr["porcentajeCpu"]);
                            equipo.VelocidadCpu = ManejoNulos.ManageNullStr(dr["velocidadCpu"]);
                            equipo.ProcesosCpu = ManejoNulos.ManageNullInteger(dr["procesosCpu"]);
                            equipo.FechaRegistro = ManejoNulos.ManageNullDate(dr["fechaRegistro"]);


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
            return equipo;
        }
    }
}
