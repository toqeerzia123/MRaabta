using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class DownloadCSVController : Controller
    {
        // GET: DownloadCSV
        public FileResult Index()
        {
            StringBuilder sb = new StringBuilder();
            
            DataTable dt = (DataTable)Session["ExtractDtToCSV"];

            for (int k = 0; k < dt.Columns.Count; k++)
            {
                sb.Append(dt.Columns[k].ColumnName + ',');
            }
            sb.Append("\r\n");
            for (int m=0;m<dt.Rows.Count;m++) {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sb.Append(String.Format("{0}", dt.Rows[m][j].ToString()) + ',');
                }
                sb.Append("\r\n");
            } 
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Report.csv");
        }
    }
}