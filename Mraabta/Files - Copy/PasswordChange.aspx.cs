using MRaabta.App_Code;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace MRaabta.Files
{
    public partial class PasswordChange : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        // bayer_Function b_fun = new bayer_Function();
        Password_Change b_fun = new Password_Change();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_UserInfo();
            }
        }


        protected void Get_UserInfo()
        {
            clvar._UserName = Session["U_NAME"].ToString();

            DataSet ds = b_fun.Get_UserEmailAddress(clvar);

            if (ds.Tables[0].Rows.Count != 0)
            {
                txt_name.Text = ds.Tables[0].Rows[0]["NAME"].ToString();
                txt_salary_code.Text = ds.Tables[0].Rows[0]["U_CODE"].ToString();
                txt_last_date.Text = ds.Tables[0].Rows[0]["modify_date"].ToString();
                txt_expire_date.Text = ds.Tables[0].Rows[0]["INACTIVE_DATE"].ToString();
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_old_password.Text != "" && txt_new_password.Text != "")
            {
                clvar._UserName = Session["U_NAME"].ToString();
                clvar._OldUserName = "AND U_PASSWORD = '" + txt_old_password.Text + "' AND BTS_USER = '1'";

                DataSet ds = b_fun.Get_UserEmailAddress(clvar);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    int a = int.Parse(ds.Tables[0].Rows[0]["CHANGE_PASS_FLAG"].ToString());
                    int b = int.Parse("01");
                    clvar._Version = (a + b).ToString();

                    clvar._UserId = ds.Tables[0].Rows[0]["U_ID"].ToString();

                    if (ds.Tables[0].Rows[0]["U_PASSWORD"].ToString() == txt_new_password.Text)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                               "alert('Please Don't Insert Old Password...');", true);

                        error.Text = "Please Don't Insert Old Password...";

                        txt_old_password.Text = "";
                        txt_new_password.Text = "";
                    }
                    else
                    {
                        error.Text = "";
                        clvar._password = txt_new_password.Text;

                        b_fun.Update_PasswordChange(clvar);

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                               "alert('New Password has been Generated...');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                                "alert('Incorrect Password...');", true);
                    txt_old_password.Text = "";
                    txt_new_password.Text = "";
                }
            }
        }

    }
}