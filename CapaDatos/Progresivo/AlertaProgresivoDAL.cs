using CapaEntidad.Progresivo;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Progresivo
{
    public class AlertaProgresivoDAL
    {
        private readonly string _conexion = string.Empty;

        public AlertaProgresivoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public long GuardarAlertaProgresivo(AlertaProgresivoEntidad alertaProgresivo)
        {
            long insertedId = 0;

            string query = @"
            INSERT INTO PRO_Alerta_Progresivo
            (
                SalaId,
                SalaNombre,
                ProgresivoNombre,
                Descripcion,
                Tipo,
                FechaRegistro
            )

            OUTPUT INSERTED.Id

            VALUES
            (
                @p1,
                @p2,
                @p3,
                @p4,
                @p5,
                @p6
            )
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(alertaProgresivo.SalaId));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(alertaProgresivo.SalaNombre));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(alertaProgresivo.ProgresivoNombre));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(alertaProgresivo.Descripcion));
                    command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(alertaProgresivo.Tipo));
                    command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(alertaProgresivo.FechaRegistro));

                    insertedId = Convert.ToInt64(command.ExecuteScalar());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return insertedId;
        }

        public long GuardarAlertaProgresivoDetalle(AlertaProgresivoDetalleEntidad progresivoDetalle)
        {
            long insertedId = 0;

            string query = @"
            INSERT INTO PRO_Alerta_ProgresivoDetalle
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
                AlertaId,
                ProActual
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
                @p24,
                @p25
            )
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(progresivoDetalle.ProgresivoID));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(progresivoDetalle.NroPozos));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManegeNullBool(progresivoDetalle.PorCredito));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManegeNullBool(progresivoDetalle.BaseOculto));
                    command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(progresivoDetalle.FechaIni));
                    command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(progresivoDetalle.FechaFin));
                    command.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(progresivoDetalle.NroJugadores));
                    command.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(progresivoDetalle.ProgresivoImagenID));
                    command.Parameters.AddWithValue("@p9", ManejoNulos.ManegeNullBool(progresivoDetalle.PagoCaja));
                    command.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(progresivoDetalle.DuracionPantalla));
                    command.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(progresivoDetalle.Simbolo));
                    command.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullInteger(progresivoDetalle.Estado));
                    command.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullStr(progresivoDetalle.FechaIni_desc));
                    command.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullStr(progresivoDetalle.FechaFin_desc));
                    command.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullInteger(progresivoDetalle.indice));
                    command.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullStr(progresivoDetalle.Estado_desc));
                    command.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullStr(progresivoDetalle.ProgresivoImagen_desc));
                    command.Parameters.AddWithValue("@p18", ManejoNulos.ManegeNullBool(progresivoDetalle.RegHistorico));
                    command.Parameters.AddWithValue("@p19", ManejoNulos.ManageNullStr(progresivoDetalle.ProgresivoImagenNombre));
                    command.Parameters.AddWithValue("@p20", ManejoNulos.ManageNullInteger(progresivoDetalle.ProgresivoIDOnline));
                    command.Parameters.AddWithValue("@p21", ManejoNulos.ManageNullStr(progresivoDetalle.ProgresivoNombreOnline));
                    command.Parameters.AddWithValue("@p22", ManejoNulos.ManageNullInteger(progresivoDetalle.SalaId));
                    command.Parameters.AddWithValue("@p23", ManejoNulos.ManageNullDate(progresivoDetalle.FechaRegistro));
                    command.Parameters.AddWithValue("@p24", ManejoNulos.ManageNullInteger64(progresivoDetalle.AlertaId));
                    command.Parameters.AddWithValue("@p25", ManejoNulos.ManegeNullBool(progresivoDetalle.ProActual));

                    insertedId = Convert.ToInt64(command.ExecuteScalar());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return insertedId;
        }

        public bool GuardarAlertaProgresivoPozo(AlertaProgresivoPozoEntidad progresivoPozo)
        {
            bool inserted = false;

            string query = @"
            INSERT INTO PRO_Alerta_ProgresivoPozo
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
                DetalleId,
                AlertaId,
                ProActual
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

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(progresivoPozo.ProgresivoID));
                    command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(progresivoPozo.DetalleProgresivoID));
                    command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(progresivoPozo.PozoID));
                    command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDouble(progresivoPozo.Actual));
                    command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDouble(progresivoPozo.Anterior));
                    command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDouble(progresivoPozo.ActualOculto));
                    command.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDouble(progresivoPozo.AnteriorOculto));
                    command.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullDate(progresivoPozo.Fecha));
                    command.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(progresivoPozo.TipoPozo));
                    command.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(progresivoPozo.Estado));
                    command.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullDouble(progresivoPozo.MontoMin));
                    command.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullDouble(progresivoPozo.MontoBase));
                    command.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullDouble(progresivoPozo.MontoMax));
                    command.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullDouble(progresivoPozo.IncPozo1));
                    command.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullDouble(progresivoPozo.IncPozo2));
                    command.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullDouble(progresivoPozo.MontoOcMin));
                    command.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullDouble(progresivoPozo.MontoOcMax));
                    command.Parameters.AddWithValue("@p18", ManejoNulos.ManageNullDouble(progresivoPozo.IncOcPozo1));
                    command.Parameters.AddWithValue("@p19", ManejoNulos.ManageNullDouble(progresivoPozo.IncOcPozo2));
                    command.Parameters.AddWithValue("@p20", ManejoNulos.ManegeNullBool(progresivoPozo.Parametro));
                    command.Parameters.AddWithValue("@p21", ManejoNulos.ManageNullDouble(progresivoPozo.Punto));
                    command.Parameters.AddWithValue("@p22", ManejoNulos.ManageNullDouble(progresivoPozo.Prob1));
                    command.Parameters.AddWithValue("@p23", ManejoNulos.ManageNullDouble(progresivoPozo.Prob2));
                    command.Parameters.AddWithValue("@p24", ManejoNulos.ManageNullInteger(progresivoPozo.Indice));
                    command.Parameters.AddWithValue("@p25", ManejoNulos.ManageNullInteger(progresivoPozo.EstadoInicial));
                    command.Parameters.AddWithValue("@p26", ManejoNulos.ManageNullInteger(progresivoPozo.Dificultad));
                    command.Parameters.AddWithValue("@p27", ManejoNulos.ManageNullInteger(progresivoPozo.RsJugadores));
                    command.Parameters.AddWithValue("@p28", ManejoNulos.ManageNullInteger(progresivoPozo.RsApuesta));
                    command.Parameters.AddWithValue("@p29", ManejoNulos.ManageNullStr(progresivoPozo.Dificultad_desc));
                    command.Parameters.AddWithValue("@p30", ManejoNulos.ManageNullStr(progresivoPozo.Estado_desc));
                    command.Parameters.AddWithValue("@p31", ManejoNulos.ManageNullDouble(progresivoPozo.TrigMin));
                    command.Parameters.AddWithValue("@p32", ManejoNulos.ManageNullDouble(progresivoPozo.TrigMax));
                    command.Parameters.AddWithValue("@p33", ManejoNulos.ManageNullDouble(progresivoPozo.Top));
                    command.Parameters.AddWithValue("@p34", ManejoNulos.ManageNullDouble(progresivoPozo.TopAnt));
                    command.Parameters.AddWithValue("@p35", ManejoNulos.ManageNullStr(progresivoPozo.TMin));
                    command.Parameters.AddWithValue("@p36", ManejoNulos.ManageNullStr(progresivoPozo.TMax));
                    command.Parameters.AddWithValue("@p37", ManejoNulos.ManageNullInteger64(progresivoPozo.DetalleId));
                    command.Parameters.AddWithValue("@p38", ManejoNulos.ManageNullInteger64(progresivoPozo.AlertaId));
                    command.Parameters.AddWithValue("@p39", ManejoNulos.ManegeNullBool(progresivoPozo.ProActual));

                    int rowsAffected = Convert.ToInt32(command.ExecuteNonQuery());

                    if(rowsAffected > 0)
                    {
                        inserted = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return inserted;
        }

        public List<AlertaProgresivoEntidad> ListarAlertasProgresivoSala(int salaId, DateTime fromDate, DateTime toDate)
        {
            List<AlertaProgresivoEntidad> lista = new List<AlertaProgresivoEntidad>();

            string query = @"
            SELECT
	            apro.Id,
	            apro.SalaId,
                sala.Nombre AS SalaNombre,
	            apro.ProgresivoNombre,
	            apro.Descripcion,
                apro.Tipo,
	            apro.FechaRegistro
            FROM PRO_Alerta_Progresivo apro
            INNER JOIN Sala sala ON sala.CodSala = apro.SalaId
            WHERE apro.SalaId = @w1 AND CONVERT(DATE, apro.FechaRegistro) BETWEEN CONVERT(DATE, @w2) AND CONVERT(DATE, @w3)
            ORDER BY apro.FechaRegistro DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", salaId);
                    command.Parameters.AddWithValue("@w2", fromDate);
                    command.Parameters.AddWithValue("@w3", toDate);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            AlertaProgresivoEntidad alertaProgresivo = new AlertaProgresivoEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger64(data["Id"]),
                                SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]),
                                SalaNombre = ManejoNulos.ManageNullStr(data["SalaNombre"]),
                                ProgresivoNombre = ManejoNulos.ManageNullStr(data["ProgresivoNombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(data["Descripcion"]),
                                Tipo = ManejoNulos.ManageNullInteger(data["Tipo"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"])
                            };

                            lista.Add(alertaProgresivo);
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

        public AlertaProgresivoEntidad ObtenerAlertaProgresivo(long alertaId)
        {
            AlertaProgresivoEntidad alerta = new AlertaProgresivoEntidad();

            string query = @"
            SELECT
	            apro.Id,
	            apro.SalaId,
                sala.Nombre AS SalaNombre,
	            apro.ProgresivoNombre,
	            apro.Descripcion,
                apro.Tipo,
	            apro.FechaRegistro
            FROM PRO_Alerta_Progresivo apro
            INNER JOIN Sala sala ON sala.CodSala = apro.SalaId
            WHERE apro.Id = @w1
            ORDER BY apro.FechaRegistro DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", alertaId);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.Read())
                        {
                            alerta.Id = ManejoNulos.ManageNullInteger64(data["Id"]);
                            alerta.SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]);
                            alerta.SalaNombre = ManejoNulos.ManageNullStr(data["SalaNombre"]);
                            alerta.ProgresivoNombre = ManejoNulos.ManageNullStr(data["ProgresivoNombre"]);
                            alerta.Descripcion = ManejoNulos.ManageNullStr(data["Descripcion"]);
                            alerta.Tipo = ManejoNulos.ManageNullInteger(data["Tipo"]);
                            alerta.FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return alerta;
        }

        public List<AlertaProgresivoDetalleEntidad> ListarAlertaProgresivoDetalles(long alertaId)
        {
            List<AlertaProgresivoDetalleEntidad> lista = new List<AlertaProgresivoDetalleEntidad>();

            string query = @"
            SELECT
	            aprod.Id,
	            aprod.ProgresivoID,
	            aprod.NroPozos,
	            aprod.PorCredito,
	            aprod.BaseOculto,
	            aprod.FechaIni,
	            aprod.FechaFin,
	            aprod.NroJugadores,
	            aprod.ProgresivoImagenID,
	            aprod.PagoCaja,
	            aprod.DuracionPantalla,
	            aprod.Simbolo,
	            aprod.Estado,
	            aprod.FechaIni_desc,
	            aprod.FechaFin_desc,
	            aprod.indice,
	            aprod.Estado_desc,
	            aprod.ProgresivoImagen_desc,
	            aprod.RegHistorico,
	            aprod.ProgresivoImagenNombre,
	            aprod.ProgresivoIDOnline,
	            aprod.ProgresivoNombreOnline,
	            aprod.SalaId,
	            aprod.FechaRegistro,
	            aprod.AlertaId,
	            aprod.ProActual
            FROM PRO_Alerta_ProgresivoDetalle aprod
            WHERE aprod.AlertaId = @w1
            ORDER BY aprod.ProActual DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", alertaId);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            AlertaProgresivoDetalleEntidad alertaProgresivoDetalle = new AlertaProgresivoDetalleEntidad
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
                                AlertaId = ManejoNulos.ManageNullInteger64(data["AlertaId"]),
                                ProActual = ManejoNulos.ManegeNullBool(data["ProActual"])
                            };

                            lista.Add(alertaProgresivoDetalle);
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

        public List<AlertaProgresivoPozoEntidad> ListarAlertaProgresivoPozos(long alertaId, long detalleId)
        {
            List<AlertaProgresivoPozoEntidad> lista = new List<AlertaProgresivoPozoEntidad>();

            string query = @"
            SELECT
	            aprop.ProgresivoID,
	            aprop.DetalleProgresivoID,
	            aprop.PozoID,
	            aprop.Actual,
	            aprop.Anterior,
	            aprop.ActualOculto,
	            aprop.AnteriorOculto,
	            aprop.Fecha,
	            aprop.TipoPozo,
	            aprop.Estado,
	            aprop.MontoMin,
	            aprop.MontoBase,
	            aprop.MontoMax,
	            aprop.IncPozo1,
	            aprop.IncPozo2,
	            aprop.MontoOcMin,
	            aprop.MontoOcMax,
	            aprop.IncOcPozo1,
	            aprop.IncOcPozo2,
	            aprop.Parametro,
	            aprop.Punto,
	            aprop.Prob1,
	            aprop.Prob2,
	            aprop.Indice,
	            aprop.EstadoInicial,
	            aprop.Dificultad,
	            aprop.RsJugadores,
	            aprop.RsApuesta,
	            aprop.Dificultad_desc,
	            aprop.Estado_desc,
	            aprop.TrigMin,
	            aprop.TrigMax,
	            aprop.TopAct,
	            aprop.TopAnt,
	            aprop.TMin,
	            aprop.TMax,
	            aprop.DetalleId,
	            aprop.AlertaId,
	            aprop.ProActual
            FROM PRO_Alerta_ProgresivoPozo aprop
            WHERE aprop.AlertaId = @w1 AND aprop.DetalleId = @w2
            ORDER BY aprop.ProActual DESC, aprop.TipoPozo ASC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", alertaId);
                    command.Parameters.AddWithValue("@w2", detalleId);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            AlertaProgresivoPozoEntidad alertaProgresivoPozo = new AlertaProgresivoPozoEntidad
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
                                DetalleId = ManejoNulos.ManageNullInteger64(data["DetalleId"]),
                                AlertaId = ManejoNulos.ManageNullInteger64(data["AlertaId"]),
                                ProActual = ManejoNulos.ManegeNullBool(data["ProActual"])
                            };

                            lista.Add(alertaProgresivoPozo);
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
		public List<AlertaProgresivoEntidad> ListarAlertasProgresivoSalaYTipo(int[] salas, DateTime fromDate, DateTime toDate, int type)
		{
			List<AlertaProgresivoEntidad> lista = new List<AlertaProgresivoEntidad>();

            string querySalas = string.Empty;
            if(salas.Length > 0)
            {
                querySalas = $@" and apro.SalaId in ({String.Join(",",salas)})";
            }
			string query = $@"
            SELECT
	            apro.Id,
	            apro.SalaId,
	            apro.ProgresivoNombre,
	            apro.Descripcion,
                apro.Tipo,
	            apro.FechaRegistro
            FROM PRO_Alerta_Progresivo apro
            WHERE apro.FechaRegistro BETWEEN @w2 AND @w3
and apro.Tipo = @w4 {querySalas}
ORDER BY apro.FechaRegistro DESC
            ";

			try
			{
				using(SqlConnection connection = new SqlConnection(_conexion))
				{
					connection.Open();

					SqlCommand command = new SqlCommand(query, connection);

					command.Parameters.AddWithValue("@w2", fromDate);
					command.Parameters.AddWithValue("@w3", toDate);
                    command.Parameters.AddWithValue("@w4", type);

					using(SqlDataReader data = command.ExecuteReader())
					{
						while(data.Read())
						{
							AlertaProgresivoEntidad alertaProgresivo = new AlertaProgresivoEntidad
							{
								Id = ManejoNulos.ManageNullInteger64(data["Id"]),
								SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]),
								ProgresivoNombre = ManejoNulos.ManageNullStr(data["ProgresivoNombre"]),
								Descripcion = ManejoNulos.ManageNullStr(data["Descripcion"]),
								Tipo = ManejoNulos.ManageNullInteger(data["Tipo"]),
								FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"])
							};

							lista.Add(alertaProgresivo);
						}
					}
				}
			} catch(Exception exception)
			{
                lista = new List<AlertaProgresivoEntidad>();
			}

			return lista;
		}
	}
}
