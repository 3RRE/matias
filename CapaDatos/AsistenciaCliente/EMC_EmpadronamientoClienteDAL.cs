using CapaEntidad.AsistenciaCliente;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.AsistenciaCliente
{
    public class EMC_EmpadronamientoClienteDAL
    {
        string _conexion = string.Empty;
        public EMC_EmpadronamientoClienteDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<EMC_EmpadronamientoClienteEntidad> GetListadoEmpadronamientoCliente(DateTime fechaIni, DateTime fechaFin, int SalaId)
        {
            List<EMC_EmpadronamientoClienteEntidad> lista = new List<EMC_EmpadronamientoClienteEntidad>();
            string consulta = @"SELECT  epr.[id]
      , epr.[cliente_id]
        ,epr.observacion
        ,epr.reniec
        ,epr.entrega_dni
	  ,cli.ApelPat
	  ,cli.ApelMat
	  ,cli.Nombre
	  ,cli.NombreCompleto
  ,cli.NroDoc
        ,epr.cod_sala
	  ,epr.fecha
      ,sa.Nombre salanombre
,epr.usuario_id
,seg.UsuarioNombre
  FROM [EMC_EmpadronamientoCliente] epr
  left join AST_Cliente cli on cli.Id=epr.cliente_id
  left join Sala sa on sa.CodSala=epr.cod_sala 
  left join SEG_Usuario seg on seg.UsuarioID=epr.usuario_id
where epr.cod_sala = @p3 
and CONVERT(date, epr.fecha) between CONVERT(date, @p1) and CONVERT(date, @p2) order by epr.[cliente_id] desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    query.Parameters.AddWithValue("@p3", SalaId);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var asistenciaCliente = new EMC_EmpadronamientoClienteEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(dr["cliente_id"]),
                                fecha = ManejoNulos.ManageNullDate(dr["fecha"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["salanombre"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                observacion = ManejoNulos.ManageNullStr(dr["observacion"]),
                                entrega_dni = ManejoNulos.ManegeNullBool(dr["entrega_dni"]),
                                reniec = ManejoNulos.ManegeNullBool(dr["reniec"])
                            };
                            lista.Add(asistenciaCliente);
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

        public EMC_EmpadronamientoClienteEntidad GetEmpadronamientoCliente(DateTime fechaIni,string nrodoc)
        {
            EMC_EmpadronamientoClienteEntidad registro = new EMC_EmpadronamientoClienteEntidad();
            string consulta = @"SELECT  epr.[id]
      , epr.[cliente_id]
	  ,cli.ApelPat
	  ,cli.ApelMat
	  ,cli.Nombre
	  ,cli.NombreCompleto
      ,cli.NroDoc
      ,epr.cod_sala
	  ,epr.fecha

  FROM [EMC_EmpadronamientoCliente] epr
  left join AST_Cliente cli on cli.Id=epr.cliente_id
where  CONVERT(date, epr.fecha) = CONVERT(date, @p1) and cli.NroDoc=@p2";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", nrodoc);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            registro.id = ManejoNulos.ManageNullInteger64(dr["id"]);
                            registro.cliente_id = ManejoNulos.ManageNullInteger64(dr["cliente_id"]);
                            registro.fecha = ManejoNulos.ManageNullDate(dr["fecha"]);
                            registro.ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]);
                            registro.ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]);
                            registro.NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]);
                            registro.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                            registro.NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]);
                            registro.cod_sala= ManejoNulos.ManageNullInteger(dr["cod_sala"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return registro;
        }

        public bool GuardarEmpadronamientoCliente(EMC_EmpadronamientoClienteEntidad cliente)
        {
            bool respuesta = false;
            string consulta = @"
INSERT INTO [dbo].[EMC_EmpadronamientoCliente]
           ([cliente_id]
           ,[cod_sala]
           ,[fecha]
           ,[usuario_id]
           ,[apuestaImportante]
           ,[codMaquina]
           ,[tipocliente_id]
           ,[tipofrecuencia_id]
           ,[tipojuego_id]
           ,[observacion]
           ,[entrega_dni]
           ,[reniec])
     VALUES
           (@p0
           ,@p1
           ,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11);";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger64(cliente.cliente_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger64(cliente.cod_sala));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(cliente.fecha));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger64(cliente.usuario_id));

                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDecimal(cliente.apuestaImportante));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(cliente.codMaquina));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(cliente.tipocliente_id));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(cliente.tipofrecuencia_id));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(cliente.tipojuego_id));



                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(cliente.observacion));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManegeNullBool(cliente.entrega_dni));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManegeNullBool(cliente.reniec));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }


        public bool GuardarEmpadronamientoClienteMobil(EMC_EmpadronamientoClienteEntidad empadronamiento)
        {
            bool respuesta = false;
            string consulta = @"
INSERT INTO dbo.EMC_EmpadronamientoCliente
            (
                cliente_id,
                cod_sala,
                fecha,
                usuario_id,
                apuestaImportante,
                observacion,
                entrega_dni,
                reniec,
                zona_id_in,
                estado,
                registro_entrada
            )

            output INSERTED.ID

            VALUES
            (
                @p1,
                @p2,
                @p3,
                @p4,
                @p5,
                @p6,
                @p7,
                @p8,
                @p9,
                @p10,
                @p11
            );";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var command = new SqlCommand(consulta, con);
                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger64(empadronamiento.cliente_id));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(empadronamiento.cod_sala));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(empadronamiento.fecha));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger64(empadronamiento.usuario_id));
                    command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDecimal(empadronamiento.apuestaImportante));
                    command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(empadronamiento.observacion));
                    command.Parameters.AddWithValue("@p7", ManejoNulos.ManegeNullBool(empadronamiento.entrega_dni));
                    command.Parameters.AddWithValue("@p8", ManejoNulos.ManegeNullBool(empadronamiento.reniec));
                    command.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(empadronamiento.ZonaIdIn));
                    command.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(empadronamiento.Estado));
                    command.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(empadronamiento.RegistroEntrada));
                    command.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public bool EliminarEmpadronamientoCliente(Int64 id)
        {

            string consulta = @"DELETE FROM [dbo].[EMC_EmpadronamientoCliente]
                                WHERE id=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);

                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // Empadronamiento Cliente
        public List<EMC_EmpadronamientoClienteEntidad> ListarEmpadronamientoCliente(int roomId, DateTime fromDate, DateTime toDate)
        {
            List<EMC_EmpadronamientoClienteEntidad> lista = new List<EMC_EmpadronamientoClienteEntidad>();

            string query = @"
            SELECT
	            empdro.id,
                empdro.cliente_id,
                empdro.cod_sala,
                empdro.fecha,
                empdro.usuario_id,
                empdro.apuestaImportante,
                empdro.codMaquina,
                empdro.tipocliente_id,
                empdro.tipofrecuencia_id,
                empdro.tipojuego_id,
                empdro.observacion,
                empdro.entrega_dni,
                empdro.reniec,
                empdro.zona_id_in,
                empdro.zona_id_out,
                empdro.fecha_salida,
                empdro.estado,
	            cliente.NroDoc AS ClienteNroDoc,
	            cliente.NombreCompleto AS ClienteNombreCompleto,
	            sala.Nombre AS SalaNombre,
	            zonaIn.Nombre AS ZonaNombreIn,
	            zonaOut.Nombre AS ZonaNombreOut,
	            usuario.UsuarioNombre AS UsuarioNombre
            FROM dbo.EMC_EmpadronamientoCliente empdro
            LEFT JOIN AST_Cliente cliente ON cliente.Id = empdro.cliente_id
            LEFT JOIN Sala sala ON sala.CodSala = empdro.cod_sala
            LEFT JOIN SL_Zona zonaIn ON zonaIn.Id = empdro.zona_id_in
            LEFT JOIN SL_Zona zonaOut ON zonaOut.Id = empdro.zona_id_out
            LEFT JOIN SEG_Usuario usuario ON usuario.UsuarioID = empdro.usuario_id
            WHERE empdro.cod_sala = @w1 AND CONVERT(date, empdro.fecha) BETWEEN CONVERT(date, @w2) AND CONVERT(date, @w3)
            ORDER BY empdro.fecha DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", roomId);
                    command.Parameters.AddWithValue("@w2", fromDate);
                    command.Parameters.AddWithValue("@w3", toDate);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            EMC_EmpadronamientoClienteEntidad empadronamientoCliente = new EMC_EmpadronamientoClienteEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(data["id"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(data["cliente_id"]),
                                cod_sala = ManejoNulos.ManageNullInteger(data["cod_sala"]),
                                fecha = ManejoNulos.ManageNullDate(data["fecha"]),
                                usuario_id = ManejoNulos.ManageNullInteger64(data["usuario_id"]),
                                apuestaImportante = ManejoNulos.ManageNullFloat(data["apuestaImportante"]),
                                codMaquina = ManejoNulos.ManageNullStr(data["codMaquina"]),
                                tipocliente_id = ManejoNulos.ManageNullInteger(data["tipocliente_id"]),
                                tipofrecuencia_id = ManejoNulos.ManageNullInteger(data["tipofrecuencia_id"]),
                                tipojuego_id = ManejoNulos.ManageNullInteger(data["tipojuego_id"]),
                                observacion = ManejoNulos.ManageNullStr(data["observacion"]),
                                entrega_dni = ManejoNulos.ManegeNullBool(data["entrega_dni"]),
                                reniec = ManejoNulos.ManegeNullBool(data["reniec"]),
                                ZonaIdIn = ManejoNulos.ManageNullInteger(data["zona_id_in"]),
                                ZonaIdOut = ManejoNulos.ManageNullInteger(data["zona_id_out"]),
                                FechaSalida = ManejoNulos.ManageNullDate(data["fecha_salida"]),
                                Estado = ManejoNulos.ManageNullInteger(data["estado"]),
                                NroDoc = ManejoNulos.ManageNullStr(data["ClienteNroDoc"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(data["ClienteNombreCompleto"]),
                                NombreSala = ManejoNulos.ManageNullStr(data["SalaNombre"]),
                                ZonaNombreIn = ManejoNulos.ManageNullStr(data["ZonaNombreIn"]),
                                ZonaNombreOut = ManejoNulos.ManageNullStr(data["ZonaNombreOut"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(data["UsuarioNombre"])
                            };

                            lista.Add(empadronamientoCliente);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return lista;
        }

        public EMC_EmpadronamientoClienteEntidad ObtenerEmpadronamientoCliente(int customerId, DateTime todayDate)
        {
            EMC_EmpadronamientoClienteEntidad empadronamiento = new EMC_EmpadronamientoClienteEntidad();

            string query = @"
            SELECT
	            empdro.id,
	            empdro.cliente_id,
                empdro.cod_sala,
	            empdro.fecha,
	            empdro.usuario_id,
	            empdro.apuestaImportante,
	            empdro.codMaquina,
	            empdro.tipocliente_id,
	            empdro.tipofrecuencia_id,
	            empdro.tipojuego_id,
	            empdro.observacion,
	            empdro.entrega_dni,
	            empdro.reniec,
	            empdro.zona_id_in,
	            empdro.zona_id_out,
	            empdro.fecha_salida,
                empdro.estado,
	            cliente.NroDoc AS ClienteNroDoc,
	            cliente.NombreCompleto AS ClienteNombreCompleto,
	            sala.Nombre AS SalaNombre,
	            zonaIn.Nombre AS ZonaNombreIn,
	            zonaOut.Nombre AS ZonaNombreOut,
	            usuario.UsuarioNombre AS UsuarioNombre
            FROM dbo.EMC_EmpadronamientoCliente empdro
            LEFT JOIN AST_Cliente cliente ON cliente.Id = empdro.cliente_id
            LEFT JOIN Sala sala ON sala.CodSala = empdro.cod_sala
            LEFT JOIN SL_Zona zonaIn ON zonaIn.Id = empdro.zona_id_in
            LEFT JOIN SL_Zona zonaOut ON zonaOut.Id = empdro.zona_id_out
            LEFT JOIN SEG_Usuario usuario ON usuario.UsuarioID = empdro.usuario_id
            WHERE empdro.cliente_id = @w1 AND CONVERT(DATE, empdro.fecha) = CONVERT(DATE, @w2)
            ORDER BY empdro.fecha DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", customerId);
                    command.Parameters.AddWithValue("@w2", todayDate);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.Read())
                        {
                            empadronamiento.id = ManejoNulos.ManageNullInteger64(data["id"]);
                            empadronamiento.cliente_id = ManejoNulos.ManageNullInteger64(data["cliente_id"]);
                            empadronamiento.cod_sala = ManejoNulos.ManageNullInteger(data["cod_sala"]);
                            empadronamiento.fecha = ManejoNulos.ManageNullDate(data["fecha"]);
                            empadronamiento.usuario_id = ManejoNulos.ManageNullInteger64(data["usuario_id"]);
                            empadronamiento.apuestaImportante = ManejoNulos.ManageNullFloat(data["apuestaImportante"]);
                            empadronamiento.codMaquina = ManejoNulos.ManageNullStr(data["codMaquina"]);
                            empadronamiento.tipocliente_id = ManejoNulos.ManageNullInteger(data["tipocliente_id"]);
                            empadronamiento.tipofrecuencia_id = ManejoNulos.ManageNullInteger(data["tipofrecuencia_id"]);
                            empadronamiento.tipojuego_id = ManejoNulos.ManageNullInteger(data["tipojuego_id"]);
                            empadronamiento.observacion = ManejoNulos.ManageNullStr(data["observacion"]);
                            empadronamiento.entrega_dni = ManejoNulos.ManegeNullBool(data["entrega_dni"]);
                            empadronamiento.reniec = ManejoNulos.ManegeNullBool(data["reniec"]);
                            empadronamiento.ZonaIdIn = ManejoNulos.ManageNullInteger(data["zona_id_in"]);
                            empadronamiento.ZonaIdOut = ManejoNulos.ManageNullInteger(data["zona_id_out"]);
                            empadronamiento.FechaSalida = ManejoNulos.ManageNullDate(data["fecha_salida"]);
                            empadronamiento.Estado = ManejoNulos.ManageNullInteger(data["estado"]);
                            empadronamiento.NroDoc = ManejoNulos.ManageNullStr(data["ClienteNroDoc"]);
                            empadronamiento.NombreCompleto = ManejoNulos.ManageNullStr(data["ClienteNombreCompleto"]);
                            empadronamiento.NombreSala = ManejoNulos.ManageNullStr(data["SalaNombre"]);
                            empadronamiento.ZonaNombreIn = ManejoNulos.ManageNullStr(data["ZonaNombreIn"]);
                            empadronamiento.ZonaNombreOut = ManejoNulos.ManageNullStr(data["ZonaNombreOut"]);
                            empadronamiento.UsuarioNombre = ManejoNulos.ManageNullStr(data["UsuarioNombre"]);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return empadronamiento;
        }

        public EMC_EmpadronamientoClienteEntidad ObtenerEmpadronamientoCliente(long empadronamientoId)
        {
            EMC_EmpadronamientoClienteEntidad empadronamiento = new EMC_EmpadronamientoClienteEntidad();

            string query = @"
            SELECT
	            empdro.id,
	            empdro.cliente_id,
                empdro.cod_sala,
	            empdro.fecha,
	            empdro.usuario_id,
	            empdro.apuestaImportante,
	            empdro.codMaquina,
	            empdro.tipocliente_id,
	            empdro.tipofrecuencia_id,
	            empdro.tipojuego_id,
	            empdro.observacion,
	            empdro.entrega_dni,
	            empdro.reniec,
	            empdro.zona_id_in,
	            empdro.zona_id_out,
	            empdro.fecha_salida,
                empdro.estado,
	            cliente.NroDoc AS ClienteNroDoc,
	            cliente.NombreCompleto AS ClienteNombreCompleto,
	            sala.Nombre AS SalaNombre,
	            zonaIn.Nombre AS ZonaNombreIn,
	            zonaOut.Nombre AS ZonaNombreOut,
	            usuario.UsuarioNombre AS UsuarioNombre
            FROM dbo.EMC_EmpadronamientoCliente empdro
            LEFT JOIN AST_Cliente cliente ON cliente.Id = empdro.cliente_id
            LEFT JOIN Sala sala ON sala.CodSala = empdro.cod_sala
            LEFT JOIN SL_Zona zonaIn ON zonaIn.Id = empdro.zona_id_in
            LEFT JOIN SL_Zona zonaOut ON zonaOut.Id = empdro.zona_id_out
            LEFT JOIN SEG_Usuario usuario ON usuario.UsuarioID = empdro.usuario_id
            WHERE empdro.id = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", empadronamientoId);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.Read())
                        {
                            empadronamiento.id = ManejoNulos.ManageNullInteger64(data["id"]);
                            empadronamiento.cliente_id = ManejoNulos.ManageNullInteger64(data["cliente_id"]);
                            empadronamiento.cod_sala = ManejoNulos.ManageNullInteger(data["cod_sala"]);
                            empadronamiento.fecha = ManejoNulos.ManageNullDate(data["fecha"]);
                            empadronamiento.usuario_id = ManejoNulos.ManageNullInteger64(data["usuario_id"]);
                            empadronamiento.apuestaImportante = ManejoNulos.ManageNullFloat(data["apuestaImportante"]);
                            empadronamiento.codMaquina = ManejoNulos.ManageNullStr(data["codMaquina"]);
                            empadronamiento.tipocliente_id = ManejoNulos.ManageNullInteger(data["tipocliente_id"]);
                            empadronamiento.tipofrecuencia_id = ManejoNulos.ManageNullInteger(data["tipofrecuencia_id"]);
                            empadronamiento.tipojuego_id = ManejoNulos.ManageNullInteger(data["tipojuego_id"]);
                            empadronamiento.observacion = ManejoNulos.ManageNullStr(data["observacion"]);
                            empadronamiento.entrega_dni = ManejoNulos.ManegeNullBool(data["entrega_dni"]);
                            empadronamiento.reniec = ManejoNulos.ManegeNullBool(data["reniec"]);
                            empadronamiento.ZonaIdIn = ManejoNulos.ManageNullInteger(data["zona_id_in"]);
                            empadronamiento.ZonaIdOut = ManejoNulos.ManageNullInteger(data["zona_id_out"]);
                            empadronamiento.FechaSalida = ManejoNulos.ManageNullDate(data["fecha_salida"]);
                            empadronamiento.Estado = ManejoNulos.ManageNullInteger(data["estado"]);
                            empadronamiento.NroDoc = ManejoNulos.ManageNullStr(data["ClienteNroDoc"]);
                            empadronamiento.NombreCompleto = ManejoNulos.ManageNullStr(data["ClienteNombreCompleto"]);
                            empadronamiento.NombreSala = ManejoNulos.ManageNullStr(data["SalaNombre"]);
                            empadronamiento.ZonaNombreIn = ManejoNulos.ManageNullStr(data["ZonaNombreIn"]);
                            empadronamiento.ZonaNombreOut = ManejoNulos.ManageNullStr(data["ZonaNombreOut"]);
                            empadronamiento.UsuarioNombre = ManejoNulos.ManageNullStr(data["UsuarioNombre"]);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return empadronamiento;
        }

        public long GuardarEmpadronamientoClienteV2(EMC_EmpadronamientoClienteEntidad empadronamiento)
        {
            long empadronamientoId = 0;

            string query = @"
            INSERT INTO dbo.EMC_EmpadronamientoCliente
            (
                cliente_id,
                cod_sala,
                fecha,
                usuario_id,
                apuestaImportante,
                observacion,
                entrega_dni,
                reniec,
                zona_id_in,
                estado,
                registro_entrada
            )

            output INSERTED.ID

            VALUES
            (
                @p1,
                @p2,
                @p3,
                @p4,
                @p5,
                @p6,
                @p7,
                @p8,
                @p9,
                @p10,
                @p11
            )
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger64(empadronamiento.cliente_id));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(empadronamiento.cod_sala));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(empadronamiento.fecha));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger64(empadronamiento.usuario_id));
                    command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDecimal(empadronamiento.apuestaImportante));
                    command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(empadronamiento.observacion));
                    command.Parameters.AddWithValue("@p7", ManejoNulos.ManegeNullBool(empadronamiento.entrega_dni));
                    command.Parameters.AddWithValue("@p8", ManejoNulos.ManegeNullBool(empadronamiento.reniec));
                    command.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(empadronamiento.ZonaIdIn));
                    command.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(empadronamiento.Estado));
                    command.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullInteger(empadronamiento.RegistroEntrada));

                    empadronamientoId = Convert.ToInt64(command.ExecuteScalar());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return empadronamientoId;
        }

        public bool RegistrarFechaHoraSalida(EMC_EmpadronamientoClienteEntidad empadronamiento)
        {
            bool response = false;

            string query = @"
            UPDATE dbo.EMC_EmpadronamientoCliente
            SET
                fecha_salida = @p1,
                estado = @p2,
                usuario_id_out = @p3,
                registro_salida = @p4
            WHERE
                id = @w1
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(empadronamiento.FechaSalida));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(empadronamiento.Estado));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(empadronamiento.UsuarioIdOut));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(empadronamiento.RegistroSalida));
                    command.Parameters.AddWithValue("@w1", ManejoNulos.ManageNullInteger64(empadronamiento.id));

                    int rowsAffected = command.ExecuteNonQuery();

                    if(rowsAffected > 0)
                    {
                        response = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return response;
        }
    }
}
