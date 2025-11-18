using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MantenimientoBD
{
    public class TablaInformacionEntidad
    {
       public string TableName { get; set; }
       public string SchemaName { get; set; }
       public Int64 rows { get; set; }
       public double TotalSpaceKB { get; set; }
       public double TotalSpaceMB { get; set; }
       public double UsedSpaceKB { get; set; }
       public double UsedSpaceMB { get; set; }
       public double UnusedSpaceKB { get; set; }
       public double UnusedSpaceMB { get; set; }
    }
}
