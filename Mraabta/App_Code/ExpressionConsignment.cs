using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace MRaabta.App_Code
{
    public class ExpressionConsignment
    {

        public ExpressionConsignment()
        {
        }

        Cl_Variables clvar = new Cl_Variables();

        #region EXPRESSION_CONSIGNMENT_APPROVAL_CODE

        //public DataSet Consignment(Cl_Variables clvar)
        //{
        //    DataSet Ds_1 = new DataSet();

        //    try
        //    {
        //        string query = "SELECT * FROM Consignment c WHERE /*c.serviceTypeName = 'EXPRESSIONS' and*/ c.consignmentNumber = '" + clvar.consignmentNo + "'";

        //        SqlConnection orcl = new SqlConnection(clvar.Strcon());
        //        orcl.Open();
        //        SqlCommand orcd = new SqlCommand(query, orcl);
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(Ds_1);
        //        orcl.Close();
        //    }
        //    catch (Exception Err)
        //    { }
        //    finally
        //    { }

        //    return Ds_1;
        //}

        //public DataTable ConsignmentExpressionDetail(Cl_Variables clvar)
        //{
        //    string query = "select \n" +
        //                                "ced.consignementNo, ced.itemId, ced.itemQty, ced.amount, ced.gst, ced.serviceCharges, \n" +
        //                                "ep.code + ' -- ' +ep.name itemcode, ced.message, ced.status, ced.createdon, ced.createdby, ced.modifiedon, ced.modifiedby \n" +
        //                                "from ConsignmentExpressionDetail ced , ExpressionProduct ep  \n" +
        //                                "where  \n" +
        //                                "ced.itemId = ep.id \n" +
        //                                "and ced.consignementNo = '" + clvar.consignmentNo + "'";

        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        SqlConnection orcl = new SqlConnection(clvar.Strcon2());
        //        orcl.Open();
        //        SqlCommand orcd = new SqlCommand(query, orcl);
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(dt);
        //        orcl.Close();
        //    }
        //    catch (Exception ex)
        //    { }
        //    return dt;

        //}

        public DataSet Consignment(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM Consignment c WHERE /*c.serviceTypeName = 'EXPRESSIONS' and*/ c.consignmentNumber = '" + clvar.consignmentNo + "'";

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

        public DataTable ConsignmentExpressionDetail(Cl_Variables clvar)
        {
            string query = "select \n" +
                                        "ced.id, ced.consignementNo, ced.itemId, ced.itemQty, ced.amount, ced.gst, ced.serviceCharges, \n" +
                                        "ep.code + ' -- ' +ep.name itemcode, ced.message, CASE WHEN (ced.status is null or ced.status = '') then '1' else ced.status end status, ced.createdon, ced.createdby, ced.modifiedon, ced.modifiedby \n" +
                                        "from ConsignmentExpressionDetail ced , ExpressionProduct ep  \n" +
                                        "where  \n" +
                                        "ced.itemId = ep.id \n" +
                                        "and ced.consignementNo = '" + clvar.consignmentNo + "'";

            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        public string Update_ExpressionConsignmentApproval(Cl_Variables obj)
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
                               //  + "		    [creditClientId] = '" + obj.CustomerClientID + "' , \n"
                               //  + "		    [weight] = '" + obj.Weight + "' , \n"
                               //  + "		    [weightUnit] = '" + obj.Unit + "' , \n"
                               //   + "		    [discount] = '" + obj.Discount + "' , \n"
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
                               + "		    [consignmentScreen] = '11' , \n"
                               + "		    [isInsured] = '" + obj.isInsured + "' , \n"
                               + "		    [isReturned] = '" + obj.isReturned + "' , \n"
                               + "		    [consigneeCNICNo] = '" + obj.ConsigneeCNIC + "' , \n"
                               + "		    [cutOffTimeShift] = '" + obj.cutOffTimeShift.ToString("yyyy-MM-dd hh:mm:ss") + "' , \n"
                               + "		    [bookingDate] = '" + obj.BookingDate + "' , \n"
                               + "		    [cnClientType] = '" + obj.cnClientType + "' , \n"
                               + "		    [destinationExpressCenterCode] = '" + obj.destinationExpressCenterCode + "' , \n"
                               + "		    [originExpressCenter] = '" + obj.expresscenter + "' , \n"
                               + "		    [receivedFromRider] = '" + obj.receivedFromRider + "' , \n"
                               + "		    [chargedAmount] = '" + obj.ChargeAmount + "' , \n"
                               + "		    [isApproved] = '1' , \n"
                               + "		    [isPriceComputed] = '1' , \n"
                               + "		    [accountReceivingDate] = '" + obj.expressionDeliveryDateTime.ToString("yyyy-MM-dd hh:mm:ss") + "' \n"
                               + "          Where CONSIGNMENTNUMBER = '" + obj.consignmentNo + "' ";


                SqlConnection sqlcon = new SqlConnection(clvar.Strcon2());
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

        public string Update_ExpressionConsignmentApproval2(Cl_Variables obj)
        {
            clvar.Error = "";
            try
            {
                string sql = "update Consignment set \n"
                               + "		    [isApproved] = '1' , \n"
                               + "		    [accountReceivingDate] = '" + obj.expressionDeliveryDateTime.ToString("yyyy-MM-dd hh:mm:ss") + "' \n"
                               + "          Where CONSIGNMENTNUMBER = '" + obj.consignmentNo + "' ";


                SqlConnection sqlcon = new SqlConnection(clvar.Strcon2());
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


        public string Add_Consignment(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon2());
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
                //   sqlcmd.Parameters.AddWithValue("@cod", obj.StateID);
                sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                sqlcmd.Parameters.AddWithValue("@status", obj.status);
                sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", "0");
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
                sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", "0");
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
                sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", "0");
                sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", obj.cnScreenId);
                sqlcmd.Parameters.AddWithValue("@isInsured", "0");
                sqlcmd.Parameters.AddWithValue("@isReturned", "0");
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.BookingDate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@receivedFromRider", "0");
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
                //    sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                sqlcmd.Parameters.AddWithValue("@cnScreenId", "11");
                sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);

                //   SqlParameter P_XCode = new SqlParameter("@XCode", SqlDbType.VarChar, 256);
                //    P_XCode.Direction = ParameterDirection.Output;

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


        #endregion

    }
}