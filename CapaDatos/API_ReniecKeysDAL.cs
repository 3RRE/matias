using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos {
    public class API_ReniecKeysDAL {
        string _conexion = string.Empty;
        public API_ReniecKeysDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<API_ReniecKeysEntidad> ListaKeys() {
            List<API_ReniecKeysEntidad> result = new List<API_ReniecKeysEntidad>();
                string consulta = $@"
                    SELECT [Id]
      ,[ApiKey]
      ,[ApiURL]
      ,[Estado]
  FROM [dbo].[API_ReniecKeys]
                ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new API_ReniecKeysEntidad {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                ApiKey= ManejoNulos.ManageNullStr(dr["ApiKey"]),
                                ApiURL= ManejoNulos.ManageNullStr(dr["ApiURL"]),
                                Estado= ManejoNulos.ManageNullInteger(dr["Estado"])
                            };
                            result.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                result = new List<API_ReniecKeysEntidad>();
            }
            return result;
        }
    }
}
