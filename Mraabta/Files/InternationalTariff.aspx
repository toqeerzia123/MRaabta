<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="InternationalTariff.aspx.cs" Inherits="MRaabta.Files.InternationalTariff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function loader() {
            document.getElementById('<%=loader.ClientID %>').style.display = "";
        }
    </script>
    <div runat="server" id="loader" style="background-color: rgb(238, 238, 238); float: left;
        height: 442px; opacity: 0.7; position: absolute; text-align: center; display: none;
        top: 11%; width: 84% !important;">
        <div class="loader">
        </div>
    </div>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important" class="input-form">
        <tr style="padding: 0px 0px 0px 0px !important;">
            <td colspan="9" style="padding-bottom: 1px !important; padding-top: 1px !important;">
                <h4 style="font-family: Calibri; margin: 0px !important;">
                    Customer Info.</h4>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Branch
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_branch" runat="server" AppendDataBoundItems="true" CssClass="dropdown">
                    <asp:ListItem Value="0">Select Branch</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important;">
                Account No.
            </td>
            <td class="input-field" style="width: 17% !important">
                <asp:TextBox ID="txt_accountNo" runat="server" AutoPostBack="true" OnTextChanged="txt_accountNo_TextChanged"></asp:TextBox>
                <asp:HiddenField ID="creditclientid" runat="server" />
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important">
                ServiceType
            </td>
            <td class="input-field" style="width: 17% !important">
                <asp:DropDownList ID="dd_serviceType" runat="server" CssClass="dropdown" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space">
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                From Zone
            </td>
            <td class="input-field" style="width: 15% !important">
                <asp:TextBox ID="txt_fromZone" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important;">
                Client Name
            </td>
            <td class="input-field" style="width: 17% !important">
                <asp:TextBox ID="txt_clientName" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important">
                To Zone
            </td>
            <td class="input-field" style="width: 17% !important">
                <asp:DropDownList ID="dd_zone" runat="server" AppendDataBoundItems="true" CssClass="dropdown">
                    <asp:ListItem Value="0">Select Zone</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space">
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
            </td>
            <td class="input-field" style="width: 15% !important">
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important;">
            </td>
            <td class="input-field" style="width: 17% !important">
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important">
            </td>
            <td class="input-field" align="right" style="width: 25% !important;">
                <asp:Button ID="btn_showTariff" runat="server" Text="Show Tariff" CssClass="button"
                    Width="50%" Style="float: right; text-align: center !important; padding-left: 0px !important;
                    padding-right: 0px !important;" OnClick="btn_showTariff_Click" />
                <br />
                <asp:Button ID="btn_Default" runat="server" Text="Apply Default" CssClass="button"
                    Width="50%" Style="float: right; text-align: center !important; padding-left: 0px !important;
                    padding-right: 0px !important;" OnClick="btn_Default_Click" Enabled="false" />
            </td>
            <td class="space">
            </td>
        </tr>
    </table>
    <table style="font-family: Calibri; font-size: medium;" class="input-form">
        <tr>
            <td colspan="9">
                <h4 style="font-family: Calibri; margin: 0px !important;">
                    Currency Info.</h4>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Currency
            </td>
            <td class="input-field" style="width: 15% !important">
                <asp:DropDownList ID="dd_currency" runat="server" OnSelectedIndexChanged="dd_currency_SelectedIndexChanged"
                    AutoPostBack="true" Width="90%">
                </asp:DropDownList>
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important;">
            </td>
            <td class="input-field" style="width: 17% !important">
                <asp:TextBox ID="txt_currency" runat="server"></asp:TextBox>
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important">
            </td>
            <td class="input-field" style="width: 17% !important">
                <%--<asp:Button ID="btn_addWeight" runat="server" CssClass="button" Width="50%" Text="Add Weight"
                    Style="float: right; text-align: center !important; padding-left: 0px !important;
                    padding-right: 0px !important;" OnClick="btn_addWeight_Click" />--%>
            </td>
            <td class="space">
            </td>
        </tr>
    </table>
    <table style="font-family: Calibri; font-size: medium;" class="input-form">
        <tr>
            <td colspan="13">
                <h4 style="font-family: Calibri; margin: 0px !important;">
                    Weight Bracket Info.</h4>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                From Weight
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_fromWeight" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0 5% !important;">
            </td>
            <td class="field" style="width: 10% !important;">
                To Weight
            </td>
            <td class="input-field" style="width: 12% !important">
                <asp:TextBox ID="txt_toWeight" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0 5% !important;">
            </td>
            <td class="field" style="width: 8% !important">
                Price
            </td>
            <td class="input-field" style="width: 12% !important">
                <asp:TextBox ID="txt_price" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0 10px !important;">
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Add. Weight
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_addWeight" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0 5% !important;">
            </td>
            <td class="field" style="width: 10% !important;">
                Add. Price
            </td>
            <td class="input-field" style="width: 12% !important">
                <asp:TextBox ID="txt_addPrice" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0 5% !important;">
            </td>
            <td class="field" style="width: 8% !important">
            </td>
            <%--<td class="input-field" style="width: 12% !important">
                
            </td>--%>
            <td class="input-field" style="width: 12% !important">
                <asp:Button ID="btn_addPrice" runat="server" CssClass="button" Width="90%" Text="Add Price"
                    Style="float: right; text-align: center !important; padding-left: 0px !important;
                    padding-right: 0px !important;" OnClick="btn_addPrice_Click" />
            </td>
            <td class="space" style="margin: 0 10px !important;">
            </td>
        </tr>
    </table>
    <div style="width: 100%; height: 250px; overflow: scroll;">
        <span id="Table_1" class="tbl-large">
            <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_tariff" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                OnRowCommand="gv_tariff_RowCommand" EmptyDataText="No Tarrif Available">
                <Columns>
                    <asp:BoundField HeaderText="Destination" DataField="Destination" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="From Weight" DataField="FromWeight" />
                    <asp:BoundField HeaderText="To Weight" DataField="ToWeight" />
                    <asp:BoundField HeaderText="Price" DataField="Price" />
                    <asp:BoundField HeaderText="Additional Weight" DataField="addFactor" />
                    <asp:BoundField HeaderText="Additional Price" DataField="addFactorPrice" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btn_delete" runat="server" Text="Delete" CommandName="del" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                            <asp:HiddenField ID="hd_destinationID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "DestinationID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </span>
    </div>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
            OnClientClick="loader()" />
        &nbsp;
        <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel_Click" />
    </div>
</asp:Content>
