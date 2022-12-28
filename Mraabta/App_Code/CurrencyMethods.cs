using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;


/// <summary>
/// Summary description for CurrencyMethods
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class CurrencyMethods
    {
        Cl_Variables clvar = new Cl_Variables();
        public CurrencyMethods()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //SqlConnection con = Connection.MakeConnection();




        public void InsertCurrecy(int bid, int tid, double rate, DateTime from, DateTime to)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand Currency = new SqlCommand("INSERT into [CurrencyConversions] (baseCurrencyId,toCurrencyId,Rate,fromDate,toDate) VALUES (@bcId, @tcId, @rate,@frmDate,@toDate)", con);
                Currency.Parameters.AddWithValue("@bcId", bid);
                Currency.Parameters.AddWithValue("@tcId", tid);
                Currency.Parameters.AddWithValue("@rate", rate);
                Currency.Parameters.AddWithValue("@frmDate", from);
                Currency.Parameters.AddWithValue("@toDate", to);
                Currency.ExecuteScalar();

            }
            catch (Exception e)
            {


            }
            finally { con.Close(); }
        }
        public DataTable Currency(int bid, int tid)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter dat = new SqlDataAdapter("Select f.Id,c.code,f.fromDate,f.toDate,f.Rate from Currencies c , CurrencyConversions f where (f.baseCurrencyId=@bid AND f.toCurrencyId=@tid And c.Id=2 And f.[status] is  null)", con);
                dat.SelectCommand.Parameters.AddWithValue("@bid", bid);
                dat.SelectCommand.Parameters.AddWithValue("@tid", tid);
                dat.Fill(dt);

                return dt;
            }
            finally { con.Close(); }
        }

        public void Delete(int id)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand Currency = new SqlCommand("update CurrencyConversions set [status] = 0 where Id =@Id  ", con);
                Currency.Parameters.AddWithValue("@Id", id);
                Currency.ExecuteScalar();
            }
            catch (Exception e)
            {
            }
            finally { con.Close(); }
        }

        public void UpdateCurrencyConversion(int id, int bid, int tid, double rate, DateTime from, DateTime to)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand Currency = new SqlCommand("update CurrencyConversions set baseCurrencyId=@bid, toCurrencyId=@tid , Rate=@rate,fromDate=@fDate ,toDate=@tDate where Id=@id", con);
                Currency.Parameters.AddWithValue("@id", id);
                Currency.Parameters.AddWithValue("@bid", bid);
                Currency.Parameters.AddWithValue("@tid", tid);
                Currency.Parameters.AddWithValue("@rate", rate);
                Currency.Parameters.AddWithValue("@fDate", from);
                Currency.Parameters.AddWithValue("@tDate", to);
                Currency.ExecuteScalar();
            }
            catch (Exception e)
            {
            }
            finally { con.Close(); }

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
        public DataTable GetClientStaff(Cl_Variables clvar)
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

        public string AddCustomer(Cl_Variables clvar, DataTable modifiers, DataTable staff)
        {
            string query = "";

            return "";
        }


    }



}