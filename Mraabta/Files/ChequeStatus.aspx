<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="ChequeStatus.aspx.cs" Inherits="MRaabta.Files.ChequeStatus" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .abc {
            border: 0;
            border
        }
    </style>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
        class="input-form">
        <tr style="float: none !important;">
            <td colspan="9" style="float: none !important; font-variant: small-caps !important; width: 200px; padding-bottom: 5px !important; font-size: large; text-align: center;">
                <b>Cheque Status</b>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px!important;">Client Bank
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_clientBank" runat="server" Width="97%" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">.::Select Client Bank::.</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Deposit Bank
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_depositBank" runat="server" Width="97%" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">.::Select Deposit Bank::.</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Month
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_month" runat="server"></asp:TextBox>
                <Ajax1:CalendarExtender ID="CalendarExtender1" BehaviorID="calendar1" runat="server"
                    Enabled="True" Format="MMM-yyyy" TargetControlID="txt_month"></Ajax1:CalendarExtender>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px!important;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;"></td>
            <td class="input-field" style="width: 15% !important;">
                <asp:Button ID="btn_GetCheques" runat="server" CssClass="button" Text="Get Cheques"
                    OnClick="btn_GetCheques_Click" />
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="float: none !important;">
            <td colspan="9" style="float: none !important; width: 200px; padding-bottom: 5px !important; font-size: large; text-align: left;">
                <asp:Label ID="Errorid" runat="server" Font-Size="Medium" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
        <span id="Span1" class="tbl-large">
            <asp:Label ID="Label1" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_cheques_" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                EnableSortingAndPagingCallbacks="False" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE"
                BorderStyle="None" BorderWidth="1px" Width="97%">
                <RowStyle Font-Bold="false" />
                <Columns>
                    <asp:BoundField ItemStyle-HorizontalAlign="Left" HeaderText="Cheque No" DataField="ChequeNo"
                        ItemStyle-Width="15%" />
                    <asp:BoundField ItemStyle-HorizontalAlign="Right" HeaderText="Amount" DataField="Amount"
                        DataFormatString="{0:N0}" ItemStyle-Width="15%" />
                    <asp:BoundField HeaderText="Deposit Bank" DataField="depositBank" ItemStyle-Width="20%" />
                    <asp:TemplateField HeaderText="Cheque Status" ItemStyle-Width="50%">
                        <ItemTemplate>
                            <asp:RadioButtonList ID="rbtn_gChequeStatus" runat="server" Width="100%" CssClass="abc">
                            </asp:RadioButtonList>
                            <asp:HiddenField ID="hd_id" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                            <asp:HiddenField ID="hd_updateID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "updateID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="Redeem Amt" DataField="RedeemAmt" />--%>
                </Columns>
            </asp:GridView>
        </span>
    </div>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click" />
        &nbsp;
    </div>
</asp:Content>
