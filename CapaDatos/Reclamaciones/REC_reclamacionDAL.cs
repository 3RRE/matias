using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using S3k.Utilitario;
using CapaEntidad.Reclamaciones;
using S3k.Utilitario.clases_especial;

namespace CapaDatos.Reclamaciones
{
    public class REC_reclamacionDAL
    {
        string _conexion = "";
        public REC_reclamacionDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

		public (Int64 reclamaciontotalsala, ClaseError error) REC_reclamacionTotalSalaJson(Int64 codsala)
		{
			Int64 total = 0;
			ClaseError error = new ClaseError();
			string consulta = @"SELECT 
								COUNT(*) as total							
                                FROM[dbo].[REC_reclamacion] 
                                where sala_id=@p0;";
			try
			{
				using (var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@p0", codsala);
					using (var dr = query.ExecuteReader())
					{
						if (dr.HasRows)
						{
							while (dr.Read())
							{
								total = ManejoNulos.ManageNullInteger64(dr["total"]);								
							}
						}
					}

				}
			}
			catch (Exception ex)
			{
				error.Key = ex.Data.Count.ToString();
				error.Value = ex.Message;
			}
			return (reclamaciontotalsala: total, error);
		}
		public (List<REC_reclamacionEntidad> rec_reclamacionLista, ClaseError error) REC_reclamacionListarJson()
        {
            List<REC_reclamacionEntidad> lista = new List<REC_reclamacionEntidad>();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT 
								id
								,fecha
								,nombre
								,direccion
								,documento
								,telefono
								,correo
								,tipo
								,monto
								,descripcion
								,tipo_reclamo
								,tipo_local
								,sala_id
								,local_nombre
								,razon_social
								,local_direccion
								,correo_jop
								,correo_sop
								,referencia
								,detalle
								,pedido
								,hash
								,atencionsala
								,atencionlegal
								,atencion,nombrepadre_madre,fecha_enviolegal,usuario_sala,usuario_legal,direcciones_adjuntas
                                FROM[dbo].[REC_reclamacion] 
                                order by fecha desc;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var fila = new REC_reclamacionEntidad
								{
									id = ManejoNulos.ManageNullInteger64(dr["id"]),
									fecha = ManejoNulos.ManageNullDate(dr["fecha"]),
									nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
									direccion = ManejoNulos.ManageNullStr(dr["direccion"]),
									documento = ManejoNulos.ManageNullStr(dr["documento"]),
									telefono = ManejoNulos.ManageNullStr(dr["telefono"]),
									correo = ManejoNulos.ManageNullStr(dr["correo"]),
									tipo = ManejoNulos.ManageNullStr(dr["tipo"]),
									monto = ManejoNulos.ManageNullDouble(dr["monto"]),
									descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
									tipo_reclamo = ManejoNulos.ManageNullStr(dr["tipo_reclamo"]),
									tipo_local = ManejoNulos.ManageNullStr(dr["tipo_local"]),
									sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
									local_nombre = ManejoNulos.ManageNullStr(dr["local_nombre"]),
									razon_social = ManejoNulos.ManageNullStr(dr["razon_social"]),
									local_direccion = ManejoNulos.ManageNullStr(dr["local_direccion"]),
									correo_jop = ManejoNulos.ManageNullStr(dr["correo_jop"]),
									correo_sop = ManejoNulos.ManageNullStr(dr["correo_sop"]),
									referencia = ManejoNulos.ManageNullStr(dr["referencia"]),
									detalle = ManejoNulos.ManageNullStr(dr["detalle"]),
									pedido = ManejoNulos.ManageNullStr(dr["pedido"]),
									hash = ManejoNulos.ManageNullStr(dr["hash"]),

									atencionsala = ManejoNulos.ManageNullStr(dr["atencionsala"]),
									atencionlegal = ManejoNulos.ManageNullStr(dr["atencionlegal"]),
									atencion = ManejoNulos.ManageNullInteger(dr["atencion"]),
                                    nombrepadre_madre = ManejoNulos.ManageNullStr(dr["nombrepadre_madre"]),
                                    fecha_enviolegal = ManejoNulos.ManageNullDate(dr["fecha_enviolegal"]),
                                    usuario_sala = ManejoNulos.ManageNullStr(dr["usuario_sala"]),
                                    usuario_legal = ManejoNulos.ManageNullStr(dr["usuario_legal"]),
                                    direcciones_adjuntas = ManejoNulos.ManageNullStr(dr["direcciones_adjuntas"]),
								};

                                lista.Add(fila);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (rec_reclamacionLista: lista, error);
        }

		public (List<REC_reclamacionEntidad> rec_reclamacionLista, ClaseError error) REC_reclamacionListarxSalaFechaJson(string salas, DateTime fechaini, DateTime fechafin)
		{
			List<REC_reclamacionEntidad> lista = new List<REC_reclamacionEntidad>();
			ClaseError error = new ClaseError();
			string consulta = @"SELECT 
								id
								,codigo
								,fecha
								,nombre
								,direccion
								,documento
								,telefono
								,correo
								,tipo
								,monto
								,descripcion
								,tipo_reclamo
								,tipo_local
								,sala_id
								,local_nombre
								,razon_social
								,local_direccion
								,correo_jop
								,correo_sop
								,referencia
								,detalle
								,pedido
								,hash
								,estado
								,atencionsala
								,atencionlegal
								,atencion,nombrepadre_madre,fecha_enviolegal,usuario_sala,usuario_legal,direcciones_adjuntas,desistimiento,ruta_firma_desistimiento
                                FROM[dbo].[REC_reclamacion] where " + salas + " CONVERT(date, fecha) between @p0 and @p1 order by id desc;";
			try
			{
				using (var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@p0", fechaini);
					query.Parameters.AddWithValue("@p1", fechafin);
					using (var dr = query.ExecuteReader())
					{
						if (dr.HasRows)
						{
							while (dr.Read())
							{
								var fila = new REC_reclamacionEntidad
								{
									id = ManejoNulos.ManageNullInteger64(dr["id"]),
									fecha = ManejoNulos.ManageNullDate(dr["fecha"]),
									codigo = ManejoNulos.ManageNullStr(dr["codigo"]),
									nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
									direccion = ManejoNulos.ManageNullStr(dr["direccion"]),
									documento = ManejoNulos.ManageNullStr(dr["documento"]),
									telefono = ManejoNulos.ManageNullStr(dr["telefono"]),
									correo = ManejoNulos.ManageNullStr(dr["correo"]),
									tipo = ManejoNulos.ManageNullStr(dr["tipo"]),
									monto = ManejoNulos.ManageNullDouble(dr["monto"]),
									descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
									tipo_reclamo = ManejoNulos.ManageNullStr(dr["tipo_reclamo"]),
									tipo_local = ManejoNulos.ManageNullStr(dr["tipo_local"]),
									sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
									local_nombre = ManejoNulos.ManageNullStr(dr["local_nombre"]),
									razon_social = ManejoNulos.ManageNullStr(dr["razon_social"]),
									local_direccion = ManejoNulos.ManageNullStr(dr["local_direccion"]),
									correo_jop = ManejoNulos.ManageNullStr(dr["correo_jop"]),
									correo_sop = ManejoNulos.ManageNullStr(dr["correo_sop"]),
									referencia = ManejoNulos.ManageNullStr(dr["referencia"]),
									detalle = ManejoNulos.ManageNullStr(dr["detalle"]),
									pedido = ManejoNulos.ManageNullStr(dr["pedido"]),
									hash = ManejoNulos.ManageNullStr(dr["hash"]),
									estado = ManejoNulos.ManageNullInteger(dr["estado"]),
									atencionsala = ManejoNulos.ManageNullStr(dr["atencionsala"]),
									atencionlegal = ManejoNulos.ManageNullStr(dr["atencionlegal"]),
									atencion = ManejoNulos.ManageNullInteger(dr["atencion"]),
                                    nombrepadre_madre = ManejoNulos.ManageNullStr(dr["nombrepadre_madre"]),
                                    fecha_enviolegal = ManejoNulos.ManageNullDate(dr["fecha_enviolegal"]),
                                    usuario_sala = ManejoNulos.ManageNullStr(dr["usuario_sala"]),
                                    usuario_legal = ManejoNulos.ManageNullStr(dr["usuario_legal"]),
                                    direcciones_adjuntas = ManejoNulos.ManageNullStr(dr["direcciones_adjuntas"]),
                                    desistimiento = ManejoNulos.ManageNullInteger(dr["desistimiento"]),
                                    ruta_firma_desistimiento = ManejoNulos.ManageNullStr(dr["ruta_firma_desistimiento"]),
								};

								lista.Add(fila);
							}
						}
					}

				}
			}
			catch (Exception ex)
			{
				error.Key = ex.Data.Count.ToString();
				error.Value = ex.Message;
			}
			return (rec_reclamacionLista: lista, error);
		}

		public (REC_reclamacionEntidad rec_reclamacion, ClaseError error) REC_reclamacionIdObtenerJson(Int64 id)
        {
			REC_reclamacionEntidad objeto = new REC_reclamacionEntidad();
            ClaseError error = new ClaseError();
            string consulta = @"
			SELECT
				id,
				fecha,
				codigo,
				nombre,
				direccion,
				documento,
				telefono,
				correo,
				tipo,
				monto,
				descripcion,
				tipo_reclamo,
				tipo_local,
				sala_id,
				local_nombre,
				razon_social,
				local_direccion,
				correo_jop,
				correo_sop,
				referencia,
				detalle,
				pedido,
				hash,
				estado,
				atencionsala,
				atencionlegal,
				atencion,
				adjunto,
				nombrepadre_madre,
				fecha_enviolegal,
				usuario_sala,
				usuario_legal,
				direcciones_adjuntas,
				desistimiento,
				ruta_firma_desistimiento,
				enviar_adjunto
			FROM REC_reclamacion
			where id = @p0
			";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
								objeto.id = ManejoNulos.ManageNullInteger64(dr["id"]);
								objeto.fecha = ManejoNulos.ManageNullDate(dr["fecha"]);
								objeto.codigo = ManejoNulos.ManageNullStr(dr["codigo"]);
								objeto.nombre = ManejoNulos.ManageNullStr(dr["nombre"]);
								objeto.direccion = ManejoNulos.ManageNullStr(dr["direccion"]);
								objeto.documento = ManejoNulos.ManageNullStr(dr["documento"]);
								objeto.telefono = ManejoNulos.ManageNullStr(dr["telefono"]);
								objeto.correo = ManejoNulos.ManageNullStr(dr["correo"]);
								objeto.tipo = ManejoNulos.ManageNullStr(dr["tipo"]);
								objeto.monto = ManejoNulos.ManageNullDouble(dr["monto"]);
								objeto.descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]);
								objeto.tipo_reclamo = ManejoNulos.ManageNullStr(dr["tipo_reclamo"]);
								objeto.tipo_local = ManejoNulos.ManageNullStr(dr["tipo_local"]);
								objeto.sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]);
								objeto.local_nombre = ManejoNulos.ManageNullStr(dr["local_nombre"]);
								objeto.razon_social = ManejoNulos.ManageNullStr(dr["razon_social"]);
								objeto.local_direccion = ManejoNulos.ManageNullStr(dr["local_direccion"]);
								objeto.correo_jop = ManejoNulos.ManageNullStr(dr["correo_jop"]);
								objeto.correo_sop = ManejoNulos.ManageNullStr(dr["correo_sop"]);
								objeto.referencia = ManejoNulos.ManageNullStr(dr["referencia"]);
								objeto.detalle = ManejoNulos.ManageNullStr(dr["detalle"]);
								objeto.pedido = ManejoNulos.ManageNullStr(dr["pedido"]);
								objeto.hash = ManejoNulos.ManageNullStr(dr["hash"]);
								objeto.estado = ManejoNulos.ManageNullInteger(dr["estado"]);
								objeto.atencionsala = ManejoNulos.ManageNullStr(dr["atencionsala"]);
								objeto.atencionlegal = ManejoNulos.ManageNullStr(dr["atencionlegal"]);
								objeto.atencion = ManejoNulos.ManageNullInteger(dr["atencion"]);
								objeto.adjunto = ManejoNulos.ManageNullStr(dr["adjunto"]);
								objeto.nombrepadre_madre = ManejoNulos.ManageNullStr(dr["nombrepadre_madre"]);
								objeto.fecha_enviolegal = ManejoNulos.ManageNullDate(dr["fecha_enviolegal"]);
								objeto.usuario_sala = ManejoNulos.ManageNullStr(dr["usuario_sala"]);
								objeto.usuario_legal = ManejoNulos.ManageNullStr(dr["usuario_legal"]);
								objeto.direcciones_adjuntas = ManejoNulos.ManageNullStr(dr["direcciones_adjuntas"]);
								objeto.desistimiento = ManejoNulos.ManageNullInteger(dr["desistimiento"]);
								objeto.ruta_firma_desistimiento = ManejoNulos.ManageNullStr(dr["ruta_firma_desistimiento"]);
                                objeto.enviar_adjunto = ManejoNulos.ManageNullInteger(dr["enviar_adjunto"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (rec_reclamacion: objeto, error);
        }

		public (REC_reclamacionEntidad rec_reclamacion, ClaseError error) REC_reclamacionHashObtenerJson(string doc)
		{
			REC_reclamacionEntidad objeto = new REC_reclamacionEntidad();
			ClaseError error = new ClaseError();
			string consulta = @"
			SELECT
				id,
				fecha,
				codigo,
				nombre,
				direccion,
				documento,
				telefono,
				correo,
				tipo,
				monto,
				descripcion,
				tipo_reclamo,
				tipo_local,
				sala_id,
				local_nombre,
				razon_social,
				local_direccion,
				correo_jop,
				correo_sop,
				referencia,
				detalle,
				pedido,
				hash,
				estado,
				atencionsala,
				atencionlegal,
				atencion,
				adjunto,
				nombrepadre_madre,
				fecha_enviolegal,
				usuario_sala,
				usuario_legal,
				direcciones_adjuntas,
				desistimiento,
				ruta_firma_desistimiento,
				enviar_adjunto
			FROM REC_reclamacion
			WHERE hash = @p0
			";

			try
			{
				using (var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@p0", doc);
					using (var dr = query.ExecuteReader())
					{
						if (dr.HasRows)
						{
							while (dr.Read())
							{
								objeto.id = ManejoNulos.ManageNullInteger64(dr["id"]);
								objeto.codigo = ManejoNulos.ManageNullStr(dr["codigo"]);
								objeto.fecha = ManejoNulos.ManageNullDate(dr["fecha"]);
								objeto.nombre = ManejoNulos.ManageNullStr(dr["nombre"]);
								objeto.direccion = ManejoNulos.ManageNullStr(dr["direccion"]);
								objeto.documento = ManejoNulos.ManageNullStr(dr["documento"]);
								objeto.telefono = ManejoNulos.ManageNullStr(dr["telefono"]);
								objeto.correo = ManejoNulos.ManageNullStr(dr["correo"]);
								objeto.tipo = ManejoNulos.ManageNullStr(dr["tipo"]);
								objeto.monto = ManejoNulos.ManageNullDouble(dr["monto"]);
								objeto.descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]);
								objeto.tipo_reclamo = ManejoNulos.ManageNullStr(dr["tipo_reclamo"]);
								objeto.tipo_local = ManejoNulos.ManageNullStr(dr["tipo_local"]);
								objeto.sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]);
								objeto.local_nombre = ManejoNulos.ManageNullStr(dr["local_nombre"]);
								objeto.razon_social = ManejoNulos.ManageNullStr(dr["razon_social"]);
								objeto.local_direccion = ManejoNulos.ManageNullStr(dr["local_direccion"]);
								objeto.correo_jop = ManejoNulos.ManageNullStr(dr["correo_jop"]);
								objeto.correo_sop = ManejoNulos.ManageNullStr(dr["correo_sop"]);
								objeto.referencia = ManejoNulos.ManageNullStr(dr["referencia"]);
								objeto.detalle = ManejoNulos.ManageNullStr(dr["detalle"]);
								objeto.pedido = ManejoNulos.ManageNullStr(dr["pedido"]);
								objeto.hash = ManejoNulos.ManageNullStr(dr["hash"]);
								objeto.atencionsala = ManejoNulos.ManageNullStr(dr["atencionsala"]);
								objeto.atencionlegal = ManejoNulos.ManageNullStr(dr["atencionlegal"]);
								objeto.atencion = ManejoNulos.ManageNullInteger(dr["atencion"]);
                                objeto.adjunto = ManejoNulos.ManageNullStr(dr["adjunto"]);
                                objeto.estado = ManejoNulos.ManageNullInteger(dr["estado"]);
								objeto.nombrepadre_madre = ManejoNulos.ManageNullStr(dr["nombrepadre_madre"]);
								objeto.fecha_enviolegal = ManejoNulos.ManageNullDate(dr["fecha_enviolegal"]);
								objeto.usuario_sala = ManejoNulos.ManageNullStr(dr["usuario_sala"]);
								objeto.usuario_legal = ManejoNulos.ManageNullStr(dr["usuario_legal"]);
								objeto.direcciones_adjuntas = ManejoNulos.ManageNullStr(dr["direcciones_adjuntas"]);
								objeto.desistimiento = ManejoNulos.ManageNullInteger(dr["desistimiento"]);
								objeto.ruta_firma_desistimiento = ManejoNulos.ManageNullStr(dr["ruta_firma_desistimiento"]);
                                objeto.enviar_adjunto = ManejoNulos.ManageNullInteger(dr["enviar_adjunto"]);
                            }
						}
					}
				}
			}
			catch (Exception ex)
			{
				error.Key = ex.Data.Count.ToString();
				error.Value = ex.Message;
			}
			return (rec_reclamacion: objeto, error);
		}
		public (Int64 REC_reclamacionInsertado, ClaseError error) REC_reclamacionInsertarJson(REC_reclamacionEntidad REC_reclamacion)
        {
            Int64 idREC_reclamacionInsertado = 0;
            string consulta = @"
            INSERT INTO [dbo].[REC_reclamacion]
            (					codigo
								,fecha
								,nombre
								,direccion
								,documento
								,telefono
								,correo
								,tipo
								,monto
								,descripcion
								,tipo_reclamo
								,tipo_local
								,sala_id
								,local_nombre
								,razon_social
								,local_direccion
								,correo_jop
								,correo_sop
								,referencia
								,detalle
								,pedido
								,hash,estado,nombrepadre_madre,direcciones_adjuntas)
	            VALUES (@codigo,@fecha
								,@nombre
								,@direccion
								,@documento
								,@telefono
								,@correo
								,@tipo
								,@monto
								,@descripcion
								,@tipo_reclamo
								,@tipo_local
								,@sala_id
								,@local_nombre
								,@razon_social
								,@local_direccion
								,@correo_jop
								,@correo_sop
								,@referencia
								,@detalle
								,@pedido
								,@hash,@estado,@nombrepadre_madre,@direcciones_adjuntas) 
                SELECT SCOPE_IDENTITY()";
            ClaseError error = new ClaseError();
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@codigo", ManejoNulos.ManageNullStr(REC_reclamacion.codigo));
					query.Parameters.AddWithValue("@fecha", ManejoNulos.ManageNullDate(REC_reclamacion.fecha));
					query.Parameters.AddWithValue("@nombre", ManejoNulos.ManageNullStr(REC_reclamacion.nombre));
					query.Parameters.AddWithValue("@direccion", ManejoNulos.ManageNullStr(REC_reclamacion.direccion));
					query.Parameters.AddWithValue("@documento", ManejoNulos.ManageNullStr(REC_reclamacion.documento));
					query.Parameters.AddWithValue("@telefono", ManejoNulos.ManageNullStr(REC_reclamacion.telefono));
					query.Parameters.AddWithValue("@correo", ManejoNulos.ManageNullStr(REC_reclamacion.correo));
					query.Parameters.AddWithValue("@tipo", ManejoNulos.ManageNullStr(REC_reclamacion.tipo));
					query.Parameters.AddWithValue("@monto", ManejoNulos.ManageNullDouble(REC_reclamacion.monto));
					query.Parameters.AddWithValue("@descripcion", ManejoNulos.ManageNullStr(REC_reclamacion.descripcion));
					query.Parameters.AddWithValue("@tipo_reclamo", ManejoNulos.ManageNullStr(REC_reclamacion.tipo_reclamo));
					query.Parameters.AddWithValue("@tipo_local", ManejoNulos.ManageNullStr(REC_reclamacion.tipo_local));
					query.Parameters.AddWithValue("@sala_id", ManejoNulos.ManageNullInteger64(REC_reclamacion.sala_id));
					query.Parameters.AddWithValue("@local_nombre", ManejoNulos.ManageNullStr(REC_reclamacion.local_nombre));
					query.Parameters.AddWithValue("@razon_social", ManejoNulos.ManageNullStr(REC_reclamacion.razon_social));
					query.Parameters.AddWithValue("@local_direccion", ManejoNulos.ManageNullStr(REC_reclamacion.local_direccion));
					query.Parameters.AddWithValue("@correo_jop", ManejoNulos.ManageNullStr(REC_reclamacion.correo_jop));
					query.Parameters.AddWithValue("@correo_sop", ManejoNulos.ManageNullStr(REC_reclamacion.correo_sop));
					query.Parameters.AddWithValue("@referencia", ManejoNulos.ManageNullStr(REC_reclamacion.referencia));
					query.Parameters.AddWithValue("@detalle", ManejoNulos.ManageNullStr(REC_reclamacion.detalle));
					query.Parameters.AddWithValue("@pedido", ManejoNulos.ManageNullStr(REC_reclamacion.pedido));
					query.Parameters.AddWithValue("@hash", ManejoNulos.ManageNullStr(REC_reclamacion.hash));
					query.Parameters.AddWithValue("@estado",0);
					query.Parameters.AddWithValue("@nombrepadre_madre", ManejoNulos.ManageNullStr(REC_reclamacion.nombrepadre_madre));
					query.Parameters.AddWithValue("@direcciones_adjuntas", ManejoNulos.ManageNullStr(REC_reclamacion.direcciones_adjuntas));

					idREC_reclamacionInsertado = Int64.Parse(query.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (REC_reclamacionInsertado: idREC_reclamacionInsertado, error);
        }

        public (bool REC_reclamacionEditado, ClaseError error) REC_reclamacionEditarJson(REC_reclamacionEntidad REC_reclamacion)
        {
            ClaseError error = new ClaseError();
            bool response = false;
            string consulta = @"UPDATE REC_reclamacion
	                            SET  fecha= @fecha
								,nombre= @nombre
								,direccion= @direccion
								,documento= @documento
								,telefono= @telefono
								,correo= @correo
								,tipo= @tipo
								,monto= @monto
								,descripcion= @descripcion
								,tipo_reclamo= @tipo_reclamo
								,tipo_local= @tipo_local
								,sala_id= @sala_id
								,local_nombre= @local_nombre
								,razon_social= @razon_social
								,local_direccion= @local_direccion
								,correo_jop= @correo_jop
								,correo_sop= @correo_sop
								,referencia= @referencia
								,detalle= @detalle
								,pedido= @pedido
								,hash= @hash,nombrepadre_madre=@nombrepadre_madre
	                            WHERE id=@id;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
				    query.Parameters.AddWithValue("@id", ManejoNulos.ManageNullInteger64(REC_reclamacion.id));

					query.Parameters.AddWithValue("@fecha", ManejoNulos.ManageNullDate(REC_reclamacion.fecha));
					query.Parameters.AddWithValue("@nombre", ManejoNulos.ManageNullStr(REC_reclamacion.nombre));
					query.Parameters.AddWithValue("@direccion", ManejoNulos.ManageNullStr(REC_reclamacion.direccion));
					query.Parameters.AddWithValue("@documento", ManejoNulos.ManageNullStr(REC_reclamacion.documento));
					query.Parameters.AddWithValue("@telefono", ManejoNulos.ManageNullStr(REC_reclamacion.telefono));
					query.Parameters.AddWithValue("@correo", ManejoNulos.ManageNullStr(REC_reclamacion.correo));
					query.Parameters.AddWithValue("@tipo", ManejoNulos.ManageNullStr(REC_reclamacion.tipo));
					query.Parameters.AddWithValue("@monto", ManejoNulos.ManageNullDecimal(REC_reclamacion.monto));
					query.Parameters.AddWithValue("@descripcion", ManejoNulos.ManageNullStr(REC_reclamacion.descripcion));
					query.Parameters.AddWithValue("@tipo_reclamo", ManejoNulos.ManageNullStr(REC_reclamacion.tipo_reclamo));
					query.Parameters.AddWithValue("@tipo_local", ManejoNulos.ManageNullStr(REC_reclamacion.tipo_local));
					query.Parameters.AddWithValue("@sala_id", ManejoNulos.ManageNullInteger64(REC_reclamacion.sala_id));
					query.Parameters.AddWithValue("@local_nombre", ManejoNulos.ManageNullStr(REC_reclamacion.local_nombre));
					query.Parameters.AddWithValue("@razon_social", ManejoNulos.ManageNullStr(REC_reclamacion.razon_social));
					query.Parameters.AddWithValue("@local_direccion", ManejoNulos.ManageNullStr(REC_reclamacion.local_direccion));
					query.Parameters.AddWithValue("@correo_jop", ManejoNulos.ManageNullStr(REC_reclamacion.correo_jop));
					query.Parameters.AddWithValue("@correo_sop", ManejoNulos.ManageNullStr(REC_reclamacion.correo_sop));
					query.Parameters.AddWithValue("@referencia", ManejoNulos.ManageNullStr(REC_reclamacion.referencia));
					query.Parameters.AddWithValue("@detalle", ManejoNulos.ManageNullStr(REC_reclamacion.detalle));
					query.Parameters.AddWithValue("@pedido", ManejoNulos.ManageNullStr(REC_reclamacion.pedido));
					query.Parameters.AddWithValue("@hash", ManejoNulos.ManageNullStr(REC_reclamacion.hash));
					query.Parameters.AddWithValue("@nombrepadre_madre", ManejoNulos.ManageNullStr(REC_reclamacion.nombrepadre_madre));

                
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (REC_reclamacionEditado: response, error);
        }

		public (bool REC_reclamacionEditado, ClaseError error) ReclamacionAtencionSalaJson(REC_reclamacionEntidad REC_reclamacion)
		{
			ClaseError error = new ClaseError();
			bool response = false;
			string consulta = @"UPDATE REC_reclamacion
	                            SET  
                                usuario_sala=@usuario_sala,
								atencionsala= @atencionsala,
								atencion=1
	                            WHERE id=@id;";
			try
			{
				using (var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@id", ManejoNulos.ManageNullInteger64(REC_reclamacion.id));
					query.Parameters.AddWithValue("@atencionsala", ManejoNulos.ManageNullStr(REC_reclamacion.atencionsala));
					query.Parameters.AddWithValue("@usuario_sala", ManejoNulos.ManageNullStr(REC_reclamacion.usuario_sala));
					query.ExecuteNonQuery();
					response = true;
				}
			}
			catch (Exception ex)
			{
				error.Key = ex.Data.Count.ToString();
				error.Value = ex.Message;
			}
			return (REC_reclamacionEditado: response, error);
		}

		public (bool REC_reclamacionEditado, ClaseError error) ReclamacionAtencionLegalJson(REC_reclamacionEntidad REC_reclamacion)
		{
			ClaseError error = new ClaseError();
			bool response = false;
			string consulta = @"UPDATE REC_reclamacion
	                            SET  
								atencionlegal= @atencionlegal,
								atencion=2,
                                fecha_enviolegal=@fecha_enviolegal,
                                usuario_legal=@usuario_legal
	                            WHERE id=@id;";
			try
			{
				using (var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@id", ManejoNulos.ManageNullInteger64(REC_reclamacion.id));
					query.Parameters.AddWithValue("@atencionlegal", ManejoNulos.ManageNullStr(REC_reclamacion.atencionlegal));
					query.Parameters.AddWithValue("@fecha_enviolegal", ManejoNulos.ManageNullDate(REC_reclamacion.fecha_enviolegal));
					query.Parameters.AddWithValue("@usuario_legal", ManejoNulos.ManageNullStr(REC_reclamacion.usuario_legal));
					query.ExecuteNonQuery();
					response = true;
				}
			}
			catch (Exception ex)
			{
				error.Key = ex.Data.Count.ToString();
				error.Value = ex.Message;
			}
			return (REC_reclamacionEditado: response, error);
		}

		public (bool rec_reclamacionEliminado, ClaseError error) REC_reclamacionEliminarJson(Int64 id)
        {
            bool response = false;
            string consulta = @"DELETE FROM REC_reclamacion
	                                WHERE id=@id;";
            ClaseError error = new ClaseError();
            try
                {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();

                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@id", ManejoNulos.ManageNullInteger64(id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }

            return (rec_reclamacionEliminado: response, error);
        }

		public (bool REC_reclamacionEditado, ClaseError error) reclamacionEditarHashJson(REC_reclamacionEntidad REC_reclamacion)
		{
			ClaseError error = new ClaseError();
			bool response = false;
			string consulta = @"UPDATE REC_reclamacion
	                            SET  hash=@hash
								,local_nombre=@local_nombre
								,razon_social=@razon_social
								,local_direccion=@local_direccion
	                            WHERE id=@id;";
			try
			{
				using (var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@id", ManejoNulos.ManageNullInteger64(REC_reclamacion.id));
					query.Parameters.AddWithValue("@local_nombre", ManejoNulos.ManageNullStr(REC_reclamacion.local_nombre));
					query.Parameters.AddWithValue("@razon_social", ManejoNulos.ManageNullStr(REC_reclamacion.razon_social));
					query.Parameters.AddWithValue("@local_direccion", ManejoNulos.ManageNullStr(REC_reclamacion.local_direccion));
					query.Parameters.AddWithValue("@hash", ManejoNulos.ManageNullStr(REC_reclamacion.hash));
					query.ExecuteNonQuery();
					response = true;
				}
			}
			catch (Exception ex)
			{
				error.Key = ex.Data.Count.ToString();
				error.Value = ex.Message;
			}
			return (REC_reclamacionEditado: response, error);
		}
        public (bool REC_reclamacionEditado,ClaseError error) REC_ReclamacionEditarAdjunto(REC_reclamacionEntidad REC_reclamacion)
        {
            ClaseError error = new ClaseError();
            bool response = false;
            string consulta = @"UPDATE REC_reclamacion
	                            SET  adjunto=@adjunto
	                            WHERE id=@id;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@id", ManejoNulos.ManageNullInteger64(REC_reclamacion.id));
                    query.Parameters.AddWithValue("@adjunto", ManejoNulos.ManageNullStr(REC_reclamacion.adjunto));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (REC_reclamacionEditado: response, error);
        }

        public (List<REC_reclamacionEntidad> lista, ClaseError error) REC_reclamacionListarporIdsJson(string ids)
        {
            List<REC_reclamacionEntidad> lista= new List<REC_reclamacionEntidad>();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT id
								,fecha
								,codigo
								,nombre
								,direccion
								,documento
								,telefono
								,correo
								,tipo
								,monto
								,descripcion
								,tipo_reclamo
								,tipo_local
								,sala_id
								,local_nombre
								,razon_social
								,local_direccion
								,correo_jop
								,correo_sop
								,referencia
								,detalle
								,pedido
								,hash
								,estado
								,atencionsala
								,atencionlegal
								,atencion,adjunto,nombrepadre_madre,fecha_enviolegal,usuario_sala,usuario_legal,direcciones_adjuntas
                                FROM [dbo].[REC_reclamacion]
                                where " + ids+";";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                REC_reclamacionEntidad reclamacion = new REC_reclamacionEntidad()
                                {
                                    id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                    fecha = ManejoNulos.ManageNullDate(dr["fecha"]),
                                    codigo = ManejoNulos.ManageNullStr(dr["codigo"]),
                                    nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                    direccion = ManejoNulos.ManageNullStr(dr["direccion"]),
                                    documento = ManejoNulos.ManageNullStr(dr["documento"]),
                                    telefono = ManejoNulos.ManageNullStr(dr["telefono"]),
                                    correo = ManejoNulos.ManageNullStr(dr["correo"]),
                                    tipo = ManejoNulos.ManageNullStr(dr["tipo"]),
                                    monto = ManejoNulos.ManageNullDouble(dr["monto"]),
                                    descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                    tipo_reclamo = ManejoNulos.ManageNullStr(dr["tipo_reclamo"]),
                                    tipo_local = ManejoNulos.ManageNullStr(dr["tipo_local"]),
                                    sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                    local_nombre = ManejoNulos.ManageNullStr(dr["local_nombre"]),
                                    razon_social = ManejoNulos.ManageNullStr(dr["razon_social"]),
                                    local_direccion = ManejoNulos.ManageNullStr(dr["local_direccion"]),
                                    correo_jop = ManejoNulos.ManageNullStr(dr["correo_jop"]),
                                    correo_sop = ManejoNulos.ManageNullStr(dr["correo_sop"]),
                                    referencia = ManejoNulos.ManageNullStr(dr["referencia"]),
                                    detalle = ManejoNulos.ManageNullStr(dr["detalle"]),
                                    pedido = ManejoNulos.ManageNullStr(dr["pedido"]),
                                    hash = ManejoNulos.ManageNullStr(dr["hash"]),
                                    estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                                    atencionsala = ManejoNulos.ManageNullStr(dr["atencionsala"]),
                                    atencionlegal = ManejoNulos.ManageNullStr(dr["atencionlegal"]),
                                    atencion = ManejoNulos.ManageNullInteger(dr["atencion"]),
                                    adjunto = ManejoNulos.ManageNullStr(dr["adjunto"]),
                                    nombrepadre_madre = ManejoNulos.ManageNullStr(dr["nombrepadre_madre"]),
                                    fecha_enviolegal = ManejoNulos.ManageNullDate(dr["fecha_enviolegal"]),
                                    usuario_sala = ManejoNulos.ManageNullStr(dr["usuario_sala"]),
                                    usuario_legal = ManejoNulos.ManageNullStr(dr["usuario_legal"]),
                                    direcciones_adjuntas = ManejoNulos.ManageNullStr(dr["direcciones_adjuntas"]),
                                };
                                lista.Add(reclamacion);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (lista: lista, error);
        }
        public (bool REC_reclamacionEditado, ClaseError error) reclamacionGuardarDesistimientoJson(REC_reclamacionEntidad REC_reclamacion)
        {
            ClaseError error = new ClaseError();
            bool response = false;
            string consulta = @"UPDATE REC_reclamacion
	                            SET  
								desistimiento=@desistimiento
								,ruta_firma_desistimiento=@ruta_firma_desistimiento
	                            WHERE id=@id;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@desistimiento", ManejoNulos.ManageNullInteger(REC_reclamacion.desistimiento));
                    query.Parameters.AddWithValue("@ruta_firma_desistimiento", ManejoNulos.ManageNullStr(REC_reclamacion.ruta_firma_desistimiento));
                    query.Parameters.AddWithValue("@id", ManejoNulos.ManageNullInteger(REC_reclamacion.id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (REC_reclamacionEditado: response, error);
        }

        public (bool enviarAdjuntoEditado, ClaseError error) ActualizarEnviarAdjunto(long reclamacionId, int enviarAdjunto)
		{
            ClaseError error = new ClaseError();

            bool response = false;

            string query = @"
			UPDATE REC_reclamacion
			SET  
				enviar_adjunto = @p1
			WHERE id = @w1
			";

            try
			{
                using (SqlConnection connection = new SqlConnection(_conexion))
				{
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(enviarAdjunto));
                    command.Parameters.AddWithValue("@w1", ManejoNulos.ManageNullInteger64(reclamacionId));

                    int rowsAffected = command.ExecuteNonQuery();

					if (rowsAffected > 0)
					{
						response = true;
					}
                }
            }
			catch (Exception exception)
			{
                error.Key = exception.Data.Count.ToString();
                error.Value = exception.Message;
            }

            return (enviarAdjuntoEditado: response, error);
        }
    }
}