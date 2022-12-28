<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="VMMDpoExpenseLog.aspx.cs" Inherits="MRaabta.Files.VMMDpoExpenseLog" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .datebox
        {
            background: url(images/calendar_1.png) no-repeat;
            background-position: 2px 3px !important;
            padding: 0px 0px 0px 25px;
            width: 150px;
            height: 20px;
            border: 1px solid #CCC;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            border-radius: 5px;
            -moz-box-shadow: 0 1px 1px #ccc inset, 0 1px 0 #fff;
            -webkit-box-shadow: 0 1px 1px #CCC inset, 0 1px 0 #FFF;
        }
        
        .datebox:focus
        {
            background-color: #FFF;
            border-color: #CCC;
            outline: none;
            -moz-box-shadow: 0 0 0 1px #e8c291 inset;
            -webkit-box-shadow: 0 0 0 1px #E8C291 inset;
            box-shadow: 0 0 0 1px #CCC inset;
        }
        
        .txtbox
        {
            padding: 0px 0px 0px 2px;
            width: 170px;
            height: 20px;
            border: 1px solid #CCC;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            border-radius: 5px;
            -moz-box-shadow: 0 1px 1px #ccc inset, 0 1px 0 #fff;
            -webkit-box-shadow: 0 1px 1px #CCC inset, 0 1px 0 #FFF;
        }
        
        .txtbox:focus
        {
            background-color: #FFF;
            border-color: #ccc;
            outline: none;
            -moz-box-shadow: 0 0 0 1px #e8c291 inset;
            -webkit-box-shadow: 0 0 0 1px #E8C291 inset;
            box-shadow: 0 0 0 1px #ccc inset;
        }
        
        .ddlbox
        {
            border: 1px solid #CCC;
            border-radius: 5px;
            width: 180px;
            height: 20px;
        }
        
        .buttonload
        {
            background-color: #ccc; /* Green background */
            color: white; /* White text */
            padding: 12px 24px; /* Some padding */
            font-size: 16px; /* Set a font-size */
        }
        
        .fa
        {
            margin-left: -12px;
            margin-right: 8px;
        }
        
        .time
        {
            width: 120px;
            height: 20px;
        }
        
        #padding
        {
            padding-left: 10px;
        }
        
        #padding1
        {
            padding-right: 10px;
        }
    </style>
    <script type="text/javascript">
        function checkDate(sender, args) {
            if (sender._selectedDate > new Date()) {
                alert("You cannot select future date!");
                sender._selectedDate = new Date();
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }
        function ValidateExpense() {
            var division = "<%=ddl_products.ClientID %>";
            var dsr = "<%=dd_DSR.ClientID %>";
            var vtype = "<%=V_type.ClientID%>";
            var driver = "<%=dd_driver.ClientID%>";
            var dvan = "<%=dd_van.ClientID %>";
            var fueltype = "<%=ddl_fueltype.ClientID %>";

            if (document.getElementById(division).selectedIndex == 0) {
                alert("SELECT PRODUCT!");
                return false;
            }
            else if (document.getElementById(fueltype).selectedIndex == 0) {
                alert("SELECT FUEL TYPE!");
                return false;
            }
            else if (document.getElementById("<%=txt_fuel_amt.ClientID%>").value == "") {
                alert("ENTER FUEL AMOUNT!");
                document.getElementById("<%=txt_fuel_amt.ClientID%>").focus();
                return false;
            }

//            else if (document.getElementById(dsr).selectedIndex == 0) {
//                alert("SELECT DSR!");
//                return false;
//            }
            else if (document.getElementById("<%=txt_mtr.ClientID%>").value == "") {
                alert("ENTER METER READING (OPN)!");
                document.getElementById("<%=txt_mtr.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txt_mtr2.ClientID%>").value == "") {
                alert("ENTER METER READING (CURR)!");
                document.getElementById("<%=txt_mtr2.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById(vtype).checked == "") {
                alert("SELECT VEHICLE TYPE!");
                return false;
            }
            //else if (document.getElementById(driver).selectedIndex == 0) {
            //    alert("SELECT DRIVER!");
            //    return false;
            //}
            else if (document.getElementById(dvan).selectedIndex == 0) {
                alert("SELECT VEHICLE!");
                return false;
            }
            return true;
        }

        function ValidateGIN() {
            var division = "<%=ddl_products.ClientID %>";
            var dsr = "<%=dd_DSR.ClientID %>";

            if (document.getElementById(division).selectedIndex == 0) {
                alert("SELECT LOG DATE!");
                return false;
            }
            else if (document.getElementById(dsr).selectedIndex == 0) {
                alert("SELECT DSR!");
                return false;
            }
            return true;
        }

        function TotalKm() {
            var text1 = document.getElementById('<%= txt_mtr.ClientID %>');
            var text2 = document.getElementById('<%= txt_mtr2.ClientID %>');
            if (text1.value.length == 0 || text2.value.length == 0) {
//                text1.value = 0;
//                text2.value = 0;
            }

            var x = parseFloat(text1.value);
            var y = parseFloat(text2.value);

            document.getElementById('<%= txt_km.ClientID %>').value = y - x;
        }

    </script>
    <link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>VEHICLE LOG SHEET</legend>
        <div class="heading" id="heading" runat="server">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Label ID="Errorid" runat="server" ForeColor="Red"></asp:Label>
                    <div id="sub_menu">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="font-size: xx-small; color: Red;">
            REQUIRED (*)
        </div>
        <asp:UpdatePanel ID="mypanel" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td class="cell-two-mini" style="font-size: x-small;" width="40px" colspan="1">
                            <b>Log Date *</b>
                        </td>
                        <td class="cell-two-mini" id="padding" width="50px" colspan="1">
                            <asp:TextBox ID="logdate" runat="server" CssClass="datebox" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender2" OnClientDateSelectionChanged="checkDate"
                                TargetControlID="logdate" runat="server" Format="dd/MM/yyyy" PopupButtonID="Image1">
                            </Ajax1:CalendarExtender>
                        </td>
                        <td class="cell-two-mini" style="font-size: x-small;" width="50px" colspan="1">
                            <b>&nbsp;Zone *</b>
                        </td>
                        <td class="cell-two-mini" id="padding1" width="80px" colspan="1">
                            <asp:DropDownList ID="ddl_zone" CssClass="ddlbox" OnSelectedIndexChanged="ddl_zone_SelectedIndexChanged"
                                runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="cell-two-mini" id="padding" style="font-size: x-small;" width="30px" colspan="1">
                            <b>Routes *</b>
                        </td>
                        <td class="cell-two-mini" id="padding" width="80px" colspan="1">
                            <asp:DropDownList ID="ddl_routes" CssClass="ddlbox" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell-two-mini" style="font-size: x-small;" width="30px" colspan="1">
                            <b>Product *</b>
                        </td>
                        <td class="cell-two-mini" id="padding" width="80px" colspan="1">
                            <asp:DropDownList ID="ddl_products" CssClass="ddlbox" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                            <%--                            <asp:DropDownList ID="dd_ItemLevel1" runat="server" CssClass="ddlbox" AppendDataBoundItems="true"
                                AutoPostBack="true">
                                <asp:ListItem Value="0"> Select Product </asp:ListItem>
                                <asp:ListItem Value="1"> DOMESTIC </asp:ListItem>
                                <asp:ListItem Value="2"> ROADNRAIL </asp:ListItem>
                            </asp:DropDownList>--%>
                        </td>
                        <td class="cell-two-mini" id="padding1" style="font-size: x-small;" width="30px"
                            colspan="1">
                            <b>Branch *</b>
                        </td>
                        <td class="cell-two-mini" id="padding1" width="80px" colspan="1">
                            <asp:DropDownList ID="dd_br" OnSelectedIndexChanged="dd_br_SelectedIndexChanged"
                                CssClass="ddlbox" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="cell-two-mini" id="padding" width="30px" style="font-size: x-small;" colspan="1">
                            <b>Departure Date*</b>
                        </td>
                        <td class="cell-two-mini" id="padding" width="80px" colspan="1">
                            <asp:TextBox ID="deprtdate" CssClass="datebox" runat="server" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender1" OnClientDateSelectionChanged="checkDate"
                                TargetControlID="deprtdate" runat="server" Format="dd/MM/yyyy" PopupButtonID="Image1">
                            </Ajax1:CalendarExtender>
                        </td>
                        <td class="cell-two-mini" style="font-size: x-small;" width="30px" colspan="1">
                            <%-- <b>&nbsp;GC </b>--%>
                        </td>
                        <td class="cell-two-mini" style="font-size: x-small;" width="80px" colspan="1">
                            <asp:TextBox ID="txt_gc" Visible="false" CssClass="txtbox" runat="server" MaxLength="7"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator2" ControlToValidate="txt_gc" runat="server"
                                ErrorMessage="Numbers Allowed" Operator="DataTypeCheck" Type="String"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell-two-mini" style="font-size: x-small;" width="30px" colspan="1">
                            <b>Driver1 *</b>
                        </td>
                        <td class="cell-two-mini" id="padding" width="80px" colspan="1">
                            <asp:DropDownList ID="dd_DSR" CssClass="ddlbox" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="cell-two-mini" style="font-size: x-small;" width="40px" colspan="1">
                            <b>&nbsp;Meter Reading(Opn) *</b>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1" style="font-size: x-small;">
                            <asp:TextBox ID="txt_mtr" CssClass="txtbox" runat="server" MaxLength="7" onkeyup="TotalKm()"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator3" ControlToValidate="txt_mtr" runat="server"
                                ErrorMessage="Integers only please." Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                        </td>
                        <td class="cell-two-mini" id="padding" width="30px" style="font-size: x-small;" colspan="1">
                            <b>Departure Time*</b>
                        </td>
                        <td class="cell-two-mini" id="padding" width="80px" colspan="1" style="font-size: x-small;">
                            <telerik:RadTimePicker RenderMode="Lightweight" ID="RadTimePicker1" Width="150px"
                                displayvalueformat="hh:mm" selectorformat="hh:mm" runat="server" Skin="Web20">
                                <TimeView Interval="00:30:00" runat="server" />
                            </telerik:RadTimePicker>
                            <%--<asp:TextBox runat="server" ID="RadTimePicker1"></asp:TextBox>--%>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="RadTimePicker1"
                                ErrorMessage="Select time!" Font-Size="XX-Small" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell-two-mini" style="font-size: x-small;" width="30px" colspan="1">
                            <%--  <b>Driver2  </b>--%>
                        </td>
                        <td class="cell-two-mini" id="padding" width="80px" colspan="1">
                            <asp:DropDownList ID="dd_driver" Visible="false" CssClass="ddlbox" runat="server"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="cell-two-mini" style="font-size: x-small;" width="40px" colspan="1">
                            <b>&nbsp;Meter Reading(Curr) *</b>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1" style="font-size: x-small;">
                            <asp:TextBox ID="txt_mtr2" CssClass="txtbox" runat="server" MaxLength="10" onkeyup="TotalKm()"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txt_mtr2" runat="server"
                                ErrorMessage="Integers only please." Font-Size="XX-Small" ForeColor="Red" Operator="DataTypeCheck"
                                Type="Integer"></asp:CompareValidator>
                            <asp:CompareValidator runat="server" ID="cmpNumbers" ControlToValidate="txt_mtr"
                                ControlToCompare="txt_mtr2" Operator="LessThan" Type="Integer" Font-Size="XX-Small"
                                ForeColor="Red" ErrorMessage="Current should be greater than Open Reading." /><br />
                        </td>
                        <td class="cell-two-mini" id="padding" width="30px" style="font-size: x-small;" colspan="1">
                            <b>Arrival Date *</b>
                        </td>
                        <td class="cell-two-mini" id="padding" width="80px" colspan="1">
                            <asp:TextBox ID="ArvlDate" CssClass="datebox" runat="server" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender5" TargetControlID="ArvlDate" runat="server"
                                Format="dd/MM/yyyy" PopupButtonID="Image1">
                            </Ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell-two-mini" style="font-size: x-small;" width="30px" colspan="1">
                            <b>Vehicle *</b>
                        </td>
                        <td class="cell-two-mini" width="50px" id="padding" colspan="1">
                            <asp:RadioButtonList Style="font-size: xx-small;" AutoPostBack="true" runat="server"
                                ID="V_type" OnSelectedIndexChanged="V_type_SelectedIndexChanged" RepeatDirection="Horizontal"
                                Width="250px">
                                <asp:ListItem Value="01" Selected="True"><b>Company Mntd.</b></asp:ListItem>
                                <asp:ListItem Value="02"><b>Cont.Van</b></asp:ListItem>
                                <asp:ListItem Value="03"><b>Outsourced Van</b></asp:ListItem>
                            </asp:RadioButtonList>
                            <br />
                            <asp:DropDownList runat="server" Visible="true" CssClass="ddlbox" ID="dd_van" OnSelectedIndexChanged="Vechicle_type_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:TextBox ID="OutsVan" Visible="false" CssClass="txtbox" runat="server"></asp:TextBox>
                        </td>
                        <td class="cell-two-mini" style="font-size: x-small;" width="40px" colspan="1">
                            <table width="100px">
                                <tr>
                                    <td style="width: 71px;">
                                        <b>Total KM</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 71px;">
                                        <b>Operation Location *</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 71px; height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 71px;">
                                        <b>Nature Of Duty/Operation *</b>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <table>
                                <tr>
                                    <td style="height: 3px;">
                                        <asp:TextBox runat="server" ID="txt_km"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 3px;">
                                        <asp:DropDownList ID="ddl_oper_loc" CssClass="ddlbox" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 5px; height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddl_natureofduty" runat="server" CssClass="ddlbox">
                                            <%--<asp:ListItem Value="0"> SELECT OPERATION</asp:ListItem>--%>
                                            <asp:ListItem Value="1"> PICKUP </asp:ListItem>
                                            <asp:ListItem Value="2"> LINE-HALL </asp:ListItem>
                                            <asp:ListItem Value="3" Selected="True"> DELIVERY </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="cell-two-mini" id="padding" style="font-size: x-small;" width="40px" colspan="1">
                            <table width="100px">
                                <tr>
                                    <td style="width: 71px;">
                                        <b>Arrival Time *</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 71px; height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 71px;">
                                        <b>Fuel *</b>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="cell-two-mini" id="padding" style="font-size: x-small;" width="80px" colspan="1">
                            <table>
                                <tr>
                                    <td style="height: 3px;">
                                        <telerik:RadTimePicker RenderMode="Lightweight" ID="RadTimePicker2" Width="150px"
                                            displayvalueformat="hh:mm" selectorformat="hh:mm" runat="server" Skin="Web20">
                                            <TimeView Interval="00:30:00" runat="server" />
                                        </telerik:RadTimePicker>
                                        <%--<asp:TextBox runat="server" ID="RadTimePicker2"></asp:TextBox>--%>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="RadTimePicker2"
                                            ErrorMessage="Select time!" Font-Size="XX-Small" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 5px;">
                                        <asp:DropDownList ID="ddl_fueltype" runat="server" CssClass="ddlbox">
                                            <asp:ListItem Value="0"> SELECT FUEL TYPE</asp:ListItem>
                                            <asp:ListItem Value="1"> PETROL </asp:ListItem>
                                            <asp:ListItem Value="2"> CNG </asp:ListItem>
                                            <asp:ListItem Value="3"> DESIEL </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell-two-mini" style="font-size: x-small;" width="40px" colspan="1">
                            <table>
                                <tr>
                                    <td style="width: 71px;">
                                        <b>Vehicle Type *</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 71px; height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 71px;">
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="cell-two-mini" id="padding" width="80px" colspan="1">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="dd_vtype" CssClass="ddlbox" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="cell-two-mini" style="font-size: x-small;" width="40px" colspan="1">
                            <table>
                                <tr>
                                    <td style="width: 71px;">
                                        <%--<b>Helper 1</b>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 71px; height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 71px;">
                                        <%--<b>Helper 2</b>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <table>
                                <tr>
                                    <td>
                                        <b>
                                            <asp:TextBox Visible="false" runat="server" CssClass="txtbox" MaxLength="80" ID="txt_helper_1"></asp:TextBox></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <asp:TextBox Visible="false" runat="server" CssClass="txtbox" MaxLength="80" ID="txt_helper_2"></asp:TextBox></b>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="cell-two-mini" id="padding" style="font-size: x-small;" width="40px" colspan="1">
                            <table width="100px">
                                <tr>
                                    <td style="width: 71px;">
                                        <b>Fuel in ltr *</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 71px;">
                                        <b>Remarks</b>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="cell-two-mini" id="padding" width="80px" colspan="1">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt_fuel_amt" CssClass="txtbox" runat="server" MaxLength="10"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator4" ControlToValidate="txt_fuel_amt" runat="server"
                                            ErrorMessage="Enter Amount In Numbers" Font-Size="XX-Small" ForeColor="Red" Operator="DataTypeCheck"
                                            Type="Integer"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <asp:TextBox runat="server" MaxLength="200" Height="20px" CssClass="txtbox" TextMode="MultiLine"
                                                ID="txt_remrks"></asp:TextBox></b>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btn_Submit" runat="server" Visible="true" OnClientClick="javascript:return ValidateExpense();"
                                CssClass="button" OnClick="btn_Submit_Click" Text="Save Vehicle Log" />
                            <asp:Button ID="btn_update" runat="server" Visible="false" OnClientClick="javascript:return ValidateExpense();"
                                CssClass="button" OnClick="btn_update_Click" Text="Update Vehicle Log" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <fieldset>
        <br />
        <br />
        <div id="dv_edit" visible="true" runat="server">
            <asp:GridView ID="gv_edit" runat="server" AutoGenerateColumns="False" CellPadding="4"
                ForeColor="#333333" GridLines="Both" Width="100%" OnRowCommand="gv_data_RowCommand"
                HeaderStyle-HorizontalAlign="Left" CssClass="font-style" ShowHeaderWhenEmpty="true">
                <AlternatingRowStyle BackColor="#f5f5f5" />
                <Columns>
                    <asp:BoundField DataField="log_no" HeaderText="LOG NO" />
                    <asp:BoundField DataField="log_date" HeaderText="LOG DATE" />
                    <asp:BoundField DataField="driver1" HeaderText="DRIVER-1" />
                    <asp:BoundField DataField="driver2" HeaderText="DRIVER-2" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lb_edit" runat="server" Text="EDIT" CommandArgument='<% #Eval("log_no")%>'
                                CommandName="EDIT"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" ForeColor="White" />
                <HeaderStyle BackColor="#ee701b" ForeColor="White" BorderColor="White" />
                <PagerStyle BackColor="#f5f5f5" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#f5f5f5" />
            </asp:GridView>
        </div>
    </fieldset>
</asp:Content>
