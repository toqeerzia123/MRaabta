<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Manage_Tariff_Sim.aspx.cs" Inherits="MRaabta.Files.Manage_Tariff_Sim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            border-radius: 5px !important;
            color: White;
            font-family: Calibri;
            font-size: small;
            cursor: pointer;
            float: right;
            border-style: none;
            border-color: inherit;
            border-width: 0;
            padding: 3px 20px;
            background-color: #5f5a8d;
        }
        .style2
        {
            border-radius: 5px !important;
            color: White;
            font-family: Calibri;
            font-size: small;
            cursor: pointer;
            border-style: none;
            border-color: inherit;
            border-width: 0;
            padding: 3px 20px;
            background-color: #5f5a8d;
        }
    </style>
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
            <asp:HyperLink ID="hyp_1" runat="server" Target="_blank"></asp:HyperLink>
            </td>
            <td class="input-field" style="width: 17% !important">
                <%--   <asp:DropDownList ID="dd_toZone" runat="server" AppendDataBoundItems="true" CssClass="dropdown">
                    <asp:ListItem Value="0">Select Zone</asp:ListItem>
                </asp:DropDownList>--%>
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
            <td class="input-field" style="width: 17% !important">
                <asp:Button ID="btn_showTariff" runat="server" Text="Show Tariff" CssClass="style2"
                    Width="50%" Style="float: right; text-align: center; padding-left: 0px; padding-right: 0px;"
                    OnClick="btn_showTariff_Click" />
            </td>
            <td class="space">
            </td>
        </tr>
    </table>
    <table style="font-family: Calibri; font-size: medium;" class="input-form">
        <tr>
            <td colspan="9">
                <h4 style="font-family: Calibri; margin: 0px !important;">
                    Additional Weight Info.</h4>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Add. Weight
            </td>
            <td class="input-field" style="width: 15% !important">
                <asp:TextBox ID="txt_additionalWeight" runat="server"></asp:TextBox>
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
            <td class="input-field" style="width: 17% !important">
                <asp:Button ID="btn_addWeight" runat="server" CssClass="button" Width="50%" Text="Add Weight"
                    Style="float: right; text-align: center !important; padding-left: 0px !important;
                    padding-right: 0px !important;" OnClick="btn_addWeight_Click" Visible="false" />
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
            <td class="space" style="margin: 0 10px !important;">
            </td>
            <td class="field" style="width: 10% !important;">
                To Weight
            </td>
            <td class="input-field" style="width: 12% !important">
                <asp:TextBox ID="txt_toWeight" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0 10px !important;">
            </td>
            <td class="field" style="width: 10% !important">
            </td>
            <td class="input-field" style="width: 12% !important">
            </td>
            <td class="space" style="margin: 0 10px !important;">
            </td>
            <td class="field" style="width: 8% !important">
            </td>
            <td class="input-field" style="width: 12% !important">
                <asp:Button ID="btn_addPrice" runat="server" CssClass="style1" Text="Add Price" Style="text-align: center;
                    padding-left: 0px; padding-right: 0px;" OnClick="btn_addPrice_Click" />
            </td>
            <td class="space" style="margin: 0 10px !important;">
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
            <td class="input-field" style="width: 17% !important">
            </td>
            <td class="space">
            </td>
        </tr>
    </table>
    <div style="width: 100%; height: 250px; overflow: scroll;">
        <span id="Table_1" class="tbl-large">
            <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
            <asp:UpdatePanel ID="up_1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gv_tariff" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                        AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                        OnRowCommand="gv_tariff_RowCommand" EmptyDataText="No Tariff Available" OnDataBound="gv_tariff_DataBound"
                        OnRowDataBound="gv_tariff_RowDataBound" OnSelectedIndexChanged="gv_tariff_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField HeaderText="PR">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_pr" runat="server" AutoPostBack="true" OnCheckedChanged="cb_pr_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="From Weight" DataField="FromWeight" />
                            <asp:BoundField HeaderText="To Weight" DataField="ToWeight" />
                            <asp:BoundField HeaderText="Additional Weight" DataField="addFactor" />
                            <asp:TemplateField HeaderText="LOCAL">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Local" Width="50px" runat="server" MaxLength="11"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SAME">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Same" Width="50px" runat="server" MaxLength="11"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DIFF">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Diff" Width="50px" runat="server" MaxLength="11"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                    <asp:HiddenField ID="isUpdated" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isUPDATED") %>' />
                                    <asp:HiddenField ID="hd_id" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                    <asp:HiddenField ID="hd_isupdated" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isUpdated") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btn_delete" runat="server" Text="Delete" CommandName="del" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                    <asp:HiddenField ID="isUpdated_" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isUPDATED") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </span>
    </div>
    <div style="width: 100%; height: 250px; overflow: scroll;">
        <span id="Span1" class="tbl-large">
            <asp:Label ID="Label1" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_tariff_" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" 
            BorderStyle="None" BorderWidth="1px"
                EmptyDataText="No Tariff Available" 
            onrowcommand="gv_tariff__RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="Destination" DataField="Destination" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="Service Type" DataField="ServiceID" />
                    <asp:BoundField HeaderText="From Weight" DataField="FromWeight" />
                    <asp:BoundField HeaderText="To Weight" DataField="ToWeight" />
                    <asp:BoundField HeaderText="Price" DataField="Price" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" Text="Delete" CommandName="del" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isUPDATED") %>' />
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
            OnClientClick="return confirm('Are you sure, Previous Tariffs will be Updated')" />
        &nbsp;
        <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel_Click" />
        &nbsp;
        <asp:Button ID="btn_applyDefaultTariff" runat="server" Text="Apply Default Tariff"
            CssClass="button" OnClientClick="return confirm('Are you sure you wish to apply Default Tariff?');"
            OnClick="btn_applyDefaultTariff_Click" Visible="false" />
    </div>
</asp:Content>
