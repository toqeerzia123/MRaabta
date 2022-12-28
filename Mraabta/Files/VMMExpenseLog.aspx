<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="VMMExpenseLog.aspx.cs" Inherits="MRaabta.Files.VMMExpenseLog" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .datebox {
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

            .datebox:focus {
                background-color: #FFF;
                border-color: #CCC;
                outline: none;
                -moz-box-shadow: 0 0 0 1px #e8c291 inset;
                -webkit-box-shadow: 0 0 0 1px #E8C291 inset;
                box-shadow: 0 0 0 1px #CCC inset;
            }

        .txtbox {
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

            .txtbox:focus {
                background-color: #FFF;
                border-color: #ccc;
                outline: none;
                -moz-box-shadow: 0 0 0 1px #e8c291 inset;
                -webkit-box-shadow: 0 0 0 1px #E8C291 inset;
                box-shadow: 0 0 0 1px #ccc inset;
            }

        .ddlbox {
            border: 1px solid #CCC;
            border-radius: 5px;
            width: 180px;
            height: 20px;
        }

        .buttonload {
            background-color: #ccc; /* Green background */
            color: white; /* White text */
            padding: 12px 24px; /* Some padding */
            font-size: 16px; /* Set a font-size */
        }

        .fa {
            margin-left: -12px;
            margin-right: 8px;
        }

        .time {
            width: 120px;
            height: 20px;
        }

        .ltrKgLabel:after {
            content: " Ltr./Kg";
        }

        .style1 {
            width: 120px;
            padding: 2px;
            vertical-align: top;
            height: 24px;
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
            var division = "<%=dd_ItemLevel1.ClientID %>";
            var dsr = "<%=dd_DSR.ClientID %>";
            var vtype = "<%=V_type.ClientID%>";
            var Vradio = document.getElementsByName("V_type");
            var edate = "<%=expensedate.ClientID%>";
            var mainc = "<%=CategoryDropDownList.ClientID%>";
            var smc = "<%=SubCategory.ClientID%>";

            if (document.getElementById("<%=expensedate.ClientID%>").value == "") {
                alert("ENTER EXPENSE DATE!");
                document.getElementById("<%=expensedate.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById(division).selectedIndex == 0) {
                alert("SELECT DIVISION!");
                document.getElementById("<%=dd_ItemLevel1.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById(dsr).selectedIndex == 0) {
                alert("SELECT DSR!");
                document.getElementById("<%=dd_DSR.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById(vtype).checked == "") {
                alert("SELECT VEHICLE TYPE!");
                return false;
            }
            else if (document.getElementById(mainc).selectedIndex == 0) {
                alert("SELECT MAINTENANCE MAIN CATEGORY!");
                return false;
            }
            else if (document.getElementById(smc).selectedIndex == 0) {
                alert("SELECT MAINTENANCE SUB CATEGORY!");
                return false;
            }
            else if (document.getElementById("<%=billno.ClientID%>").value == "") {
                alert("ENTER BILL NO.!");
                document.getElementById("<%=billno.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=billamount.ClientID%>").value == "") {
                alert("ENTER BILL AMOUNT!");
                document.getElementById("<%=billamount.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=billdate.ClientID%>").value == "") {
                alert("ENTER BILL DATE!");
                document.getElementById("<%=billdate.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=meterreading.ClientID%>").value == "") {
                alert("ENTER METER READING!");
                document.getElementById("<%=meterreading.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txt_vendor.ClientID%>").value == "") {
                alert("ENTER VENDOR!");
                document.getElementById("<%=txt_vendor.ClientID%>").focus();
                return false;
            }
    return true;
}
    </script>
    <div class="heading" id="heading" runat="server">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Label ID="Errorid" runat="server" ForeColor="Red"></asp:Label>
                <div id="sub_menu">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
    <fieldset>
        <legend>EXPENSE SHEET</legend>
        <div style="font-size: xx-small; color: Red;">
            REQUIRED (*)
        </div>
        <asp:UpdatePanel ID="mypanel" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:HiddenField ID="record_id" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Expense Date *</b>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <asp:TextBox ID="expensedate" runat="server" CssClass="datebox" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender3" OnClientDateSelectionChanged="checkDate"
                                TargetControlID="expensedate" runat="server" Format="dd/MM/yyyy" PopupButtonID="Image1">
                            </Ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>

                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Zone*</b>
                        </td> 
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <asp:DropDownList ID="dd_zone" runat="server" CssClass="ddlbox"
                                AutoPostBack="true" OnSelectedIndexChanged="dd_zone_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                       
                        <td class="cell-two-mini"  style="font-size: small;" width="30px" colspan="1">
                           <%-- <b>Product *</b>--%>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <asp:DropDownList Visible="false" ID="dd_ItemLevel1" runat="server" CssClass="ddlbox"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>

                    </tr>
                    <tr id="dsr" runat="server">
                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <%--<b>Driver </b>--%>  
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <asp:DropDownList ID="dd_DSR" Visible="false" CssClass="ddlbox" runat="server"
                                AutoPostBack="true"> 
                            </asp:DropDownList> 
                        </td>
                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Vehicle *</b>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <asp:RadioButtonList Style="font-size: small;" runat="server" ID="V_type" AutoPostBack="true"
                                RepeatDirection="Horizontal" OnSelectedIndexChanged="V_type_SelectedIndexChanged"
                                Width="400px">
                                <asp:ListItem Selected="True" Value="01"><b>Company Maintained</b></asp:ListItem>
                                <asp:ListItem Value="02"><b>Cont. Van</b></asp:ListItem>
                                <asp:ListItem Value="03"><b>Outsourced Van</b></asp:ListItem>
                            </asp:RadioButtonList>
                            <br />
                            <asp:DropDownList runat="server" Visible="true" CssClass="ddlbox" ID="dd_van">
                            </asp:DropDownList>
                            <asp:TextBox ID="OutsVan" PlaceHolder="Enter Vehicle" CssClass="txtbox" Visible="false"
                                runat="server" MaxLength="3"></asp:TextBox>
                        </td>
                    </tr> 
                    <tr>
                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1"><b>Staff Workshop</b></td>
                        <td><asp:DropDownList runat="server" ID="dd_staff"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Expense Category *</b>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <asp:DropDownList ID="CategoryDropDownList" runat="server" CssClass="ddlbox"
                                AutoPostBack="true" OnSelectedIndexChanged="SubCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Vehicle Model *</b>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <asp:DropDownList ID="dd_vtype" CssClass="ddlbox" runat="server"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1" style="font-size: small;" width="30px" colspan="1">
                            <b>Expense Sub Category *</b>
                        </td>
                        <td class="style1" width="80px" colspan="1">
                            <asp:DropDownList ID="SubCategory" runat="server" CssClass="ddlbox"
                                AutoPostBack="true" OnSelectedIndexChanged="SubCategory_SelectedIndexChanged1"
                                OnTextChanged="SubCategory_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                        <td class="style1" style="font-size: small;" width="30px" colspan="1">
                            <b>Bill Date *</b>
                        </td>
                        <td class="style1" width="80px" colspan="1">
                            <asp:TextBox ID="billdate" runat="server" CssClass="datebox" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender1" OnClientDateSelectionChanged="checkDate"
                                TargetControlID="billdate" runat="server" Format="dd/MM/yyyy" PopupButtonID="Image1">
                            </Ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Vendor *</b>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <asp:TextBox ID="txt_vendor" CssClass="txtbox" MaxLength="20" runat="server" />
                        </td>
                        
                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Bill Amount *</b>
                        </td>
                        <td>
                            <asp:TextBox ID="billamount" runat="server"  CssClass="txtbox"
                                MaxLength="10"></asp:TextBox><br />
                           <%-- placeholder="Bill Amount"--%>
                            <asp:CompareValidator ID="CompareValidator1" ControlToValidate="billamount" runat="server"
                                ErrorMessage="Integers only please." Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Detail</b>
                        </td>
                        <td class="cell-two-mini" width="80px" colspan="1">
                            <asp:TextBox ID="detail" placeholder="Enter Details" runat="server" MaxLength="200"
                                Height="50px" CssClass="txtbox" TextMode="MultiLine" />
                        </td>
                       
                        <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Work Order *</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_workorder_amt" runat="server"  CssClass="txtbox"
                                MaxLength="10"></asp:TextBox><br />
                            <%--placeholder="Work Order Amount"--%>
                            <asp:CompareValidator ID="CompareValidator4" ControlToValidate="txt_workorder_amt" runat="server"
                                ErrorMessage="Integers only please." Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                         <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Meter Reading *</b>
                        </td>
                        <td>
                            <asp:TextBox ID="meterreading" runat="server" CssClass="txtbox" MaxLength="7"></asp:TextBox><br />
                            <asp:CompareValidator ID="CompareValidator3" ControlToValidate="meterreading" runat="server"
                                ErrorMessage="Integers only please." Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                        </td>
                        <td >
                            <%--class="cell-two-mini" style="font-size: small;" colspan="1"--%>
                           <%-- <b>Qty *</b>--%>
                        </td>
                        <%--class="cell-two-mini" width="80px" colspan="1"--%>
                        <td >
                            <asp:TextBox ID="qtytxtbox" runat="server" CssClass="txtbox" Visible="false" MaxLength="10"></asp:TextBox>
                            <span ></span>​<br />
                            <%--class="ltrKgLabel"--%>
                            <asp:CompareValidator ID="CompareValidator2" ControlToValidate="qtytxtbox" runat="server"
                                ErrorMessage="Integers only please." ForeColor="Red" Font-Size="XX-Small" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                            <Ajax1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" MinimumPrefixLength="1"
                                ServiceMethod="GetCountryInfo" TargetControlID="qtytxtbox">
                            </Ajax1:AutoCompleteExtender>
                        </td>
                       
                    </tr>
                    <tr>
                         <td class="cell-two-mini" style="font-size: small;" width="30px" colspan="1">
                            <b>Remarks</b>
                        </td>
                        <td>
                            <asp:TextBox ID="remarks" placeholder="Enter Remarks" runat="server" MaxLength="200"
                                Height="50px" CssClass="txtbox" TextMode="MultiLine" />
                        </td>
                         <td >
                            <%--class="cell-two-mini" style="font-size: small;" width="40px" colspan="1"--%>
                          <%--  <b>Bill No. *</b>--%>
                        </td>
                       <%-- class="cell-two-mini" width="80px" colspan="1"--%>
                        <td >
                            <asp:TextBox ID="billno" runat="server" Visible="false" CssClass="txtbox" MaxLength="15"></asp:TextBox>
                        </td>
                        
                       
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btn_Submit" runat="server" Text="Add Expense" CssClass="button" OnClientClick="javascript:return ValidateExpense();"
                                OnClick="btn_Submit_Click" />
                            <asp:Button ID="btn_update" runat="server" Visible="false" OnClientClick="javascript:return ValidateExpense();"
                                CssClass="button" OnClick="btn_update_Click" Text="Update Expense" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <fieldset>
        <div id="dv_edit" visible="true" runat="server"> 
            <asp:GridView ID="gv_edit" runat="server" AutoGenerateColumns="False" CellPadding="5"
                ForeColor="#333333 " GridLines="Both" Width="100%" OnRowCommand="gv_data_RowCommand"
                HeaderStyle-HorizontalAlign="Left" CssClass="font-style" ShowHeaderWhenEmpty="true">
                <AlternatingRowStyle BackColor="#f5f5f5" />
                <Columns>
                    <asp:BoundField DataField="EID" HeaderText="EXPENSE NO" />
                    <asp:BoundField DataField="TODAY_DATE" HeaderText="TODAY DATE" />
                    <asp:BoundField DataField="MMC" HeaderText="MAIN CATEGORY" />
                    <asp:BoundField DataField="MSC" HeaderText="SUB CATEGORY" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lb_edit" runat="server" Text="EDIT" Visible="true"
                                CommandArgument='<%# Eval("EID") %>' CommandName="EDIT"></asp:LinkButton>
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
        <asp:HiddenField ID="hd_edit" runat="server" />
        <asp:HiddenField ID="hd_subcat" runat="server" />
    </fieldset>
</asp:Content>
