using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using System.Text;
using MRaabta.App_Start;

namespace MRaabta.Controllers
{
    public class RetailCODController : Controller
    {
        // GET: RetailCOD
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            con.Close();
        }

        [SkipFilter]
        public async Task<ActionResult> AddressLabel(string CN)
        {
            try
            {
                CN = DecryptString(CN);

                con.Open();
                var response = await GetConsignmentData(CN);
                con.Close();
                for (int j = 0; j < response.Count - 1; j++)
                {
                    //DataRow Currentdr = dt.Rows[j];
                    //DataRow dr = dt.Rows[j + 1];
                    if (response[j].ConsignmentNumber.ToString() == response[j + 1].ConsignmentNumber.ToString())
                    {
                        String CurrentdrRowService = response[j].SuppService.ToString();
                        String drService = response[j + 1].SuppService.ToString();
                        float CurrentdrRowServicePrice = float.Parse(response[j].SuppCharges.ToString());
                        float drServicePrice = float.Parse(response[j + 1].SuppCharges.ToString());
                        float drcalculatedGST = float.Parse(response[j + 1].calculatedGST.ToString());
                        float CurrentcalculatedGST = float.Parse(response[j].calculatedGST.ToString());

                        response[j].SuppService = CurrentdrRowService + ", " + drService;
                        response[j].SuppCharges = (CurrentdrRowServicePrice + drServicePrice).ToString();
                        response[j].calculatedGST = (drcalculatedGST + CurrentcalculatedGST).ToString();

                        response.RemoveAt(j + 1);
                        --j;
                    }
                }
                double chargedAmountResp = 0, totalAmountResp = 0, gstResp = 0;
                string DiscountResp, SuppChargesResp;

                for (int i = 0; i < response.Count; i++)
                {
                    chargedAmountResp = double.Parse(response[i].ChargedAmount.ToString());
                    totalAmountResp = double.Parse(response[i].totalAmount.ToString());
                    gstResp = double.Parse(response[i].GST.ToString());
                    DiscountResp = response[i].Discount;
                    SuppChargesResp = response[i].SuppCharges;

                    // --------- If Discount Applied ---------
                    if (response[i].DiscountID != "")
                    {
                        // FOR PERCENTAGE DISCOUNT
                        if (response[i].DiscountValueType == "1")
                        {
                            response[i].Discount = Math.Round(Math.Abs(chargedAmountResp - (totalAmountResp + gstResp)), 2).ToString("N0");
                            response[i].ChargedAmount = Math.Round(Math.Abs(chargedAmountResp), 2).ToString("N0");
                            response[i].GST = Math.Round(gstResp, 2).ToString("N0");
                            response[i].totalAmount = Math.Round(totalAmountResp, 2).ToString("N0");

                            if (response[i].SuppCharges != "")
                            {
                                //response[i].totalAmount = (Math.Round(totalAmountResp, 2) + Math.Round(double.Parse(SuppChargesResp), 2)).ToString("N0");
                                //response[i].GST = (Math.Round(gstResp, 2) + Math.Round(double.Parse(response[i].calculatedGST), 2)).ToString("N0");
                                //response[i].ChargedAmount = (Math.Round(chargedAmountResp, 2) + Math.Round(double.Parse(SuppChargesResp), 2) + Math.Round(double.Parse(response[i].calculatedGST), 2)).ToString("N0");
                                
                                response[i].totalAmount = (Math.Round(totalAmountResp, 2)).ToString("N0");
                                response[i].GST = (Math.Round(gstResp, 2)).ToString("N0");
                                response[i].ChargedAmount = (Math.Round(chargedAmountResp, 2)).ToString("N0");
                            }
                        }

                        // FOR AMOUNT DISCOUNT
                        if (response[i].DiscountValueType == "2")
                        {
                            response[i].Discount = Math.Round(double.Parse(DiscountResp), 2).ToString("N0");
                            response[i].ChargedAmount = Math.Round(Math.Abs(chargedAmountResp), 2).ToString("N0");
                            response[i].GST = Math.Round(gstResp, 2).ToString("N0");

                            if (response[i].SuppCharges != "")
                            {
                                //response[i].GST = (Math.Round(gstResp, 2) + Math.Round(double.Parse(response[i].calculatedGST), 2)).ToString("N0");
                                //response[i].ChargedAmount = (Math.Round(totalAmountResp, 2) + Math.Round(gstResp, 2) + Math.Round(double.Parse(SuppChargesResp), 2) + Math.Round(double.Parse(response[i].calculatedGST), 2)).ToString("N0"); //double.Parse(response[i].SuppCharges)), 2).ToString();

                                response[i].GST = (Math.Round(gstResp, 2)).ToString("N0");
                                response[i].ChargedAmount = (Math.Round(totalAmountResp, 2)).ToString("N0"); 
                            }
                        }
                    }
                    // --------- If Discount Not Applied ---------
                    else
                    {
                        response[i].Discount = "N/A";
                        response[i].GST = Math.Round(gstResp, 2).ToString("N0");
                        response[i].ChargedAmount = (Math.Round(totalAmountResp, 2) + Math.Round(gstResp, 2)).ToString("N0");
                        response[i].totalAmount = (Math.Round(totalAmountResp, 2)).ToString("N0");

                        if (response[i].SuppCharges != "")
                        {
                            //response[i].totalAmount = (Math.Round(totalAmountResp, 2) + Math.Round(double.Parse(SuppChargesResp), 2)).ToString("N0");
                            //response[i].GST = (Math.Round(gstResp, 2) + Math.Round(double.Parse(response[i].calculatedGST), 2)).ToString("N0");
                            //response[i].ChargedAmount = (Math.Round(totalAmountResp, 2) + Math.Round(gstResp, 2) + Math.Round(double.Parse(SuppChargesResp), 2) + Math.Round(double.Parse(response[i].calculatedGST), 2)).ToString("N0"); //double.Parse(response[i].SuppCharges)), 2).ToString();

                            response[i].totalAmount = (Math.Round(totalAmountResp, 2)).ToString("N0");
                            response[i].GST = (Math.Round(gstResp, 2)).ToString("N0");
                            response[i].ChargedAmount = (Math.Round(totalAmountResp, 2) + Math.Round(gstResp, 2)).ToString("N0"); //double.Parse(response[i].SuppCharges)), 2).ToString();
                        }
                    }
                }
                if (response == null || response.Count == 0)
                {
                    ViewBag.ErrorMessage = "No Record found";
                }
                return View(response);
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

        public async Task<List<RetailCODConsignmentModel>> GetConsignmentData(string CNs)
        {
            List<RetailCODConsignmentModel> response = new List<RetailCODConsignmentModel>();
            string[] CNSplit = CNs.Split(',');
            StringBuilder sb = new StringBuilder();
            foreach (var item in CNSplit)
            {
                sb.Append($"'{item}',");
            }
            var resultCN = sb.ToString().TrimEnd(',');
            var rs = await con.QueryAsync<RetailCODConsignmentModel>($@"
                                    SELECT FORMAT(GETDATE(), 'dd/MM/yyyy HH:mm:ss') PrintDateTime,
                                    c.consignmentNumber,       c.pieces,c.consigner Shipper,c.consignerCellNo [ShipperContact],c.shipperAddress,
                                    c.[address] ConsigneeAddress,c.consignee,
                                    c.[weight],c.consigneePhoneNo ConsigneeContact,
                                    cdn.codAmount,
                                    isnull(c.insuarancePercentage,'') DeclaredInsuranceValue,c.serviceTypeName SERVICE, ob.name  Origin,c.remarks,c.couponNumber,
                                    c.totalAmount totalAmount, c.gst, c.chargedAmount ChargedAmount, od.NAME Destination,
                                    cdn.productDescription ProductDetail, cdn.orderRefNo CustomerRef,
                                    FORMAT(c.bookingDate, 'dd/MM/yyyy HH:mm:ss') bookingDate,c.createdBy,c.width DimensionWidth,c.breadth DimensionBreadth,
                                    c.height DimensionHeight,
                                    isnull(c.DiscountID,'') DiscountID  ,isnull(pm.name,'') [SuppService],isnull(cm.calculatedValue,'') [SuppCharges], 
                                    isnull(calculatedGST,'') calculatedGST, 
                                    isnull(dm.DiscountValueType,'0') DiscountValueType,  dm.DiscountValue  Discount ,  
                                    zu.U_NAME BookingStaff, cm.AlternateValue ValDeclared, cm.modifiedCalculationValue DeclaredInsuranceValue, cm.priceModifierId,
                                    c.riderCode BookingCode, p.Name Province, c.consignerCNICNo cnic
                                    FROM Consignment c
                                    INNER JOIN branches ob ON  ob.branchCode = c.orgin
                                    INNER JOIN branches od ON  od.branchCode = c.destination
                                    INNER JOIN CODConsignmentDetail_New cdn ON cdn.consignmentNumber=c.consignmentNumber
                                    INNER JOIN ZNI_USER1 zu on zu.U_ID=c.createdBy
                                    LEFT JOIN ConsignmentModifier cm ON cm.consignmentNumber=c.consignmentNumber 
                                    LEFT JOIN PriceModifiers pm  ON pm.id = cm.priceModifierId  
                                    LEFT JOIN MNP_DiscountConsignment dc ON  c.consignmentNumber = dc.ConsignmentNumber 
                                    LEFT JOIN MnP_MasterDiscount dm ON  dc.DiscountID = dm.DiscountID 
                                    LEFT JOIN Province AS p ON ob.Province = p.ProvinceId
                                    WHERE  c.consignmentNumber in({resultCN})");
            return rs.ToList();
        }
        [SkipFilter]
        public async Task<ActionResult> RetailCODCRF(string Account, string PhoneNo)
        {
            var response = new RetailCODCRFModel();
            try
            {
                con.Open();
                response = await GetCODCustomerData(Account,PhoneNo);
                if (response == null)
                {
                    ViewBag.ErrorMessage = "No Record found";
                }
            }
            catch (Exception er)
            {
                string msg = "Error occured: " + er.Message.ToString();
                msg = msg.Replace("\n", String.Empty);
                msg = msg.Replace("\r", String.Empty);
                ViewBag.ErrorMessage = msg;
            }
            finally
            {
                con.Close();
            }
            return View(response);
        }

        public async Task<RetailCODCRFModel> GetCODCustomerData(string Account, string PhoneNo)
        {
            string condition = "";
            if (Account != "")
            {
                condition = $@" where cu.AccountNumber = '{Account}'";
            }
            if (PhoneNo != "" && PhoneNo!= null)
            {
                condition = $@" where cu.MobileNumber = '{PhoneNo}'";
            }
            if(Account!="" && PhoneNo != "" && PhoneNo!=null)
            {
                condition = $@" where cu.AccountNumber = '{Account}' and cu.MobileNumber = '{PhoneNo}' ";
            }
            string sql = $@"SELECT cu.CustomerName,CONVERT(VARCHAR, cu.createdOn, 106) RegistrationDate,mprs.EmployeeUsername RSEName,
                            ec.name CourierCenterName,cu.address RegisteredAddress, cu.CNIC,cu.AccountNumber,b2.Name BankName,cu.IBFT                   BankAccNo,cu.AccountTitle           BankAccountTitle,
                            cu.AccountNumber, b.name Branch, z.name Zone,cu.MobileNumber BankContactNo,cu.BankBranchName BranchName,cu.BankBranchCode
                            FROM MNP_RETAIL_COD_CUSTOMERS cu
                            INNER JOIN MnP_Retail_Staff mprs ON  mprs.UserId = cu.CreatedBy
                            INNER JOIN ExpressCenters ec ON  ec.expressCenterCode = mprs.ExpressCenterCode
                            INNER JOIN Branches b ON b.branchCode=cu.Branch
                            INNER JOIN Zones z ON z.zoneCode=b.zoneCode
                            LEFT JOIN Banks b2 ON b2.Id=cu.BankName
                            {condition} ";
            var rs = await con.QueryFirstOrDefaultAsync<RetailCODCRFModel>(sql);
            return rs;
        }

        public string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public string EncryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAK$2-P0N*78+17";
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
            string EncryptionKey = "MAK$2-P0N*78+17";
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
 
        public ActionResult FirebaseTest()
        {
            return View();
        }
    }
}