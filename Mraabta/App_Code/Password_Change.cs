using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using System.Data.SqlClient;

/// <summary>
/// Summary description for bayer_Function
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class Password_Change
    {

        public Password_Change()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        Cl_Variables clvar = new Cl_Variables();

        #region BTSCODE


        // Get User Id
        public DataSet Get_UserEmailAddress(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select convert(nvarchar(10), INACTIVE_DATE, 105) INACTIVE_DATE, convert(nvarchar(10), modify_date, 105) modify_date, * from ZNI_USER1 where UPPER(U_NAME) = UPPER('" + clvar._UserName + "') \n" + clvar._OldUserName;

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        // Update User Password
        public void Update_PasswordChange(Variable clvar)
        {
            try
            {
                string query = "UPDATE ZNI_USER1 SET U_PASSWORD = '" + clvar._password + "', CHANGE_PASS_FLAG = '" + clvar._Version + "', \n" +
                                " Modify_Date = GETDATE(), \n " +
                                " INACTIVE_DATE = '" + DateTime.Now.AddDays(30).ToString("yyyy-MM-dd") + "' \n " +
                                " WHERE U_ID = '" + clvar._UserId + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
        }


        #endregion

    }
}