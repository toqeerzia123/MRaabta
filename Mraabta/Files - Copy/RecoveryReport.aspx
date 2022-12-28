<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="RecoveryReport.aspx.cs" Inherits="MRaabta.Files.RecoveryReport" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <script type="text/javascript">
        function isNumberKeyWithDecimal(evt) {
            var status = false;
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                status = false;
            if (charCode == 46)
                status = true;
            if (charCode > 47 && charCode < 58)
                status = true;
            return status;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }



    </script>
    <style>
        .table th {
            border: 1px solid black;
            margin: 0;
            padding: 0;
        }

        .table td {
            border: 1px solid black;
            margin: 0;
            padding: 0;
        }

        tr.spaceUnder > td {
            padding-bottom: 2px;
        }

        .div_header {
            width: 100%;
            height: 23px;
            font-size: 14px;
            margin-bottom:1px;
            
        }

            .div_header div {
                float: left;
                background-color: gray;
                width: 16.4%;
                font-size: 14px;
                border: 1px solid #000;
                color:black;
            }
       

        .bottom div {

            float: left;
            width: 16.4%;
            height: 36px;
            border: 0.5px solid #000;
            color:black;
        }

        .saveBtn {
            margin-top: 5px;
            float: right;
            margin:8px 25px 10px 0px;
        }

       

          

        }
    </style>
   

    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Recovery Report
                </h3>
            </td>
        </tr>
    </table>

    <div runat="server" id="ServiceTable" style="display: none;margin-top:3px">

        <table cellpadding="0" cellspacing="0" style="color:black; border: solid; border-width: thin; height: auto; border-color: black; margin-left: 5px; font-size:14px; width: 98%;">

            
            <tr class="spaceUnder">
                <td class="field" style="width: 7% !important; text-align: left !important; padding-right: 5px !important;">
                    <b>Zone:</b>
                </td>
                <td style="width: 7%; text-align: left;">
                    <div runat="server" id="zoneLbl"></div>
                </td>

                <td class="field" style="width: 7% !important; text-align: left !important; padding-right: 5px !important;">
                    <b>Branch:</b>
                </td>
                <td style="width: 8%; text-align: left;">
                    <div runat="server" id="branchLbl"></div>
                </td>

                <td class="field" style="width: 8% !important; text-align: left !important; padding-right: 5px !important;">
                    <b>Express Center:</b>
                </td>
                <td style="width: 14%; text-align: left;">
                    <div runat="server" id="ECLbl"></div>
                </td>
            </tr>
            <tr class="spaceUnder">
                <td class="field" style="width: 7% !important; text-align: left !important; padding-right: 5px !important;">
                    <b>Booking Code:</b>
                </td>
                <td style="width: 7%; text-align: left;">
                    <div runat="server" id="bookingLbl"></div>
                </td>

                <td class="field" style="width: 7% !important; text-align: left !important; padding-right: 5px !important;">
                    <b>DSSP Number:</b>
                </td>
                <td style="width: 8%; text-align: left;">
                    <div runat="server" id="DSSPLbl"></div>
                </td>

                 <td class="field" style="width: 6% !important; text-align: left !important; padding-right: 5px !important;">
                    <b>Total CN:</b>
                </td>
                <td style="width: 14%; text-align: left;">
                    <div runat="server" id="totalCNLbl"></div>
                </td>

            </tr>

            <tr class="spaceUnder">
               

               <%-- <td class="field" style="width: 7% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Total CN Amount:</b>
                </td>
                <td style="width: 10%; text-align: left;">
                    <div runat="server" id="totalCNAmountLbl"></div>
                </td>--%>

                 <td class="field" style="width: 8% !important; text-align: left !important; padding-right: 5px !important;">
                    <b>Total Amount:</b>
                </td>
                <td style="width: 7%; text-align: left;">
                    <div runat="server" id="TotalAmountCollectLbl"></div>
                </td>

                <%--  <td class="field" style="width: 8% !important; text-align: left !important; padding-right: 5px !important;">
                    <b>Collected Amount:</b>
                </td>
                <td style="width: 12%; text-align: left;">
                    <asp:TextBox id="totalAmountCollected" runat="server" AutoPostBack="false"  onkeypress="return isNumberKeyWithDecimal(event)"  OnTextChanged="totalAmountCollected_TextChanged"></asp:TextBox> Rs
                </td>

                <td class="field" style="width: 8% !important; text-align: left !important; padding-right: 5px !important;">
                    <b>Remarks:</b>
                </td>
                <td style="width: 14%; text-align: left;">
                    <asp:TextBox id="AllRemarks" runat="server" Width="190px" ></asp:TextBox>
                </td>--%>

               
            </tr>
           <%-- <tr class="spaceUnder">
                 <td class="field" style="width: 8% !important; text-align: left !important; padding-right: 5px !important;">
                    <b>Pending Amount:</b>
                </td>
                <td style="width: 7%; text-align: left;">
                    <div runat="server" id="pendingAmountLbl"></div>
                </td>

               <td style="width:8%">
                     <asp:Button ID="SaveSheet2" runat="server" Text="Save Data" CssClass="button saveBtn" OnClick="btn_Save_Data" Width="100px" Visible="false" />
               </td>


            </tr>--%>
        </table>
        <!--
              <table class="table"    cellpadding="0" cellspacing="0" style="font-size: 14px;margin-left:8px; display: inline-block; float: left; width: 24%;">
                                    
                  <tr style="text-align: center;background-color:lightgray;color:black">
                        <th><b>Service</b></th>
                        <th><b>CN</b></th>
                        <th><b>Amount</b></th>
                  </tr>
                                   
                  <asp:Literal ID="ServiceSummaryLiteral" runat="server"  />
              </table>
-->
        <div style="margin-left: 5px;margin-top:3px">
            <div style="text-align: center;background-color:gray;border: 1px solid #000;font-size:14px;width: 98.4%;color:black;"><b>Service Wise Summary</b></div>
            <div class="div_header" style="">
                <div style="text-align: center;"><b>Service</b></div>
                <div style="text-align: center;"><b>CN</b></div>
                <div style="text-align: center;"><b>Amount</b></div>

                <div style="text-align: center" runat="server" id="secondHeader1"><b>Service</b></div>
                <div style="text-align: center" runat="server" id="secondHeader2"><b>CN</b></div>
                <div style="text-align: center" runat="server" id="secondHeader3"><b>Amount</b></div>
            </div>
            <div class="bottom">
                <asp:Label ID="ServiceSummaryLiteral12" runat="server" />

            </div>
        </div>
        <div style="overflow-y: scroll; height: 600px; /*width: 74%; */width: 99.5%; float: left; margin-left: 5px;margin-top:3px">
            <asp:GridView runat="server" ID="ConsignmentTable" Width="100%" CellPadding="0" CellSpacing="0" GridLines="Both" ForeColor="Black" AutoGenerateColumns="false" BorderColor="Black" HeaderStyle-HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1px" CssClass=" " OnRowCreated="grvMergeHeader_RowCreated">
                <Columns>
                    <asp:TemplateField HeaderText="S.No." HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="2%" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ConsignmentNumber" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="CN Number" ItemStyle-Width="5%" HeaderStyle-ForeColor="Black"     ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="BookingDate" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="Booking Date" ItemStyle-Width="5%" HeaderStyle-ForeColor="Black"  ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="ServiceType" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="13%" HeaderText="Service"  HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="Destination" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="Destination" ItemStyle-Width="7%"  HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" HeaderStyle-VerticalAlign="Middle" />
                    <asp:BoundField DataField="DiscountCN" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="Discount CN" ItemStyle-Width="7%" HeaderStyle-ForeColor="Black"  ItemStyle-HorizontalAlign ="Center" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="AmountCollect" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" HeaderText="CN Amount" ItemStyle-Width="7%"  HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                   <%-- <asp:TemplateField HeaderText="Collected Amount" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="7%"  HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <asp:TextBox ID="AmountCollectGrid" Text='<%# Bind("Collected_Amount") %>'  onkeypress="return isNumberKeyWithDecimal(event)"   AutoPostBack="false"  runat="server"></asp:TextBox>
                            
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="FranchiseComission" HeaderStyle-ForeColor="Black" HeaderStyle-Font-Size="11" ItemStyle-Font-Size="Small" ItemStyle-Width="7%" HeaderText="Franchise Commission" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                   <%-- <asp:TemplateField HeaderText="Remarks" HeaderStyle-ForeColor="Black" HeaderStyle-Font-Size="11"   ItemStyle-Font-Size="Small" ItemStyle-Width="7%" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center"   >
                        <ItemTemplate>
                            <asp:TextBox ID="RemarksCN" Text='<%# Bind("Remarks") %>'  MaxLength="150" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <div>
              
        </div>
        <div style="clear: both">
             <b>
                    <%--<div runat="server" id="statusMsg2" style="color: red; font: bold;font-size:14px"></div>--%>
                </b>
            <%--<asp:Button runat="server" ID="SaveSheet" CssClass="button saveBtn" OnClick="btn_Save_Data" Width="100px"  Text="Save Data" Font-Size="16" />--%>
        </div>

    </div>
</asp:Content>