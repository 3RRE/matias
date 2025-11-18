using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class Destinatario_DetalleDAL
    {
        string _conexion = string.Empty;
        public Destinatario_DetalleDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }       
        public bool DestinatarioDetalleInsertarJson(Destinatario_DetalleEntidad destinatarioDetalle)
        {
            bool response = false;
            string consulta = @"insert into Destinatario_Detalle(EmailID,TipoEmail,Activo) values(@p0,@p1,1)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", destinatarioDetalle.EmailID);
                    query.Parameters.AddWithValue("@p1", destinatarioDetalle.TipoEmail);
                    query.ExecuteNonQuery();
                    response= true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        public bool DestinatarioDetalleEliminarJson(int tipoEmail)
        {
            bool response = false;
            string consulta = @"delete from Destinatario_Detalle where TipoEmail=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tipoEmail);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
    }
}
