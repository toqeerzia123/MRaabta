using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Globalization;

/// <summary>
/// Summary description for Consignemnts
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class Consignemnts
    {
        public Consignemnts()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        Cl_Variables clvar = new Cl_Variables();

        public DataSet User(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM ZNI_USER1 z WHERE z.U_ID = '" + clvar.createdBy + "'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet Consignment(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM Consignment c WHERE c.consignmentNumber = '" + clvar.consignmentNo + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet CustomerInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM CreditClients cc WHERE cc.accountNo = '" + clvar.AccountNo + "' and cc.branchCode='" + clvar.Branch + "'";

                string sqlString = "SELECT cc.*,\n" +
                "       case\n" +
                "         when cu.accountNO is not null then\n" +
                "          '1'\n" +
                "         else\n" +
                "          '0'\n" +
                "       end isAPIClient\n" +
                "  FROM CreditClients cc\n" +
                "  left outer join CODUSERS cu\n" +
                "    on cu.accountno = cc.accountno\n" +
                "   and cu.creditCLientID = cc.id\n" +
                "\n" +
                " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
                "   and cc.branchCode = '" + clvar.Branch + "'\n" +
                "   and cc.isActive = '1'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ClientTarifInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT tm.*, convert(nvarchar(10), tm.toWeight) ToWeight_ \n"
                 + "  FROM [APL_BTS].[dbo].[tempClientTariff] tm, Branches b \n"
                 + "WHERE  \n"
                 + " tm.FromZoneCode = b.zoneCode \n"
                 + " AND b.branchCode ='" + clvar.origin + "' \n"
                 + " AND tm.ToZoneCode =(Select zoneCode FROM Branches WHERE Branches.branchCode = (SELECT ec.bid   \n"
                 + " FROM ExpressCenters ec WHERE ec.expressCenterCode = '" + clvar.Destination + "')) \n"
                 + " AND tm.ServiceID ='" + clvar.ServiceType + "' \n"
                 + " AND Client_Id ='" + clvar.AccountNo + "' \n"
                 + " and chkdeleted ='False'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ClientTarifInformation_(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT tm.*, convert(nvarchar(10), tm.toWeight) ToWeight_ \n"
                 + "  FROM [APL_BTS].[dbo].[tempClientTariff] tm, Branches b \n"
                 + "WHERE  \n"
                 + " tm.FromZoneCode = b.zoneCode \n"
                 + " AND b.branchCode ='" + clvar.origin + "' \n"
                 + " AND tm.ToZoneCode =(Select zoneCode FROM Branches WHERE Branches.branchCode = (SELECT ec.bid   \n"
                 + " FROM ExpressCenters ec WHERE ec.expressCenterCode = '" + clvar.Destination + "')) \n"
                 //+ " AND tm.ServiceID ='" + clvar.ServiceType + "' \n"
                 + " AND Client_Id ='" + clvar.AccountNo + "' \n"
                 + " and chkdeleted ='False'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ClientTarifInformationLocal(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT tm.*, convert(nvarchar(10), tm.toWeight) ToWeight_ \n"
                 + "  FROM [APL_BTS].[dbo].[tempClientTariff] tm, Branches b \n"
                 + "WHERE  \n"
                 + " tm.FromZoneCode = b.zoneCode \n"
                 + " AND b.branchCode ='" + clvar.origin + "' \n"
                 + " AND tm.ToZoneCode = '17' \n"
                 + " AND tm.ServiceID ='" + clvar.ServiceType + "' \n"
                 + " AND Client_Id ='" + clvar.AccountNo + "' \n"
                 + " and chkdeleted ='False'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ClientTarifInformationLocal_(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT tm.*, convert(nvarchar(10), tm.toWeight) ToWeight_ \n"
                 + "  FROM [APL_BTS].[dbo].[tempClientTariff] tm, Branches b \n"
                 + "WHERE  \n"
                 + " tm.FromZoneCode = b.zoneCode \n"
                 + " AND b.branchCode ='" + clvar.origin + "' \n"
                 + " AND tm.ToZoneCode = '17' \n"
                 //  + " AND tm.ServiceID ='" + clvar.ServiceType + "' \n"
                 + " AND Client_Id ='" + clvar.AccountNo + "' \n"
                 + " and chkdeleted ='False'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ProductTypeInfo(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM ProductType pt WHERE pt.CreditClientID ='" + clvar.CustomerClientID + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet Consignment_Consignee(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM Consignment c WHERE c.consigneePhoneNo like '%" + clvar.ConsigneeCell + "%'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet RiderInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM Riders r WHERE r.branchId ='" + clvar.origin + "' and ridercode='" + clvar.RiderCode + "' and r.status = '1'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet BranchGSTInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * \n"
               + "FROM   BranchGST bg \n"
               + "WHERE  bg.branchCode = '" + clvar.origin + "' \n"
               + "       AND bg.createdOn = ( \n"
               + "               SELECT MAX(bg2.createdOn) \n"
               + "               FROM   BranchGST bg2 \n"
               + "               WHERE  bg2.branchCode = '" + clvar.origin + "' \n"
               + "           )";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        // Insert Statement 
        public string Add_Consignment(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            //    obj.Call_Log_ID = int.Parse(Get_maxValue()) + 1;
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                // SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                // SqlParameter SParam = new SqlParameter("ReturnValue", SqlDbType.Int);
                // SParam.Direction = ParameterDirection.ReturnValue;
                //
                sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                sqlcmd.Parameters.AddWithValue("@consigner", obj.Consigner);
                sqlcmd.Parameters.AddWithValue("@consignee", obj.Consignee);
                sqlcmd.Parameters.AddWithValue("@couponNumber", obj.CouponNo);
                sqlcmd.Parameters.AddWithValue("@customerType", obj.Customertype);
                sqlcmd.Parameters.AddWithValue("@orgin", obj.origin);
                sqlcmd.Parameters.AddWithValue("@destination", obj.Destination);
                sqlcmd.Parameters.AddWithValue("@pieces", obj.pieces);
                sqlcmd.Parameters.AddWithValue("@serviceTypeName", obj.ServiceTypeName);
                sqlcmd.Parameters.AddWithValue("@creditClientId", obj.CustomerClientID);
                sqlcmd.Parameters.AddWithValue("@consignmentTypeId", obj.Con_Type);
                sqlcmd.Parameters.AddWithValue("@weight", obj.Weight);
                sqlcmd.Parameters.AddWithValue("@weightUnit", obj.Unit);
                sqlcmd.Parameters.AddWithValue("@discount", obj.Discount);
                sqlcmd.Parameters.AddWithValue("@cod", obj.isCod);
                sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                sqlcmd.Parameters.AddWithValue("@status", obj.status);
                sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", obj.isCreatedFromMMUs);
                sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                sqlcmd.Parameters.AddWithValue("@remarks", obj.Remarks);
                sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                sqlcmd.Parameters.AddWithValue("@gst", obj.gst);
                sqlcmd.Parameters.AddWithValue("@width", obj.width);
                sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                sqlcmd.Parameters.AddWithValue("@height", obj.height);
                sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", obj.expressionGreetingCard);
                sqlcmd.Parameters.AddWithValue("@expressionMessage", obj.expressionMessage);
                sqlcmd.Parameters.AddWithValue("@consigneePhoneNo", obj.ConsigneeCell);
                sqlcmd.Parameters.AddWithValue("@expressionconsignmentRefNumber", obj.expressionconsignmentRefNumber);
                sqlcmd.Parameters.AddWithValue("@otherCharges", obj.Othercharges);
                sqlcmd.Parameters.AddWithValue("@routeCode", obj.routeCode);
                sqlcmd.Parameters.AddWithValue("@docPouchNo", obj.docPouchNo);
                sqlcmd.Parameters.AddWithValue("@consignerPhoneNo", obj.ConsignerPhone);
                sqlcmd.Parameters.AddWithValue("@consignerCellNo", obj.ConsignerCell);
                sqlcmd.Parameters.AddWithValue("@consignerCNICNo", obj.ConsignerCNIC);
                sqlcmd.Parameters.AddWithValue("@consignerAccountNo", obj.consignerAccountNo);
                sqlcmd.Parameters.AddWithValue("@consignerEmail", obj.consignerEmail);
                sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", obj.docIsHomeDelivery);
                sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", "0");
                sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.Bookingdate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@receivedFromRider", obj.receivedFromRider);
                sqlcmd.Parameters.AddWithValue("@chargedAmount", obj.ChargeAmount);
                sqlcmd.Parameters.AddWithValue("@cnState", obj.cnState);
                sqlcmd.Parameters.AddWithValue("@cardRefNo", obj.cardRefNo);
                sqlcmd.Parameters.AddWithValue("@manifestNo", obj.manifestNo);
                sqlcmd.Parameters.AddWithValue("@manifestId", obj.manifestId);
                sqlcmd.Parameters.AddWithValue("@originBrName", obj.originBrName);
                sqlcmd.Parameters.AddWithValue("@orderRefNo", obj.orderRefNo);
                sqlcmd.Parameters.AddWithValue("@customerName", obj.customerName);
                sqlcmd.Parameters.AddWithValue("@productTypeId", obj.productTypeId);
                sqlcmd.Parameters.AddWithValue("@productDescription", obj.productDescription);
                sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                sqlcmd.Parameters.AddWithValue("@cnScreenId", obj.cnScreenId);
                sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);

                //SqlParameter P_XCode = new SqlParameter("@XCode", SqlDbType.VarChar, 256);
                //P_XCode.Direction = ParameterDirection.Output;

                //  sqlcmd.Parameters.Add(P_XCode);
                // sqlcmd.Parameters.Add(SParam);
                sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
        }

        public string Update_Consignment(Cl_Variables clvar)
        {
            clvar.Error = "";
            int IsUnique = 0;
            //    obj.Call_Log_ID = int.Parse(Get_maxValue()) + 1;
            try
            {
                string sql = "UPDATE Consignment \n"
               + "SET orgin = '" + clvar.origin + "', \n"
               + "    originExpressCenter = '" + clvar.expresscenter + "',  \n"
               + "    destinationExpressCenterCode ='" + clvar.destinationExpressCenterCode + "', \n"
               + "    destination = '" + clvar.Destination + "', \n"
               + "    [weight] = '" + clvar.Weight + "',  \n"
               + "    consignerAccountNo ='" + clvar.AccountNo + "',  \n"
               + "    riderCode = '" + clvar.RiderCode + "', \n"
               + "    serviceTypeName = '" + clvar.ServiceTypeName + "', \n"
               + "    consigner = '" + clvar.Consigner + "', \n"
               + "    consignee = '" + clvar.Consignee + "', \n"
               + "    consignerPhoneNo = '', \n"
               + "    consignerCellNo = '" + clvar.ConsignerCell + "',  \n"
               + "    consignerCNICNo = '" + clvar.ConsignerCNIC + "', \n"
               + "    consigneeCNICNo = '" + clvar.ConsigneeCNIC + "',  \n"
               + "    expressCenterCode = '" + clvar.expresscenter + "',pieces='" + clvar.pieces + "', \n"
               + "    branchCode = '" + clvar.origin + "',shipperAddress ='" + clvar.ConsignerAddress + "',[address] = '" + clvar.ConsigneeAddress + "',creditClientId = '" + clvar.CustomerClientID + "' \n"
               + "WHERE consignmentNumber ='" + clvar.consignmentNo + "'  \n"
               + "";


                SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                SqlTransaction trans;
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                trans = sqlcon.BeginTransaction();
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                try
                {
                    sqlcmd.CommandText = sql;
                    sqlcmd.ExecuteNonQuery();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    sqlcon.Close();
                }
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
        }

        public string WriteToDatabase_(Cl_Variables clvar)
        {
            string error = "";
            try
            {


                //using (SqlConnection connection =

                //        new SqlConnection(clvar.Strcon()))
                //{

                //    // make sure to enable triggers

                //    // more on triggers in next post

                //    SqlBulkCopy bulkCopy = new SqlBulkCopy(connection);

                //    // set the destination table name

                //    bulkCopy.DestinationTableName = "dbo.ConsignmentExpressionDetail";

                //    connection.Open();

                //    // write the data in the “dataTable”

                //    bulkCopy.WriteToServer(clvar.expresssion);

                //    connection.Close();

                //}

                List<string> queries = new List<string>();
                foreach (DataRow row in clvar.expresssion.Rows)
                {
                    if (row["status"].ToString() == "2")
                    {
                        error = "insert into ConsignmentExpressionDetail (ConsignementNo, itemID, itemqty, amount, message, Status, CreatedOn, CreatedBy, gst, serviceCharges) \n" +
                                " VALUES (\n" +
                                "'" + row["consignementNo"].ToString() + "',\n" +
                                "'" + row["itemId"].ToString() + "',\n" +
                                "'" + row["itemQty"].ToString() + "',\n" +
                                "'" + row["amount"].ToString() + "',\n" +
                                "'" + row["message"].ToString() + "',\n" +
                                "'" + DBNull.Value + "',\n" +
                                "GETDATE(),\n" +
                                "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                "'" + row["gst"].ToString() + "',\n" +
                                "'" + row["serviceCharges"].ToString() + "'\n" +

                                ")";
                        queries.Add(error);
                    }
                    else if (row["status"].ToString() == "0")
                    {
                        error = "delete from ConsignmentExpressionDetail where id = '" + row["id"].ToString() + "'";
                        queries.Add(error);
                    }
                }

                SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                SqlTransaction trans;
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                trans = sqlcon.BeginTransaction();
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                error = "";
                try
                {
                    foreach (string sql in queries)
                    {
                        sqlcmd.CommandText = sql;
                        sqlcmd.ExecuteNonQuery();
                    }


                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    sqlcon.Close();
                }


            }
            catch (Exception er)
            {

                error = er.Message;
            }
            // reset

            return error;

        }

        public string Add_ConsignmentModifier(DataTable dt, Cl_Variables clvar)
        {
            clvar.Error = "";
            if (dt.Rows.Count > 0)
            {

                using (SqlConnection con = new SqlConnection(clvar.Strcon()))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        try
                        {


                            //Set the database table name
                            sqlBulkCopy.DestinationTableName = "dbo.ConsignmentModifier";

                            //[OPTIONAL]: Map the DataTable columns with that of the database table
                            //sqlBulkCopy.ColumnMappings.Add("PriceModifierID", "");
                            //sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                            //sqlBulkCopy.ColumnMappings.Add("Country", "Country");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            clvar.Error = ex.Message;
                        }
                    }
                }
            }
            return clvar.Error;
        }

        public int Add_InternationalConsignmentDetail(Cl_Variables clvar)
        {
            int i = 0;
            string query = "INSERT INTO InternationalConsignmentDetail (CONSIGNMENTNO, SERVICETYPE) VALUES ('" + clvar.consignmentNo + "', '" + clvar.ServiceTypeName + "')";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                i = sqlcmd.ExecuteNonQuery();

            }
            catch (Exception x)
            { }
            finally { sqlcon.Close(); }
            return i;
        }
        public string AddConsignmentSender(Cl_Variables clvar)
        {
            int count = 0;
            string error = "";
            string query = "Insert into MNP_INTERNATIONALCONSIGNMENT_SENDER (ConsignmentNumber, CompanyID) Values \n" +
                            "(\n" +
                            "   '" + clvar.consignmentNo + "',\n" +
                            "   '" + clvar.Company + "'\n" +
                            ")";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();

                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return error;
        }

        public string DeleteConsignment(Cl_Variables clvar)
        {
            string error = "";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("SP_DeleteConsignment", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@ConsignmentNumber", clvar.consignmentNo);
                sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return error;
        }

        public int AddServiceType(Cl_Variables clvar)
        {
            string error = "";
            int count = 0;
            string query = "insert into servicetypes_new (servicetypename, name, status, description, createdby, createdon, \n" +
                            "companyid, isintl, serviceTypeCategory, sname, isFranchised, commissionid, products)" +
                            " VALUES ('" + clvar.ServiceTypeName + "','" + clvar.ServiceTypeName + "', '" + clvar.status + "',\n" +
                            "         '" + clvar.productDescription + "', '" + HttpContext.Current.Session["NAME"].ToString() + "',\n" +
                            "         GETDATE(), '" + clvar.Company + "', '" + clvar.IsInternational + "', '" + clvar.ServiceTypeCategory + "','', \n" +
                            "         '" + clvar.IsFranchised + "', '','')";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

        public int UpdateServiceType(Cl_Variables clvar)
        {
            string error = "";
            int count = 0;
            string query = "UPDATE servicetypes_new SET servicetypename = '" + clvar.ServiceTypeName + "',\n" +
                            " name = '" + clvar.ServiceTypeName + "', \n" +
                            " status = '" + clvar.status + "',\n" +
                            " description = '" + clvar.productDescription + "',\n" +
                            " Modifiedby ='" + HttpContext.Current.Session["NAME"].ToString() + "',\n" +
                            " modifiedon = GETDATE(), \n" +
                            " companyid = '" + clvar.Company + "', \n" +
                            " isintl = '" + clvar.IsInternational + "',\n" +
                            " serviceTypeCategory = '" + clvar.ServiceTypeCategory + "', \n" +
                            " isFranchised = '" + clvar.IsFranchised + "', \n" +
                            " commissionid = '" + clvar.CommissionID + "'\n" +
                            " Where id = '" + clvar.serviceTypeId + "'";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

        public int AddCurrencies(Cl_Variables clvar)
        {
            string error = "";
            int count = 0;
            string query = "INSERT INTO CURRENCIES (NAME, SYMBOL, CODE, COUNTRYID, STATUS, CREATEDBY, CREATEDON)\n" +
                            " VALUES\n" +
                            "(      \n" +
                            "'" + clvar.CurrencyName + "',\n" +
                            "'" + clvar.CurrencySymbol + "',\n" +
                            "'" + clvar.CurrencyCode + "',\n" +
                            "'" + clvar.destinationCountryCode + "',\n" +
                            "'" + clvar.status + "',\n" +
                            "'" + HttpContext.Current.Session["NAME"].ToString() + "',\n" +
                            "GETDATE()" +
                            "       )";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

        public int UpdateCurrency(Cl_Variables clvar)
        {
            string error = "";
            int count = 0;
            string query = "UPDATE CURRENCIES SET\n" +
                           " NAME = '" + clvar.CurrencyName + "',\n" +
                           " SYMBOL = '" + clvar.CurrencySymbol + "',\n" +
                           " CODE = '" + clvar.CurrencyCode + "',\n" +
                           " COUNTRYID = '" + clvar.destinationCountryCode + "',\n" +
                           " STATUS = '" + clvar.status + "'\n" +
                           " WHERE ID = '" + clvar.CurrencyID + "'";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

        public int InsertTariff(Cl_Variables clvar, List<DataRow> dr)
        {

            clvar.FromZoneCode = GetZoneOfBranch(clvar).Rows[0]["ZoneCode"].ToString();
            string error = "";
            int count = 0;
            string query = "INSERT INTO tempClientTariff (tariffCode, Client_id, serviceID, BranchCode, FromZoneCode, ToZoneCode, FromWeight, ToWeight, Price, AdditionalFactor, AddtionalFactorSZ, AddtionalFactorDZ, chkDefaultTariff, chkDeleted, isIntlTariff,  CreatedOn)\n";

            for (int i = 0; i < dr.Count - 1; i++)
            {
                query += "SELECT '0', " + clvar.CustomerClientID + ",'" + clvar.ServiceTypeName + "', '" + clvar.Branch + "', '" + clvar.FromZoneCode + "', '" + dr[i]["DestinationID"].ToString() + "', '" + dr[i]["FromWeight"].ToString() + "', '" + dr[i]["ToWeight"].ToString() + "', \n" +
                        "'" + dr[i]["Price"].ToString() + "', '" + dr[i]["AddFactor"].ToString() + "', '0', '0', '0', '0', '0', GETDATE()\n" +
                        "UNION ALL \n";
            }
            query += "SELECT '0', " + clvar.CustomerClientID + ",'" + clvar.ServiceTypeName + "', '" + clvar.Branch + "', '" + clvar.FromZoneCode + "', '" + dr[dr.Count - 1]["DestinationID"].ToString() + "', '" + dr[dr.Count - 1]["FromWeight"].ToString() + "', '" + dr[dr.Count - 1]["ToWeight"].ToString() + "', \n" +
                        "'" + dr[dr.Count - 1]["Price"].ToString() + "', '" + dr[dr.Count - 1]["AddFactor"].ToString() + "', '0', '0', '0', '0', '0', GETDATE()\n" +
                        "";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;

        }

        public int DeleteTariff(Cl_Variables clvar)
        {
            int count = 0;
            string error = "";
            string query = "UPDATE TEMPCLIENTTARIFF SET chkDELETED = '1' where id = '" + clvar.TariffID + "'";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

        public int UpdateTarrif(Cl_Variables clvar, List<DataRow> dr)
        {

            string query = "CREATE TABLE [dbo].[tempTariff](\n" +
                "\t[Id] [bigint] NOT NULL,\n" +
            "\t[Client_Id] [bigint] NOT NULL,\n" +
            "  [TariffCode] [varchar](50) NOT NULL,\n" +
            "  [ServiceID] [varchar](50) NULL,\n" +
            "  [BranchCode] [varchar](50) NULL,\n" +
            "  [FromZoneCode] [varchar](50) NULL,\n" +
            "  [ToZoneCode] [varchar](50) NULL,\n" +
            "  [FromWeight] [float] NULL,\n" +
            "  [ToWeight] [float] NULL,\n" +
            "  [Price] [float] NULL,\n" +
            "  [additionalFactor] [float] NULL,\n" +
            "  [addtionalFactorSZ] [float] NULL,\n" +
            "  [addtionalFactorDZ] [float] NULL,\n" +
            "  [chkDefaultTariff] [bit] NULL,\n" +
            "  [chkDeleted] [bit] NULL,\n" +
            "  [isIntlTariff] [bit] NULL,\n" +
            "  [createdOn] [datetime] NULL,\n" +
            "\n" +
            ")";

            string query1 = "INSERT INTO tempTariff (ID, tariffCode, Client_id, serviceID, BranchCode, FromZoneCode, ToZoneCode, FromWeight, ToWeight, Price, AdditionalFactor, AddtionalFactorSZ, AddtionalFactorDZ, chkDefaultTariff, chkDeleted, isIntlTariff,  CreatedOn)\n";

            for (int i = 0; i < dr.Count - 1; i++)
            {
                query1 += "SELECT '" + dr[i]["ID"].ToString() + "', '0', " + clvar.CustomerClientID + ",'" + clvar.ServiceTypeName + "', '" + clvar.Branch + "', '" + clvar.FromZoneCode + "', '" + dr[i]["DestinationID"].ToString() + "', '" + dr[i]["FromWeight"].ToString() + "', '" + dr[i]["ToWeight"].ToString() + "', \n" +
                        "'" + dr[i]["Price"].ToString() + "', '" + dr[i]["AddFactor"].ToString() + "', '0', '0', '0', '0', '0', GETDATE()\n" +
                        "UNION ALL \n";
            }
            query1 += "SELECT '" + dr[dr.Count - 1]["ID"].ToString() + "', '0', " + clvar.CustomerClientID + ",'" + clvar.ServiceTypeName + "', '" + clvar.Branch + "', '" + clvar.FromZoneCode + "', '" + dr[dr.Count - 1]["DestinationID"].ToString() + "', '" + dr[dr.Count - 1]["FromWeight"].ToString() + "', '" + dr[dr.Count - 1]["ToWeight"].ToString() + "', \n" +
                        "'" + dr[dr.Count - 1]["Price"].ToString() + "', '" + dr[dr.Count - 1]["AddFactor"].ToString() + "', '0', '0', '0', '0', '0', GETDATE()\n" +
                        "";


            string query2 = "update tempClientTariff\n" +
            "set Client_Id = t.Client_Id,\n" +
            "\tTariffCode = t.TariffCode,\n" +
            "\tServiceID = t.ServiceID,\n" +
            "\tBranchCode = t.BranchCode,\n" +
            "\tFromZoneCode = t.fromZoneCode,\n" +
            "\tToZoneCode = t.ToZoneCode,\n" +
            "\tFromWeight = t.FromWeight,\n" +
            "\tToWeight = t.ToWeight,\n" +
            "\tPrice = t.Price,\n" +
            "\tadditionalFactor = t.additionalFactor,\n" +
            "\taddtionalFactorDZ = t.addtionalFactorDZ,\n" +
            "\taddtionalFactorSZ = t.addtionalFactorSZ,\n" +
            "\tchkDefaultTariff = t.chkDefaultTariff,\n" +
            "\tchkDeleted = t.chkDeleted,\n" +
            "\tisIntlTariff = t.isIntlTariff,\n" +
            "\tcreatedOn = t.createdOn\n" +
            "from tempTariff t where tempClientTariff.id = t.id";
            int count = 0;
            string query3 = "DROP TABLE tempTariff";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;
                count = sqlcmd.ExecuteNonQuery();
                sqlcmd = new SqlCommand(query1, sqlcon);
                sqlcmd.ExecuteNonQuery();
                sqlcmd = new SqlCommand(query2, sqlcon);
                count = sqlcmd.ExecuteNonQuery();
                sqlcmd = new SqlCommand(query3, sqlcon);
                sqlcmd.ExecuteNonQuery();

                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                //error = ex.Message;
            }
            return count;

        }



        public DataTable GetConsignmentDetail(Cl_Variables clvar)
        {


            string sqlString = "select c.consignmentNumber ConNo,\n" +
            "     c.consignerAccountNo AccountNo,\n" +
            "     c.riderCode,\n" +
            "     ct.name ConType,\n" +
            "     cc.cityName City,\n" +
            "     b2.name Branch,\n" +
            "     c.weight,\n" +
            "     c.serviceTypeName,\n" +
            "     c.discount,\n" +
            "     c.pieces,\n" +
            "     c.consignee,\n" +
            "     c.consigneePhoneNo ConsigneeCell,\n" +
            "     c.consigneeCNICNo ConsigneeCNIC,\n" +
            "     c.consigner,\n" +
            "     c.consignerCellNo ConsignerCell,\n" +
            "     c.consignerCNICNo ConsignerCNIC,\n" +
            "     c.couponNumber Coupon,\n" +
            "     c.decalaredValue DeclaredValue,\n" +
            "     c.PakageContents PackageContents,\n" +
            "     c.address Address,\n" +
            "     c.shipperAddress,\n" +
            "     c.bookingDate,\n" +
            "     b.name ORIGIN,\n" +
            "     c.originExpressCenter,\n" +
            "     ec.name,\n" +
            "     c.insuarancePercentage,\n" +
            "     c.totalAmount,\n" +
            "     c.chargedAmount,\n" +
            "     c.isInsured,\n" +
            "     c.dayType,c.gst, ccc.CODTYPE\n" +
            "     from Consignment c\n" +
            "     inner join Zones z\n" +
            "     on z.zoneCode = c.zoneCode\n" +
            "     inner join Branches b\n" +
            "     on b.branchCode = c.Orgin\n" +
            "     inner join Branches b2\n" +
            "\t   on b2.branchCode = c.destination\n" +
            "\t   inner join ConsignmentType ct\n" +
            "\t   on ct.id = c.consignmentTypeId\n" +
            "\t   inner join Cities cc\n" +
            "\t   on cc.id = b2.cityId\n" +
            "\t   left outer join ExpressCenters ec\n" +
            "\t   on ec.expressCenterCode = c.originExpressCenter\n" +
            "\t   inner join CreditClients ccc\n" +
            "\t    on ccc.id = c.creditClientId\n" +
            "\t    where c.consignmentNumber = '" + clvar.consignmentNo + "' --and c.createdBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        public DataTable GetConsignmentDetail_(Cl_Variables clvar)
        {
            string sqlString = "select c.*, c.consignmentNumber ConNo,\n" +
            "     c.consignerAccountNo AccountNo,\n" +
            "     c.riderCode,\n" +
            "     ct.name ConType,\n" +
            "     cc.cityName City,\n" +
            "     b2.name Branch,\n" +
            "     c.weight,\n" +
            "     c.serviceTypeName,\n" +
            "     c.discount,\n" +
            "     c.pieces,\n" +
            "     c.consignee,\n" +
            "     c.consigneePhoneNo ConsigneeCell,\n" +
            "     c.consigneeCNICNo ConsigneeCNIC,\n" +
            "     c.consigner,\n" +
            "     c.consignerCellNo ConsignerCell,\n" +
            "     c.consignerCNICNo ConsignerCNIC,\n" +
            "     c.couponNumber Coupon,\n" +
            "     c.decalaredValue DeclaredValue,\n" +
            "     c.PakageContents PackageContents,\n" +
            "     c.address Address,\n" +
            "     c.shipperAddress,\n" +
            "     c.bookingDate,\n" +
            "     b.name ORIGIN,\n" +
            "     c.originExpressCenter,\n" +
            "     ec.name,\n" +
            "     c.insuarancePercentage,\n" +
            "     c.totalAmount,\n" +
            "     c.chargedAmount,\n" +
            "     c.isInsured,\n" +
            "     c.dayType,c.gst, ccc.CODTYPE\n" +
            "     from Consignment c\n" +
            "     inner join Zones z\n" +
            "     on z.zoneCode = c.zoneCode\n" +
            "     inner join Branches b\n" +
            "     on b.branchCode = c.branchCode\n" +
            "     inner join Branches b2\n" +
            "\t   on b2.branchCode = c.destination\n" +
            "\t   inner join ConsignmentType ct\n" +
            "\t   on ct.id = c.consignmentTypeId\n" +
            "\t   inner join Cities cc\n" +
            "\t   on cc.id = b2.cityId\n" +
            "\t   left outer join ExpressCenters ec\n" +
            "\t   on ec.expressCenterCode = c.originExpressCenter\n" +
            "\t   inner join CreditClients ccc\n" +
            "\t    on ccc.id = c.creditClientId\n" +
            "\t    where c.consignmentNumber = '" + clvar.consignmentNo + "'";



            sqlString = "select c.*, c.consignmentNumber ConNo,\n" +
           "     c.consignerAccountNo AccountNo,\n" +
           "     c.riderCode,\n" +
           "     ct.name ConType,\n" +
           "\n" +
           "     b2.name Branch,\n" +
           "     c.weight,\n" +
           "     c.serviceTypeName,\n" +
           "     c.discount,\n" +
           "     c.pieces,\n" +
           "     c.consignee,\n" +
           "     c.consigneePhoneNo ConsigneeCell,\n" +
           "     c.consigneeCNICNo ConsigneeCNIC,\n" +
           "     c.consigner,\n" +
           "     c.consignerCellNo ConsignerCell,\n" +
           "     c.consignerCNICNo ConsignerCNIC,\n" +
           "     c.couponNumber Coupon,\n" +
           "     c.decalaredValue DeclaredValue,\n" +
           "     c.PakageContents PackageContents,\n" +
           "     c.address Address,\n" +
           "     c.shipperAddress,\n" +
           "     c.bookingDate,\n" +
           "     b.name ORIGIN,\n" +
           "     c.originExpressCenter,\n" +
           "\n" +
           "     c.insuarancePercentage,\n" +
           "     c.totalAmount,\n" +
           "     c.chargedAmount,\n" +
           "     c.isInsured,\n" +
           "     c.dayType,c.gst\n" +
           "     from Consignment c\n" +
           "     --inner join Zones z\n" +
           "     --on z.zoneCode = c.zoneCode\n" +
           "     inner join Branches b\n" +
           "     on b.branchCode = c.orgin\n" +
           "     inner join Branches b2\n" +
           "     on b2.branchCode = c.destination\n" +
           "     left outer join ConsignmentType ct\n" +
           "     on ct.id = c.consignmentTypeId\n" +
           "\n" +
           "\n" +
           "      where c.consignmentNumber = '" + clvar.consignmentNo + "'";




            sqlString = "select ISNULL(mco.isRunsheetAllowed, 1) RunsheetAllowed,\n" +
           "       c.*,\n" +
           "       c.consignmentNumber ConNo,\n" +
           "       c.consignerAccountNo AccountNo,\n" +
           "       c.riderCode,\n" +
           "       ct.name ConType,\n" +
           "\n" +
           "       b2.name               Branch,\n" +
           "       c.weight,\n" +
           "       c.serviceTypeName,\n" +
           "       c.discount,\n" +
           "       c.pieces,\n" +
           "       c.consignee,\n" +
           "       c.consigneePhoneNo    ConsigneeCell,\n" +
           "       c.consigneeCNICNo     ConsigneeCNIC,\n" +
           "       c.consigner,\n" +
           "       c.consignerCellNo     ConsignerCell,\n" +
           "       c.consignerCNICNo     ConsignerCNIC,\n" +
           "       c.couponNumber        Coupon,\n" +
           "       c.decalaredValue      DeclaredValue,\n" +
           "       c.PakageContents      PackageContents,\n" +
           "       c.address             Address,\n" +
           "       c.shipperAddress,\n" +
           "       c.bookingDate,\n" +
           "       b.name                ORIGIN,\n" +
           "       c.originExpressCenter,\n" +
           "\n" +
           "       c.insuarancePercentage,\n" +
           "       c.totalAmount,\n" +
           "       c.chargedAmount,\n" +
           "       c.isInsured,\n" +
           "       c.dayType,\n" +
           "       c.gst\n" +
           "  from Consignment c\n" +
           "--inner join Zones z\n" +
           "--on z.zoneCode = c.zoneCode\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = c.orgin\n" +
           " inner join Branches b2\n" +
           "    on b2.branchCode = c.destination\n" +
           "  left outer join ConsignmentType ct\n" +
           "    on ct.id = c.consignmentTypeId\n" +
           "  left outer join mnp_consignmentOperations mco\n" +
           "    on mco.consignmentId = c.consignmentNumber\n" +
           "\n" +
           " where c.consignmentNumber = '" + clvar.consignmentNo + "'";


            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        public DataTable GetCodDetailByConsignmentNumber(Cl_Variables clvar)
        {
            string query = "SELECT * FROM CODConsignmentDetail cd where cd.consignmentNumber = '" + clvar.consignmentNo + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetConsignmentModifier(Cl_Variables clvar)
        {

            string sqlString = "select p.name,\n" +
            "         p.description,\n" +
            "         c.calculatedValue,\n" +
            "         c.calculationBase,\n" +
            "         c.modifiedCalculationValue\n" +
            "    from ConsignmentModifier c\n" +
            "   inner join PriceModifiers p\n" +
            "      on p.id = c.priceModifierId\n" +
            "   where c.consignmentNumber = '" + clvar.consignmentNo + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetConsingerByCellNo(Cl_Variables clvar)
        {


            string sqlString = "select cc.ConsignerName,\n" +
            "       cc.ConsignerAddress,\n" +
            "       cc.ConsignerCellNo,\n" +
            "       cc.ClientId,\n" +
            "       cc.ClientAccountNo,\n" +
            "       cc.ConsignerCNIC\n" +
            "  FROM ConsignmentClient cc\n" +
            " where cc.ConsignmentId =\n" +
            "       (select MAX(ccc.consignmentid)\n" +
            "          from ConsignmentClient ccc\n" +
            "         where ccc.ConsignerCellNo = '" + clvar.ConsignerCell + "'\n" +
            "           and ccc.ConsignerCellNo is not null\n" +
            "           and ccc.ConsignerAddress is not null\n" +
            "           and ccc.ConsignerName is not null\n" +
            "           and ccc.ConsignerName <> '0')";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetConsigneeByCellNo(Cl_Variables clvar)
        {
            string query = "select * from ConsignmentClientCellInfo cc where cc.CellNoID = '" + clvar.ConsigneeCell.TrimStart('0') + "' ";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public string PerformDayEnd()
        {
            int count = 0;

            #region MyRegion
            //string query = "Insert Into MNP_DAYEND (DATE, CONSIGNMENTNUMBER, CON_CREATEDBY, CON_CREATEDON, DAYEND_CREATEDBY, DAYEND_CREATEDON)  \n";
            //query += "select (SELECT workingdate FROM CALENDAR c where year = YEAR('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString())).ToString("yyyy-MM-dd") + "') and MONTH = MONTH('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString())).ToString("yyyy-MM-dd") + "') and DAYdate = DAY('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString())).ToString("yyyy-MM-dd") + "')), \n" +
            //    "c.consignmentNumber, c.createdby, c.createdon, '" + HttpContext.Current.Session["U_ID"].ToString() + "', GetDATE()\n" +
            //"\n" +
            //"  FROM Consignment c\n" +
            //"  --inner join Branches b\n" +
            //"  --on b.branchCode = c.orgin\n" +
            //" where CONVERT(date, c.bookingDate, 105) = '" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).ToString("yyyy-MM-dd") + "'\n" +
            //"   and c.expresscentercode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "' and c.createdBy is not null and c.createdon is not null\n" +
            //" --order by contype, serviceTypeName";

            //string query1 = "UPDATE CONSIGNMENT SET CUTOFFTIME = GETDATE() where consignmentNumber in ( SELECT \n" +
            //    "c.consignmentNumber \n" +
            //"\n" +
            //"  FROM Consignment c\n" +
            //"  --inner join Branches b\n" +
            //"  --on b.branchCode = c.orgin\n" +
            //" where CONVERT(date, c.bookingDate, 105) = '" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).ToString("yyyy-MM-dd") + "'\n" +
            //"   and c.expresscentercode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "' and c.createdBy is not null and c.createdon is not null)\n" +
            //" --order by contype, serviceTypeName";

            //string query2 = "UPDATE EXPRESSCENTERs set workingDate = (SELECT WorkingDate FROM CALENDAR c where year = YEAR('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "') and MONTH = MONTH('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "')" +
            //"and DAYdate = case (select DAYOFF from ExpressCenters ec where ec.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"] + "') when (SELECT day from Calendar where workingdate = '" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "')\n" +
            //" then  DAY('" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(2).ToString("yyyy-MM-dd") + "')\n" +
            //" else  DAY('" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "')\n" +
            //" end) where expresscentercode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'";

            //string query3 = "SELECT WorkingDate FROM CALENDAR c where year = YEAR('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "') and MONTH = MONTH('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "')" +
            //"and DAYdate = case (select DAYOFF from ExpressCenters ec where ec.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"] + "') when (SELECT day from Calendar where workingdate = '" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "')\n" +
            //" then  DAY('" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(2).ToString("yyyy-MM-dd") + "')\n" +
            //" else  DAY('" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "')\n" +
            //" end"; 
            #endregion




            string sqlString = "Insert into MNP_DAYEND(Date, ZoneCode, BranchCode, ExpressCenter, ConsignmentNumber, Con_createdOn, Con_CreatedBy, DayEnd_CreatedBy, DayEnd_CreatedOn) select ec.WorkingDate,\n" +
            "       '" + HttpContext.Current.Session["ZoneCode"].ToString() + "',\n" +
            "       '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
            "       c.originExpressCenter,\n" +
            "       c.consignmentNumber,\n" +
            "       c.createdOn,\n" +
            "       c.createdBy,\n" +
            "       '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
            "       GETDATE()\n" +
            "  from Consignment c\n" +
            " Inner join ExpressCenters ec\n" +
            "    on c.originExpressCenter = ec.expressCenterCode\n" +
            "   and CONVERT(date, c.createdOn, 105) = ec.WorkingDate\n" +
            " inner join ZNI_USER1 z\n" +
            "    on CAST(z.U_ID as varchar) = cast(c.createdBy as varchar)\n" +
            " where c.originExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
            "   and CONVERT(date, c.createdOn, 105) = '" + HttpContext.Current.Session["WorkingDate"].ToString() + "'\n" +
            "   and z.U_TYPE = '1'\n" +
            "";



            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            sqlcmd.CommandTimeout = 300;
            try
            {
                string insertedDateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                sqlString = "Insert into MNP_DAYEND(Date, ZoneCode, BranchCode, ExpressCenter,DayEnd_CreatedBy, DayEnd_CreatedOn) output inserted.DayEnd_CreatedOn \n" +
                "Values('" + HttpContext.Current.Session["WorkingDate"].ToString() + "',\n" +
                "       '" + HttpContext.Current.Session["ZoneCode"].ToString() + "',\n" +
                "       '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                "       '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'," +
                "       '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                "       '" + insertedDateTime + "'" +
                ")";



                string sqlString1 = "update expressCenters Set workingdate = ( select cc.workingdate\n" +
                "  from Calendar cc\n" +
                " where cc.ID in (select case\n" +
                "                          when (select DAY from Calendar where workingdate = DATEADD(D,1,cast(ec.WorkingDate as date)) and COMPANYID = '01') <> ec.DayOff then\n" +
                "                           (c.ID + 1)\n" +
                "                          else\n" +
                "                           (c.ID + 2)\n" +
                "                        end ID\n" +
                "                   from Calendar c\n" +
                "                  inner join ExpressCenters ec\n" +
                "                     on ec.WorkingDate = c.workingdate\n" +
                "                  where c.COMPANYID = '01'\n" +
                "                    and ec.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "')) OUTPUT INSERTED.WorkingDate \n" +
                " where expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'";

                DateTime insertedTimes = new DateTime();
                sqlcmd.CommandText = sqlString;
                //count = sqlcmd.ExecuteNonQuery();
                string time = "";
                time = sqlcmd.ExecuteScalar().ToString();

                string sqlString2 = "update consignment set cutoffTime = '" + insertedDateTime /*DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)*/ + "' where consignmentNumber in (select \n" +
                   "       c.consignmentNumber\n" +
                   "  from Consignment c\n" +
                   " Inner join ExpressCenters ec\n" +
                   "    on c.originExpressCenter = ec.expressCenterCode\n" +
                   "   and CONVERT(date, c.createdOn, 105) = ec.WorkingDate\n" +
                   " inner join ZNI_USER1 z\n" +
                   "    on CAST(z.U_ID as varchar) = cast(c.createdBy as varchar)\n" +
                   " where c.originExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
                   "   and CONVERT(date, c.createdOn, 105) = '" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).ToString("yyyy-MM-dd") + "'\n" +
                   "   and z.U_TYPE = '1')\n";


                string sqlString3 = "select \n" +
                   "       c.consignmentNumber\n" +
                   "  from Consignment c\n" +
                   " Inner join ExpressCenters ec\n" +
                   "    on c.originExpressCenter = ec.expressCenterCode\n" +
                   "   and CONVERT(date, c.createdOn, 105) = ec.WorkingDate\n" +
                   " inner join ZNI_USER1 z\n" +
                   "    on CAST(z.U_ID as varchar) = cast(c.createdBy as varchar)\n" +
                   " where c.originExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
                   "   and CONVERT(date, c.createdOn, 105) = '" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).ToString("yyyy-MM-dd") + "'\n" +
                   "   and z.U_TYPE = '1'\n";


                sqlcmd.CommandText = sqlString2;
                sqlcmd.CommandTimeout = 600;
                count = sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = sqlString1;
                object obj = sqlcmd.ExecuteScalar();

                DateTime newDate;

                DateTime.TryParse(obj.ToString(), out newDate);
                HttpContext.Current.Session["WorkingDate"] = newDate.ToString("dd MMM yyyy");


                trans.Commit();


            }
            catch (Exception ex)
            {
                trans.Rollback();
                count = 0;
                sqlcon.Close();
                return ex.Message;
                ;
            }
            finally { sqlcon.Close(); }
            return "OK";
        }
        public DataTable CheckWorkingDate()
        {

            DataTable dt = new DataTable();
            string query = "select MAX(d.Date) from MNP_DAYEND d where d.ExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            return dt;

        }
        #region LAST DAY END METHOD
        //public int PerformDayEnd()
        //{
        //    int count = 0;

        //    string query = "Insert Into MNP_DAYEND (DATE, CONSIGNMENTNUMBER, CON_CREATEDBY, CON_CREATEDON, DAYEND_CREATEDBY, DAYEND_CREATEDON)  \n";
        //    query += "select (SELECT workingdate FROM CALENDAR c where year = YEAR('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString())).ToString("yyyy-MM-dd") + "') and MONTH = MONTH('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString())).ToString("yyyy-MM-dd") + "') and DAYdate = DAY('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString())).ToString("yyyy-MM-dd") + "')), \n" +
        //        "c.consignmentNumber, c.createdby, c.createdon, '" + HttpContext.Current.Session["U_ID"].ToString() + "', GetDATE()\n" +
        //    "\n" +
        //    "  FROM Consignment c\n" +
        //    "  --inner join Branches b\n" +
        //    "  --on b.branchCode = c.orgin\n" +
        //    " where c.bookingDate = '" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).ToString("yyyy-MM-dd") + "'\n" +
        //    "   and c.expresscentercode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
        //    " --order by contype, serviceTypeName";

        //    string query1 = "UPDATE CONSIGNMENT SET CUTOFFTIME = GETDATE() where consignmentNumber in ( SELECT \n" +
        //        "c.consignmentNumber \n" +
        //    "\n" +
        //    "  FROM Consignment c\n" +
        //    "  --inner join Branches b\n" +
        //    "  --on b.branchCode = c.orgin\n" +
        //    " where c.bookingDate = '" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).ToString("yyyy-MM-dd") + "'\n" +
        //    "   and c.expresscentercode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "')\n" +
        //    " --order by contype, serviceTypeName";

        //    string query2 = "UPDATE EXPRESSCENTERs set workingDate = (SELECT WorkingDate FROM CALENDAR c where year = YEAR('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "') and MONTH = MONTH('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "')" +
        //    "and DAYdate = case (select DAYOFF from ExpressCenters ec where ec.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"] + "') when (SELECT day from Calendar where workingdate = '" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "')\n" +
        //    " then  DAY('" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(2).ToString("yyyy-MM-dd") + "')\n" +
        //    " else  DAY('" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "')\n" +
        //    " end) where expresscentercode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'";

        //    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
        //    SqlTransaction trans;
        //    sqlcon.Open();
        //    SqlCommand sqlcmd = new SqlCommand();
        //    sqlcmd.Connection = sqlcon;
        //    trans = sqlcon.BeginTransaction();
        //    sqlcmd.Transaction = trans;
        //    sqlcmd.CommandType = CommandType.Text;
        //    sqlcmd.CommandTimeout = 300;
        //    try
        //    {
        //        sqlcmd.CommandText = query;
        //        count = sqlcmd.ExecuteNonQuery();

        //        sqlcmd.CommandText = query1;
        //        count = sqlcmd.ExecuteNonQuery();

        //        sqlcmd.CommandText = query2;
        //        count = sqlcmd.ExecuteNonQuery();

        //        trans.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        count = 0;
        //    }
        //    return count;
        //} 
        #endregion
        public DataTable GetNewDayForDayEnd()
        {
            //string query = "SELECT * FROM CALENDAR c where year = YEAR('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "') and MONTH = MONTH('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "')"+ 
            //"and DAYdate = DAY('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "')";
            string query = "SELECT * FROM CALENDAR c where year = YEAR('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "') and MONTH = MONTH('" + (DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "')" +
            "and DAYdate = case (select DAYOFF from ExpressCenters ec where ec.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"] + "') when (SELECT day from Calendar where workingdate = '" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "')\n" +
            " then  DAY('" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(2).ToString("yyyy-MM-dd") + "')\n" +
            " else  DAY('" + DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "')\n" +
            " end";


            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        public DataTable GetExpressCenterName(Cl_Variables clvar)
        {
            string query = "select c.NAME FROM ExpressCenters c where c.expressCenterCode = '" + clvar.expresscenter + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        public DataTable GetDayCloseReport(Cl_Variables clvar)
        {
            string sqlString = "select c.consignmentNumber [CN No],\n" +
            "\t   c.consigner Consigner,\n" +
            "\t   c.consignee,\n" +
            "\t   c.serviceTypeName,\n" +
            "\t   b.name Origin,\n" +
            "\t   c.destination Destination,\n" +
            "\t   c.consignerAccountNo AccNo,\n" +
            "\t   c.weight,\n" +
            "\t   c.gst TAX,\n" +
            "\t   c.chargedAmount,\n" +
            "     case\n" +
            "\t\t\twhen c.isInsured = '1'\n" +
            "\t\t\tthen (c.decalaredValue * (c.insuarancePercentage/100)) + c.totalAmount\n" +
            "\t\t\telse c.totalAmount\n" +
            "\t\tend TotalAmount," +
            "\t   case\n" +
            "\t\t\twhen c.orgin = c.destination\n" +
            "\t\t\tthen 'Local'\n" +
            "\t\t\telse 'Domestic'\n" +
            "\t   end ConType\n" +
            "\n" +
            "  FROM Consignment c\n" +
            "  inner join Branches b\n" +
            "  on b.branchCode = c.orgin\n" +
            " where c.bookingDate = '" + clvar.Day + "'\n" +
            "   and c.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
            " order by contype, serviceTypeName";



            sqlString = "select c.consignmentNumber [CN No], c.customerType,\n" +
           "\t   c.consigner Consigner,\n" +
           "\t   c.consignee,\n" +
           "\t   c.serviceTypeName,\n" +
           "\t   b.name Origin,\n" +
           "\t   (               \n" +
           "\t    SELECT b2.name    \n" +
           "\t    FROM   Branches b2    \n" +
           "\t   WHERE  b2.branchCode = c.destination    \n" +
           "\t )   DestinationBranch,   \n" +
           "\t  (        \n" +
           "\t      SELECT ec.name   \n" +
           "\t      FROM   ExpressCenters ec   \n" +
           "\t     WHERE  ec.expressCenterCode = c.destinationExpressCenterCode    \n" +
           "\t  )    Destination,     \n" +
           "\t   c.consignerAccountNo AccNo,\n" +
           "\t   c.weight,\n" +
           "\t   c.gst TAX,\n" +
           "\t   c.chargedAmount,\n" +
           "     case\n" +
           "\t\t\twhen c.isInsured = '1'\n" +
           "\t\t\tthen (c.decalaredValue * (c.insuarancePercentage/100)) + c.totalAmount\n" +
           "\t\t\telse c.totalAmount\n" +
           "\t\tend TotalAmount," +
           "\t   case when (select COUNT(vc.ConsignmentNo) from MNP_VOID_Consignment vc where vc.ConsignmentNo = c.consignmentNumber) > 0 then 'VOID'\n" +
           "\t\t\twhen c.orgin = c.destination\n" +
           "\t\t\tthen 'Local'\n" +
           "\t\t\telse 'Domestic'\n" +
           "\t   end ConType\n" +
           "\n" +
           "  FROM Consignment c\n" +
           "  inner join Branches b\n" +
           "  on b.branchCode = c.orgin\n" +
           " left outer join InternationalConsignmentDetail cd on cd.consignmentNo = c.consignmentNumber \n " +
           " where c.bookingDate = '" + clvar.Day + "'\n" +
           "   and c.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
           " order by contype, serviceTypeName";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 300;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        #region MyRegion
        //public DataTable GetDayCloseReport(Cl_Variables clvar)
        //{
        //    string sqlString = "select c.consignmentNumber [CN No],\n" +
        //    "\t   c.consigner Consigner,\n" +
        //    "\t   c.consignee,\n" +
        //    "\t   c.serviceTypeName,\n" +
        //    "\t   b.name Origin,\n" +
        //    "\t   c.destination Destination,\n" +
        //    "\t   c.consignerAccountNo AccNo,\n" +
        //    "\t   c.weight,\n" +
        //    "\t   c.gst TAX,\n" +
        //    "\t   c.chargedAmount,\n" +
        //    "     case\n" +
        //    "\t\t\twhen c.isInsured = '1'\n" +
        //    "\t\t\tthen (c.decalaredValue * (c.insuarancePercentage/100)) + c.totalAmount\n" +
        //    "\t\t\telse c.totalAmount\n" +
        //    "\t\tend TotalAmount," +
        //    "\t   case\n" +
        //    "\t\t\twhen c.orgin = c.destination\n" +
        //    "\t\t\tthen 'Local'\n" +
        //    "\t\t\telse 'Domestic'\n" +
        //    "\t   end ConType\n" +
        //    "\n" +
        //    "  FROM Consignment c\n" +
        //    "  inner join Branches b\n" +
        //    "  on b.branchCode = c.orgin\n" +
        //    " where c.bookingDate = '" + clvar.Day + "'\n" +
        //    "   and c.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
        //    " order by contype, serviceTypeName";



        //    sqlString = "select c.consignmentNumber [CN No], c.customerType,\n" +
        //   "\t   c.consigner Consigner,\n" +
        //   "\t   c.consignee,\n" +
        //   "\t   c.serviceTypeName,\n" +
        //   "\t   b.name Origin,\n" +
        //   "\t   (               \n" +
        //   "\t    SELECT b2.name    \n" +
        //   "\t    FROM   Branches b2    \n" +
        //   "\t   WHERE  b2.branchCode = c.destination    \n" +
        //   "\t )   DestinationBranch,   \n" +
        //   "\t  (        \n" +
        //   "\t      SELECT ec.name   \n" +
        //   "\t      FROM   ExpressCenters ec   \n" +
        //   "\t     WHERE  ec.expressCenterCode = c.destinationExpressCenterCode    \n" +
        //   "\t  )    Destination,     \n" +
        //   "\t   c.consignerAccountNo AccNo,\n" +
        //   "\t   c.weight,\n" +
        //   "\t   c.gst TAX,\n" +
        //   "\t   c.chargedAmount,\n" +
        //   "     case\n" +
        //   "\t\t\twhen c.isInsured = '1'\n" +
        //   "\t\t\tthen (c.decalaredValue * (c.insuarancePercentage/100)) + c.totalAmount\n" +
        //   "\t\t\telse c.totalAmount\n" +
        //   "\t\tend TotalAmount," +
        //   "\t   case when (select COUNT(vc.ConsignmentNo) from MNP_VOID_Consignment vc where vc.ConsignmentNo = c.consignmentNumber) > 0 then 'VOID'\n" +
        //   "\t\t\twhen c.orgin = c.destination\n" +
        //   "\t\t\tthen 'Local'\n" +
        //   "\t\t\telse 'Domestic'\n" +
        //   "\t   end ConType\n" +
        //   "\n" +
        //   "  FROM Consignment c\n" +
        //   "  inner join Branches b\n" +
        //   "  on b.branchCode = c.orgin\n" +
        //   " left outer join InternationalConsignmentDetail cd on cd.consignmentNo = c.consignmentNumber \n " +
        //   " where c.bookingDate = '" + clvar.Day + "'\n" +
        //   "   and c.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
        //   " order by contype, serviceTypeName";
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlConnection orcl = new SqlConnection(clvar.Strcon());
        //        orcl.Open();
        //        SqlCommand orcd = new SqlCommand(sqlString, orcl);
        //        orcd.CommandTimeout = 300;
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(dt);
        //        orcl.Close();
        //    }
        //    catch (Exception)
        //    { }

        //    return dt;
        //} 
        #endregion

        public DataTable GetBranchCodeForEC()
        {
            DataTable dt = new DataTable();

            string query = "SELECT BID FROM EXPRESSCENTERS WHERE EXPRESSCENTERCODE = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'";
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        public string VoidConsignment(Cl_Variables clvar)
        {
            string error = "";
            int count = 0;
            string query = "Insert Into MNP_VOID_Consignment (ConsignmentNo, Void, Remarks, DateEntry, ExpressCenter, BranchCode, CreatedBy, CreatedOn) Values \n" +
                            " (\n" +
                            "   '" + clvar.consignmentNo + "',\n" +
                            "   '" + clvar.VoidConsignment + "',\n" +
                            "   '" + clvar.Remarks + "',\n" +
                            "   '" + HttpContext.Current.Session["WorkingDate"].ToString() + "',\n" +
                            "   '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
                            "   '" + clvar.Branch + "',\n" +
                            "   '" + HttpContext.Current.Session["NAME"].ToString() + "',\n" +
                            "   GetDate()\n" +
                            ")";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();

                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count.ToString();
        }

        public string GenerateManifest(Cl_Variables clvar, DataTable dt)
        {
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            int count = 0;
            try
            {
                CommonFunction func = new CommonFunction();
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                string branchName = func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString();

                string query = "insert into MNP_Manifest (manifestNumber, origin, destination, branchCode, zoneCode, manifestType, date, createdBy, createdOn, isTemp)\n" +
                               " VALUES ( \n" +
                               "'" + clvar.manifestNo + "',\n" +
                               "'" + clvar.origin + "',\n" +
                               "'" + clvar.destination + "',\n" +
                               "'" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                               "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
                               "'" + clvar.ServiceTypeName + "',\n" +
                               "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
                               "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                               " GETDATE() ,\n" +
                               "'0'\n" +
                               ")";
                string query_ = "Insert into rvdbo.manifest (ManifestID, ManifestNo , ServiceTypeID, ManifestDate, ZoneID, OriginBranchID, DestBranchID,  isTemp,  isDemanifested) " +
                                "Values " +
                                "(" +
                                "'" + clvar.manifestNo.TrimStart('0') + "',\n" +
                                "'" + clvar.manifestNo + "',\n" +
                                "'" + clvar.serviceTypeId + "',\n" +
                                "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
                                "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
                                "'" + clvar.origin + "',\n" +
                                "'" + clvar.destination + "', '0', '0')";
                string query1_ = "Insert into rvdbo.Manifestconsignment (manifestID, ConsignmentID) ";
                string query1 = "INSERT INTO MNP_ConsignmentManifest (manifestNumber,consignmentNumber ) \n";
                string trackQuery = "INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber, stateId, CurrentLocation, manifestNumber, transactionTime) \n";
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
                    query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "','" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
                    trackQuery += "SELECT '" + dt.Rows[i][0].ToString() + "', '2', '" + branchName + "', '" + clvar.manifestNo + "',  GETDATE() \n" +
                                  " UNION ALL \n";

                }
                int j = dt.Rows.Count - 1;
                query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[j][0].ToString() + "'";
                query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "', '" + dt.Rows[j][0].ToString() + "'";
                trackQuery += "SELECT '" + dt.Rows[j][0].ToString() + "', '2', '" + branchName + "', '" + clvar.manifestNo + "',  GETDATE() \n";


                //SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                //SqlTransaction trans;
                //sqlcon.Open();
                //SqlCommand sqlcmd = new SqlCommand();
                //sqlcmd.Connection = sqlcon;
                //trans = sqlcon.BeginTransaction();
                //sqlcmd.Transaction = trans;
                //sqlcmd.CommandType = CommandType.Text;

                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    count = 0;
                    return "NOT OK";
                }
                //sqlcmd.CommandText = query_;
                //count = sqlcmd.ExecuteNonQuery();
                sqlcmd.CommandText = query1;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    count = 0;
                    return "NOT OK";
                }
                //sqlcmd.CommandText = query1_;
                //sqlcmd.ExecuteNonQuery();
                sqlcmd.CommandText = trackQuery;
                sqlcmd.ExecuteNonQuery();
                trans.Commit();
                // trans.Rollback();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                count = 0;
                return ex.Message;
            }
            finally
            {
                sqlcon.Close();
            }

            return "OK";
        }
        public string UpdateManifest(Cl_Variables clvar, DataTable dt, GridView gv)
        {
            int count = 0;
            string insertQuery = " INSERT INTO MNP_ConsignmentManifest (manifestNumber,consignmentNumber ) \n";
            string updateQuery = "";
            string delQuery = "";

            string case1 = "case consignmentNumber \n";
            string case2 = "case consignmentNumber \n";
            string case3 = "case consignmentNumber \n";
            string case4 = "case consignmentNumber \n";

            string updateCns = "";
            string delCns = "";
            List<string> insertCnsList = new List<string>();

            bool updateFlag = false;
            bool delFlag = false;
            bool insertFlag = false;

            foreach (GridViewRow row in gv.Rows)
            {
                if ((row.FindControl("hd_isModified") as HiddenField).Value == "DELETE")
                {
                    delFlag = true;
                    delCns += "'" + row.Cells[1].Text.ToString() + "'";
                    continue;
                }
                if ((row.FindControl("hd_isModified") as HiddenField).Value == "INSERT")
                {
                    insertFlag = true;
                    insertCnsList.Add("'" + row.Cells[1].Text.ToString() + "'");
                    count++;
                    continue;
                }
                case1 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("dd_gorigin") as DropDownList).SelectedValue + "'\n";
                case2 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("dd_contype") as DropDownList).SelectedValue + "'\n";
                case3 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("txt_gWeight") as TextBox).Text + "'\n";
                case4 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("txt_gPieces") as TextBox).Text + "'\n";
                updateCns += "'" + row.Cells[1].Text.ToString() + "'";

                updateFlag = true;
            }

            if (insertFlag)
            {
                for (int i = 0; i < insertCnsList.Count - 1; i++)
                {
                    insertQuery += "SELECT '" + clvar.manifestNo + "', " + insertCnsList[i].ToString() + " \n UNION ALL \n";
                }
                insertQuery += "SELECT '" + clvar.manifestNo + "', " + insertCnsList[insertCnsList.Count - 1].ToString() + "  \n";

            }
            updateCns = updateCns.Replace("''", "','");
            case1 += " end\n";
            case2 += " end\n";
            case3 += " end\n";
            case4 += " end\n";
            updateQuery = "UPDATE CONSIGNMENT SET Orgin = " + case1 + ", ConsignmentTypeID = " + case2 + ", weight = " + case3 + ", pieces = " + case4 + " where consignmentNumber in (" + updateCns + ")";

            delCns = delCns.Replace("''", "','");
            delQuery = "DELETE from MNP_ConsignmentManifest where manifestNumber = '" + clvar.manifestNo + "' and consignmentNumber in (" + delCns + ")";


            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                if (insertFlag)
                {
                    sqlcmd.CommandText = insertQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                if (updateFlag)
                {
                    sqlcmd.CommandText = updateQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                if (delFlag)
                {
                    sqlcmd.CommandText = delQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();

                return ex.Message;
            }

            sqlcon.Close();

            return "OK";
        }


        public string InsertConsignmentsFromRunsheet(Cl_Variables clvar, DataTable dt)
        {
            string trackQuery = "insert into ConsignmentsTrackingHistory \n" +
                                   "  (consignmentNumber, stateID, currentLocation, transactionTime)\n";



            //" ) ";
            int count = 0;
            string check = "";
            string query = "";
            string query1 = "";

            query = " INSERT INTO CONSIGNMENT (ConsignmentNumber, Orgin, Destination, CreditClientId, ConsignerAccountNo, weight , pieces, syncid,bookingDate,createdOn,zoneCode, branchCode, serviceTypeName, consignmentTypeId) ";

            //}
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                query += " SELECT '" + dt.Rows[i]["ConNo"].ToString() + "', '" + dt.Rows[i]["Origin"].ToString() + "', '" + dt.Rows[i]["Name"].ToString() + "', '" + clvar.CustomerClientID + "', '" + clvar.AccountNo + "', '" + clvar.Weight + "', '" + clvar.pieces + "' , NewID(), GETDATE(), GETDATE(), '" + HttpContext.Current.Session["zonecode"].ToString() + "',\n" +
                            "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "', 'overnight', '12'\n" +
                            " UNION ALL";
                trackQuery += " SELECT   '" + dt.Rows[i]["CONNO"].ToString() + "',\n" +
                                   "   '1',\n" +
                                   "   '" + dt.Rows[i]["CITYCODE"].ToString() + "',\n" +
                                   "   GETDATE()\n UNION ALL";
            }
            int j = dt.Rows.Count - 1;
            query += " SELECT '" + dt.Rows[j]["ConNo"].ToString() + "', '" + dt.Rows[j]["Origin"].ToString() + "', '" + dt.Rows[j]["Name"].ToString() + "', '" + clvar.CustomerClientID + "', '" + clvar.AccountNo + "', '" + clvar.Weight + "', '" + clvar.pieces + "' , NewID(),GETDATE(), GETDATE(),'" + HttpContext.Current.Session["zonecode"].ToString() + "',\n" +
                     "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "', 'overnight', '12'\n";

            trackQuery += " SELECT   '" + dt.Rows[j]["CONNO"].ToString() + "',\n" +
                                   "   '1',\n" +
                                   "   '" + dt.Rows[j]["CITYCODE"].ToString() + "',\n" +
                                   "   GETDATE()\n";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                sqlcmd.CommandText = trackQuery;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "Track Failed";
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                return "NOT OK" + ex.Message;
            }
            return "OK";
        }


        public int GenerateManifest1(Cl_Variables clvar, DataTable dt)
        {
            int count = 0;
            string query = "insert into Manifest (manifestNumber, origin, destination, branchCode, zoneCode, manifestType, date, createdBy, createdOn, isTemp)\n" +
                           " VALUES ( \n" +
                           "'" + clvar.manifestNo + "',\n" +
                           "'" + clvar.origin + "',\n" +
                           "'" + clvar.destination + "',\n" +
                           "'" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                           "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
                           "'" + clvar.ServiceTypeName + "',\n" +
                           "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
                           "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                           " GETDATE() ,\n" +
                           "'0'\n" +
                           ")";
            //string query_ = "Insert into rvdbo.manifest (ManifestID, ManifestNo , ServiceTypeID, ManifestDate, ZoneID, OriginBranchID, DestBranchID,  isTemp,  isDemanifested) " +
            //                "Values " +
            //                "(" +
            //                "'" + clvar.manifestNo.TrimStart('0') + "',\n" +
            //                "'" + clvar.manifestNo + "',\n" +
            //                "'" + clvar.serviceTypeId + "',\n" +
            //                "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
            //                "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
            //                "'" + clvar.origin + "',\n" +
            //                "'" + clvar.destination + "', '0', '0')";
            //string query1_ = "Insert into rvdbo.Manifestconsignment (manifestID, ConsignmentID) ";
            //string query1 = "INSERT INTO ConsignmentManifest (manifestNumber,consignmentNumber ) \n";
            //for (int i = 0; i < dt.Rows.Count - 1; i++)
            //{
            //    query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
            //    query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "','" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
            //}
            //int j = dt.Rows.Count - 1;
            //query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[j][0].ToString() + "'";
            //query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "', '" + dt.Rows[j][0].ToString() + "'";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                }
            }
            catch (Exception ex)
            {
                //trans.Rollback();
                count = 0;
            }
            finally
            {
                sqlcon.Close();
            }

            return count;
        }

        public int GenerateManifest2(Cl_Variables clvar, DataTable dt)
        {
            int count = 0;
            //string query = "insert into Manifest (manifestNumber, origin, destination, branchCode, zoneCode, manifestType, date, createdBy, createdOn, isTemp)\n" +
            //               " VALUES ( \n" +
            //               "'" + clvar.manifestNo + "',\n" +
            //               "'" + clvar.origin + "',\n" +
            //               "'" + clvar.destination + "',\n" +
            //               "'" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
            //               "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
            //               "'" + clvar.ServiceTypeName + "',\n" +
            //               "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
            //               "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
            //               " GETDATE() ,\n" +
            //               "'0'\n" +
            //               ")";
            string query_ = "Insert into rvdbo.manifest (ManifestID, ManifestNo , ServiceTypeID, ManifestDate, ZoneID, OriginBranchID, DestBranchID,  isTemp,  isDemanifested) " +
            "Values " +
            "(" +
            "'" + clvar.manifestNo.TrimStart('0') + "',\n" +
            "'" + clvar.manifestNo + "',\n" +
            "'" + clvar.serviceTypeId + "',\n" +
            "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
            "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
            "'" + clvar.origin + "',\n" +
            "'" + clvar.destination + "', '0', '0')";
            //string query1_ = "Insert into rvdbo.Manifestconsignment (manifestID, ConsignmentID) ";
            //string query1 = "INSERT INTO ConsignmentManifest (manifestNumber,consignmentNumber ) \n";
            //for (int i = 0; i < dt.Rows.Count - 1; i++)
            //{
            //    query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
            //    query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "','" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
            //}
            //int j = dt.Rows.Count - 1;
            //query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[j][0].ToString() + "'";
            //query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "', '" + dt.Rows[j][0].ToString() + "'";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query_;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sqlcon.Close();
            }

            return count;
        }

        public int GenerateManifest3(Cl_Variables clvar, DataTable dt)
        {
            int count = 0;
            //string query = "insert into Manifest (manifestNumber, origin, destination, branchCode, zoneCode, manifestType, date, createdBy, createdOn, isTemp)\n" +
            //               " VALUES ( \n" +
            //               "'" + clvar.manifestNo + "',\n" +
            //               "'" + clvar.origin + "',\n" +
            //               "'" + clvar.destination + "',\n" +
            //               "'" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
            //               "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
            //               "'" + clvar.ServiceTypeName + "',\n" +
            //               "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
            //               "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
            //               " GETDATE() ,\n" +
            //               "'0'\n" +
            //               ")";
            //string query_ = "Insert into rvdbo.manifest (ManifestID, ManifestNo , ServiceTypeID, ManifestDate, ZoneID, OriginBranchID, DestBranchID,  isTemp,  isDemanifested) " +
            //"Values " +
            //"(" +
            //"'" + clvar.manifestNo.TrimStart('0') + "',\n" +
            //"'" + clvar.manifestNo + "',\n" +
            //"'" + clvar.serviceTypeId + "',\n" +
            //"'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
            //"'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
            //"'" + clvar.origin + "',\n" +
            //"'" + clvar.destination + "', '0', '0')";
            string query1_ = "Insert into rvdbo.Manifestconsignment (manifestID, ConsignmentID) ";
            // string query1 = "INSERT INTO ConsignmentManifest (manifestNumber,consignmentNumber ) \n";
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                //  query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
                query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "','" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
            }
            int j = dt.Rows.Count - 1;
            //query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[j][0].ToString() + "'";
            query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "', '" + dt.Rows[j][0].ToString() + "'";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query1_;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sqlcon.Close();
            }

            return count;
        }

        public int GenerateManifest4(Cl_Variables clvar, DataTable dt)
        {
            int count = 0;
            //string query = "insert into Manifest (manifestNumber, origin, destination, branchCode, zoneCode, manifestType, date, createdBy, createdOn, isTemp)\n" +
            //               " VALUES ( \n" +
            //               "'" + clvar.manifestNo + "',\n" +
            //               "'" + clvar.origin + "',\n" +
            //               "'" + clvar.destination + "',\n" +
            //               "'" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
            //               "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
            //               "'" + clvar.ServiceTypeName + "',\n" +
            //               "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
            //               "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
            //               " GETDATE() ,\n" +
            //               "'0'\n" +
            //               ")";
            //string query_ = "Insert into rvdbo.manifest (ManifestID, ManifestNo , ServiceTypeID, ManifestDate, ZoneID, OriginBranchID, DestBranchID,  isTemp,  isDemanifested) " +
            //"Values " +
            //"(" +
            //"'" + clvar.manifestNo.TrimStart('0') + "',\n" +
            //"'" + clvar.manifestNo + "',\n" +
            //"'" + clvar.serviceTypeId + "',\n" +
            //"'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
            //"'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
            //"'" + clvar.origin + "',\n" +
            //"'" + clvar.destination + "', '0', '0')";
            //string query1_ = "Insert into rvdbo.Manifestconsignment (manifestID, ConsignmentID) ";
            string query1 = "INSERT INTO ConsignmentManifest (manifestNumber,consignmentNumber ) \n";
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
                // query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "','" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
            }
            int j = dt.Rows.Count - 1;
            query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[j][0].ToString() + "'";
            //query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "', '" + dt.Rows[j][0].ToString() + "'";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query1;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sqlcon.Close();
            }

            return count;
        }

        public DataTable GetConsignmentDetailForNewManifest(Cl_Variables clvar)
        {

            string sqlString = "select c.consignmentNumber,\n" +
            "       c.orgin,\n" +
            "       b.name              OriginName,\n" +
            "       c.destination,\n" +
            "       b2.name             DestinationName,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces, 'NO' ISMODIFIED\n" +
            "  from consignment c\n" +
            " inner join Branches b\n" +
            "    on c.orgin = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on c.destination = b2.branchCode\n" +
            " where c.consignmentNumber like '" + clvar.consignmentNo + "'\n" +
            "   --and c.serviceTypeName like '" + clvar.ServiceTypeName + "'\n" +
            "   --and c.orgin = '" + clvar.origin + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetConsignmentDetailByManifestNumber(Cl_Variables clvar)
        {

            string sqlString = "select c.consignmentNumber, c.consigner, c.consignee, c.weight,\n" +
            "       c.orgin,\n" +
            "       b.name              OriginName,\n" +
            "       c.destination,\n" +
            "       b2.name             DestinationName,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces, 'NO' ISMODIFIED,\n" +
            "       (Select DATE from MNP_Manifest where manifestNumber = '" + clvar.manifestNo + "') ManifestDate" +
            "  from consignment c\n" +
            " inner join Branches b\n" +
            "    on c.orgin = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on c.destination = b2.branchCode\n" +
            " where c.consignmentNumber in ( SELECT cm.ConsignmentNumber from MNP_ConsignmentManifest cm where cm.manifestNumber = '" + clvar.manifestNo + "'  )";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetManifestHeader(Cl_Variables clvar)
        {

            string sqlString = "\n" +
            "select b3.name          Branch,\n" +
            "       m.date,\n" +
            "       m.manifestNumber,\n" +
            "       b.name           Origin,\n" +
            "       b2.name          Destination\n" +
            "  from MNP_Manifest m\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = m.origin\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = m.destination\n" +
            " inner join Branches b3\n" +
            "    on b3.branchCode = m.branchCode\n" +
            " where m.manifestNumber = '" + clvar.consignmentNo + "'";

            sqlString = "\n" +
           "select b3.name          Branch,\n" +
           "       m.date, m.origin OCODE, m.Destination DCODE,\n" +
           "       m.manifestNumber,\n" +
           "       b.name           Origin,\n" +
           "       b2.name          Destination, m.manifestType, m.date\n" +
           "  from MNP_Manifest m\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = m.origin\n" +
           " inner join Branches b2\n" +
           "    on b2.branchCode = m.destination\n" +
           " inner join Branches b3\n" +
           "    on b3.branchCode = m.branchCode\n" +
           " where m.manifestNumber = '" + clvar.manifestNo + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetRoutesByDestination()
        {
            string query = "  select r.MovementRouteCode, r.Name from rvdbo.MovementRoute r where r.DestBranchId = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetLoadingBags(Cl_Variables clvar)
        {

            string sqlString = "select b.BagNo,\n" +
            "\t   b.TotalWeight,\n" +
            "\t   b.OriginBranchId,\n" +
            "\t   b.DestBranchId,\n" +
            "\t   case\n" +
            "\t\t\twhen b.IsUnBaged = 0\n" +
            "\t\t\t\t then 'Bagged'\n" +
            "\t\t\twhen b.IsUnBaged=1\n" +
            "\t\t\t\t then 'Unbagged'\n" +
            "\t   end Status,\n" +
            "\t   b.SealNo,\n" +
            "\t   '' Reason\n" +
            "  from rvdbo.Loading l\n" +
            " inner join rvdbo.LoadingBag lb\n" +
            "    on lb.LoadingId = l.LoadingId\n" +
            " inner join rvdbo.Bag b\n" +
            "  on b.BagId = lb.BagId\n" +
            " where l.LoadingId = '" + clvar.LoadingID + "'";

            sqlString = "select b.BagNo,\n" +
                "'' Description, \n" +
           "\t   b.TotalWeight,\n" +
           "\t   b1.name OriginBranchId,\n" +
           "\t   b2.name DestBranchId,\n" +
           "\t   case\n" +
           "\t\t\twhen b.IsUnBaged = 0\n" +
           "\t\t\t\t then 'Bagged'\n" +
           "\t\t\twhen b.IsUnBaged=1\n" +
           "\t\t\t\t then 'Unbagged'\n" +
           "\t   end Status,\n" +
           "\t   b.SealNo,\n" +
           "\t   '' Reason\n" +
           "  from rvdbo.Loading l\n" +
           " inner join rvdbo.LoadingBag lb\n" +
           "    on lb.LoadingId = l.LoadingId\n" +
           " inner join rvdbo.Bag b\n" +
           "  on b.BagId = lb.BagId\n" +
           " inner join Branches b1\n" +
           " on b1.branchCode = b.OriginBranchId\n" +
           " inner join Branches b2\n" +
           " on b2.branchCode = b.DestBranchId\n" +

           " where l.LoadingId = '" + clvar.LoadingID + "' and l.DestBranchId = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;


        }
        public DataTable GetLoading(Cl_Variables clvar)
        {

            string sqlString = "select l.LoadingId,\n" +
            "       l.TransportTypeId,\n" +
            "       l.VehicleId,\n" +
            "       b.name            OriginBranch,\n" +
            "       b2.name           DestBranchID,\n" +
            "       l.LoadingDate,\n" +
            "       l.Description LoadDescription,\n" +
            "       v.description + ' (' + v.name + ')' VehicleDescription,\n" +
            "       lu.AttributeDesc TransportType , l.CourierName, r.name RouteName\n" +
            "  from rvdbo.Loading l\n" +
            " inner join Branches b\n" +
            "    on l.OriginBranchId = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on l.DestBranchId = b2.branchCode\n" +
            " inner join Vehicle v\n" +
            "    on v.Id = l.VehicleId\n" +
            " inner join rvdbo.Lookup lu\n" +
            "    on lu.Id = l.TransportTypeId\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " inner join rvdbo.MovementRoute r \n" +
            "    on r.MovementRouteId = l.MovementRouteId\n" +
            " where l.LoadingId = '" + clvar.LoadingID + "' and l.DestBranchId = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;

        }

        public int InsertUnloading(Cl_Variables clvar)
        {
            int count = 0;
            string query = "update rvdbo.Loading set isunloaded = '1' where loadingID = '" + clvar.LoadingID + "'";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();

                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                //error = ex.Message;
            }
            return count;
        }

        public int PerformUnloading(Cl_Variables clvar)
        {
            string normalBags = "";
            int count = 0;
            string query = "update rvdbo.Loading set isunloaded = '1' where loadingID = '" + clvar.LoadingID + "'";
            foreach (string str in clvar.NormalBags)
            {
                normalBags += "'" + str + "'";
            }
            normalBags = normalBags.Replace("''", "','");
            string query2 = "Insert into BagTracking (bagnumber, createdby, createdon, arrivedAt, ArrivalDate, arrivalTime, status) Select b.bagNumber, '" + HttpContext.Current.Session["U_ID"].ToString() + "', GetDate(), '" + clvar.Branch + "', '" + HttpContext.Current.Session["WorkingDate"].ToString() + "', '" + DateTime.Now.TimeOfDay + "', '2'  from Bag b where b.bagNumber in (" + normalBags + ") ";
            string query1 = "Insert into rvdbo.BagTracking ( BagId, ArriveAtBranchID, ArriveOn, IsFinal) select b.BagId, '" + clvar.Branch + "', GETDATE(), '1' from rvdbo.Bag b where b.BagNo in (" + normalBags + ") ";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query2;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();

                    return count;
                }
                count = 0;
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();

                    return count;
                }
                count = 0;
                sqlcmd.CommandText = query1;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();

                    return count;
                }
                trans.Commit();
            }
            catch (Exception ex)
            { trans.Rollback(); }
            return count;
        }

        public DataTable GetBagDetail(Cl_Variables clvar)
        {

            string sqlString = "select b.bagNumber,\n" +
            "       b.origin,\n" +
            "       b1.name       OriginName,\n" +
            "       b.destination,\n" +
            "       b2.name       DestName,\n" +
            "       b.totalWeight,\n" +
            "       b.sealNo,\n" +
            "       b.date,\n" +
            "       b.description\n" +
            "  from Bag b\n" +
            " inner join Branches b1\n" +
            "    on b.origin = b1.branchCode\n" +
            "\n" +
            " inner join Branches b2\n" +
            "    on b.destination = b2.branchCode\n" +
            " where b.bagNumber = '" + clvar.BagNumber + "' --and b.destination = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;


        }

        public int RerouteBag(Cl_Variables clvar)
        {
            int count = 0;
            string query = " update BagTracking set modifiedby = '" + HttpContext.Current.Session["U_ID"].ToString() + "', modifiedON = GETDATE(), DepartedTo = '" + clvar.destId + "', departedDate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', status = '1' where  id = (Select MAX(id) from BagTracking where bagNumber = '" + clvar.BagNumber + "')";

            string query1 = " update rvdbo.bagTracking set departureToBranchID = '" + clvar.destId + "', DepartureOn = GetDATE(), isFinal = '0' where  BagTrackingId = (select MAX(BagTrackingId) from rvdbo.BagTracking where BagId = (select BagId from rvdbo.Bag where BagNo = '" + clvar.BagNumber + "'))";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return count;
                }
                sqlcmd.CommandText = query1;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return count;
                }
                trans.Commit();
            }
            catch (Exception ex)
            { trans.Rollback(); }
            finally { sqlcon.Close(); }

            return count;
        }

        public DataTable GetBagManifests(Cl_Variables clvar)
        {

            string sqlString = "select b.BagManifestId,\n" +
            "   \t   m.ManifestNo,\n" +
            "   \t   b.ManifestId,\n" +
            "   \t   m.OriginBranchId,\n" +
            "   \t   b1.name Origin,\n" +
            "   \t   b2.name Dest,\n" +
            "   \t   m.DestBranchId, '' Description\n" +
            "  from rvdbo.BagManifest b\n" +
            " inner join rvdbo.Manifest m\n" +
            "    on m.ManifestId = b.ManifestId\n" +
            " inner join Branches b1\n" +
            "    on b1.branchCode = m.OriginBranchId\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = m.DestBranchId\n" +
            " where b.BagId = (select BagId from rvdbo.Bag  where BagNo = '" + clvar.BagNumber + "')";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;


        }

        public string UpdateBagManifests(Cl_Variables clvar)
        {
            string error = "";
            int count = 0;
            string numbers = "";
            string shorts = "";
            string shortsRemarks = "";
            string bagManifestID = "";
            for (int i = 0; i < clvar.ReceivedManifests.Count; i++)
            {
                numbers += "'" + clvar.ReceivedManifests[i] + "'";
            }

            numbers = numbers.Replace("''", "','");
            for (int i = 0; i < clvar.ShortManifests.Count; i++)
            {
                shorts += "'" + clvar.ShortManifests[i] + "'";
            }
            shorts = shorts.Replace("''", "','");

            for (int i = 0; i < clvar.ShortManifestRemarks.Count; i++)
            {
                shortsRemarks += clvar.ShortManifestRemarks[i] + "\n";
            }

            foreach (string item in clvar.BagManifestID)
            {
                bagManifestID += "'" + item + "'";
            }
            bagManifestID = bagManifestID.Replace("''", "','");
            //shortsRemarks += clvar.ShortManifestRemarks[clvar.ShortManifestRemarks.Count - 1];
            string query = "UPDATE rvdbo.bagManifest set UnBagStateID = '5' where ManifestID in (select rm.ManifestId from rvdbo.Manifest rm\n" +
                           "where rm.ManifestNo in (" + numbers + ")";

            string query1 = "Update BagManifest set statusCode = '5' where manifestNumber in (" + numbers + ")";

            string specialQuery = "";
            string query2 = "UPDATE rvdbo.bagManifest set UnBagStateID = '6', remarks = CASE BAGMANIFESTID " + clvar.CheckCondition + " end where BagManifestID in (" + bagManifestID + ")";

            string query3 = "Update bagmanifest set statusCode = '6', reason = CASE MANIFESTNUMBER " + shortsRemarks + " end where manifestNumber in (" + shorts + ")";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                if (clvar.ReceivedManifests.Count > 0)
                {
                    sqlcmd.CommandText = query;
                    count = sqlcmd.ExecuteNonQuery();
                    sqlcmd.CommandText = query1;
                    count = sqlcmd.ExecuteNonQuery();
                }


                if (clvar.BagManifestID.Count > 0)
                {
                    sqlcmd.CommandText = query2;
                    count = sqlcmd.ExecuteNonQuery();
                }

                if (clvar.ShortManifests.Count > 0)
                {
                    sqlcmd.CommandText = query3;
                    count = sqlcmd.ExecuteNonQuery();
                }







                trans.Commit();
            }
            catch (Exception ex)
            { trans.Rollback(); error = ex.Message; }
            return error;
        }

        public DataTable GetConsignmentsInManifests(Cl_Variables clvar)
        {
            string sqlString = "select cm.manifestNumber ManifestId, \n"
                + "	   m.manifestNumber ManifestNo, \n"
                + "	   cm.DeManifestStateID, \n"
                + "	   c.consignmentNumber, \n"
                + "	   cm.Remarks, \n"
                + "	   c.orgin, \n"
                + "	   b1.name Origin, \n"
                + "	   b2.name Dest, \n"
                + "	   c.destination, \n"
                + "	   c.consignmentTypeId, \n"
                + "	   ct.name ConType, \n"
                + "	   c.serviceTypeName, \n"
                + "	   c.weight \n"
                + " \n"
                + "FROM MNP_ConsignmentManifest cm \n"
                + " \n"
                + " inner join Consignment c \n"
                + "    on c.consignmentNumber = cast (cm.consignmentNumber as varchar) \n"
                + " \n"
                + " inner join MNP_Manifest m \n"
                + "	on m.manifestNumber = cm.manifestNumber \n"
                + " \n"
                + " inner join Branches b1 \n"
                + "	on b1.branchCode = c.orgin \n"
                + " \n"
                + " inner join Branches b2 \n"
                + "	on b2.branchCode = c.destination \n"
                + " \n"
                + " inner join ConsignmentType ct \n"
                + "	on ct.id = c.consignmentTypeId"
                + " where m.manifestNumber = '" + clvar.manifestNo + "'\n"
                + "   and m.destination = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetLoadingHeader(Cl_Variables clvar)
        {

            string sqlString = "select lu.AttributeDesc TransportType,\n" +
            "       l.date,\n" +
            "       v.MakeModel + ' (' + v.Description + ')' VehicleName,\n" +
            "       l.courierName,\n" +
            "       b1.name OrgName,\n" +
            "       b2.name DestName,\n" +
            "       l.description\n" +
            "  from Loading l\n" +
            " inner join rvdbo.Lookup lu\n" +
            "    on lu.AttributeValue = l.transportationType\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " inner join rvdbo.Vehicle v\n" +
            "    on v.VehicleCode = l.vehicleId\n" +
            " inner join Branches b1\n" +
            "    on b1.branchCode = l.origin\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = l.destination\n" +
            " where l.id = '" + clvar.LoadingID + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        public DataTable GetLoadingDetail(Cl_Variables clvar)
        {

            string sqlString = "select lb.loadingId,\n" +
            "\t   b.bagNumber, b.totalWeight,\n" +
            "\t   b1.name OrgName,\n" +
            "\t   b2.name DestName,\n" +
            "\t   b.sealNo\n" +
            "  from LoadingBag lb\n" +
            " inner join Bag b\n" +
            "\ton b.bagNumber = lb.bagNumber\n" +
            " inner join Branches b1\n" +
            "\ton b1.branchCode = b.origin\n" +
            " inner join Branches b2\n" +
            "\ton b2.branchCode = b.destination\n" +
            " where lb.loadingId = '" + clvar.LoadingID + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public string Demanifest(Cl_Variables clvar)
        {
            string error = "";
            string check1 = "";
            string check2 = "";
            foreach (string item in clvar.NormalBags)
            {
                check1 += "'" + item + "'";
            }
            check1 = check1.Replace("''", "','");


            string query = " update MNP_ConsignmentManifest set statusCode = '5' where consignmentNumber in (" + check1 + ")";

            string query1 = "update MNP_ConsignmentManifest set DemanifestStateID = '5' where consignmentNumber in (" + check1 + ")";
            check1 = "";
            foreach (string item in clvar.ShortReceivedBags)
            {
                check1 += "'" + item + "'";
            }
            check1 = check1.Replace("''", "','");


            foreach (string item in clvar.ShortManifestRemarks)
            {
                check2 += item;
            }

            string query2 = "update MNP_ConsignmentManifest set statusCode = '6', reason = CASE consignmentNumber " + check2 + " end where consignmentNumber in (" + check1 + ")";

            string query3 = "update MNP_ConsignmentManifest set DemanifestStateID = '6' , Remarks = Case consignmentNumber " + check2 + " end where consignmentNumber in (" + check1 + ")";

            string query6 = "INSERT into ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, manifestNumber, TransactionTime) \n";
            int j = clvar.NormalBags.Count - 1;
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            for (int i = 0; i < clvar.NormalBags.Count - 1; i++)
            {
                query6 += "  SELECT '" + clvar.NormalBags[i] + "', '7', '', '" + clvar.manifestNo + "', GETDATE()\n" +
                            "UNION ALL";
            }
            query6 += "  SELECT '" + clvar.NormalBags[j] + "', '7', '', '" + clvar.manifestNo + "', GETDATE()\n";

            string query8 = "UPDATE Mnp_Manifest \n" +
                      "SET isDemanifested = '1', DemanifestDate = GETDATE(), DemanifestBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "'   " +
                      "WHERE manifestNumber ='" + clvar.manifestNo + "'";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            int count = 0;
            try
            {

                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                count = 0;

                sqlcmd.CommandText = query1;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }


                count = 0;
                if (clvar.ShortReceivedBags.Count > 0)
                {
                    sqlcmd.CommandText = query2;
                    count = sqlcmd.ExecuteNonQuery();
                    if (count == 0)
                    {
                        trans.Rollback();
                        return "NOT OK";
                    }
                    count = 0;
                    sqlcmd.CommandText = query3;
                    count = sqlcmd.ExecuteNonQuery();
                    if (count == 0)
                    {
                        trans.Rollback();
                        return "NOT OK";
                    }
                    count = 0;
                }
                sqlcmd.CommandText = query6;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                count = 0;


                sqlcmd.CommandText = query8;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                count = 0;

                // sqlcmd.CommandText = query3;
                // count = sqlcmd.ExecuteNonQuery();
                // if (count == 0)
                // {
                //     trans.Rollback();
                //     return "NOT OK";
                // }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;
            }

            sqlcon.Close();


            return "OK";
        }

        public string GenerateRunsheet(Cl_Variables clvar)
        {
            CommonFunction func = new CommonFunction();
            Int64 runsheetNumber = (Convert.ToInt64(func.GetMaxRunsheetNumber().Rows[0][0].ToString())) + 1;
            string error = "";
            int count = 0;

            string query = "insert into Runsheet (runsheetNumber, routeCode, createdBy, createdOn, runsheetDate, branchCode, runsheetType, route, syncID)\n" +
            "\t\t\tValues   (\n" +
            "                   '" + runsheetNumber.ToString() + "',\n" +
            "                   '" + clvar.routeCode + "',\n" +
            "                   '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
            "                   GetDate(),\n" +
            "                   '" + DateTime.Parse(clvar.RunSheetDate).ToString("yyyy-MM-dd") + "',\n" +
            "                   '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
            "                   '" + clvar.RunSheetTypeID + "',\n" +
            "                   '" + clvar.RouteDesc + "', NEWID()" +
            "\t\t\t)";

            string query2 = "insert into rvdbo.Runsheet (RunsheetId, RunsheetNo, RunsheetDate, RouteId, BranchId, ZoneId, RunsheetTypeId, RiderId, createdon, CreatedById )\n" +
            "              Values (\n" +
            "                       '" + runsheetNumber.ToString() + "',\n" +
            "                       '" + runsheetNumber.ToString() + "',\n" +
            "                       '" + DateTime.Parse(clvar.RunSheetDate).ToString("yyyy-MM-dd") + "',\n" +
            "                       '" + clvar.routeCode + "',\n" +
            "                       '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
            "                       '" + HttpContext.Current.Session["ZoneCode"].ToString() + "',\n" +
            "                       '" + clvar.RunSheetTypeID + "',\n" +
            "                       '" + clvar.riderCode + "',\n" +
            "                       GetDAte(),\n" +
            "                       '" + HttpContext.Current.Session["U_ID"].ToString() + "'" +
            "                )";

            string query3 = "insert into RunsheetConsignment (runsheetNumber, consignmentNumber, createdBy, createdOn, Status, SortOrder,branchcode,RouteCode) \n";
            string query4 = "insert into rvdbo.RunsheetConsignment (RunsheetId, ConsignmentId, CreatedOn, StatusId, PODUpdateTypeId, SortOrder, IsPODSpecified, BranchCode )";
            string query6 = "INSERT into ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, riderName, runsheetNumber, TransactionTime) \n";
            int j = clvar.ClvarListStr.Count - 1;
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            for (int i = 0; i < clvar.ClvarListStr.Count - 1; i++)
            {
                query3 += " SELECT '" + runsheetNumber.ToString() + "', '" + clvar.ClvarListStr[i] + "', '" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE(), 'UNDELIVERED', '" + (i + 1).ToString() + "', '" + HttpContext.Current.Session["branchcode"].ToString() + "','" + clvar.routeCode + "'\n" +
                          " UNION ALL \n";
                query4 += "  SELECT '" + runsheetNumber.ToString() + "', '" + clvar.ClvarListStr[i] + "', GETDATE(), '56', '112','" + (i + 1).ToString() + "', '0', '" + clvar.Branch + "'\n" +
                            "UNION ALL \n";
                query6 += "  SELECT '" + clvar.ClvarListStr[i] + "', '8', '" + func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString() + "', '" + clvar.RiderCode + "', '" + runsheetNumber.ToString() + "', GETDATE()\n" +
                            "UNION ALL";
            }
            query3 += " SELECT '" + runsheetNumber.ToString() + "', '" + clvar.ClvarListStr[j] + "', '" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE(), 'UNDELIVERED', '" + (j + 1) + "', '" + HttpContext.Current.Session["branchcode"].ToString() + "','" + clvar.routeCode + "'\n";
            query4 += "SELECT '" + runsheetNumber.ToString() + "', '" + clvar.ClvarListStr[j] + "', GETDATE(), '56', '112', '" + (j + 1) + "', '0', '" + clvar.Branch + "'\n";
            query6 += "  SELECT '" + clvar.ClvarListStr[j] + "', '8', '" + func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString() + "', '" + clvar.RiderCode + "', '" + runsheetNumber.ToString() + "', GETDATE()\n";
            string query5 = "insert into RiderRunsheet (ridercode, runsheetNumber, createdBy, createdOn, expIdTemp) Values (\n" +
                           "                       '" + clvar.riderCode + "',\n" +
                           "                       '" + runsheetNumber.ToString() + "',\n" +
                           "                       '" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE(),\n" +
                           "                       (select r.expressCenterId from Riders r where r.riderCode = '" + clvar.riderCode + "' and r.status = '1' and branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "')\n" +
                           "                )";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;

            try
            {
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                count = 0;
                //sqlcmd.CommandText = query2;
                //count = sqlcmd.ExecuteNonQuery();
                //if (count == 0)
                //{
                //    trans.Rollback();
                //    return "NOT OK";
                //}
                //count = 0;
                sqlcmd.CommandText = query3;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                count = 0;
                //sqlcmd.CommandText = query4;
                //count = sqlcmd.ExecuteNonQuery();
                //if (count == 0)
                //{
                //    trans.Rollback();
                //    return "NOT OK";
                //}
                //count = 0;
                sqlcmd.CommandText = query5;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                count = 0;
                sqlcmd.CommandText = query6;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "OK";
                }
                trans.Commit();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return "Could Not Save Runsheet Error: " + ex.Message;
            }
            return runsheetNumber.ToString();
        }


        public string InsertConsignmentsFromRunsheet_(Cl_Variables clvar, DataTable dt)
        {
            int count = 0;
            string check = "";
            string query = "";
            string query1 = "";
            query = " INSERT INTO CONSIGNMENT (ConsignmentNumber, Orgin, Destination, CreditClientId, ConsignerAccountNo, syncid) ";
            //foreach (DataRow row in dt.Rows)
            //{
            //    query1 += "";

            //}
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                query += " SELECT '" + dt.Rows[i]["ConNo"].ToString() + "', '" + dt.Rows[i]["Origin"].ToString() + "', '" + dt.Rows[i]["Name"].ToString() + "', '" + clvar.CustomerClientID + "', '" + clvar.AccountNo + "' , NewID()\n" +
                        " UNION ALL";
            }
            int j = dt.Rows.Count - 1;
            query += " SELECT '" + dt.Rows[j]["ConNo"].ToString() + "', '" + dt.Rows[j]["Origin"].ToString() + "', '" + dt.Rows[j]["Name"].ToString() + "', '" + clvar.CustomerClientID + "', '" + clvar.AccountNo + "' , NewID()\n" +
                        "";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                return "NOT OK" + ex.Message;
            }
            return "OK";
        }

        /*
        public DataTable GetConsignmentsForRunsheet(Cl_Variables clvar)
        {

            string sqlString = "select rc.SortOrder,\n" +
            "       rc.runsheetNumber,\n" +
            "       c.consignmentNumber,\n" +
            "       c.consignee,\n" +
            "       c.orgin,\n" +
            "       b.name ONAME,\n" +
            "       c.pieces,\n" +
            "       rc.time,\n" +
            "       rc.receivedBy,\n" +
            "       rc.Status,\n" +
            "       rc.deliveryDate,\n" +
            "       rc.Reason,\n" +
            "       rc.Comments\n" +
            "  from Runsheet r\n" +
            " inner join RunsheetConsignment rc\n" +
            "    on r.runsheetNumber = rc.runsheetNumber\n" +
            " inner join Consignment c\n" +
            "    on c.consignmentNumber = rc.consignmentNumber\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = c.orgin\n" +
            " where rc.runsheetNumber = '" + clvar.RunSheetNumber + "'\n" +
            " order by SortOrder";

            sqlString = "\tselect rc.SortOrder,\n" +
           "\t\t   rc.RunsheetId,\n" +
           "\t\t   c.consignmentNumber,\n" +
           "\t\t   c.consignee,\n" +
           "\t\t   c.orgin,\n" +
           "\t\t   b.name ONAME,\n" +
           "\t\t   c.pieces,\n" +
           "\t\t   rc.DeliveryDateTime ,\n" +
           "\t\t   rc.receivedBy,\n" +
           "\t\t   rc.StatusId,\n" +
           "\t\t   rc.ReasonId,\n" +
           "\t\t   rc.Comments\n" +
           "\t from Runsheet r\n" +
           "\tinner join rvdbo.RunsheetConsignment  rc\n" +
           "\ton r.runsheetNumber = CAST(rc.RunsheetId as varchar)\n" +
           "\tinner join Consignment c\n" +
           "\ton c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
           "\tinner join Branches b\n" +
           "\ton b.branchCode = c.orgin\n" +
           "where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "'\n" +
           "order by SortOrder";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        */

        public DataTable GetConsignmentsForRunsheet_(Cl_Variables clvar)
        {

            string sqlString = "select rc.SortOrder,\n" +
            "       rc.runsheetNumber,\n" +
            "       c.consignmentNumber,\n" +
            "       c.consignee,\n" +
            "       c.orgin,\n" +
            "       b.name ONAME,\n" +
            "       c.pieces,\n" +
            "       rc.time,\n" +
            "       rc.receivedBy,\n" +
            "       rc.Status,\n" +
            "       rc.deliveryDate,\n" +
            "       rc.Reason,\n" +
            "       rc.Comments\n" +
            "  from Runsheet r\n" +
            " inner join RunsheetConsignment rc\n" +
            "    on r.runsheetNumber = rc.runsheetNumber\n" +
            " inner join Consignment c\n" +
            "    on c.consignmentNumber = rc.consignmentNumber\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = c.orgin\n" +
            " where rc.runsheetNumber = '" + clvar.RunSheetNumber + "'\n" +
            " order by SortOrder";

            sqlString = "\tselect rc.SortOrder,\n" +
           "\t\t   rc.RunsheetId,\n" +
           "\t\t   c.consignmentNumber,\n" +
           "\t\t   c.consignee,\n" +
           "\t\t   c.orgin,\n" +
           "\t\t   b.name ONAME,\n" +
           "\t\t   c.pieces,\n" +
           "\t\t   rc.DeliveryDateTime ,\n" +
           "\t\t   rc.receivedBy,\n" +
           "\t\t   rc.StatusId,\n" +
           "\t\t   rc.ReasonId,\n" +
           "\t\t   rc.Comments\n" +
           "\t from Runsheet r\n" +
           "\tinner join rvdbo.RunsheetConsignment  rc\n" +
           "\ton r.runsheetNumber = CAST(rc.RunsheetId as varchar)\n" +
           "\tinner join Consignment c\n" +
           "\ton c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
           "\tinner join Branches b\n" +
           "\ton b.branchCode = c.orgin\n" +

           "where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "order by SortOrder DESC";


            sqlString = "select rc.SortOrder,\n" +
           "       rc.RunsheetId,\n" +
           "       c.consignmentNumber,\n" +
           "       c.consignee,\n" +
           "       c.orgin,\n" +
           "       b.name ONAME,\n" +
           "       c.pieces,\n" +
           "       rc.DeliveryDateTime,\n" +
           "       rc.receivedBy,\n" +
           "       rc.StatusId,\n" +
           "       rc.ReasonId,\n" +
           "       rc.Comments\n" +
           "  from Runsheet r\n" +
           " inner join rvdbo.RunsheetConsignment rc\n" +
           "    on r.runsheetNumber = CAST(rc.RunsheetId as varchar)\n" +
           "    AND r.branchCode = rc.BranchCode\n" +
           //" inner join RunsheetConsignment rcc\n" +
           //"    on rcc.runsheetNumber = rc.RunsheetId\n" +
           //"   and rcc.consignmentNumber = rc.ConsignmentId\n" +
           //"   and rcc.branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           //"\n" +
           " inner join Consignment c\n" +
           "    on c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = c.orgin\n" +
           " where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "'\n" +
           "   and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           " group by rc.SortOrder,\n" +
           "       rc.RunsheetId,\n" +
           "       c.consignmentNumber,\n" +
           "       c.consignee,\n" +
           "       c.orgin,\n" +
           "       b.name ,\n" +
           "       c.pieces,\n" +
           "       rc.DeliveryDateTime,\n" +
           "       rc.receivedBy,\n" +
           "       rc.StatusId,\n" +
           "       rc.ReasonId,\n" +
           "       rc.Comments\n" +
           " order by SortOrder ";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        /*
        public DataTable GetConsignmentsForRunsheet(Cl_Variables clvar)
        {

            string sqlString = "select rc.SortOrder,\n" +
            "       rc.runsheetNumber,\n" +
            "       c.consignmentNumber,\n" +
            "       c.consignee,\n" +
            "       c.orgin,\n" +
            "       b.name ONAME,\n" +
            "       c.pieces,\n" +
            "       rc.time,\n" +
            "       rc.receivedBy,\n" +
            "       rc.Status,\n" +
            "       rc.deliveryDate,\n" +
            "       rc.Reason,\n" +
            "       rc.Comments\n" +
            "  from Runsheet r\n" +
            " inner join RunsheetConsignment rc\n" +
            "    on r.runsheetNumber = rc.runsheetNumber\n" +
            " inner join Consignment c\n" +
            "    on c.consignmentNumber = rc.consignmentNumber\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = c.orgin\n" +
            " where rc.runsheetNumber = '" + clvar.RunSheetNumber + "'\n" +
            " order by SortOrder";

            sqlString = "\tselect rc.SortOrder,\n" +
           "\t\t   rc.RunsheetId,\n" +
           "\t\t   c.consignmentNumber,\n" +
           "\t\t   c.consignee,\n" +
           "\t\t   c.orgin,\n" +
           "\t\t   b.name ONAME,\n" +
           "\t\t   c.pieces,\n" +
           "\t\t   rc.DeliveryDateTime ,\n" +
           "\t\t   rc.receivedBy,\n" +
           "\t\t   rc.StatusId,\n" +
           "\t\t   rc.ReasonId,\n" +
           "\t\t   rc.Comments\n" +
           "\t from Runsheet r\n" +
           "\tinner join rvdbo.RunsheetConsignment  rc\n" +
           "\ton r.runsheetNumber = CAST(rc.RunsheetId as varchar)\n" +
           "\tinner join Consignment c\n" +
           "\ton c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
           "\tinner join Branches b\n" +
           "\ton b.branchCode = c.orgin\n" +

           "where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "order by SortOrder DESC";


            sqlString = "select rc.SortOrder,\n" +
           "       rc.RunsheetId,\n" +
           "       c.consignmentNumber,\n" +
           "       c.consignee,\n" +
           "       c.orgin,\n" +
           "       b.name ONAME,\n" +
           "       c.pieces,\n" +
           "       rc.DeliveryDateTime,\n" +
           "       rc.receivedBy,\n" +
           "       rc.StatusId,\n" +
           "       rc.ReasonId,\n" +
           "       rc.Comments, rc.Receiver_CNIC\n" +
           "  from Runsheet r\n" +
           " inner join rvdbo.RunsheetConsignment rc\n" +
           "    on r.runsheetNumber = CAST(rc.RunsheetId as varchar)\n" +
           "\n" +
           " inner join RunsheetConsignment rcc\n" +
           "    on rcc.runsheetNumber = rc.RunsheetId\n" +
           "   and rcc.consignmentNumber = rc.ConsignmentId\n" +
           "   and rcc.branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "\n" +
           " inner join Consignment c\n" +
           "    on c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = c.orgin\n" +
           " where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "'\n" +
           "   and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           " group by rc.SortOrder,\n" +
           "       rc.RunsheetId,\n" +
           "       c.consignmentNumber,\n" +
           "       c.consignee,\n" +
           "       c.orgin,\n" +
           "       b.name ,\n" +
           "       c.pieces,\n" +
           "       rc.DeliveryDateTime,\n" +
           "       rc.receivedBy,\n" +
           "       rc.StatusId,\n" +
           "       rc.ReasonId,\n" +
           "       rc.Comments,rc.Receiver_CNIC\n" +
           " order by SortOrder ";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        */

        #region Commented by Rabi on 05/08/2016 07:43PM
        //public DataTable GetConsignmentsForRunsheet(Cl_Variables clvar)
        //{

        //    string sqlString = "select rc.SortOrder,\n" +
        //    "       rc.runsheetNumber,\n" +
        //    "       c.consignmentNumber,\n" +
        //    "       c.consignee,\n" +
        //    "       c.orgin,\n" +
        //    "       b.name ONAME,\n" +
        //    "       c.pieces,\n" +
        //    "       rc.time,\n" +
        //    "       rc.receivedBy,\n" +
        //    "       rc.Status,\n" +
        //    "       rc.deliveryDate,\n" +
        //    "       rc.Reason,\n" +
        //    "       rc.Comments\n" +
        //    "  from Runsheet r\n" +
        //    " inner join RunsheetConsignment rc\n" +
        //    "    on r.runsheetNumber = rc.runsheetNumber\n" +
        //    " inner join Consignment c\n" +
        //    "    on c.consignmentNumber = rc.consignmentNumber\n" +
        //    " inner join Branches b\n" +
        //    "    on b.branchCode = c.orgin\n" +
        //    " where rc.runsheetNumber = '" + clvar.RunSheetNumber + "'\n" +
        //    " order by SortOrder";

        //    sqlString = "\tselect rc.SortOrder,\n" +
        //   "\t\t   rc.RunsheetId,\n" +
        //   "\t\t   c.consignmentNumber,\n" +
        //   "\t\t   c.consignee,\n" +
        //   "\t\t   c.orgin,\n" +
        //   "\t\t   b.name ONAME,\n" +
        //   "\t\t   c.pieces,\n" +
        //   "\t\t   rc.DeliveryDateTime ,\n" +
        //   "\t\t   rc.receivedBy,\n" +
        //   "\t\t   rc.StatusId,\n" +
        //   "\t\t   rc.ReasonId,\n" +
        //   "\t\t   rc.Comments\n" +
        //   "\t from Runsheet r\n" +
        //   "\tinner join rvdbo.RunsheetConsignment  rc\n" +
        //   "\ton r.runsheetNumber = CAST(rc.RunsheetId as varchar)\n" +
        //   "\tinner join Consignment c\n" +
        //   "\ton c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
        //   "\tinner join Branches b\n" +
        //   "\ton b.branchCode = c.orgin\n" +

        //   "where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
        //   "order by SortOrder DESC";


        //    sqlString = "select rc.SortOrder,\n" +
        //   "       rc.RunsheetId,\n" +
        //   "       c.consignmentNumber,\n" +
        //   "       c.consignee,\n" +
        //   "       c.orgin,\n" +
        //   "       b.name ONAME,\n" +
        //   "       c.pieces,\n" +
        //   "       rc.DeliveryDateTime,\n" +
        //   "       rc.receivedBy,\n" +
        //   "       rc.StatusId,\n" +
        //   "       rc.ReasonId,\n" +
        //   "       rc.Comments, rc.Receiver_CNIC, rc.Relation\n" +
        //   "  from Runsheet r\n" +
        //   "INNER JOIN rvdbo.RunsheetConsignment rc       \n" +
        //   "     ON  r.runsheetNumber = CAST(rc.RunsheetId AS VARCHAR)    \n" +
        //   "     AND r.branchCode = rc.BranchCode   \n" +
        //   "INNER JOIN RunsheetConsignment RC2           \n" +
        //   "     ON  R.runsheetNumber = RC2.runsheetNumber    \n" +
        //   "     AND rc.RunsheetId = RC2.runsheetNumber       \n" +
        //   "     AND rc.ConsignmentId = RC2.consignmentNumber \n" +
        //   "     AND rc.BranchCode = RC2.branchcode           \n" +
        //   "     AND r.createdBy = RC2.createdby           \n" +
        //   "INNER JOIN Consignment c                        \n" +
        //   "    on c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
        //   " inner join Branches b\n" +
        //   "    on b.branchCode = c.orgin\n" +
        //   " where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "'\n" +
        //   "   and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
        //   " group by rc.SortOrder,\n" +
        //   "       rc.RunsheetId,\n" +
        //   "       c.consignmentNumber,\n" +
        //   "       c.consignee,\n" +
        //   "       c.orgin,\n" +
        //   "       b.name ,\n" +
        //   "       c.pieces,\n" +
        //   "       rc.DeliveryDateTime,\n" +
        //   "       rc.receivedBy,\n" +
        //   "       rc.StatusId,\n" +
        //   "       rc.ReasonId,\n" +
        //   "       rc.Comments,rc.Receiver_CNIC, rc.Relation\n" +
        //   " order by SortOrder ";

        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlConnection orcl = new SqlConnection(clvar.Strcon());
        //        orcl.Open();
        //        SqlCommand orcd = new SqlCommand(sqlString, orcl);
        //        orcd.CommandType = CommandType.Text;
        //        orcd.CommandTimeout = 300;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(dt);
        //        orcl.Close();
        //    }
        //    catch (Exception)
        //    { }

        //    return dt;
        //} 
        #endregion

        public DataTable GetConsignmentsForRunsheet(Cl_Variables clvar)
        {

            string sqlString = "select rc.SortOrder,\n" +
            "       rc.runsheetNumber,\n" +
            "       c.consignmentNumber,\n" +
            "       c.consignee,\n" +
            "       c.orgin,\n" +
            "       b.name ONAME,\n" +
            "       c.pieces,\n" +
            "       rc.time,\n" +
            "       rc.receivedBy,\n" +
            "       rc.Status,\n" +
            "       rc.deliveryDate,\n" +
            "       rc.Reason,\n" +
            "       rc.Comments\n" +
            "  from Runsheet r\n" +
            " inner join RunsheetConsignment rc\n" +
            "    on r.runsheetNumber = rc.runsheetNumber\n" +
            " inner join Consignment c\n" +
            "    on c.consignmentNumber = rc.consignmentNumber\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = c.orgin\n" +
            " where rc.runsheetNumber = '" + clvar.RunSheetNumber + "'\n" +
            " order by SortOrder";

            sqlString = "\tselect rc.SortOrder,\n" +
           "\t\t   rc.RunsheetId,\n" +
           "\t\t   c.consignmentNumber,\n" +
           "\t\t   c.consignee,\n" +
           "\t\t   c.orgin,\n" +
           "\t\t   b.name ONAME,\n" +
           "\t\t   c.pieces,\n" +
           "\t\t   rc.DeliveryDateTime ,\n" +
           "\t\t   rc.receivedBy,\n" +
           "\t\t   rc.StatusId,\n" +
           "\t\t   rc.ReasonId,\n" +
           "\t\t   rc.Comments\n" +
           "\t from Runsheet r\n" +
           "\tinner join rvdbo.RunsheetConsignment  rc\n" +
           "\ton r.runsheetNumber = CAST(rc.RunsheetId as varchar)\n" +
           "\tinner join Consignment c\n" +
           "\ton c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
           "\tinner join Branches b\n" +
           "\ton b.branchCode = c.orgin\n" +

           "where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "order by SortOrder DESC";


            sqlString = "select rc.SortOrder,\n" +
           "       rc.RunsheetId,\n" +
           "       c.consignmentNumber,\n" +
           "       c.consignee,\n" +
           "       c.orgin,\n" +
           "       b.name ONAME,\n" +
           "       c.pieces,\n" +
           "       rc.DeliveryDateTime,\n" +
           "       rc.receivedBy,\n" +
           "       rc.StatusId,\n" +
           "       rc.ReasonId,\n" +
           "       rc.Comments, rc.Receiver_CNIC, rc.Relation\n" +
           "  from Runsheet r\n" +
           "INNER JOIN rvdbo.RunsheetConsignment rc       \n" +
           "     ON  r.runsheetNumber = CAST(rc.RunsheetId AS VARCHAR)    \n" +
           "     AND r.branchCode = rc.BranchCode   \n" +
           "INNER JOIN RunsheetConsignment RC2           \n" +
           "     ON  R.runsheetNumber = RC2.runsheetNumber AND R.createdBy = RC2.createdBy      \n" +
           "     AND rc.RunsheetId = RC2.runsheetNumber       \n" +
           "     AND rc.ConsignmentId = RC2.consignmentNumber \n" +
           "     AND rc.BranchCode = RC2.branchcode           \n" +
           "INNER JOIN Consignment c                        \n" +
           "    on c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = c.orgin\n" +
           " where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "'\n" +
           "   and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "   and r.routeCode = '" + clvar.routeCode + "'\n" +
           " group by rc.SortOrder,\n" +
           "       rc.RunsheetId,\n" +
           "       c.consignmentNumber,\n" +
           "       c.consignee,\n" +
           "       c.orgin,\n" +
           "       b.name ,\n" +
           "       c.pieces,\n" +
           "       rc.DeliveryDateTime,\n" +
           "       rc.receivedBy,\n" +
           "       rc.StatusId,\n" +
           "       rc.ReasonId,\n" +
           "       rc.Comments,rc.Receiver_CNIC, rc.Relation\n" +
           " order by SortOrder ";


            sqlString = "select ROW_NUMBER() OVER(order by rc2.SortOrder desc) SortOrder,\n" +
           "       rc2.runsheetNumber  RunsheetId,\n" +
           "       c.consignmentNumber,\n" +
           "       c.consignee,\n" +
           "       c.orgin,\n" +
           "       b.name              ONAME,\n" +
           "       c.pieces,\n" +
           "       cast(rc2.time  as time)  DeliveryDateTime,\n" +
           "       RC2.receivedBy,\n" +
           "       RC2.Status          StatusId,\n" +
           "       rc2.Reason          ReasonId,\n" +
           "       rc2.Comments,\n" +
           "       rc2.Receiver_CNIC,\n" +
           "       rc2.Relation\n" +
           "  from Runsheet r\n" +
           " INNER JOIN RunsheetConsignment RC2\n" +
           "    ON R.runsheetNumber = RC2.runsheetNumber\n" +
           "   and r.branchCode = RC2.branchcode\n" +
           "   and r.routeCode = RC2.RouteCode\n" +
           "   AND R.createdBy = RC2.createdBy\n" +
           "\n" +
           " INNER JOIN Consignment c\n" +
           "    on c.consignmentNumber = CAST(rc2.consignmentNumber as varchar)\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = c.orgin\n" +
           " where cast(RC2.runsheetNumber as varchar) = '" + clvar.RunSheetNumber + "'\n" +
           "   and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "   and r.routeCode = '" + clvar.routeCode + "'\n" +
           " group by rc2.SortOrder,\n" +
           "          rc2.runsheetNumber,\n" +
           "          c.consignmentNumber,\n" +
           "          c.consignee,\n" +
           "          c.orgin,\n" +
           "          b.name,\n" +
           "          c.pieces,\n" +
           "          rc2.time,\n" +
           "          RC2.receivedBy,\n" +
           "          RC2.Status,\n" +
           "          rc2.Reason,\n" +
           "          rc2.Comments,\n" +
           "          rc2.Receiver_CNIC,\n" +
           "          rc2.Relation\n" +
           " order by SortOrder ";


            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        public string UpdatePOD_(System.Web.UI.WebControls.GridView gv, Cl_Variables clvar)
        {

            CommonFunction func = new CommonFunction();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string branchName = func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString();
            int count = 0;
            string timeCheck = "";
            string receivedByCheck = "";
            string deliveryDateCheck = "";
            string statusCheck = "";
            string statusCheck1 = "";
            string reasonCheck = "";
            string reasonCheck1 = "";
            string commentsCheck = "";
            string consignmentNumbers = "";

            string trackQuery = "INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, RunsheetNumber, riderName, TransactionTime, Reason, StatusTime)";
            bool flag1 = false;

            foreach (System.Web.UI.WebControls.GridViewRow row in gv.Rows)
            {
                Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)row.FindControl("txt_gDelvDate");
                System.Web.UI.WebControls.DropDownList status = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gStatus");
                System.Web.UI.WebControls.TextBox receivedBy = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gReceivedBy");
                System.Web.UI.WebControls.TextBox time = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gTime");
                System.Web.UI.WebControls.DropDownList reason = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gReason");
                System.Web.UI.WebControls.TextBox comments = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gComments");
                consignmentNumbers += "'" + row.Cells[1].Text + "'";

                if (status.SelectedValue != "56" && status.SelectedValue != "55")
                {
                    if (picker.SelectedDate == null)
                    {
                        deliveryDateCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL\n";
                        timeCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL\n";
                    }
                    else
                    {
                        deliveryDateCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + "' \n";
                        if (time.Text != "")
                        {
                            timeCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000' \n";
                        }
                        else
                        {
                            timeCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL \n";
                        }


                    }
                }

                if (status.SelectedValue != "0")
                {
                    statusCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + status.SelectedItem.Text + "' \n";
                    statusCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN '" + status.SelectedValue + "' \n";
                }
                else
                {
                    statusCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL \n";
                    statusCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN NULL \n";
                }
                if (reason.SelectedValue != "0")
                {
                    reasonCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + reason.SelectedItem.Text + "' \n";
                    reasonCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN '" + reason.SelectedValue + "' \n";
                }
                else
                {
                    reasonCheck += " WHEN '" + row.Cells[1].Text + "' THEN '' \n";
                    reasonCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN '' \n";
                }

                //if (comments.Text.Trim() != "")
                //{
                flag1 = true;
                commentsCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + comments.Text + "' \n";
                //}

                receivedByCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + receivedBy.Text + "' \n";



            }
            consignmentNumbers = consignmentNumbers.Replace("''", "','");
            string conNumbers = "";
            bool track = false;
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)gv.Rows[i].FindControl("txt_gDelvDate");
                System.Web.UI.WebControls.DropDownList status = (System.Web.UI.WebControls.DropDownList)gv.Rows[i].FindControl("dd_gStatus");
                System.Web.UI.WebControls.TextBox receivedBy = (System.Web.UI.WebControls.TextBox)gv.Rows[i].FindControl("txt_gReceivedBy");
                System.Web.UI.WebControls.TextBox time = (System.Web.UI.WebControls.TextBox)gv.Rows[i].FindControl("txt_gTime");
                System.Web.UI.WebControls.DropDownList reason = (System.Web.UI.WebControls.DropDownList)gv.Rows[i].FindControl("dd_gReason");
                System.Web.UI.WebControls.TextBox comments = (System.Web.UI.WebControls.TextBox)gv.Rows[i].FindControl("txt_gComments");
                //System.Web.UI.WebControls.Label Llb = (System.Web.UI.WebControls.Label)gv.Rows[i].FindControl("Lbl_1");

                conNumbers = gv.Rows[i].Cells[1].Text;
                if (gv.Rows[i].Enabled == true)
                {
                    track = true;
                    trackQuery += " SELECT '" + conNumbers + "', '10', '" + branchName + "', '" + clvar.RunsheetNumber + "', '" + clvar.RiderCode + "', GETDATE(), '" + status.SelectedItem.Text + "',\n";
                    if (status.SelectedValue != "56")
                    {
                        trackQuery += "'" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000'\n" +
                                  " UNION ALL\n";
                    }
                    else
                    {
                        trackQuery += "GETDATE()\n" +
                                  " UNION ALL\n";
                    }


                }

            }
            //int j = gv.Rows.Count - 1;
            trackQuery = trackQuery.Remove(trackQuery.Length - 12);

            //string query = "Update runsheetConsignment Set time = Case ConsignmentNumber " + timeCheck + " end,\n" +
            //                "                        receivedBy = Case ConsignmentNumber " + receivedByCheck + " end,\n" +
            //                "                          Comments = Case ConsignmentNumber " + commentsCheck + " end,\n" +
            //                "                            Status = Case ConsignmentNumber " + statusCheck + " end,\n" +
            //                "                            Reason = Case ConsignmentNumber " + reasonCheck + " end, \n" +
            //                "                      DeliveryDate = Case ConsignmentNumber " + deliveryDateCheck + " end\n" +
            //                " where ConsignmentNumber in (" + consignmentNumbers + ") and runsheetNumber = '" + clvar._RunsheetNumber + "' and branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
            //string query1 = "Update rvdbo.runsheetConsignment Set DeliveryDateTime = Case ConsignmentId " + timeCheck + " end,\n" +
            //                "                        receivedBy = Case ConsignmentId " + receivedByCheck + " end,\n" +
            //                "                          Comments = Case ConsignmentId " + commentsCheck + " end,\n" +
            //                "                          StatusID = Case ConsignmentId " + statusCheck1 + " end,\n" +
            //                "                          ReasonID = Case ConsignmentId " + reasonCheck1 + " end--, \n" +
            //    //"                      DeliveryDate = Case ConsignmentNumber " + deliveryDateCheck + " end\n" +
            //                " where ConsignmentId in (" + consignmentNumbers + ") and RunsheetID = '" + clvar._RunsheetNumber + "' and BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            string query2 = "INSERT INTO CONSIGNMENTSTRACKING";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;

            try
            {
                //sqlcmd.CommandText = query;
                //count = sqlcmd.ExecuteNonQuery();
                //if (count == 0)
                //{
                //    trans.Rollback();
                //    return "NOT OK";
                //}
                //count = 0;
                //sqlcmd.CommandText = query1;
                //count = sqlcmd.ExecuteNonQuery();
                //if (count == 0)
                //{
                //    trans.Rollback();
                //    return "NOT OK";
                //}
                if (track)
                {
                    sqlcmd.CommandText = trackQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;
            }

            sqlcon.Close();
            return "OK";
        }

        #region Commented by Rabi on 06/08/2016 02:38PM
        //public string UpdatePOD_Consignment(System.Web.UI.WebControls.GridView gv, Cl_Variables clvar)
        //{
        //    ArrayList Consignment_ = new ArrayList();
        //    string Consignment = "";
        //    foreach (System.Web.UI.WebControls.GridViewRow row in gv.Rows)
        //    {
        //        Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)row.FindControl("txt_gDelvDate");
        //        System.Web.UI.WebControls.DropDownList status = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gStatus");
        //        System.Web.UI.WebControls.TextBox receivedBy = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gReceivedBy");
        //        System.Web.UI.WebControls.TextBox time = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gTime");
        //        System.Web.UI.WebControls.DropDownList reason = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gReason");
        //        System.Web.UI.WebControls.TextBox comments = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gComments");
        //        System.Web.UI.WebControls.Label Lable = (System.Web.UI.WebControls.Label)row.FindControl("Lbl_1");

        //        Consignment = "'" + row.Cells[1].Text + "'";
        //        DateTime dt;
        //        if (picker.SelectedDate != null)
        //        {
        //            dt = Convert.ToDateTime(picker.SelectedDate);
        //        }
        //        else
        //        {
        //            dt = Convert.ToDateTime("1900-01-01");
        //        }
        //        if (status.SelectedValue == "55" && Lable.Text != string.Empty)
        //        {
        //            string query = "Update runsheetConsignment Set time = '" + time.Text + "' ,\n" +
        //                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
        //                            "                          Comments = '" + comments.Text + "' ,\n" +
        //                            "                            Status = '" + status.SelectedValue + "' ,\n" +
        //                            "                            Reason = '" + reason.SelectedValue + "' , \n" +
        //                            "                      DeliveryDate = '" + Lable.Text + "'\n" +
        //                            " where ConsignmentNumber = " + Consignment + " and runsheetNumber = '" + clvar._RunsheetNumber + "' and branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
        //            string query1 = "Update rvdbo.runsheetConsignment Set DeliveryDateTime = '" + time.Text + "' ,\n" +
        //                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
        //                            "                          Comments = '" + comments.Text + "' ,\n" +
        //                            "                          StatusID = '" + status.SelectedValue + "' ,\n" +
        //                            "                          ReasonID = '" + reason.SelectedValue + "' \n" +
        //                // "                      DeliveryDate =  " + Lable.Text + " \n" +
        //                            " where ConsignmentId = " + Consignment + " and RunsheetID = '" + clvar._RunsheetNumber + "' and BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

        //            Consignment_.Add(query);
        //            Consignment_.Add(query1);

        //        }
        //        else if (time.Text == "__:__")
        //        {
        //            string query = "Update runsheetConsignment Set time = '" + dt.ToString("yyyy-MM-dd") + "' ,\n" +
        //                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
        //                            "                          Comments = '" + comments.Text + "' ,\n" +
        //                            "                            Status = '" + status.SelectedValue + "' ,\n" +
        //                            "                            Reason = '" + reason.SelectedValue + "' , \n" +
        //                            "                      DeliveryDate = '" + dt.ToString("yyyy-MM-dd") + "' \n" +
        //                            " where ConsignmentNumber = " + Consignment + " and runsheetNumber = '" + clvar._RunsheetNumber + "' and branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
        //            string query1 = "Update rvdbo.runsheetConsignment Set DeliveryDateTime = '" + dt.ToString("yyyy-MM-dd") + "' ,\n" +
        //                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
        //                            "                          Comments = '" + comments.Text + "' ,\n" +
        //                            "                          StatusID = '" + status.SelectedValue + "' ,\n" +
        //                            "                          ReasonID = '" + reason.SelectedValue + "'  \n" +
        //                //    "                      DeliveryDate =  " + picker.Text + " \n" +
        //                            " where ConsignmentId = " + Consignment + " and RunsheetID = '" + clvar._RunsheetNumber + "' and BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

        //            Consignment_.Add(query);
        //            Consignment_.Add(query1);

        //        }
        //        else
        //        {
        //            string query = "Update runsheetConsignment Set time = '" + time.Text + "' ,\n" +
        //                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
        //                            "                          Comments = '" + comments.Text + "' ,\n" +
        //                            "                            Status = '" + status.SelectedValue + "' ,\n" +
        //                            "                            Reason = '" + reason.SelectedValue + "' , \n" +
        //                            "                      DeliveryDate = '" + dt.ToString("yyyy-MM-dd") + "' \n" +
        //                            " where ConsignmentNumber = " + Consignment + " and runsheetNumber = '" + clvar._RunsheetNumber + "' and branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
        //            string query1 = "Update rvdbo.runsheetConsignment Set DeliveryDateTime = '" + time.Text + "' ,\n" +
        //                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
        //                            "                          Comments = '" + comments.Text + "' ,\n" +
        //                            "                          StatusID = '" + status.SelectedValue + "' ,\n" +
        //                            "                          ReasonID = '" + reason.SelectedValue + "'  \n" +
        //                //    "                      DeliveryDate =  " + picker.Text + " \n" +
        //                            " where ConsignmentId = " + Consignment + " and RunsheetID = '" + clvar._RunsheetNumber + "' and BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

        //            Consignment_.Add(query);
        //            Consignment_.Add(query1);


        //        }

        //    }

        //    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
        //    SqlTransaction trans;

        //    sqlcon.Open();
        //    SqlCommand dbCommand = new SqlCommand("", sqlcon);

        //    var sqlStatementArray = dbCommand.CommandText.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        //    trans = sqlcon.BeginTransaction();

        //    try
        //    {
        //        dbCommand.Transaction = trans;
        //        foreach (string sqlStatement in Consignment_)
        //        {
        //            dbCommand.CommandText = sqlStatement;
        //            dbCommand.ExecuteNonQuery();
        //        }
        //        trans.Commit();
        //    }
        //    catch (Exception Ex)
        //    {
        //        trans.Rollback();
        //        return Ex.Message;
        //    }


        //    return "OK";
        //} 
        #endregion
        public string UpdatePOD_Consignment(System.Web.UI.WebControls.GridView gv, Cl_Variables clvar)
        {
            CommonFunction func = new CommonFunction();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string branchName = func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString();
            string trackQuery = "";
            DataTable consignmentOps = GetConsignmentOps(gv, clvar);
            DataTable consignmentOps_ = new DataTable();
            consignmentOps_.Columns.Add("consignmentID");
            consignmentOps_.Columns.Add("Operation");
            consignmentOps_.AcceptChanges();
            ArrayList Consignment_ = new ArrayList();
            string Consignment = "";
            foreach (System.Web.UI.WebControls.GridViewRow row in gv.Rows)
            {
                Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)row.FindControl("txt_gDelvDate");
                System.Web.UI.WebControls.DropDownList status = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gStatus");
                System.Web.UI.WebControls.TextBox receivedBy = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gReceivedBy");
                System.Web.UI.WebControls.TextBox time = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gTime");
                System.Web.UI.WebControls.DropDownList reason = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gReason");
                System.Web.UI.WebControls.TextBox comments = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gComments");
                //System.Web.UI.WebControls.Label Lable = (System.Web.UI.WebControls.Label)row.FindControl("Lbl_1");
                if (consignmentOps.Select("ConsignmentID = '" + row.Cells[1].Text + "'").Count() == 0)
                {
                    DataRow dr = consignmentOps_.NewRow();
                    dr["consignmentID"] = row.Cells[1].Text;
                    dr["Operation"] = "insert";
                    consignmentOps_.Rows.Add(dr);
                    consignmentOps_.AcceptChanges();
                }
                else
                {
                    DataRow dr = consignmentOps_.NewRow();
                    dr["consignmentID"] = row.Cells[1].Text;
                    dr["Operation"] = "update";
                    consignmentOps_.Rows.Add(dr);
                    consignmentOps_.AcceptChanges();
                }

                Consignment = "'" + row.Cells[1].Text + "'";
                if (row.Enabled == true)
                {
                    if (reason.SelectedValue != "0")
                    {
                        if (status.SelectedValue == "55")
                        {
                            string query = "Update runsheetConsignment Set time = '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000' ,\n" +
                                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
                                            "                          Comments = '" + comments.Text + "' ,\n" +
                                            "                            Status = '" + status.SelectedValue + "' ,\n" +
                                            "                            Reason = '" + reason.SelectedValue + "' , \n" +
                                            "                      DeliveryDate = '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + "'\n" +
                                            " where ConsignmentNumber = " + Consignment + " and runsheetNumber = '" + clvar._RunsheetNumber + "' and branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
                            string query1 = "Update rvdbo.runsheetConsignment Set DeliveryDateTime = '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000' ,\n" +
                                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
                                            "                          Comments = '" + comments.Text + "' ,\n" +
                                            "                          StatusID = '" + status.SelectedValue + "' ,\n" +
                                            "                          ReasonID = '" + reason.SelectedValue + "' \n" +
                                            // "                      DeliveryDate =  " + Lable.Text + " \n" +
                                            " where ConsignmentId = " + Consignment + " and RunsheetID = '" + clvar._RunsheetNumber + "' and BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
                            trackQuery = "INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, RunsheetNumber, riderName, TransactionTime, Reason, StatusTime)\n" +
                                            " VALUES (\n" +
                                            "'" + row.Cells[1].Text + "', '10', '" + branchName + "', '" + clvar.RunsheetNumber + "', '" + clvar.RiderCode + "', GETDATE(), '" + status.SelectedItem.Text + "','" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000'\n" +
                                            ")";
                            Consignment_.Add(query);
                            Consignment_.Add(trackQuery);
                            string query2 = "";
                            if (consignmentOps_.Select("consignmentID = '" + row.Cells[1].Text + "'")[0]["Operation"].ToString() == "insert")
                            {
                                query2 = "insert into mnp_consignmentOperations (\n" +
                                    "consignmentID,\n" +
                                    " operationalType,\n" +
                                    " OriginBranchID,\n" +
                                    " DestBranchID,\n" +
                                    " ConsignmentTypeID,\n" +
                                    " isReturned,\n" +
                                    " CnStatus,\n" +
                                    "isMisrouted,\n" +
                                    "weight,\n" +
                                    "ScreenID,\n" +
                                    "ServiceTypeid,\n" +
                                    "CreatedOn,\n" +
                                    "NoOfPieces,\n" +
                                    "isRunsheetAllowed,\n" +
                                    "isDelivered\n" +
                                    ")\n" +
                                    "VALUES (\n" +
                                    "'" + row.Cells[1].Text + "',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0',\n" +//isreturned
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0.5',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "GETDATE(),\n" +
                                    "'1',\n" +
                                    "'0',\n" +
                                    "'1')";
                            }
                            else
                            {
                                query2 = "UPDATE mnp_consignmentOperations set isReturned = '0', isDelivered = '1', isRunsheetAllowed = '0' where consignmentid = '" + row.Cells[1].Text + "'";
                            }

                            Consignment_.Add(query2);
                            //Consignment_.Add(query1);

                        }
                        else
                        {
                            string query = "Update runsheetConsignment Set --time = '" + time.Text + "' ,\n" +
                                            "                        --receivedBy = '" + receivedBy.Text + "' ,\n" +
                                            "                          Comments = '" + comments.Text + "' ,\n" +
                                            "                           Status = '" + status.SelectedValue + "', \n" +
                                            "                            Reason = '" + reason.SelectedValue + "' --, \n" +
                                            "                      --DeliveryDate = '" + picker.DateInput.Text + "' \n" +
                                            " where ConsignmentNumber = " + Consignment + " and runsheetNumber = '" + clvar._RunsheetNumber + "' and branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
                            string query1 = "Update rvdbo.runsheetConsignment Set --DeliveryDateTime = " + time.Text + "' ,\n" +
                                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
                                            "                          Comments = '" + comments.Text + "' ,\n" +
                                            "                          StatusID = '" + status.SelectedValue + "' ,\n" +
                                            "                          ReasonID = '" + reason.SelectedValue + "'  \n" +
                                            //    "                      DeliveryDate =  " + picker.Text + " \n" +
                                            " where ConsignmentId = " + Consignment + " and RunsheetID = '" + clvar._RunsheetNumber + "' and BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
                            trackQuery = "INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, RunsheetNumber, riderName, TransactionTime, Reason, StatusTime)\n" +
                                            " VALUES (\n" +
                                            "'" + row.Cells[1].Text + "', '10', '" + branchName + "', '" + clvar.RunsheetNumber + "', '" + clvar.RiderCode + "', GETDATE(), '" + status.SelectedItem.Text + "', GETDATE()\n" +
                                            ")";
                            Consignment_.Add(query);
                            Consignment_.Add(trackQuery);
                            string query2 = "";
                            if (consignmentOps_.Select("consignmentID = '" + row.Cells[1].Text + "'")[0]["Operation"].ToString() == "insert")
                            {
                                if (status.SelectedItem.Text.ToUpper() == "RETURNED")
                                {
                                    query2 = "insert into mnp_consignmentOperations (\n" +
                                    "consignmentID,\n" +
                                    " operationalType,\n" +
                                    " OriginBranchID,\n" +
                                    " DestBranchID,\n" +
                                    " ConsignmentTypeID,\n" +
                                    " isReturned,\n" +
                                    " CnStatus,\n" +
                                    "isMisrouted,\n" +
                                    "weight,\n" +
                                    "ScreenID,\n" +
                                    "ServiceTypeid,\n" +
                                    "CreatedOn,\n" +
                                    "NoOfPieces,\n" +
                                    "isRunsheetAllowed,\n" +
                                    "isDelivered\n" +
                                    ")\n" +
                                    "VALUES (\n" +
                                    "'" + row.Cells[1].Text + "',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'1',\n" +//isreturned
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0.5',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "GETDATE(),\n" +
                                    "'1',\n" +
                                    "'0',\n" + //isRunsheetAllowed
                                    "'0')"; //isDelivered
                                }
                                else
                                {
                                    query2 = "insert into mnp_consignmentOperations (\n" +
                                   "consignmentID,\n" +
                                   " operationalType,\n" +
                                   " OriginBranchID,\n" +
                                   " DestBranchID,\n" +
                                   " ConsignmentTypeID,\n" +
                                   " isReturned,\n" +
                                   " CnStatus,\n" +
                                   "isMisrouted,\n" +
                                   "weight,\n" +
                                   "ScreenID,\n" +
                                   "ServiceTypeid,\n" +
                                   "CreatedOn,\n" +
                                   "NoOfPieces,\n" +
                                   "isRunsheetAllowed,\n" +
                                   "isDelivered\n" +
                                   ")\n" +
                                   "VALUES (\n" +
                                   "'" + row.Cells[1].Text + "',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0',\n" +//isreturned
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0.5',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "GETDATE(),\n" +
                                   "'1',\n" +
                                   "'1',\n" + //isRunsheetAllowed
                                   "'0')"; //isDelivered
                                }

                            }
                            else
                            {
                                if (status.SelectedItem.Text.ToUpper() == "RETURNED")
                                {
                                    query2 = "UPDATE mnp_consignmentOperations set isReturned = '1', isDelivered = '0', isRunsheetAllowed = '0' where consignmentid = '" + row.Cells[1].Text + "'";
                                }
                                else
                                {
                                    query2 = "UPDATE mnp_consignmentOperations set isReturned = '0', isDelivered = '0', isRunsheetAllowed = '1' where consignmentid = '" + row.Cells[1].Text + "'";
                                }
                            }
                            Consignment_.Add(query2);
                            //Consignment_.Add(query1);

                        }
                    }


                }
            }

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;

            sqlcon.Open();
            SqlCommand dbCommand = new SqlCommand("", sqlcon);

            var sqlStatementArray = dbCommand.CommandText.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            trans = sqlcon.BeginTransaction();

            try
            {
                dbCommand.Transaction = trans;
                foreach (string sqlStatement in Consignment_)
                {
                    dbCommand.CommandText = sqlStatement;
                    dbCommand.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (Exception Ex)
            {
                trans.Rollback();

                return Ex.Message;
            }

            sqlcon.Close();

            return "OK";
        }
        public DataTable GetConsignmentOps(GridView gv, Cl_Variables clvar)
        {
            string numbers = "";
            foreach (GridViewRow row in gv.Rows)
            {
                numbers += "'" + row.Cells[1].Text + "'";
            }
            numbers = numbers.Replace("''", "','");

            string query = "select * from mnp_consignmentOperations c where c.consignmentId in (" + numbers + ")";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            return dt;
        }
        public string UpdatePOD_Consignment_CC(System.Web.UI.WebControls.GridView gv, Cl_Variables clvar)
        {
            ArrayList Consignment_ = new ArrayList();
            string Consignment = "";
            foreach (System.Web.UI.WebControls.GridViewRow row in gv.Rows)
            {
                Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)row.FindControl("txt_gDelvDate");
                System.Web.UI.WebControls.DropDownList status = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gStatus");
                System.Web.UI.WebControls.TextBox receivedBy = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gReceivedBy");
                System.Web.UI.WebControls.TextBox time = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gTime");
                System.Web.UI.WebControls.DropDownList reason = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gReason");
                System.Web.UI.WebControls.TextBox comments = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gComments");
                System.Web.UI.WebControls.Label Lable = (System.Web.UI.WebControls.Label)row.FindControl("Lbl_1");
                System.Web.UI.WebControls.TextBox cnic = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gReceivedByCNIC");
                System.Web.UI.WebControls.DropDownList relations = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gRelation");


                Consignment = "'" + row.Cells[1].Text + "'";
                DateTime dt = Convert.ToDateTime(picker.SelectedDate);

                if (status.SelectedValue == "55" && Lable.Text != "")
                {
                    string query = "Update runsheetConsignment Set time = '" + time.Text + "' ,\n" +
                                    "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
                                    "                          Comments = '" + comments.Text + "' ,\n" +
                                    "                            Status = '" + status.SelectedValue + "' ,\n" +
                                    "                            Reason = '" + reason.SelectedValue + "' , \n" +
                                    "                      DeliveryDate = '" + Lable.Text + "'\n" +
                                    " where ConsignmentNumber = " + Consignment + " and runsheetNumber = '" + clvar.RunSheetNumber + "' and branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
                    string query1 = "Update rvdbo.runsheetConsignment Set DeliveryDateTime = '" + time.Text + "' ,\n" +
                                    "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
                                    "                          Comments = '" + comments.Text + "' ,\n" +
                                    "                          StatusID = '" + status.SelectedValue + "' ,\n" +
                                    "                          ReasonID = '" + reason.SelectedValue + "', \n" +
                                    "                          Receiver_CNIC = '" + cnic.Text + "',\n" +
                                    "                          Relation = '" + relations.SelectedItem.Value + "' \n" +
                                    " where ConsignmentId = " + Consignment + " and RunsheetID = '" + clvar.RunSheetNumber + "' and BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

                    Consignment_.Add(query);
                    Consignment_.Add(query1);

                }
                else
                {
                    string query = "Update runsheetConsignment Set time = '" + time.Text + "' ,\n" +
                                    "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
                                    "                          Comments = '" + comments.Text + "' ,\n" +
                                    "                            Status = '" + status.SelectedValue + "' ,\n" +
                                    "                            Reason = '" + reason.SelectedValue + "' , \n" +
                                    "                      DeliveryDate = '" + dt.ToShortDateString() + "' \n" +
                                    " where ConsignmentNumber = " + Consignment + " and runsheetNumber = '" + clvar.RunSheetNumber + "' and branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
                    string query1 = "Update rvdbo.runsheetConsignment Set DeliveryDateTime = " + time.Text + "' ,\n" +
                                    "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
                                    "                          Comments = '" + comments.Text + "' ,\n" +
                                    "                          StatusID = '" + status.SelectedValue + "' ,\n" +
                                    "                          ReasonID = '" + reason.SelectedValue + "'  \n" +
                                    "                          Receiver_CNIC = '" + cnic.Text + "',\n" +
                                    "                          Relation = '" + relations.SelectedItem.Value + "' \n" +
                                    " where ConsignmentId = " + Consignment + " and RunsheetID = '" + clvar.RunSheetNumber + "' and BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

                    Consignment_.Add(query);
                    Consignment_.Add(query1);

                }

            }

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand dbCommand = new SqlCommand("", sqlcon);

            var sqlStatementArray = dbCommand.CommandText.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string sqlStatement in Consignment_)
            {
                dbCommand.CommandText = sqlStatement;
                dbCommand.ExecuteNonQuery();
            }

            return "OK";
        }
        /*
        public string UpdatePOD(System.Web.UI.WebControls.GridView gv, Cl_Variables clvar)
        {
            CommonFunction func = new CommonFunction();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string branchName = func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString();
            int count = 0;
            string timeCheck = "";
            string receivedByCheck = "";
            string deliveryDateCheck = "";
            string statusCheck = "";
            string statusCheck1 = "";
            string reasonCheck = "";
            string reasonCheck1 = "";
            string commentsCheck = "";
            string consignmentNumbers = "";
            string CheckCNIC = "";

            string trackQuery = "INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, RunsheetNumber, riderName, TransactionTime, Reason, StatusTime)";
            bool flag1 = false;

            foreach (System.Web.UI.WebControls.GridViewRow row in gv.Rows)
            {
                Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)row.FindControl("txt_gDelvDate");
                System.Web.UI.WebControls.DropDownList status = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gStatus");
                System.Web.UI.WebControls.TextBox receivedBy = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gReceivedBy");
                System.Web.UI.WebControls.TextBox time = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gTime");
                System.Web.UI.WebControls.DropDownList reason = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gReason");
                System.Web.UI.WebControls.TextBox comments = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gComments");
                System.Web.UI.WebControls.TextBox cnic = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gReceivedByCNIC");
                consignmentNumbers += "'" + row.Cells[1].Text + "'";


                if (status.SelectedValue != "56")
                {
                    if (picker.SelectedDate == null)
                    {
                        deliveryDateCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL\n";
                        timeCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL\n";
                    }
                    else
                    {
                        deliveryDateCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + "' \n";
                        if (time.Text != "")
                        {
                            timeCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000' \n";
                        }
                        else
                        {
                            timeCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL \n";
                        }


                    }
                }

                if (status.SelectedValue != "0")
                {
                    statusCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + status.SelectedItem.Text + "' \n";
                    statusCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN '" + status.SelectedValue + "' \n";
                }
                else
                {
                    statusCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL \n";
                    statusCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN NULL \n";
                }
                if (reason.SelectedValue != "0")
                {
                    reasonCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + reason.SelectedItem.Text + "' \n";
                    reasonCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN '" + reason.SelectedValue + "' \n";
                }
                else
                {
                    reasonCheck += " WHEN '" + row.Cells[1].Text + "' THEN '' \n";
                    reasonCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN '' \n";
                }

                CheckCNIC += " WHEN '" + row.Cells[1].Text + "' THEN '" + cnic.Text + "'\n ";
                //if (comments.Text.Trim() != "")
                //{
                flag1 = true;
                commentsCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + comments.Text + "' \n";
                //}

                receivedByCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + receivedBy.Text + "' \n";



            }
            consignmentNumbers = consignmentNumbers.Replace("''", "','");
            string conNumbers = "";
            bool track = false;
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)gv.Rows[i].FindControl("txt_gDelvDate");
                System.Web.UI.WebControls.DropDownList status = (System.Web.UI.WebControls.DropDownList)gv.Rows[i].FindControl("dd_gStatus");
                System.Web.UI.WebControls.TextBox receivedBy = (System.Web.UI.WebControls.TextBox)gv.Rows[i].FindControl("txt_gReceivedBy");
                System.Web.UI.WebControls.TextBox time = (System.Web.UI.WebControls.TextBox)gv.Rows[i].FindControl("txt_gTime");
                System.Web.UI.WebControls.DropDownList reason = (System.Web.UI.WebControls.DropDownList)gv.Rows[i].FindControl("dd_gReason");
                System.Web.UI.WebControls.TextBox comments = (System.Web.UI.WebControls.TextBox)gv.Rows[i].FindControl("txt_gComments");
                conNumbers = gv.Rows[i].Cells[1].Text;

                if (status.SelectedValue != "56")
                {
                    track = true;
                    trackQuery += " SELECT '" + conNumbers + "', '10', '" + branchName + "', '" + clvar.RunSheetNumber + "', '" + clvar.RiderCode + "', GETDATE(), '" + reason.SelectedItem.Text + "','" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000'\n" +
                                  " UNION ALL\n";
                }
            }
            //int j = gv.Rows.Count - 1;
            trackQuery = trackQuery.Remove(trackQuery.Length - 12);

            string query = " Update runsheetConsignment Set time = Case ConsignmentNumber " + timeCheck + " end,\n" +
                            "                        receivedBy = Case ConsignmentNumber " + receivedByCheck + " end,\n" +
                            "                          Comments = Case ConsignmentNumber " + commentsCheck + " end,\n" +
                            "                            Status = Case ConsignmentNumber " + statusCheck + " end,\n" +
                            "                            Reason = Case ConsignmentNumber " + reasonCheck + " end, \n" +
                            "                      DeliveryDate = Case ConsignmentNumber " + deliveryDateCheck + " end,\n" +
                            "                     Receiver_CNIC = Case ConsignmentNumber " + CheckCNIC + " end\n" +
                            " where ConsignmentNumber in (" + consignmentNumbers + ") and runsheetNumber = '" + clvar.RunSheetNumber + "' and branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
            string query1 = "Update rvdbo.runsheetConsignment Set DeliveryDateTime = Case ConsignmentId " + timeCheck + " end,\n" +
                            "                        receivedBy = Case ConsignmentId " + receivedByCheck + " end,\n" +
                            "                          Comments = Case ConsignmentId " + commentsCheck + " end,\n" +
                            "                          StatusID = Case ConsignmentId " + statusCheck1 + " end,\n" +
                            "                          ReasonID = Case ConsignmentId " + reasonCheck1 + " end, \n" +
                            "                     Receiver_CNIC = Case ConsignmentId " + CheckCNIC + " end\n" +
                            " where ConsignmentId in (" + consignmentNumbers + ") and RunsheetID = '" + clvar.RunSheetNumber + "' and BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            string query2 = "INSERT INTO CONSIGNMENTSTRACKING ";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;

            try
            {
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                count = 0;
                sqlcmd.CommandText = query1;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                if (track)
                {
                    sqlcmd.CommandText = trackQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;
            }

            return "OK";
        }
        */

        public string UpdatePOD(System.Web.UI.WebControls.GridView gv, Cl_Variables clvar)
        {
            CommonFunction func = new CommonFunction();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string branchName = func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString();
            int count = 0;
            string timeCheck = "";
            string receivedByCheck = "";
            string deliveryDateCheck = "";
            string statusCheck = "";
            string statusCheck1 = "";
            string reasonCheck = "";
            string reasonCheck1 = "";
            string commentsCheck = "";
            string consignmentNumbers = "";
            string CheckCNIC = "";
            string checkRelation = "";
            string trackQuery = "INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, RunsheetNumber, riderName, TransactionTime, Reason, StatusTime)";
            bool flag1 = false;
            bool cnicFlag = false;
            foreach (System.Web.UI.WebControls.GridViewRow row in gv.Rows)
            {
                Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)row.FindControl("txt_gDelvDate");
                System.Web.UI.WebControls.DropDownList status = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gStatus");
                System.Web.UI.WebControls.TextBox receivedBy = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gReceivedBy");
                System.Web.UI.WebControls.TextBox time = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gTime");
                System.Web.UI.WebControls.DropDownList reason = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gReason");
                System.Web.UI.WebControls.TextBox comments = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gComments");
                System.Web.UI.WebControls.TextBox cnic = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gReceivedByCNIC");
                System.Web.UI.WebControls.DropDownList relations = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gRelation");
                consignmentNumbers += "'" + row.Cells[1].Text + "'";


                if (status.SelectedValue != "56")
                {
                    if (picker.SelectedDate == null)
                    {
                        deliveryDateCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL\n";
                        timeCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL\n";
                    }
                    else
                    {
                        deliveryDateCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + "' \n";
                        if (time.Text != "")
                        {
                            timeCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000' \n";
                        }
                        else
                        {
                            timeCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL \n";
                        }


                    }
                }

                if (status.SelectedValue != "0")
                {
                    statusCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + status.SelectedItem.Text + "' \n";
                    statusCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN '" + status.SelectedValue + "' \n";
                }
                else
                {
                    statusCheck += " WHEN '" + row.Cells[1].Text + "' THEN NULL \n";
                    statusCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN NULL \n";
                }
                if (reason.SelectedValue != "0")
                {
                    reasonCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + reason.SelectedItem.Text + "' \n";
                    reasonCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN '" + reason.SelectedValue + "' \n";
                }
                else
                {
                    reasonCheck += " WHEN '" + row.Cells[1].Text + "' THEN '' \n";
                    reasonCheck1 += " WHEN '" + row.Cells[1].Text + "' THEN '' \n";
                }


                if (cnic.Text != "")
                {
                    CheckCNIC += " WHEN '" + row.Cells[1].Text + "' THEN '" + cnic.Text + "'\n ";
                    checkRelation += " WHEN '" + row.Cells[1].Text + "' THEN '" + relations.SelectedValue + "'";
                    cnicFlag = true;
                }
                //if (comments.Text.Trim() != "")
                //{
                flag1 = true;
                commentsCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + comments.Text + "' \n";
                //}

                receivedByCheck += " WHEN '" + row.Cells[1].Text + "' THEN '" + receivedBy.Text + "' \n";



            }
            consignmentNumbers = consignmentNumbers.Replace("''", "','");
            string conNumbers = "";
            bool track = false;
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)gv.Rows[i].FindControl("txt_gDelvDate");
                System.Web.UI.WebControls.DropDownList status = (System.Web.UI.WebControls.DropDownList)gv.Rows[i].FindControl("dd_gStatus");
                System.Web.UI.WebControls.TextBox receivedBy = (System.Web.UI.WebControls.TextBox)gv.Rows[i].FindControl("txt_gReceivedBy");
                System.Web.UI.WebControls.TextBox time = (System.Web.UI.WebControls.TextBox)gv.Rows[i].FindControl("txt_gTime");
                System.Web.UI.WebControls.DropDownList reason = (System.Web.UI.WebControls.DropDownList)gv.Rows[i].FindControl("dd_gReason");
                System.Web.UI.WebControls.TextBox comments = (System.Web.UI.WebControls.TextBox)gv.Rows[i].FindControl("txt_gComments");
                conNumbers = gv.Rows[i].Cells[1].Text;

                if (status.SelectedValue != "56")
                {
                    track = true;
                    trackQuery += " SELECT '" + conNumbers + "', '10', '" + branchName + "', '" + clvar.RunSheetNumber + "', '" + clvar.RiderCode + "', GETDATE(), '" + reason.SelectedItem.Text + "','" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000'\n" +
                                  " UNION ALL\n";
                }
            }
            //int j = gv.Rows.Count - 1;
            trackQuery = trackQuery.Remove(trackQuery.Length - 12);

            string query = " Update runsheetConsignment Set time = Case ConsignmentNumber " + timeCheck + " end,\n" +
                            "                        receivedBy = Case ConsignmentNumber " + receivedByCheck + " end,\n" +
                            "                          Comments = Case ConsignmentNumber " + commentsCheck + " end,\n" +
                            "                            Status = Case ConsignmentNumber " + statusCheck + " end,\n" +
                            "                            Reason = Case ConsignmentNumber " + reasonCheck + " end, \n" +
                            "                      DeliveryDate = Case ConsignmentNumber " + deliveryDateCheck + " end\n";
            if (cnicFlag)
            {
                query += " ,                    Receiver_CNIC = Case ConsignmentNumber " + CheckCNIC + " end,\n" +
                            "                          Relation = Case ConsignmentNumber " + checkRelation + " end\n";
            }
            query += " where ConsignmentNumber in (" + consignmentNumbers + ") and runsheetNumber = '" + clvar.RunSheetNumber + "' and branchcode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
            string query1 = "Update rvdbo.runsheetConsignment Set DeliveryDateTime = Case ConsignmentId " + timeCheck + " end,\n" +
                            "                        receivedBy = Case ConsignmentId " + receivedByCheck + " end,\n" +
                            "                          Comments = Case ConsignmentId " + commentsCheck + " end,\n" +
                            "                          StatusID = Case ConsignmentId " + statusCheck1 + " end,\n" +
                            "                          ReasonID = Case ConsignmentId " + reasonCheck1 + " end \n";
            if (cnicFlag)
            {
                query1 += ",                    Receiver_CNIC = Case ConsignmentId " + CheckCNIC + " end,\n" +
                            "                          Relation = Case ConsignmentId " + checkRelation + " end\n";
            }
            query1 += " where ConsignmentId in (" + consignmentNumbers + ") and RunsheetID = '" + clvar.RunSheetNumber + "' and BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            string query2 = "INSERT INTO CONSIGNMENTSTRACKING ";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;

            try
            {
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                count = 0;
                sqlcmd.CommandText = query1;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                if (track)
                {
                    sqlcmd.CommandText = trackQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;
            }

            return "OK";
        }

        public DataTable GetRunsheetDetail_(Cl_Variables clvar)
        {

            string sqlString = "select r.routeCode,\n" +
            "       rt.name,\n" +
            "       rr.riderCode,\n" +
            "       rrr.firstName + ' ' + rrr.lastName RiderName,\n" +
            "       r.runsheetDate\n" +
            "  from Runsheet r\n" +
            " inner join RiderRunsheet rr\n" +
            "    on rr.runsheetNumber = r.runsheetNumber\n" +
            " inner join Riders rrr\n" +
            "    on rrr.riderCode = rr.riderCode\n" +
            "   and r.branchCode = rrr.branchId\n" +
            " inner join Routes rt\n" +
            "    on rt.routeCode = r.routeCode\n" +
            " where r.runsheetNumber = '" + clvar.RunSheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        #region Commented by Rabi on 05/08/2016 07:45 PM
        //public DataTable GetRunsheetDetail(Cl_Variables clvar)
        //{

        //    string sqlString = "select r.routeCode,\n" +
        //    "       rt.name,\n" +
        //    "       rr.riderCode,\n" +
        //    "       rrr.firstName + ' ' + rrr.lastName RiderName,\n" +
        //    "       r.runsheetDate\n" +
        //    "  from Runsheet r\n" +
        //    " inner join RiderRunsheet rr\n" +
        //    "    on rr.runsheetNumber = r.runsheetNumber\n" +
        //    " inner join Riders rrr\n" +
        //    "    on rrr.riderCode = rr.riderCode\n" +
        //    "   and r.branchCode = rrr.branchId\n" +
        //    " inner join Routes rt\n" +
        //    "    on rt.routeCode = r.routeCode\n" +
        //    "    INNER JOIN Branches b        \n" +
        //    "    on b.branchCode = r.branchCode  \n" +
        //    "     AND b.cityId = rt.cityId     \n" +
        //    " where r.runsheetNumber = '" + clvar.RunSheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlConnection orcl = new SqlConnection(clvar.Strcon());
        //        orcl.Open();
        //        SqlCommand orcd = new SqlCommand(sqlString, orcl);
        //        orcd.CommandType = CommandType.Text;
        //        orcd.CommandTimeout = 300;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(dt);
        //        orcl.Close();
        //    }
        //    catch (Exception)
        //    { }

        //    return dt;
        //} 
        #endregion
        #region COMMENTED By Rabi on 06/08/2016 02:09 PM
        //public DataTable GetRunsheetDetail(Cl_Variables clvar)
        //{

        //    string sqlString = "select r.routeCode,\n" +
        //    "       rt.name,\n" +
        //    "       rr.riderCode,\n" +
        //    "       rrr.firstName + ' ' + rrr.lastName RiderName,\n" +
        //    "       r.runsheetDate\n" +
        //    "  from Runsheet r\n" +
        //    " inner join RiderRunsheet rr\n" +
        //    "    on rr.runsheetNumber = r.runsheetNumber\n" +
        //    " inner join Riders rrr\n" +
        //    "    on rrr.riderCode = rr.riderCode\n" +
        //    "   and r.branchCode = rrr.branchId\n" +
        //    " inner join Routes rt\n" +
        //    "    on rt.routeCode = r.routeCode\n" +
        //    " where r.runsheetNumber = '" + clvar.RunSheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and r.routeCode = '" + clvar.riderCode + "'";

        //    sqlString = "select r.routeCode,\n" +
        //   "       rt.name,\n" +
        //   "       rr.riderCode,\n" +
        //   "       rrr.firstName + ' ' + rrr.lastName RiderName,\n" +
        //   "       r.runsheetDate\n" +
        //   "  from Runsheet r\n" +
        //   " inner join RiderRunsheet rr\n" +
        //   "    on rr.runsheetNumber = r.runsheetNumber\n" +
        //   " inner join Riders rrr\n" +
        //   "    on rrr.riderCode = rr.riderCode\n" +
        //   "   and r.branchCode = rrr.branchId\n" +
        //   " inner join Routes rt\n" +
        //   "    on rt.routeCode = r.routeCode\n" +
        //   "   and rrr.routeCode = rt.routeCode\n" +
        //   " where r.runsheetNumber = '" + clvar.RunSheetNumber + "'\n" +
        //   "   and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
        //   "   and rrr.riderCode = '" + clvar.riderCode + "'";

        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlConnection orcl = new SqlConnection(clvar.Strcon());
        //        orcl.Open();
        //        SqlCommand orcd = new SqlCommand(sqlString, orcl);
        //        orcd.CommandType = CommandType.Text;
        //        orcd.CommandTimeout = 300;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(dt);
        //        orcl.Close();
        //    }
        //    catch (Exception)
        //    { }

        //    return dt;
        //} 
        #endregion

        public DataTable GetRunsheetDetail(Cl_Variables clvar)
        {

            string sqlString = "select r.routeCode,\n" +
            "       rt.name,\n" +
            "       rr.riderCode,\n" +
            "       rrr.firstName + ' ' + rrr.lastName RiderName,\n" +
            "       r.runsheetDate\n" +
            "  from Runsheet r\n" +
            " inner join RiderRunsheet rr\n" +
            "    on rr.runsheetNumber = r.runsheetNumber\n" +
            " inner join Riders rrr\n" +
            "    on rrr.riderCode = rr.riderCode\n" +
            "   and r.branchCode = rrr.branchId\n" +
            " inner join Routes rt\n" +
            "    on rt.routeCode = r.routeCode\n" +
            " where r.runsheetNumber = '" + clvar.RunSheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and r.routeCode = '" + clvar.riderCode + "'";

            sqlString = "select r.routeCode,\n" +
           "       rt.name,\n" +
           "       rr.riderCode,\n" +
           "       rrr.firstName + ' ' + rrr.lastName RiderName,\n" +
           "       r.runsheetDate\n" +
           "  from Runsheet r\n" +
           " inner join RiderRunsheet rr\n" +
           "    on rr.runsheetNumber = r.runsheetNumber\n" +
           " inner join Riders rrr\n" +
           "    on rrr.riderCode = rr.riderCode\n" +
           "   and r.branchCode = rrr.branchId\n" +
           " inner join Routes rt\n" +
           "    on rt.routeCode = r.routeCode\n" +
           "   and rrr.routeCode = rt.routeCode\n" +
           " where r.runsheetNumber = '" + clvar.RunSheetNumber + "'\n" +
           "   and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "   and rrr.riderCode = '" + clvar.riderCode + "'";



            sqlString = "select r.routeCode,\n" +
           "       rt.name,\n" +
           "       rr.riderCode,\n" +
           "       rrr.firstName + ' ' + rrr.lastName RiderName,\n" +
           "       r.runsheetDate\n" +
           "  from Runsheet r\n" +
           " inner join RiderRunsheet rr\n" +
           "    on rr.runsheetNumber = r.runsheetNumber\n" +
           " inner join Riders rrr\n" +
           "    on rrr.riderCode = rr.riderCode\n" +
           "   and r.branchCode = rrr.branchId\n" +
           " inner join Routes rt\n" +
           "    on rt.routeCode = r.routeCode\n" +
           "   and rrr.routeCode = rt.routeCode\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = r.branchCode\n" +
           "   and rt.cityId = b.cityId\n" +
            " where r.runsheetNumber = '" + clvar.RunSheetNumber + "'\n" +
          "   and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
          "   and rrr.riderCode = '" + clvar.riderCode + "'";


            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        /* ========================== */

        public DataTable GetCardConsignmentByRefNumber(Cl_Variables clvar)
        {

            string sqlString = "select b.name OrgName,\n" +
            "cc.* from CardConsignment cc\n" +
            "inner join Branches b\n" +
            "on b.branchCode = cc.orgin\n" +
            "where cc.refrenceNo like '" + clvar.RefNumber + "' AND cc.isCNGenerated = '0' ";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetCreditClientByAccountNo(Cl_Variables clvar)
        {

            string sqlString = "select * from CreditClients c where c.accountNo = '" + clvar.AccountNo + "' ";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetRiderExpressCenterCode(Cl_Variables clvar)
        {

            string sqlString = "select expressCenterId from riders c where c.ridercode = '" + clvar.RiderCode + "' ";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        // public string Insert_CardConsignment(Cl_Variables clvar, DataTable dt)
        public int Insert_CardConsignment(Cl_Variables clvar)
        {
            string error = "";
            int count = 0;

            string query = "insert into Consignment \n" +
                                "  (consignmentNumber, consigner, consignee, customerType, orgin, destination, pieces, serviceTypeName, creditClientId, \n" +
                                "   weight, weightUnit, cod, address, createdBy, createdOn, zoneCode, branchCode ,riderCode, consignerAccountNo, \n" +
                                "   bookingDate, expressCenterCode, consignmentTypeId, isApproved, syncId, couponnumber, accountReceivingDate, destinationExpressCenterCode)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar.consignmentNo + "',\n" +
                                "   '" + clvar.Consigner + "',\n" +
                                "   '" + clvar.Consignee + "',\n" +
                                "   '2',\n" +
                                //   "   '" + clvar.origin + "',\n" +
                                "   '" + HttpContext.Current.Session["branchcode"].ToString() + "', \n" +
                                "   '" + clvar.Destination + "',\n" +
                                "   '1',\n" +
                                "   '" + clvar.ServiceType + "',\n" +
                                "   '" + clvar.CreditClientID + "',\n" +
                                "   '" + clvar.ToWeight + "',\n" +
                                "   '1',\n" +
                                "   '" + clvar.Insurance + "',\n" +
                                "   '" + clvar.shipperAddress + "',\n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                "   GETDATE(), \n" +
                                "   '" + HttpContext.Current.Session["ZoneCode"].ToString() + "',\n" +
                                "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                                "   '" + clvar.RiderCode + "',\n" +
                                "   '" + clvar.AccountNo + "',\n" +
                                "   '" + clvar.Day + "', \n" +
                                "   '" + clvar.expresscenter + "',\n" +
                                "   '12',\n" +
                                "   '1', NEWID(), \n" +
                                "   '" + clvar.RefNo + "', \n" +
                                "   GETDATE(),\n" +
                                "   '" + clvar.destinationExpressCenterCode + "'\n" +
                                " ) ";

            string query2 = "insert into ConsignmentsTrackingHistory (consignmentNumber, stateID, currentLocation, transactionTime )\n" +
                                " Values (\n" +
                                "           '" + clvar.consignmentNo + "',\n" +
                                "           '1',\n" +
                                "           '" + clvar.destinationCity + "',\n" +
                                "           '" + clvar.Day + "' \n" +
                                "        )";

            string query3 = "insert into ConsignmentsTrackingHistory (consignmentNumber, stateID, currentLocation, manifestNumber, transactionTime )\n" +
                                " Values (\n" +
                                "           '" + clvar.consignmentNo + "',\n" +
                                "           '2',\n" +
                                "           '" + clvar.destinationCity + "',\n" +
                                "           '" + clvar.manifestNo + "',\n" +
                                "           '" + clvar.Day + "' \n" +
                                "        )";


            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                }
                count = 0;
                sqlcmd.CommandText = query2;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                }
                count = 0;
                sqlcmd.CommandText = query3;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                }



                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                count = 0;
            }
            finally
            {
                sqlcon.Close();
            }
            return count;
        }

        public int Insert_CardManifest(Cl_Variables clvar, DataTable dt)
        {
            string error = "";
            int count = 0;

            string query1 = "insert into mnp_Manifest (manifestNumber, origin, destination, branchCode, zoneCode, manifestType, date, createdBy, createdOn, isTemp)\n" +
                           " VALUES ( \n" +
                           "'" + clvar.manifestNo + "',\n" +
                           "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "', \n" +
                           "'" + clvar.Destination + "',\n" +
                           "'" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                           "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
                           "'" + clvar.ServiceType + "',\n" +
                           "'" + clvar.Day + "',\n" +
                           "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                           " GETDATE() ,\n" +
                           "'0'\n" +
                           ")";

            //string query2 = "Insert into rvdbo.manifest (ManifestID, ManifestNo , ServiceTypeID, ManifestDate, ZoneID, OriginBranchID, DestBranchID,  isTemp,  isDemanifested) " +
            //                "Values " +
            //                "(" +
            //                "'" + clvar.manifestNo.TrimStart('0') + "',\n" +
            //                "'" + clvar.manifestNo + "',\n" +
            //                "'" + clvar.serviceTypeId + "',\n" +
            //                "'" + clvar.Day + "',\n" +
            //                "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
            //                "'" + clvar.origin + "',\n" +
            //                "'" + clvar.Destination + "', '0', '0')";

            //   string query1_ = "Insert into rvdbo.Manifestconsignment (ManifestId, ConsignmentId) ";
            string query3 = "INSERT INTO mnp_ConsignmentManifest (manifestNumber,consignmentNumber ) \n";
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                query3 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
                //      query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "','" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
            }
            int j = dt.Rows.Count - 1;
            query3 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[j][0].ToString() + "'";
            //    query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "', '" + dt.Rows[j][0].ToString() + "'";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query1;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                }
                //count = 0;
                //sqlcmd.CommandText = query2;
                //count = sqlcmd.ExecuteNonQuery();
                //if (count == 0)
                //{
                //    trans.Rollback();
                //}

                //count = 0;
                //sqlcmd.CommandText = query1_;
                //count = sqlcmd.ExecuteNonQuery();
                //if (count == 0)
                //{
                //    trans.Rollback();
                //}
                count = 0;
                sqlcmd.CommandText = query3;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                count = 0;
            }
            finally
            {
                sqlcon.Close();
            }
            return count;
        }

        public DataTable Get_CardCreditClients(Cl_Variables clvar)
        {

            string sqlString = "select * from CreditClients where accountNo = '" + clvar.AccountNo + "' AND \n" +
                                " branchCode = '" + HttpContext.Current.Session["branchCode"].ToString() + "' AND isActive = '1' ";

            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable Get_CardManifest(Cl_Variables clvar)
        {
            string sqlString = "select * from mnp_Manifest where manifestNumber = '" + clvar.manifestNo + "'  ";

            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable Get_CardConsignment(Cl_Variables clvar)
        {
            string sqlString = "select * from Consignment where ConsignmentNumber = '" + clvar.consignmentNo + "'  ";

            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public int CardConsignmentApprovalStatus(Cl_Variables clvar)
        {
            string error = "";
            int count = 0;

            string query = "update Consignment set chargedAmount = '" + clvar.ChargeAmount + "' and isApproved = '1' where consignmentNumber = '" + clvar.consignmentNo + "'  ";


            SqlConnection sqlcon = new SqlConnection(clvar.StrconLive());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                count = 0;
            }
            finally
            {
                sqlcon.Close();
            }
            return count;
        }


        /* ========================== */

        public DataTable ExistingConsignmentInRunsheetSameDay(Cl_Variables clvar)
        {
            string CNs = "";
            foreach (string str in clvar.ClvarListStr)
            {
                CNs += "'" + str + "'";
            }

            CNs = CNs.Replace("''", "','");

            string query = "select * from RunsheetConsignment rc where rc.consignmentNumber in (" + CNs + ") and Convert(date,rc.createdOn,105) like  '" + DateTime.Parse(clvar.RunSheetDate).ToString("yyyy-MM-dd") + "'";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.StrconLive());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;

        }

        public string ApproveConsignments(Cl_Variables clvar)
        {
            string query = "UPDATE CONSIGNMENT SET ISAPPROVED = '" + clvar.status.ToString() + "'  where consignmentNumber = '" + clvar.consignmentNo + "'";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            if (clvar.status == 1)
            {
                return "Approved";
            }
            else
            {
                return "UnApproved";
            }
            // return "OK";
        }

        /*
        public string ApproveDomesticConsignment(Cl_Variables clvar, DataTable pm)
        {

            string query = "  update Consignment set\n" +
                           "            creditClientId = '" + clvar.CustomerClientID + "',\n" +
                           "        consignerAccountNo = '" + clvar.consignerAccountNo + "',\n" +
                           "               destination = '" + clvar.destination + "',\n" +
                           "           serviceTypeName = '" + clvar.ServiceTypeName + "',\n" +
                           "                 consigner = '" + clvar.Consigner + "',\n" +
                           "                 consignee = '" + clvar.Consignee + "',\n" +
                           "                    weight = '" + clvar.Weight.ToString() + "',\n" +
                           "      accountReceivingDate = '" + DateTime.Parse(clvar.LoadingDate).ToString("yyyy-MM-dd") + "', \n    " +
                           "                 riderCode = '" + clvar.riderCode + "',\n";
            if (clvar.OriginExpressCenterCode.ToString() != "")
            {
                query += "       originExpressCenter = '" + clvar.OriginExpressCenterCode + "',\n";
            }
            query += "            consignmentTypeId = '" + clvar.cnTypeId + "',\n" +
                     "                chargedAmount = '" + clvar.TotalAmount.ToString() + "',\n" +
                     "                   isApproved = '" + clvar.status + "',\n" +
                     "                  totalAmount = '" + clvar.ChargeAmount + "',\n" +
                     "                          Gst = '" + clvar.gst + "',\n" +
                     "              ispriceComputed = '1',\n" +
                     "                 CustomerType = '" + clvar.Customertype + "',\n" +
                     "                   modifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                     "                   ModifiedON = GETDATE(),\n" +
                     "            ExpressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
                     " destinationExpressCenterCode = '" + clvar.destinationExpressCenterCode + "'" +
                    // "                --        iscod = '" + clvar.isCod + "'" +
                     "   where consignmentNumber = '" + clvar.consignmentNo + "'";
            string newPMQuery = "";
            string updatePMQuery = "";
            string removePMQuery = "";
            if (pm.Rows.Count > 0)
            {
                DataTable newPM = new DataTable();
                try
                {
                    pm.Select("NEW = '1'").CopyToDataTable();
                }
                catch (Exception ex)
                { }

                DataTable updatePM = new DataTable();
                try
                {
                    updatePM = pm.Select("NEW = ''").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                DataTable delPM = new DataTable();
                try
                {
                    delPM = pm.Select("NEW = 'REMOVED'").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                if (newPM.Rows.Count > 0)
                {
                    string temp = "CASE pm.ID ";
                    string temp1 = "CASE pm.ID ";
                    string temp2 = "CASE PM.ID ";
                    string temp4 = "CASE PM.ID ";
                    string temp3 = "";

                    foreach (DataRow dr in newPM.Rows)
                    {
                        temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                        temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                        temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                        temp4 += " WHEN '" + dr[0].ToString() + "' then '" + dr["calculationBase"].ToString() + "'\n";
                        temp3 += "'" + dr[0].ToString() + "',";
                    }
                    temp += "END CalculatedValue,";
                    temp1 += "END CalculatedGST, ";
                    temp2 += "END SORTORDER ";
                    temp4 += "END CALCULATIONBASE,";
                    temp3 = temp3.TrimEnd(',');
                    newPMQuery = "INSERT INTO CONSIGNMENTMODIFIER (priceModifierID, ConsignmentNumber, createdBy, CreatedON, modifiedCalculationValue, calculatedValue, CalculatedGSt, CalculationBase, SortOrder)\n" +
                                 " select pm.id, '" + clvar.consignmentNo + "' ConsignmentNumber, '" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy, GETDATE() CreatedON, pm.calculationValue, " + temp + temp1 + temp4 + temp2 + " from PriceModifiers pm WHERE pm.ID in (" + temp3 + ")";
                }
                if (updatePM.Rows.Count > 0)
                {
                    string temp = "CASE priceModifierID ";
                    string temp1 = "CASE priceModifierID ";
                    string temp2 = "CASE priceModifierID ";
                    string temp3 = "";
                    foreach (DataRow dr in updatePM.Rows)
                    {
                        temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                        temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                        temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                        temp3 += "'" + dr[0].ToString() + "',";
                    }
                    temp += "END ";
                    temp1 += "END  ";
                    temp2 += "END ";
                    temp3 = temp3.TrimEnd(',');
                    updatePMQuery = "UPDATE CONSIGNMENTMODIFIER SET CalculatedValue = " + temp + ", CalculatedGST = " + temp1 + " \n" +
                                 "  WHERE priceModifierID in (" + temp3 + ")";

                }
                if (delPM.Rows.Count > 0)
                {
                    removePMQuery += "DELETE FROM CONSIGNMENTMODIFIER where pricemodifierID in (";
                    foreach (DataRow dr in delPM.Rows)
                    {
                        removePMQuery += "'" + dr[0].ToString() + "',";
                    }
                    removePMQuery = removePMQuery.TrimEnd(',') + ") AND ConsignmentNumber = '" + clvar.consignmentNo + "'";
                }
            }
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            int count = 0;
            try
            {
                sqlcmd.CommandText = query;
                sqlcmd.ExecuteNonQuery();

                if (newPMQuery != "")
                {
                    sqlcmd.CommandText = newPMQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                if (updatePMQuery != "")
                {
                    sqlcmd.CommandText = updatePMQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                if (removePMQuery != "")
                {
                    sqlcmd.CommandText = removePMQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;
            }
            return "OK";
        }
         */

        public string ApproveDomesticConsignment(Cl_Variables clvar, DataTable pm, bool cod)
        {
            string auditQuery = "insert into MNP_consignmentunapproval (ConsignmentNumber, TransactionTime, status, USERID) VALUES (\n" +
                "'" + clvar.consignmentNo + "', GETDATE(), '1', '" + HttpContext.Current.Session["U_ID"].ToString() + "')";

            string query = "  update Consignment set\n" +
                           "            creditClientId = '" + clvar.CustomerClientID + "',\n" +
                           "        consignerAccountNo = '" + clvar.consignerAccountNo + "',\n" +
                           "               destination = '" + clvar.destination + "',\n" +
                           "           serviceTypeName = '" + clvar.ServiceTypeName + "',\n" +
                           "                 consigner = '" + clvar.Consigner + "',\n" +
                           "                 consignee = '" + clvar.Consignee + "',\n" +
                           "                    weight = '" + clvar.Weight.ToString() + "',\n" +
                           "      accountReceivingDate = '" + DateTime.Parse(clvar.LoadingDate).ToString("yyyy-MM-dd") + "', \n    " +
                           "                 riderCode = '" + clvar.riderCode + "',\n";
            if (clvar.OriginExpressCenterCode.ToString() != "")
            {
                query += "       originExpressCenter = '" + clvar.OriginExpressCenterCode + "',\n";
            }
            query += "            consignmentTypeId = '" + clvar.cnTypeId + "',\n" +
                     "                chargedAmount = '" + clvar.TotalAmount.ToString() + "',\n" +
                     "                   isApproved = '" + clvar.status + "',\n" +
                     "                  totalAmount = '" + clvar.ChargeAmount + "',\n" +
                     "                          Gst = '" + clvar.gst + "',\n" +
                     "              ispriceComputed = '1',\n" +
                     "                 CustomerType = '" + clvar.Customertype + "',\n" +
                     "                   modifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                     "                   ModifiedON = GETDATE(),\n" +
                     //"                   pieces = '" + clvar.pieces.ToString() + "',\n" +
                     "            ExpressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
                     "            destinationExpressCenterCode = '" + clvar.destinationExpressCenterCode + "',\n";
            //"            Address = '" + clvar.ConsigneeAddress + "',\n";

            if (clvar.isCod == false)
            {
                query += "                        cod = '0',\n";
            }

            if (clvar.isCod == true)
            {
                query += "                        cod = '1',\n";
            }
            query += " Bookingdate =CONVERT(datetime,'" + clvar.Bookingdate + "',105), ";
            query += " orgin = '" + clvar.origin + "', ispayable = '0', isinvoiced = '0'\n";
            query += "    where consignmentNumber = '" + clvar.consignmentNo + "'";
            string newPMQuery = "";
            string updatePMQuery = "";
            string removePMQuery = "";
            if (pm.Rows.Count > 0)
            {
                DataTable newPM = new DataTable();
                try
                {
                    pm.Select("NEW = '1'").CopyToDataTable();
                }
                catch (Exception ex)
                { }

                DataTable updatePM = new DataTable();
                try
                {
                    updatePM = pm.Select("NEW = ''").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                DataTable delPM = new DataTable();
                try
                {
                    delPM = pm.Select("NEW = 'REMOVED'").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                if (newPM.Rows.Count > 0)
                {
                    string temp = "CASE pm.ID ";
                    string temp1 = "CASE pm.ID ";
                    string temp2 = "CASE PM.ID ";
                    string temp4 = "CASE PM.ID ";
                    string temp3 = "";

                    foreach (DataRow dr in newPM.Rows)
                    {
                        temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                        temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                        temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                        temp4 += " WHEN '" + dr[0].ToString() + "' then '" + dr["calculationBase"].ToString() + "'\n";
                        temp3 += "'" + dr[0].ToString() + "',";
                    }
                    temp += "END CalculatedValue,";
                    temp1 += "END CalculatedGST, ";
                    temp2 += "END SORTORDER ";
                    temp4 += "END CALCULATIONBASE,";
                    temp3 = temp3.TrimEnd(',');
                    newPMQuery = "INSERT INTO CONSIGNMENTMODIFIER (priceModifierID, ConsignmentNumber, createdBy, CreatedON, modifiedCalculationValue, calculatedValue, CalculatedGSt, CalculationBase, SortOrder)\n" +
                                 " select pm.id, '" + clvar.consignmentNo + "' ConsignmentNumber, '" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy, GETDATE() CreatedON, pm.calculationValue, " + temp + temp1 + temp4 + temp2 + " from PriceModifiers pm WHERE pm.ID in (" + temp3 + ")";
                }
                if (updatePM.Rows.Count > 0)
                {
                    string temp = "CASE priceModifierID ";
                    string temp1 = "CASE priceModifierID ";
                    string temp2 = "CASE priceModifierID ";
                    string temp3 = "";
                    foreach (DataRow dr in updatePM.Rows)
                    {
                        temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                        temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                        temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                        temp3 += "'" + dr[0].ToString() + "',";
                    }
                    temp += "END ";
                    temp1 += "END  ";
                    temp2 += "END ";
                    temp3 = temp3.TrimEnd(',');
                    updatePMQuery = "UPDATE CONSIGNMENTMODIFIER SET CalculatedValue = " + temp + ", CalculatedGST = " + temp1 + " \n" +
                                 "  WHERE priceModifierID in (" + temp3 + ")";

                }
                if (delPM.Rows.Count > 0)
                {
                    removePMQuery += "DELETE FROM CONSIGNMENTMODIFIER where pricemodifierID in (";
                    foreach (DataRow dr in delPM.Rows)
                    {
                        removePMQuery += "'" + dr[0].ToString() + "',";
                    }
                    removePMQuery = removePMQuery.TrimEnd(',') + ") AND ConsignmentNumber = '" + clvar.consignmentNo + "'";
                }
            }


            string codQuery = "";
            if (cod & clvar.CatID != "1")
            {
                CommonFunction cf = new CommonFunction();
                DataTable dt_ = cf.GetCODConsignmentForApproval(clvar);
                if (dt_.Rows.Count == 0)
                {
                    codQuery = "Insert into CODConsignmentDetail (ConsignmentNumber, OrderRefNo, ProductTypeId, ProductDescription, ChargeCodAmount, CodAmount, CalculatedAmount ) Values \n" +
                        "(\n" +
                        "'" + clvar.consignmentNo.Trim() + "',\n" +
                        "'" + clvar.orderRefNo + "',\n" +
                        "'" + clvar.productTypeId + "',\n" +
                        "'" + clvar.productDescription + "',\n" +
                        "'" + clvar.chargeCODAmount + "',\n" +
                        "'" + clvar.codAmount + "',\n" +
                        "'" + clvar.calculatedCodAmount + "'\n" +
                        ")";
                }
                else
                {
                    codQuery = "UPDATE CODConsignmentDetail   \n" +
                               " SET    orderRefNo             = '" + clvar.orderRefNo + "', \n" +
                               "        productDescription     = '" + clvar.productDescription + "', \n" +
                               "        codAmount              = " + clvar.codAmount + ", \n" +
                               "        calculatedAmount       = " + clvar.calculatedCodAmount + " \n" +
                               " WHERE  consignmentNumber      = '" + clvar.consignmentNo.Trim() + "' ";
                }
            }


            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            int count = 0;
            try
            {
                sqlcmd.CommandText = query;
                sqlcmd.ExecuteNonQuery();

                if (newPMQuery != "")
                {
                    sqlcmd.CommandText = newPMQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                if (updatePMQuery != "")
                {
                    sqlcmd.CommandText = updatePMQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                if (removePMQuery != "")
                {
                    sqlcmd.CommandText = removePMQuery;
                    sqlcmd.ExecuteNonQuery();
                }


                if (codQuery != "")
                {
                    sqlcmd.CommandText = codQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                sqlcmd.CommandText = auditQuery;
                sqlcmd.ExecuteNonQuery();

                trans.Commit();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;
            }
            return "OK";
        }
        public string ApproveDomesticConsignment_new(Cl_Variables clvar, DataTable pm, bool cod)
        {
            string auditQuery = "insert into MNP_consignmentunapproval (ConsignmentNumber, TransactionTime, status, USERID) VALUES (\n" +
      "'" + clvar.consignmentNo + "', GETDATE(), '1', '" + HttpContext.Current.Session["U_ID"].ToString() + "')";

            string query = "  update Consignment set\n" +
                           "            creditClientId = '" + clvar.CustomerClientID + "',\n" +
                           "        consignerAccountNo = '" + clvar.consignerAccountNo + "',\n" +
                           "               destination = '" + clvar.destination + "',\n" +
                           "           serviceTypeName = '" + clvar.ServiceTypeName + "',\n" +
                           "                 consigner = '" + clvar.Consigner + "',\n" +
                           "                 consignee = '" + clvar.Consignee + "',\n" +
                           "                    weight = '" + clvar.Weight.ToString() + "',\n" +
                           "      accountReceivingDate = '" + DateTime.Parse(clvar.LoadingDate).ToString("yyyy-MM-dd") + "', \n    " +
                           "                 riderCode = '" + clvar.riderCode + "',\n";
            if (clvar.OriginExpressCenterCode.ToString() != "")
            {
                query += "       originExpressCenter = '" + clvar.OriginExpressCenterCode + "',\n";
            }
            query += "            consignmentTypeId = '" + clvar.cnTypeId + "',\n" +
                     "                chargedAmount = '" + clvar.TotalAmount.ToString() + "',\n" +
                     "                   isApproved = '" + clvar.status + "',\n" +
                     "                  totalAmount = '" + clvar.ChargeAmount + "',\n" +
                     "                          Gst = '" + clvar.gst + "',\n" +
                     "              ispriceComputed = '0',\n" +
                     "                 CustomerType = '" + clvar.Customertype + "',\n" +
                     "                   modifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                     "                   ModifiedON = GETDATE(),\n" +
                     "                   pieces = '" + clvar.pieces.ToString() + "',\n" +
                     "            ExpressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
                     "            destinationExpressCenterCode = '" + clvar.destinationExpressCenterCode + "'," +
                     "            Address = '" + clvar.ConsigneeAddress + "',\n";

            if (clvar.isCod == false)
            {
                query += "                        cod = '0',\n";
            }

            if (clvar.isCod == true)
            {
                query += "                        cod = '1',\n";
            }
            query += " Bookingdate =CONVERT(datetime,'" + clvar.Bookingdate + "',105), ";
            query += " orgin = '" + clvar.origin + "', ispayable = '0', isinvoiced = '0'\n";
            query += "    where consignmentNumber = '" + clvar.consignmentNo + "'";
            string newPMQuery = "";
            string updatePMQuery = "";
            string removePMQuery = "";
            if (pm.Rows.Count > 0)
            {
                DataTable newPM = new DataTable();
                try
                {
                    pm.Select("NEW = '1'").CopyToDataTable();
                }
                catch (Exception ex)
                { }

                DataTable updatePM = new DataTable();
                try
                {
                    updatePM = pm.Select("NEW = ''").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                DataTable delPM = new DataTable();
                try
                {
                    delPM = pm.Select("NEW = 'REMOVED'").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                if (newPM.Rows.Count > 0)
                {
                    string temp = "CASE pm.ID ";
                    string temp1 = "CASE pm.ID ";
                    string temp2 = "CASE PM.ID ";
                    string temp4 = "CASE PM.ID ";
                    string temp3 = "";

                    foreach (DataRow dr in newPM.Rows)
                    {
                        temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                        temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                        temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                        temp4 += " WHEN '" + dr[0].ToString() + "' then '" + dr["calculationBase"].ToString() + "'\n";
                        temp3 += "'" + dr[0].ToString() + "',";
                    }
                    temp += "END CalculatedValue,";
                    temp1 += "END CalculatedGST, ";
                    temp2 += "END SORTORDER ";
                    temp4 += "END CALCULATIONBASE,";
                    temp3 = temp3.TrimEnd(',');
                    newPMQuery = "INSERT INTO CONSIGNMENTMODIFIER (priceModifierID, ConsignmentNumber, createdBy, CreatedON, modifiedCalculationValue, calculatedValue, CalculatedGSt, CalculationBase, SortOrder)\n" +
                                 " select pm.id, '" + clvar.consignmentNo + "' ConsignmentNumber, '" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy, GETDATE() CreatedON, pm.calculationValue, " + temp + temp1 + temp4 + temp2 + " from PriceModifiers pm WHERE pm.ID in (" + temp3 + ")";
                }
                if (updatePM.Rows.Count > 0)
                {
                    string temp = "CASE priceModifierID ";
                    string temp1 = "CASE priceModifierID ";
                    string temp2 = "CASE priceModifierID ";
                    string temp3 = "";
                    foreach (DataRow dr in updatePM.Rows)
                    {
                        temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                        temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                        temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                        temp3 += "'" + dr[0].ToString() + "',";
                    }
                    temp += "END ";
                    temp1 += "END  ";
                    temp2 += "END ";
                    temp3 = temp3.TrimEnd(',');
                    updatePMQuery = "UPDATE CONSIGNMENTMODIFIER SET CalculatedValue = " + temp + ", CalculatedGST = " + temp1 + " \n" +
                                 "  WHERE priceModifierID in (" + temp3 + ")";

                }
                if (delPM.Rows.Count > 0)
                {
                    removePMQuery += "DELETE FROM CONSIGNMENTMODIFIER where pricemodifierID in (";
                    foreach (DataRow dr in delPM.Rows)
                    {
                        removePMQuery += "'" + dr[0].ToString() + "',";
                    }
                    removePMQuery = removePMQuery.TrimEnd(',') + ") AND ConsignmentNumber = '" + clvar.consignmentNo + "'";
                }
            }


            string codQuery = "";
            if (cod & clvar.CatID != "1")
            {
                CommonFunction cf = new CommonFunction();
                DataTable dt_ = cf.GetCODConsignmentForApproval(clvar);
                if (dt_.Rows.Count == 0)
                {
                    codQuery = "Insert into CODConsignmentDetail (ConsignmentNumber, OrderRefNo, ProductTypeId, ProductDescription, ChargeCodAmount, CodAmount, CalculatedAmount ) Values \n" +
                        "(\n" +
                        "'" + clvar.consignmentNo.Trim() + "',\n" +
                        "'" + clvar.orderRefNo + "',\n" +
                        "'" + clvar.productTypeId + "',\n" +
                        "'" + clvar.productDescription + "',\n" +
                        "'" + clvar.chargeCODAmount + "',\n" +
                        "'" + clvar.codAmount + "',\n" +
                        "'" + clvar.calculatedCodAmount + "'\n" +
                        ")";
                }
                else
                {
                    codQuery = "UPDATE CODConsignmentDetail   \n" +
                               " SET    orderRefNo             = '" + clvar.orderRefNo + "', \n" +
                               "        productDescription     = '" + clvar.productDescription + "', \n" +
                               "        codAmount              = " + clvar.codAmount + ", \n" +
                               "        calculatedAmount       = " + clvar.calculatedCodAmount + " \n" +
                               " WHERE  consignmentNumber      = '" + clvar.consignmentNo.Trim() + "' ";
                }
            }


            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            int count = 0;
            try
            {
                sqlcmd.CommandText = query;
                sqlcmd.ExecuteNonQuery();

                if (newPMQuery != "")
                {
                    sqlcmd.CommandText = newPMQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                if (updatePMQuery != "")
                {
                    sqlcmd.CommandText = updatePMQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                if (removePMQuery != "")
                {
                    sqlcmd.CommandText = removePMQuery;
                    sqlcmd.ExecuteNonQuery();
                }


                if (codQuery != "")
                {
                    sqlcmd.CommandText = codQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                sqlcmd.CommandText = auditQuery;
                sqlcmd.ExecuteNonQuery();

                trans.Commit();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;
            }
            return "OK";
        }

        /*
        public string InsertConsignmentFromApprovalScreen(Cl_Variables clvar, DataTable pm, bool cod)
        {
            string query = "insert into consignment (consignmentNumber,\n" +
            "\t   orgin,\n" +
            "\t   consigner,\n" +
            "\t   consignee,\n" +
            "\t   destination,\n" +
            "\t   weight,\n" +
            "\t   riderCode,\n" +
            "\t   originExpressCenter,\n" +
            "\t   consignmentTypeId,\n" +
            "\t   chargedAmount,\n" +
            "\t   gst,\n" +
            "\t   status,\n" +
            "\t   isApproved,\n" +
            "\t   cod,\n" +
            "\t   creditClientId,\n" +
            "\t   bookingDate,\n" +
            "\t   createdBy,\n" +
            "\t   createdOn,\n" +
            "\t   customerType,\n" +
            "     zoneCode,\n" +
            "     serviceTypeName, totalAmount, syncID, AccountReceivingDate, ispriceComputed, expressCenterCode,destinationExpressCenterCode, consignerAccountNo) VALUES (" +
            "'" + clvar.consignmentNo + "'\n," +
            "'" + clvar.origin + "'\n," +
            "'" + clvar.Consigner + "'\n," +
            "'" + clvar.Consignee + "'\n," +
            "'" + clvar.destination + "'\n," +
            "'" + clvar.Weight + "'\n," +
            "'" + clvar.riderCode + "'\n," +
            "'" + clvar.OriginExpressCenterCode + "'\n," +
            "'" + clvar.cnTypeId + "'\n," +
            "'" + clvar.TotalAmount + "'\n," +
            "'" + clvar.gst + "'\n," +
            "'" + clvar.status + "'\n," +
            "'" + int.Parse(clvar.status.ToString()) + "'\n,";
            if (cod)
            {
                query += "'1'\n,";
            }
            else
            {
                query += "'0'\n,";
            }

            query += "'" + clvar.CustomerClientID + "'\n," +
            "'" + clvar.Bookingdate.ToString("yyyy-MM-dd") + "'\n," +
            "'" + HttpContext.Current.Session["U_ID"].ToString() + "'\n," +
            " GETDATE()\n," +
            "'" + clvar.Customertype + "'\n," +
            "'" + clvar.Zone + "'\n," +
            "'" + clvar.ServiceTypeName + "'\n" +
            ", '" + clvar.ChargeAmount + "', NEWID(),'" + DateTime.Parse(clvar.LoadingDate).ToString("yyyy-MM-dd") + "','1',\n" +
            "'" + HttpContext.Current.Session["ExpressCenter"].ToString() + "', '" + clvar.destinationExpressCenterCode + "', '" + clvar.consignerAccountNo + "')";

            string stateQuery = "Insert into consignmentStates (ConsignmentNumber, state, isInvoiced, CreatedBy, CreatedOn, UniqueID) VALUES (" +
                "'" + clvar.consignmentNo + "',\n" +
                "'NEW',\n" +
                "'0',\n" +
                "'" + HttpContext.Current.Session["U_ID"].ToString() + "'\n," +
                " GETDATE()\n," +
                " NEWID()\n" +
                ")";

            string pmQuery = "";
            if (pm.Rows.Count > 0)
            {
                string temp = "CASE pm.ID ";
                string temp1 = "CASE pm.ID ";
                string temp2 = "CASE PM.ID ";
                string temp4 = "CASE PM.ID ";
                string temp3 = "";

                foreach (DataRow dr in pm.Rows)
                {
                    temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                    temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                    temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                    temp4 += " WHEN '" + dr[0].ToString() + "' then '" + dr["calculationBase"].ToString() + "'\n";
                    temp3 += "'" + dr[0].ToString() + "',";
                }
                temp += "END CalculatedValue,";
                temp1 += "END CalculatedGST, ";
                temp2 += "END SORTORDER ";
                temp4 += "END CALCULATIONBASE,";
                temp3 = temp3.TrimEnd(',');
                pmQuery = "INSERT INTO CONSIGNMENTMODIFIER (priceModifierID, ConsignmentNumber, createdBy, CreatedON, modifiedCalculationValue, calculatedValue, CalculatedGSt, CalculationBase, SortOrder)\n" +
                             " select pm.id, '" + clvar.consignmentNo + "' ConsignmentNumber, '" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy, GETDATE() CreatedON, pm.calculationValue, " + temp + temp1 + temp4 + temp2 + " from PriceModifiers pm WHERE pm.ID in (" + temp3 + ")";
            }
            CommonFunction func = new CommonFunction();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string trackQuery = "insert into ConsignmentsTrackingHistory (ConsignmentNumber, stateID, currentLocation, TransactionTime) VALUES \n" +
                "('" + clvar.consignmentNo + "', '1', '" + func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString() + "', GETDATE())";

            //string codQuery = "";
            //if (cod)
            //{
            //    codQuery = "Insert into CODConsignmentDetail (ConsignmentNumber, OrderRefNumber, CustomerName, ProductTypeId, ProductDescription, ChargeCodAmount, CodAmount, CalculatedAmount ) Values (\n" +
            //        "(\n" +
            //        "'" + clvar.consignmentNo + "',\n" +
            //        "'" + clvar.productTypeId + "',\n" +
            //        "'" + clvar.productDescription + "',\n" +
            //        "'" + clvar.chargeCODAmount + "',\n" +
            //        "'" + clvar.codAmount + "',\n" +
            //        "'" + clvar.calculatedCodAmount + "',\n" +
            //        "";
            //}
            int count = 0;
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {

                sqlcmd.CommandText = query;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = stateQuery;
                sqlcmd.ExecuteNonQuery();

                if (pmQuery.ToString() != "")
                {
                    sqlcmd.CommandText = pmQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                sqlcmd.CommandText = trackQuery;
                sqlcmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;

            }
            finally { sqlcon.Close(); }
            return "OK";
        }
        */

        public string InsertConsignmentFromApprovalScreen(Cl_Variables clvar, DataTable pm, bool cod)
        {
            string auditQuery = "insert into MNP_consignmentunapproval (ConsignmentNumber, TransactionTime, status, USERID) VALUES (\n" +
     "'" + clvar.consignmentNo + "', GETDATE(), '1', '" + HttpContext.Current.Session["U_ID"].ToString() + "')";


            string query = "insert into consignment (consignmentNumber,\n" +
            "\t   orgin,\n" +
            "\t   consigner,\n" +
            "\t   consignee,\n" +
            "\t   destination,\n" +
            "\t   weight,\n" +
            "\t   riderCode,\n" +
            "\t   originExpressCenter,\n" +
            "\t   consignmentTypeId,\n" +
            "\t   chargedAmount,\n" +
            "\t   gst,\n" +
            "\t   status,\n" +
            "\t   isApproved,\n" +
            "\t   cod,\n" +
            "\t   creditClientId,\n" +
            "\t   createdBy,\n" +
            "\t   createdOn,\n" +
            "\t   customerType,\n" +
            "     zoneCode,BranchCode,\n" +
            "     serviceTypeName, totalAmount, syncID, AccountReceivingDate, ispriceComputed, expressCenterCode,destinationExpressCenterCode, consignerAccountNo,BookingDate,pieces,address, ispayable, isinvoiced) VALUES (" +
            "'" + clvar.consignmentNo + "'\n," +
            "'" + clvar.origin + "'\n," +
            "'" + clvar.Consigner + "'\n," +
            "'" + clvar.Consignee + "'\n," +
            "'" + clvar.destination + "'\n," +
            "'" + clvar.Weight + "'\n," +
            "'" + clvar.riderCode + "'\n," +
            "'" + clvar.OriginExpressCenterCode + "'\n," +
            "'" + clvar.cnTypeId + "'\n," +
            "'" + clvar.TotalAmount + "'\n," +
            "'" + clvar.gst + "'\n," +
            "'" + clvar.status + "'\n," +
            "'" + int.Parse(clvar.status.ToString()) + "'\n,";
            if (cod)
            {
                query += "'1'\n,";
            }
            else
            {
                query += "'0'\n,";
            }

            query += "'" + clvar.CustomerClientID + "'\n," +
            "'" + HttpContext.Current.Session["U_ID"].ToString() + "'\n," +
            " GETDATE()\n," +
            "'" + clvar.Customertype + "'\n," +
            "'" + HttpContext.Current.Session["ZoneCode"].ToString() + "'\n," +
            "'" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n," +
            "'" + clvar.ServiceTypeName + "'\n" +
            ", '" + clvar.ChargeAmount + "', NEWID(),'" + DateTime.Parse(clvar.LoadingDate).ToString("yyyy-MM-dd") + "','1',\n" +
            "'" + HttpContext.Current.Session["ExpressCenter"].ToString() + "', '" + clvar.destinationExpressCenterCode + "', '" + clvar.consignerAccountNo + "',CONVERT(datetime,'" + clvar.Bookingdate + "',105)," + clvar.pieces.ToString() + ",'" + clvar.ConsigneeAddress + "','0','0')";

            string stateQuery = "Insert into consignmentStates (ConsignmentNumber, state, isInvoiced, CreatedBy, CreatedOn, UniqueID) VALUES (" +
                "'" + clvar.consignmentNo + "',\n" +
                "'NEW',\n" +
                "'0',\n" +
                "'" + HttpContext.Current.Session["U_ID"].ToString() + "'\n," +
                " GETDATE()\n," +
                " NEWID()\n" +
                ")";

            string pmQuery = "";
            if (pm.Rows.Count > 0)
            {
                string temp = "CASE pm.ID ";
                string temp1 = "CASE pm.ID ";
                string temp2 = "CASE PM.ID ";
                string temp4 = "CASE PM.ID ";
                string temp3 = "";

                foreach (DataRow dr in pm.Rows)
                {
                    temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                    temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                    temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                    temp4 += " WHEN '" + dr[0].ToString() + "' then '" + dr["calculationBase"].ToString() + "'\n";
                    temp3 += "'" + dr[0].ToString() + "',";
                }
                temp += "END CalculatedValue,";
                temp1 += "END CalculatedGST, ";
                temp2 += "END SORTORDER ";
                temp4 += "END CALCULATIONBASE,";
                temp3 = temp3.TrimEnd(',');
                pmQuery = "INSERT INTO CONSIGNMENTMODIFIER (priceModifierID, ConsignmentNumber, createdBy, CreatedON, modifiedCalculationValue, calculatedValue, CalculatedGSt, CalculationBase, SortOrder)\n" +
                             " select pm.id, '" + clvar.consignmentNo + "' ConsignmentNumber, '" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy, GETDATE() CreatedON, pm.calculationValue, " + temp + temp1 + temp4 + temp2 + " from PriceModifiers pm WHERE pm.ID in (" + temp3 + ")";
            }
            CommonFunction func = new CommonFunction();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string trackQuery = "insert into ConsignmentsTrackingHistory (ConsignmentNumber, stateID, currentLocation, TransactionTime) VALUES \n" +
                "('" + clvar.consignmentNo + "', '1', '" + func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString() + "', GETDATE())";

            string codQuery = "";
            if (cod & clvar.CatID != "1")
            {
                codQuery = "Insert into CODConsignmentDetail (ConsignmentNumber, OrderRefNo, ProductTypeId, ProductDescription, ChargeCodAmount, CodAmount, CalculatedAmount ) Values \n" +
                    "(\n" +
                    "'" + clvar.consignmentNo + "',\n" +
                    "'" + clvar.orderRefNo + "',\n" +
                    "'" + clvar.productTypeId + "',\n" +
                    "'" + clvar.productDescription + "',\n" +
                    "'" + clvar.chargeCODAmount + "',\n" +
                    "'" + clvar.codAmount + "',\n" +
                    "'" + clvar.calculatedCodAmount + "'\n" +
                    ")";
            }
            int count = 0;
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {

                sqlcmd.CommandText = query;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = stateQuery;
                sqlcmd.ExecuteNonQuery();

                if (pmQuery.ToString() != "")
                {
                    sqlcmd.CommandText = pmQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                if (codQuery.ToString() != "")
                {
                    sqlcmd.CommandText = codQuery;
                    sqlcmd.ExecuteNonQuery();
                }



                sqlcmd.CommandText = trackQuery;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = auditQuery;
                sqlcmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;

            }
            finally { sqlcon.Close(); }
            return "OK";
        }
        public string InsertConsignmentFromApprovalScreen_OP(Cl_Variables clvar, DataTable pm, bool cod)
        {
            string auditQuery = "insert into MNP_consignmentunapproval (ConsignmentNumber, TransactionTime, status, USERID) VALUES (\n" +
     "'" + clvar.consignmentNo + "', GETDATE(), '1', '" + HttpContext.Current.Session["U_ID"].ToString() + "')";


            string query = "insert into consignment (consignmentNumber,\n" +
            "\t   orgin,\n" +
            "\t   consigner,\n" +
            "\t   consignee,\n" +
            "\t   destination,\n" +
            "\t   weight,\n" +
            "\t   riderCode,\n" +
            "\t   originExpressCenter,\n" +
            "\t   consignmentTypeId,\n" +
            "\t   chargedAmount,\n" +
            "\t   gst,\n" +
            "\t   status,\n" +
            "\t   isApproved,\n" +
            "\t   cod,\n" +
            "\t   creditClientId,\n" +
            "\t   createdBy,\n" +
            "\t   createdOn,\n" +
            "\t   customerType,\n" +
            "     zoneCode,BranchCode,\n" +
            "     serviceTypeName, totalAmount, syncID, AccountReceivingDate, ispriceComputed, expressCenterCode,destinationExpressCenterCode, consignerAccountNo,BookingDate,pieces,address, ispayable, isinvoiced) VALUES (" +
            "'" + clvar.consignmentNo + "'\n," +
            "'" + clvar.origin + "'\n," +
            "'" + clvar.Consigner + "'\n," +
            "'" + clvar.Consignee + "'\n," +
            "'" + clvar.destination + "'\n," +
            "'" + clvar.Weight + "'\n," +
            "'" + clvar.riderCode + "'\n," +
            "'" + clvar.OriginExpressCenterCode + "'\n," +
            "'" + clvar.cnTypeId + "'\n," +
            "'" + clvar.TotalAmount + "'\n," +
            "'" + clvar.gst + "'\n," +
            "'" + clvar.status + "'\n," +
            "'" + int.Parse(clvar.status.ToString()) + "'\n,";
            if (cod)
            {
                query += "'1'\n,";
            }
            else
            {
                query += "'0'\n,";
            }

            query += "'" + clvar.CustomerClientID + "'\n," +
            "'" + HttpContext.Current.Session["U_ID"].ToString() + "'\n," +
            " GETDATE()\n," +
            "'" + clvar.Customertype + "'\n," +
            "'" + HttpContext.Current.Session["ZoneCode"].ToString() + "'\n," +
            "'" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n," +
            "'" + clvar.ServiceTypeName + "'\n" +
            ", '" + clvar.ChargeAmount + "', NEWID(),'" + DateTime.Parse(clvar.LoadingDate).ToString("yyyy-MM-dd") + "','0',\n" +
            "'" + HttpContext.Current.Session["ExpressCenter"].ToString() + "', '" + clvar.destinationExpressCenterCode + "', '" + clvar.consignerAccountNo + "',CONVERT(datetime,'" + clvar.Bookingdate + "',105)," + clvar.pieces.ToString() + ",'" + clvar.ConsigneeAddress + "','0','0')";

            string stateQuery = "Insert into consignmentStates (ConsignmentNumber, state, isInvoiced, CreatedBy, CreatedOn, UniqueID) VALUES (" +
                "'" + clvar.consignmentNo + "',\n" +
                "'NEW',\n" +
                "'0',\n" +
                "'" + HttpContext.Current.Session["U_ID"].ToString() + "'\n," +
                " GETDATE()\n," +
                " NEWID()\n" +
                ")";

            string pmQuery = "";
            if (pm.Rows.Count > 0)
            {
                string temp = "CASE pm.ID ";
                string temp1 = "CASE pm.ID ";
                string temp2 = "CASE PM.ID ";
                string temp4 = "CASE PM.ID ";
                string temp3 = "";

                foreach (DataRow dr in pm.Rows)
                {
                    temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                    temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                    temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                    temp4 += " WHEN '" + dr[0].ToString() + "' then '" + dr["calculationBase"].ToString() + "'\n";
                    temp3 += "'" + dr[0].ToString() + "',";
                }
                temp += "END CalculatedValue,";
                temp1 += "END CalculatedGST, ";
                temp2 += "END SORTORDER ";
                temp4 += "END CALCULATIONBASE,";
                temp3 = temp3.TrimEnd(',');
                pmQuery = "INSERT INTO CONSIGNMENTMODIFIER (priceModifierID, ConsignmentNumber, createdBy, CreatedON, modifiedCalculationValue, calculatedValue, CalculatedGSt, CalculationBase, SortOrder)\n" +
                             " select pm.id, '" + clvar.consignmentNo + "' ConsignmentNumber, '" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy, GETDATE() CreatedON, pm.calculationValue, " + temp + temp1 + temp4 + temp2 + " from PriceModifiers pm WHERE pm.ID in (" + temp3 + ")";
            }
            CommonFunction func = new CommonFunction();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string trackQuery = "insert into ConsignmentsTrackingHistory (ConsignmentNumber, stateID, currentLocation, TransactionTime) VALUES \n" +
                "('" + clvar.consignmentNo + "', '1', '" + func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString() + "', GETDATE())";

            string codQuery = "";
            if (cod & clvar.CatID != "1")
            {
                codQuery = "Insert into CODConsignmentDetail (ConsignmentNumber, OrderRefNo, ProductTypeId, ProductDescription, ChargeCodAmount, CodAmount, CalculatedAmount ) Values \n" +
                    "(\n" +
                    "'" + clvar.consignmentNo + "',\n" +
                    "'" + clvar.orderRefNo + "',\n" +
                    "'" + clvar.productTypeId + "',\n" +
                    "'" + clvar.productDescription + "',\n" +
                    "'" + clvar.chargeCODAmount + "',\n" +
                    "'" + clvar.codAmount + "',\n" +
                    "'" + clvar.calculatedCodAmount + "'\n" +
                    ")";
            }
            int count = 0;
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {

                sqlcmd.CommandText = query;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = stateQuery;
                sqlcmd.ExecuteNonQuery();

                if (pmQuery.ToString() != "")
                {
                    sqlcmd.CommandText = pmQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                if (codQuery.ToString() != "")
                {
                    sqlcmd.CommandText = codQuery;
                    sqlcmd.ExecuteNonQuery();
                }



                sqlcmd.CommandText = trackQuery;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = auditQuery;
                sqlcmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;

            }
            finally { sqlcon.Close(); }
            return "OK";
        }

        public string Add_Consignment_(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());

            sqlcon.Open();

            using (SqlTransaction dbTrans = sqlcon.BeginTransaction())
            {
                try
                {
                    string sql = "INSERT INTO Consignment \n"
               + "		  ( \n"
               + "		    [consignmentNumber], \n"
               + "		    [consigner], \n"
               + "		    [consignee], \n"
               + "		    [couponNumber], \n"
               + "		    [customerType], \n"
               + "		    [orgin], \n"
               + "		    [destination], \n"
               + "		    [pieces], \n"
               + "		    [serviceTypeName], \n"
               + "		    [creditClientId], \n"
               + "		    [weight], \n"
               + "		    [weightUnit], \n"
               + "		    [discount], \n"
               + "		    [cod], \n"
               + "		    [address], \n"
               + "		    [createdBy], \n"
               + "		    [createdOn] \n"
               + "		    --,[modifiedBy],[modifiedOn] \n"
               + "		    , \n"
               + "		    [status], \n"
               + "		    [totalAmount], \n"
               + "		    [zoneCode], \n"
               + "		    [branchCode], \n"
               + "		    [expressCenterCode], \n"
               + "		    [syncStatus], \n"
               + "		    [consignmentTypeId], \n"
               + "		    [isCreatedFromMMU], \n"
               + "		    [deliveryType], \n"
               + "		    [remarks], \n"
               + "		    [shipperAddress] \n"
               + "		    --,[transactionNumber] \n"
               + "		    , \n"
               + "		    [riderCode], \n"
               + "		    [gst], \n"
               + "		    [width], \n"
               + "		    [breadth], \n"
               + "		    [height], \n"
               + "		    [PakageContents], \n"
               + "		    [expressionDeliveryDateTime], \n"
               + "		    [expressionGreetingCard], \n"
               + "		    [expressionMessage], \n"
               + "		    [consigneePhoneNo], \n"
               + "		    [expressionconsignmentRefNumber], \n"
               + "		    [otherCharges], \n"
               + "		    [routeCode], \n"
               + "		    [docPouchNo], \n"
               + "		    [consignerPhoneNo], \n"
               + "		    [consignerCellNo], \n"
               + "		    [consignerCNICNo], \n"
               + "		    [consignerAccountNo], \n"
               + "		    [consignerEmail], \n"
               + "		    [docIsHomeDelivery], \n"
               + "		    [cutOffTime], \n"
               + "		    [destinationCountryCode], \n"
               + "		    [decalaredValue], \n"
               + "		    [insuarancePercentage], \n"
               + "		    [consignmentScreen], \n"
               + "		    [isInsured], \n"
               + "		    [isReturned], \n"
               + "		    [consigneeCNICNo], \n"
               + "		    [cutOffTimeShift], \n"
               + "		    [bookingDate], \n"
               + "		    [cnClientType], \n"
               + "		    [syncState], \n"
               + "		    [syncId], \n"
               + "		    [destinationExpressCenterCode], \n"
               + "		    [isApproved], \n"
               + "		    [deliveryStatus], \n"
               + "		    [dayType], \n"
               + "		    [originExpressCenter], \n"
               + "		    [isPriceComputed], \n"
               + "		    [isNormalTariffApplied], \n"
               + "		    [receivedFromRider], \n"
               + "		    [chargedAmount], \n"
               + "		    [misRouted], \n"
               + "		    [accountReceivingDate] \n"
               + "		  ) \n"
               + "		VALUES \n"
               + "		  ( \n"
               + "		    @consignmentNumber, \n"
               + "		    @consigner, \n"
               + "		    @consignee, \n"
               + "		    @couponNumber, \n"
               + "		    @customerType, \n"
               + "		    @orgin, \n"
               + "		    @destination, \n"
               + "		    @pieces, \n"
               + "		    @serviceTypeName, \n"
               + "		    @creditClientId, \n"
               + "		    @weight, \n"
               + "		    @weightUnit, \n"
               + "		    @discount, \n"
               + "		    @cod, \n"
               + "		    @address, \n"
               + "		    @createdBy, \n"
               + "		    GetDate()--,@createdOn,@modifiedBy,@modifiedOn \n"
               + "		    , \n"
               + "		    @status, \n"
               + "		    @totalAmount, \n"
               + "		    @zoneCode, \n"
               + "		    @branchCode, \n"
               + "		    @expressCenterCode, \n"
               + "		    2--,@syncStatus \n"
               + "		    , \n"
               + "		    @consignmentTypeId, \n"
               + "		    @isCreatedFromMMU, \n"
               + "		    @deliveryType, \n"
               + "		    @remarks, \n"
               + "		    @shipperAddress \n"
               + "		    --,@transactionNumber \n"
               + "		    , \n"
               + "		    @riderCode, \n"
               + "		    @gst, \n"
               + "		    @width, \n"
               + "		    @breadth, \n"
               + "		    @height, \n"
               + "		    @PakageContents, \n"
               + "		    @expressionDeliveryDateTime, \n"
               + "		    @expressionGreetingCard, \n"
               + "		    @expressionMessage, \n"
               + "		    @consigneePhoneNo, \n"
               + "		    @expressionconsignmentRefNumber, \n"
               + "		    @otherCharges, \n"
               + "		    @routeCode, \n"
               + "		    @docPouchNo, \n"
               + "		    @consignerPhoneNo, \n"
               + "		    @consignerCellNo, \n"
               + "		    @consignerCNICNo, \n"
               + "		    @consignerAccountNo, \n"
               + "		    @consignerEmail, \n"
               + "		    @docIsHomeDelivery, \n"
               + "		    @cutOffTime, \n"
               + "		    @destinationCountryCode, \n"
               + "		    @decalaredValue, \n"
               + "		    @insuarancePercentage, \n"
               + "		    @consignmentScreen, \n"
               + "		    @isInsured, \n"
               + "		    @isReturned, \n"
               + "		    @consigneeCNICNo, \n"
               + "		    @cutOffTimeShift, \n"
               + "		    @bookingDate, \n"
               + "		    @cnClientType, \n"
               + "		    5--,@syncState \n"
               + "		    , \n"
               + "		    NEWID()--,@syncId \n"
               + "		    , \n"
               + "		    @destinationExpressCenterCode, \n"
               + "		    1--,@isApproved \n"
               + "		    , \n"
               + "		    0, \n"
               + "		    @dayType, \n"
               + "		    @originExpressCenter, \n"
               + "		    0--,@isPriceComputed \n"
               + "		    , \n"
               + "		    0--,@isNormalTariffApplied \n"
               + "		    , \n"
               + "		    @receivedFromRider, \n"
               + "		    @chargedAmount, \n"
               + "		    0, \n"
               + "		    @accountReceivingDate \n"
               + "		  ) \n"
               + "		";

                    using (SqlCommand sqlcmd = new SqlCommand(sql, sqlcon))
                    {
                        sqlcmd.Transaction = dbTrans;

                        // dbCommand.Parameters.Add("id", SqlType.VarChar).Value = id;
                        sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                        sqlcmd.Parameters.AddWithValue("@consigner", obj.Consigner);
                        sqlcmd.Parameters.AddWithValue("@consignee", obj.Consignee);
                        sqlcmd.Parameters.AddWithValue("@couponNumber", obj.CouponNo);
                        sqlcmd.Parameters.AddWithValue("@customerType", obj.Customertype);
                        sqlcmd.Parameters.AddWithValue("@orgin", obj.origin);
                        sqlcmd.Parameters.AddWithValue("@destination", obj.Destination);
                        sqlcmd.Parameters.AddWithValue("@pieces", obj.pieces);
                        sqlcmd.Parameters.AddWithValue("@serviceTypeName", obj.ServiceTypeName);
                        sqlcmd.Parameters.AddWithValue("@creditClientId", obj.CustomerClientID);
                        sqlcmd.Parameters.AddWithValue("@consignmentTypeId", obj.Con_Type);
                        sqlcmd.Parameters.AddWithValue("@weight", obj.Weight);
                        sqlcmd.Parameters.AddWithValue("@weightUnit", obj.Unit);
                        sqlcmd.Parameters.AddWithValue("@discount", obj.Discount);
                        sqlcmd.Parameters.AddWithValue("@cod", "0");
                        sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                        sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                        sqlcmd.Parameters.AddWithValue("@status", obj.status);
                        sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                        sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                        sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                        sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                        sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", obj.isCreatedFromMMUs);
                        sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                        sqlcmd.Parameters.AddWithValue("@remarks", obj.Remarks);
                        sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                        sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                        sqlcmd.Parameters.AddWithValue("@gst", obj.gst);
                        sqlcmd.Parameters.AddWithValue("@width", obj.width);
                        sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                        sqlcmd.Parameters.AddWithValue("@height", obj.height);
                        sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                        sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                        sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", obj.expressionGreetingCard);
                        sqlcmd.Parameters.AddWithValue("@expressionMessage", obj.expressionMessage);
                        sqlcmd.Parameters.AddWithValue("@consigneePhoneNo", obj.ConsigneeCell);
                        sqlcmd.Parameters.AddWithValue("@expressionconsignmentRefNumber", obj.expressionconsignmentRefNumber);
                        sqlcmd.Parameters.AddWithValue("@otherCharges", obj.Othercharges);
                        sqlcmd.Parameters.AddWithValue("@routeCode", obj.routeCode);
                        sqlcmd.Parameters.AddWithValue("@docPouchNo", obj.docPouchNo);
                        sqlcmd.Parameters.AddWithValue("@consignerPhoneNo", obj.ConsignerPhone);
                        sqlcmd.Parameters.AddWithValue("@consignerCellNo", obj.ConsignerCell);
                        sqlcmd.Parameters.AddWithValue("@consignerCNICNo", obj.ConsignerCNIC);
                        sqlcmd.Parameters.AddWithValue("@consignerAccountNo", obj.consignerAccountNo);
                        sqlcmd.Parameters.AddWithValue("@consignerEmail", obj.consignerEmail);
                        sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", obj.docIsHomeDelivery);
                        sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                        sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                        sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                        sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                        sqlcmd.Parameters.AddWithValue("@consignmentScreen", "12");
                        sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                        sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                        sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                        sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                        sqlcmd.Parameters.AddWithValue("@bookingDate", obj.Bookingdate);
                        sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                        sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                        sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                        sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.expresscenter);
                        sqlcmd.Parameters.AddWithValue("@receivedFromRider", obj.receivedFromRider);
                        sqlcmd.Parameters.AddWithValue("@chargedAmount", obj.ChargeAmount);
                        sqlcmd.Parameters.AddWithValue("@cnState", obj.cnState);
                        sqlcmd.Parameters.AddWithValue("@cardRefNo", obj.cardRefNo);
                        sqlcmd.Parameters.AddWithValue("@manifestNo", obj.manifestNo);
                        sqlcmd.Parameters.AddWithValue("@manifestId", obj.manifestId);
                        sqlcmd.Parameters.AddWithValue("@originBrName", obj.originBrName);
                        sqlcmd.Parameters.AddWithValue("@orderRefNo", obj.orderRefNo);
                        sqlcmd.Parameters.AddWithValue("@customerName", obj.customerName);
                        sqlcmd.Parameters.AddWithValue("@productTypeId", obj.productTypeId);
                        sqlcmd.Parameters.AddWithValue("@productDescription", obj.productDescription);
                        sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                        sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                        //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                        //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                        sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                        sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                        sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                        sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                        sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                        sqlcmd.Parameters.AddWithValue("@cnScreenId", obj.cnScreenId);
                        sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                        sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                        sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                        sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);
                        sqlcmd.Parameters.AddWithValue("@accountReceivingDate", obj.Day);

                        sqlcmd.ExecuteNonQuery();
                    }

                    dbTrans.Commit();
                }
                catch (SqlException)
                {
                    dbTrans.Rollback();

                    throw; // bubble up the exception and preserve the stack trace
                }

            }
            sqlcon.Close();
            //return IsUnique;
            return clvar.Error;
        }

        public string Add_Consignment_2(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());

            sqlcon.Open();

            using (SqlTransaction dbTrans = sqlcon.BeginTransaction())
            {
                try
                {
                    string sql = "INSERT INTO Consignment \n"
               + "		  ( \n"
               + "		    [consignmentNumber], \n"
               + "		    [consigner], \n"
               + "		    [consignee], \n"
               + "		    [couponNumber], \n"
               + "		    [customerType], \n"
               + "		    [orgin], \n"
               + "		    [destination], \n"
               + "		    [pieces], \n"
               + "		    [serviceTypeName], \n"
               + "		    [creditClientId], \n"
               + "		    [weight], \n"
               + "		    [weightUnit], \n"
               + "		    [discount], \n"
               + "		    [cod], \n"
               + "		    [address], \n"
               + "		    [createdBy], \n"
               + "		    [createdOn] \n"
               + "		    --,[modifiedBy],[modifiedOn] \n"
               + "		    , \n"
               + "		    [status], \n"
               + "		    [totalAmount], \n"
               + "		    [zoneCode], \n"
               + "		    [branchCode], \n"
               + "		    [expressCenterCode], \n"
               + "		    [syncStatus], \n"
               + "		    [consignmentTypeId], \n"
               + "		    [isCreatedFromMMU], \n"
               + "		    [deliveryType], \n"
               + "		    [remarks], \n"
               + "		    [shipperAddress] \n"
               + "		    --,[transactionNumber] \n"
               + "		    , \n"
               + "		    [riderCode], \n"
               + "		    [gst], \n"
               + "		    [width], \n"
               + "		    [breadth], \n"
               + "		    [height], \n"
               + "		    [PakageContents], \n"
               + "		    [expressionDeliveryDateTime], \n"
               + "		    [expressionGreetingCard], \n"
               + "		    [expressionMessage], \n"
               + "		    [consigneePhoneNo], \n"
               + "		    [expressionconsignmentRefNumber], \n"
               + "		    [otherCharges], \n"
               + "		    [routeCode], \n"
               + "		    [docPouchNo], \n"
               + "		    [consignerPhoneNo], \n"
               + "		    [consignerCellNo], \n"
               + "		    [consignerCNICNo], \n"
               + "		    [consignerAccountNo], \n"
               + "		    [consignerEmail], \n"
               + "		    [docIsHomeDelivery], \n"
               + "		    [cutOffTime], \n"
               + "		    [destinationCountryCode], \n"
               + "		    [decalaredValue], \n"
               + "		    [insuarancePercentage], \n"
               + "		    [consignmentScreen], \n"
               + "		    [isInsured], \n"
               + "		    [isReturned], \n"
               + "		    [consigneeCNICNo], \n"
               + "		    [cutOffTimeShift], \n"
               + "		    [bookingDate], \n"
               + "		    [cnClientType], \n"
               + "		    [syncState], \n"
               + "		    [syncId], \n"
               + "		    [destinationExpressCenterCode], \n"
               + "		    [isApproved], \n"
               + "		    [deliveryStatus], \n"
               + "		    [dayType], \n"
               + "		    [originExpressCenter], \n"
               + "		    [isPriceComputed], \n"
               + "		    [isNormalTariffApplied], \n"
               + "		    [receivedFromRider], \n"
               + "		    [chargedAmount], \n"
               + "		    [misRouted] \n"
               + "		  ) \n"
               + "		VALUES \n"
               + "		  ( \n"
               + "		    @consignmentNumber, \n"
               + "		    @consigner, \n"
               + "		    @consignee, \n"
               + "		    @couponNumber, \n"
               + "		    @customerType, \n"
               + "		    @orgin, \n"
               + "		    @destination, \n"
               + "		    @pieces, \n"
               + "		    @serviceTypeName, \n"
               + "		    @creditClientId, \n"
               + "		    @weight, \n"
               + "		    @weightUnit, \n"
               + "		    @discount, \n"
               + "		    @cod, \n"
               + "		    @address, \n"
               + "		    @createdBy, \n"
               + "		    GetDate()--,@createdOn,@modifiedBy,@modifiedOn \n"
               + "		    , \n"
               + "		    @status, \n"
               + "		    @totalAmount, \n"
               + "		    @zoneCode, \n"
               + "		    @branchCode, \n"
               + "		    @expressCenterCode, \n"
               + "		    2--,@syncStatus \n"
               + "		    , \n"
               + "		    @consignmentTypeId, \n"
               + "		    @isCreatedFromMMU, \n"
               + "		    @deliveryType, \n"
               + "		    @remarks, \n"
               + "		    @shipperAddress \n"
               + "		    --,@transactionNumber \n"
               + "		    , \n"
               + "		    @riderCode, \n"
               + "		    @gst, \n"
               + "		    @width, \n"
               + "		    @breadth, \n"
               + "		    @height, \n"
               + "		    @PakageContents, \n"
               + "		    @expressionDeliveryDateTime, \n"
               + "		    @expressionGreetingCard, \n"
               + "		    @expressionMessage, \n"
               + "		    @consigneePhoneNo, \n"
               + "		    @expressionconsignmentRefNumber, \n"
               + "		    @otherCharges, \n"
               + "		    @routeCode, \n"
               + "		    @docPouchNo, \n"
               + "		    @consignerPhoneNo, \n"
               + "		    @consignerCellNo, \n"
               + "		    @consignerCNICNo, \n"
               + "		    @consignerAccountNo, \n"
               + "		    @consignerEmail, \n"
               + "		    @docIsHomeDelivery, \n"
               + "		    @cutOffTime, \n"
               + "		    @destinationCountryCode, \n"
               + "		    @decalaredValue, \n"
               + "		    @insuarancePercentage, \n"
               + "		    @consignmentScreen, \n"
               + "		    @isInsured, \n"
               + "		    @isReturned, \n"
               + "		    @consigneeCNICNo, \n"
               + "		    @cutOffTimeShift, \n"
               + "		    @bookingDate, \n"
               + "		    @cnClientType, \n"
               + "		    5--,@syncState \n"
               + "		    , \n"
               + "		    NEWID()--,@syncId \n"
               + "		    , \n"
               + "		    @destinationExpressCenterCode, \n"
               + "		    0--,@isApproved \n"
               + "		    , \n"
               + "		    0, \n"
               + "		    @dayType, \n"
               + "		    @originExpressCenter, \n"
               + "		    0--,@isPriceComputed \n"
               + "		    , \n"
               + "		    0--,@isNormalTariffApplied \n"
               + "		    , \n"
               + "		    @receivedFromRider, \n"
               + "		    @chargedAmount, \n"
               + "		    0 \n"
               + "		  ) \n"
               + "		";

                    using (SqlCommand sqlcmd = new SqlCommand(sql, sqlcon))
                    {
                        sqlcmd.Transaction = dbTrans;

                        // dbCommand.Parameters.Add("id", SqlType.VarChar).Value = id;
                        sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                        sqlcmd.Parameters.AddWithValue("@consigner", obj.Consigner);
                        sqlcmd.Parameters.AddWithValue("@consignee", obj.Consignee);
                        sqlcmd.Parameters.AddWithValue("@couponNumber", obj.CouponNo);
                        sqlcmd.Parameters.AddWithValue("@customerType", obj.Customertype);
                        sqlcmd.Parameters.AddWithValue("@orgin", obj.origin);
                        sqlcmd.Parameters.AddWithValue("@destination", obj.Destination);
                        sqlcmd.Parameters.AddWithValue("@pieces", obj.pieces);
                        sqlcmd.Parameters.AddWithValue("@serviceTypeName", obj.ServiceTypeName);
                        sqlcmd.Parameters.AddWithValue("@creditClientId", obj.CustomerClientID);
                        sqlcmd.Parameters.AddWithValue("@consignmentTypeId", obj.Con_Type);
                        sqlcmd.Parameters.AddWithValue("@weight", obj.Weight);
                        sqlcmd.Parameters.AddWithValue("@weightUnit", obj.Unit);
                        sqlcmd.Parameters.AddWithValue("@discount", obj.Discount);
                        sqlcmd.Parameters.AddWithValue("@cod", "0");
                        sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                        sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                        sqlcmd.Parameters.AddWithValue("@status", obj.status);
                        sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                        sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                        sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                        sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                        sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", obj.isCreatedFromMMUs);
                        sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                        sqlcmd.Parameters.AddWithValue("@remarks", obj.Remarks);
                        sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                        sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                        sqlcmd.Parameters.AddWithValue("@gst", obj.gst);
                        sqlcmd.Parameters.AddWithValue("@width", obj.width);
                        sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                        sqlcmd.Parameters.AddWithValue("@height", obj.height);
                        sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                        sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                        sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", obj.expressionGreetingCard);
                        sqlcmd.Parameters.AddWithValue("@expressionMessage", obj.expressionMessage);
                        sqlcmd.Parameters.AddWithValue("@consigneePhoneNo", obj.ConsigneeCell);
                        sqlcmd.Parameters.AddWithValue("@expressionconsignmentRefNumber", obj.expressionconsignmentRefNumber);
                        sqlcmd.Parameters.AddWithValue("@otherCharges", obj.Othercharges);
                        sqlcmd.Parameters.AddWithValue("@routeCode", obj.routeCode);
                        sqlcmd.Parameters.AddWithValue("@docPouchNo", obj.docPouchNo);
                        sqlcmd.Parameters.AddWithValue("@consignerPhoneNo", obj.ConsignerPhone);
                        sqlcmd.Parameters.AddWithValue("@consignerCellNo", obj.ConsignerCell);
                        sqlcmd.Parameters.AddWithValue("@consignerCNICNo", obj.ConsignerCNIC);
                        sqlcmd.Parameters.AddWithValue("@consignerAccountNo", obj.consignerAccountNo);
                        sqlcmd.Parameters.AddWithValue("@consignerEmail", obj.consignerEmail);
                        sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", obj.docIsHomeDelivery);
                        sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                        sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                        sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                        sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                        sqlcmd.Parameters.AddWithValue("@consignmentScreen", obj.cnScreenId);
                        sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                        sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                        sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                        sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                        sqlcmd.Parameters.AddWithValue("@bookingDate", obj.Bookingdate);
                        sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                        sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                        sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                        sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.expresscenter);
                        sqlcmd.Parameters.AddWithValue("@receivedFromRider", obj.receivedFromRider);
                        sqlcmd.Parameters.AddWithValue("@chargedAmount", obj.ChargeAmount);
                        sqlcmd.Parameters.AddWithValue("@cnState", obj.cnState);
                        sqlcmd.Parameters.AddWithValue("@cardRefNo", obj.cardRefNo);
                        sqlcmd.Parameters.AddWithValue("@manifestNo", obj.manifestNo);
                        sqlcmd.Parameters.AddWithValue("@manifestId", obj.manifestId);
                        sqlcmd.Parameters.AddWithValue("@originBrName", obj.originBrName);
                        sqlcmd.Parameters.AddWithValue("@orderRefNo", obj.orderRefNo);
                        sqlcmd.Parameters.AddWithValue("@customerName", obj.customerName);
                        sqlcmd.Parameters.AddWithValue("@productTypeId", obj.productTypeId);
                        sqlcmd.Parameters.AddWithValue("@productDescription", obj.productDescription);
                        sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                        sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                        sqlcmd.Parameters.AddWithValue("@calculatedCodAmount", obj.calculatedCodAmount);
                        //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                        //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                        sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                        sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                        sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                        sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                        sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                        sqlcmd.Parameters.AddWithValue("@cnScreenId", obj.cnScreenId);
                        sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                        sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                        sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                        sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);
                        sqlcmd.Parameters.AddWithValue("@accountReceivingDate", obj.Day);

                        sqlcmd.ExecuteNonQuery();
                    }

                    dbTrans.Commit();
                }
                catch (SqlException)
                {
                    dbTrans.Rollback();

                    throw; // bubble up the exception and preserve the stack trace
                }

            }
            sqlcon.Close();
            //return IsUnique;
            return clvar.Error;
        }

        public string Update_Consignment_(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;

            try
            {
                string sql = "update Consignment set \n"
                               + "		    [consigner] = '" + obj.Consigner + "' , \n"
                               + "		    [consignee]  = '" + obj.Consignee + "' , \n"
                               + "		    [couponNumber]  = '" + obj.CouponNo + "' , \n"
                               + "		    [customerType]  = '" + obj.Customertype + "' , \n"
                               + "		    [orgin]  = '" + obj.origin + "' , \n"
                               + "		    [destination]  = '" + obj.Destination + "' , \n"
                               + "		    [pieces]  = '" + obj.pieces + "' , \n"
                               + "		    [serviceTypeName]  = '" + obj.ServiceTypeName + "' , \n"
                               + "		    [creditClientId] = '" + obj.CustomerClientID + "' , \n"
                               + "		    [weight] = '" + obj.Weight + "' , \n"
                               + "		    [weightUnit] = '" + obj.Unit + "' , \n"
                               + "		    [discount] = '" + obj.Discount + "' , \n"
                               + "		    [cod] = '0' , \n"
                               + "		    [address] = '" + obj.ConsigneeAddress + "' , \n"
                               + "		    [modifiedBy]  = '" + obj.createdBy + "' , \n"
                               + "          [modifiedOn]  = GETDATE() ,\n"
                               + "		    [status]  = '" + obj.status + "' , \n"
                               + "		    [totalAmount] = '" + obj.TotalAmount + "' , \n"
                               + "		    [zoneCode] = '" + obj.Zone + "' , \n"
                               + "		    [branchCode]  = '" + obj.origin + "' , \n"
                               + "		    [expressCenterCode] = '" + obj.expresscenter + "' , \n"
                               //  + "		    [syncStatus] = '"++"' , \n"
                               + "		    [consignmentTypeId] = '" + obj.Con_Type + "' , \n"
                               + "		    [isCreatedFromMMU] = '" + obj.isCreatedFromMMUs + "' , \n"
                               + "		    [deliveryType] = '" + obj.deliveryType + "' , \n"
                               + "		    [remarks] = '" + obj.Remarks + "' , \n"
                               + "		    [shipperAddress] = '" + obj.ConsignerAddress + "' , \n"
                               + "		    [riderCode] = '" + obj.RiderCode + "' , \n"
                               + "		    [gst] = '" + obj.gst + "' , \n"
                               + "		    [width] = '" + obj.width + "' , \n"
                               + "		    [breadth] = '" + obj.breadth + "' , \n"
                               + "		    [height] = '" + obj.height + "' , \n"
                               + "		    [PakageContents] = '" + obj.PakageContents + "' , \n"
                               + "		    [expressionDeliveryDateTime] = '" + obj.expressionDeliveryDateTime.ToString("yyyy-MM-dd hh:mm:ss") + "' , \n"
                               + "		    [expressionGreetingCard] = '" + obj.expressionGreetingCard + "' , \n"
                               + "		    [expressionMessage] = '" + obj.expressionMessage + "' , \n"
                               + "		    [consigneePhoneNo] = '" + obj.ConsigneeCell + "' , \n"
                               + "		    [expressionconsignmentRefNumber] = '" + obj.expressionconsignmentRefNumber + "' , \n"
                               + "		    [otherCharges] = '" + obj.Othercharges + "' , \n"
                               + "		    [routeCode] = '" + obj.routeCode + "' , \n"
                               + "		    [docPouchNo] = '" + obj.docPouchNo + "' , \n"
                               + "		    [consignerPhoneNo] = '" + obj.ConsignerPhone + "' , \n"
                               + "		    [consignerCellNo] = '" + obj.ConsignerCell + "' , \n"
                               + "		    [consignerCNICNo] = '" + obj.ConsignerCNIC + "' , \n"
                               + "		    [consignerAccountNo] = '" + obj.consignerAccountNo + "' , \n"
                               + "		    [consignerEmail] = '" + obj.consignerEmail + "' , \n"
                               + "		    [docIsHomeDelivery] = '" + obj.docIsHomeDelivery + "' , \n"
                               + "		    [cutOffTime] = '" + obj.cutOffTime.ToString("yyyy-MM-dd hh:mm:ss") + "' , \n"
                               + "		    [destinationCountryCode] = '" + obj.destinationCountryCode + "' , \n"
                               + "		    [decalaredValue] = '" + obj.Declaredvalue + "' , \n"
                               + "		    [insuarancePercentage] = '" + obj.insuarancePercentage + "' , \n"
                               + "		    [consignmentScreen] = '" + obj.cnScreenId + "' , \n"
                               + "		    [isInsured] = '" + obj.isInsured + "' , \n"
                               + "		    [isReturned] = '" + obj.isReturned + "' , \n"
                               + "		    [consigneeCNICNo] = '" + obj.ConsigneeCNIC + "' , \n"
                               + "		    [cutOffTimeShift] = '" + obj.cutOffTimeShift.ToString("yyyy-MM-dd hh:mm:ss") + "' , \n"
                               + "		    [bookingDate] = '" + obj.Bookingdate.ToString("yyyy-MM-dd hh:mm:ss") + "' , \n"
                               + "		    [cnClientType] = '" + obj.cnClientType + "' , \n"
                               + "		    [destinationExpressCenterCode] = '" + obj.destinationExpressCenterCode + "' , \n"
                               + "		    [originExpressCenter] = '" + obj.expresscenter + "' , \n"
                               + "		    [receivedFromRider] = '" + obj.receivedFromRider + "' , \n"
                               + "		    [chargedAmount] = '" + obj.ChargeAmount + "' , \n"
                               + "		    [isApproved] = '1' , \n"
                               + "		    [isPriceComputed] = '1' , \n"
                               + "		    [accountReceivingDate] = '" + obj.expressionDeliveryDateTime.ToString("yyyy-MM-dd hh:mm:ss") + "' \n"
                               + "          Where CONSIGNMENTNUMBER = '" + obj.consignmentNo + "' ";

                SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                SqlTransaction trans;
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                trans = sqlcon.BeginTransaction();
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                try
                {
                    sqlcmd.CommandText = sql;
                    sqlcmd.ExecuteNonQuery();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    sqlcon.Close();
                }
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
        }

        #region Insert_ConsignmentTrackingHistory COMMENTED ON 08/08/2016 05:54PM
        public void Insert_ConsignmentTrackingHistory(Cl_Variables clvar)
        {
            try
            {
                string query = "insert into ConsignmentsTrackingHistory \n" +
                                "  (consignmentNumber, stateID, currentLocation, riderName, transactionTime)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar.consignmentNo + "',\n" +
                                "   '" + clvar.StateID + "',\n" +
                                "   '" + clvar._CityCode + "',\n" +

                                "   '" + clvar.RiderCode + "',\n" +
                                "   GETDATE()\n" +
                                " ) ";

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
        public void Insert_ConsignmentTrackingHistory(Variable clvar)
        {
            try
            {
                string query = "insert into ConsignmentsTrackingHistory \n" +
                                "  (consignmentNumber, stateID, currentLocation, manifestNumber, bagNumber, loadingNumber, mawbNumber, runsheetNumber, riderName, transactionTime)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._ConsignmentNo + "',\n" +
                                "   '" + clvar._StateId + "',\n" +
                                "   '" + clvar._CityCode + "',\n" +
                                "   '" + clvar._Manifest + "',\n" +
                                "   '" + clvar._BagNumber + "',\n" +
                                "   '" + clvar._LoadingId + "',\n" +
                                "   '" + clvar._MawbNumber + "',\n" +
                                "   '" + clvar._RunsheetNumber + "',\n" +
                                "   '" + clvar.RiderCode + "',\n" +
                                "   GETDATE()\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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



        public DataTable GetConsignmentsForInvoice(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string sqlString = "select substring(CAST(CONVERT(date, c.bookingdate, 105) as varchar),0,10) BookingDate,\n" +
            "\t   c.consignmentNumber,\n" +
            "\t   c.pieces,\n" +
            "\t   c.serviceTypeName,\n" +
            "\t   c.destination dCode,\n" +
            "\t   b.name Destination,\n" +
            "\t   c.weight,\n" +
            "\t   c.totalAmount, c.isPriceComputed,\n" +
            "\t   case when ic.consignmentNumber is null\n" +
            "\t\t\tthen\n" +
            "\t\t\t\t'0'\n" +
            "\t\t\telse '1'\n" +
            "\t\t\tend isInvoiced\n" +
            "\n" +
            "\t   from Consignment c\n" +
            "\n" +
            "\t   inner join Branches b\n" +
            "\t   on c.destination = b.branchCode\n" +
            "\n" +
            "\t   inner join ServiceTypes st\n" +
            "\t   on st.serviceTypeName = c.serviceTypeName\n" +
            "\n" +
            "\t   left outer join ConsignmentModifier cm\n" +
            "\t   on cm.consignmentNumber = c.consignmentNumber\n" +
            "\n" +
            "     left outer join InvoiceConsignment ic\n" +
            "     on cast(ic.consignmentAmount as varchar) = c.consignmentNumber\n" +
            "\n" +
            "     where c.creditClientId = '" + clvar.CustomerClientID + "'\n" +
            "     and st.companyId = '" + clvar.Company + "'\n" +
            "     and c.bookingDate between '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "     order by 2";



            sqlString = "select cc.id,\n" +
            "       z.name Zone,\n" +
            "       bb.name Branch,\n" +
            "       cc.accountNo,\n" +
            "       cc.name 'Client Name',\n" +
            "       cast(c.bookingDate AS DATE) bookingDate,\n" +
            "       c.consignmentNumber ConsignmentNumber,\n" +
            "       c.pieces pieces,\n" +
            "       case\n" +
            "         when c.consignmentTypeId = '13' then\n" +
            "          'Hand Carry'\n" +
            "         else\n" +
            "          c.serviceTypeName\n" +
            "       end as ServiceTypeName,\n" +
            "       b.name Destination,\n" +
            "       c.weight Weight,\n" +
            "       c.totalAmount totalAmount,\n" +
            "       case\n" +
            "         When c.serviceTypeName in ('Road n Rail',\n" +
            "                                    'Flyer',\n" +
            "                                    'NTS',\n" +
            "                                    'HEC',\n" +
            "                                    'Bank to Bank',\n" +
            "                                    'Bulk Shipment',\n" +
            "                                    'overnight',\n" +
            "                                    'Return Service',\n" +
            "                                    'Same Day',\n" +
            "                                    'Second Day',\n" +
            "                                    'Smart Box',\n" +
            "                                    'Sunday & Holiday',\n" +
            "                                    'Smart Box',\n" +
            "                                    'Hand Carry',\n" +
            "                                    'Smart Cargo',\n" +
            "                                    'MB10',\n" +
            "                                    'MB2',\n" +
            "                                    'MB20',\n" +
            "                                    'MB30',\n" +
            "                                    'MB5',\n" +
            "                                    'Aviation Sale') then\n" +
            "          'Domestic'\n" +
            "         When c.serviceTypeName in ('Expressions',\n" +
            "                                    'International Expressions',\n" +
            "                                    'Mango',\n" +
            "                                    'Mango Petty') then\n" +
            "          'Expression'\n" +
            "         When c.serviceTypeName in ('International 5 Percent Discount Tariff Non Doc',\n" +
            "                                    'International 10 Percent Discount Tariff Non Doc',\n" +
            "                                    'International 15 Percent Discount tariff Non Doc',\n" +
            "                                    'International 20 Percent Discount tariff Non Doc',\n" +
            "                                    'International 25 Percent Discount Tariff Non Doc',\n" +
            "                                    'International Cargo',\n" +
            "                                    'International_Box',\n" +
            "                                    'International_Non-Doc',\n" +
            "                                    'International_Non-Doc_Special_Hub_2014',\n" +
            "                                    'Logex') then\n" +
            "          'SAMPLE'\n" +
            "         When c.serviceTypeName in\n" +
            "              ('International Discount Tariff 5 Percent',\n" +
            "               'International 10 Percent Discount tariff',\n" +
            "               'International 15 percent Discount tariff',\n" +
            "               'International 20 Percent Discount tariff',\n" +
            "               'International 25 Percent Discount tariff',\n" +
            "               'International Special Rates from KHI',\n" +
            "               'International Special Rates from Up Country',\n" +
            "               'International Student Package Tariff',\n" +
            "               'International_Doc',\n" +
            "               'International_Doc_Special_Hub') then\n" +
            "          'DOCUMENT'\n" +
            "       END as Product,\n" +
            "       c.isPriceComputed\n" +
            "  from Consignment c\n" +
            " inner join CreditClients cc\n" +
            "    on c.creditClientId = cc.id\n" +
            " inner join Branches bb\n" +
            "    on cc.branchCode = bb.branchCode\n" +
            " INNER JOIN Branches AS b\n" +
            "    ON b.branchCode = c.destination\n" +
            " inner join Zones z\n" +
            "    on bb.zoneCode = z.zoneCode\n" +
            " inner join ServiceTypes st\n" +
            "    on c.serviceTypeName = st.serviceTypeName\n" +
            " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
            "   AND cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "   and st.companyId = '" + clvar.Company + "'";

            sqlString = "select * from (\n" +
           "select ic.consignmentNumber iConsignmentNumber, cc.id,\n" +
           "       z.name Zone,\n" +
           "       bb.name Branch,\n" +
           "       cc.accountNo,\n" +
           "       cc.name 'Client Name',\n" +
           "       cast(c.bookingDate AS DATE) bookingDate,\n" +
           "       c.consignmentNumber ConsignmentNumber,\n" +
           "       c.pieces pieces,\n" +
           "       case\n" +
           "         when c.consignmentTypeId = '13' then\n" +
           "          'Hand Carry'\n" +
           "         else\n" +
           "          c.serviceTypeName\n" +
           "       end as ServiceTypeName,\n" +
           "       b.name Destination,\n" +
           "       c.weight Weight,\n" +
           "       c.totalAmount totalAmount,\n" +
           "       case\n" +
           "         When c.serviceTypeName in ('Road n Rail',\n" +
           "                                    'Flyer',\n" +
           "                                    'NTS',\n" +
           "                                    'HEC',\n" +
           "                                    'Bank to Bank',\n" +
           "                                    'Bulk Shipment',\n" +
           "                                    'overnight',\n" +
           "                                    'Return Service',\n" +
           "                                    'Same Day',\n" +
           "                                    'Second Day',\n" +
           "                                    'Smart Box',\n" +
           "                                    'Sunday & Holiday',\n" +
           "                                    'Smart Box',\n" +
           "                                    'Hand Carry',\n" +
           "                                    'Smart Cargo',\n" +
           "                                    'MB10',\n" +
           "                                    'MB2',\n" +
           "                                    'MB20',\n" +
           "                                    'MB30',\n" +
           "                                    'MB5',\n" +
           "                                    'Aviation Sale') then\n" +
           "          'Domestic'\n" +
           "         When c.serviceTypeName in ('Expressions',\n" +
           "                                    'International Expressions',\n" +
           "                                    'Mango',\n" +
           "                                    'Mango Petty') then\n" +
           "          'Expression'\n" +
           "         When c.serviceTypeName in ('International 5 Percent Discount Tariff Non Doc',\n" +
           "                                    'International 10 Percent Discount Tariff Non Doc',\n" +
           "                                    'International 15 Percent Discount tariff Non Doc',\n" +
           "                                    'International 20 Percent Discount tariff Non Doc',\n" +
           "                                    'International 25 Percent Discount Tariff Non Doc',\n" +
           "                                    'International Cargo',\n" +
           "                                    'International_Box',\n" +
           "                                    'International_Non-Doc',\n" +
           "                                    'International_Non-Doc_Special_Hub_2014',\n" +
           "                                    'Logex') then\n" +
           "          'SAMPLE'\n" +
           "         When c.serviceTypeName in\n" +
           "              ('International Discount Tariff 5 Percent',\n" +
           "               'International 10 Percent Discount tariff',\n" +
           "               'International 15 percent Discount tariff',\n" +
           "               'International 20 Percent Discount tariff',\n" +
           "               'International 25 Percent Discount tariff',\n" +
           "               'International Special Rates from KHI',\n" +
           "               'International Special Rates from Up Country',\n" +
           "               'International Student Package Tariff',\n" +
           "               'International_Doc',\n" +
           "               'International_Doc_Special_Hub') then\n" +
           "          'DOCUMENT'\n" +
           "       END as Product,\n" +
           "       c.isPriceComputed\n" +
           "  from Consignment c\n" +
           " inner join CreditClients cc\n" +
           "    on c.creditClientId = cc.id\n" +
           " inner join Branches bb\n" +
           "    on cc.branchCode = bb.branchCode\n" +
           " INNER JOIN Branches AS b\n" +
           "    ON b.branchCode = c.destination\n" +
           " inner join Zones z\n" +
           "    on bb.zoneCode = z.zoneCode\n" +
           " inner join ServiceTypes st\n" +
           "    on c.serviceTypeName = st.serviceTypeName\n" +
           " left outer join InvoiceConsignment ic\n" +
           " on ic.consignmentNumber = c.consignmentNumber\n" +
           " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
           "   AND cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
           "   and st.companyId = '" + clvar.Company + "'\n" +
           "   ) temp where temp.iConsignmentNumber is null";


            string sql = " \n"
               + "SELECT * \n"
               + "FROM   ( \n"
               + "          SELECT   -- ic.consignmentNumber     iConsignmentNumber, \n"
               + "           cc.id, \n"
               + "           z.name Zone, \n"
               + "           bb.name Branch, \n"
               + "           cc.accountNo, \n"
               + "           cc.name 'Client Name', \n"
               + "           CAST(c.bookingDate AS DATE) bookingDate, \n"
               + "           c.consignmentNumber ConsignmentNumber, \n"
               + "           c.pieces pieces, \n"
               + "           CASE  \n"
               + "                WHEN c.consignmentTypeId = '13' THEN 'Hand Carry' \n"
               + "                ELSE c.serviceTypeName \n"
               + "           END AS ServiceTypeName, \n"
               + "           b.name Destination, \n"
               + "           c.weight WEIGHT, \n"
               + "           c.totalAmount totalAmount, \n"
               + "           CASE  \n"
               + "                WHEN c.serviceTypeName IN ('Road n Rail', 'Flyer', 'NTS', 'HEC',  \n"
               + "                                          'Bank to Bank', 'Bulk Shipment',  \n"
               + "                                          'overnight', 'Return Service',  \n"
               + "                                          'Same Day', 'Second Day', 'Smart Box',  \n"
               + "                                          'Sunday & Holiday', 'Smart Box',  \n"
               + "                                          'Hand Carry', 'Smart Cargo', 'MB10',  \n"
               + "                                          'MB2', 'MB20', 'MB30', 'MB5',  \n"
               + "                                          'Aviation Sale') THEN 'Domestic' \n"
               + "                WHEN c.serviceTypeName IN ('Expressions',  \n"
               + "                                          'International Expressions', 'Mango',  \n"
               + "                                          'Mango Petty') THEN 'Expression' \n"
               + "                WHEN c.serviceTypeName IN ( \n"
               + "                                          'International 5 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International 10 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International 15 Percent Discount tariff Non Doc',  \n"
               + "                                          'International 20 Percent Discount tariff Non Doc',  \n"
               + "                                          'International 25 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International Cargo',  \n"
               + "                                          'International_Box',  \n"
               + "                                          'International_Non-Doc',  \n"
               + "                                          'International_Non-Doc_Special_Hub_2014',  \n"
               + "                                          'Logex') THEN 'SAMPLE' \n"
               + "                WHEN c.serviceTypeName IN ( \n"
               + "                                          'International Discount Tariff 5 Percent',  \n"
               + "                                          'International 10 Percent Discount tariff',  \n"
               + "                                          'International 15 percent Discount tariff',  \n"
               + "                                          'International 20 Percent Discount tariff',  \n"
               + "                                          'International 25 Percent Discount tariff',  \n"
               + "                                          'International Special Rates from KHI',  \n"
               + "                                          'International Special Rates from Up Country',  \n"
               + "                                          'International Student Package Tariff',  \n"
               + "                                          'International_Doc',  \n"
               + "                                          'International_Doc_Special_Hub') THEN  \n"
               + "                     'DOCUMENT' \n"
               + "           END AS Product, \n"
               + "           c.isPriceComputed \n"
               + "           FROM Consignment c \n"
               + "           INNER JOIN CreditClients cc \n"
               + "           ON c.creditClientId = cc.id \n"
               + "           INNER JOIN Branches bb \n"
               + "           ON cc.branchCode = bb.branchCode \n"
               + "           INNER JOIN Branches AS b \n"
               + "           ON b.branchCode = c.destination \n"
               + "           INNER JOIN Zones z \n"
               + "           ON bb.zoneCode = z.zoneCode \n"
               + "           INNER JOIN ServiceTypes st \n"
               + "           ON c.serviceTypeName = st.serviceTypeName \n"
               + "            \n"
               + "           WHERE cc.accountNo = '" + clvar.AccountNo + "' \n"
               + "           AND cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' \n"
               + "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n"
               + "   and st.companyId = '" + clvar.Company + "'\n"
               + "       ) temp \n"
               + "       LEFT OUTER JOIN InvoiceConsignment ic \n"
               + "            ON  ic.consignmentNumber = temp.consignmentNumber \n"
               + "       LEFT OUTER JOIN Invoice i \n"
               + "            ON  ic.invoiceNumber = i.invoiceNumber \n"
               + "            AND isnull(i.IsInvoiceCanceled,'0') = '0'";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);


            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetCODConsignmentsForInvoice(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string sqlString = "select substring(CAST(CONVERT(date, c.bookingdate, 105) as varchar),0,10) BookingDate,\n" +
            "\t   c.consignmentNumber,\n" +
            "\t   c.pieces,\n" +
            "\t   c.serviceTypeName,\n" +
            "\t   c.destination dCode,\n" +
            "\t   b.name Destination,\n" +
            "\t   c.weight,\n" +
            "\t   c.totalAmount, c.isPriceComputed,\n" +
            "\t   case when ic.consignmentNumber is null\n" +
            "\t\t\tthen\n" +
            "\t\t\t\t'0'\n" +
            "\t\t\telse '1'\n" +
            "\t\t\tend isInvoiced\n" +
            "\n" +
            "\t   from Consignment c\n" +
            "\n" +
            "\t   inner join Branches b\n" +
            "\t   on c.destination = b.branchCode\n" +
            "\n" +
            "\t   inner join ServiceTypes st\n" +
            "\t   on st.serviceTypeName = c.serviceTypeName\n" +
            "\n" +
            "\t   left outer join ConsignmentModifier cm\n" +
            "\t   on cm.consignmentNumber = c.consignmentNumber\n" +
            "\n" +
            "     left outer join InvoiceConsignment ic\n" +
            "     on cast(ic.consignmentAmount as varchar) = c.consignmentNumber\n" +
            "\n" +
            "     where c.creditClientId = '" + clvar.CustomerClientID + "'\n" +
            "     and st.companyId = '" + clvar.Company + "'\n" +
            "     and c.bookingDate between '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "     order by 2";



            sqlString = "select cc.id,\n" +
            "       z.name Zone,\n" +
            "       bb.name Branch,\n" +
            "       cc.accountNo,\n" +
            "       cc.name 'Client Name',\n" +
            "       cast(c.bookingDate AS DATE) bookingDate,\n" +
            "       c.consignmentNumber ConsignmentNumber,\n" +
            "       c.pieces pieces,\n" +
            "       case\n" +
            "         when c.consignmentTypeId = '13' then\n" +
            "          'Hand Carry'\n" +
            "         else\n" +
            "          c.serviceTypeName\n" +
            "       end as ServiceTypeName,\n" +
            "       b.name Destination,\n" +
            "       c.weight Weight,\n" +
            "       c.totalAmount totalAmount,\n" +
            "       case\n" +
            "         When c.serviceTypeName in ('Road n Rail',\n" +
            "                                    'Flyer',\n" +
            "                                    'NTS',\n" +
            "                                    'HEC',\n" +
            "                                    'Bank to Bank',\n" +
            "                                    'Bulk Shipment',\n" +
            "                                    'overnight',\n" +
            "                                    'Return Service',\n" +
            "                                    'Same Day',\n" +
            "                                    'Second Day',\n" +
            "                                    'Smart Box',\n" +
            "                                    'Sunday & Holiday',\n" +
            "                                    'Smart Box',\n" +
            "                                    'Hand Carry',\n" +
            "                                    'Smart Cargo',\n" +
            "                                    'MB10',\n" +
            "                                    'MB2',\n" +
            "                                    'MB20',\n" +
            "                                    'MB30',\n" +
            "                                    'MB5',\n" +
            "                                    'Aviation Sale') then\n" +
            "          'Domestic'\n" +
            "         When c.serviceTypeName in ('Expressions',\n" +
            "                                    'International Expressions',\n" +
            "                                    'Mango',\n" +
            "                                    'Mango Petty') then\n" +
            "          'Expression'\n" +
            "         When c.serviceTypeName in ('International 5 Percent Discount Tariff Non Doc',\n" +
            "                                    'International 10 Percent Discount Tariff Non Doc',\n" +
            "                                    'International 15 Percent Discount tariff Non Doc',\n" +
            "                                    'International 20 Percent Discount tariff Non Doc',\n" +
            "                                    'International 25 Percent Discount Tariff Non Doc',\n" +
            "                                    'International Cargo',\n" +
            "                                    'International_Box',\n" +
            "                                    'International_Non-Doc',\n" +
            "                                    'International_Non-Doc_Special_Hub_2014',\n" +
            "                                    'Logex') then\n" +
            "          'SAMPLE'\n" +
            "         When c.serviceTypeName in\n" +
            "              ('International Discount Tariff 5 Percent',\n" +
            "               'International 10 Percent Discount tariff',\n" +
            "               'International 15 percent Discount tariff',\n" +
            "               'International 20 Percent Discount tariff',\n" +
            "               'International 25 Percent Discount tariff',\n" +
            "               'International Special Rates from KHI',\n" +
            "               'International Special Rates from Up Country',\n" +
            "               'International Student Package Tariff',\n" +
            "               'International_Doc',\n" +
            "               'International_Doc_Special_Hub') then\n" +
            "          'DOCUMENT'\n" +
            "       END as Product,\n" +
            "       c.isPriceComputed\n" +
            "  from Consignment c\n" +
            " inner join CreditClients cc\n" +
            "    on c.creditClientId = cc.id\n" +
            " inner join Branches bb\n" +
            "    on cc.branchCode = bb.branchCode\n" +
            " INNER JOIN Branches AS b\n" +
            "    ON b.branchCode = c.destination\n" +
            " inner join Zones z\n" +
            "    on bb.zoneCode = z.zoneCode\n" +
            " inner join ServiceTypes st\n" +
            "    on c.serviceTypeName = st.serviceTypeName\n" +
            " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
            "   AND cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "   and st.companyId = '" + clvar.Company + "'";

            sqlString = "select * from (\n" +
           "select ic.consignmentNumber iConsignmentNumber, cc.id,\n" +
           "       z.name Zone,\n" +
           "       bb.name Branch,\n" +
           "       cc.accountNo,\n" +
           "       cc.name 'Client Name',\n" +
           "       cast(c.bookingDate AS DATE) bookingDate,\n" +
           "       c.consignmentNumber ConsignmentNumber,\n" +
           "       c.pieces pieces,\n" +
           "       case\n" +
           "         when c.consignmentTypeId = '13' then\n" +
           "          'Hand Carry'\n" +
           "         else\n" +
           "          c.serviceTypeName\n" +
           "       end as ServiceTypeName,\n" +
           "       b.name Destination,\n" +
           "       c.weight Weight,\n" +
           "       c.totalAmount totalAmount,\n" +
           "       case\n" +
           "         When c.serviceTypeName in ('Road n Rail',\n" +
           "                                    'Flyer',\n" +
           "                                    'NTS',\n" +
           "                                    'HEC',\n" +
           "                                    'Bank to Bank',\n" +
           "                                    'Bulk Shipment',\n" +
           "                                    'overnight',\n" +
           "                                    'Return Service',\n" +
           "                                    'Same Day',\n" +
           "                                    'Second Day',\n" +
           "                                    'Smart Box',\n" +
           "                                    'Sunday & Holiday',\n" +
           "                                    'Smart Box',\n" +
           "                                    'Hand Carry',\n" +
           "                                    'Smart Cargo',\n" +
           "                                    'MB10',\n" +
           "                                    'MB2',\n" +
           "                                    'MB20',\n" +
           "                                    'MB30',\n" +
           "                                    'MB5',\n" +
           "                                    'Aviation Sale') then\n" +
           "          'Domestic'\n" +
           "         When c.serviceTypeName in ('Expressions',\n" +
           "                                    'International Expressions',\n" +
           "                                    'Mango',\n" +
           "                                    'Mango Petty') then\n" +
           "          'Expression'\n" +
           "         When c.serviceTypeName in ('International 5 Percent Discount Tariff Non Doc',\n" +
           "                                    'International 10 Percent Discount Tariff Non Doc',\n" +
           "                                    'International 15 Percent Discount tariff Non Doc',\n" +
           "                                    'International 20 Percent Discount tariff Non Doc',\n" +
           "                                    'International 25 Percent Discount Tariff Non Doc',\n" +
           "                                    'International Cargo',\n" +
           "                                    'International_Box',\n" +
           "                                    'International_Non-Doc',\n" +
           "                                    'International_Non-Doc_Special_Hub_2014',\n" +
           "                                    'Logex') then\n" +
           "          'SAMPLE'\n" +
           "         When c.serviceTypeName in\n" +
           "              ('International Discount Tariff 5 Percent',\n" +
           "               'International 10 Percent Discount tariff',\n" +
           "               'International 15 percent Discount tariff',\n" +
           "               'International 20 Percent Discount tariff',\n" +
           "               'International 25 Percent Discount tariff',\n" +
           "               'International Special Rates from KHI',\n" +
           "               'International Special Rates from Up Country',\n" +
           "               'International Student Package Tariff',\n" +
           "               'International_Doc',\n" +
           "               'International_Doc_Special_Hub') then\n" +
           "          'DOCUMENT'\n" +
           "       END as Product,\n" +
           "       c.isPriceComputed\n" +
           "  from Consignment c\n" +
           " inner join CreditClients cc\n" +
           "    on c.creditClientId = cc.id\n" +
           " inner join Branches bb\n" +
           "    on cc.branchCode = bb.branchCode\n" +
           " INNER JOIN Branches AS b\n" +
           "    ON b.branchCode = c.destination\n" +
           " inner join Zones z\n" +
           "    on bb.zoneCode = z.zoneCode\n" +
           " inner join ServiceTypes st\n" +
           "    on c.serviceTypeName = st.serviceTypeName\n" +
           " left outer join InvoiceConsignment ic\n" +
           " on ic.consignmentNumber = c.consignmentNumber\n" +
           " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
           "   AND cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
           "   and st.companyId = '" + clvar.Company + "'\n" +
           "   ) temp where temp.iConsignmentNumber is null";


            string sql = " \n"
               + "SELECT * \n"
               + "FROM   ( \n"
               + "          SELECT   -- ic.consignmentNumber     iConsignmentNumber, \n"
               + "           cc.id, \n"
               + "           z.name Zone, \n"
               + "           bb.name Branch, \n"
               + "           cc.accountNo, \n"
               + "           cc.name 'Client Name', \n"
               + "           CAST(c.bookingDate AS DATE) bookingDate, \n"
               + "           c.consignmentNumber ConsignmentNumber, \n"
               + "           c.pieces pieces, \n"
               + "           CASE  \n"
               + "                WHEN c.consignmentTypeId = '13' THEN 'Hand Carry' \n"
               + "                ELSE c.serviceTypeName \n"
               + "           END AS ServiceTypeName, \n"
               + "           b.name Destination, \n"
               + "           c.weight WEIGHT, \n"
               + "           c.totalAmount totalAmount, \n"
               + "           CASE  \n"
               + "                WHEN c.serviceTypeName IN ('Road n Rail', 'Flyer', 'NTS', 'HEC',  \n"
               + "                                          'Bank to Bank', 'Bulk Shipment',  \n"
               + "                                          'overnight', 'Return Service',  \n"
               + "                                          'Same Day', 'Second Day', 'Smart Box',  \n"
               + "                                          'Sunday & Holiday', 'Smart Box',  \n"
               + "                                          'Hand Carry', 'Smart Cargo', 'MB10',  \n"
               + "                                          'MB2', 'MB20', 'MB30', 'MB5',  \n"
               + "                                          'Aviation Sale') THEN 'Domestic' \n"
               + "                WHEN c.serviceTypeName IN ('Expressions',  \n"
               + "                                          'International Expressions', 'Mango',  \n"
               + "                                          'Mango Petty') THEN 'Expression' \n"
               + "                WHEN c.serviceTypeName IN ( \n"
               + "                                          'International 5 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International 10 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International 15 Percent Discount tariff Non Doc',  \n"
               + "                                          'International 20 Percent Discount tariff Non Doc',  \n"
               + "                                          'International 25 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International Cargo',  \n"
               + "                                          'International_Box',  \n"
               + "                                          'International_Non-Doc',  \n"
               + "                                          'International_Non-Doc_Special_Hub_2014',  \n"
               + "                                          'Logex') THEN 'SAMPLE' \n"
               + "                WHEN c.serviceTypeName IN ( \n"
               + "                                          'International Discount Tariff 5 Percent',  \n"
               + "                                          'International 10 Percent Discount tariff',  \n"
               + "                                          'International 15 percent Discount tariff',  \n"
               + "                                          'International 20 Percent Discount tariff',  \n"
               + "                                          'International 25 Percent Discount tariff',  \n"
               + "                                          'International Special Rates from KHI',  \n"
               + "                                          'International Special Rates from Up Country',  \n"
               + "                                          'International Student Package Tariff',  \n"
               + "                                          'International_Doc',  \n"
               + "                                          'International_Doc_Special_Hub') THEN  \n"
               + "                     'DOCUMENT' \n"
               + "           END AS Product, \n"
               + "           c.isPriceComputed \n"
               + "           FROM Consignment c \n"
               + "           INNER JOIN CreditClients cc \n"
               + "           ON c.creditClientId = cc.id \n"
               + "           INNER JOIN Branches bb \n"
               + "           ON cc.branchCode = bb.branchCode \n"
               + "           INNER JOIN Branches AS b \n"
               + "           ON b.branchCode = c.destination \n"
               + "           INNER JOIN Zones z \n"
               + "           ON bb.zoneCode = z.zoneCode \n"
               + "           INNER JOIN ServiceTypes st \n"
               + "           ON c.serviceTypeName = st.serviceTypeName \n"
               + "            \n"
               + "           WHERE cc.accountNo = '" + clvar.AccountNo + "' \n"
               + "           AND cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and c.cod = '1'\n"
               + "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n"
               + "   and st.companyId = '" + clvar.Company + "'\n"
               + "       ) temp \n"
               + "       LEFT OUTER JOIN InvoiceConsignment ic \n"
               + "            ON  ic.consignmentNumber = temp.consignmentNumber \n"
               + "       LEFT OUTER JOIN Invoice i \n"
               + "            ON  ic.invoiceNumber = i.invoiceNumber \n"
               + "            AND isnull(i.IsInvoiceCanceled,'0') = '0'";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);


            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public string GenerateManualInvoice(Cl_Variables clvar)
        {
            string invoiceNumber = "";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MnP_spGenerateManualInvoices", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                IDbDataParameter InvoiceNumber = sqlcmd.CreateParameter();
                InvoiceNumber.ParameterName = "@InvoiceNumber_";
                InvoiceNumber.Direction = System.Data.ParameterDirection.Output;
                InvoiceNumber.DbType = System.Data.DbType.String;
                InvoiceNumber.Size = 50;
                sqlcmd.Parameters.Add(InvoiceNumber);

                sqlcmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                sqlcmd.Parameters.AddWithValue("@DateFrom", clvar.FromDate.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@DateTo", clvar.ToDate.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@InvoiceDate", clvar.LoadingDate);
                sqlcmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["U_ID"].ToString());
                sqlcmd.Parameters.AddWithValue("@ClientId", clvar.CreditClientID);
                sqlcmd.Parameters.AddWithValue("@CompanyId", clvar.Company);

                sqlcmd.ExecuteNonQuery();
                invoiceNumber = InvoiceNumber.Value.ToString();

            }
            catch (Exception ex)
            { return ex.Message; }
            finally { sqlcon.Close(); }

            return invoiceNumber;
        }
        public string GenerateManualCODInvoice(Cl_Variables clvar)
        {
            string invoiceNumber = "";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("spGenerateCODInvoice_COD", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                IDbDataParameter InvoiceNumber = sqlcmd.CreateParameter();
                InvoiceNumber.ParameterName = "@InvoiceNumber";
                InvoiceNumber.Direction = System.Data.ParameterDirection.Output;
                InvoiceNumber.DbType = System.Data.DbType.String;
                InvoiceNumber.Size = 50;
                sqlcmd.Parameters.Add(InvoiceNumber);

                sqlcmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                sqlcmd.Parameters.AddWithValue("@DateFrom", clvar.FromDate.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@DateTo", clvar.ToDate.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@InvoiceDate", clvar.LoadingDate);
                sqlcmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["U_ID"].ToString());
                sqlcmd.Parameters.AddWithValue("@CreatedById", HttpContext.Current.Session["U_ID"].ToString());
                sqlcmd.Parameters.AddWithValue("@ClientId", clvar.CreditClientID);
                sqlcmd.Parameters.AddWithValue("@CompanyId", clvar.Company);

                sqlcmd.ExecuteNonQuery();
                invoiceNumber = InvoiceNumber.Value.ToString();

            }
            catch (Exception ex)
            { return ex.Message; }
            finally { sqlcon.Close(); }

            return invoiceNumber;
        }


        public DataSet Check_CODConsignmentDetail(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select * from CODConsignmentDetail where consignmentNumber = '" + clvar.consignmentNo + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
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




        public int[] InsertRnRTariff(Cl_Variables clvar, DataTable dt)
        {
            int[] count = new int[3];
            string query1 = "";
            string query2 = "";
            string query3 = "";
            string query4 = "";
            string query5 = "";
            string query6 = "";
            int insertCount = 0;
            int updateCount = 0;
            int deleteCount = 0;
            DataRow[] drInsert = dt.Select("ISUPDATED = 'INSERT'", "");
            insertCount = drInsert.Count();
            DataRow[] drDelete = dt.Select("ISUPDATED = 'DELETE'", "");
            deleteCount = drDelete.Count();
            DataRow[] drUpdate = dt.Select("ISUPDATED = 'UPDATE'", "");
            updateCount = drUpdate.Count();

            #region Insertion Query For New Records
            if (drInsert.Count() > 0)
            {
                query1 = "INSERT INTO RnR_Tarrif (Client_ID, FromCatID, ToCatID, IsDefault, Value)";
                for (int i = 0; i < drInsert.Count() - 1; i++)
                {
                    query1 += "SELECT '" + drInsert[i]["ClientID"].ToString() + "', " +
                                  "       '" + drInsert[i]["FromCatID"].ToString() + "', " +
                                  "       '" + drInsert[i]["TOCatID"].ToString() + "', " +
                                  "       '0', " +
                                  "       '" + drInsert[i]["Price"].ToString() + " '\n" +
                                  "UNION ALL \n";
                }
                int j = drInsert.Count() - 1;
                query1 += "SELECT '" + drInsert[j]["ClientID"].ToString() + "', " +
                                  "       '" + drInsert[j]["FromCatID"].ToString() + "', " +
                                  "       '" + drInsert[j]["TOCatID"].ToString() + "', " +
                                  "       '0', " +
                                  "       '" + drInsert[j]["Price"].ToString() + "' \n";
            }
            else
            {
                insertCount = 0;
            }
            #endregion

            #region Delete Query
            if (drDelete.Count() > 0)
            {

                query2 = "DELETE FROM RnR_Tarrif WHERE ID in (";
                for (int i = 0; i < drDelete.Count(); i++)
                {
                    query2 += "'" + drDelete[i]["ID"].ToString() + "',";
                }
                query2 = query2.TrimEnd(',') + ")";
            }
            else
            {
                deleteCount = 0;
            }
            #endregion

            #region Update Query
            if (drUpdate.Count() > 0)
            {
                query3 = "CREATE TABLE [DBO].[RNR_TARRIF_TEMP](\n" +
                           "\t[ID] [BIGINT] NOT NULL,\n" +
                           "\t[CLIENT_ID] [BIGINT] NOT NULL,\n" +
                           "\t[FROMCATID] [VARCHAR](50) NULL,\n" +
                           "\t[TOCATID] [VARCHAR](50) NULL,\n" +
                           "\t[ISDEFAULT] [BIT] NULL,\n" +
                           "\t[VALUE] [FLOAT] NULL\n" +
                           ")";

                query4 = "INSERT INTO RNR_TARRIF_TEMP (ID, Client_ID, FromCatID, ToCatID, IsDefault, Value)\n";
                for (int i = 0; i < drUpdate.Count() - 1; i++)
                {
                    query4 += "SELECT     '" + drUpdate[i]["ID"].ToString() + "', " +
                                  "       '" + drUpdate[i]["ClientID"].ToString() + "', " +
                                  "       '" + drUpdate[i]["FromCatID"].ToString() + "', " +
                                  "       '" + drUpdate[i]["TOCatID"].ToString() + "', " +
                                  "       '0', " +
                                  "       '" + drUpdate[i]["Price"].ToString() + "'\n" +
                                  "UNION ALL \n";
                }
                int j = drUpdate.Count() - 1;
                query4 += "SELECT         '" + drUpdate[j]["ID"].ToString() + "', " +
                                  "       '" + drUpdate[j]["ClientID"].ToString() + "', " +
                                  "       '" + drUpdate[j]["FromCatID"].ToString() + "', " +
                                  "       '" + drUpdate[j]["TOCatID"].ToString() + "', " +
                                  "       '0', " +
                                  "       '" + drUpdate[j]["Price"].ToString() + "'\n";
                query5 = " UPDATE RnR_Tarrif\n" +
                   "   SET CLIENT_ID = T.CLIENT_ID,\n" +
                   "       FROMCATID = T.FROMCATID,\n" +
                   "       TOCATID   = T.TOCATID,\n" +
                   "       ISDEFAULT = T.ISDEFAULT,\n" +
                   "       VALUE     = T.VALUE FROM RNR_TARRIF_TEMP T\n" +
                   " WHERE RnR_Tarrif.ID = T.ID";

                query6 = "DROP TABLE RNR_TARRIF_TEMP";

            }
            else
            {
                updateCount = 0;
            }
            #endregion




            #region MyRegion
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["ISUPDATED"].ToString() == "INSERT")
            //    {
            //        if (i < dt.Rows.Count - 1)
            //        {
            //            query1 += "SELECT '" + dt.Rows[i]["ClientID"].ToString() + "', " +
            //                      "       '" + dt.Rows[i]["FromCatID"].ToString() + "', " +
            //                      "       '" + dt.Rows[i]["TOCatID"].ToString() + "', " +
            //                      "       '0', " +
            //                      "       '" + dt.Rows[i]["Price"].ToString() + " '\n" +
            //                      "UNION ALL \n";
            //        }
            //    }
            //    if (dt.Rows[i]["ISUPDATED"].ToString() == "DELETE")
            //    {

            //    }
            //    if (dt.Rows[i]["ISUPDATED"].ToString() == "UPDATE")
            //    {
            //        if (i < dt.Rows.Count - 1)
            //        {

            //        }
            //    }
            //}
            //int j = dt.Rows.Count - 1;
            //if (dt.Rows[j]["ISUPDATED"].ToString() == "INSERT")
            //{

            //}
            //if (dt.Rows[j]["ISUPDATED"].ToString() == "UPDATE")
            //{

            //} 
            #endregion






            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                if (insertCount != 0)
                {
                    sqlcmd.CommandText = query1;
                    count[0] = sqlcmd.ExecuteNonQuery();
                }
                else
                {
                    count[0] = 0;
                }
                if (deleteCount != 0)
                {
                    sqlcmd.CommandText = query2;
                    count[1] = sqlcmd.ExecuteNonQuery();
                }
                else
                {
                    count[1] = 0;
                }
                if (updateCount != 0)
                {
                    sqlcmd.CommandText = query3;
                    sqlcmd.ExecuteNonQuery();
                    sqlcmd.CommandText = query4;
                    sqlcmd.ExecuteNonQuery();
                    sqlcmd.CommandText = query5;
                    count[2] = sqlcmd.ExecuteNonQuery();
                    sqlcmd.CommandText = query6;
                    sqlcmd.ExecuteNonQuery();
                }
                else
                {
                    count[2] = 0;
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                count[0] = 0;
                count[1] = 0;
                count[2] = 0;
            }
            finally
            {
                sqlcon.Close();
            }

            return count;
        }




        public int UpdateTarrif_(Cl_Variables clvar, List<DataRow> dr)
        {
            int count = 0;
            string IDs = "";
            foreach (DataRow row in dr)
            {
                IDs += "'" + row["ID"].ToString() + "'";
            }
            IDs = IDs.Replace("''", "','");
            string query = "UPDATE TempClientTariff set ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', ModifiedOn = GETDATE(), AdditionalFactor = '" + dr[0]["AddFactor"].ToString() + "' where id in (" + IDs + ")";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;
                count = sqlcmd.ExecuteNonQuery();
                //sqlcmd = new SqlCommand(query1, sqlcon);
                //sqlcmd.ExecuteNonQuery();
                //sqlcmd = new SqlCommand(query2, sqlcon);
                //count = sqlcmd.ExecuteNonQuery();
                //sqlcmd = new SqlCommand(query3, sqlcon);
                //sqlcmd.ExecuteNonQuery();

                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                //error = ex.Message;
            }
            return count;

        }

        //Addition IN COnsignment

        public string Add_Consignment_Validation(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            //    obj.Call_Log_ID = int.Parse(Get_maxValue()) + 1;
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp_Validation", sqlcon);
                // SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                // SqlParameter SParam = new SqlParameter("ReturnValue", SqlDbType.Int);
                // SParam.Direction = ParameterDirection.ReturnValue;
                //
                sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                sqlcmd.Parameters.AddWithValue("@consigner", obj.Consigner);
                sqlcmd.Parameters.AddWithValue("@consignee", obj.Consignee);
                sqlcmd.Parameters.AddWithValue("@couponNumber", obj.CouponNo);
                sqlcmd.Parameters.AddWithValue("@customerType", obj.Customertype);
                sqlcmd.Parameters.AddWithValue("@orgin", obj.origin);
                sqlcmd.Parameters.AddWithValue("@destination", obj.Destination);
                sqlcmd.Parameters.AddWithValue("@pieces", obj.pieces);
                sqlcmd.Parameters.AddWithValue("@serviceTypeName", obj.ServiceTypeName);
                sqlcmd.Parameters.AddWithValue("@creditClientId", obj.CustomerClientID);
                sqlcmd.Parameters.AddWithValue("@consignmentTypeId", obj.Con_Type);
                sqlcmd.Parameters.AddWithValue("@weight", obj.Weight);
                sqlcmd.Parameters.AddWithValue("@weightUnit", obj.Unit);
                sqlcmd.Parameters.AddWithValue("@discount", obj.Discount);
                sqlcmd.Parameters.AddWithValue("@cod", obj.isCod);
                sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                sqlcmd.Parameters.AddWithValue("@status", obj.status);
                sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", obj.isCreatedFromMMUs);
                sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                sqlcmd.Parameters.AddWithValue("@remarks", obj.Remarks);
                sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                sqlcmd.Parameters.AddWithValue("@gst", obj.gst);
                sqlcmd.Parameters.AddWithValue("@width", obj.width);
                sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                sqlcmd.Parameters.AddWithValue("@height", obj.height);
                sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", obj.expressionGreetingCard);
                sqlcmd.Parameters.AddWithValue("@expressionMessage", obj.expressionMessage);
                sqlcmd.Parameters.AddWithValue("@consigneePhoneNo", obj.ConsigneeCell);
                sqlcmd.Parameters.AddWithValue("@expressionconsignmentRefNumber", obj.expressionconsignmentRefNumber);
                sqlcmd.Parameters.AddWithValue("@otherCharges", obj.Othercharges);
                sqlcmd.Parameters.AddWithValue("@routeCode", obj.routeCode);
                sqlcmd.Parameters.AddWithValue("@docPouchNo", obj.docPouchNo);
                sqlcmd.Parameters.AddWithValue("@consignerPhoneNo", obj.ConsignerPhone);
                sqlcmd.Parameters.AddWithValue("@consignerCellNo", obj.ConsignerCell);
                sqlcmd.Parameters.AddWithValue("@consignerCNICNo", obj.ConsignerCNIC);
                sqlcmd.Parameters.AddWithValue("@consignerAccountNo", obj.consignerAccountNo);
                sqlcmd.Parameters.AddWithValue("@consignerEmail", obj.consignerEmail);
                sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", obj.docIsHomeDelivery);
                sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", "0");
                sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.Bookingdate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@receivedFromRider", obj.receivedFromRider);
                sqlcmd.Parameters.AddWithValue("@chargedAmount", obj.ChargeAmount);
                sqlcmd.Parameters.AddWithValue("@cnState", obj.cnState);
                sqlcmd.Parameters.AddWithValue("@cardRefNo", obj.cardRefNo);
                sqlcmd.Parameters.AddWithValue("@manifestNo", obj.manifestNo);
                sqlcmd.Parameters.AddWithValue("@manifestId", obj.manifestId);
                sqlcmd.Parameters.AddWithValue("@originBrName", obj.originBrName);
                sqlcmd.Parameters.AddWithValue("@orderRefNo", obj.orderRefNo);
                sqlcmd.Parameters.AddWithValue("@customerName", obj.customerName);
                sqlcmd.Parameters.AddWithValue("@productTypeId", obj.productTypeId);
                sqlcmd.Parameters.AddWithValue("@productDescription", obj.productDescription);
                sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                sqlcmd.Parameters.AddWithValue("@cnScreenId", obj.cnScreenId);
                sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);

                //SqlParameter P_XCode = new SqlParameter("@XCode", SqlDbType.VarChar, 256);
                //P_XCode.Direction = ParameterDirection.Output;

                //  sqlcmd.Parameters.Add(P_XCode);
                // sqlcmd.Parameters.Add(SParam);
                sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
        }

        public DataTable Add_OcsValidation(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_OCS_Validation", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds.Tables[1];
        }

        public DataTable Add_OcsValidation_Actual(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_OCS_Validation_Actual", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.Parameters.AddWithValue("@weight", clvar.Weight);

                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds.Tables[1];
        }

        public DataTable Add_RNRValidation(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_RNR_Validation", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds.Tables[0];
        }

        public DataTable Add_RNRValidation_Actual(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_RNR_Validation_Actual", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.Parameters.AddWithValue("@weight", clvar.Weight);

                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds.Tables[0];
        }

        public DataTable Add_intValidation_Actual(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_Int_Validation", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.Parameters.AddWithValue("@weight", clvar.Weight);

                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds.Tables[0];
        }

        public string InsertTrackingFromBagging(Variable clvar_, Cl_Variables clvar)
        {


            string query = "";
            //clvar.manifestNo = clvar_.Manifest;
            clvar.manifestNo = clvar_.Manifest;
            DataTable dt = GetConsignmentDetailByManifestNumber(clvar);
            if (dt.Rows.Count > 0)
            {


                query += "Insert into ConsignmentsTrackingHistory (ConsignmentNumber, stateID, currentLocation, manifestNumber, bagnumber, transactionTime, SealNo)";

                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    query += "Select '" + dt.Rows[i]["consignmentNumber"].ToString() + "', '3', '" + clvar_._CityCode + "', " + clvar_._Manifest + ", '" + clvar_._BagNumber + "', GETDATE(), '" + clvar_.Seal + "' UNION ALL\n";
                }
                int j = dt.Rows.Count - 1;
                query += "Select '" + dt.Rows[j]["consignmentNumber"].ToString() + "', '3', '" + clvar_._CityCode + "', " + clvar_._Manifest + ", '" + clvar_._BagNumber + "', GETDATE(), '" + clvar_.Seal + "' \n";
                SqlConnection con = new SqlConnection(clvar.Strcon());
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                { return ex.Message; }
                finally { con.Close(); }
                return "OK";
            }
            else
            {
                return "NOT OK";
            }
        }

        public string InsertConsignmentsFromRunsheet_mm(Cl_Variables clvar, DataTable dt)
        {
            string trackQuery = "";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;

            //" ) ";
            int count = 0;
            string check = "";
            string query = "";
            string query1 = "";

            //query = " INSERT INTO CONSIGNMENT (ConsignmentNumber, Orgin, Destination, CreditClientId, ConsignerAccountNo, weight , pieces, syncid,bookingDate,createdOn) ";
            //foreach (DataRow row in dt.Rows)
            //{
            //    query1 += "";

            //}
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    query = " INSERT INTO CONSIGNMENT (ConsignmentNumber, Orgin, Destination, CreditClientId, ConsignerAccountNo, weight , pieces, syncid,bookingDate,createdOn,zoneCode, branchCode, serviceTypeName, consignmentTypeId) ";
                    query += " Values( '" + dt.Rows[i]["ConNo"].ToString() + "', '" + dt.Rows[i]["Origin"].ToString() + "', '" + dt.Rows[i]["Name"].ToString() + "', '" + clvar.CustomerClientID + "', '" + clvar.AccountNo + "', '" + clvar.Weight + "', '" + clvar.pieces + "' , NewID(), GETDATE(), GETDATE(), '" + HttpContext.Current.Session["zonecode"].ToString() + "',\n" +
                                "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "', '" + clvar.ServiceTypeName + "','12')";

                    sqlcmd.CommandText = query;
                    sqlcmd.ExecuteNonQuery();

                    trackQuery = "insert into ConsignmentsTrackingHistory \n" +
                                   "  (consignmentNumber, stateID, currentLocation, transactionTime)\n";
                    trackQuery += " VALUES(   '" + dt.Rows[i]["CONNO"].ToString() + "',\n" +
                                   "   '1',\n" +
                                   "   '" + dt.Rows[i]["CITYCODE"].ToString() + "',\n" +
                                   "   GETDATE()\n )";
                    sqlcmd.CommandText = trackQuery;
                    sqlcmd.ExecuteNonQuery();


                    trans.Commit();
                }
                catch (Exception ex)
                {

                    //throw;
                }
                //query += " SELECT '" + dt.Rows[i]["ConNo"].ToString() + "', '" + dt.Rows[i]["Origin"].ToString() + "', '" + dt.Rows[i]["Name"].ToString() + "', '" + clvar.CustomerClientID + "', '" + clvar.AccountNo + "', '" + clvar.Weight + "', '" + clvar.pieces + "' , NewID(), GETDATE(), GETDATE()\n" +
                //        " UNION ALL";

            }

            sqlcon.Close();



            return "OK";
        }


        public int GetCNCountForDayEnd()
        {
            string query = "select \n" +
                "           COUNT(c.consignmentNumber)\n" +
                "           FROM Consignment c\n" +
                "         where CONVERT(date, c.createdon, 105) = '" + HttpContext.Current.Session["WorkingDate"].ToString() + "'\n" +
                "           and c.originexpresscenter = '" + HttpContext.Current.Session["Expresscenter"].ToString() + "'\n";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd); orcd.CommandTimeout = 400;
                oda.Fill(dt);

                orcl.Close();
            }
            catch (Exception)
            { }
            int count = 0;
            int.TryParse(dt.Rows[0][0].ToString(), out count);
            return count;

        }

        public string SavePODSmsEntry(string cn, string cellNo)
        {
            string query = "insert into MNP_PodSmsDelivery (ConsignmentNo, CellNo, createdon, createdBy) Values ('" + cn + "','" + cellNo + "', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "')";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                con.Close();
                return ex.Message;
            }
            return "OK";
        }

        public string SavePODSmsEntry(string cn, string cellNo, string Type)
        {
            string query = "insert into MNP_PodSmsDelivery (ConsignmentNo, CellNo, createdon, createdBy,Type) Values ('" + cn + "','" + cellNo + "', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "','" + Type + "')";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                con.Close();
                return ex.Message;
            }
            return "OK";
        }

        public DataTable GetZoneOfBranch(Cl_Variables clvar)
        {
            string query = "select * from Branches b where b.branchCode = '" + clvar.Branch + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd); orcd.CommandTimeout = 400;
                oda.Fill(dt);

                orcl.Close();
            }
            catch (Exception)
            { }
            return dt;
        }


        public int InsertInternationalTariff(Cl_Variables clvar, List<DataRow> dr)
        {
            string error = "";
            int count = 0;
            clvar.FromZoneCode = GetZoneOfBranch(clvar).Rows[0]["ZoneCode"].ToString();
            string query = "INSERT INTO tempClientTariff (tariffCode, Client_id, serviceID, BranchCode, FromZoneCode, ToZoneCode, FromWeight, ToWeight, Price, AdditionalFactor, AddtionalFactorSZ, AddtionalFactorDZ, CurrencyCodeid, chkDefaultTariff, chkDeleted, isIntlTariff,  CreatedOn)\n";

            for (int i = 0; i < dr.Count - 1; i++)
            {
                query += "SELECT '0', " + clvar.CustomerClientID + ",'" + clvar.ServiceTypeName + "', '" + clvar.Branch + "', '" + clvar.FromZoneCode + "', '" + dr[i]["DestinationID"].ToString() + "', '" + dr[i]["FromWeight"].ToString() + "', '" + dr[i]["ToWeight"].ToString() + "', \n" +
                        "'" + dr[i]["Price"].ToString() + "', '" + dr[i]["AddFactor"].ToString() + "', '0', '" + dr[i]["addFactorPrice"].ToString() + "','" + clvar.CurrencyCode + "', '0', '0', '1', GETDATE()\n" +
                        "UNION ALL \n";
            }
            query += "SELECT '0', " + clvar.CustomerClientID + ",'" + clvar.ServiceTypeName + "', '" + clvar.Branch + "', '" + clvar.FromZoneCode + "', '" + dr[dr.Count - 1]["DestinationID"].ToString() + "', '" + dr[dr.Count - 1]["FromWeight"].ToString() + "', '" + dr[dr.Count - 1]["ToWeight"].ToString() + "', \n" +
                        "'" + dr[dr.Count - 1]["Price"].ToString() + "', '" + dr[dr.Count - 1]["AddFactor"].ToString() + "', '0', '" + dr[dr.Count - 1]["addFactorPrice"].ToString() + "','" + clvar.CurrencyCode + "', '0', '0', '1', GETDATE()";
            
            query += " insert into tbl_TariffChange (accNo, creditclientid, modifiedby, modifiedon, recompute) values ((select accountno from CreditClients where id = " + clvar.CustomerClientID + "), " + clvar.CustomerClientID + ", '" + HttpContext.Current.Session["U_ID"].ToString() + "', getdate(), 0); ";
            
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;

        }
        public int UpdateInternationalTarrif(Cl_Variables clvar, List<DataRow> dr)
        {

            string query = "CREATE TABLE [dbo].[tempTariffInternational](\n" +
                "\t[Id] [bigint] NOT NULL,\n" +
            "\t[Client_Id] [bigint] NOT NULL,\n" +
            "  [TariffCode] [varchar](50) NOT NULL,\n" +
            "  [ServiceID] [varchar](50) NULL,\n" +
            "  [BranchCode] [varchar](50) NULL,\n" +
            "  [FromZoneCode] [varchar](50) NULL,\n" +
            "  [ToZoneCode] [varchar](50) NULL,\n" +
            "  [FromWeight] [float] NULL,\n" +
            "  [ToWeight] [float] NULL,\n" +
            "  [Price] [float] NULL,\n" +
            "  [additionalFactor] [float] NULL,\n" +
            "  [addtionalFactorSZ] [float] NULL,\n" +
            "  [addtionalFactorDZ] [float] NULL,\n" +
            "  [chkDefaultTariff] [bit] NULL,\n" +
            "  [chkDeleted] [bit] NULL,\n" +
            "  [isIntlTariff] [bit] NULL,\n" +
            "  [createdOn] [datetime] NULL,\n" +
            "\n" +
            ")";

            string query1 = "INSERT INTO tempTariffInternational (ID, tariffCode, Client_id, serviceID, BranchCode, FromZoneCode, ToZoneCode, FromWeight, ToWeight, Price, AdditionalFactor, AddtionalFactorSZ, AddtionalFactorDZ, chkDefaultTariff, chkDeleted, isIntlTariff,  CreatedOn)\n";

            for (int i = 0; i < dr.Count - 1; i++)
            {
                query1 += "SELECT '" + dr[i]["ID"].ToString() + "', '0', " + clvar.CustomerClientID + ",'" + clvar.ServiceTypeName + "', '" + clvar.Branch + "', '" + clvar.FromZoneCode + "', '" + dr[i]["DestinationID"].ToString() + "', '" + dr[i]["FromWeight"].ToString() + "', '" + dr[i]["ToWeight"].ToString() + "', \n" +
                        "'" + dr[i]["Price"].ToString() + "', '" + dr[i]["AddFactor"].ToString() + "', '0', '0', '0', '0', '1', GETDATE()\n" +
                        "UNION ALL \n";
            }
            query1 += "SELECT '" + dr[dr.Count - 1]["ID"].ToString() + "', '0', " + clvar.CustomerClientID + ",'" + clvar.ServiceTypeName + "', '" + clvar.Branch + "', '" + clvar.FromZoneCode + "', '" + dr[dr.Count - 1]["DestinationID"].ToString() + "', '" + dr[dr.Count - 1]["FromWeight"].ToString() + "', '" + dr[dr.Count - 1]["ToWeight"].ToString() + "', \n" +
                        "'" + dr[dr.Count - 1]["Price"].ToString() + "', '" + dr[dr.Count - 1]["AddFactor"].ToString() + "', '0', '0', '0', '0', '1', GETDATE()\n" +
                        "";


            string query2 = "update tempClientTariff\n" +
            "set Client_Id = t.Client_Id,\n" +
            "\tTariffCode = t.TariffCode,\n" +
            "\tServiceID = t.ServiceID,\n" +
            "\tBranchCode = t.BranchCode,\n" +
            "\tFromZoneCode = t.fromZoneCode,\n" +
            "\tToZoneCode = t.ToZoneCode,\n" +
            "\tFromWeight = t.FromWeight,\n" +
            "\tToWeight = t.ToWeight,\n" +
            "\tPrice = t.Price,\n" +
            "\tadditionalFactor = t.additionalFactor,\n" +
            "\taddtionalFactorDZ = t.addtionalFactorDZ,\n" +
            "\taddtionalFactorSZ = t.addtionalFactorSZ,\n" +
            "\tchkDefaultTariff = t.chkDefaultTariff,\n" +
            "\tchkDeleted = t.chkDeleted,\n" +
            "\tisIntlTariff = t.isIntlTariff,\n" +
            "\tcreatedOn = t.createdOn\n" +
            "from tempTariffInternational t where tempClientTariff.id = t.id";
            int count = 0;
            string query3 = "DROP TABLE tempTariffInternational";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;
                count = sqlcmd.ExecuteNonQuery();
                sqlcmd = new SqlCommand(query1, sqlcon);
                sqlcmd.ExecuteNonQuery();
                sqlcmd = new SqlCommand(query2, sqlcon);
                count = sqlcmd.ExecuteNonQuery();
                sqlcmd = new SqlCommand(query3, sqlcon);
                sqlcmd.ExecuteNonQuery();

                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                //error = ex.Message;
            }
            return count;

        }
        public void DemanifestWithoutData(Cl_Variables clvar, GridView gv)
        {
            List<string> queries = new List<string>();
            string headerQuery = "insert into Mnp_Manifest (manifestNumber, manifestType, date, origin, Destination, createdBy, createdOn, zoneCode, branchCode, syncId, isDemanifested, DemanifestDate)\n" +
                "				   Values('" + clvar.manifestNo + "',\n" +
                "                         '" + clvar.ServiceTypeName + "',\n" +
                "                         '" + clvar.LoadingDate + "',\n" +
                "                         '" + clvar.origin + "',\n" +
                "                         '" + clvar.destination + "',\n" +
                "                         '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                "                         GETDATE(),\n" +
                "                         '" + HttpContext.Current.Session["ZoneCode"].ToString() + "',\n" +
                "                         '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                "                         NEWID(),\n" +
                "                         '1',\n" +
                "                         GETDATE() \n" +
                "                         )";
            string detailQuery = "";
            foreach (GridViewRow row in gv.Rows)
            {
                string cn = row.Cells[1].Text;
                string status = (row.FindControl("dd_gStatus") as DropDownList).SelectedValue;
                string reason = (row.FindControl("dd_gStatus") as DropDownList).SelectedItem.Text;
                detailQuery = "insert into MNP_COnsignmentManifest (ManifestNumber, ConsignmentNumber, StatusCode, Reason, DemanifestStateID, Remarks) \n" +
                              " VALUES (\n" +
                              "'" + clvar.manifestNo + "', '" + cn + "', '" + status + "', '" + reason + "','" + status + "', '" + reason + "'  \n" +
                              ")";
                string trackQuery = "insert into ConsignmentsTrackingHistory (consignmentNumber, stateid, CurrentLocation, ManifestNumber, TransactionTime) VALUES(\n" +
                                    " '" + cn + "', '7', '" + clvar.BranchName + "', '" + clvar.manifestNo + "', GETDATE() )";
                queries.Add(detailQuery);
                queries.Add(trackQuery);
            }


            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;

            try
            {
                sqlcmd.CommandText = headerQuery;
                sqlcmd.ExecuteNonQuery();

                foreach (string str in queries)
                {
                    sqlcmd.CommandText = str;
                    sqlcmd.ExecuteNonQuery();
                }

                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();

            }
            finally { sqlcon.Close(); }




            //DataRow[] newCns = dt.Select("ISMODIFIED = 'NEW CONSIGNMENT'");
            //if (newCns.Count() > 0)
            //{
            //    string newCNinsertionQuery = "";
            //}
        }

        public DataTable GetConsignmentDetailForNewManifest_(Cl_Variables clvar)
        {

            string sqlString = "select c.consignmentNumber,\n" +
            "       c.orgin,\n" +
            "       b.name              OriginName,\n" +
            "       c.destination,\n" +
            "       b2.name             DestinationName,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces, 'NO' ISMODIFIED\n" +
            "  from consignment_ops c\n" +
            " inner join Branches b\n" +
            "    on c.orgin = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on c.destination = b2.branchCode\n" +
            " where c.consignmentNumber like '" + clvar.consignmentNo + "'\n" +
            "   --and c.serviceTypeName like '" + clvar.ServiceTypeName + "'\n" +
            "   --and c.orgin = '" + clvar.origin + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public string Insert_CardConsignment(Cl_Variables clvar, DataTable cn, DataTable man, DataTable track)
        {

            //using (SqlConnection con = new SqlConnection(clvar.Strcon2()))
            //{
            //    using (SqlCommand cmd = new SqlCommand("Bulk_CardCNInsert"))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Connection = con;
            //        cmd.Parameters.AddWithValue("@tblCustomers", dt);
            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
            //}
            SqlConnection con = new SqlConnection(clvar.Strcon());
            // SqlTransaction transaction;
            SqlCommand cmd = new SqlCommand();
            //SqlCommand cmd1 = new SqlCommand();
            //SqlCommand cmd2 = new SqlCommand();
            cmd.Connection = con;
            //cmd1.Connection = con;
            //cmd2.Connection = con;

            con.Open();
            //transaction = con.BeginTransaction();

            try
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Bulk_Card_CNInsert";
                cmd.Parameters.AddWithValue("@tblCustomers", cn);
                cmd.Parameters.AddWithValue("@tblCustomers1", man);
                cmd.Parameters.AddWithValue("@tblCustomers2", track);
                cmd.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd.Parameters.AddWithValue("@manifestType", "overnight");
                cmd.Parameters.AddWithValue("@manifestDate", clvar.Bookingdate);
                cmd.Parameters.AddWithValue("@origin", clvar.origin);
                cmd.Parameters.AddWithValue("@destination", clvar.destination);
                cmd.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@error_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@error_message"].Value.ToString() == "0")
                {
                    con.Close();
                    con.Dispose();
                    return "0";
                }
                // Command Objects for the transaction
                //cmd.Transaction = transaction;
                //cmd1.Transaction = transaction;
                //cmd2.Transaction = transaction;
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd1.CommandType = CommandType.StoredProcedure;
                //cmd2.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "Bulk_CardCNInsert";

                //cmd.Parameters.AddWithValue("@tblCustomers", cn);
                ////con.Open();
                //cmd.ExecuteNonQuery();
                //if (cmd.Parameters["@error_message"].ToString() != "1")
                //{
                //    transaction.Rollback();
                //    return "Could Not Save Consignments.";
                //}
                //cmd1.CommandText = "Bulk_Manifest";
                //cmd1.Parameters.AddWithValue("@tblCustomers", man);
                //cmd1.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                //cmd1.Parameters.AddWithValue("@manifestType", "overnight");
                //cmd1.Parameters.AddWithValue("@manifestDate", clvar.Bookingdate);
                //cmd1.Parameters.AddWithValue("@origin", clvar.origin);
                //cmd1.Parameters.AddWithValue("@destination", clvar.destination);
                //cmd1.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                //cmd1.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                //cmd1.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                //cmd1.ExecuteNonQuery();
                //if (cmd1.Parameters["@result"].ToString() != "1")
                //{

                //    transaction.Rollback();
                //    return "Could not generate Manifest.";
                //}

                //cmd2.CommandText = "Bulk_ConsignmentsTrackingHistory";
                //cmd2.Parameters.AddWithValue("@tblCustomers", track);
                //cmd2.ExecuteNonQuery();

                //transaction.Commit();
            }

            catch (SqlException sqlEx)
            {

            }

            finally
            {
                con.Close();
                con.Dispose();
            }
            return "1";
        }

        public string Insert_Card_Consignment(Cl_Variables clvar, DataTable cn, DataTable man, DataTable track)
        {


            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {

                // Command Objects for the transaction
                cmd.Transaction = transaction;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Bulk_Card_CNInsert";

                cmd.Parameters.AddWithValue("@tblCustomers", cn);
                cmd.Parameters.AddWithValue("@tblCustomers1", man);
                cmd.Parameters.AddWithValue("@tblCustomer2", track);
                cmd.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd.Parameters.AddWithValue("@manifestType", "overnight");
                cmd.Parameters.AddWithValue("@manifestDate", clvar.Bookingdate);
                cmd.Parameters.AddWithValue("@origin", clvar.origin);
                cmd.Parameters.AddWithValue("@destination", clvar.destination);
                cmd.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["ZoneCode"].ToString());

                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@error_message"].ToString() != "1")
                {
                    transaction.Rollback();
                    return "Could Not Save CardConsignments.";
                }
                transaction.Commit();
            }

            catch (SqlException sqlEx)
            {
                transaction.Rollback();
            }

            finally
            {
                con.Close();
                con.Dispose();
            }
            return "ok";
        }

        public DataTable GetConsignmentDetailByManifestNumber_2(Cl_Variables clvar)
        {

            string sqlString = "select c.consignmentNumber, c.consigner, c.consignee, c.weight,\n" +
            "       c.orgin,\n" +
            "       b.name              OriginName,\n" +
            "       c.destination,\n" +
            "       b2.name             DestinationName,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces, 'NO' ISMODIFIED,\n" +
            "       (Select DATE from MNP_Manifest where manifestNumber = '" + clvar.manifestNo + "') ManifestDate" +
            "  from CardConsignment_Temp c\n" +
            " inner join Branches b\n" +
            "    on c.orgin = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on c.destination = b2.branchCode\n" +
            " where c.consignmentNumber in ( SELECT cm.ConsignmentNumber from MNP_ConsignmentManifest cm where cm.manifestNumber = '" + clvar.manifestNo + "'  )";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }




    }
}