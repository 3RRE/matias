using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad.ControlAcceso;
using S3k.Utilitario;

namespace CapaDatos.ControlAcceso
{
    public class CAL_BandaDAL
    {
        string conexion = string.Empty;
        public CAL_BandaDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_BandaEntidad> GetAllBanda()
        {
            List<CAL_BandaEntidad> lista = new List<CAL_BandaEntidad>();
            string consulta = @" SELECT em.BandaID ,em.Descripcion, em.CodUbigeo, 
(select  paisid from ubigeo u1 (nolock) where u1.PaisId =(select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId='000' and u1.ProvinciaId ='000' and u1.DistritoId='000') 'codpais' ,
(select  nombre from ubigeo u1 (nolock) where u1.PaisId =(select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId='000' and u1.ProvinciaId ='000' and u1.DistritoId='000') 'Pais' ,
case when (select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)<> '000'  then 
(select departamentoid from ubigeo u1 (nolock) where 	u1.PaisId  = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId =(select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)  and u1.ProvinciaId ='000' and u1.DistritoId='000') else '' end 'coddepartamento' 
,case when (select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)<> '000'  then 
(select REPLACE([nombre],'DEPARTAMENTO ','') as nombre from ubigeo u1 (nolock) where 	u1.PaisId  = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId =(select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)  and u1.ProvinciaId ='000' and u1.DistritoId='000') else '' end 'Departamento' 
,case when (select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <> '000' then 
(select ProvinciaId  from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DistritoId='000')
else '' end 'codprovincia' 

,case when (select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <> '000' then 
(select nombre from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DistritoId='000' )
else '' end 'Provincia' 
,case when (select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <>'000' then
 (select DistritoId from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and u1.DistritoId =(select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)  )
 else '' end 'coddistrito'
,case when (select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <>'000' then
 (select nombre from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and u1.DistritoId =(select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) )
 else '' end 'Distrito'
   ,em.FechaRegistro ,em.FechaModificacion,em.Estado FROM CAL_Banda em (nolock) ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new CAL_BandaEntidad
                            {
                                BandaID =  ManejoNulos.ManageNullInteger(dr["BandaID"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                Pais = ManejoNulos.ManageNullStr(dr["Pais"]),
                                Departamento = ManejoNulos.ManageNullStr(dr["Departamento"]),
                                Provincia = ManejoNulos.ManageNullStr(dr["Provincia"]),
                                Distrito = ManejoNulos.ManageNullStr(dr["Distrito"]),
                                codPais = ManejoNulos.ManageNullStr(dr["codPais"]),
                                codDepartamento = ManejoNulos.ManageNullStr(dr["codDepartamento"]),
                                codProvincia = ManejoNulos.ManageNullStr(dr["codProvincia"]),
                                codDistrito = ManejoNulos.ManageNullStr(dr["codDistrito"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
        public List<CAL_BandaEntidad> GetAllBandaActive()
        {
            List<CAL_BandaEntidad> lista = new List<CAL_BandaEntidad>();
            string consulta = @" SELECT em.BandaID ,em.Descripcion, em.CodUbigeo, 
(select  paisid from ubigeo u1 (nolock) where u1.PaisId =(select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId='000' and u1.ProvinciaId ='000' and u1.DistritoId='000' and Estado = 1) 'codpais' ,
(select  nombre from ubigeo u1 (nolock) where u1.PaisId =(select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId='000' and u1.ProvinciaId ='000' and u1.DistritoId='000' and Estado = 1) 'Pais' ,
case when (select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)<> '000'  then 
(select departamentoid from ubigeo u1 (nolock) where 	u1.PaisId  = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId =(select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)  and u1.ProvinciaId ='000' and u1.DistritoId='000' and Estado = 1) else '' end 'coddepartamento' 
,case when (select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)<> '000'  then 
(select nombre from ubigeo u1 (nolock) where 	u1.PaisId  = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId =(select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)  and u1.ProvinciaId ='000' and u1.DistritoId='000' and Estado = 1) else '' end 'Departamento' 
,case when (select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <> '000' then 
(select ProvinciaId  from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DistritoId='000' and Estado = 1 )
else '' end 'codprovincia' 

,case when (select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <> '000' then 
(select nombre from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DistritoId='000' and Estado = 1 )
else '' end 'Provincia' 
,case when (select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <>'000' then
 (select DistritoId from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and u1.DistritoId =(select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and Estado = 1  )
 else '' end 'coddistrito'
,case when (select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <>'000' then
 (select nombre from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and u1.DistritoId =(select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and Estado = 1  )
 else '' end 'Distrito'
   ,em.FechaRegistro ,em.FechaModificacion,em.Estado FROM CAL_Banda em (nolock) 
where 
 em.Estado = 1 ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new CAL_BandaEntidad
                            {
                                BandaID = ManejoNulos.ManageNullInteger(dr["BandaID"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]),
                                Pais = ManejoNulos.ManageNullStr(dr["Pais"]),
                                Departamento = ManejoNulos.ManageNullStr(dr["Departamento"]),
                                Provincia = ManejoNulos.ManageNullStr(dr["Provincia"]),
                                Distrito = ManejoNulos.ManageNullStr(dr["Distrito"]),
                                codPais = ManejoNulos.ManageNullStr(dr["codPais"]),
                                codDepartamento = ManejoNulos.ManageNullStr(dr["codDepartamento"]),
                                codProvincia = ManejoNulos.ManageNullStr(dr["codProvincia"]),
                                codDistrito = ManejoNulos.ManageNullStr(dr["codDistrito"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
        public CAL_BandaEntidad GetIDBanda(int id)
        {
            CAL_BandaEntidad item = new CAL_BandaEntidad();
            string consulta = @"SELECT em.BandaID ,em.Descripcion, em.CodUbigeo, 
(select  paisid from ubigeo u1 (nolock) where u1.PaisId =(select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId='000' and u1.ProvinciaId ='000' and u1.DistritoId='000') 'codpais' ,
(select  nombre from ubigeo u1 (nolock) where u1.PaisId =(select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId='000' and u1.ProvinciaId ='000' and u1.DistritoId='000') 'Pais' ,
case when (select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)<> '000'  then 
(select departamentoid from ubigeo u1 (nolock) where 	u1.PaisId  = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId =(select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)  and u1.ProvinciaId ='000' and u1.DistritoId='000') else '' end 'coddepartamento' 
,case when (select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)<> '000'  then 
(select nombre from ubigeo u1 (nolock) where 	u1.PaisId  = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and u1.DepartamentoId =(select u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)  and u1.ProvinciaId ='000' and u1.DistritoId='000') else '' end 'Departamento' 
,case when (select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <> '000' then 
(select ProvinciaId  from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DistritoId='000' )
else '' end 'codprovincia' 

,case when (select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <> '000' then 
(select nombre from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DistritoId='000' )
else '' end 'Provincia' 
,case when (select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <>'000' then
 (select DistritoId from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and u1.DistritoId =(select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo)  )
 else '' end 'coddistrito'
,case when (select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) <>'000' then
 (select nombre from ubigeo u1 (nolock) where u1.PaisId = (select  u.PaisId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.DepartamentoId =(select  u.DepartamentoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) and  u1.ProvinciaId =(select  u.ProvinciaId from ubigeo u (nolock) where u.CodUbigeo  =em.CodUbigeo) and u1.DistritoId =(select  u.DistritoId from ubigeo u (nolock) where u.CodUbigeo  = em.CodUbigeo) )
 else '' end 'Distrito'
   ,em.FechaRegistro ,em.FechaModificacion,em.Estado FROM CAL_Banda em (nolock) 
where BandaID = @pBandaID ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pBandaID", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.BandaID = ManejoNulos.ManageNullInteger(dr["BandaID"]);
                                item.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                item.CodUbigeo = ManejoNulos.ManageNullInteger(dr["CodUbigeo"]);
                                item.Pais = ManejoNulos.ManageNullStr(dr["Pais"]);
                                item.Departamento = ManejoNulos.ManageNullStr(dr["Departamento"]);
                                item.Provincia = ManejoNulos.ManageNullStr(dr["Provincia"]);
                                item.Distrito = ManejoNulos.ManageNullStr(dr["Distrito"]);
                                item.codPais = ManejoNulos.ManageNullStr(dr["codpais"]);
                                item.codDepartamento = ManejoNulos.ManageNullStr(dr["coddepartamento"]);
                                item.codProvincia = ManejoNulos.ManageNullStr(dr["codprovincia"]);
                                item.codDistrito = ManejoNulos.ManageNullStr(dr["coddistrito"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarBanda(CAL_BandaEntidad Entidad)
        {
            int IdInsertado = 0;
            string consulta = @" insert into CAL_Banda (Descripcion , CodUbigeo,FechaRegistro ,FechaModificacion ,Estado) 
                                OUTPUT Inserted.BandaID   
	                            values (@pDescripcion, @pCodUbigeo, @pFechaRegistro ,@pFechaModificacion ,@pEstado)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pCodUbigeo", ManejoNulos.ManageNullInteger(Entidad.CodUbigeo));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarBanda(CAL_BandaEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @"update CAL_Banda set  Descripcion = @pDescripcion ,CodUbigeo  = @pCodUbigeo ,FechaModificacion  = @pFechaModificacion  ,Estado  = @pEstado where  BandaID = @pBanda";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pBanda", ManejoNulos.ManageNullInteger(Entidad.BandaID));
                    query.Parameters.AddWithValue("@pCodUbigeo", ManejoNulos.ManageNullInteger(Entidad.CodUbigeo));
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarBanda(int id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_Banda 
                                WHERE BandaID  = @pBandaID";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pBandaID", ManejoNulos.ManageNullInteger(id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }




    }
}
