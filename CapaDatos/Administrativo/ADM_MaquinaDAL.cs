using CapaEntidad.Administrativo;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Administrativo
{
    public class ADM_MaquinaDAL
    {
        string _conexion = string.Empty;

        public ADM_MaquinaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ADM_MaquinaEntidad> GetListadoADM_MaquinaPorSala(int CodSala)
        {
            List<ADM_MaquinaEntidad> lista = new List<ADM_MaquinaEntidad>();
            string consulta = @"SELECT [CodMaquina]
      ,[CodJuego]
      ,[CodComparador]
      ,[CodModeloBilletero]
      ,[CodVolatilidad]
      ,[CodLinea]
      ,[CodTipoMaquina]
      ,[CodModeloHopper]
      ,[CodContrato]
      ,[CodClasificacion]
      ,[CodMueble]
      ,[CodSala]
      ,[CodEmpresa]
      ,[CodZona]
      ,[CodIsla]
      ,[CodPantalla]
      ,[CodMoneda]
      ,[CodFicha]
      ,[CodMedioJuego]
      ,[CodModeloMaquina]
      ,[CodAlmacen]
      ,[CodFormula]
      ,[CodEstadoMaquina]
      ,[CodMaquinaLey]
      ,[CodAlterno]
      ,[NroFabricacion]
      ,[FechaFabricacion]
      ,[FechaReconstruccion]
      ,[NroSerie]
      ,[ValorComercial]
      ,[CordX]
      ,[CordY]
      ,[Segmento]
      ,[Posicion]
      ,[ApuestaMaxima]
      ,[ApuestaMinima]
      ,[Hopper]
      ,[CreditoFicha]
      ,[Token]
      ,[FechaRegistro]
      ,[FechaModificacion]
      ,[Activo]
      ,[Estado]
      ,[CodTipoFicha]
      ,[PorcentajeDevolucion]
      ,[CodRD]
      ,[CodUsuario]
      ,[RetiroTemporal]
  FROM [dbo].[ADM_Maquina] where [CodSala]=@CodSala";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", CodSala);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var maquina = new ADM_MaquinaEntidad
                            {
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                CodJuego = ManejoNulos.ManageNullInteger(dr["CodJuego"]),
                                CodComparador = ManejoNulos.ManageNullInteger(dr["CodComparador"]),
                                CodModeloBilletero = ManejoNulos.ManageNullInteger(dr["CodModeloBilletero"]),
                                CodVolatilidad = ManejoNulos.ManageNullInteger(dr["CodVolatilidad"]),
                                CodLinea = ManejoNulos.ManageNullInteger(dr["CodLinea"]),
                                CodTipoMaquina = ManejoNulos.ManageNullInteger(dr["CodTipoMaquina"]),
                                CodModeloHopper = ManejoNulos.ManageNullInteger(dr["CodModeloHopper"]),
                                CodContrato = ManejoNulos.ManageNullInteger(dr["CodContrato"]),
                                CodClasificacion = ManejoNulos.ManageNullInteger(dr["CodClasificacion"]),
                                CodMueble = ManejoNulos.ManageNullInteger(dr["CodMueble"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodZona = ManejoNulos.ManageNullInteger(dr["CodZona"]),
                                CodIsla = ManejoNulos.ManageNullInteger(dr["CodIsla"]),
                                CodPantalla = ManejoNulos.ManageNullInteger(dr["CodPantalla"]),
                                CodMoneda = ManejoNulos.ManageNullInteger(dr["CodMoneda"]),
                                CodFicha = ManejoNulos.ManageNullInteger(dr["CodFicha"]),
                                CodMedioJuego = ManejoNulos.ManageNullInteger(dr["CodMedioJuego"]),
                                CodModeloMaquina = ManejoNulos.ManageNullInteger(dr["CodModeloMaquina"]),
                                CodAlmacen = ManejoNulos.ManageNullInteger(dr["CodAlmacen"]),
                                CodFormula = ManejoNulos.ManageNullInteger(dr["CodFormula"]),
                                CodEstadoMaquina = ManejoNulos.ManageNullInteger(dr["CodEstadoMaquina"]),
                                CodMaquinaLey = ManejoNulos.ManageNullStr(dr["CodMaquinaLey"]),
                                CodAlterno = ManejoNulos.ManageNullStr(dr["CodAlterno"]),
                                NroFabricacion = ManejoNulos.ManageNullStr(dr["NroFabricacion"]),
                                FechaFabricacion = ManejoNulos.ManageNullDate(dr["FechaFabricacion"]),
                                FechaReconstruccion = ManejoNulos.ManageNullDate(dr["FechaReconstruccion"]),
                                NroSerie = ManejoNulos.ManageNullStr(dr["NroSerie"]),
                                ValorComercial = ManejoNulos.ManageNullInteger(dr["ValorComercial"]),
                                CordX = ManejoNulos.ManageNullInteger(dr["CordX"]),
                                CordY = ManejoNulos.ManageNullInteger(dr["CordY"]),
                                Segmento = ManejoNulos.ManageNullInteger(dr["Segmento"]),
                                Posicion = ManejoNulos.ManageNullInteger(dr["Posicion"]),
                                ApuestaMaxima = ManejoNulos.ManageNullInteger(dr["ApuestaMaxima"]),
                                ApuestaMinima = ManejoNulos.ManageNullInteger(dr["ApuestaMinima"]),
                                Hopper = ManejoNulos.ManageNullInteger(dr["Hopper"]),
                                CreditoFicha = ManejoNulos.ManageNullInteger(dr["CreditoFicha"]),
                                Token = ManejoNulos.ManageNullDecimal(dr["Token"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodTipoFicha = ManejoNulos.ManageNullInteger(dr["CodTipoFicha"]),
                                PorcentajeDevolucion = ManejoNulos.ManageNullInteger(dr["PorcentajeDevolucion"]),
                                CodRD = ManejoNulos.ManageNullInteger(dr["CodRD"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                RetiroTemporal = ManejoNulos.ManageNullInteger(dr["RetiroTemporal"]),
                            };

                            lista.Add(maquina);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<ADM_MaquinaEntidad>();
            }
            return lista;
        }
        public bool EditarADM_Maquina(ADM_MaquinaEntidad maquina)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[ADM_Maquina]
   SET 
       [CodJuego] = @CodJuego
      ,[CodComparador] = @CodComparador
      ,[CodModeloBilletero] = @CodModeloBilletero
      ,[CodVolatilidad] = @CodVolatilidad
      ,[CodLinea] = @CodLinea
      ,[CodTipoMaquina] = @CodTipoMaquina
      ,[CodModeloHopper] = @CodModeloHopper
      ,[CodContrato] = @CodContrato
      ,[CodClasificacion] = @CodClasificacion
      ,[CodMueble] = @CodMueble
      ,[CodSala] = @CodSala
      ,[CodEmpresa] = @CodEmpresa
      ,[CodZona] = @CodZona
      ,[CodIsla] = @CodIsla
      ,[CodPantalla] = @CodPantalla
      ,[CodMoneda] = @CodMoneda
      ,[CodFicha] = @CodFicha
      ,[CodMedioJuego] = @CodMedioJuego
      ,[CodModeloMaquina] = @CodModeloMaquina
      ,[CodAlmacen] = @CodAlmacen
      ,[CodFormula] = @CodFormula
      ,[CodEstadoMaquina] = @CodEstadoMaquina
      ,[CodMaquinaLey] = @CodMaquinaLey
      ,[CodAlterno] = @CodAlterno
      ,[NroFabricacion] = @NroFabricacion
      ,[FechaFabricacion] = @FechaFabricacion
      ,[FechaReconstruccion] = @FechaReconstruccion
      ,[NroSerie] = @NroSerie
      ,[ValorComercial] = @ValorComercial
      ,[CordX] = @CordX
      ,[CordY] = @CordY
      ,[Segmento] = @Segmento
      ,[Posicion] = @Posicion
      ,[ApuestaMaxima] = @ApuestaMaxima
      ,[ApuestaMinima] = @ApuestaMinima
      ,[Hopper] = @Hopper
      ,[CreditoFicha] = @CreditoFicha
      ,[Token] = @Token
      ,[FechaRegistro] = @FechaRegistro
      ,[FechaModificacion] = @FechaModificacion
      ,[Activo] = @Activo
      ,[Estado] = @Estado
      ,[CodTipoFicha] = @CodTipoFicha
      ,[PorcentajeDevolucion] = @PorcentajeDevolucion
      ,[CodRD] = @CodRD
      ,[CodUsuario] = @CodUsuario
      ,[RetiroTemporal] = @RetiroTemporal
 WHERE CodMaquina=@CodMaquina                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@CodJuego", ManejoNulos.ManageNullInteger(maquina.CodJuego));
                    query.Parameters.AddWithValue("@CodComparador", ManejoNulos.ManageNullInteger(maquina.CodComparador));
                    query.Parameters.AddWithValue("@CodModeloBilletero", ManejoNulos.ManageNullInteger(maquina.CodModeloBilletero));
                    query.Parameters.AddWithValue("@CodVolatilidad", ManejoNulos.ManageNullInteger(maquina.CodVolatilidad));
                    query.Parameters.AddWithValue("@CodLinea", ManejoNulos.ManageNullInteger(maquina.CodLinea));
                    query.Parameters.AddWithValue("@CodTipoMaquina", ManejoNulos.ManageNullInteger(maquina.CodTipoMaquina));
                    query.Parameters.AddWithValue("@CodModeloHopper", ManejoNulos.ManageNullInteger(maquina.CodModeloHopper));
                    query.Parameters.AddWithValue("@CodContrato", ManejoNulos.ManageNullInteger(maquina.CodContrato));
                    query.Parameters.AddWithValue("@CodClasificacion", ManejoNulos.ManageNullInteger(maquina.CodClasificacion));
                    query.Parameters.AddWithValue("@CodMueble", ManejoNulos.ManageNullInteger(maquina.CodMueble));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(maquina.CodSala));
                    query.Parameters.AddWithValue("@CodEmpresa", ManejoNulos.ManageNullInteger(maquina.CodEmpresa));
                    query.Parameters.AddWithValue("@CodZona", ManejoNulos.ManageNullInteger(maquina.CodZona));
                    query.Parameters.AddWithValue("@CodIsla", ManejoNulos.ManageNullInteger(maquina.CodIsla));
                    query.Parameters.AddWithValue("@CodPantalla", ManejoNulos.ManageNullInteger(maquina.CodPantalla));
                    query.Parameters.AddWithValue("@CodMoneda", ManejoNulos.ManageNullInteger(maquina.CodMoneda));
                    query.Parameters.AddWithValue("@CodFicha", ManejoNulos.ManageNullInteger(maquina.CodFicha));
                    query.Parameters.AddWithValue("@CodMedioJuego", ManejoNulos.ManageNullInteger(maquina.CodMedioJuego));
                    //query.Parameters.AddWithValue("@CodModeloMaquina", ManejoNulos.ManageNullInteger(maquina.CodModeloMaquina==0?SqlInt32.Null:maquina.CodModeloMaquina));
                    query.Parameters.AddWithValue("@CodModeloMaquina", maquina.CodModeloMaquina == 0 ? SqlInt32.Null : maquina.CodModeloMaquina);
                    query.Parameters.AddWithValue("@CodAlmacen", ManejoNulos.ManageNullInteger(maquina.CodAlmacen));
                    query.Parameters.AddWithValue("@CodFormula", ManejoNulos.ManageNullInteger(maquina.CodFormula));
                    query.Parameters.AddWithValue("@CodEstadoMaquina", ManejoNulos.ManageNullInteger(maquina.CodEstadoMaquina));
                    query.Parameters.AddWithValue("@CodMaquinaLey", ManejoNulos.ManageNullStr(maquina.CodMaquinaLey));
                    query.Parameters.AddWithValue("@CodAlterno", ManejoNulos.ManageNullStr(maquina.CodAlterno));
                    query.Parameters.AddWithValue("@NroFabricacion", ManejoNulos.ManageNullStr(maquina.NroFabricacion));
                    query.Parameters.AddWithValue("@FechaFabricacion", ManejoNulos.ManageNullDate(maquina.FechaFabricacion));
                    query.Parameters.AddWithValue("@FechaReconstruccion", ManejoNulos.ManageNullDate(maquina.FechaReconstruccion));
                    query.Parameters.AddWithValue("@NroSerie", ManejoNulos.ManageNullStr(maquina.NroSerie));
                    query.Parameters.AddWithValue("@ValorComercial", ManejoNulos.ManageNullDecimal(maquina.ValorComercial));
                    query.Parameters.AddWithValue("@CordX", ManejoNulos.ManageNullInteger(maquina.CordX));
                    query.Parameters.AddWithValue("@CordY", ManejoNulos.ManageNullInteger(maquina.CordY));
                    query.Parameters.AddWithValue("@Segmento", ManejoNulos.ManageNullInteger(maquina.Segmento));
                    query.Parameters.AddWithValue("@Posicion", ManejoNulos.ManageNullInteger(maquina.Posicion));
                    query.Parameters.AddWithValue("@ApuestaMaxima", ManejoNulos.ManageNullDecimal(maquina.ApuestaMaxima));
                    query.Parameters.AddWithValue("@ApuestaMinima", ManejoNulos.ManageNullDecimal(maquina.ApuestaMinima));
                    query.Parameters.AddWithValue("@Hopper", ManejoNulos.ManageNullInteger(maquina.Hopper));
                    query.Parameters.AddWithValue("@CreditoFicha", ManejoNulos.ManageNullDecimal(maquina.CreditoFicha));
                    query.Parameters.AddWithValue("@Token", ManejoNulos.ManageNullDecimal(maquina.Token));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(maquina.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(maquina.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(maquina.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(maquina.Estado));
                    query.Parameters.AddWithValue("@CodTipoFicha", ManejoNulos.ManageNullInteger(maquina.CodTipoFicha));
                    query.Parameters.AddWithValue("@PorcentajeDevolucion", ManejoNulos.ManageNullDecimal(maquina.PorcentajeDevolucion));
                    query.Parameters.AddWithValue("@CodRD", ManejoNulos.ManageNullInteger(maquina.CodRD));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(maquina.CodUsuario));
                    query.Parameters.AddWithValue("@RetiroTemporal", ManejoNulos.ManageNullInteger(maquina.RetiroTemporal));
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullInteger(maquina.CodMaquina));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }

            return respuesta;
        }
        public int GuardarADM_Maquina(ADM_MaquinaEntidad maquina)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[ADM_Maquina]
           ([CodJuego]
           ,[CodComparador]
           ,[CodModeloBilletero]
           ,[CodVolatilidad]
           ,[CodLinea]
           ,[CodTipoMaquina]
           ,[CodModeloHopper]
           ,[CodContrato]
           ,[CodClasificacion]
           ,[CodMueble]
           ,[CodSala]
           ,[CodEmpresa]
           ,[CodZona]
           ,[CodIsla]
           ,[CodPantalla]
           ,[CodMoneda]
           ,[CodFicha]
           ,[CodMedioJuego]
           ,[CodModeloMaquina]
           ,[CodAlmacen]
           ,[CodFormula]
           ,[CodEstadoMaquina]
           ,[CodMaquinaLey]
           ,[CodAlterno]
           ,[NroFabricacion]
           ,[FechaFabricacion]
           ,[FechaReconstruccion]
           ,[NroSerie]
           ,[ValorComercial]
           ,[CordX]
           ,[CordY]
           ,[Segmento]
           ,[Posicion]
           ,[ApuestaMaxima]
           ,[ApuestaMinima]
           ,[Hopper]
           ,[CreditoFicha]
           ,[Token]
           ,[FechaRegistro]
           ,[FechaModificacion]
           ,[Activo]
           ,[Estado]
           ,[CodTipoFicha]
           ,[PorcentajeDevolucion]
           ,[CodRD]
           ,[CodUsuario]
           ,[RetiroTemporal])
Output Inserted.CodMaquina
     VALUES
           (
            @CodJuego
           ,@CodComparador
           ,@CodModeloBilletero
           ,@CodVolatilidad
           ,@CodLinea
           ,@CodTipoMaquina
           ,@CodModeloHopper
           ,@CodContrato
           ,@CodClasificacion
           ,@CodMueble
           ,@CodSala
           ,@CodEmpresa
           ,@CodZona
           ,@CodIsla
           ,@CodPantalla
           ,@CodMoneda
           ,@CodFicha
           ,@CodMedioJuego
           ,@CodModeloMaquina
           ,@CodAlmacen
           ,@CodFormula
           ,@CodEstadoMaquina
           ,@CodMaquinaLey
           ,@CodAlterno
           ,@NroFabricacion
           ,@FechaFabricacion
           ,@FechaReconstruccion
           ,@NroSerie
           ,@ValorComercial
           ,@CordX
           ,@CordY
           ,@Segmento
           ,@Posicion
           ,@ApuestaMaxima
           ,@ApuestaMinima
           ,@Hopper
           ,@CreditoFicha
           ,@Token
           ,@FechaRegistro
           ,@FechaModificacion
           ,@Activo
           ,@Estado
           ,@CodTipoFicha
           ,@PorcentajeDevolucion
           ,@CodRD
           ,@CodUsuario
           ,@RetiroTemporal)
;";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodJuego", ManejoNulos.ManageNullInteger(maquina.CodJuego));
                    query.Parameters.AddWithValue("@CodComparador", ManejoNulos.ManageNullInteger(maquina.CodComparador));
                    query.Parameters.AddWithValue("@CodModeloBilletero", ManejoNulos.ManageNullInteger(maquina.CodModeloBilletero));
                    query.Parameters.AddWithValue("@CodVolatilidad", ManejoNulos.ManageNullInteger(maquina.CodVolatilidad));
                    query.Parameters.AddWithValue("@CodLinea", ManejoNulos.ManageNullInteger(maquina.CodLinea));
                    query.Parameters.AddWithValue("@CodTipoMaquina", ManejoNulos.ManageNullInteger(maquina.CodTipoMaquina));
                    query.Parameters.AddWithValue("@CodModeloHopper", ManejoNulos.ManageNullInteger(maquina.CodModeloHopper));
                    query.Parameters.AddWithValue("@CodContrato", ManejoNulos.ManageNullInteger(maquina.CodContrato));
                    query.Parameters.AddWithValue("@CodClasificacion", ManejoNulos.ManageNullInteger(maquina.CodClasificacion));
                    query.Parameters.AddWithValue("@CodMueble", ManejoNulos.ManageNullInteger(maquina.CodMueble));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(maquina.CodSala));
                    query.Parameters.AddWithValue("@CodEmpresa", ManejoNulos.ManageNullInteger(maquina.CodEmpresa));
                    query.Parameters.AddWithValue("@CodZona", ManejoNulos.ManageNullInteger(maquina.CodZona));
                    query.Parameters.AddWithValue("@CodIsla", ManejoNulos.ManageNullInteger(maquina.CodIsla));
                    query.Parameters.AddWithValue("@CodPantalla", ManejoNulos.ManageNullInteger(maquina.CodPantalla));
                    query.Parameters.AddWithValue("@CodMoneda", ManejoNulos.ManageNullInteger(maquina.CodMoneda));
                    query.Parameters.AddWithValue("@CodFicha", ManejoNulos.ManageNullInteger(maquina.CodFicha));
                    query.Parameters.AddWithValue("@CodMedioJuego", ManejoNulos.ManageNullInteger(maquina.CodMedioJuego));
                    //query.Parameters.AddWithValue("@CodModeloMaquina", ManejoNulos.ManageNullInteger(maquina.CodModeloMaquina==0?SqlInt32.Null:maquina.CodModeloMaquina));
                    query.Parameters.AddWithValue("@CodModeloMaquina", maquina.CodModeloMaquina==0?SqlInt32.Null:maquina.CodModeloMaquina);
                    query.Parameters.AddWithValue("@CodAlmacen", ManejoNulos.ManageNullInteger(maquina.CodAlmacen));
                    query.Parameters.AddWithValue("@CodFormula", ManejoNulos.ManageNullInteger(maquina.CodFormula));
                    query.Parameters.AddWithValue("@CodEstadoMaquina", ManejoNulos.ManageNullInteger(maquina.CodEstadoMaquina));
                    query.Parameters.AddWithValue("@CodMaquinaLey", ManejoNulos.ManageNullStr(maquina.CodMaquinaLey));
                    query.Parameters.AddWithValue("@CodAlterno", ManejoNulos.ManageNullStr(maquina.CodAlterno));
                    query.Parameters.AddWithValue("@NroFabricacion", ManejoNulos.ManageNullStr(maquina.NroFabricacion));
                    query.Parameters.AddWithValue("@FechaFabricacion", ManejoNulos.ManageNullDate(maquina.FechaFabricacion));
                    query.Parameters.AddWithValue("@FechaReconstruccion", ManejoNulos.ManageNullDate(maquina.FechaReconstruccion));
                    query.Parameters.AddWithValue("@NroSerie", ManejoNulos.ManageNullStr(maquina.NroSerie));
                    query.Parameters.AddWithValue("@ValorComercial", ManejoNulos.ManageNullDecimal(maquina.ValorComercial));
                    query.Parameters.AddWithValue("@CordX", ManejoNulos.ManageNullInteger(maquina.CordX));
                    query.Parameters.AddWithValue("@CordY", ManejoNulos.ManageNullInteger(maquina.CordY));
                    query.Parameters.AddWithValue("@Segmento", ManejoNulos.ManageNullInteger(maquina.Segmento));
                    query.Parameters.AddWithValue("@Posicion", ManejoNulos.ManageNullInteger(maquina.Posicion));
                    query.Parameters.AddWithValue("@ApuestaMaxima", ManejoNulos.ManageNullDecimal(maquina.ApuestaMaxima));
                    query.Parameters.AddWithValue("@ApuestaMinima", ManejoNulos.ManageNullDecimal(maquina.ApuestaMinima));
                    query.Parameters.AddWithValue("@Hopper", ManejoNulos.ManageNullInteger(maquina.Hopper));
                    query.Parameters.AddWithValue("@CreditoFicha", ManejoNulos.ManageNullDecimal(maquina.CreditoFicha));
                    query.Parameters.AddWithValue("@Token", ManejoNulos.ManageNullDecimal(maquina.Token));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(maquina.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(maquina.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(maquina.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(maquina.Estado));
                    query.Parameters.AddWithValue("@CodTipoFicha", ManejoNulos.ManageNullInteger(maquina.CodTipoFicha));
                    query.Parameters.AddWithValue("@PorcentajeDevolucion", ManejoNulos.ManageNullDecimal(maquina.PorcentajeDevolucion));
                    query.Parameters.AddWithValue("@CodRD", ManejoNulos.ManageNullInteger(maquina.CodRD));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(maquina.CodUsuario));
                    query.Parameters.AddWithValue("@RetiroTemporal", ManejoNulos.ManageNullInteger(maquina.RetiroTemporal));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                IdInsertado = 0;
            }
            return IdInsertado;
        }


        public List<ADM_MaquinaEntidad> GetListadoMaquinasActivasPorSala(int codSala) {
            var lista = new List<ADM_MaquinaEntidad>();
            const string consulta = @"
SELECT  [CodMaquina],
        [CodSala],
        [CodMaquinaLey],
        [CodAlterno],
        [FechaRegistro],
        [FechaModificacion],
        [Activo],
        [Estado],
        [CodUsuario]
FROM [dbo].[ADM_Maquina]
WHERE [CodSala] = @CodSala
  AND [Activo] = 1
  AND [Estado] = 1";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@CodSala", codSala);

                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                lista.Add(new ADM_MaquinaEntidad {
                                    CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    CodMaquinaLey = ManejoNulos.ManageNullStr(dr["CodMaquinaLey"]),
                                    CodAlterno = ManejoNulos.ManageNullStr(dr["CodAlterno"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                    FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                    Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                    Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                    CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"])
                                });
                            }
                        }
                    }
                }
            } catch {
                return new List<ADM_MaquinaEntidad>();
            }
            return lista;
        }
        public List<ADM_MaquinaEntidad> GetListadoMaquinasPorIds(IEnumerable<int> ids) {
            var idList = (ids ?? Enumerable.Empty<int>()).Distinct().ToList();
            if(!idList.Any())
                return new List<ADM_MaquinaEntidad>();

            var paramNames = idList.Select((_, i) => "@p" + i).ToList();
            var sql = $@"
                SELECT  [CodMaquina],
                        [CodSala],
                        [CodMaquinaLey],
                        [CodAlterno],
                        [Activo],
                        [Estado]
                FROM [dbo].[ADM_Maquina]
                WHERE [CodMaquina] IN ({string.Join(",", paramNames)})
                ";

            var lista = new List<ADM_MaquinaEntidad>();
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(sql, con)) {
                        for(int i = 0; i < idList.Count; i++)
                            cmd.Parameters.AddWithValue(paramNames[i], idList[i]);

                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                lista.Add(new ADM_MaquinaEntidad {
                                    CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    CodMaquinaLey = ManejoNulos.ManageNullStr(dr["CodMaquinaLey"]),
                                    CodAlterno = ManejoNulos.ManageNullStr(dr["CodAlterno"]),
                                    Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                    Estado = ManejoNulos.ManageNullInteger(dr["Estado"])
                                });
                            }
                        }
                    }
                }
            } catch {
                return new List<ADM_MaquinaEntidad>();
            }
            return lista;
        }

    }
}
