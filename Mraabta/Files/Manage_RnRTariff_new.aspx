<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Manage_RnRTariff_new.aspx.cs" Inherits="MRaabta.Files.Manage_RnRTariff_new" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function loader() {
            document.getElementById('<%=loader.ClientID %>').style.display = "";
        }
        function AllowChange(evt) {
            var acc = document.getElementById('<%= txt_accountNo.ClientID %>');

            if (acc.value == "0") {
                alert('Cannot Change Cash Tariff');
                return false;
            }
            return true;
        }
    </script>
    <div runat="server" id="loader" style="background-color: rgb(238, 238, 238); float: left;
        height: 301px; opacity: 0.7; position: absolute; text-align: center; display: none;
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
            <td class="field" style="width: 10% !important;">
                Account No.
            </td>
            <td class="input-field" style="width: 17% !important">
                <asp:TextBox ID="txt_accountNo" runat="server" AutoPostBack="true" OnTextChanged="txt_accountNo_TextChanged"></asp:TextBox>
                <asp:HiddenField ID="creditclientid" runat="server" />
            </td>
            <td class="field" style="width: 10% !important;">
                Service Type
            </td>
            <td class="input-field" style="width: 17% !important">
                <asp:DropDownList ID="dd_serviceType" runat="server" AppendDataBoundItems="true"
                    CssClass="dropdown">
                    <asp:ListItem Value="0">Select ServiceType</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Client Name
            </td>
            <td class="input-field" style="width: 15% !important">
                <asp:TextBox ID="txt_clientName" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important">
            </td>
            <td class="input-field" style="width: 17% !important">
                <asp:Button ID="btn_showTariff" runat="server" Text="Show Tariff" CssClass="button"
                    Width="50%" Style="float: right; text-align: center !important; padding-left: 0px !important;
                    padding-right: 0px !important;" OnClick="btn_showTariff_Click" />
            </td>
            <td class="space">
            </td>
        </tr>
    </table>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important" class="input-form">
        <tr style="padding: 0px 0px 0px 0px !important;">
            <td colspan="9" style="padding-bottom: 1px !important; padding-top: 1px !important;">
                <h4 style="font-family: Calibri; margin: 0px !important;">
                    Weight Bracket Info.</h4>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                From Zone
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_fromCat" runat="server" AppendDataBoundItems="true" CssClass="dropdown">
                    <asp:ListItem Value="0">From Zone</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="field" style="width: 10% !important;">
                To Zone
            </td>
            <td class="input-field" style="width: 17% !important">
                <asp:DropDownList ID="dd_toCat" runat="server" AppendDataBoundItems="true" CssClass="dropdown">
                    <asp:ListItem Value="0">To Zone</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space">
            </td>
            <td class="space">
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Price
            </td>
            <td class="input-field" style="width: 15% !important">
                <asp:TextBox ID="txt_price" runat="server"></asp:TextBox>
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important;">
            </td>
            <td class="input-field" style="width: 17% !important">
                <asp:Button ID="btn_add" runat="server" Text="Add" CssClass="button" Width="50%"
                    Style="float: right; text-align: center !important; padding-left: 0px !important;
                    padding-right: 0px !important;" OnClick="btn_add_Click" />
            </td>
            <td class="space">
            </td>
        </tr>
    </table>
    <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
        <span id="Span1" class="tbl-large">
            <asp:Label ID="Label1" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_tariff_Actual" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                OnRowCommand="gv_tariff_Actual_RowCommand" EmptyDataText="No Data Available"
                OnRowDataBound="gv_tariff_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="ServiceTypeName" DataField="ServiceTypeName" />
                    <asp:BoundField HeaderText="From Category" DataField="FromZone_" />
                    <asp:BoundField HeaderText="To Category" DataField="ToZone_" />
                    <%--<asp:BoundField HeaderText="Price" DataField="Price" />--%>
                    <asp:TemplateField HeaderText="Price">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_gvPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "price") %>'
                                onkeypress="return AllowChange(event);" CssClass="newbox" Enabled="false"></asp:TextBox>
                            <asp:HiddenField ID="hd_id" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btn_delete" CssClass="button" runat="server" Text="Delete" CommandName="del"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                            <asp:HiddenField ID="hd_isupdated" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isUpdated") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:ButtonField CommandName="ed" ControlStyle-CssClass="button"
                        Text="Edit" ButtonType="Button" />--%>
                </Columns>
            </asp:GridView>
        </span>
        <asp:Button ID="Button1" runat="server" Text="Apply Default Tariff" CssClass="button"
            Visible="false" />
    </div>
    <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
        <span id="Table_1" class="tbl-large">
            <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_tariff" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                OnRowCommand="gv_tariff_RowCommand" EmptyDataText="No Data Available" OnRowDataBound="gv_tariff_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="ServiceTypeName" DataField="ServiceTypeName" />
                    <asp:BoundField HeaderText="From Category" DataField="FromZone_" />
                    <asp:BoundField HeaderText="To Category" DataField="ToZone_" />
                    <%--<asp:BoundField HeaderText="Price" DataField="Price" />--%>
                    <asp:TemplateField HeaderText="Price">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_gvPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "price") %>'
                                onkeypress="return AllowChange(event);" CssClass="newbox"></asp:TextBox>
                            <asp:HiddenField ID="hd_id" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btn_delete" CssClass="button" runat="server" Text="Delete" CommandName="del"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                            <asp:HiddenField ID="hd_isupdated" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isUpdated") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:ButtonField CommandName="ed" ControlStyle-CssClass="button"
                        Text="Edit" ButtonType="Button" />--%>
                </Columns>
            </asp:GridView>
        </span>
        <asp:Button ID="btn_applyDefault" runat="server" Text="Apply Default Tariff" CssClass="button"
            Visible="false" />
    </div>
    <div style="width: 100%; text-align: center; margin: 30px 0;">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
            OnClientClick="loader()" />
        &nbsp;
        <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel_Click" />
    </div>
</asp:Content>
