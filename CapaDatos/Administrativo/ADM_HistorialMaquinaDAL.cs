using CapaEntidad.Administrativo;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Administrativo
{
    public class ADM_HistorialMaquinaDAL
    {
        string _conexion = string.Empty;

        public ADM_HistorialMaquinaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ADM_HistorialMaquinaEntidad> GetListadoADM_HistorialMaquinaPorSala(int CodSala)
        {
            List<ADM_HistorialMaquinaEntidad> lista = new List<ADM_HistorialMaquinaEntidad>();
            string consulta = @"
SELECT [CodHistorialMaquina]
      ,[CodMaquina]
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
      ,[FechaOperacionIni]
      ,[FechaOperacionFin]
      ,[ResumenCambios]
      ,[CodUsuario]
  FROM [ADM_HistorialMaquina] where [CodSala]=@CodSala and Activo=1";
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
                            var maquina = new ADM_HistorialMaquinaEntidad
                            {
                                CodHistorialMaquina = ManejoNulos.ManageNullInteger(dr["CodHistorialMaquina"]),
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
                                ValorComercial = ManejoNulos.ManageNullDecimal(dr["ValorComercial"]),
                                CordX = ManejoNulos.ManageNullInteger(dr["CordX"]),
                                CordY = ManejoNulos.ManageNullInteger(dr["CordY"]),
                                Segmento = ManejoNulos.ManageNullInteger(dr["Segmento"]),
                                Posicion = ManejoNulos.ManageNullInteger(dr["Posicion"]),
                                ApuestaMaxima = ManejoNulos.ManageNullDecimal(dr["ApuestaMaxima"]),
                                ApuestaMinima = ManejoNulos.ManageNullDecimal(dr["ApuestaMinima"]),
                                Hopper = ManejoNulos.ManageNullInteger(dr["Hopper"]),
                                CreditoFicha = ManejoNulos.ManageNullDecimal(dr["CreditoFicha"]),
                                Token = ManejoNulos.ManageNullDecimal(dr["Token"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodTipoFicha = ManejoNulos.ManageNullInteger(dr["CodTipoFicha"]),
                                PorcentajeDevolucion = ManejoNulos.ManageNullDecimal(dr["PorcentajeDevolucion"]),
                                FechaOperacionIni = ManejoNulos.ManageNullDate(dr["FechaOperacionIni"]),
                                FechaOperacionFin = ManejoNulos.ManageNullDate(dr["FechaOperacionFin"]),
                                ResumenCambios = ManejoNulos.ManageNullStr(dr["ResumenCambios"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                            };

                            lista.Add(maquina);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<ADM_HistorialMaquinaEntidad>();
            }
            return lista;
        }
        public int GuardarADM_HistorialMaquina(ADM_HistorialMaquinaEntidad historial)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[ADM_HistorialMaquina]
           ([CodMaquina]
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
           ,[FechaOperacionIni]
           ,[FechaOperacionFin]
           ,[ResumenCambios]
           ,[CodUsuario])
Output Inserted.CodHistorialMaquina
     VALUES
           (@CodMaquina
           ,@CodJuego
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
           ,@FechaOperacionIni
           ,@FechaOperacionFin
           ,@ResumenCambios
           ,@CodUsuario)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullInteger(historial.CodMaquina));
                    query.Parameters.AddWithValue("@CodJuego", ManejoNulos.ManageNullInteger(historial.CodJuego));
                    query.Parameters.AddWithValue("@CodComparador", ManejoNulos.ManageNullInteger(historial.CodComparador));
                    query.Parameters.AddWithValue("@CodModeloBilletero", ManejoNulos.ManageNullInteger(historial.CodModeloBilletero));
                    query.Parameters.AddWithValue("@CodVolatilidad", ManejoNulos.ManageNullInteger(historial.CodVolatilidad));
                    query.Parameters.AddWithValue("@CodLinea", ManejoNulos.ManageNullInteger(historial.CodLinea));
                    query.Parameters.AddWithValue("@CodTipoMaquina", ManejoNulos.ManageNullInteger(historial.CodTipoMaquina));
                    query.Parameters.AddWithValue("@CodModeloHopper", ManejoNulos.ManageNullInteger(historial.CodModeloHopper));
                    query.Parameters.AddWithValue("@CodContrato", ManejoNulos.ManageNullInteger(historial.CodContrato));
                    query.Parameters.AddWithValue("@CodClasificacion", ManejoNulos.ManageNullInteger(historial.CodClasificacion));
                    query.Parameters.AddWithValue("@CodMueble", ManejoNulos.ManageNullInteger(historial.CodMueble));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(historial.CodSala));
                    query.Parameters.AddWithValue("@CodEmpresa", ManejoNulos.ManageNullInteger(historial.CodEmpresa));
                    query.Parameters.AddWithValue("@CodZona", ManejoNulos.ManageNullInteger(historial.CodZona));
                    query.Parameters.AddWithValue("@CodIsla", ManejoNulos.ManageNullInteger(historial.CodIsla));
                    query.Parameters.AddWithValue("@CodPantalla", ManejoNulos.ManageNullInteger(historial.CodPantalla));
                    query.Parameters.AddWithValue("@CodMoneda", ManejoNulos.ManageNullInteger(historial.CodMoneda));
                    query.Parameters.AddWithValue("@CodFicha", ManejoNulos.ManageNullInteger(historial.CodFicha));
                    query.Parameters.AddWithValue("@CodMedioJuego", ManejoNulos.ManageNullInteger(historial.CodMedioJuego));
                    query.Parameters.AddWithValue("@CodModeloMaquina", ManejoNulos.ManageNullInteger(historial.CodModeloMaquina));
                    query.Parameters.AddWithValue("@CodAlmacen", ManejoNulos.ManageNullInteger(historial.CodAlmacen));
                    query.Parameters.AddWithValue("@CodFormula", ManejoNulos.ManageNullInteger(historial.CodFormula));
                    query.Parameters.AddWithValue("@CodEstadoMaquina", ManejoNulos.ManageNullInteger(historial.CodEstadoMaquina));
                    query.Parameters.AddWithValue("@CodMaquinaLey", ManejoNulos.ManageNullStr(historial.CodMaquinaLey));
                    query.Parameters.AddWithValue("@CodAlterno", ManejoNulos.ManageNullStr(historial.CodAlterno));
                    query.Parameters.AddWithValue("@NroFabricacion", ManejoNulos.ManageNullStr(historial.NroFabricacion));
                    query.Parameters.AddWithValue("@FechaFabricacion", ManejoNulos.ManageNullDate(historial.FechaFabricacion));
                    query.Parameters.AddWithValue("@FechaReconstruccion", ManejoNulos.ManageNullDate(historial.FechaReconstruccion));
                    query.Parameters.AddWithValue("@NroSerie", ManejoNulos.ManageNullStr(historial.NroSerie));
                    query.Parameters.AddWithValue("@ValorComercial", ManejoNulos.ManageNullDecimal(historial.ValorComercial));
                    query.Parameters.AddWithValue("@CordX", ManejoNulos.ManageNullInteger(historial.CordX));
                    query.Parameters.AddWithValue("@CordY", ManejoNulos.ManageNullInteger(historial.CordY));
                    query.Parameters.AddWithValue("@Segmento", ManejoNulos.ManageNullInteger(historial.Segmento));
                    query.Parameters.AddWithValue("@Posicion", ManejoNulos.ManageNullInteger(historial.Posicion));
                    query.Parameters.AddWithValue("@ApuestaMaxima", ManejoNulos.ManageNullDecimal(historial.ApuestaMaxima));
                    query.Parameters.AddWithValue("@ApuestaMinima", ManejoNulos.ManageNullDecimal(historial.ApuestaMinima));
                    query.Parameters.AddWithValue("@Hopper", ManejoNulos.ManageNullInteger(historial.Hopper));
                    query.Parameters.AddWithValue("@CreditoFicha", ManejoNulos.ManageNullDecimal(historial.CreditoFicha));
                    query.Parameters.AddWithValue("@Token", ManejoNulos.ManageNullDecimal(historial.Token));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(historial.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(historial.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(historial.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(historial.Estado));
                    query.Parameters.AddWithValue("@CodTipoFicha", ManejoNulos.ManageNullInteger(historial.CodTipoFicha));
                    query.Parameters.AddWithValue("@PorcentajeDevolucion", ManejoNulos.ManageNullDecimal(historial.PorcentajeDevolucion));
                    query.Parameters.AddWithValue("@FechaOperacionIni", ManejoNulos.ManageNullDate(historial.FechaOperacionIni));
                    query.Parameters.AddWithValue("@FechaOperacionFin", ManejoNulos.ManageNullDate(historial.FechaOperacionFin));
                    query.Parameters.AddWithValue("@ResumenCambios", ManejoNulos.ManageNullStr(historial.ResumenCambios));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(historial.CodUsuario));
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
        public bool EditarADM_HistorialMaquina(ADM_HistorialMaquinaEntidad historial)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[ADM_HistorialMaquina]
   SET 
CodMaquina=@CodMaquina,
CodJuego=@CodJuego,
CodComparador=@CodComparador,
CodModeloBilletero=@CodModeloBilletero,
CodVolatilidad=@CodVolatilidad,
CodLinea=@CodLinea,
CodTipoMaquina=@CodTipoMaquina,
CodModeloHopper=@CodModeloHopper,
CodContrato=@CodContrato,
CodClasificacion=@CodClasificacion,
CodMueble=@CodMueble,
CodSala=@CodSala,
CodEmpresa=@CodEmpresa,
CodZona=@CodZona,
CodIsla=@CodIsla,
CodPantalla=@CodPantalla,
CodMoneda=@CodMoneda,
CodFicha=@CodFicha,
CodMedioJuego=@CodMedioJuego,
CodModeloMaquina=@CodModeloMaquina,
CodAlmacen=@CodAlmacen,
CodFormula=@CodFormula,
CodEstadoMaquina=@CodEstadoMaquina,
CodMaquinaLey=@CodMaquinaLey,
CodAlterno=@CodAlterno,
NroFabricacion=@NroFabricacion,
FechaFabricacion=@FechaFabricacion,
FechaReconstruccion=@FechaReconstruccion,
NroSerie=@NroSerie,
ValorComercial=@ValorComercial,
CordX=@CordX,
CordY=@CordY,
Segmento=@Segmento,
Posicion=@Posicion,
ApuestaMaxima=@ApuestaMaxima,
ApuestaMinima=@ApuestaMinima,
Hopper=@Hopper,
CreditoFicha=@CreditoFicha,
Token=@Token,
FechaRegistro=@FechaRegistro,
FechaModificacion=@FechaModificacion,
Activo=@Activo,
Estado=@Estado,
CodTipoFicha=@CodTipoFicha,
PorcentajeDevolucion=@PorcentajeDevolucion,
FechaOperacionIni=@FechaOperacionIni,
FechaOperacionFin=@FechaOperacionFin,
ResumenCambios=@ResumenCambios,
CodUsuario=@CodUsuario
 WHERE CodHistorialMaquina=@CodHistorialMaquina
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullInteger(historial.CodMaquina));
                    query.Parameters.AddWithValue("@CodJuego", ManejoNulos.ManageNullInteger(historial.CodJuego));
                    query.Parameters.AddWithValue("@CodComparador", ManejoNulos.ManageNullInteger(historial.CodComparador));
                    query.Parameters.AddWithValue("@CodModeloBilletero", ManejoNulos.ManageNullInteger(historial.CodModeloBilletero));
                    query.Parameters.AddWithValue("@CodVolatilidad", ManejoNulos.ManageNullInteger(historial.CodVolatilidad));
                    query.Parameters.AddWithValue("@CodLinea", ManejoNulos.ManageNullInteger(historial.CodLinea));
                    query.Parameters.AddWithValue("@CodTipoMaquina", ManejoNulos.ManageNullInteger(historial.CodTipoMaquina));
                    query.Parameters.AddWithValue("@CodModeloHopper", ManejoNulos.ManageNullInteger(historial.CodModeloHopper));
                    query.Parameters.AddWithValue("@CodContrato", ManejoNulos.ManageNullInteger(historial.CodContrato));
                    query.Parameters.AddWithValue("@CodClasificacion", ManejoNulos.ManageNullInteger(historial.CodClasificacion));
                    query.Parameters.AddWithValue("@CodMueble", ManejoNulos.ManageNullInteger(historial.CodMueble));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(historial.CodSala));
                    query.Parameters.AddWithValue("@CodEmpresa", ManejoNulos.ManageNullInteger(historial.CodEmpresa));
                    query.Parameters.AddWithValue("@CodZona", ManejoNulos.ManageNullInteger(historial.CodZona));
                    query.Parameters.AddWithValue("@CodIsla", ManejoNulos.ManageNullInteger(historial.CodIsla));
                    query.Parameters.AddWithValue("@CodPantalla", ManejoNulos.ManageNullInteger(historial.CodPantalla));
                    query.Parameters.AddWithValue("@CodMoneda", ManejoNulos.ManageNullInteger(historial.CodMoneda));
                    query.Parameters.AddWithValue("@CodFicha", ManejoNulos.ManageNullInteger(historial.CodFicha));
                    query.Parameters.AddWithValue("@CodMedioJuego", ManejoNulos.ManageNullInteger(historial.CodMedioJuego));
                    query.Parameters.AddWithValue("@CodModeloMaquina", ManejoNulos.ManageNullInteger(historial.CodModeloMaquina));
                    query.Parameters.AddWithValue("@CodAlmacen", ManejoNulos.ManageNullInteger(historial.CodAlmacen));
                    query.Parameters.AddWithValue("@CodFormula", ManejoNulos.ManageNullInteger(historial.CodFormula));
                    query.Parameters.AddWithValue("@CodEstadoMaquina", ManejoNulos.ManageNullInteger(historial.CodEstadoMaquina));
                    query.Parameters.AddWithValue("@CodMaquinaLey", ManejoNulos.ManageNullStr(historial.CodMaquinaLey));
                    query.Parameters.AddWithValue("@CodAlterno", ManejoNulos.ManageNullStr(historial.CodAlterno));
                    query.Parameters.AddWithValue("@NroFabricacion", ManejoNulos.ManageNullStr(historial.NroFabricacion));
                    query.Parameters.AddWithValue("@FechaFabricacion", ManejoNulos.ManageNullDate(historial.FechaFabricacion));
                    query.Parameters.AddWithValue("@FechaReconstruccion", ManejoNulos.ManageNullDate(historial.FechaReconstruccion));
                    query.Parameters.AddWithValue("@NroSerie", ManejoNulos.ManageNullStr(historial.NroSerie));
                    query.Parameters.AddWithValue("@ValorComercial", ManejoNulos.ManageNullDecimal(historial.ValorComercial));
                    query.Parameters.AddWithValue("@CordX", ManejoNulos.ManageNullInteger(historial.CordX));
                    query.Parameters.AddWithValue("@CordY", ManejoNulos.ManageNullInteger(historial.CordY));
                    query.Parameters.AddWithValue("@Segmento", ManejoNulos.ManageNullInteger(historial.Segmento));
                    query.Parameters.AddWithValue("@Posicion", ManejoNulos.ManageNullInteger(historial.Posicion));
                    query.Parameters.AddWithValue("@ApuestaMaxima", ManejoNulos.ManageNullDecimal(historial.ApuestaMaxima));
                    query.Parameters.AddWithValue("@ApuestaMinima", ManejoNulos.ManageNullDecimal(historial.ApuestaMinima));
                    query.Parameters.AddWithValue("@Hopper", ManejoNulos.ManageNullInteger(historial.Hopper));
                    query.Parameters.AddWithValue("@CreditoFicha", ManejoNulos.ManageNullDecimal(historial.CreditoFicha));
                    query.Parameters.AddWithValue("@Token", ManejoNulos.ManageNullDecimal(historial.Token));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(historial.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(historial.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(historial.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(historial.Estado));
                    query.Parameters.AddWithValue("@CodTipoFicha", ManejoNulos.ManageNullInteger(historial.CodTipoFicha));
                    query.Parameters.AddWithValue("@PorcentajeDevolucion", ManejoNulos.ManageNullDecimal(historial.PorcentajeDevolucion));
                    query.Parameters.AddWithValue("@FechaOperacionIni", ManejoNulos.ManageNullDate(historial.FechaOperacionIni));
                    query.Parameters.AddWithValue("@FechaOperacionFin", ManejoNulos.ManageNullDate(historial.FechaOperacionFin));
                    query.Parameters.AddWithValue("@ResumenCambios", ManejoNulos.ManageNullStr(historial.ResumenCambios));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(historial.CodUsuario));
                    query.Parameters.AddWithValue("@CodHistorialMaquina", ManejoNulos.ManageNullInteger(historial.CodHistorialMaquina));
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
    }
}
