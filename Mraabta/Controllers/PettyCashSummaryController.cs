using Dapper;
using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MRaabta.Controllers
{
    public class PettyCashSummaryController : Controller
    {

        SqlConnection con;
         
        public PettyCashSummaryController()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            PettyCashModel model = new PettyCashModel();
            try
            {
                if (Session["U_ID"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                var abc = Session["U_ID"].ToString();
                var zones = await GetZones();
                var branches = await GetBranches("");
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;

                ViewBag.Zones = zones.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                ViewBag.Branches = branches.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
            
                string msg = "Error occured: " + ex.Message.ToString();
                msg = msg.Replace("\n", String.Empty);
                msg = msg.Replace("\r", String.Empty);
                ViewBag.ErrorMessage = msg;
                return View(model);
            }
        }

        internal async Task<List<PettyCashDropdown>> GetBranches(string ZoneCode)
        {
            try
            {
                
                string SessionBranch = "";

                if (ZoneCode == "")
                {
                    if (Session["BranchCode"].ToString().ToUpper() == "ALL")
                    {
                        SessionBranch = "";
                    }
                    else
                    {
                        SessionBranch = "'" + Session["BRANCHCODE"].ToString().Replace(",", "','") + "'";
                        SessionBranch = "AND BranchCode in (" + SessionBranch + ")";
                    }
                }
                else
                {
                    var zone = ZoneCode.Split(',');
                    ZoneCode = "'" + string.Join("','", zone) + "'";
                    string branchCondition = "";
                    if (!string.IsNullOrEmpty(Session["BRANCHCODE"].ToString()) && Session["BRANCHCODE"].ToString() != "ALL")
                    {
                        branchCondition = " and BranchCode in ('" + Session["BRANCHCODE"].ToString().Replace(",", "','") + "')";
                    }
                    ZoneCode = " AND ZoneCode in (" + ZoneCode + ") " + branchCondition + " ";
                }
                string sql = @" SELECT NAME Text, 
                                branchCode Value  
                         FROM   Branches  
                        WHERE  STATUS = '1'  " + SessionBranch + " " + ZoneCode + "  ORDER BY NAME ";

                await con.OpenAsync();
                var rs = await con.QueryAsync<PettyCashDropdown>(sql);
                var branches = rs.ToList();
                branches.Insert(0, new PettyCashDropdown { Value = "ALL", Text = "ALL" });
               // var List = branches.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                //return Json(new { Data = List }, JsonRequestBehavior.AllowGet);
                return branches;
            }
            catch (Exception er)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetBranchesJson(string ZoneCode)
        {
            var branches = await GetBranches(ZoneCode);
            return Json(new { Status = true, Message = "Success", Data = branches }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> PettyCashSummaryReport(string[] Zone, string[] Branch, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                PettyCashModel data = new PettyCashModel();
                int totalDays = (EndDate - StartDate).Days;
                if (totalDays > 31 || totalDays < 0)
                { 
                    return Json(new { Status = false, Message =   "Maximum date is 31 days" });
                }
                await con.OpenAsync();
                data.DataList = await GetSummaryReport(Zone, Branch,  StartDate ,  EndDate );
                con.Close();
                if (data.DataList.Count > 0)
                {
                    data.EncryptedStartDate = Encrypt(StartDate.ToString("yyyy-MM-dd"));
                    data.EncryptedEndDate = Encrypt(EndDate.ToString("yyyy-MM-dd"));
                    foreach (var item in data.DataList)
                    {
                        item.EncryptedBranch = Encrypt(item.branch);
                        item.EncryptedBranchCode = Encrypt(item.branchCode);
                        item.EncryptedZoneCode = Encrypt(item.zoneCode);
                        item.Company = Encrypt(item.Company);
                        item.cnoteComma = item.cnote.ToString("N0");
                        item.dnoteComma = item.dnote.ToString("N0");
                        item.Balance = Double.Parse(item.Balance).ToString("N0");
                    }
                }
                return Json(new { Status=true,Message= "Success", Data = data });
            }
            catch (Exception ex)
            {
            
                string msg = "Error occured: " + ex.Message.ToString();
                msg = msg.Replace("\n", String.Empty);
                msg = msg.Replace("\r", String.Empty);
                ViewBag.ErrorMessage = msg;
                return Json(new { Status = false, Message = "Error "+ex.Message.ToString()});
            }
            finally
            {
                con.Close();
            }
        }
      
        private async Task<List<PettyCashDropdown>> GetZones()
        {
            try
            {
                var response = new List<PettyCashDropdown>();
                string zone = "";
                if (!string.IsNullOrEmpty(Session["ZONECODE"].ToString()))
                {
                    zone = "'" + Session["ZONECODE"].ToString().Replace(",", "','") + "'";
                }
                string sqlString = " SELECT Z.zoneCode Value,Z.name Text FROM ZONES Z  " +
                "WHERE Z.zoneCode in  (" + zone + ")  and status=1   ORDER BY NAME ";

                await con.OpenAsync();
                var rs = await con.QueryAsync<PettyCashDropdown>(sqlString);
                response = rs.ToList();
                return response;
            } 
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        private async Task<List<PettyCashData>> GetSummaryReport(string[] Zone, string[] Branch, DateTime StartDate, DateTime EndDate)
        {
                string ZoneCodes = "";
                string LocationCondition = "";
                if (Branch.Contains("ALL"))
                {
                    if (!Branch.Contains(","))
                    {
                        ZoneCodes = "'" + string.Join("','", Zone) + "'";
                    }
                    else
                    {
                        ZoneCodes = string.Join(",", Zone);
                    }
                    if (Session["BRANCHCODE"].ToString() != "ALL")
                    {
                        LocationCondition = " AND pch.zonecode IN (" + ZoneCodes + ")  AND pch.Branch in ('" + Session["BRANCHCODE"].ToString().Replace(",", "','") + "')";
                    }
                    else
                    {
                        LocationCondition = " AND pch.zonecode IN (" + ZoneCodes + ") ";
                    }
                }
                else
                {
                    var Branches = string.Join("','", Branch);
                    LocationCondition = " AND pch.BRANCH IN ('" + Branches + "') ";
                }

                string sql = @"  SELECT 
                 DateName( month , DateAdd( month ,Month(a.Date) , -1 ) ) + '-' + Cast(YEAR(a.Date) AS VARCHAR)     MONTH, z.name zone,z.zoneCode,b.branchCode,b.name branch,sum(ISNULL(a.Credit, 0)) cnote, sum(ISNULL(a.Debit, 0))  dnote, sum(ISNULL(a.Credit, 0)) - sum(ISNULL(a.Debit, 0))  Balance
                ,1 Company  FROM   (
                           SELECT '1' RANK,                  pch.COMPANY,                  pch.BRANCH,
                                  pch.[YEAR],                  pch.[MONTH],             pcd.DATE  'Date',
                                  pch.express_center,
                                  CASE 
                                       WHEN pcd.AMOUNT < 0 THEN (pcd.AMOUNT * -1)
                                       ELSE 0
                                  END             Credit,
                                  CASE 
                                       WHEN pcd.amount < 0 THEN 0
                                       ELSE pcd.AMOUNT
                                  END             Debit,                  'Petty Cash' expense,
                                  'AMOUNT TRANSFERRED FROM CIH TO PETTY CASH' + '(' + pmode.[desc]
                                  + CASE 
                                         WHEN pcd.cash_type = '2' THEN ' CHEQUE NO. ' + pcd.chque_no
                                         ELSE ''
                                    END + ')' DESCRIPTION,
                                  '' narrate,                  pch.ID,                  cm.sdesc_OF     comp1
                           FROM   PC_CIH_head  AS pch
                                  INNER JOIN PC_CIH_detail AS pcd
                                       ON  pcd.head_id = pch.ID
                                  LEFT OUTER JOIN PC_cash_mode pmode
                                       ON  pmode.ID = pcd.cash_type
                                  INNER JOIN COMPANY_OF cm
                                       ON  cm.code_OF = pch.COMPANY
                                       AND pch.COMPANY = '1'
                           WHERE  pcd.[DATE] BETWEEN CAST('" + StartDate.ToString("yyyy-MM-dd") + @"' AS DATE) AND CAST('" + EndDate.ToString("yyyy-MM-dd") + @"' AS DATE)
                                   " + LocationCondition + @" 
           
                           UNION ALL
           
                           SELECT '2' RANK,                  pch.COMPANY,                  pch.BRANCH,
                                  pch.[YEAR],                  pch.[MONTH],      pcd.DATE  'Date',
                                  pch.express_center,
                                  CASE 
                                       WHEN pcd.AMOUNT < 0 THEN 0
                                       ELSE pcd.AMOUNT
                                  END                      Credit,
                                  CASE 
                                       WHEN pcd.AMOUNT < 0 THEN (pcd.AMOUNT * -1)
                                       ELSE 0
                                  END                      Debit,                  m.description            expense,
                                  s.sub_desc               DESCRIPTION,                  pcd.NARRATE,                  pch.ID,
                                  cm.sdesc_OF              comp1
                                  FROM   PC_head               AS pch
                                  INNER JOIN PC_detail  AS pcd
                                       ON  pcd.head_id = pch.ID --and pch.CREATED_BY=pcd.CREATED_BY
                                  LEFT OUTER JOIN pc_mainHead m
                                       ON  pcd.expense = m.code
                                  LEFT OUTER JOIN pc_subHead s
                                       ON  pcd.[desc] = s.subcode
                                       AND pcd.EXPENSE = s.headcode
                                  INNER JOIN COMPANY_OF cm
                                       ON  cm.code_OF = pch.COMPANY
                                       AND pch.COMPANY = '1'
                           WHERE  pcd.[DATE] BETWEEN CAST('" + StartDate.ToString("yyyy-MM-dd") + @"' AS DATE) AND CAST('" + EndDate.ToString("yyyy-MM-dd") + @"' AS DATE)
                                      " + LocationCondition + @" 
                                  AND pcd.status = '0'
                       ) a
                       INNER JOIN Branches b
                            ON  a.BRANCH = b.branchCode
                       INNER JOIN Zones z ON b.zoneCode = z.zoneCode
                       LEFT OUTER JOIN ExpressCenters e
                            ON  a.express_center = e.expressCenterCode
                GROUP BY    MONTH(a.Date),YEAR(a.Date) ,b.name, z.name  ,z.zoneCode,b.branchCode
                    ORDER BY
                1,2,3 ASC  ";
                var rs = await con.QueryAsync<PettyCashData>(sql);
                return rs.ToList();
            
        }
        
        [HttpGet]
        public async Task<ActionResult> PettyCashSummaryDetail(string StartDate, string EndDate, string zoneCode, string branchCode, string BranchName, string Company, string status)
        {
            try
            {
                await con.OpenAsync();
                StartDate = Decrypt(StartDate);
                EndDate = Decrypt(EndDate);
                zoneCode = Decrypt(zoneCode);
                branchCode = Decrypt(branchCode);
                BranchName = Decrypt(BranchName);
                Company = Decrypt(Company);

                PettyCashModel pettyCashModel = new PettyCashModel();
                pettyCashModel.StartDate = DateTime.Parse(StartDate);
                pettyCashModel.EndDate = DateTime.Parse(EndDate);
                ViewBag.CreatedUser = Session["U_NAME"].ToString();
                pettyCashModel.Status = status;
                pettyCashModel.BranchName = BranchName;

                pettyCashModel.DataList = await GetPettyReportDetails(StartDate, EndDate, zoneCode, branchCode, status);
                pettyCashModel.TotalCredit = pettyCashModel.DataList.Sum(a => a.cnote).ToString("N0");
                pettyCashModel.TotalDebit = pettyCashModel.DataList.Sum(a => a.dnote).ToString("N0");
                foreach (var item in pettyCashModel.DataList)
                {
                    item.dnoteComma = item.dnote.ToString("N0");
                    item.cnoteComma = item.cnote.ToString("N0");
                }
                var rs = pettyCashModel.DataList.Where(a => !string.IsNullOrEmpty(a.comp1)).FirstOrDefault();
                if (rs != null)
                {
                    ViewBag.CompanyName = rs.comp1;
                }
                return View(pettyCashModel);
            }
            catch (Exception er)
            {
                string msg = "Error occured: " + er.Message.ToString();
                msg = msg.Replace("\n", String.Empty);
                msg = msg.Replace("\r", String.Empty);
                ViewBag.ErrorMessage = msg;
                return View();
            }
            finally
            {
                con.Close();
            }

        }
        private async Task<List<PettyCashData>> GetPettyReportDetails(string StartDate, string EndDate, string ZoneCode, string BranchCode, string status)
        { 
            string BalanceQuery = "";
            string ExtraTransactions = "";
            if (status == "1")
            {
                BalanceQuery = "     0     dnote, SUM(isnull(a.Credit,0))     cnote  ";
                ExtraTransactions = "  WHERE isnull(a.Credit,0)>0  ";
            }
            else if (status == "2")
            {
                BalanceQuery = "     SUM(isnull(a.Debit,0))      dnote, 0     cnote  ";
                ExtraTransactions = "   WHERE isnull(a.Debit,0)>0   ";
            }
                string sql = @" SELECT      z.name Zone,          a.YEAR,       a.MONTH,       a.Date, " + BalanceQuery + @" ,      b.name       branch,a.chque_no,
                         a.expense description,          isnull(a.comp1,'') comp1 
            FROM   (
                       SELECT '1' RANK,                  pch.COMPANY,                  pch.BRANCH,                  pch.[YEAR],                  pch.[MONTH],
                              CONVERT(VARCHAR, pcd.DATE, 106) 'Date',                  pch.express_center,pcd.chque_no,
                              CASE
                                   WHEN pcd.AMOUNT < 0 THEN (pcd.AMOUNT * -1)
                                   ELSE 0
                              END             Credit,
                              CASE
                                   WHEN pcd.amount < 0 THEN 0
                                   ELSE pcd.AMOUNT
                              END             Debit,                  'Petty Cash' expense,
                              'AMOUNT TRANSFERRED FROM CIH TO PETTY CASH' + '(' + pmode.[desc]
                              + CASE WHEN pcd.cash_type = '2' THEN ' CHEQUE NO. ' + pcd.chque_no ELSE '' END + ')' DESCRIPTION,
                              '' narrate,                  pch.ID,                  cm.sdesc_OF     comp1
                       FROM   PC_CIH_head  AS pch
                              INNER JOIN PC_CIH_detail AS pcd
                                   ON  pcd.head_id = pch.ID
                              LEFT OUTER JOIN PC_cash_mode pmode
                                   ON  pmode.ID = pcd.cash_type
                              INNER JOIN COMPANY_OF cm
                                   ON  cm.code_OF = pch.COMPANY
                       and pch.COMPANY = '1'
             where pcd.[DATE] BETWEEN CAST(@StartDate AS date) AND CAST(@EndDate  AS date)
              and pch.BRANCH in  (@BranchCode)
   
                       UNION ALL

                       SELECT '2' RANK,                  pch.COMPANY,                  pch.BRANCH,                  pch.[YEAR],                  pch.[MONTH],
                              CONVERT(VARCHAR, pcd.DATE, 106) 'Date',                  pch.express_center,'' chque_no,
                              CASE
                                   WHEN pcd.AMOUNT < 0 THEN 0
                                   ELSE pcd.AMOUNT
                              END                      Credit,
                              CASE
                                   WHEN pcd.AMOUNT < 0 THEN (pcd.AMOUNT * -1)
                                   ELSE 0
                              END                      Debit,                  m.description            expense,                  s.sub_desc               DESCRIPTION,
                              pcd.NARRATE,                  pch.ID,                  cm.sdesc_OF              comp1           
                       FROM   PC_head               AS pch
                              INNER JOIN PC_detail  AS pcd
                                   ON  pcd.head_id = pch.ID --and pch.CREATED_BY=pcd.CREATED_BY

                              LEFT OUTER JOIN pc_mainHead m
                                   ON  pcd.expense = m.code
                              LEFT OUTER JOIN pc_subHead s
                                   ON  pcd.[desc] = s.subcode
                                   AND pcd.EXPENSE = s.headcode
                              INNER JOIN COMPANY_OF cm
                                   ON  cm.code_OF = pch.COMPANY
                       and pch.COMPANY = '1'
             where pcd.[DATE] BETWEEN CAST(@StartDate AS date) AND CAST(@EndDate  AS date)
              and pch.BRANCH in  (@BranchCode)
               and pcd.status='0'         ) a
                   INNER JOIN Branches b
                        ON  a.BRANCH = b.branchCode
                     INNER JOIN Zones z ON z.zoneCode=b.zoneCode
                   LEFT OUTER JOIN ExpressCenters e
                        ON  a.express_center = e.expressCenterCode
                " + ExtraTransactions + @"

                GROUP BY
                       z.name   ,a.YEAR,a.MONTH,a.Date, 
                       b.name, a.expense,  ISNULL(a.comp1, '')   
                           ,a.chque_no     
            ORDER BY       5,       1";

                var rs = await con.QueryAsync<PettyCashData>(sql, new { @StartDate = StartDate, @EndDate = EndDate, @BranchCode = BranchCode });
                return rs.ToList();
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAK$2-PBN*99+12";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAK$2-PBN*99+12";
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

 
    }
}