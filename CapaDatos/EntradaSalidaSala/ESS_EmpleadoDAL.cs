using CapaEntidad.BUK;
using CapaEntidad.EntradaSalidaSala;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.EntradaSalidaSala {
    public class ESS_EmpleadoDAL {
        string conexion = string.Empty;
        public ESS_EmpleadoDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ESS_EmpleadoEntidad> ListarEmpleado() {
            List<ESS_EmpleadoEntidad> lista = new List<ESS_EmpleadoEntidad>();
            string consulta = @"SELECT 
                             emp.[IdEmpleado]
                            ,emp.[IdEmpleadoSEG]
                            ,emp.[Nombre]
                            ,emp.[ApellidoPaterno]
                            ,emp.[ApellidoMaterno]
                            ,emp.[IdTipoDocumentoRegistro]
                            ,emp.[DocumentoRegistro]
                            ,emp.[IdCargo]
                            ,emp.[UsuarioRegistro]
                            ,emp.[UsuarioModificacion]
                            ,emp.[FechaModificacion]
                            ,emp.[FechaRegistro]
                            ,emp.[Estado]
                            ,emp.[IdEmpresaExterna]
                            ,cg.[Nombre] as NombreCargo
                              FROM [ESS_EmpleadoExterno] as emp
                              left join ESS_Cargo as cg
                              on emp.IdCargo = cg.IdCargo";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_EmpleadoEntidad {
                                IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                                IdEmpleadoSEG = ManejoNulos.ManageNullInteger(dr["IdEmpleadoSEG"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                IdTipoDocumentoRegistro = ManejoNulos.ManageNullInteger(dr["IdTipoDocumentoRegistro"]),
                                DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]),
                                IdCargo= ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                IdEmpresaExterna = ManejoNulos.ManageNullInteger(dr["IdEmpresaExterna"]),

                            };
                            var itemCargo = new ESS_CargoEntidad() {
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["NombreCargo"])
                            };
                            item.Cargo = itemCargo;
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                lista = new List<ESS_EmpleadoEntidad>();
            }
            return lista;
        }
        public List<ESS_EmpleadoEntidad> ListarEmpleadoPorEstado(int estado) {
            List<ESS_EmpleadoEntidad> lista = new List<ESS_EmpleadoEntidad>();
            string consulta = @"SELECT 
                             emp.[IdEmpleado]
                            ,emp.[IdEmpleadoSEG]
                            ,emp.[Nombre]
                            ,emp.[ApellidoPaterno]
                            ,emp.[ApellidoMaterno]
                            ,emp.[IdTipoDocumentoRegistro]
                            ,emp.[DocumentoRegistro]
                            ,CASE 
                                WHEN emp.[IdTipoDocumentoRegistro] = 1 THEN 'DNI'
                                WHEN emp.[IdTipoDocumentoRegistro] = 2 THEN 'PAS'
                                WHEN emp.[IdTipoDocumentoRegistro] = 3 THEN 'CE'
                                ELSE 'OTRO'
                            END as TipoDocumento
                            ,emp.[IdCargo]
                            ,emp.[UsuarioRegistro]
                            ,emp.[UsuarioModificacion]
                            ,emp.[FechaModificacion]
                            ,emp.[FechaRegistro]
                            ,emp.[Estado]
                            ,emp.[IdEmpresaExterna]
                            ,cg.[Nombre] as NombreCargo
                              FROM [ESS_EmpleadoExterno] as emp
                              left join ESS_Cargo as cg on emp.IdCargo = cg.IdCargo
                              left join TipoDOI as td ON emp.IdTipoDocumentoRegistro = td.DOIID
                              where emp.Estado = @Estado";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_EmpleadoEntidad {
                                IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                                IdEmpleadoSEG = ManejoNulos.ManageNullInteger(dr["IdEmpleadoSEG"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                IdTipoDocumentoRegistro = ManejoNulos.ManageNullInteger(dr["IdTipoDocumentoRegistro"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]),
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                IdEmpresaExterna = ManejoNulos.ManageNullInteger(dr["IdEmpresaExterna"]),
                            };
                            var itemCargo = new ESS_CargoEntidad() {
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["NombreCargo"])
                            };
                            item.Cargo = itemCargo;
                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                lista = new List<ESS_EmpleadoEntidad>();
            }
            return lista;
        }
        public ESS_EmpleadoEntidad ObtenerEmpleadoPorId(int id) {
            ESS_EmpleadoEntidad item = new ESS_EmpleadoEntidad();
            string consulta = @"SELECT [IdEmpleado]
      ,[IdEmpleadoSEG]
      ,[Nombre]
      ,[ApellidoPaterno]
      ,[ApellidoMaterno]
      ,[IdTipoDocumentoRegistro]
      ,[DocumentoRegistro]
      ,[IdCargo]
      ,[UsuarioRegistro]
      ,[UsuarioModificacion]
      ,[FechaModificacion]
      ,[FechaRegistro]
      ,[Estado]
      ,[IdEmpresaExterna]
  FROM [ESS_EmpleadoExterno] where Estado = @Estado where IdEmpleado = @IdEmpleado";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpleado", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]);
                                item.IdEmpleadoSEG = ManejoNulos.ManageNullInteger(dr["IdEmpleadoSEG"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]);
                                item.ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]);
                                item.IdTipoDocumentoRegistro = ManejoNulos.ManageNullInteger(dr["IdTipoDocumentoRegistro"]);
                                item.DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]);
                                item.IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]);
                                item.UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]);
                                item.UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.IdEmpresaExterna = ManejoNulos.ManageNullInteger(dr["IdEmpresaExterna"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                item = new ESS_EmpleadoEntidad();
            }
            return item;

        }
        public int InsertarEmpleado(ESS_EmpleadoEntidad model) {
            int IdInsertado = 0;

            string verificarConsulta = @"SELECT COUNT(1) 
                                 FROM [ESS_EmpleadoExterno] 
                                 WHERE DocumentoRegistro = @DocumentoRegistro";

            string consulta = @"INSERT INTO [ESS_EmpleadoExterno]
           ([IdEmpleadoSEG]
           ,[Nombre]
           ,[ApellidoPaterno]
           ,[ApellidoMaterno]
           ,[IdTipoDocumentoRegistro]
           ,[DocumentoRegistro]
           ,[IdCargo]
           ,[UsuarioRegistro]
           ,[FechaRegistro]
           ,[Estado]
           ,[IdEmpresaExterna])
OUTPUT INSERTED.IdEmpleado
     VALUES
           (@IdEmpleadoSEG
           ,@Nombre
           ,@ApellidoPaterno
           ,@ApellidoMaterno
           ,@IdTipoDocumentoRegistro
           ,@DocumentoRegistro
           ,@IdCargo
           ,@UsuarioRegistro
           ,@FechaRegistro
           ,@Estado
           ,@IdEmpresaExterna)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();

                    var verificarCmd = new SqlCommand(verificarConsulta, con);
                    verificarCmd.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(model.DocumentoRegistro).ToUpper().Trim());

                    int existe = Convert.ToInt32(verificarCmd.ExecuteScalar());
                    if(existe > 0) {
                        return -1;
                    }

                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpleadoSEG", ManejoNulos.ManageNullInteger(model.IdEmpleadoSEG));
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(model.ApellidoPaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(model.ApellidoMaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@IdTipoDocumentoRegistro", ManejoNulos.ManageNullInteger(model.IdTipoDocumentoRegistro));
                    query.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(model.DocumentoRegistro).ToUpper().Trim());
                    query.Parameters.AddWithValue("@IdCargo", ManejoNulos.ManageNullInteger(model.IdCargo));
                    query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(model.UsuarioRegistro));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(model.FechaRegistro));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@IdEmpresaExterna", ManejoNulos.ManageNullInteger(model.IdEmpresaExterna));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarEmpleado(ESS_EmpleadoEntidad model) {
            bool respuesta = false;
            string consulta = @"UPDATE [ESS_EmpleadoExterno]
   SET [IdEmpleadoSEG] = @IdEmpleadoSEG
      ,[Nombre] = @Nombre
      ,[ApellidoPaterno] = @ApellidoPaterno
      ,[ApellidoMaterno] = @ApellidoMaterno
      ,[IdTipoDocumentoRegistro] = @IdTipoDocumentoRegistro
      ,[DocumentoRegistro] = @DocumentoRegistro
      ,[IdCargo] = @IdCargo
      ,[UsuarioModificacion] = @UsuarioModificacion
      ,[FechaModificacion] = @FechaModificacion
      ,[Estado] = @Estado
                     WHERE IdEmpleado = @IdEmpleado";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpleadoSEG", ManejoNulos.ManageNullInteger(model.IdEmpleadoSEG));
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(model.ApellidoPaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(model.ApellidoMaterno).ToUpper().Trim());
                    query.Parameters.AddWithValue("@IdTipoDocumentoRegistro", ManejoNulos.ManageNullInteger(model.IdTipoDocumentoRegistro));
                    query.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(model.DocumentoRegistro).ToUpper().Trim());
                    query.Parameters.AddWithValue("@IdCargo", ManejoNulos.ManageNullInteger(model.IdCargo));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(model.UsuarioModificacion));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(model.FechaModificacion));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                respuesta = false;
            }
            return respuesta;
        }

        public List<ESS_EmpleadoEntidad> ObtenerEmpleadoExternoActivosPorTermino(int idempresa, string term) {
            List<ESS_EmpleadoEntidad> lista = new List<ESS_EmpleadoEntidad>();
            string consulta = $@"
                SELECT top 10 e.[IdEmpleado]
                  ,e.[IdTipoDocumentoRegistro]  
                  ,e.[DocumentoRegistro]
                  ,(e.[Nombre] + ' ' + e.[ApellidoPaterno] + ' ' + e.[ApellidoMaterno]) AS NombreCompleto
                  ,e.[Nombre]
                  ,e.[ApellidoPaterno]
                  ,e.[ApellidoMaterno]
                  ,e.[IdCargo]
                  ,e.[IdEmpresaExterna]
                  ,e.[Estado]
                  ,c.[Nombre] AS NombreCargo
                ,emp.[Nombre] AS NombreEmpresaExterna
                 ,td.[Nombre] AS NombreDocumento
              FROM [ESS_EmpleadoExterno] e
                    INNER JOIN [ESS_CargoExterno] c ON e.IdCargo = c.IdCargo
                    INNER JOIN [ESS_EmpresaExterna] emp ON e.IdEmpresaExterna = emp.IdEmpresaExterna
                    LEFT JOIN [AST_TipoDocumento] td ON e.IdTipoDocumentoRegistro = td.Id
                    where (e.Estado = 1) and e.IdEmpresaExterna = {idempresa}
                   AND ((e.[Nombre] + ' ' + e.[ApellidoPaterno] + ' ' + e.[ApellidoMaterno]) LIKE '%{term}%' 
                   OR e.[DocumentoRegistro] LIKE '%{term}%')
                 ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con); 
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_EmpleadoEntidad {
                                IdEmpleado = ManejoNulos.ManageNullInteger(dr["IdEmpleado"]),
                                IdTipoDocumentoRegistro = ManejoNulos.ManageNullInteger(dr["IdTipoDocumentoRegistro"]),
                                DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                                IdEmpresaExterna = ManejoNulos.ManageNullInteger(dr["IdEmpresaExterna"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NombreCargo = ManejoNulos.ManageNullStr(dr["NombreCargo"]),
                                NombreEmpresaExterna = ManejoNulos.ManageNullStr(dr["NombreEmpresaExterna"]),
                                NombreDocumento = ManejoNulos.ManageNullStr(dr["NombreDocumento"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                return new List<ESS_EmpleadoEntidad>();
            }
            return lista;
        }
    }
}
