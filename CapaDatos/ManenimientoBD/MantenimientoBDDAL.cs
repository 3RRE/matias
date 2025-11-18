using CapaEntidad;
using CapaEntidad.ControlAcceso;
using CapaEntidad.MantenimientoBD;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ManenimientoBD
{
    public class MantenimientoBDDAL
    {
        string _conexion = string.Empty;
        public MantenimientoBDDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public bool GenerarBackup(string RutaBackup)
        {
            bool response = false;
            string consulta = $@"
                    SET NOCOUNT ON;

                    DECLARE 
                          @FileName NVARCHAR(1024)
                        , @DBName NVARCHAR(256)
                        , @PathName NVARCHAR(256)
                        , @Message NVARCHAR(2048)
                        , @IsCompressed BIT
	                    , @DatabaseSelected nvarchar(256)

                    SELECT 
                          @PathName = @RutaBackup
                        , @IsCompressed = 1 
	                    ,@DatabaseSelected='BD_SEGURIDAD_PJ'

                    DECLARE db CURSOR LOCAL READ_ONLY FAST_FORWARD FOR  
                          SELECT
                              sd.name
                            , file_path = @PathName+ name+'_'+FileDate + '.bak'
                        FROM sys.databases sd
                        CROSS JOIN (
                            SELECT FileDate = REPLACE(CONVERT(VARCHAR(10), GETDATE(), 103), '/', '_')
                        ) fd
                        WHERE sd.state_desc != 'OFFLINE'
                            AND sd.name = @DatabaseSelected
                        ORDER BY sd.name 

                    OPEN db

                    FETCH NEXT FROM db INTO 
                          @DBName
                        , @FileName  

                    WHILE @@FETCH_STATUS = 0 BEGIN 

                        DECLARE @SQL NVARCHAR(MAX)

                        SELECT @Message = REPLICATE('-', 80) + CHAR(13) + CONVERT(VARCHAR(20), GETDATE(), 120) + N': ' + @DBName
                        RAISERROR (@Message, 0, 1) WITH NOWAIT

                        SELECT @SQL = 
                        'BACKUP DATABASE [' + @DBName + ']
                        TO DISK = N''' + @FileName + '''
                        WITH FORMAT, ' + CASE WHEN @IsCompressed = 1 THEN N'COMPRESSION, ' ELSE '' END + N'INIT, STATS = 15;' 

                        EXEC sys.sp_executesql @SQL

                        FETCH NEXT FROM db INTO 
                              @DBName
                            , @FileName 

                    END   

                    CLOSE db   
                    DEALLOCATE db
                    ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@RutaBackup", RutaBackup);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return response;
        }
        public bool ShrinkDatabase(string DatabaseFile, string DatabaseLog)
        {
            bool response = false;
            string consulta = @"ALTER DATABASE @Database SET RECOVERY SIMPLE
                            GO
                            DBCC SHRINKFILE (@DataBaseLog, 1)
                            GO
                            ALTER DATABASE @Database SET RECOVERY FULL
                            GO";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Database", DatabaseFile);
                    query.Parameters.AddWithValue("@DatabaseLog", DatabaseLog);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return response;
        }
        public bool RebuildIndex(string DatabaseFile, string DatabaseLog)
        {
            bool response = false;
            string consulta = @"ALTER DATABASE @Database SET RECOVERY SIMPLE
                            GO
                            DBCC SHRINKFILE (@DataBaseLog, 1)
                            GO
                            ALTER DATABASE @Database SET RECOVERY FULL
                            GO";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Database", DatabaseFile);
                    query.Parameters.AddWithValue("@DatabaseLog", DatabaseLog);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return response;
        }
        public bool LimpiarTabla(string Tabla, DateTime Fecha, string Columna)
        {
            bool response = false;
            string consulta = $@"delete from {Tabla} where  convert(date,{Columna})<=convert(date,@Fecha)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.CommandTimeout = 300;
                    query.Parameters.AddWithValue("@Fecha", Fecha);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return response;
        }
        public DatabaseInformacionEntidad InformacionDatabase()
        {
            DatabaseInformacionEntidad item = new DatabaseInformacionEntidad();
            string consulta = @"SELECT 
                              database_name = DB_NAME(database_id)
                            , log_size_mb = CAST(SUM(CASE WHEN type_desc = 'LOG' THEN size END) * 8. / 1024 AS DECIMAL(8,2))
                            , row_size_mb = CAST(SUM(CASE WHEN type_desc = 'ROWS' THEN size END) * 8. / 1024 AS DECIMAL(8,2))
                            , total_size_mb = CAST(SUM(size) * 8. / 1024 AS DECIMAL(8,2))
                        FROM sys.master_files WITH(NOWAIT)
                        WHERE database_id = DB_ID() -- for current db 
                        GROUP BY database_id";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.database_name = ManejoNulos.ManageNullStr(dr["database_name"]);
                                item.log_size_mb = ManejoNulos.ManageNullDouble(dr["log_size_mb"]);
                                item.row_size_mb = ManejoNulos.ManageNullDouble(dr["row_size_mb"]);
                                item.total_size_mb = ManejoNulos.ManageNullDouble(dr["total_size_mb"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                item = new DatabaseInformacionEntidad();
            }
            return item;

        }
        public  List<TablaInformacionEntidad> InformacionTablas()
        {
            List<TablaInformacionEntidad> result = new List<TablaInformacionEntidad>();
            string consulta = @"SELECT 
                                t.NAME AS TableName,
                                s.Name AS SchemaName,
                                p.rows,
                                SUM(a.total_pages) * 8 AS TotalSpaceKB, 
                                CAST(ROUND(((SUM(a.total_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS TotalSpaceMB,
                                SUM(a.used_pages) * 8 AS UsedSpaceKB, 
                                CAST(ROUND(((SUM(a.used_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS UsedSpaceMB, 
                                (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB,
                                CAST(ROUND(((SUM(a.total_pages) - SUM(a.used_pages)) * 8) / 1024.00, 2) AS NUMERIC(36, 2)) AS UnusedSpaceMB
                            FROM 
                                sys.tables t
                            INNER JOIN      
                                sys.indexes i ON t.OBJECT_ID = i.object_id
                            INNER JOIN 
                                sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
                            INNER JOIN 
                                sys.allocation_units a ON p.partition_id = a.container_id
                            LEFT OUTER JOIN 
                                sys.schemas s ON t.schema_id = s.schema_id
                            WHERE 
                                t.NAME NOT LIKE 'dt%' 
                                AND t.is_ms_shipped = 0
                                AND i.OBJECT_ID > 255 
                            GROUP BY 
                                t.Name, s.Name, p.Rows
                            ORDER BY 
                                TotalSpaceMB DESC, t.Name";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item=new TablaInformacionEntidad() {
                                    TableName = ManejoNulos.ManageNullStr(dr["TableName"]),
                                    SchemaName = ManejoNulos.ManageNullStr(dr["SchemaName"]),
                                    rows = ManejoNulos.ManageNullInteger64(dr["rows"]),
                                    TotalSpaceKB = ManejoNulos.ManageNullDouble(dr["TotalSpaceKB"]),
                                    TotalSpaceMB = ManejoNulos.ManageNullDouble(dr["TotalSpaceMB"]),
                                    UsedSpaceKB = ManejoNulos.ManageNullDouble(dr["UsedSpaceKB"]),
                                    UsedSpaceMB = ManejoNulos.ManageNullDouble(dr["UsedSpaceMB"]),
                                    UnusedSpaceKB = ManejoNulos.ManageNullDouble(dr["UnusedSpaceKB"]),
                                    UnusedSpaceMB = ManejoNulos.ManageNullDouble(dr["UnusedSpaceMB"]),
                                };
                                result.Add(item);
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = new List<TablaInformacionEntidad>();
            }
            return result;

        }
        public List<BackupInformacionEntidad> ListarBackups(string RutaBackup)
        {
            List<BackupInformacionEntidad> result = new List<BackupInformacionEntidad>();
            string consulta = $@"EXEC sp_configure 'show advanced options', 1;
                                
                                RECONFIGURE;
                                
                                EXEC sp_configure 'xp_cmdshell',1;
                                
                                RECONFIGURE;
                                
                EXEC xp_cmdshell  'dir /a-d /b {RutaBackup}*.bak' ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new BackupInformacionEntidad()
                                {
                                    Nombre = ManejoNulos.ManageNullStr(dr["output"]),
                                };
                                result.Add(item);
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = new List<BackupInformacionEntidad>();
            }
            return result;
        }
        public double TamanioBackup(string RutaBackup)
        {
            double result = 0;
            string consulta = $@"RESTORE HEADERONLY FROM DISK='{RutaBackup}'";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                result = ManejoNulos.ManageNullDouble(dr["BackupSize"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            return result;
        }
        public bool EliminarArchivoBackup(string RutaBackup)
        {
            bool result = false;
            string consulta = $@"EXEC sp_configure 'show advanced options', 1;
                                
                                RECONFIGURE;
                                
                                EXEC sp_configure 'xp_cmdshell',1;
                                
                                RECONFIGURE;
                                
                EXEC xp_cmdshell  'del /f ""{RutaBackup}""' ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return result;
        }
    }
}
