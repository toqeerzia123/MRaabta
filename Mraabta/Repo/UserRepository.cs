using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;

namespace MRaabta.Repo
{
    public class UserRepository
    {
        SqlConnection con;
        public UserRepository()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task<UserModel> Login(string uname, string pass)
        {
            try
            {
                await con.OpenAsync();
                var user = await con.QueryFirstOrDefaultAsync<UserModel>(@"SELECT u.[U_CODE] as [Code],
                                                                            u.[U_ID] as [Uid],
                                                                            u.[U_NAME] as [UserName],
                                                                            u.[U_PASSWORD] as [Password],
                                                                            u.[PROFILE] as [Profile],
                                                                            UPPER(u.NAME) AS [Name],
                                                                            u.[U_TYPE] as [Type],
                                                                            u.[STATUS] as [Status],
                                                                            u.[ACTIVE_DATE] as [ActivityDate],
                                                                            u.[INACTIVE_DATE] as [InactiveDate],
                                                                            u.[CHANGE_PASS_FLAG] as [ChangePassFlag],
                                                                            u.[USER_MAC_ADD] as [UserMacAddress],
                                                                            u.[EXCEL_PERMISSION] as [ExcelPermission],
                                                                            u.[DSG_CODE] as [DsgCode],
                                                                            u.[ZONECODE] as [ZoneCode],
                                                                            u.[bts_user] as [BTSUser],
                                                                            u.[BRANCHCODE] as [BranchCode],
                                                                            b.name as [BranchName],
                                                                            u.[ExpressCenter] as [ExpressCenter],
                                                                            e.workingdate as [WorkingDate],
                                                                            e.name as [ExpressCenterName],
                                                                            t.id as [LocationId],
                                                                            t.Name as [LocationName]
                                                                            FROM ZNI_USER1 u
                                                                            left join Branches b on b.branchCode = u.branchcode
                                                                            left outer JOIN EXPRESSCENTERs e ON e.expressCenterCode = u.ExpressCenter
                                                                            left outer join mnp_locations t on u.locationid = t.id
                                                                            WHERE UPPER(u.U_NAME) = @uname AND u.U_PASSWORD = @pass AND bts_user = '1' and isnull(u.status , 0) = '1';", new { uname, pass });
                con.Close();
                return user;
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
        }
        public List<MenuModel> GetMenues(int uid)
        {
            try
            {
                con.Open();
                var menus = con.Query<MenuModel>(@"select m.Menu_Id as [MenuId],
                                                            m.Menu_Name as [MenuName],
                                                            u.Profile as [Profile]
                                                            from Profile_Head ph , Profile_Detail pd, main_menu m, ZNI_USER1 u
                                                            where 
                                                            u.Profile = ph.Profile_Id
                                                            and ph.Profile_Id = pd.Profile_Id
                                                            and pd.MainMenu_Id = m.Menu_Id
                                                            and u.U_ID = @uid
                                                            and m.Status = '1'
                                                            and u.bts_User = '1'
                                                            group by
                                                            m.Menu_Id, m.Menu_Name, u.U_NAME, u.Profile, m.HyperLink
                                                            Order by
                                                            m.Menu_Id, m.Menu_Name, u.U_NAME, u.Profile, m.HyperLink", new { uid });

                var submenus = con.Query<SubMenuModel>(@"SELECT m.Menu_id as [MenuId],
                                                                    pd.Profile_Id as [ProfileId],
                                                                    c.sub_menu_name as [SubMenuName],
                                                                    c.Hyperlink  as [Hyperlink]
                                                                    FROM   main_menu m
                                                                    INNER JOIN child_menu c ON  c.main_Menu_id = m.Menu_id
                                                                    INNER JOIN Profile_Detail pd
                                                                    ON  pd.ChildMenu_Id = c.Child_MenuId
                                                                    WHERE c.status='1' and m.Menu_id in @mids and pd.Profile_Id in @pids;", new { @mids = menus.Select(x => x.MenuId).ToList(), @pids = menus.Select(x => x.Profile).ToList() });

                con.Close();

                foreach (var menu in menus)
                {
                    menu.SubMenus = submenus.Where(x => x.MenuId == menu.MenuId && x.ProfileId == menu.Profile).ToList();
                }

                return menus.ToList();
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
        }

        public async Task<List<int>> GetDeliveryProfiles(int id)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<int>(@"select distinct Profile_Id from Profile_Detail where ChildMenu_Id = @id;", new { id });
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                return null;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                return null;
            }
        }

        public bool IsAllowed(int uid, int profile, string controller)
        {
            try
            {
                con.Open();
                var query = $@"select count(*) from ZNI_USER1 u 
                            inner join Profile_Detail pd on u.Profile = pd.Profile_Id
                            inner join Child_Menu cm on pd.ChildMenu_Id = cm.Child_MenuId
                            where u.U_ID = {uid} and u.Profile = {profile} and LOWER(cm.Controller) = '{controller}';";
                var rs = con.QueryFirstOrDefault<int>(query);
                con.Close();
                return rs > 0;
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                return false;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                return false;
            }
        }
    }
}