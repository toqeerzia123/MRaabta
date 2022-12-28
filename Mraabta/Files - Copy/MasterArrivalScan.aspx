<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="MasterArrivalScan.aspx.cs" Inherits="MRaabta.Files.MasterArrivalScan" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function FnKeyPress(sender, e) {
            var curkey = e.which || e.keyCode;
            if (curkey == 9) {
                __doPostBack(sender.id, "");
                return false;
            }
        }

    </script>
    <script language="javascript" type="text/javascript">
        function CheckNumeric(e) {

            if (window.event) // IE 
            {
                if ((e.keyCode < 48 || e.keyCode > 57) & e.keyCode != 8 & e.keyCode == 9) {
                    event.returnValue = false;
                    return false;

                }
            }
            else { // Fire Fox
                if ((e.which < 48 || e.which > 57) & e.which != 8 & e.which == 9) {
                    e.preventDefault();
                    return false;

                }
            }
        }
        function isNumberKey(evt) {

            var count = 1;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
                return false;
            }
            else {

                if (charCode == 110 || charCode == 46) {
                    count++;
                }
                if (count > 1) {
                    return false
                }
            }

            return true;
        }
        function isNumberKeyDecimal(evt, txt) {


            var count = 0;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
                return false;
            }
            else {

                if (charCode == 110 || charCode == 46) {
                    count++;
                    if (txt.value.includes('.')) {
                        return false;
                    }
                }
                if (count > 1) {
                    return false
                }
            }

            return true;
        }
        function checkRider(txt) {
            var txt_cn = document.getElementById('<%= txt_consignment.ClientID %>');

            var rider = document.getElementById('<%= txt_ridercode.ClientID %>');
            if (rider.value.trim() == "") {
                alert('Enter Rider Code');
                txt.value = "";
                rider.focus();
                return false;


            }

            var grid = document.getElementById('<%= GridView.ClientID %>');
            if (grid != null) {
                for (var i = 1; i < grid.rows.length; i++) {
                    if (txt_cn.value.trim() == grid.rows[i].cells[1].innerText) {
                        alert('Consignment Already Scanned');
                        txt_cn.value = "";
                        return false;
                    }
                }
            }
            var controlGrid = document.getElementById('<%= cnControls.ClientID %>');
            var prefixNotFound = false;
            var message = "";

            for (var i = 1; i < controlGrid.rows.length; i++) {
                var row = controlGrid.rows[i];
                var prefix = row.cells[0].innerText;
                var length_ = parseInt(row.cells[1].innerText);
                if (prefix == "52190") {
                    var a = 0;
                }
                if (txt_cn.value.substring(0, prefix.length) == prefix) {
                    if (txt_cn.value.length != length_) {
                        message = "Invalid Length of CN";

                        prefixNotFound = true;
                    }
                    else {

                        prefixNotFound = false;
                        break;
                    }
                }
                else {
                    if (message == "") {
                        message = "Invalid Prefix";
                    }

                    prefixNotFound = true;

                }
            }

            if (prefixNotFound) {
                alert(message);
                txt_cn.value = "";
                txt_cn.focus();
                return false;
            }




            return true;
        }
        function ChangeMode(rb) {

            var txt_arrival = document.getElementById('<%= txt_arrivalID.ClientID %>');
            var rbt = document.getElementById('<%= rbtn_mode.ClientID %>');
            var inputs = rbt.getElementsByTagName('input');
            var selectedValue = "";
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    selectedValue = inputs[i].value;
                    break;
                }

            }

            if (selectedValue == "0") {
                txt_arrival.disabled = true;
            }
            else {
                txt_arrival.disabled = false;
            }
        }
        function CalculateTotalWeight() {
            debugger;
            var grid = document.getElementById('<%= GridView.ClientID %>');
            var txt_weight = document.getElementById('<%= txt_weight.ClientID %>');
            var totalWeight = 0;
            for (var i = 1; i < grid.rows.length; i++) {
                var tr = grid.rows[i];

                var trWeight = tr.cells[4].children[0].value;

                var floatWeight = 0;
                floatWeight = parseFloat(trWeight);

                if (!isNaN(floatWeight)) {
                    totalWeight += floatWeight;
                }
            }
            txt_weight.value = totalWeight;

        }
    </script>
    <script src="../Js/FusionCharts.js" type="text/javascript"></script>
    <body>
        <div>
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <script type="text/javascript">
                        function loader() {
                            document.getElementById('<%=loader.ClientID %>').style.display = "";
                        }
                    </script>
                    <div runat="server" id="loader" style="float: left; opacity: 0.7; position: absolute; text-align: center; display: none; top: 50%; width: 84% !important;">
                        <div class="loader">
                            <img src="../images/Loading_Movie-02.gif" />
                        </div>
                    </div>
                    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                        <tr>
                            <td colspan="12" align="center" class="head_column">
                                <h3>Master Arrival Scan
                                </h3>
                            </td>
                        </tr>
                    </table>
                    <style>
                        .search {
                            float: right;
                            width: 10%;
                            background: #5f5a8d;
                            padding: 3px;
                            position: relative;
                            right: 99px;
                            margin: 0px 0px 15px;
                            top: 7px;
                            text-align: center;
                        }

                            .search a {
                                color: #fff;
                                text-decoration: none;
                            }
                    </style>
                    <div class="search">
                        <a href="SearchArrivalScan.aspx">Search Arrival Scan</a>
                    </div>
                    <table class="input-form" style="width: 90%">
                        <tr>
                            <td class="field">Arrival Mode
                            </td>
                            <td class="input-field" style="width: 33%;">
                                <span style="float: left; width: 80%">
                                    <asp:RadioButtonList ID="rbtn_mode" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                        onclick="ChangeMode(this)">
                                        <asp:ListItem Value="0" Selected="True">New</asp:ListItem>
                                        <asp:ListItem Value="1">Edit</asp:ListItem>
                                    </asp:RadioButtonList>
                                </span>
                            </td>
                            <td class="space"></td>
                            <td class="field">Arrival ID
                            </td>
                            <td class="input-field">
                                <asp:TextBox ID="txt_arrivalID" runat="server" Enabled="false" AutoPostBack="true" OnTextChanged="txt_arrivalID_TextChanged1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="field">Rider Code:
                            </td>
                            <td class="input-field" style="width: 33%;">
                                <span style="float: left; width: 80%">
                                    <asp:TextBox ID="txt_ridercode" runat="server" OnTextChanged="txt_ridercode_TextChanged"
                                        AutoPostBack="true" Style="width: 96%; padding: 4px;"></asp:TextBox>
                                </span>
                            </td>
                            <td class="space"></td>
                            <td class="field">Rider Name:
                            </td>
                            <td class="input-field">
                                <asp:TextBox ID="txt_ridername" runat="server" Columns="8"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="field">Total Weight:
                            </td>
                            <td class="input-field" style="width: 33%;">
                                <span style="float: left; width: 80%">
                                    <asp:TextBox ID="txt_weight" runat="server"
                                        Enabled="false" Style="width: 96%; padding: 4px;"></asp:TextBox>
                                    <%--ontextchanged="txt_weight_TextChanged1"--%>
                                </span>
                            </td>
                            <td class="space"></td>
                            <td class="field">Service Type:
                            </td>
                            <td class="input-field" style="width: 30%;">
                                <span style="float: left; width: 85%">
                                    <asp:DropDownList ID="dd_servicetype" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hd_ExpressCenter" runat="server" />
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="field">Consignment Type:
                            </td>
                            <td class="input-field" style="width: 33%;">
                                <span style="float: left; width: 80%">
                                    <asp:DropDownList ID="dd_contype" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </span>
                            </td>
                            <td class="space"></td>
                            <td class="field">Consignment No:
                            </td>
                            <td class="input-field">
                                <%--  <telerik:RadNumericTextBox RenderMode="Lightweight" ID="txt_consignment" runat="server"
                            resolvedrendermode="Classic" Width="100%" Skin="Web20" MaxLength="20" OnTextChanged="txt_consignment_TextChanged"
                            DataType="System.Int32">
                        </telerik:RadNumericTextBox>--%>
                                <asp:TextBox ID="txt_consignment" runat="server" MaxLength="15" AutoPostBack="True"
                                    onkeypress="return isNumberKey(event);" onchange="if ( checkRider(this) == false ) return;"
                                    Width="230px" OnTextChanged="txt_consignment_TextChanged">
                                </asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div style="position: relative; width: 50px; left: 15px;">
                        <asp:Button ID="Button1" runat="server" Text="Save" CssClass="button1" OnClick="Btn_Save_Click"
                            OnClientClick="loader();" CausesValidation="false" UseSubmitBehavior="false" />
                    </div>
                    <asp:Label ID="error_msg" runat="server" CssClass="error_msg"></asp:Label>
                    <div style="float: left; text-align: right; width: 96%; margin: 0px 0px 10px; font-weight: bold;">
                        <asp:Literal ID="lbl_count" runat="server"></asp:Literal>
                    </div>
                    <span id="Table_1" class="tbl-large">
                        <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                            AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE"
                            BorderStyle="None" AllowPaging="false"
                            BorderWidth="1px" OnRowDeleting="OnRowDeleting" OnRowCommand="GridView_RowCommand"
                            OnDataBound="GridView_DataBound" OnRowDataBound="OnRowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Button ID="btn_remove" runat="server" Text="Remove" CssClass="button" CommandName="remove"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ConsignmentNumber") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Consignment No" DataField="ConsignmentNumber" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="10%"></asp:BoundField>
                                <asp:BoundField HeaderText="Service Type" DataField="ServiceTypeName" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="10%"></asp:BoundField>
                                <asp:BoundField HeaderText="Consignment Type" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="10%"></asp:BoundField>
                                <asp:TemplateField HeaderText="Weight" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_gWeight" runat="server" onkeypress="return isNumberKeyDecimal(event, this);" onchange="CalculateTotalWeight();" Style="overflow: hidden;" Text='<%# DataBinder.Eval(Container.DataItem, "Weight") %>'
                                            Width="85%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pieces" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_gPieces" runat="server" Style="overflow: hidden;" Text='<%# DataBinder.Eval(Container.DataItem, "Pieces") %>'
                                            Width="85%" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:HiddenField ID="hd_arrived" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "arrived") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField HeaderText="Weight" DataField="Weight" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="10%"></asp:BoundField>--%>
                                <asp:BoundField HeaderText="Rider Code" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                                <asp:BoundField HeaderText="Rider Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </span>
                    <div style="position: relative; width: 50px; left: 15px;">
                        <asp:Button ID="submit" runat="server" Text="Save" CssClass="button1" OnClick="Btn_Save_Click"
                            CausesValidation="false" UseSubmitBehavior="false" OnClientClick="loader();" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="display: none;">
                <asp:GridView ID="cnControls" runat="server">
                    <Columns>
                        <asp:BoundField HeaderText="Prefix" DataField="Prefix" />
                        <asp:BoundField HeaderText="Length" DataField="Length" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
</asp:Content>
