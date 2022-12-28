<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PaymentInstrument.aspx.cs" Inherits="MRaabta.Files.PaymentInstrument" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function Chk_ZoneCheck() {
            debugger;
            var dd_zone = document.getElementById('<%= dd_zone.ClientID %>');
            var dd_branch = document.getElementById('<%= dd_branch.ClientID %>');

            var chk_zone = document.getElementById('<%= chk_Allzone.ClientID%>');
            var chk_branch = document.getElementById('<%= chk_Allbranch.ClientID%>');


            if (chk_zone.checked) {
                dd_zone.disabled = true;
                dd_branch.disabled = true;
                chk_branch.checked = true;
                chk_branch.disabled = true;
            }
            else {
                dd_zone.disabled = false;
                chk_branch.disabled = false;
                if (chk_branch.checked) {
                    dd_branch.disabled = true;
                }
                else {
                    dd_branch.disabled = false;
                }
            }


        }

        
       
    </script>
    <style>
        .RadioList td
        {
            border: 0px !important;
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="float: left">
        <asp:Label ID="Errorid" runat="server" ForeColor="Red"></asp:Label>
    </div>
    <br />
    <fieldset class="input-form" style="width: 98% !important; margin-left: 1% !important;
        margin-right: 1% !important; padding: 0px !important;">
        <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
            padding-top: 0px !important; width: 100%">
            <tr>
                <td class="input-field" style="width: 100% !important; text-align: center !important;
                    font-variant: small-caps; font-size: larger; font-weight: bold;" colspan="8">
                    Payment Instrument Allocation
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 8% !important; padding-left: 1%;">
                    Zone
                </td>
                <td class="input-field" style="width: 25% !important;">
                    <asp:DropDownList ID="dd_zone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_zone_SelectedIndexChanged"
                        Width="60%" CssClass="dropdown" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">Select Zone</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="chk_Allzone" runat="server" Text="All" AutoPostBack="false" Width="15%"
                        onclick="Chk_ZoneCheck()" TextAlign="Right" />
                </td>
                <td class="space" style="margin: 0px !important; width: 1% !important;">
                </td>
                <td class="field" style="width: 8% !important;">
                    Branch
                </td>
                <td class="input-field" style="width: 25% !important;">
                    <asp:DropDownList ID="dd_branch" AutoPostBack="true" Width="60%" runat="server" CssClass="dropdown"
                        AppendDataBoundItems="true">
                        <asp:ListItem Value="0">Select Branch</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="chk_Allbranch" runat="server" Text="All" onclick="Chk_ZoneCheck()"
                        TextAlign="Right" Width="15%" />
                </td>
                <td class="space" style="margin: 0px !important; width: 1% !important;">
                </td>
                <td class="field" style="width: 8% !important;">
                    Account No.
                </td>
                <td class="input-field" style="width: 20% !important;">
                    <asp:TextBox ID="txt_accountNo" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 8% !important; padding-left: 1%;">
                    From Date
                </td>
                <td class="input-field" style="width: 25% !important;">
                    <asp:TextBox ID="txt_fromdate" runat="server" AutoPostBack="false" Width="60%"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                        Format="yyyy-MM-dd">
                    </Ajax1:CalendarExtender>
                </td>
                <td class="space" style="margin: 0px !important; width: 1% !important;">
                </td>
                <td class="field" style="width: 8% !important;">
                    To Date
                </td>
                <td class="input-field" style="width: 25% !important;">
                    <asp:TextBox ID="txt_todate" runat="server" AutoPostBack="false" Width="60%"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server"
                        Format="yyyy-MM-dd">
                    </Ajax1:CalendarExtender>
                </td>
                <td class="space" style="margin: 0px !important; width: 1% !important;">
                </td>
                <td class="field" style="width: 8% !important;">
                </td>
                <td class="input-field" style="width: 20% !important;">
                    <asp:Button ID="btn_search" runat="server" Text="Search" CssClass="button" OnClick="btn_search_Click"
                        Width="85%" OnClientClick="javascript:return Validate();" />
                </td>
            </tr>
            <tr>
                <td class="input-field" style="width: 24% !important; margin-left: 42%;" colspan="8">
                </td>
            </tr>
        </table>
    </fieldset>
    <div id="div2" class="tbl-large" runat="server" style="width: 100%; height: 360px;
        overflow: scroll;">
        <asp:GridView ID="gv_payments" runat="server" CssClass="mGrid" Width="67%" AutoGenerateColumns="false"
            OnRowDataBound="gv_payments_RowDataBound">
            <HeaderStyle />
            <Columns>
                <asp:TemplateField HeaderText="SNo.">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ZoneName" HeaderText="Zone" />
                <asp:BoundField DataField="BranchName" HeaderText="Branch" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="AccountNo" HeaderText="Acc.#"  ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="AccName" HeaderText="Account"  ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="BenName" HeaderText="Beneficiary Name"  ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="BenAccNo" HeaderText="Beneficiary Bank Acc#"  ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="BenBankCode" HeaderText="Bank Code"  ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="BenBankName" HeaderText="Bank Name"  ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="PaymentID" HeaderText="PaymentID" />
                <asp:BoundField DataField="PaidOn" HeaderText="Payment Date" />
                <asp:BoundField DataField="RRAmount" HeaderText="COD. AMT" DataFormatString="{0:N2}"  ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="InvoiceAmount" HeaderText="INV. AMT"  DataFormatString="{0:N2}"  ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="NetPayable" HeaderText="Net Paid"  DataFormatString="{0:N2}"  ItemStyle-HorizontalAlign="Right"/>
                <%--<asp:BoundField DataField="ChequeNo" HeaderText="CHEQUE #" />--%>
                <asp:TemplateField HeaderText="Instrument Mode">
                    <ItemTemplate>
                        <asp:RadioButtonList ID="rbtn_gMode" runat="server" Style="border: 0px !important;
                            border-collapse: collapse;" CssClass="RadioList" RepeatColumns="2" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">CHEQUE</asp:ListItem>
                            <asp:ListItem Value="2">IBFT</asp:ListItem>
                        </asp:RadioButtonList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Instrument No.">
                    <ItemTemplate>
                        <asp:TextBox ID="txt_gInstrumentNumber" runat="server" Style="overflow: hidden;"
                            Text='<%# DataBinder.Eval(Container.DataItem, "InstrumentNumber") %>'></asp:TextBox>
                        <asp:HiddenField ID="hd_zone" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ZoneCode") %>' />
                        <asp:HiddenField ID="hd_branch" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "BranchCode") %>' />
                        <asp:HiddenField ID="hd_creditClientID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CreditClientID") %>' />
                        <asp:HiddenField ID="hd_instrumentMode" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "InstrumentMode") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_chk" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="text-align: center; width: 100%;">
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" 
            onclick="btn_save_Click" />
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" 
            onclick="btn_reset_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
