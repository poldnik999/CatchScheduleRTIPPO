using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace lab6
{
    internal class ExportMaster
    {
        public static void exportExcel(DataTable table)
        {
            Excel.Application exApp = new Excel.Application();
            exApp.Workbooks.Add(); Excel.Worksheet wsh = (Excel.Worksheet)exApp.ActiveSheet;
            int i, j;
            for (i = 0; i < table.Rows.Count; i++)
            {
                for (j = 1; j < table.Columns.Count; j++)
                {
                    wsh.Cells[i + 1, j + 1] = table.Rows[i][j].ToString();
                }
            }
            exApp.Visible = true;
        }
    }
}
