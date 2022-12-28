<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PrebookingDataLoader.aspx.cs" Inherits="MRaabta.Files.PrebookingDataLoader" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function Reset() {
            var win = window.open('PrebookingDataLoader.aspx', '_parent');
            if (win) {
                win.focus();
                return false;
            }
        }
        function LoaderStart() {
            loader.style.display = 'block';
        }
    </script>
    <div id="loader" style="background-color: rgb(238, 238, 238); float: left; height: 500px;
        opacity: 0.7; position: absolute; text-align: center; display: none; top: 11%;
        width: 84% !important;">
        <div class="loader">
        </div>
    </div>
    <div style="width: 100%; text-align: center;">
        <asp:Label ID="Errorid" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
    </div>
    <table style="width: 90%; border-collapse: collapse; margin-left: 5%; margin-right: 5%;
        font-family: Calibri; font-size: small;">
        <tr>
            <td colspan="6" style="text-align: center; font-size: medium; font-variant: small-caps;
                font-weight: bold; padding-top: 10px; padding-bottom: 15px;">
                Pre-Booking Data Loader
                <br />
            </td>
        </tr>
        <tr>
            <td style="width: 10%; padding-left: 5px; float: left; font-weight: bold; padding-bottom: 10px;">
                Date
            </td>
            <td style="width: 20%; padding-left: 5px; float: left; font-weight: bold; padding-bottom: 10px;">
                <asp:TextBox ID="txt_date" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                <Ajax1:CalendarExtender ID="calendar1" runat="server" TargetControlID="txt_date"
                    Format="yyyy-MM-dd">
                </Ajax1:CalendarExtender>
            </td>
            <td style="width: 10%; padding-left: 5px; float: left; font-weight: bold; padding-bottom: 10px;">
                Account Number
            </td>
            <td style="width: 15%; padding-left: 5px; float: left; font-weight: bold; padding-bottom: 10px;">
                <asp:TextBox ID="txt_AccountNumber" runat="server" Width="90%" AutoPostBack="True"
                    OnTextChanged="txt_AccountNumber_TextChanged"></asp:TextBox>
                <asp:HiddenField ID="hd_clientID" runat="server" />
            </td>
            <td style="width: 10%; padding-left: 5px; float: left; font-weight: bold; padding-bottom: 10px;">
                Account Name
            </td>
            <td style="width: 30%; padding-left: 5px; float: left; font-weight: bold; padding-bottom: 10px;">
                <asp:TextBox ID="txt_accountName" runat="server" Width="90%" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 10%; padding-left: 5px; float: left; font-weight: bold; padding-bottom: 10px;">
                File
            </td>
            <td style="width: 55%; padding-left: 5px; float: left; font-weight: bold; padding-bottom: 10px;"
                colspan="5">
                <asp:FileUpload ID="upload1" runat="server" Width="95%" Enabled="true" ViewStateMode="Enabled" />
            </td>
        </tr>
        <tr>
            <td style="width: 10%; padding-left: 5px; float: left; font-weight: bold; padding-bottom: 10px;">
                &nbsp;
            </td>
            <td style="width: 15%; text-align: center; float: left; text-align: left; padding-bottom: 10px;">
                <asp:Button ID="btn_upload" runat="server" Text="Upload File" CssClass="button" OnClick="btn_upload_Click"
                    OnClientClick="LoaderStart();" />
            </td>
        </tr>
        <tr>
            <td style="width: 100%; text-align: left; font-weight: bold;">
                Please Note: Excel file must be in CSV Format and must only contain 'ReferenceNo, Consignee Name, Consignee
                Address and Consignee Phone Number' with the following Lengths.
                <br />
                ReferenceNo (50)
                <br />
                Consignee Name (70)
                <br />
                Consignee Address (200)
                 <br />
                Consignee Phone Number (12)
            </td>
        </tr>
    </table>
    <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
        <span id="Table_1" class="tbl-large">
            <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_consignments" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                EmptyDataText="No Errors Found in Data.">
                <Columns>
                    <asp:BoundField HeaderText="Ref No." DataField="RefNo" />
                    <asp:BoundField HeaderText="Account #." DataField="AccountNo" />
                    <asp:BoundField HeaderText="Origin" DataField="OriginName" />
                    <asp:BoundField HeaderText="Consignee" DataField="Consignee" />
                    <asp:TemplateField HeaderText="Address">
                        <ItemTemplate>
                            <asp:Label ID="lbl_gAddress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Address") %>'></asp:Label>
                            <asp:HiddenField ID="hd_origin" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "orgin") %>' />
                            <asp:HiddenField ID="hd_creditClientID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CreditClientID") %>' />
                            <asp:HiddenField ID="hd_refDate" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "refDate") %>' />
                            <asp:HiddenField ID="hd_duplicate" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Consigner" DataField="Consigner" />
                </Columns>
            </asp:GridView>
        </span>
    </div>
    <div style="width: 100%; text-align: center;">
        <%--<asp:Button ID="btn_Save" runat="server" CssClass="button" Text="Save File" OnClick="btn_Save_Click"
                    OnClientClick="LoaderStart();" />--%>
        <button id="btn_reset" class="button" onclick="return Reset();">
            Reset</button>
        &nbsp;
    </div>
</asp:Content>
