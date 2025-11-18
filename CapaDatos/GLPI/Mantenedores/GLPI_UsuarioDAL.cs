using CapaEntidad.GLPI.Mantenedores;
using S3k.Utilitario;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI.Mantenedores {
    public class GLPI_UsuarioDAL {
        private readonly string _conexion;

        public GLPI_UsuarioDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<GLPI_Usuario> ObtenerUsuariosPorAccion(string accionTicket) {
            List<GLPI_Usuario> items = new List<GLPI_Usuario>();
            string consulta = @"
                SELECT 	                
	                usu.UsuarioID AS Id,
	                emp.Nombres,
	                emp.ApellidosPaterno,
	                emp.ApellidosMaterno,
	                emp.DOI AS NumeroDocumento,
	                car.Descripcion AS NombreCargo,
	                usu.UsuarioNombre AS NombreUsuario
                FROM SEG_Usuario AS usu
                INNER JOIN SEG_Empleado AS emp ON emp.EmpleadoID = usu.EmpleadoID
                INNER JOIN SEG_Cargo AS car ON car.CargoID = emp.CargoID
                INNER JOIN SEG_RolUsuario AS rolusu ON rolusu.UsuarioID = usu.UsuarioID
                INNER JOIN SEG_Rol AS rol ON rol.WEB_RolID = rolusu.WEB_RolID
                INNER JOIN SEG_PermisoRol AS pr ON pr.WEB_RolID = rol.WEB_RolID
                INNER JOIN SEG_Permiso AS p ON p.WEB_PermID = pr.WEB_PermID
                WHERE rol.WEB_RolEstado = 1 AND p.WEB_PermNombre = @Accion
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Accion", accionTicket);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        private GLPI_Usuario ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_Usuario {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidosPaterno"]),
                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidosMaterno"]),
                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                NombreCargo = ManejoNulos.ManageNullStr(dr["NombreCargo"]),
            };
        }
    }
}
