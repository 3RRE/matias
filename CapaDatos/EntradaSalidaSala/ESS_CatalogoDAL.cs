using CapaEntidad.EntradaSalidaSala;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Security.Cryptography;


namespace CapaDatos.EntradaSalidaSala {
    public class ESS_CatalogoDAL {
        string _conexion = string.Empty;
       
        public ESS_CatalogoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ESS_cboCatalogoEntidad> ListadoCatalogo(int IdTipoCatalogo) {
            List<ESS_cboCatalogoEntidad> lista = new List<ESS_cboCatalogoEntidad>();
            string consulta = @"SELECT                             
                                A.IdCatalogo as value,
                                A.Denominacion as label
                                FROM ESS_Catalogo AS A
                                JOIN ESS_TipoCatalogo B on B.IdTipoCatalogo = A.IdTipoCatalogo
                                where A.IdTipoCatalogo = "+ IdTipoCatalogo; 
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con); 
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_cboCatalogoEntidad {
                                value = ManejoNulos.ManageNullInteger(dr["value"]),
                                label = ManejoNulos.ManageNullStr(dr["label"]), 
                            };
                            lista.Add(item);
                        }
                    }
                }


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }
    }

}
 