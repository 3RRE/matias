using CapaEntidad.Campañas;
using CapaEntidad.TITO;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace CapaDatos.Campaña {
    public class CMP_TicketDAL {
        string _conexion = string.Empty;

        public CMP_TicketDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CMP_TicketEntidad> GetListadoTicketcompleto() {
            List<CMP_TicketEntidad> lista = new List<CMP_TicketEntidad>();
            string consulta = @"SELECT [id]
                              ,[campaña_id]
                              ,[fechareg]
                              ,[item]
                              ,[nroticket]
                              ,[monto]
                              ,[fecharegsala]
                              ,[origen]
                              ,[usuario_id]
                              ,[estado]
                          FROM [dbo].[CMP_Tickets] order by id desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campaña = new CMP_TicketEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]),
                                item = ManejoNulos.ManageNullInteger64(dr["item"]),
                                nroticket = ManejoNulos.ManageNullStr(dr["nroticket"]),
                                monto = ManejoNulos.ManageNullDouble(dr["monto"]),
                                fecharegsala = ManejoNulos.ManageNullDate(dr["fecharegsala"]),
                                origen = ManejoNulos.ManageNullStr(dr["origen"]),
                                usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"])

                            };

                            lista.Add(campaña);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public List<CMP_TicketEntidad> GetListadoTicketCampaña(Int64 campaña_id) {
            List<CMP_TicketEntidad> lista = new List<CMP_TicketEntidad>();

            string consulta = @"SELECT t.[id]
                              ,t.[campaña_id]
                              ,t.[fechareg]
                              ,t.[item]
                              ,t.[nroticket]
                              ,t.[monto]
                              ,t.[fecharegsala]
                              ,t.[origen]
                              ,t.[usuario_id]
                              ,t.[estado]

                              ,t.[Apeclie]
                              ,t.[NomClie]
                              ,t.[Correo]
                              ,t.[Dni]
                              ,t.[FechaNacimiento]
                                ,s.UsuarioNombre
                                ,t.cliente_id
                                ,c.[NroDoc]
                                  ,c.[NombreCompleto]
                                  ,c.[ApelPat]
                                  ,c.[ApelMat]
                                    ,c.[Nombre]
                                  ,c.[Celular1]
                          FROM [dbo].[CMP_Tickets] t 
                                left join AST_Cliente c on c.Id=t.cliente_id
                               left join SEG_Usuario s on s.UsuarioID=usuario_id where t.campaña_id= @p0 
                                order by t.id desc;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", campaña_id);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var campaña = new CMP_TicketEntidad {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                campaña_id = ManejoNulos.ManageNullInteger(dr["campaña_id"]),
                                fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]),
                                item = ManejoNulos.ManageNullInteger64(dr["item"]),
                                nroticket = ManejoNulos.ManageNullStr(dr["nroticket"]),
                                monto = ManejoNulos.ManageNullDouble(dr["monto"]),
                                fecharegsala = ManejoNulos.ManageNullDate(dr["fecharegsala"]),
                                origen = ManejoNulos.ManageNullStr(dr["origen"]),
                                usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                nombre_usuario = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),

                                Apeclie = ManejoNulos.ManageNullStr(dr["Apeclie"]),
                                NomClie = ManejoNulos.ManageNullStr(dr["NomClie"]),
                                Correo = ManejoNulos.ManageNullStr(dr["Correo"]),
                                FechaNacimiento = ManejoNulos.ManageNullStr(dr["FechaNacimiento"]),
                                Dni = ManejoNulos.ManageNullStr(dr["Dni"]),

                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                Celular1 = ManejoNulos.ManageNullStr(dr["Celular1"]),
                                cliente_id = ManejoNulos.ManageNullInteger(dr["cliente_id"])
                            };

                            lista.Add(campaña);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public CMP_TicketEntidad GetTicketID(int id) {
            CMP_TicketEntidad ticket = new CMP_TicketEntidad();
            string consulta = @"SELECT [id]
                              ,[campaña_id]
                              ,[fechareg]
                              ,[item]
                              ,[nroticket]
                              ,[monto]
                              ,[fecharegsala]
                              ,[origen]
                              ,[usuario_id]
                              ,[estado]
                          FROM [dbo].[CMP_Tickets] where id=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                ticket.id = ManejoNulos.ManageNullInteger64(dr["id"]);
                                ticket.fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]);
                                ticket.item = ManejoNulos.ManageNullInteger64(dr["item"]);
                                ticket.nroticket = ManejoNulos.ManageNullStr(dr["nroticket"]);
                                ticket.monto = ManejoNulos.ManageNullDouble(dr["monto"]);
                                ticket.fecharegsala = ManejoNulos.ManageNullDate(dr["fecharegsala"]);
                                ticket.origen = ManejoNulos.ManageNullStr(dr["origen"]);
                                ticket.usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]);
                                ticket.estado = ManejoNulos.ManageNullInteger(dr["estado"]);
                            }
                        }
                    };


                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return ticket;
        }

        public CMP_TicketEntidad GetTicketxnromonto(int item, string nroticket, double monto) {
            CMP_TicketEntidad ticket = new CMP_TicketEntidad();
            string consulta = @"SELECT [id]
                              ,[campaña_id]
                              ,[fechareg]
                              ,[item]
                              ,[nroticket]
                              ,[monto]
                              ,[fecharegsala]
                              ,[origen]
                              ,[usuario_id]
                              ,[estado]
                          FROM [dbo].[CMP_Tickets] where item=@p1 AND nroticket=@p2 and monto=@p3";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", item);
                    query.Parameters.AddWithValue("@p2", nroticket);
                    query.Parameters.AddWithValue("@p3", monto);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                ticket.id = ManejoNulos.ManageNullInteger64(dr["id"]);
                                ticket.fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]);
                                ticket.item = ManejoNulos.ManageNullInteger64(dr["item"]);
                                ticket.nroticket = ManejoNulos.ManageNullStr(dr["nroticket"]);
                                ticket.monto = ManejoNulos.ManageNullDouble(dr["monto"]);
                                ticket.fecharegsala = ManejoNulos.ManageNullDate(dr["fecharegsala"]);
                                ticket.origen = ManejoNulos.ManageNullStr(dr["origen"]);
                                ticket.usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]);
                                ticket.estado = ManejoNulos.ManageNullInteger(dr["estado"]);
                            }
                        }
                    };


                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return ticket;
        }

        public int GuardarTicket(CMP_TicketEntidad ticket) {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
                INSERT INTO [dbo].[CMP_Tickets] (
                    [campaña_id],[fechareg],[item],[nroticket],[monto]
                    ,[fecharegsala],[origen],[usuario_id],[estado],[Apeclie]
                    ,[NomClie],[Correo],[Dni],[FechaNacimiento],cliente_id
                    ,fecha_apertura,SalaOrigen,SalaFisica 
                )
                Output Inserted.id
                VALUES (
                    @p0,@p1,@p2,@p3,@p4
                    ,@p5,@p6,@p7,@p8,@p9
                    ,@p10,@p11,@p12,@p13,@p14
                    ,@p15,@p16,@p17
                );
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger64(ticket.campaña_id) == 0 ? SqlInt32.Null : ticket.campaña_id);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(ticket.fechareg));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger64(ticket.item));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(ticket.nroticket));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDecimal(ticket.monto));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(ticket.fecharegsala));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(ticket.origen));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(ticket.usuario_id));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(ticket.estado));

                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(ticket.Apeclie));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(ticket.NomClie));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(ticket.Correo));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(ticket.Dni));
                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullStr(ticket.FechaNacimiento));
                    query.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullInteger64(ticket.cliente_id));

                    query.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullDate(ticket.FechaApertura));
                    query.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullInteger(ticket.SalaOrigen));
                    query.Parameters.AddWithValue("@p17", ManejoNulos.ManegeNullBool(ticket.SalaFisica));

                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool eliminarTicket(int id) {
            bool respuesta = false;
            string consulta = @"Delete from [dbo].[CMP_Tickets]
                 WHERE id=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        // ATP
        public List<CMP_TicketEntidad> GetTicketsEnlazadosItems(int salaId, List<int> D00H_Items) {
            List<CMP_TicketEntidad> listTickets = new List<CMP_TicketEntidad>();

            string query = $@"
            SELECT
                tickets.id,
                tickets.campaña_id,
                tickets.fechareg,
                tickets.item,
                tickets.nroticket,
                tickets.monto,
                tickets.fecha_apertura,
                tickets.fecharegsala,
                tickets.origen,
                tickets.usuario_id,
                tickets.estado,
                tickets.cliente_id,
                cliente.NombreCompleto,
                cliente.NroDoc,
                cliente.Celular1,
                usuario.UsuarioNombre,
                campania.id AS campania_id,
                campania.nombre AS campania_nombre
            FROM CMP_Tickets tickets
            LEFT JOIN AST_Cliente cliente ON cliente.Id = tickets.cliente_id
            LEFT JOIN SEG_Usuario usuario ON usuario.UsuarioID = tickets.usuario_id
            LEFT JOIN CMP_Campaña campania ON campania.id = tickets.campaña_id
            WHERE tickets.item IN ({string.Join(",", D00H_Items)}) AND campania.sala_id = @w1
            ORDER BY tickets.fechareg DESC
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", salaId);

                    using(SqlDataReader reader = command.ExecuteReader()) {
                        while(reader.Read()) {
                            CMP_TicketEntidad tickets = new CMP_TicketEntidad {
                                id = ManejoNulos.ManageNullInteger64(reader["id"]),
                                campaña_id = ManejoNulos.ManageNullInteger(reader["campaña_id"]),
                                fechareg = ManejoNulos.ManageNullDate(reader["fechareg"]),
                                item = ManejoNulos.ManageNullInteger64(reader["item"]),
                                nroticket = ManejoNulos.ManageNullStr(reader["nroticket"]),
                                monto = ManejoNulos.ManageNullDouble(reader["monto"]),
                                FechaApertura = ManejoNulos.ManageNullDate(reader["fecha_apertura"]),
                                fecharegsala = ManejoNulos.ManageNullDate(reader["fecharegsala"]),
                                origen = ManejoNulos.ManageNullStr(reader["origen"]),
                                usuario_id = ManejoNulos.ManageNullInteger(reader["usuario_id"]),
                                estado = ManejoNulos.ManageNullInteger(reader["estado"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(reader["cliente_id"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(reader["NombreCompleto"]),
                                NroDoc = ManejoNulos.ManageNullStr(reader["NroDoc"]),
                                Celular1 = ManejoNulos.ManageNullStr(reader["Celular1"]),
                                nombre_usuario = ManejoNulos.ManageNullStr(reader["UsuarioNombre"]),

                                Campania = new CMP_CampañaEntidad {
                                    id = ManejoNulos.ManageNullInteger64(reader["campania_id"]),
                                    nombre = ManejoNulos.ManageNullStr(reader["campania_nombre"])
                                }
                            };

                            listTickets.Add(tickets);
                        }
                    }

                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);
            }

            return listTickets;
        }

        public List<CMP_TicketEntidad> GetTicketsEnlazadosItemsV2(int salaId, List<int> D00H_Items)
        {
            List<CMP_TicketEntidad> listTickets = new List<CMP_TicketEntidad>();

            string query = $@"
            SELECT
                tickets.id,
                tickets.campaña_id,
                tickets.fechareg,
                tickets.item,
                tickets.nroticket,
                tickets.monto,
                tickets.fecha_apertura,
                tickets.fecharegsala,
                tickets.origen,
                tickets.usuario_id,
                tickets.estado,
                tickets.cliente_id,
                cliente.NombreCompleto,
                cliente.NroDoc,
                cliente.Celular1,
                usuario.UsuarioNombre,
                campania.id AS campania_id,
                campania.nombre AS campania_nombre
            FROM CMP_Tickets tickets
            LEFT JOIN AST_Cliente cliente ON cliente.Id = tickets.cliente_id
            LEFT JOIN SEG_Usuario usuario ON usuario.UsuarioID = tickets.usuario_id
            LEFT JOIN CMP_Campaña campania ON campania.id = tickets.campaña_id
            WHERE
                tickets.item IN ({string.Join(",", D00H_Items)})
                AND (campania.sala_id = @w1 OR (tickets.SalaOrigen = @w1 AND tickets.SalaFisica = 0))
            ORDER BY
                tickets.fechareg DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@w1", salaId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CMP_TicketEntidad tickets = new CMP_TicketEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(reader["id"]),
                                campaña_id = ManejoNulos.ManageNullInteger(reader["campaña_id"]),
                                fechareg = ManejoNulos.ManageNullDate(reader["fechareg"]),
                                item = ManejoNulos.ManageNullInteger64(reader["item"]),
                                nroticket = ManejoNulos.ManageNullStr(reader["nroticket"]),
                                monto = ManejoNulos.ManageNullDouble(reader["monto"]),
                                FechaApertura = ManejoNulos.ManageNullDate(reader["fecha_apertura"]),
                                fecharegsala = ManejoNulos.ManageNullDate(reader["fecharegsala"]),
                                origen = ManejoNulos.ManageNullStr(reader["origen"]),
                                usuario_id = ManejoNulos.ManageNullInteger(reader["usuario_id"]),
                                estado = ManejoNulos.ManageNullInteger(reader["estado"]),
                                cliente_id = ManejoNulos.ManageNullInteger64(reader["cliente_id"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(reader["NombreCompleto"]),
                                NroDoc = ManejoNulos.ManageNullStr(reader["NroDoc"]),
                                Celular1 = ManejoNulos.ManageNullStr(reader["Celular1"]),
                                nombre_usuario = ManejoNulos.ManageNullStr(reader["UsuarioNombre"]),

                                Campania = new CMP_CampañaEntidad
                                {
                                    id = ManejoNulos.ManageNullInteger64(reader["campania_id"]),
                                    nombre = ManejoNulos.ManageNullStr(reader["campania_nombre"])
                                }
                            };

                            listTickets.Add(tickets);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return listTickets;
        }

        public CMP_TicketEntidad GetTicketItem(int item, string nroticket, double monto) {
            CMP_TicketEntidad ticket = new CMP_TicketEntidad();

            string query = @"
            SELECT
                id,
                campaña_id,
                fechareg,
                item,
                nroticket,
                monto,
                fecharegsala,
                origen,
                usuario_id,
                estado,
                cliente_id,
                fecha_apertura
            FROM CMP_Tickets
            WHERE item = @p1 AND nroticket = @p2 AND monto = @p3
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p1", item);
                    command.Parameters.AddWithValue("@p2", nroticket);
                    command.Parameters.AddWithValue("@p3", monto);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        if(data.HasRows) {
                            while(data.Read()) {
                                ticket.id = ManejoNulos.ManageNullInteger64(data["id"]);
                                ticket.campaña_id = ManejoNulos.ManageNullInteger(data["campaña_id"]);
                                ticket.fechareg = ManejoNulos.ManageNullDate(data["fechareg"]);
                                ticket.item = ManejoNulos.ManageNullInteger64(data["item"]);
                                ticket.nroticket = ManejoNulos.ManageNullStr(data["nroticket"]);
                                ticket.monto = ManejoNulos.ManageNullDouble(data["monto"]);
                                ticket.fecharegsala = ManejoNulos.ManageNullDate(data["fecharegsala"]);
                                ticket.origen = ManejoNulos.ManageNullStr(data["origen"]);
                                ticket.usuario_id = ManejoNulos.ManageNullInteger(data["usuario_id"]);
                                ticket.estado = ManejoNulos.ManageNullInteger(data["estado"]);
                                ticket.cliente_id = ManejoNulos.ManageNullInteger64(data["cliente_id"]);
                                ticket.FechaApertura = ManejoNulos.ManageNullDate(data["fecha_apertura"]);
                            }
                        }
                    }
                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);
            }

            return ticket;
        }

        public List<Reporte_Detalle_TITO_Caja> ATPGuardarTickets(int usuarioId, int campaniaId, int customer_id, List<Reporte_Detalle_TITO_Caja> tickets) {
            List<Reporte_Detalle_TITO_Caja> ticketsInsertado = new List<Reporte_Detalle_TITO_Caja>();

            string query = @"
            INSERT INTO CMP_Tickets (
                campaña_id,
                fechareg,
                item,
                nroticket,
                monto,
                fecharegsala,
                origen,
                usuario_id,
                estado,
                Apeclie,
                NomClie,
                Correo,
                Dni,
                FechaNacimiento,
                cliente_id,
                fecha_apertura
            )

            OUTPUT INSERTED.id

            VALUES (
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
                @p16
            )
            ";

            int instancia = 2;

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    foreach(Reporte_Detalle_TITO_Caja ticket in tickets) {
                        SqlCommand command = new SqlCommand(query, connection);

                        command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(campaniaId) == 0 ? SqlInt32.Null : campaniaId);
                        command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(DateTime.Now));
                        command.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(ticket.D00H_Item));
                        command.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(ticket.Ticket));
                        command.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDecimal(ticket.Monto_Dinero));
                        command.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(Convert.ToDateTime(ticket.Fecha_Proceso_Inicio)));
                        command.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(0));
                        command.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(usuarioId));
                        command.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(instancia));
                        command.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(string.Empty));
                        command.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(string.Empty));
                        command.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(string.Empty));
                        command.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullStr(string.Empty));
                        command.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullStr(string.Empty));
                        command.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullInteger64(customer_id));
                        command.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullDate(Convert.ToDateTime(ticket.FechaApertura)));

                        int idInsertado = Convert.ToInt32(command.ExecuteScalar());

                        if(idInsertado > 0) {
                            ticketsInsertado.Add(ticket);
                        }
                    }
                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);
            }

            return ticketsInsertado;
        }

        public bool ATPActualizarTicketsDiff(int salaId, List<Reporte_Detalle_TITO_Caja> tickets) {
            List<Reporte_Detalle_TITO_Caja> ticketsActualizado = new List<Reporte_Detalle_TITO_Caja>();

            string query = @"
            UPDATE CMP_Tickets
            SET
                fecharegsala = @p1,
                fecha_apertura = @p2
            FROM CMP_Tickets AS ATPTickets
            INNER JOIN CMP_Campaña AS ATPCampania
            ON ATPTickets.campaña_id = ATPCampania.id
            WHERE
                ATPTickets.item = @w1 AND ATPTickets.nroticket = @w2 AND ATPTickets.monto = @w3 AND ATPCampania.sala_id = @w4
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    foreach(Reporte_Detalle_TITO_Caja ticket in tickets) {
                        SqlCommand command = new SqlCommand(query, connection);

                        command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(Convert.ToDateTime(ticket.Fecha_Proceso_Inicio)));
                        command.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(Convert.ToDateTime(ticket.FechaApertura)));
                        command.Parameters.AddWithValue("@w1", ManejoNulos.ManageNullInteger(ticket.D00H_Item));
                        command.Parameters.AddWithValue("@w2", ManejoNulos.ManageNullStr(ticket.Ticket));
                        command.Parameters.AddWithValue("@w3", ManejoNulos.ManageNullDecimal(ticket.Monto_Dinero));
                        command.Parameters.AddWithValue("@w4", ManejoNulos.ManageNullInteger(salaId));

                        int rowsAffected = command.ExecuteNonQuery();

                        if(rowsAffected > 0) {
                            ticketsActualizado.Add(ticket);
                        }
                    }
                }
            } catch(Exception exception) {
                Console.WriteLine(exception.Message);
            }

            return ticketsActualizado.Count > 0;
        }

        #region Reporte Tickets

        public List<CMP_TicketReporteEntidad> ObtenerTicketsPorSalas(List<int> salaIds, DateTime fromDate, DateTime endDate, int campaniaTipo) {
            List<CMP_TicketReporteEntidad> listaTickets = new List<CMP_TicketReporteEntidad>();

            string consulta = $@"
            SELECT
                cmptick.id AS Id,
                cmptick.item AS Item,
                cmptick.nroticket AS NroTicket,
                cmptick.monto AS Monto,
                cmptick.fecharegsala AS FechaInicio,
                cmptick.fecha_apertura AS FechaApertura,
                cmptick.fechareg AS FechaRegistro,
                cmptick.estado AS Estado,
                cmptick.usuario_id AS UsuarioId,
                segusu.UsuarioNombre AS UsuarioNombre,
                cmptick.cliente_id AS ClienteId,
                astcli.NroDoc AS ClienteDOI,
                astcli.NombreCompleto AS ClienteNombres,
                cmptick.campaña_id AS CampaniaId,
                cmpcamp.nombre AS CampaniaNombre,
                cmpcamp.tipo AS CampaniaTipo,
                sala.CodSala AS SalaId,
                sala.Nombre AS SalaNombre
            FROM CMP_Tickets cmptick
            INNER JOIN CMP_Campaña cmpcamp ON cmpcamp.id = cmptick.campaña_id
            INNER JOIN Sala sala ON sala.CodSala = cmpcamp.sala_id
            LEFT JOIN AST_Cliente astcli ON astcli.Id = cmptick.cliente_id
            LEFT JOIN SEG_Usuario segusu ON segusu.UsuarioID = cmptick.usuario_id
            WHERE cmpcamp.sala_id IN ({string.Join(",", salaIds)}) AND cmpcamp.tipo = @campaniaTipo AND (CONVERT(DATE, cmpcamp.fechaini) BETWEEN CONVERT(DATE, @fechaInicio) AND CONVERT(DATE, @fechaFinal))
            ORDER BY cmpcamp.id DESC, cmptick.id DESC
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(consulta, connection) {
                        CommandTimeout = 0
                    };

                    command.Parameters.AddWithValue("@campaniaTipo", campaniaTipo);
                    command.Parameters.AddWithValue("@fechaInicio", fromDate);
                    command.Parameters.AddWithValue("@fechaFinal", endDate);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        while(data.Read()) {
                            CMP_TicketReporteEntidad cmpTicket = new CMP_TicketReporteEntidad {
                                Id = ManejoNulos.ManageNullInteger64(data["Id"]),
                                Item = ManejoNulos.ManageNullInteger64(data["Item"]),
                                NroTicket = ManejoNulos.ManageNullStr(data["NroTicket"]),
                                Monto = ManejoNulos.ManageNullDouble(data["Monto"]),
                                FechaInicio = ManejoNulos.ManageNullDate(data["FechaInicio"]),
                                FechaApertura = ManejoNulos.ManageNullDate(data["FechaApertura"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]),
                                Estado = ManejoNulos.ManageNullInteger(data["Estado"]),
                                UsuarioId = ManejoNulos.ManageNullInteger64(data["UsuarioId"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(data["UsuarioNombre"]),
                                ClienteId = ManejoNulos.ManageNullInteger64(data["ClienteId"]),
                                ClienteDOI = ManejoNulos.ManageNullStr(data["ClienteDOI"]),
                                ClienteNombres = ManejoNulos.ManageNullStr(data["ClienteNombres"]),
                                CampaniaId = ManejoNulos.ManageNullInteger64(data["CampaniaId"]),
                                CampaniaNombre = ManejoNulos.ManageNullStr(data["CampaniaNombre"]),
                                CampaniaTipo = ManejoNulos.ManageNullInteger(data["CampaniaTipo"]),
                                SalaId = ManejoNulos.ManageNullInteger64(data["SalaId"]),
                                SalaNombre = ManejoNulos.ManageNullStr(data["SalaNombre"])
                            };

                            listaTickets.Add(cmpTicket);
                        }
                    }
                }
            } catch(Exception) {
                listaTickets = new List<CMP_TicketReporteEntidad>();
            }

            return listaTickets;
        }

        #endregion
    }
}
