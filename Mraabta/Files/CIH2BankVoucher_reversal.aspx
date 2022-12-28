<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="CIH2BankVoucher_reversal.aspx.cs" Inherits="MRaabta.Files.CIH2BankVoucher_reversal" EnableEventValidation="false" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript">
        var submit = 0;
        function CheckDouble() {
            if (++submit > 1) {
                alert('The Button Can Only be clicked Once');
                return false;
            }
        }
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }
        function AddValidation() {

            if (document.getElementById("<%=txt_chequeDate.ClientID%>").value == "") {
                alert("Please Select Cheque Date");
                return false;
            }

            if (document.getElementById("<%=dd_depositSlipBank.ClientID%>").value == "0") {
                alert("Please Select Bank");
                return false;

            }
            if (document.getElementById("<%=txt_dslipNo.ClientID%>").value == "") {
                alert("Please Write The Deposit Slip Number");
                return false;
            }

            if (document.getElementById("<%=txt_amount.ClientID%>").value == "") {
                alert("Please Enter Amount");
                return false;
            }

            return true;
        }
    </script>
    <style>
        .input-field.rabi > input {
            width: 10%;
        }
    </style>
    <div runat="server" id="loader" style="background-color: honeydew; float: left; height: 100%; opacity: 0.7; position: absolute; text-align: center; display: none; top: 11%; width: 84% !important; padding-top: 300px;">
        <div class="loader">
            <img src="../images/Loading_Movie-02.gif" style="top: 300px !important;" />
        </div>
    </div>
    <noscript style="background-color: honeydew; float: left; height: 100%; opacity: 1; position: absolute; text-align: center; top: 11%; width: 84% !important; padding-top: 300px;">
        <div>
            You must enable javascript to continue.
        </div>
    </noscript>
    <asp:UpdatePanel ID="panel1111" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                        <h3>Cash In Hand To Bank Reversal
                        </h3>
                    </td>
                </tr>
            </table>
            <div style="text-align: center; font-size: medium; font-weight: bold; width: 100%; padding-left: 20px;">
                <asp:Label ID="Errorid" runat="server"></asp:Label>
            </div>
            <asp:Panel ID="panel1" runat="server">
                <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
                    class="input-form">
                    <%--   <tr style="float: none !important;">
                        <td style="float: none !important; font-variant: small-caps !important; width: 200px;
                            padding-bottom: 5px !important; font-size: large; text-align: center;">
                            <b>Cash In Hand To Bank</b>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="field">Company:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_company" runat="server" CssClass="dropdown width" Width="250px">
                                <%--  <asp:ListItem Text="M&P" Value="01"></asp:ListItem>
                        <asp:ListItem Text="R&R" Value="02"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td class="space"></td>
                        <td class="space"></td>
                    </tr>
                    <tr>
                        <td class="field">Year:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_year" runat="server" CssClass="dropdown width" OnSelectedIndexChanged="dd_year_SelectedIndexChanged"
                                AutoPostBack="true" Width="250px">
                                <asp:ListItem Text="2018" Value="18"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space"></td>
                        <td class="space"></td>
                        <td class="field">Month:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_month" Width="250px" runat="server" CssClass="dropdown width"
                                OnSelectedIndexChanged="dd_month_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">Zone:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_zone" runat="server" CssClass="dropdown width" Width="250px"
                                AutoPostBack="true" OnSelectedIndexChanged="dd_zone_Changed">
                                <%--  <asp:ListItem Text="M&P" Value="01"></asp:ListItem>
                        <asp:ListItem Text="R&R" Value="02"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td class="space"></td>
                        <td class="space"></td>
                        <td class="field">Branch:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_branch" runat="server" Width="250px" CssClass="dropdown width">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">Reversal Description
                        </td>
                        <td colspan="5" class="input-field">
                            <asp:TextBox ID="txt_revDesc" runat="server" Width="810px" MaxLength="1000"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">DSlip Bank:
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_depositSlipBank" runat="server" AppendDataBoundItems="true"
                                CssClass="dropdown width" Width="250px">
                                <asp:ListItem Value="0">Select Bank</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space"></td>
                        <td class="space"></td>
                        <td class="field">DSlip No:
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_dslipNo" onkeypress="if ( isNaN(this.value + String.fromCharCode(event.keyCode) )) return false;"
                                Width="240px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">Deposit Date:
                        </td>
                        <td class="input-field">
                            <asp:TextBox ID="txt_chequeDate" runat="server" Width="240px" onkeypress="return false;"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_chequeDate"
                                Format="yyyy-MM-dd"></Ajax1:CalendarExtender>
                        </td>
                        <td class="space"></td>
                        <td class="space"></td>
                        <td class="field">Amount:
                        </td>
                        <td class="input-field">
                            <asp:TextBox ID="txt_amount" onkeypress="return isNumberKey(event);" runat="server"
                                Width="240px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="field"></td>
                        <td>
                            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" CommandName="first"
                                OnClientClick="return CheckDouble();return AddValidation()" OnClick="btn_save_Click" />
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
</asp:Content>