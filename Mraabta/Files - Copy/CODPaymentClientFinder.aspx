<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="CODPaymentClientFinder.aspx.cs" Inherits="MRaabta.Files.CODPaymentClientFinder" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        
    </style>
    <table cellpadding="0" cellspacing="0" style="width: 97% !important;" class="input-form">
        <tr>
            <td style="width: 100%; float: left; font-family: Calibri; font-size: large; font-variant: small-caps;
                text-align: center;">
                <b>To Be Paid Clients(COD) Summary</b>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; float: left; font-family: Calibri; font-size: medium; font-variant: small-caps;
                text-align: center;">
                <asp:Label ID="Errorid" runat="server" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="input-field" style="width: 20% !important;">
                <div>
                    <b>Zones</b></div>
                <div>
                    <asp:DropDownList ID="dd_Zone" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                        Style="border-color: #CCCCCC !important; height: 27px !important;" CssClass="dropdown"
                        Width="70%" OnSelectedIndexChanged="dd_Zone_SelectedIndexChanged">
                        <asp:ListItem Value="0">Select Zone</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="chk_zone" runat="server" Text="All" />
                </div>
            </td>
            <td class="input-field" style="width: 20% !important;">
                <div>
                    <b>Branch</b></div>
                <div>
                    <asp:DropDownList ID="dd_branch" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                        Style="border-color: #CCCCCC !important; height: 27px !important;" CssClass="dropdown"
                        Width="70%">
                        <asp:ListItem Value="0">Select Branch</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="chk_branch" runat="server" Text="All" />
                </div>
            </td>
            <td class="input-field" style="width: 20% !important;">
                <div>
                    <b>Account Number (Optional)</b>
                </div>
                <div>
                    <asp:TextBox ID="txt_accountNumber" runat="server" CssClass="textBox" Style="border-color: #CCCCCC !important;
                        height: 20px !important;"></asp:TextBox>
                </div>
            </td>
            <td class="input-field" style="width: 20% !important;">
                <div>
                    <b>&nbsp;</b>
                </div>
                <div>
                    <asp:Button ID="btn_search" Font-Bold="true" runat="server" Text="Generate Report"
                        CssClass="button" onclick="btn_search_Click" />
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%;" class="tbl-large">
        <asp:GridView ID="gv_clientDetails" runat="server" AutoGenerateColumns="false" Width="80%"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" CssClass="mGrid" 
            ShowFooter="true" onrowdatabound="gv_clientDetails_RowDataBound">
            <Columns>
                <asp:BoundField HeaderText="Zone" DataField="ZoneName" />
                <asp:BoundField HeaderText="Branch" DataField="BranchName" />
                <asp:BoundField HeaderText="Account#" DataField="AccNo" />
                <asp:BoundField HeaderText="Account Name" DataField="AccName" />
                <asp:BoundField HeaderText="Beneficiary Name" DataField="BenName" />
                <asp:BoundField HeaderText="Bank Acc#" DataField="BenAccNo" />
                <asp:BoundField HeaderText="Bank Code" DataField="BenBankCode" />
                <asp:BoundField HeaderText="Bank Name" DataField="BenBank" />
                <asp:BoundField HeaderText="CN Count" DataField="CnCount"  DataFormatString="{0:N0}"/>
                <asp:BoundField HeaderText="DELV Count" DataField="DeliveredCount"  DataFormatString="{0:N0}"/>
                <asp:BoundField HeaderText="RR Count" DataField="RRCount"  DataFormatString="{0:N0}"/>
                <asp:BoundField HeaderText="Inv. Count" DataField="InvCount"  DataFormatString="{0:N0}" />
                <asp:BoundField HeaderText="COD AMT." DataField="CODAmt" DataFormatString="{0:N2}" />
                <asp:BoundField HeaderText="RR AMT." DataField="RRAmt" DataFormatString="{0:N2}" />
                <asp:BoundField HeaderText="Available AMT." DataField="AvailableAmt" DataFormatString="{0:N2}" />
                <asp:BoundField HeaderText="Invoice AMT." DataField="InvoiceAmt" DataFormatString="{0:N2}" />
                <asp:BoundField HeaderText="Outstanding AMT." DataField="OutstandingAMT" DataFormatString="{0:N2}" />
                <asp:BoundField HeaderText="Payable AMT." DataField="NetPayable" DataFormatString="{0:N0}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

