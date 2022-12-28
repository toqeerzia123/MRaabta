<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PC_IOU.aspx.cs" Inherits="MRaabta.Files.PC_IOU" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        function isNumberKey(evt) {

            var count = 1;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode > 47 && charCode < 58) || charCode == 110) {
                count++;
            }
            else {
                if (count == 1) {
                    return false;
                }
            }

            return true;
        }

        function DecimalNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                return false;
            else {
                var len = document.getElementById('<%=txt_Amount.ClientID%>').value.length;
                var index = document.getElementById('<%=txt_Amount.ClientID%>').value.indexOf('.');
                if (len == 0 && (index < 0 || index > 0) && charCode == 46) {
                    return false;
                }
                if (index < 0 && len > 9) {
                    if ((charCode > 48 || charCode < 57) && charCode != 46) {
                        return false;
                    }
                    else if (index < 0 && charCode == 46) {
                        return true;
                    }
                }
                if (index > 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    var CharAfterdot = (len + 1) - index;
                    if (CharAfterdot > 3) {
                        return false;
                    }
                }

            }
            return true;
        }

        function NumberWithHyphen(evt) {

            var count = 1;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 45 && charCode > 31
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 9)) {
                return false;
            }
            else {
                if (charCode == 110) {
                    count++;
                }
                if (count > 1) {
                    return false;
                }
            }

            return true;
        }

        function onlyAlphabets(e) {
            var count = 1;
            var charCode = (e.which) ? e.which : event.keyCode

            if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode > 47 && charCode < 58) || (charCode == 32)) {
                count++;
            }
            else {
                if (count == 1) {
                    return false;
                }
            }
            return true;
        }

        function onlyAlphaNumeric(e) {
            var count = 1;
            var charCode = (e.which) ? e.which : event.keyCode

            if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode > 47 && charCode < 58)) {
                count++;
            }
            else {
                if (count == 1) {
                    return false;
                }
            }
            return true;
        }

        function ClearAll() {
            var Employee_Code = document.getElementById('<%=txt_Employee_Code.ClientID %>');
            var Employee_Name = document.getElementById('<%=txt_Employee_Name.ClientID %>');
            var IOUDate = document.getElementById('<%=txt_IOUDate.ClientID %>');
            var Amount = document.getElementById('<%=txt_Amount.ClientID %>');
            var Status = document.getElementById('<%=dd_Status.ClientID %>');
            var Zone = document.getElementById('<%=dd_Zone.ClientID %>');
            var Branch = document.getElementById('<%=dd_Branch.ClientID %>');
            var Reason = document.getElementById('<%=txt_Reason.ClientID %>');
            var Remarks = document.getElementById('<%=txt_Remarks.ClientID %>');

            var Button_Calender = document.getElementById('<%=Popup_Button2.ClientID%>');

            var Reset_Button = document.getElementById('<%=btn_reset.ClientID %>');
            var Save_Button = document.getElementById('<%=btn_save.ClientID %>');

            var Label_IOUNUMBER = document.getElementById('<%=lbl_IOUNO_.ClientID%>');
            var Text_IOUNUMBER = document.getElementById('<%=txt_IOUNO_.ClientID%>');

            var IOUNO = document.getElementById('<%=txt_IOUNO.ClientID%>');
            var HF_IOUNO = document.getElementById('<%=hf_IOUNumber.ClientID%>');

            Employee_Code.value = "";
            Employee_Name.value = "";
            IOUDate.value = "";
            Amount.value = "";
            Zone.value = "0";
            Branch.value = "0";
            Reason.value = "";
            Remarks.value = "";
            Status.value = "1";
            Status.disabled = false;

            if (Status.value == "1") {
                Status.disabled = false;
                Button_Calender.setAttribute('style', 'visibility : visible');
                Text_IOUNUMBER.disabled = true;
                IOUNO.value = HF_IOUNO.value;
                //Label_IOUNUMBER.setAttribute('style', 'visibility : hidden');
                //Text_IOUNUMBER.setAttribute('style', 'visibility : hidden');
            }

            //return true;

            __doPostBack('btn_reset', '');

        }

        function CheckFlag() {
            var Employee_Code = document.getElementById('<%=txt_Employee_Code.ClientID %>');
            var Employee_Name = document.getElementById('<%=txt_Employee_Name.ClientID %>');
            var IOUDate = document.getElementById('<%=txt_IOUDate.ClientID %>');
            var Amount = document.getElementById('<%=txt_Amount.ClientID %>');
            var Status = document.getElementById('<%=dd_Status.ClientID %>');
            var Zone = document.getElementById('<%=dd_Zone.ClientID %>');
            var Branch = document.getElementById('<%=dd_Branch.ClientID %>');
            var Reason = document.getElementById('<%=txt_Reason.ClientID %>');
            var Remarks = document.getElementById('<%=txt_Remarks.ClientID %>');
            var Button_Calender = document.getElementById('<%=Popup_Button2.ClientID%>');

            var Label_IOUNUMBER = document.getElementById('<%=lbl_IOUNO_.ClientID%>');
            var Text_IOUNUMBER = document.getElementById('<%=txt_IOUNO_.ClientID%>');
            var IOUNO = document.getElementById('<%=txt_IOUNO.ClientID%>');
            var HF_IOUNO = document.getElementById('<%=hf_IOUNumber.ClientID%>');

            if (Status.value == "1") {
                IOUNO.disabled = true;
                IOUNO.value = HF_IOUNO.value;

                Button_Calender.setAttribute('style', 'visibility : visible');

                Employee_Code.disabled = false;
                Employee_Name.disabled = false;
                IOUDate.disabled = false;
                Employee_Code.value = "";
                IOUDate.value = "";
                Amount.value = "";
                Zone.value = "0";
                Branch.value = "0";
                Reason.value = "";
                Remarks.value = "";

                //Label_IOUNUMBER.setAttribute('style', 'visibility : hidden');
                //Text_IOUNUMBER.setAttribute('style', 'visibility : hidden');
            }
            else if (Status.value == "2") {
                IOUNO.disabled = false;
                IOUNO.value = "";

                Button_Calender.setAttribute('style', 'visibility : hidden');

                Employee_Code.disabled = false;
                Employee_Name.disabled = false;
                IOUDate.disabled = false;
                Employee_Code.value = "";
                IOUDate.value = "";
                Amount.value = "";
                Zone.value = "0";
                Branch.value = "0";
                Reason.value = "";
                Remarks.value = "";

                //Label_IOUNUMBER.setAttribute('style', 'visibility : visible; width: 2% !important; text-align: right !important; padding-right: 88px!important;');
                //Label_IOUNUMBER.setAttribute('class', 'field');
                //Text_IOUNUMBER.setAttribute('style', 'visibility : visible; width: 12% !important;');
                //Text_IOUNUMBER.setAttribute('class', 'input-field');
            }

            //return true;
            __doPostBack('dd_Status', '');
        }

        //window.onload = function () {
        //    CheckFlag();
        //};

    </script>
    <br />
    <style>
        body {
            font-family: Calibri;
        }

        ul.tab {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            border: 1px solid #ccc;
            background-color: #f1f1f1;
        }

            /* Float the list items side by side */
            ul.tab li {
                float: left;
            }

                /* Style the links inside the list items */
                ul.tab li a {
                    display: inline-block;
                    color: black;
                    text-align: center;
                    padding: 14px 16px;
                    text-decoration: none;
                    transition: 0.3s;
                    font-size: 17px;
                }

                    /* Change background color of links on hover */
                    ul.tab li a:hover {
                        background-color: #ddd;
                    }

                    /* Create an active/current tablink class */
                    ul.tab li a:focus, .active {
                        background-color: #ccc;
                    }

        /* Style the tab content */
        .tabcontent {
            display: none;
            padding: 6px 12px;
            border: 1px solid #ccc;
            border-top: none;
        }

        .Message {
            float: left;
            width: 80%;
        }
    </style>
    <div id="BasicInfo">
        <fieldset>
            <legend>Petty Cash IOU</legend>
            <asp:Panel ID="panel1" runat="server">
                <table style="font-family: Calibri; border: 1px solid black; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
                    class="input-form">
                    <tr style="float: none !important;">
                        <td colspan="2" style="width: 10% !important; text-align: left !important; padding-right: 5px!important;">
                            <asp:Label ID="lbl_Message" runat="server" Font-Size="Medium" CssClass="Message"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 4% !important; text-align: right !important; padding-right: 45px!important;">Flag
                        </td>
                        <td class="input-field" style="width: 13.5% !important;">
                            <asp:DropDownList ID="dd_Status" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_Status_SelectedIndexChanged" AppendDataBoundItems="true" Width="100%">
                                <%--onchange="CheckFlag(); return true;"--%>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 6.7% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td id="lbl_IOUNO_" runat="server" class="field" style="width: 2% !important; text-align: right !important; padding-right: 57px!important;">IOU Number
                        </td>
                        <td id="txt_IOUNO_" runat="server" class="input-field" style="width: 12% !important;">
                            <asp:TextBox ID="txt_IOUNO" Enabled="false" AutoPostBack="true" OnTextChanged="txt_IOUNO_TextChanged" runat="server" autocomplete="off" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="space" style="width: 0.8% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 2% !important; text-align: right !important; padding-right: 58px!important;">Employee Code
                        </td>
                        <td class="input-field" style="width: 12% !important;">
                            <asp:TextBox ID="txt_Employee_Code" runat="server" autocomplete="off" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 7.8% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 2% !important; text-align: right !important; padding-right: 58px!important;">Employee Name
                        </td>
                        <td class="input-field" style="width: 12% !important;">
                            <asp:TextBox ID="txt_Employee_Name" runat="server" autocomplete="off" onkeypress="return onlyAlphabets(event);"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 4.4% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 47px!important;">IOU Date
                        </td>
                        <td class="input-field" style="width: 12% !important;">
                            <asp:TextBox ID="txt_IOUDate" runat="server" autocomplete="off" onkeypress="return NumberWithHyphen(event);"
                                MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender_IOUDate" runat="server" TargetControlID="txt_IOUDate"
                                Format="yyyy-MM-dd" PopupButtonID="Popup_Button2"></Ajax1:CalendarExtender>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
                            <asp:ImageButton ID="Popup_Button2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                Width="20px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 4.5% !important; text-align: right !important; padding-right: 40px!important;">Zone
                        </td>
                        <td class="input-field" style="width: 13.5% !important;">
                            <asp:DropDownList ID="dd_Zone" runat="server" AppendDataBoundItems="true"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="dd_Zone_SelectedIndexChanged"
                                Width="100%">
                                <asp:ListItem Value="0" Selected="True">Select Zone</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 5.5% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 5.6% !important; text-align: right !important; padding-right: 30px!important;">Branch
                        </td>
                        <td class="input-field" style="width: 13.5% !important;">
                            <asp:DropDownList ID="dd_Branch" runat="server" AppendDataBoundItems="true" Width="100%">
                                <asp:ListItem Value="0" Selected="True">Select Branch</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 5.5% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 7% !important; text-align: right !important; padding-right: 53px!important;">Amount
                        </td>
                        <td class="input-field" style="width: 12% !important;">
                            <asp:TextBox ID="txt_Amount" runat="server" autocomplete="off" MaxLength="11" onkeypress="return DecimalNumber(event);"></asp:TextBox>
                            <br />
                            <asp:Label ID="lbl_RemainAmount" runat="server" Font-Size="Small"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="space" style="width: -1% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 5.8% !important; text-align: right !important; padding-right: 26px!important;">Reason
                        </td>
                        <td class="input-field" style="width: 3.2% !important;">
                            <textarea id="txt_Reason" runat="server" autocomplete="off" cols="24" rows="3" style="width: 400%; resize: none; font-family: Tahoma;"></textarea>
                        </td>
                        <td class="space" style="width: 16.3% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 6% !important; text-align: right !important; padding-right: 20px!important;">Remarks
                        </td>
                        <td class="input-field" style="width: 3.2% !important;">
                            <textarea id="txt_Remarks" runat="server" autocomplete="off" cols="24" rows="3" style="width: 400%; resize: none; font-family: Tahoma;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 30% !important; text-align: right !important; padding-right: 5px!important;"></td>
                        <td class="input-field" style="width: 40% !important;" colspan="2">
                            <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" Width="80px" /><%--OnClientClick="ClearAll(); return true;"--%>
                            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click" Width="80px" />
                        </td>
                    </tr>
                    <tr>
                        <asp:HiddenField ID="hf_Create_Date" runat="server" />
                        <asp:HiddenField ID="hf_ID" runat="server" />
                        <asp:HiddenField ID="hf_EditClickCheck" runat="server" />
                        <asp:HiddenField ID="hf_IOUNumber" runat="server" />
                        <asp:HiddenField ID="hf_OutstandingAmount" runat="server" />
                        <asp:HiddenField ID="hf_SettledAmount" runat="server" />
                        <asp:HiddenField ID="hf_FlagID" runat="server" />
                    </tr>
                </table>
            </asp:Panel>
            <fieldset id="PC_IOU">
                <legend>Petty Cash IOU Details</legend>
                <asp:Panel ID="New_Panel" runat="server" Height="250px" ScrollBars="Both">
                    <asp:Repeater runat="server" ID="rp_PC_IOU_DBT" OnItemCommand="rp_PC_IOU_DBT_ItemCommand"
                        OnItemDataBound="rp_PC_IOU_DBT_ItemDataBound">
                        <HeaderTemplate>
                            <table class="mGrid" id="PC_IOU_Table">
                                <tr>
                                    <th></th>
                                    <th>S. No.
                                    </th>
                                    <th>IOU Number
                                    </th>
                                    <th style="display: none">ID
                                    </th>
                                    <th>Employee<br />
                                        Code
                                    </th>
                                    <th>Employee<br />
                                        Name
                                    </th>
                                    <th>Date IOU
                                    </th>
                                    <th>Reason
                                    </th>
                                    <th>Amount
                                    </th>
                                    <th>Flag
                                    </th>
                                    <th>Remarks
                                    </th>
                                    <th>Zone
                                    </th>
                                    <th>Branch
                                    </th>
                                    <th>Create On
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: center">
                                    <asp:LinkButton ID="Edit_button" runat="server" CommandName="Edit" Text="Edit">
                                    </asp:LinkButton>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lblRowNumber" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Label ID="lbl_IOUNumber" runat="server" Text='<%#Eval("IOUNumber") %>'></asp:Label>
                                </td>
                                <td style="text-align: center; display: none;">
                                    <asp:Label ID="ID" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lbl_Employee_Code" runat="server" Text='<%# Eval("EmployeeCode") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lbl_Employee_Name" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lbl_Date_IOU" runat="server" Text='<%# Eval("DateIOU") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lbl_Reason" runat="server" Text='<%# Eval("Reason") %>'></asp:Label>
                                    <asp:HiddenField ID="hf_Long_Reason" runat="server" Value='<%# Eval("Long_Reason") %>' />
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lbl_Amount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lbl_Flag" runat="server" Text='<%# Eval("Flag") %>'></asp:Label>
                                    <asp:HiddenField ID="hf_Flag_ID" runat="server" Value='<%# Eval("Flag_ID") %>' />
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lbl_Remarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                    <asp:HiddenField ID="hf_Long_Remarks" runat="server" Value='<%# Eval("Long_Remarks") %>' />
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lbl_Zone" runat="server" Text='<%# Eval("Zone") %>'></asp:Label>
                                    <asp:HiddenField ID="hf_Zone_ID" runat="server" Value='<%# Eval("Zone_ID") %>' />
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lbl_Branch" runat="server" Text='<%# Eval("Branch") %>'></asp:Label>
                                    <asp:HiddenField ID="hf_Branch_ID" runat="server" Value='<%# Eval("Branch_ID") %>' />
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lbl_CreateOn" runat="server" Text='<%# Eval("CreateOn") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </fieldset>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
