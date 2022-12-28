<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" EnableEventValidation="false" CodeBehind="CIH2BankVoucherEdit_QA.aspx.cs" Inherits="MRaabta.Files.CIH2BankVoucherEdit_QA" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript">
        var submit = 0;
        //        function CheckDouble() {
        //            if (++submit > 1) {
        //                alert('The Button Can Only be clicked Once');
        //                return false;
        //            }
        //        }
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }
        function AddValidation() {


            return true;
        }
    </script>
    <style>
        .input-field.rabi > input
        {
            width: 10%;
        }
        
        .count
        {
            text-align: right;
            font-weight: bold;
            font-size: 15px;
        }
        .ajax__calendar_container TABLE tr
        {
            margin: 0 0 0px !important;
        }
    </style>
    <div runat="server" id="loader" style="background-color: honeydew; float: left; height: 100%;
        opacity: 0.7; position: absolute; text-align: center; display: none; top: 11%;
        width: 84% !important; padding-top: 300px;">
        <div class="loader">
            <img src="../images/Loading_Movie-02.gif" style="top: 300px !important;" />
        </div>
    </div>
    <noscript style="background-color: honeydew; float: left; height: 100%; opacity: 1;
        position: absolute; text-align: center; top: 11%; width: 84% !important; padding-top: 300px;">
        <div>
            You must enable javascript to continue.
        </div>
    </noscript>
    <asp:UpdatePanel ID="panel1111" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                        <h3>
                            Cash In Hand To Bank (EDIT)
                        </h3>
                    </td>
                </tr>
            </table>
            <div style="text-align: center; font-size: medium; font-weight: bold; width: 100%;
                padding-left: 20px;">
                <asp:Label ID="Errorid" runat="server"></asp:Label>
            </div>
            <asp:Panel ID="panel1" runat="server">
                <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
                    padding-top: 0px !important; width: 97%" class="input-form">
                    <%--   <tr style="float: none !important;">
                        <td style="float: none !important; font-variant: small-caps !important; width: 200px;
                            padding-bottom: 5px !important; font-size: large; text-align: center;">
                            <b>Cash In Hand To Bank</b>
                        </td>
                    </tr>--%>
                    <tr style="margin-top: 10px;">
                        <td class="field">
                            Company:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_company" runat="server" CssClass="dropdown width" Width="250px">
                                <%--  <asp:ListItem Text="M&P" Value="01"></asp:ListItem>
                        <asp:ListItem Text="R&R" Value="02"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td class="space">
                        </td>
                        <td class="space">
                        </td>
                        <td class="field">
                            Nature of Deposit:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_nature" runat="server" CssClass="dropdown width" Width="250px"
                                AutoPostBack="true" OnSelectedIndexChanged="naturedeposit_SelectedIndexChanged">
                                <asp:ListItem Text="COD" Value="01"></asp:ListItem>
                                <asp:ListItem Text="NONCOD" Value="02"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Year:
                        </td>
                        <td class="input-field">
                            <%--OnSelectedIndexChanged="dd_year_SelectedIndexChanged"--%>
                            <asp:DropDownList ID="dd_year" runat="server" CssClass="dropdown width" AutoPostBack="true"
                                Width="250px">
                                <asp:ListItem Text="2018" Value="18"></asp:ListItem>
                                <asp:ListItem Text="2017" Value="17"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space">
                        </td>
                        <td class="space">
                        </td>
                        <td class="field">
                            Month:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_month" Width="250px" runat="server" CssClass="dropdown width"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <%--OnSelectedIndexChanged="dd_month_SelectedIndexChanged"--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Zone:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_zone" runat="server" CssClass="dropdown width" Width="250px"
                                AutoPostBack="true" OnSelectedIndexChanged="dd_zone_Changed">
                                <%--  <asp:ListItem Text="M&P" Value="01"></asp:ListItem>
                        <asp:ListItem Text="R&R" Value="02"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td class="space">
                        </td>
                        <td class="space">
                        </td>
                        <td class="field">
                            Branch:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_branch" runat="server" Width="250px" CssClass="dropdown width">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            RR Date:
                        </td>
                        <td class="input-field">
                            <asp:TextBox ID="txt_rrdate" runat="server" Width="240px" OnTextChanged="RRDate_TextChanged"
                                AutoPostBack="true" onkeypress="return false;"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_rrdate"
                                Format="yyyy-MM-dd">
                            </Ajax1:CalendarExtender>
                        </td>
                        <td class="space">
                        </td>
                        <td class="space">
                        </td>
                        <%--<td class="field">
                            Deposit Date:
                        </td>
                        <td class="input-field">
                            <asp:TextBox ID="txt_chequeDate" runat="server" Width="240px" onkeypress="return false;"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_chequeDate"
                                Format="yyyy-MM-dd">
                            </Ajax1:CalendarExtender>
                        </td>--%>
                        <td class="field">
                            <%--DSlip No:--%>
                            RR Total Amount:
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:Label ID="lbl_totalamount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                        </td>
                        <td>
                            <asp:Button ID="btn_save" runat="server" Text="Search" CssClass="button" CommandName="first" 
                                OnClientClick="return CheckDouble();return AddValidation()" OnClick="btn_Search_Click" />
                            &nbsp; &nbsp;
                            <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label runat="server" ID="lbl_err2" Style="color: #FF0000; font-size: medium"></asp:Label>
                            <asp:Label ID="lbl_message" runat="server" ForeColor="Green"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="width: 100%; text-align: left" id="btnDiv" runat="server">
                &nbsp; &nbsp;
                <%--   <asp:Button ID="bt_ReportView" runat="server" Text="ReportView" CssClass="button"
                    OnClick="bt_ReportView_Click" Visible="true" />--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hf_remaining_val" runat="server" />
    <asp:HiddenField ID="hf_remaining_pettyCash" runat="server" />
    <asp:HiddenField ID="hf_diff" runat="server" />
    <asp:HiddenField ID="hf_petty_amount" runat="server" />
    <div style="float: left; width: 74%; text-align: right; clear: both;" id="divtotal"
        runat="server" visible="false">
        <span class="count">Total DSlip Amount: </span>
        <asp:Label ID="lbl_grandamount" runat="server" CssClass="count"></asp:Label>
        <asp:Label ID="lbl_diffamount" runat="server" CssClass="count"></asp:Label>
    </div>
    <br />
    <asp:Label ID="lbl_count" runat="server" Style="float: left; width: 73%; text-align: right;
        font-size: 15px; font-weight: bold;"></asp:Label>
    <asp:Button ID="lbl_save" runat="server" Text="Save" CssClass="button" CommandName="first" Visible="false" style="left: 4%;position: relative;"
        OnClick="btn_save_Click" />
    <span id="Table_1" class="tbl-large">
        <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
            AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="true"
            BorderWidth="1px" ShowFooter="false" OnRowCommand="gv_consignments_RowCommand" PageSize="200"
            OnRowDataBound="GridView1_RowDataBound">
            <HeaderStyle CssClass="topheader" />
            <FooterStyle CssClass="footercolor" />
            <Columns>
                <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Id" DataField="Id" HtmlEncode="False" ItemStyle-Width="10%"
                    ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                <asp:BoundField HeaderText="Deposit Date" DataField="DepositDate" HtmlEncode="False"
                    DataFormatString="{0:yyyy-MM-dd}" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                </asp:BoundField>
                <asp:TemplateField HeaderText="DSlip No" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:TextBox ID="DSlipNo" Text='<%# Bind("DSlipNo") %>' runat="server">
                        </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DSlip Amount" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:TextBox ID="DSlipAmount" Text='<%# Bind("DSlipAmount") %>' runat="server">
                        </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bank Name" ItemStyle-Width="68%">
                    <ItemTemplate>
                        <asp:Label ID="lblBank" runat="server" Text='<%# Eval("bank_code") %>' Visible="false" />
                        <asp:DropDownList ID="dd_bankname" runat="server">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </span>
</asp:Content>
