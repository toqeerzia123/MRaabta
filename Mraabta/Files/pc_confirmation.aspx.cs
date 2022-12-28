using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class pc_confirmation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != null)
                {

                    lbl_message.Text = "Your Entry Has Been Saved against ID: " + Request.QueryString["ID"].ToString() + "";
                }
                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"].ToString() == "EF" || Request.QueryString["type"].ToString() == "CIH")
                    {
                        btn.Visible = true;
                        Button1.Visible = true;
                    }

                }
            }
        }
        protected void btn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"].ToString() == "EF")
            {
                Response.Redirect("PettyCash_BranchEdit.aspx?head_id=" + Request.QueryString["ID"].ToString() + "&MODE=EDIT");
            }
            if (Request.QueryString["type"].ToString() == "CIH")
            {
                Response.Redirect("PettyCash_ReceiptEdit.aspx?head_id=" + Request.QueryString["ID"].ToString() + "&MODE=EDIT");
            }
            //  Response.Redirect("PettyCash_voucherEdit.aspx?head_id=" + Request.QueryString["ID"].ToString() + "&id=" + hf_detailID.Value + "&stat=BRANCH");

        }
        protected void btn_nextClick(object sender, EventArgs e)
        {
            if (Request.QueryString["type"].ToString() == "EF")
            {
                Response.Redirect("PettyCash_EF.aspx");
            }
            if (Request.QueryString["type"].ToString() == "CIH")
            {
                Response.Redirect("PettyCash_CIH.aspx");
            }
        }
    }
}