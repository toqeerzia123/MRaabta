using System;
using System.Web.UI;

namespace MRaabta.Files
{
    public partial class Search_Demanifest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (txt_date.Text == "")
            {
                Alert("Provide Date");
                return;
            }

            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script = String.Format(script_, "PrintDemanifest.aspx?Date=" + txt_date.Text, "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }

        protected void Alert(string MEssage)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + MEssage + "')", true);
            ErrorID.Text = MEssage;
        }
    }
}