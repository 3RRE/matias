using CapaEntidad;
using CapaEntidad.WhatsApp;
using S3k.Utilitario;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.WhatsApp
{
    public class WSP_MensajeriaClienteDAL
    {
        string _conexion = string.Empty;

        public WSP_MensajeriaClienteDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<WSP_MensajeriaClienteEntidad> ObtenerClientes()
        {
            List<WSP_MensajeriaClienteEntidad> lista = new List<WSP_MensajeriaClienteEntidad>();
            string consulta = @"
                SELECT 
	                c.Id AS 'idCliente',
	                c.SalaId AS 'idSalaCliente',
	                cs.SalaId AS 'idSala',
	                s.Nombre AS 'nombreSala',
	                c.Nombre AS 'nombreCliente',
	                c.ApelPat AS 'apellidoPaternoCliente',
	                c.ApelMat AS 'apellidoMaternoCliente',
	                c.codigoPais AS 'codigoPais',
	                c.Celular1 AS 'numero',
	                c.Celular AS 'numeroAlternativo',
	                tc.Id AS 'idTipoCliente',
	                tc.Nombre AS 'tipoCliente',
	                tf.Id AS 'idTipoFrecuencia',
	                tf.Nombre AS 'tipoFrecuencia',
	                tj.Id AS 'idTipoJuego',
	                tj.Nombre AS 'tipoJuego'
                FROM
	                AST_ClienteSala AS cs
                INNER JOIN AST_Cliente AS c ON c.Id = cs.ClienteId
                LEFT JOIN Sala AS s ON s.CodSala = cs.SalaId
                LEFT JOIN AST_TipoCliente AS tc ON tc.Id = cs.TipoClienteId
                LEFT JOIN AST_TipoFrecuencia AS tf ON tf.Id = cs.TipoFrecuenciaId
                LEFT JOIN AST_TipoJuego AS tj ON tj.Id = cs.TipoJuegoId
                ORDER BY
	                c.FechaRegistro DESC
            ";
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
                            var item = new WSP_MensajeriaClienteEntidad
                            {
                                idCliente = ManejoNulos.ManageNullInteger(dr["idCliente"]),
                                idSalaCliente = ManejoNulos.ManageNullInteger(dr["idSalaCliente"]),
                                idSala = ManejoNulos.ManageNullInteger(dr["idSala"]),
                                nombreSala = ManejoNulos.ManageNullStr(dr["nombreSala"]).Trim(),
                                nombreCliente = ManejoNulos.ManageNullStr(dr["nombreCliente"]).Trim(),
                                apellidoPaternoCliente = ManejoNulos.ManageNullStr(dr["apellidoPaternoCliente"]).Trim(),
                                apellidoMaternoCliente = ManejoNulos.ManageNullStr(dr["apellidoMaternoCliente"]).Trim(),
                                codigoPais = ManejoNulos.ManageNullStr(dr["codigoPais"]).Trim(),
                                numero = ManejoNulos.ManageNullStr(dr["numero"]).Trim(),
                                numeroAlternativo = ManejoNulos.ManageNullStr(dr["numeroAlternativo"]).Trim(),
                                idTipoCliente = ManejoNulos.ManageNullInteger(dr["idTipoCliente"]),
                                tipoCliente = ManejoNulos.ManageNullStr(dr["tipoCliente"]).Trim(),
                                idTipoFrecuencia = ManejoNulos.ManageNullInteger(dr["idTipoFrecuencia"]),
                                tipoFrecuencia = ManejoNulos.ManageNullStr(dr["tipoFrecuencia"]).Trim(),
                                idTipoJuego = ManejoNulos.ManageNullInteger(dr["idTipoJuego"]),
                                tipoJuego = ManejoNulos.ManageNullStr(dr["tipoJuego"]).Trim()
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
            return lista;
        }

        public List<WSP_MensajeriaClienteEntidad> obtenerClientesPorFiltro(string filtro)
        {
            List<WSP_MensajeriaClienteEntidad> lista = new List<WSP_MensajeriaClienteEntidad>();
            string consulta = $@"
                SELECT 
	                c.Id AS 'idCliente',
	                c.SalaId AS 'idSalaCliente',
	                cs.SalaId AS 'idSala',
	                s.Nombre AS 'nombreSala',
	                c.Nombre AS 'nombreCliente',
	                c.ApelPat AS 'apellidoPaternoCliente',
	                c.ApelMat AS 'apellidoMaternoCliente',
	                c.codigoPais AS 'codigoPais',
	                c.Celular1 AS 'numero',
	                c.Celular2 AS 'numeroAlternativo',
	                tc.Id AS 'idTipoCliente',
	                tc.Nombre AS 'tipoCliente',
	                tf.Id AS 'idTipoFrecuencia',
	                tf.Nombre AS 'tipoFrecuencia',
	                tj.Id AS 'idTipoJuego',
	                tj.Nombre AS 'tipoJuego'
                FROM
	                AST_ClienteSala AS cs
                INNER JOIN AST_Cliente AS c ON c.Id = cs.ClienteId
                LEFT JOIN Sala AS s ON s.CodSala = cs.SalaId
                LEFT JOIN AST_TipoCliente AS tc ON tc.Id = cs.TipoClienteId
                LEFT JOIN AST_TipoFrecuencia AS tf ON tf.Id = cs.TipoFrecuenciaId
                LEFT JOIN AST_TipoJuego AS tj ON tj.Id = cs.TipoJuegoId
                WHERE
                 {filtro}
                ORDER BY
	                c.FechaRegistro DESC
            ";
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
                            var item = new WSP_MensajeriaClienteEntidad
                            {
                                idCliente = ManejoNulos.ManageNullInteger(dr["idCliente"]),
                                idSalaCliente = ManejoNulos.ManageNullInteger(dr["idSalaCliente"]),
                                idSala = ManejoNulos.ManageNullInteger(dr["idSala"]),
                                nombreSala = ManejoNulos.ManageNullStr(dr["nombreSala"]).Trim(),
                                nombreCliente = ManejoNulos.ManageNullStr(dr["nombreCliente"]).Trim(),
                                apellidoPaternoCliente = ManejoNulos.ManageNullStr(dr["apellidoPaternoCliente"]).Trim(),
                                apellidoMaternoCliente = ManejoNulos.ManageNullStr(dr["apellidoMaternoCliente"]).Trim(),
                                codigoPais = ManejoNulos.ManageNullStr(dr["codigoPais"]).Trim(),
                                numero = ManejoNulos.ManageNullStr(dr["numero"]).Trim(),
                                numeroAlternativo = ManejoNulos.ManageNullStr(dr["numeroAlternativo"]).Trim(),
                                idTipoCliente = ManejoNulos.ManageNullInteger(dr["idTipoCliente"]),
                                tipoCliente = ManejoNulos.ManageNullStr(dr["tipoCliente"]).Trim(),
                                idTipoFrecuencia = ManejoNulos.ManageNullInteger(dr["idTipoFrecuencia"]),
                                tipoFrecuencia = ManejoNulos.ManageNullStr(dr["tipoFrecuencia"]).Trim(),
                                idTipoJuego = ManejoNulos.ManageNullInteger(dr["idTipoJuego"]),
                                tipoJuego = ManejoNulos.ManageNullStr(dr["tipoJuego"]).Trim()
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
            return lista;
        }
    }
}
