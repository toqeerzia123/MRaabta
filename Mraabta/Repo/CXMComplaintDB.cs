using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRaabta.Repo
{
    public class CXMComplaintDB
    {
        SqlConnection orcl;
        string assignedToLevel = null;

        public CXMComplaintDB()
        {
            orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        public async Task OpenAsync()
        {
            await orcl.OpenAsync();
        }
        
        public void Close()
        {
            orcl.Close();
        }

        internal CXMConsignment GetConsignmentDetails(string consignmentNumber)
        {
            DataTable dt = new DataTable();
            try
            {
                var rs =  orcl.QueryFirstOrDefault<CXMConsignment>(@"
                        SELECT  ob.name OriginName,ob.branchCode OriginCode,od.name destinationName,od.branchCode DestinationCode,oz.name [AllocationZoneName],oz.zoneCode [AllocationZoneCode]
                            ,c.consigner [InquirerName],c.consignerEmail [InquirerEmail]
                            ,c.consignerPhoneNo [InquirerPhoneNumber],c.consignerCellNo [InquirerCellNumber]
                            ,c.consignerAccountNo [AccountNo] ,c.weight [Weight],c.pieces [Pieces]
                            ,c.consigner [ShipperName] ,c.address [ShipperAddress],c.consignerCellNo [ShipperCell]
                            ,c.consignee [Consignee],c.consigneePhoneNo [ConsigneeCell],c.Address2 [ConsigneeAddress]
                                FROM   Consignment c
                           INNER JOIN Branches ob ON ob.branchCode=c.orgin
                           INNER JOIN Branches od ON od.branchCode=c.destination 
                           INNER JOIN zones oz ON oz.zoneCode=ob.zoneCode
                            where c.ConsignmentNumber=@ConsignmentNumber ", new { @ConsignmentNumber = consignmentNumber});
                rs.isSuccess = true;
                return rs;
            }
            catch (Exception er)
            {
                return new CXMConsignment();
            }

        }

        internal DataTable GetFixedFieldsCXMComplaint()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = @"     
                              select rt_ID, rt_name 
                              from CSD_RequestType where rn_ID =2 
                              order by rt_name ASC
                               ";
                 orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception ex)
            { }
            return dt;
        }

        internal DataTable GetCXMStandardNotes(string requestTypeValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = @" select note_ID, note_name 
                         from CSD_Request_StandardNotes where rt_ID = '" + requestTypeValue + @"'  
                         order by note_name ASC";

                 orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception ex)
            { }
            return dt;
        }

       

        internal Response_CXMComplaintSave CXMComplaint_SaveToDataBase(CXMComplaintConsignmentDetails stats)
        {
            Response_CXMComplaintSave list = new Response_CXMComplaintSave();
            try
            {
                List<string> queries = new List<string>();
                string consignment = stats.ConsignmentNumber;
                bool Duplicate = CXMComplaint_CheckDuplication(stats.ConsignmentNumber);
                if (!Duplicate)
                {
                    list.isSuccess = false;
                    list.message = "Consignment complaint already exists. Cannot submit again";
                    return list;
                }
                string RequestType = stats.RequestType;
                string InquirerType = stats.InquirerType;
                string inquirerName = stats.InquirerName;
                string phone = stats.PhoneNumber;
                string Cell = stats.CellNumber;
                string email = stats.EmailId;
                string weight = stats.Weight;
                if (weight == "")
                {
                    weight = "0";
                }
                string pieces = stats.Pieces;
                if (pieces == "")
                {
                    pieces = "0";
                }
                string shipper = stats.ShipperName;
                string shipperCell = stats.ShipperCell;
                string shipperAddress = stats.ShipperAddress;
                string Consignee = stats.ConsigneeName;
                string ConsigneeCell = stats.ConsigneeCell;
                string ConsigneeAddress = stats.ConsigneeAddress;
                int callBack = 0;

                string media = null;
                media = stats.SourceMedia;
                if (media == "Select")
                {
                    media = "-";
                }

                string Initialcallback = "";

                int userID = 3095;
                string Date = DateTime.Now.ToString();
                string userName = "Tasneem.TSS";
                string createdBylevel = "4";

                // string accountNum = ddl_account.SelectedValue.ToString();
                string accountNum = stats.AccountNo;
                string Destination = stats.Destination;
                string Origin = stats.Origin;
                string description = stats.Description;
                string assignedTo = null;

                string priority = "1";
                string allocationZone = stats.AllocationBy;
                string defaulterArea = getDefaulterZone(allocationZone);
                int nr_ID;
                int status;

                nr_ID = int.Parse(stats.RequestNature);
                status = 1;

                string note = null;
                string Dept = stats.Department;

                if (stats.StandardNotes != "" || stats.StandardNotes != null)
                {
                    note = stats.StandardNotes;
                }

                bool check = false;
                if (check == false)
                {
                    /////////////////////asigning agent by account//////////////////////////////////
                    if (check == false)
                    {
                        //DataTable dt = getAssignID(consignment); // get account Number by consignmentNumber
                        //if (dt.Rows.Count != 0)
                        //{
                        //                accountNum = dt.Rows[0].Field<string>(0);

                        DataTable assignedTo1 = AgentIDByAccountNum(accountNum); //// get account Number by AgentID
                        if (assignedTo1.Rows.Count != 0)
                        {
                            int startNum = 0;
                            int endNum = assignedTo1.Rows.Count;
                            Random ram = new Random();
                            int i = ram.Next(startNum, endNum);
                            assignedTo = assignedTo1.Rows[i].Field<int>("User_ID").ToString();

                            //int v = assignedTo1.Rows[0].Field<int>(0);
                            //  assignedTo = v.ToString();

                            check = true;
                            assignedToLevel = "4";
                        }
                    }

                    //}

                    /////////////////Asigning agent by Destination//////////////////////////////
                    if (check == false)
                    {
                        //DataTable dt1 = getDestByConNum(consignment);
                        //if (dt1.Rows.Count != 0)
                        //{
                        //    Destination = dt1.Rows[0].Field<string>(0);
                        DataTable assignedTo1 = AgentIDByDestination(Destination);
                        if (assignedTo1.Rows.Count != 0)
                        {
                            int startNum = 0;
                            int endNum = assignedTo1.Rows.Count;
                            Random ram = new Random();
                            int i = ram.Next(startNum, endNum);
                            assignedTo = assignedTo1.Rows[i].Field<int>("User_ID").ToString();

                            //int v = assignedTo1.Rows[0].Field<int>(0);
                            //  assignedTo = v.ToString();

                            check = true;
                            assignedToLevel = "4";
                        }
                    }

                    /////////////////Asigning agent by Allocation Zone//////////////////////////////
                    if (check == false)
                    {
                        string selectedZone = stats.AllocationBy;
                        DataTable assignedTo1 = AgentIDZone(selectedZone);
                        if (assignedTo1.Rows.Count != 0)
                        {
                            int startNum = 0;
                            int endNum = assignedTo1.Rows.Count;
                            Random ram = new Random();
                            int i = ram.Next(startNum, endNum);
                            assignedTo = assignedTo1.Rows[i].Field<int>("User_ID").ToString();
                            //int v = assignedTo1.Rows[0].Field<int>(0);
                            //  assignedTo = v.ToString();

                            check = true;
                            assignedToLevel = "4";
                        }

                    }

                    /////////////////Asigning agent by Team Member//////////////////////////////
                    if (check == false)
                    {

                        DataTable assignedTo1 = AgentID();
                        if (assignedTo1.Rows.Count != 0)
                        {
                            int startNum = 0;
                            int endNum = assignedTo1.Rows.Count;
                            Random ram = new Random();
                            int i = ram.Next(startNum, endNum);
                            assignedTo = assignedTo1.Rows[i].Field<int>("User_ID").ToString();
                            //int v = assignedTo1.Rows[0].Field<int>(0);
                            //  assignedTo = v.ToString();

                            check = true;


                        }
                        else
                        {
                        }
                        //else
                        //{ ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('There is no Back Line Agent Available. Kindly Create Backline Agent'); window.location='" + Request.ApplicationPath + "/NewRequest.aspx';", true); }


                    }
                }

                if (check == true)
                {
                    string succes = insertRequest(RequestType, consignment, InquirerType, inquirerName, phone, Cell, email, callBack, media, Initialcallback, userID, Date, userName, status, assignedTo, createdBylevel, priority, nr_ID, note, accountNum, Origin, Destination, description, 0, allocationZone, assignedToLevel, Dept, weight, pieces, shipper, shipperCell, shipperAddress, Consignee, ConsigneeCell, ConsigneeAddress, defaulterArea);

                    if (succes == "Succes")
                    {
                        string reqID = null;
                        DataTable ID = GetID();
                        if (ID.Rows.Count != 0)
                        {
                            int v = ID.Rows[0].Field<int>(0);
                            reqID = v.ToString();
                        }
                        list.message = " New Request Generated. Request ID: " + reqID + "!! ";
                        list.isSuccess = true;
                    }
                    else
                    {
                        list.message = " ERROR ";
                        list.isSuccess = true;
                    }


                }

                if (check == false)
                {
                    assignedTo = "";
                    string succes = insertRequest(RequestType, consignment, InquirerType, inquirerName, phone, Cell, email, callBack, media, Initialcallback, userID, Date, userName, status, assignedTo, createdBylevel, priority, nr_ID, note, accountNum, Origin, Destination, description, 0, allocationZone, assignedToLevel, Dept, weight, pieces, shipper, shipperCell, shipperAddress, Consignee, ConsigneeCell, ConsigneeAddress, defaulterArea);
                    if (succes == "Succes")
                    {
                        string reqID = null;
                        DataTable ID = GetID();
                        if (ID.Rows.Count != 0)
                        {
                            int v = ID.Rows[0].Field<int>(0);
                            reqID = v.ToString();
                        }
                        list.message = "New Request Generated. Request ID: " + reqID + "!!";
                        list.isSuccess = true;
                    }
                }

                else
                { return list; }
                list.message = "Success";
                list.isSuccess = true;
            }
            catch (Exception er)
            {
                list.message = er.Message.ToString();
                list.isSuccess = false;
            }
            finally
            {
                orcl.Close();
            }
            return list;
        }
        public DataTable GetID()
        {
            
            DataTable dt = new DataTable();

            try
            {

                string sqlString = "select top 1 nr_ID from CSD_NewRequest order by nr_ID desc";

                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { orcl.Close(); }
            return dt;

        }

        private bool CXMComplaint_CheckDuplication(string consignment)
        {
            bool chk_Cn_Duplication = true;
            DataTable dt = new DataTable();
            try
            {
                string sqlString = "select * from CSD_NewRequest where consignmentNum ='" + consignment + "' and rn_ID <> 7";
                orcl.Open();
                SqlCommand orcd1 = new SqlCommand(sqlString, orcl);
                orcd1.CommandType = CommandType.Text;
                SqlDataAdapter oda1 = new SqlDataAdapter(orcd1);
                oda1.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    chk_Cn_Duplication = false;
                }
                else { chk_Cn_Duplication = true; }
            }
            catch (Exception Err)
            {
                chk_Cn_Duplication = false;
            }
            finally
            { orcl.Close(); }
            return chk_Cn_Duplication;
        }

        private string getDefaulterZone(string allocationZone)
        {
            string area = "";
            DataTable dt = new DataTable();
            try
            {
                string sqlString = "select * from [CSD_DefaulterArea] where zonecode = '" + allocationZone + "'";

                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
                area = dt.Rows[0][0].ToString();
            }
            catch (Exception Err)
            { }
            finally
            { orcl.Close(); }
            return area;
        }


        public string insertRequest(string RequestType, string consignment, string InquirerType, string inquirerName, string phone, string cell, string email, int callBack, string media, string Initialcallback, int userID, string Date, string userName, int status, string assignedTo, string createdBylevel, string priority, int rn_ID, string note_ID, string account, string origin, string destinantion, string description, int sms, string allocationZone, string assignedToLevel, string Dept, string weight, string pieces, string shipper, string shipperCell, string shipperAddress, string Consignee, string ConsigneeCell, string ConsigneeAddress, string defaulterArea)
        {
         
            string temp = "";
            try
            {
                string sqlString = "INSERT INTO [CSD_NewRequest]\n" +
            "           ([RequestType],[ConsignmentNum]\n" +
            "           ,[InquirerType]\n" +
            "           ,[nr_InquirerName]\n" +
            "           ,[nr_Phone]\n" +
            "           ,[nr_Cell]\n" +
            "           ,[nr_Email]\n" +
            "           ,[nr_sourceMedia]\n" +
            "           ,[nr_callBack]\n" +
            "           ,[nr_initialCallBack]\n" +
            "           ,[nr_createdBy]\n" +
            "           ,[nr_CreatedDate]\n" +
            "           ,[nr_status]\n" +
            "           ,[nr_AssignedTo]\n" +
            "           ,[nr_priority]\n" +
            "           ,[rn_ID]\n" +
            "           ,[nr_note]\n" +
            "           ,[nr_account]\n" +
            "           ,[nr_origin]\n" +
            "           ,[nr_destination]\n" +
            "           ,[nr_description]\n" +
            "           ,[nr_SMS]\n" +
            "           ,[nr_allocationZone]\n" +
            "           ,[nr_emailStatus]\n" +
            "           ,[nr_AssignedToLevel], nr_dept,weight, pieces,shipper,shipperCell,shipperAddress,consignee,consigneeCell,consigneeAddress,nr_DefaulterArea,EntrySystem)\n" +
            "           OUTPUT INSERTED.nr_ID,\n" +
            "           '" + RequestType + "',\n" +
            "           '" + consignment + "',\n" +
            "           '" + InquirerType + "',\n" +
            "           '" + inquirerName + "',\n" +
            "           '" + phone + "',\n" +
            "           '" + cell + "',\n" +
            "           '" + email + "',\n" +
            "           '" + media + "',\n" +
            "           '" + callBack + "',\n" +
            "           '" + Initialcallback + "',\n" +
            "           '" + userID + "',\n" +
            "            GETDATE(),\n" +
            //"           '" + zone + "',\n" +
            "           '" + status + "',\n" +
            "           '" + assignedTo + "',\n" +
            "           '" + priority + "',\n" +
            "           '" + rn_ID + "',\n" +
            "           '" + note_ID + "',\n" +
            "           '" + account + "',\n" +
            "           '" + origin + "',\n" +
            "           '" + destinantion + "',\n" +
            "           '" + description + "',\n" +
            "           '" + sms + "',\n" +
            "           '" + allocationZone + "',\n" +
            "           '1',\n" +
            "           '" + int.Parse(assignedToLevel) + "','" + int.Parse(Dept) + "','" + double.Parse(weight) + "','" + int.Parse(pieces) + "','" + shipper + "','" + shipperCell + "','" + shipperAddress + "','" + Consignee + "','" + ConsigneeCell + "','" + ConsigneeAddress + "','" + defaulterArea + "',2\n" +
            "           INTO CSD_NewRequest_history\n" +
            "           ([nr_ID],[RequestType],[ConsignmentNum]\n" +
            "           ,[InquirerType]\n" +
            "           ,[nr_InquirerName]\n" +
            "           ,[nr_Phone]\n" +
            "           ,[nr_Cell]\n" +
            "           ,[nr_Email]\n" +
            "           ,[nr_sourceMedia]\n" +
            "           ,[nr_callBack]\n" +
            "           ,[nr_initialCallBack]\n" +
            "           ,[nr_createdBy]\n" +
            "           ,[nr_CreatedDate]\n" +
            "           ,[nr_status]\n" +
            "           ,[nr_AssignedTo]\n" +
            "           ,[nr_priority]\n" +
            "           ,[rn_ID]\n" +
            "           ,[nr_note]\n" +
            "           ,[nr_account]\n" +
            "           ,[nr_origin]\n" +
            "           ,[nr_destination]\n" +
            "           ,[nr_description]\n" +
            "           ,[nr_SMS]\n" +
            "           ,[nr_allocationZone]\n" +
            "           ,[nr_emailStatus]\n" +
            "           ,[nr_AssignedToLevel], nr_dept,weight, pieces,shipper,shipperCell,shipperAddress,consignee,consigneeCell,consigneeAddress,nr_DefaulterArea,EntrySystem)\n" +
            "     VALUES (\n" +
            "           '" + RequestType + "',\n" +
            "           '" + consignment + "',\n" +
            "           '" + InquirerType + "',\n" +
            "           '" + inquirerName + "',\n" +
            "           '" + phone + "',\n" +
            "           '" + cell + "',\n" +
            "           '" + email + "',\n" +
            "           '" + media + "',\n" +
            "           '" + callBack + "',\n" +
            "           '" + Initialcallback + "',\n" +
            "           '" + userID + "',\n" +
            "            GETDATE(),\n" +
            //"           '" + zone + "',\n" +
            "           '" + status + "',\n" +
            "           '" + assignedTo + "',\n" +
            "           '" + priority + "',\n" +
            "           '" + rn_ID + "',\n" +
            "           '" + note_ID + "',\n" +
            "           '" + account + "',\n" +
            "           '" + origin + "',\n" +
            "           '" + destinantion + "',\n" +
            "           '" + description + "',\n" +
            "           '" + sms + "',\n" +
            "           '" + allocationZone + "',\n" +
            "           '1',\n" +
            "           '" + int.Parse(assignedToLevel) + "','" + int.Parse(Dept) + "','" + double.Parse(weight) + "','" + int.Parse(pieces) + "','" + shipper + "','" + shipperCell + "','" + shipperAddress + "','" + Consignee + "','" + ConsigneeCell + "','" + ConsigneeAddress + "','" + defaulterArea + "',2)";

                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.ExecuteNonQuery();
                orcl.Close();
                temp = "Succes";
            }
            catch (Exception Err)
            {
                temp = "Error";
            }
            finally
            { orcl.Close(); }

            return temp;
        }

        public DataTable AgentID()
        {
            DataTable dt1_ = new DataTable();
            DataTable dt1 = new DataTable();
            string sqlString;
            string assignedTo = null;

            try
            {

                sqlString = "  select  Agent3_ID from CSD_LevelAgent_4 where Agent4_ID='4'";
                orcl.Open();
                SqlCommand orcd1 = new SqlCommand(sqlString, orcl);
                orcd1.CommandType = CommandType.Text;
                SqlDataAdapter oda1 = new SqlDataAdapter(orcd1);
                oda1.Fill(dt1_);


                if (dt1_.Rows.Count != 0)
                {
                    int v = dt1_.Rows[0].Field<int>(0);
                    assignedTo = v.ToString();
                    assignedToLevel = "3";
                }
            }
            catch (Exception Err)
            {

            }
            finally
            { orcl.Close(); }

            try
            {
                // assignedTo = Session["Agent_ID"].ToString();
                sqlString = "select top 1 USER_ID  FROM   CSD_Users u \n" +
                            "inner join CSD_LevelAgent_4 a on u.Agent_ID = a.Agent4_ID \n" +
                            "where u.RoleType ='4' AND u.Agent_Level = '4' AND  a.Agent3_ID ='" + assignedTo + "' and u.status <> 0 and u.[Profile_ID] <> 6";

                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt1);
                assignedToLevel = "4";
            }

            catch (Exception Err)
            {

            }
            finally
            { orcl.Close(); }
            return dt1;

        }


        private DataTable AgentIDZone(string selectedZone)
        {
            DataTable dt = new DataTable();
            try
            {
                string sqlString = "";
                string Agent_Level = "4";
                //if (Agent_Level == "4")
                //{
                if (selectedZone == "10")
                {
                    sqlString = " Select u.User_ID  from CSD_Users u inner join CSD_LevelAgent_4 a\n" +
                           " on u.Agent_ID = a.Agent4_ID\n" +
                           " INNER JOIN csd_users_assignedzones ac ON ac.user_ID = u.User_ID\n" +
                           " left JOIN CSD_Users_AssignedAccounts cuaa ON cuaa.[USER_ID] = u.[USER_ID]\n" +
                           " where u.RoleType ='4' AND  u.Agent_level = '4' AND ac.zoneID = '" + selectedZone + "' and u.status <> 0 AND cuaa.[USER_ID] IS NULL  and ac.STATUS = '1'";
                }
                else
                {
                    sqlString = " Select u.User_ID  from CSD_Users u inner join CSD_LevelAgent_4 a\n" +
                                " on u.Agent_ID = a.Agent4_ID\n" +
                                " INNER JOIN csd_users_assignedzones ac ON ac.user_ID = u.User_ID\n" +
                                " left JOIN CSD_Users_AssignedAccounts cuaa ON cuaa.[USER_ID] = u.[USER_ID]\n" +
                                " where u.RoleType ='4' AND  u.Agent_level = '4' AND ac.zoneID = '" + selectedZone + "'  and u.User_ID <> " + 3095 + " and u.status <> 0 AND cuaa.[USER_ID] IS NULL  and ac.STATUS = '1'";
                }

                //  sqlString = "select  User_ID from CSD_Users u inner join CSD_LevelAgent_4 a on u.Agent_ID = a.Agent4_ID  where a.Designation='backline' and u.zoneCode = '" + selectedZone + "' and u.Agent_level = '4' and u.User_ID <> " + Session["User_ID"].ToString() + "";
                //  }
                //else
                ////{
                //sqlString = "select u.User_ID  from CSD_Users u inner join CSD_LevelAgent_4 a\n" +
                //              "on u.Agent_ID = a.Agent4_ID\n" +
                //              "INNER JOIN csd_users_assignedzones ac ON ac.user_ID = u.User_ID\n" +
                // " where a.Designation='backline' AND  u.Agent_level = '4' AND ac.zoneID = '" + selectedZone + "'  and u.User_ID <> " + Session["User_ID"].ToString() + "";

                //sqlString = "select  User_ID from  CSD_Users u inner join CSD_LevelAgent_4 a on u.Agent_ID = a.Agent4_ID  where a.Designation='backline' and u.zoneCode = '" + selectedZone + "'";
                //  }

                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { orcl.Close(); }
            return dt;
        }

        public DataTable AgentIDByDestination(string Destination)
        {
            DataTable dt = new DataTable();
        
            try
            {
                //string sqlString = "select Agent_ID ,u.DestinationCode\n" +
                //                   " from CSD_Users u inner join CSD_LevelAgent_4 a on u.Agent_ID = a.Agent4_ID \n" +
                //                   "where a.Designation='backline' AND u.Agent_Level = 4 \n" +
                //                   "and u.DestinationCode = '" + Destination + "'";


                string sqlString = "select u.User_ID  from CSD_Users u\n" +
                "INNER JOIN csd_users_assignedBranches ac ON ac.user_ID = u.User_ID\n" +
                "left JOIN CSD_Users_AssignedAccounts cuaa ON cuaa.[USER_ID] = u.[USER_ID]\n" +
                " where u.RoleType ='4' AND  u.Agent_level = '4' AND ac.branchID = '" + Destination + "' and u.User_ID <> " + 3095 + " and u.status <> 0 AND cuaa.[USER_ID] IS NULL and u.[Profile_ID] <> 6 and ac.status='1'";

                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);

            }
            catch (Exception Err)
            {

            }
            finally
            { orcl.Close(); }
            return dt;
        }

        public DataTable AgentIDByAccountNum(string accountNum)
        {
            DataTable dt = new DataTable();
            try
            {
                // string sqlString = "select User_ID from CSD_Users where Account_No = '" + accountNum + "' and Designation = 'Backline'";

                string sqlString = "select u.User_ID  from CSD_Users u \n" +
                                   "INNER JOIN csd_users_assignedaccounts ac ON ac.user_ID = u.User_ID\n" +
                                   "where u.RoleType ='4' AND  u.Agent_level = '4' \n" +
                                   "AND ac.accountNo =  '" + accountNum + "'  and ac.STATUS = '1' and u.status <> 0 -- and u.User_ID <> " + 3095 + " and u.[Profile_ID] <> 6";

                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { orcl.Close(); }
            return dt;
        }
    }

}