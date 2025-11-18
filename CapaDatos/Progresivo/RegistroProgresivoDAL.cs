using CapaEntidad.Progresivo;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Progresivo
{
    public class RegistroProgresivoDAL
    {
        private readonly string _conexion = string.Empty;

        public RegistroProgresivoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public long HRPGuardarDetalle(RegistroProgresivoEntidad detalle)
        {
            long insertedId = 0;

            string query = @"
            INSERT INTO HistorialProgresivo_Detalle
            (
                ProgresivoID,
                NroPozos,
                PorCredito,
                BaseOculto,
                FechaIni,
                FechaFin,
                NroJugadores,
                ProgresivoImagenID,
                PagoCaja,
                DuracionPantalla,
                Simbolo,
                Estado,
                FechaIni_desc,
                FechaFin_desc,
                indice,
                Estado_desc,
                ProgresivoImagen_desc,
                RegHistorico,
                ProgresivoImagenNombre,
                ProgresivoIDOnline,
                ProgresivoNombreOnline,
                SalaId,
                FechaRegistro,
                UsuarioId
            )

            OUTPUT INSERTED.Id

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
                @p11,
                @p12,
                @p13,
                @p14,
                @p15,
                @p16,
                @p17,
                @p18,
                @p19,
                @p20,
                @p21,
                @p22,
                @p23,
                @p24
            )
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(detalle.ProgresivoID));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(detalle.NroPozos));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManegeNullBool(detalle.PorCredito));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManegeNullBool(detalle.BaseOculto));
                    command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(detalle.FechaIni));
                    command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(detalle.FechaFin));
                    command.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(detalle.NroJugadores));
                    command.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(detalle.ProgresivoImagenID));
                    command.Parameters.AddWithValue("@p9", ManejoNulos.ManegeNullBool(detalle.PagoCaja));
                    command.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(detalle.DuracionPantalla));
                    command.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(detalle.Simbolo));
                    command.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullInteger(detalle.Estado));
                    command.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullStr(detalle.FechaIni_desc));
                    command.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullStr(detalle.FechaFin_desc));
                    command.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullInteger(detalle.indice));
                    command.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullStr(detalle.Estado_desc));
                    command.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullStr(detalle.ProgresivoImagen_desc));
                    command.Parameters.AddWithValue("@p18", ManejoNulos.ManegeNullBool(detalle.RegHistorico));
                    command.Parameters.AddWithValue("@p19", ManejoNulos.ManageNullStr(detalle.ProgresivoImagenNombre));
                    command.Parameters.AddWithValue("@p20", ManejoNulos.ManageNullInteger(detalle.ProgresivoIDOnline));
                    command.Parameters.AddWithValue("@p21", ManejoNulos.ManageNullStr(detalle.ProgresivoNombreOnline));
                    command.Parameters.AddWithValue("@p22", ManejoNulos.ManageNullInteger(detalle.SalaId));
                    command.Parameters.AddWithValue("@p23", ManejoNulos.ManageNullDate(detalle.FechaRegistro));
                    command.Parameters.AddWithValue("@p24", ManejoNulos.ManageNullInteger(detalle.UsuarioId));

                    insertedId = Convert.ToInt64(command.ExecuteScalar());
                }
            }
            catch (Exception)
            {
                insertedId = 0;
            }

            return insertedId;
        }

        public bool HRPGuardarPozo(RegistroProgresivoPozoEntidad pozo)
        {
            bool inserted = false;

            string query = @"
            INSERT INTO HistorialProgresivo_Pozo
            (
                ProgresivoID,
                DetalleProgresivoID,
                PozoID,
                Actual,
                Anterior,
                ActualOculto,
                AnteriorOculto,
                Fecha,
                TipoPozo,
                Estado,
                MontoMin,
                MontoBase,
                MontoMax,
                IncPozo1,
                IncPozo2,
                MontoOcMin,
                MontoOcMax,
                IncOcPozo1,
                IncOcPozo2,
                Parametro,
                Punto,
                Prob1,
                Prob2,
                Indice,
                EstadoInicial,
                Dificultad,
                RsJugadores,
                RsApuesta,
                Dificultad_desc,
                Estado_desc,
                TrigMin,
                TrigMax,
                TopAct,
                TopAnt,
                TMin,
                TMax,
                CheckPozo,
                CheckPozoActual,
                DetalleId
            )

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
                @p11,
                @p12,
                @p13,
                @p14,
                @p15,
                @p16,
                @p17,
                @p18,
                @p19,
                @p20,
                @p21,
                @p22,
                @p23,
                @p24,
                @p25,
                @p26,
                @p27,
                @p28,
                @p29,
                @p30,
                @p31,
                @p32,
                @p33,
                @p34,
                @p35,
                @p36,
                @p37,
                @p38,
                @p39
            )
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(pozo.ProgresivoID));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(pozo.DetalleProgresivoID));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(pozo.PozoID));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDouble(pozo.Actual));
                    command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDouble(pozo.Anterior));
                    command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDouble(pozo.ActualOculto));
                    command.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDouble(pozo.AnteriorOculto));
                    command.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullDate(pozo.Fecha));
                    command.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(pozo.TipoPozo));
                    command.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(pozo.Estado));
                    command.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullDouble(pozo.MontoMin));
                    command.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullDouble(pozo.MontoBase));
                    command.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullDouble(pozo.MontoMax));
                    command.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullDouble(pozo.IncPozo1));
                    command.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullDouble(pozo.IncPozo2));
                    command.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullDouble(pozo.MontoOcMin));
                    command.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullDouble(pozo.MontoOcMax));
                    command.Parameters.AddWithValue("@p18", ManejoNulos.ManageNullDouble(pozo.IncOcPozo1));
                    command.Parameters.AddWithValue("@p19", ManejoNulos.ManageNullDouble(pozo.IncOcPozo2));
                    command.Parameters.AddWithValue("@p20", ManejoNulos.ManegeNullBool(pozo.Parametro));
                    command.Parameters.AddWithValue("@p21", ManejoNulos.ManageNullDouble(pozo.Punto));
                    command.Parameters.AddWithValue("@p22", ManejoNulos.ManageNullDouble(pozo.Prob1));
                    command.Parameters.AddWithValue("@p23", ManejoNulos.ManageNullDouble(pozo.Prob2));
                    command.Parameters.AddWithValue("@p24", ManejoNulos.ManageNullInteger(pozo.Indice));
                    command.Parameters.AddWithValue("@p25", ManejoNulos.ManageNullInteger(pozo.EstadoInicial));
                    command.Parameters.AddWithValue("@p26", ManejoNulos.ManageNullInteger(pozo.Dificultad));
                    command.Parameters.AddWithValue("@p27", ManejoNulos.ManageNullInteger(pozo.RsJugadores));
                    command.Parameters.AddWithValue("@p28", ManejoNulos.ManageNullInteger(pozo.RsApuesta));
                    command.Parameters.AddWithValue("@p29", ManejoNulos.ManageNullStr(pozo.Dificultad_desc));
                    command.Parameters.AddWithValue("@p30", ManejoNulos.ManageNullStr(pozo.Estado_desc));
                    command.Parameters.AddWithValue("@p31", ManejoNulos.ManageNullDouble(pozo.TrigMin));
                    command.Parameters.AddWithValue("@p32", ManejoNulos.ManageNullDouble(pozo.TrigMax));
                    command.Parameters.AddWithValue("@p33", ManejoNulos.ManageNullDouble(pozo.Top));
                    command.Parameters.AddWithValue("@p34", ManejoNulos.ManageNullDouble(pozo.TopAnt));
                    command.Parameters.AddWithValue("@p35", ManejoNulos.ManageNullStr(pozo.TMin));
                    command.Parameters.AddWithValue("@p36", ManejoNulos.ManageNullStr(pozo.TMax));
                    command.Parameters.AddWithValue("@p37", ManejoNulos.ManegeNullBool(pozo.CheckPozo));
                    command.Parameters.AddWithValue("@p38", ManejoNulos.ManegeNullBool(pozo.CheckPozoActual));
                    command.Parameters.AddWithValue("@p39", ManejoNulos.ManageNullInteger64(pozo.DetalleId));

                    int rowsAffected = Convert.ToInt32(command.ExecuteNonQuery());

                    if (rowsAffected > 0)
                    {
                        inserted = true;
                    }
                }
            }
            catch (Exception)
            {
                inserted = false;
            }

            return inserted;
        }

        public List<RegistroProgresivoEntidad> HRPListarDetalle(int salaId, int progresivoIdOnline, int rows)
        {
            List<RegistroProgresivoEntidad> lista = new List<RegistroProgresivoEntidad>();

            string query = $@"
            SELECT TOP {rows}
	            hprode.Id,
	            hprode.ProgresivoID,
	            hprode.NroPozos,
	            hprode.PorCredito,
	            hprode.BaseOculto,
	            hprode.FechaIni,
	            hprode.FechaFin,
	            hprode.NroJugadores,
	            hprode.ProgresivoImagenID,
	            hprode.PagoCaja,
	            hprode.DuracionPantalla,
	            hprode.Simbolo,
	            hprode.Estado,
	            hprode.FechaIni_desc,
	            hprode.FechaFin_desc,
	            hprode.indice,
	            hprode.Estado_desc,
	            hprode.ProgresivoImagen_desc,
	            hprode.RegHistorico,
	            hprode.ProgresivoImagenNombre,
	            hprode.ProgresivoIDOnline,
	            hprode.ProgresivoNombreOnline,
	            hprode.SalaId,
	            hprode.FechaRegistro,
	            hprode.UsuarioId
            FROM HistorialProgresivo_Detalle hprode WITH (NOLOCK)
            WHERE hprode.SalaId = @w1 AND hprode.ProgresivoIDOnline = @w2
            ORDER BY hprode.FechaRegistro DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", salaId);
                    command.Parameters.AddWithValue("@w2", progresivoIdOnline);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            RegistroProgresivoEntidad registroProgresivo = new RegistroProgresivoEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger64(data["Id"]),
                                ProgresivoID = ManejoNulos.ManageNullInteger(data["ProgresivoID"]),
                                NroPozos = ManejoNulos.ManageNullInteger(data["NroPozos"]),
                                PorCredito = ManejoNulos.ManegeNullBool(data["PorCredito"]),
                                BaseOculto = ManejoNulos.ManegeNullBool(data["BaseOculto"]),
                                FechaIni = ManejoNulos.ManageNullDate(data["FechaIni"]),
                                FechaFin = ManejoNulos.ManageNullDate(data["FechaFin"]),
                                NroJugadores = ManejoNulos.ManageNullInteger(data["NroJugadores"]),
                                ProgresivoImagenID = ManejoNulos.ManageNullInteger(data["ProgresivoImagenID"]),
                                PagoCaja = ManejoNulos.ManegeNullBool(data["PagoCaja"]),
                                DuracionPantalla = ManejoNulos.ManageNullInteger(data["DuracionPantalla"]),
                                Simbolo = ManejoNulos.ManageNullStr(data["Simbolo"]),
                                Estado = ManejoNulos.ManageNullInteger(data["Estado"]),
                                FechaIni_desc = ManejoNulos.ManageNullStr(data["FechaIni_desc"]),
                                FechaFin_desc = ManejoNulos.ManageNullStr(data["FechaFin_desc"]),
                                indice = ManejoNulos.ManageNullInteger(data["indice"]),
                                Estado_desc = ManejoNulos.ManageNullStr(data["Estado_desc"]),
                                ProgresivoImagen_desc = ManejoNulos.ManageNullStr(data["ProgresivoImagen_desc"]),
                                RegHistorico = ManejoNulos.ManegeNullBool(data["RegHistorico"]),
                                ProgresivoImagenNombre = ManejoNulos.ManageNullStr(data["ProgresivoImagenNombre"]),
                                ProgresivoIDOnline = ManejoNulos.ManageNullInteger(data["ProgresivoIDOnline"]),
                                ProgresivoNombreOnline = ManejoNulos.ManageNullStr(data["ProgresivoNombreOnline"]),
                                SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]),
                                UsuarioId = ManejoNulos.ManageNullInteger(data["UsuarioId"])
                            };

                            lista.Add(registroProgresivo);
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<RegistroProgresivoEntidad>();
            }

            return lista;
        }

        public RegistroProgresivoEntidad HRPObtenerDetalle(int salaId, long detalleId)
        {
            RegistroProgresivoEntidad registro = new RegistroProgresivoEntidad();

            string query = @"
            SELECT
	            hprode.Id,
	            hprode.ProgresivoID,
	            hprode.NroPozos,
	            hprode.PorCredito,
	            hprode.BaseOculto,
	            hprode.FechaIni,
	            hprode.FechaFin,
	            hprode.NroJugadores,
	            hprode.ProgresivoImagenID,
	            hprode.PagoCaja,
	            hprode.DuracionPantalla,
	            hprode.Simbolo,
	            hprode.Estado,
	            hprode.FechaIni_desc,
	            hprode.FechaFin_desc,
	            hprode.indice,
	            hprode.Estado_desc,
	            hprode.ProgresivoImagen_desc,
	            hprode.RegHistorico,
	            hprode.ProgresivoImagenNombre,
	            hprode.ProgresivoIDOnline,
	            hprode.ProgresivoNombreOnline,
	            hprode.SalaId,
	            hprode.FechaRegistro,
	            hprode.UsuarioId
            FROM HistorialProgresivo_Detalle hprode WITH (NOLOCK)
            WHERE hprode.SalaId = @w1 AND hprode.Id = @w2
            ORDER BY hprode.FechaRegistro DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", salaId);
                    command.Parameters.AddWithValue("@w2", detalleId);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.Read())
                        {
                            registro.Id = ManejoNulos.ManageNullInteger64(data["Id"]);
                            registro.ProgresivoID = ManejoNulos.ManageNullInteger(data["ProgresivoID"]);
                            registro.NroPozos = ManejoNulos.ManageNullInteger(data["NroPozos"]);
                            registro.PorCredito = ManejoNulos.ManegeNullBool(data["PorCredito"]);
                            registro.BaseOculto = ManejoNulos.ManegeNullBool(data["BaseOculto"]);
                            registro.FechaIni = ManejoNulos.ManageNullDate(data["FechaIni"]);
                            registro.FechaFin = ManejoNulos.ManageNullDate(data["FechaFin"]);
                            registro.NroJugadores = ManejoNulos.ManageNullInteger(data["NroJugadores"]);
                            registro.ProgresivoImagenID = ManejoNulos.ManageNullInteger(data["ProgresivoImagenID"]);
                            registro.PagoCaja = ManejoNulos.ManegeNullBool(data["PagoCaja"]);
                            registro.DuracionPantalla = ManejoNulos.ManageNullInteger(data["DuracionPantalla"]);
                            registro.Simbolo = ManejoNulos.ManageNullStr(data["Simbolo"]);
                            registro.Estado = ManejoNulos.ManageNullInteger(data["Estado"]);
                            registro.FechaIni_desc = ManejoNulos.ManageNullStr(data["FechaIni_desc"]);
                            registro.FechaFin_desc = ManejoNulos.ManageNullStr(data["FechaFin_desc"]);
                            registro.indice = ManejoNulos.ManageNullInteger(data["indice"]);
                            registro.Estado_desc = ManejoNulos.ManageNullStr(data["Estado_desc"]);
                            registro.ProgresivoImagen_desc = ManejoNulos.ManageNullStr(data["ProgresivoImagen_desc"]);
                            registro.RegHistorico = ManejoNulos.ManegeNullBool(data["RegHistorico"]);
                            registro.ProgresivoImagenNombre = ManejoNulos.ManageNullStr(data["ProgresivoImagenNombre"]);
                            registro.ProgresivoIDOnline = ManejoNulos.ManageNullInteger(data["ProgresivoIDOnline"]);
                            registro.ProgresivoNombreOnline = ManejoNulos.ManageNullStr(data["ProgresivoNombreOnline"]);
                            registro.SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]);
                            registro.FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]);
                            registro.UsuarioId = ManejoNulos.ManageNullInteger(data["UsuarioId"]);
                        }
                    }
                }
            }
            catch (Exception)
            {
                registro = new RegistroProgresivoEntidad();
            }

            return registro;
        }

        public List<RegistroProgresivoPozoEntidad> HRPListarPozo(long detalleId)
        {
            List<RegistroProgresivoPozoEntidad> lista = new List<RegistroProgresivoPozoEntidad>();

            string query = @"
            SELECT
	            hpropo.ProgresivoID,
	            hpropo.DetalleProgresivoID,
	            hpropo.PozoID,
	            hpropo.Actual,
	            hpropo.Anterior,
	            hpropo.ActualOculto,
	            hpropo.AnteriorOculto,
	            hpropo.Fecha,
	            hpropo.TipoPozo,
	            hpropo.Estado,
	            hpropo.MontoMin,
	            hpropo.MontoBase,
	            hpropo.MontoMax,
	            hpropo.IncPozo1,
	            hpropo.IncPozo2,
	            hpropo.MontoOcMin,
	            hpropo.MontoOcMax,
	            hpropo.IncOcPozo1,
	            hpropo.IncOcPozo2,
	            hpropo.Parametro,
	            hpropo.Punto,
	            hpropo.Prob1,
	            hpropo.Prob2,
	            hpropo.Indice,
	            hpropo.EstadoInicial,
	            hpropo.Dificultad,
	            hpropo.RsJugadores,
	            hpropo.RsApuesta,
	            hpropo.Dificultad_desc,
	            hpropo.Estado_desc,
	            hpropo.TrigMin,
	            hpropo.TrigMax,
	            hpropo.TopAct,
	            hpropo.TopAnt,
	            hpropo.TMin,
	            hpropo.TMax,
	            hpropo.CheckPozo,
	            hpropo.CheckPozoActual,
	            hpropo.DetalleId
            FROM HistorialProgresivo_Pozo hpropo WITH (NOLOCK)
            WHERE hpropo.DetalleId = @w1
            ORDER BY hpropo.TipoPozo ASC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", detalleId);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            RegistroProgresivoPozoEntidad registroProgresivoPozo = new RegistroProgresivoPozoEntidad
                            {
                                ProgresivoID = ManejoNulos.ManageNullInteger(data["ProgresivoID"]),
                                DetalleProgresivoID = ManejoNulos.ManageNullInteger(data["DetalleProgresivoID"]),
                                PozoID = ManejoNulos.ManageNullInteger(data["PozoID"]),
                                Actual = ManejoNulos.ManageNullDouble(data["Actual"]),
                                Anterior = ManejoNulos.ManageNullDouble(data["Anterior"]),
                                ActualOculto = ManejoNulos.ManageNullDouble(data["ActualOculto"]),
                                AnteriorOculto = ManejoNulos.ManageNullDouble(data["AnteriorOculto"]),
                                Fecha = ManejoNulos.ManageNullDate(data["Fecha"]),
                                TipoPozo = ManejoNulos.ManageNullInteger(data["TipoPozo"]),
                                Estado = ManejoNulos.ManageNullInteger(data["Estado"]),
                                MontoMin = ManejoNulos.ManageNullDouble(data["MontoMin"]),
                                MontoBase = ManejoNulos.ManageNullDouble(data["MontoBase"]),
                                MontoMax = ManejoNulos.ManageNullInteger(data["MontoMax"]),
                                IncPozo1 = ManejoNulos.ManageNullDouble(data["IncPozo1"]),
                                IncPozo2 = ManejoNulos.ManageNullDouble(data["IncPozo2"]),
                                MontoOcMin = ManejoNulos.ManageNullDouble(data["MontoOcMin"]),
                                MontoOcMax = ManejoNulos.ManageNullDouble(data["MontoOcMax"]),
                                IncOcPozo1 = ManejoNulos.ManageNullDouble(data["IncOcPozo1"]),
                                IncOcPozo2 = ManejoNulos.ManageNullDouble(data["IncOcPozo2"]),
                                Parametro = ManejoNulos.ManegeNullBool(data["Parametro"]),
                                Punto = ManejoNulos.ManageNullDouble(data["Punto"]),
                                Prob1 = ManejoNulos.ManageNullDouble(data["Prob1"]),
                                Prob2 = ManejoNulos.ManageNullDouble(data["Prob2"]),
                                Indice = ManejoNulos.ManageNullInteger(data["Indice"]),
                                EstadoInicial = ManejoNulos.ManageNullInteger(data["EstadoInicial"]),
                                Dificultad = ManejoNulos.ManageNullInteger(data["Dificultad"]),
                                RsJugadores = ManejoNulos.ManageNullInteger(data["RsJugadores"]),
                                RsApuesta = ManejoNulos.ManageNullInteger(data["RsApuesta"]),
                                Dificultad_desc = ManejoNulos.ManageNullStr(data["Dificultad_desc"]),
                                Estado_desc = ManejoNulos.ManageNullStr(data["Estado_desc"]),
                                TrigMin = ManejoNulos.ManageNullDouble(data["TrigMin"]),
                                TrigMax = ManejoNulos.ManageNullDouble(data["TrigMax"]),
                                Top = ManejoNulos.ManageNullDouble(data["TopAct"]),
                                TopAnt = ManejoNulos.ManageNullDouble(data["TopAnt"]),
                                TMin = ManejoNulos.ManageNullStr(data["TMin"]),
                                TMax = ManejoNulos.ManageNullStr(data["TMax"]),
                                CheckPozo = ManejoNulos.ManegeNullBool(data["CheckPozo"]),
                                CheckPozoActual = ManejoNulos.ManegeNullBool(data["CheckPozoActual"]),
                                DetalleId = ManejoNulos.ManageNullInteger64(data["DetalleId"])
                            };

                            lista.Add(registroProgresivoPozo);
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<RegistroProgresivoPozoEntidad>();
            }

            return lista;
        }

        public RegistroProgresivoEntidad HRPObtenerUltimoDetalle(int salaId, int progresivoIdOnline)
        {
            RegistroProgresivoEntidad registro = new RegistroProgresivoEntidad();

            string query = @"
            SELECT TOP 1
	            hprode.Id,
	            hprode.ProgresivoID,
	            hprode.NroPozos,
	            hprode.PorCredito,
	            hprode.BaseOculto,
	            hprode.FechaIni,
	            hprode.FechaFin,
	            hprode.NroJugadores,
	            hprode.ProgresivoImagenID,
	            hprode.PagoCaja,
	            hprode.DuracionPantalla,
	            hprode.Simbolo,
	            hprode.Estado,
	            hprode.FechaIni_desc,
	            hprode.FechaFin_desc,
	            hprode.indice,
	            hprode.Estado_desc,
	            hprode.ProgresivoImagen_desc,
	            hprode.RegHistorico,
	            hprode.ProgresivoImagenNombre,
	            hprode.ProgresivoIDOnline,
	            hprode.ProgresivoNombreOnline,
	            hprode.SalaId,
	            hprode.FechaRegistro,
	            hprode.UsuarioId
            FROM HistorialProgresivo_Detalle hprode WITH (NOLOCK)
            WHERE hprode.SalaId = @w1 AND hprode.ProgresivoIDOnline = @w2
            ORDER BY hprode.FechaRegistro DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", salaId);
                    command.Parameters.AddWithValue("@w2", progresivoIdOnline);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.Read())
                        {
                            registro.Id = ManejoNulos.ManageNullInteger64(data["Id"]);
                            registro.ProgresivoID = ManejoNulos.ManageNullInteger(data["ProgresivoID"]);
                            registro.NroPozos = ManejoNulos.ManageNullInteger(data["NroPozos"]);
                            registro.PorCredito = ManejoNulos.ManegeNullBool(data["PorCredito"]);
                            registro.BaseOculto = ManejoNulos.ManegeNullBool(data["BaseOculto"]);
                            registro.FechaIni = ManejoNulos.ManageNullDate(data["FechaIni"]);
                            registro.FechaFin = ManejoNulos.ManageNullDate(data["FechaFin"]);
                            registro.NroJugadores = ManejoNulos.ManageNullInteger(data["NroJugadores"]);
                            registro.ProgresivoImagenID = ManejoNulos.ManageNullInteger(data["ProgresivoImagenID"]);
                            registro.PagoCaja = ManejoNulos.ManegeNullBool(data["PagoCaja"]);
                            registro.DuracionPantalla = ManejoNulos.ManageNullInteger(data["DuracionPantalla"]);
                            registro.Simbolo = ManejoNulos.ManageNullStr(data["Simbolo"]);
                            registro.Estado = ManejoNulos.ManageNullInteger(data["Estado"]);
                            registro.FechaIni_desc = ManejoNulos.ManageNullStr(data["FechaIni_desc"]);
                            registro.FechaFin_desc = ManejoNulos.ManageNullStr(data["FechaFin_desc"]);
                            registro.indice = ManejoNulos.ManageNullInteger(data["indice"]);
                            registro.Estado_desc = ManejoNulos.ManageNullStr(data["Estado_desc"]);
                            registro.ProgresivoImagen_desc = ManejoNulos.ManageNullStr(data["ProgresivoImagen_desc"]);
                            registro.RegHistorico = ManejoNulos.ManegeNullBool(data["RegHistorico"]);
                            registro.ProgresivoImagenNombre = ManejoNulos.ManageNullStr(data["ProgresivoImagenNombre"]);
                            registro.ProgresivoIDOnline = ManejoNulos.ManageNullInteger(data["ProgresivoIDOnline"]);
                            registro.ProgresivoNombreOnline = ManejoNulos.ManageNullStr(data["ProgresivoNombreOnline"]);
                            registro.SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]);
                            registro.FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]);
                            registro.UsuarioId = ManejoNulos.ManageNullInteger(data["UsuarioId"]);
                        }
                    }
                }
            }
            catch (Exception)
            {
                registro = new RegistroProgresivoEntidad();
            }

            return registro;
        }
    }
}
