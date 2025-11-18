using CapaEntidad.ProgresivoRuleta.Dto;
using S3k.Utilitario;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.ProgresivoRuleta {
    public class PRU_RuletaDAL {
        private readonly string _conexion;

        public PRU_RuletaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<PRU_RuletaSelectDto> SeleccionarRuletasBySalaId(int salaId) {
            List<PRU_RuletaSelectDto> items = new List<PRU_RuletaSelectDto>();

            string query = @"
            SELECT
                rlt.IdRuleta AS Id,
                rlt.NombreRuleta AS Nombre
            FROM PRU_Configuracion rlt
            WHERE
                CodSala = @SalaId
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@SalaId", salaId);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        while(data.Read()) {
                            items.Add(new PRU_RuletaSelectDto {
                                Id = ManejoNulos.ManageNullInteger(data["Id"]),
                                Nombre = ManejoNulos.ManageNullStr(data["Nombre"])
                            });
                        }
                    }
                }
            } catch { }

            return items;
        }
    }
}
