using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for CustomerMethods
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class CustomerMethods
    {
        CL_Customer clvar = new CL_Customer();



        public CustomerMethods()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable GetClientGroups()
        {
            string query = "select * from ClientGroups cg where cg.collectionCenter='" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetClientCategories()
        {
            string query = "select * from CreditClientCategories ccc";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetClientStatusCodes()
        {
            string query = "select * from Lookup l where l.DropDownName = 'StatusCode' ";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetClientSectors()
        {
            string query = "select * from tblAdminSectors where isActive = '1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetClientIndustries()
        {
            string query = "select * from tblAdminIndustry where isActvie = '1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetClientStaffTypes()
        {
            string query = "selecT * from Client_StaffType";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetClientStaff(CL_Customer clvar)
        {

            string query = "select ua.username, ua.id\n" +
           "  from UserStaffType u\n" +
           " inner join UserAssociation ua\n" +
           "    on u.username = ua.username\n" +
           " where u.staffTypeId = '" + clvar.StaffType + "'\n" +
           "   and ua.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetSalesRoutes()
        {

            string query = "select routeCode, name from Routes r where r.status = '1' order by 2";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }
        public DataTable GetPriceModifiers()
        {
            string query = "select * from PriceModifiers where status = '1' and chkBillingModifier = '1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public string AddCustomer(CL_Customer clvar, DataTable modifiers, DataTable staff)
        {
            string query = "";

            string sqlString = "insert into CreditClients\n" +
            "  (name,\n" +
            "   contactPerson,\n" +
            "   phoneNo,\n" +
            "   faxNo,\n" +
            "   email,\n" +////////////////
            "   address,\n" +
            "   centralizedClient,\n" +
            "   regDate,\n" +
            "   regEndDate,\n" +///////////
            "   pickUpInstruction,\n" +
            "   domesticAMonTo,\n" +
            "   internationalAMonTo,\n" +//////////////
            "   domesticPackets,\n" +
            "   internationalPackets,\n" +
            "   domesticAmount,\n" +
            "   internationalAmount,\n" +
            "   status,\n" +
            "   printingStatus,\n" +
            "   billingMode,\n" +
            "   discountOnDomestic,\n" +
            "   discountOnSample,\n" +
            "   discountOnDocument,\n" +
            "   prepareBillType,\n" +
            "   creditLimit,\n" +
            "   salesTaxNo,\n" +
            "   memo,\n" +
            "   billTaxType,\n" +
            "   catId,\n" +
            "   clientGrpId,\n" +
            "   recoveryExpCenId,\n" +
            "   salesRouteId,\n" +
            "   recoveryOfficer,\n" +
            "   salesExecutive,\n" +
            "   redeemWindow,\n" +
            "   overdueCalBase,\n" +
            "   overdueValue,\n" +
            "   createdBy,\n" +
            "   createdOn,\n" +
            "   SectorId,\n" +
            "   IndustryId,\n" +
            "   accountNo,\n" +
            "   creditClientType,\n" +////////////////////////////////////////////////
            "   zoneCode,\n" +
            "   branchCode,\n" +
            "   expressCenterCode,\n" +/////////////////////
            "   ntnNo,\n" +
            "   IsCOD,\n" +
            "   isActive,\n" +///////////////////
            "   IsSpecial,\n" +
            "   isFranchisee,\n" +
            "   recoveryOfficerName,\n" +
            "   recoveryOfficer_id,\n" +
            "   isNationWide,\n" +
            "   isParent,\n" +
            "   MailingAddress,\n" +
            "   OriginEC,\n" +
            "   StatusCode,\n" +
            "   CODType,\n" +
            "   MonthlyFixCharges,\n" +
            "   IsSmsServiceActive,\n" +
            "   RNRCNCharges,\n" +
            "   HSCNCharges,\n" +
            "  )\n" +
            "Values\n" +
            "  (\n" +
            "'" + clvar.Name + "',\n" +
            "'" + clvar.ContactPerson + "',\n" +
            "'" + clvar.PhoneNo + "',\n" +
            "'" + clvar.FaxNo + "',\n" +
            "'" + clvar.CustomerEmail + "',\n" +/////////////////
            "'" + clvar.OfficialAddress + "',\n" +
            "'" + clvar.IsCentralized + "',\n" +
            "'" + clvar.RegDate + "',\n" +
            "'" + clvar.RegEndDate + "',\n" +/////////////////////////
            "'" + clvar.PickupInstructions + "',\n" +
            "'" + clvar.DomesticAmonTo + "',\n" +
            "'" + clvar.InternationalAmonTo + "',\n" +
            "'" + clvar.DomesticPackets + "',\n" +
            "'" + clvar.InternationalPackets + "',\n" +
            "'" + clvar.DomesticAmount + "',\n" +
            "'" + clvar.InternationalAmount + "',\n" +
            "'" + clvar.Status + "',\n" +
            "'" + clvar.PrintingStatus + "',\n" +
            "'" + clvar.BillingMode + "',\n" +
            "'" + clvar.DiscountOnDomestic + "',\n" +
            "'" + clvar.DiscountOnSample + "',\n" +
            "'" + clvar.DiscountOnDocument + "',\n" +
            "'" + clvar.PrepareBillType + "',\n" +
            "'" + clvar.CreditLimit + "',\n" +
            "'" + clvar.SalesTaxNo + "',\n" +
            "'" + clvar.Memo + "',\n" +
            "'" + clvar.BillTaxType + "',\n" +
            "'" + clvar.Category + "',\n" +
            "'" + clvar.ClientGroupID + "',\n" +
            "'" + clvar.RecoveryExpID + "',\n" +
            "'" + clvar.SalesRoute + "',\n" +
            "'" + clvar.RecoveryOfficer + "',\n" +
            "'" + clvar.SalesExecutive + "',\n" +
            "'" + clvar.RedeemWindow + "',\n" +
            "'" + clvar.OverdueCalculationBase + "',\n" +
            "'" + clvar.OverdueValue + "',\n" +
            "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
            " GETDATE(),\n" +
            "'" + clvar.Sector + "',\n" +
            "'" + clvar.Industry + "',\n" +
            "'" + clvar.AccountNo + "',\n" +
            "'" + clvar.CreditClientType + "',\n" +
            "'" + HttpContext.Current.Session["ZoneCode"].ToString() + "',\n" +
            "'" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
            "'" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
            "'" + clvar.NTNNo + "',\n" +
            "'" + clvar.IsCOD + "',\n" +
            "'" + clvar.IsActive + "',\n" +
            "'" + clvar.IsSpecial + "',\n" +
            "'" + clvar.IsFranchise + "',\n" +
            "'" + clvar.RecoveryOfficerName + "',\n" +
             "'" + clvar.RecoveryOfficerID + "',\n" +
            "'" + clvar.IsNationWide + "',\n" +
            "'" + clvar.IsParent + "',\n" +
            "'" + clvar.MailingAddress + "',\n" +
            "'" + clvar.OriginEc + "',\n" +
            "'" + clvar.StatusCode + "',\n" +
            "'" + clvar.CODType + "',\n" +
            "'" + clvar.MonthlyFixedCharges + "',\n" +
            "'" + clvar.IsSmsServiceActive + "',\n" +
            "'" + clvar.RnRCharges + "',\n" +
            "'" + clvar.HSCNCharges + "'\n" +
            "   )";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlString, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return "";
        }
    }
}