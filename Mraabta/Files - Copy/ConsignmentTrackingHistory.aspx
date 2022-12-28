<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsignmentTrackingHistory.aspx.cs" Inherits="MRaabta.Files.ConsignmentTrackingHistory" MasterPageFile="~/MasterPage.Master"%>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <script src="../Js/FusionCharts.js" type="text/javascript"></script>
    <body>
        <div>
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                       <h3> Consignment Tracking History </h3>
                    </td>
                </tr>
                <tr>
                    <td class="head_column_panel" width="3%">
                        Consignment Number
                    </td>
                    <td class="head_column_panel" width="15%">
                        <asp:TextBox ID="cnnumber" runat="server"></asp:TextBox>
                    </td>

                    <td class="head_column_panel" width="5%">
                        Output Type
                    </td>
                    <td class="head_column_panel" width="15%">
                        <asp:DropDownList ID="type" runat="server" CssClass="dropdown">
                            <asp:ListItem Value="">Select Output Type</asp:ListItem>
                            <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                            <asp:ListItem Value="excel">Excel</asp:ListItem>
                            <asp:ListItem Value="pdf">PDF</asp:ListItem>
                        </asp:DropDownList>
                    </td>

                    <td class="head_column_panel">
                        <asp:Button ID="Button1" runat="server" Text="Show Data" CssClass="button" OnClick="Btn_Search_Click" />
                        
                    </td>
                    <td class="head_column_panel" colspan="3">
                    </td>
                </tr>
              
            </table>
            <br />
            <asp:Literal ID="lt_graph" runat="server"></asp:Literal>
            <span id="Table_1"  class="tbl-large">
            
                <asp:Label ID="error_msg" runat="server" CssClass="error_msg"></asp:Label>
                <div class="report_name">
                    <asp:Label ID="lbl_report_name" runat="server" CssClass="lbl_rep"></asp:Label>
                    <asp:Label ID="lbl_report_version" runat="server"></asp:Label> 
                </div>

                <table id="TrackingHistory_Detail" runat="server" class="tracking_tbl">
                    <tr>
                        <td align="center" class="TrackingHistory_Detail_heading"> <h3>Consignment Tracking History Detail</h3></td>
                    </tr>
                    <tr>
                        <td class="tracking_tbl_td">
                            <table>
                                <tr>
                                     <td class="lbl"><b>Consignment Number:</b></td> 
                                     <td class="field"><asp:Label ID="lbl_con_num" runat="server"></asp:Label> </td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Origin:</b</td> 
                                     <td class="field"><asp:Label ID="lbl_orign" runat="server"></asp:Label> </td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Booking Date:</b></td> 
                                     <td class="field"><asp:Label ID="bdate" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Weight:</b></td> 
                                     <td class="field"><asp:Label ID="lbl_weight" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Delivery Time:</b></td>
                                     <td class="field"><asp:Label ID="lbl_delivery_time" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Delivery Rider:</b></td> 
                                     <td class="field"><asp:Label ID="lbl_delivery_rider" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>

                        <td class="tracking_tbl_td">
                            <table>
                                <tr>
                                     <td class="lbl"><b>Service Type:</b></td> 
                                     <td class="field"><asp:Label ID="lbl_serive_type" runat="server"></asp:Label> </td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Destination:</b</td> 
                                     <td class="field"><asp:Label ID="lbl_destination" runat="server"></asp:Label> </td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Shipper:</b></td> 
                                     <td class="field"><asp:Label ID="lbl_shipper" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Consignee:</b></td> 
                                     <td class="field"><asp:Label ID="lbl_consignee" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Received By:</b></td> 
                                     <td class="field"><asp:Label ID="lbl_received" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Comments:</b></td> 
                                     <td class="field"><asp:Label ID="lbl_comment" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>

                         <td class="tracking_tbl_td">
                            <table>
                                <tr>
                                     <td class="lbl"><b>Current Status: </b></td> 
                                     <td class="field"><asp:Label ID="lbl_status" runat="server"></asp:Label> </td>
                                </tr>
                                <tr>
                                     <td class="lbl"><b>Accout No:</b></td> 
                                     <td class="field"><asp:Label ID="lbl_account" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    
                    </tr>                  
                
                </table>

                <asp:GridView ID="gg_CustomerLedger_Month" runat="server" AutoGenerateColumns="false"
                    CssClass="mGrid floatgrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None"
                    AllowPaging="true" BorderWidth="1px" PageSize="200" OnPageIndexChanging="GridView2_PageIndexChanging">

                    <Columns>
                        <asp:TemplateField HeaderText = "S.No." ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>  
                         <asp:BoundField HeaderText="Transaction Time" DataField="transactionTime" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%"></asp:BoundField>
                         <asp:BoundField HeaderText="CStatus" DataField="TrackingStatus" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%"></asp:BoundField>
                         <asp:BoundField HeaderText="Location" DataField="currentLocation" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%"></asp:BoundField>
                         <asp:BoundField HeaderText="Message" DataField="Detail" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50%"></asp:BoundField>
                    </Columns>
                    
                </asp:GridView>


          <%--      <asp:GridView ID="excelgg" runat="server" AutoGenerateColumns="false"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None"
                    BorderWidth="1px" ShowFooter="true">
                    <Columns>
                            <asp:BoundField HeaderText="Collection Branch" DataField="CollectionBranch" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Customer Account No" DataField="CustomerAccountNo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Centralized Flag" DataField="CentralizedFlag" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Receipt ID" DataField="ReceiptID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Receipt Date" DataField="ReceiptDatep" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Receipt No" DataField="ReceiptNo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Ref No" DataField="RefNo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Cheque No" DataField="ChequeNo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Cheque Date" DataField="ChequeDate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Payment Source" DataField="PaymentSource" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Receipt Amount" DataField="ReceiptAmount" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="Cheque Status" DataField="ChequeStatus" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>                            
                    </Columns>
                </asp:GridView> --%>            
            </span>
        </div>
</asp:Content>
