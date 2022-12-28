using System;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace MRaabta
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            Uri uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
            string pageGetURL = uri.Segments.Last();
            bool authorized = false;
            if (Session["User_Info"] != null)
            {
                DateTime ExpireDate = DateTime.Parse(Session["ExpiryDate"].ToString());
                DateTime CurrentDate = DateTime.Today;
                TimeSpan difference = ExpireDate.Date - CurrentDate.Date;
                int days = (int)difference.TotalDays;

                lbl_expire_date.Text = "PASSWORD EXPIRY DAYS LEFT : " + days;
                lbl_username.Text = "WELCOME TO : " + Session["U_NAME"].ToString();
                lbl_expresscenter.Text = "EXPRESS CENTER NAME : " + Session["EXPRESSCENTERNAME"].ToString() + "(" + Session["ExpressCenter"].ToString() + ")";
                lbl_working_date.Text = "WORKING DATE: " + Session["WorkingDate"].ToString();



                SqlDataAdapter da = new SqlDataAdapter("select m.Menu_Id, m.Menu_Name, u.U_NAME, u.Profile, m.HyperLink \n " +
                                                        "from Profile_Head ph , Profile_Detail pd, main_menu m, ZNI_USER1 u\n " +
                                                        "where \n " +
                                                        "u.Profile = ph.Profile_Id\n " +
                                                        "and ph.Profile_Id = pd.Profile_Id\n " +
                                                        "and pd.MainMenu_Id = m.Menu_Id\n " +
                                                        "and u.U_ID = '" + Session["U_ID"].ToString() + "' \n " +
                                                        "and m.Status = '1' \n" +
                                                        "and u.bts_User = '1' \n" +
                                                        "group by \n " +
                                                        "m.Menu_Id, m.Menu_Name, u.U_NAME, u.Profile, m.HyperLink\n " +
                                                        "Order by\n " +
                                                        "m.Menu_Id, m.Menu_Name, u.U_NAME, u.Profile, m.HyperLink", con);

                DataTable dttc = new DataTable();
                da.Fill(dttc);
                HtmlGenericControl main = UList(null, "Menuid", "mainmenu");

                foreach (DataRow row in dttc.Rows)
                {
                    string sql = "	SELECT c.child_menuid, c.sub_menu_name, c.Child_MenuId, c.Hyperlink sublink"
                       + "	    FROM   main_menu m \n"
                       + "	    INNER JOIN child_menu c ON  c.main_Menu_id = m.Menu_id \n"
                       + "		INNER JOIN Profile_Detail pd ON  pd.ChildMenu_Id = c.Child_MenuId \n"
                       + "	    WHERE  m.Menu_id = '" + row["MENU_ID"].ToString() + "' AND pd.Profile_Id = '" + row["Profile"].ToString() + "' ";
                    da = new SqlDataAdapter(sql, con);

                    DataTable dtDist = new DataTable();
                    da.Fill(dtDist);
                    if (dtDist.Rows.Count > 0)
                    {
                        HtmlGenericControl sub_menu = LIList(row["MENU_NAME"].ToString(), row["MENU_ID"].ToString(), row["HyperLink"].ToString());
                        HtmlGenericControl ul = new HtmlGenericControl("ul");
                        foreach (DataRow r in dtDist.Rows)
                        {
                            #region MyRegion
                            ////string a =  r["sublink"].ToString();
                            ////string b = pageGetURL;
                            ////if (a == b)
                            ////{
                            //ul.Controls.Add(LIList(r["sub_menu_name"].ToString(), r["Child_MenuId"].ToString(), r["sublink"].ToString()));
                            ////}                                
                            #endregion
                            if (r["Sublink"].ToString().ToLower() == "prebookingdataloader.aspx")
                            {
                                ul.Controls.Add(NewTabLIList(r["sub_menu_name"].ToString(), r["Child_MenuId"].ToString(), r["sublink"].ToString()));
                            }
                            else
                            {
                                ul.Controls.Add(LIList(r["sub_menu_name"].ToString(), r["Child_MenuId"].ToString(), r["sublink"].ToString()));
                            }
                        }
                        sub_menu.Controls.Add(ul);
                        main.Controls.Add(sub_menu);
                    }
                    else
                    {
                        main.Controls.Add(LIList(row["MENU_NAME"].ToString(), row["MENU_ID"].ToString(), row["HyperLink"].ToString()));
                    }
                }
                Panel1.Controls.Add(main);

                if (pageGetURL.ToString().ToLower() != "btsdashoard.aspx")
                {
                    authorized = CheckExceptionScreen(HttpContext.Current.Session["Profile"].ToString(), pageGetURL.ToString());
                    Page_Usage_Log(Session["U_ID"].ToString(), pageGetURL, uri, "MRaabta", "MasterPage.master");
                }
                else
                {
                    authorized = true;
                }
                if (!authorized)
                {
                    InsertUnauthorizedAccessLog(Session["U_ID"].ToString(), uri.ToString());
                }
            }
            else
            {
                Response.Redirect("~/login");
            }
        }


        private void Page_Usage_Log(string U_ID, string pageGetURL, Uri uri, String systemType, String originName)
        {
            string query = "Insert into Page_Usage_log (U_ID,CreatedOn,URL,systemType,originName) values (@U_ID,getDate(),@URL,@systemType,@originName) ";

            // SqlConnection con = new SqlConnection(con);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@U_ID", U_ID);
                cmd.Parameters.AddWithValue("@URL", pageGetURL);
                cmd.Parameters.AddWithValue("@systemType", systemType);
                cmd.Parameters.AddWithValue("@originName", originName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
        }
        private HtmlGenericControl UList(object name, string id, string cssClass)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.ID = id;
            ul.Attributes.Add("class", cssClass);
            return ul;
        }

        protected void lnk_logout_Click(object sender, EventArgs e)
        {
            Session.Remove("NAME");
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("~/login");
        }

        private HtmlGenericControl LIList(string innerHtml, string rel, string url)
        {
            string path = Request.Url.Authority + Request.ApplicationPath + "/Files/";

            HtmlGenericControl li = new HtmlGenericControl("li");
            li.Attributes.Add("rel", rel);
            li.InnerHtml = "<a href=" + string.Format("http://" + path + "{0}", url) + ">" + innerHtml + "</a>";
            return li;
        }

        private HtmlGenericControl NewTabLIList(string innerHtml, string rel, string url)
        {
            string path = Request.Url.Authority + Request.ApplicationPath + "/Files/";

            HtmlGenericControl li = new HtmlGenericControl("li");
            li.Attributes.Add("rel", rel);
            li.InnerHtml = "<a href=" + string.Format("http://" + path + "{0}", url) + " target='_blank'>" + innerHtml + "</a>";
            return li;
        }



        private void InsertUnauthorizedAccessLog(string userID, string URL)
        {
            string query = "Insert into MnP_UnauthorizedAccessLog Values('" + userID + "','" + URL + "', GETDATE())";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            { }
            finally { con.Close(); }

        }

        private bool CheckExceptionScreen(string profile, string child)
        {
            bool resp = false;
            string sqlString = "SELECT a.Hyperlink\n" +
            "  FROM (SELECT cm.Hyperlink\n" +
            "          FROM Profile_Detail pd\n" +
            "         INNER JOIN Child_Menu cm\n" +
            "            ON cm.Child_MenuId = pd.ChildMenu_Id\n" +
            "         WHERE pd.Profile_Id = '" + profile + "'\n" +
            "           AND cm.BTSPanel = '1'\n" +
            "           AND cm.status = '1'\n" +
            "        UNION ALL\n" +
            "\n" +
            "        SELECT se.ChildURL hyperlink\n" +
            "          FROM profile_detail pd\n" +
            "         INNER JOIN Child_Menu cm\n" +
            "            ON cm.Child_MenuId = pd.ChildMenu_Id\n" +
            "         INNER JOIN Screens_Exceptions se\n" +
            "            ON se.ParentURL = cm.Hyperlink\n" +
            "         WHERE pd.Profile_Id = '" + profile + "'\n" +
            "           AND cm.BTSPanel = '1'\n" +
            "           AND cm.status = '1') A\n" +
            " WHERE A.Hyperlink = '" + child + "'";

            SqlConnection con_ = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            DataTable dt = new DataTable();

            try
            {
                con_.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con_);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    resp = true;
                }
                else
                {
                    resp = false;
                }
            }
            catch (Exception ex)
            { resp = false; }
            finally { con.Close(); }
            return resp;
        }
    }
}