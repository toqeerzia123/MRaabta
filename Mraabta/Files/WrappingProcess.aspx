<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="WrappingProcess.aspx.cs" Inherits="MRaabta.Files.WrappingProcess" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        window.onload = function () {
            document.getElementById('<%=txt_ConsignmentNo.ClientID %>').focus();
        };

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

        function DecimalNumber(evt, id) {
            var element = id;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                return false;
            else {
                var len = document.getElementById(element).value.length;
                var index = document.getElementById(element).value.indexOf('.');
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

        function onlyAlphabets(e, id) {
            var element = id;
            var count = 1;
            var MaxLengthAllowed = 90;
            var charCode = (e.which) ? e.which : event.keyCode

            if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)
                || (charCode > 47 && charCode < 58) || (charCode == 32) || (charCode == 35) || (charCode == 38)
                || (charCode == 40) || (charCode == 41) || (charCode == 44) || (charCode == 45) || (charCode == 47)
                || (charCode == 47) || (charCode == 46) || (charCode == 91) || (charCode == 93) || (charCode == 123)
                || (charCode == 125) || (charCode == 95)) {
                var len = document.getElementById(element).value.length;
                if (len > MaxLengthAllowed) {
                    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)
                        || (charCode > 47 && charCode < 58) || (charCode == 32) || (charCode == 35) || (charCode == 38)
                        || (charCode == 40) || (charCode == 41) || (charCode == 44) || (charCode == 45) || (charCode == 47)
                        || (charCode == 47) || (charCode == 46) || (charCode == 91) || (charCode == 93) || (charCode == 123)
                        || (charCode == 125) || (charCode == 95)) {
                        return false;
                    }
                }
                else {
                    count++;
                }
                //count++;
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
            var CN = document.getElementById('<%=txt_ConsignmentNo.ClientID %>');
            var Consignee = document.getElementById('<%=txt_Consignee.ClientID %>');
            var Consigner = document.getElementById('<%=txt_Consigner.ClientID %>');
            var ConsigneeAddress = document.getElementById('<%=txt_ConsigneeAddress.ClientID %>');
            var ConsignerAddress = document.getElementById('<%=txt_ConsignerAddress.ClientID %>');
            var BookingDate = document.getElementById('<%=txt_BookingDate.ClientID %>');
            var Origin = document.getElementById('<%=dd_Origin.ClientID %>');
            var Destination = document.getElementById('<%=dd_Destination.ClientID %>');
            var Pieces = document.getElementById('<%=txt_Pieces.ClientID %>');
            var Weight = document.getElementById('<%=txt_Weight.ClientID %>');
            var ServiceType = document.getElementById('<%=dd_ServiceType.ClientID %>');
            var PackageContent = document.getElementById('<%=txt_PackageContent.ClientID %>');
            var AccountNo = document.getElementById('<%=txt_AccountNo.ClientID %>');
            var WrappingCharges = document.getElementById('<%=txt_WrappingAmount.ClientID %>');
            var Comment = document.getElementById('<%=txt_Comment.ClientID %>');
            var ConsigneeMobile = document.getElementById('<%=txt_ConsigneeMobile.ClientID %>');
            var ConsigneePhoneNo = document.getElementById('<%=txt_ConsigneePhoneNo.ClientID %>');
            var Message_Label = document.getElementById('<%=lbl_Message.ClientID%>');

            var Reset_Button = document.getElementById('<%=btn_reset.ClientID %>');
            var Save_Button = document.getElementById('<%=btn_save.ClientID %>');
            var Print_Button = document.getElementById('<%=btn_print.ClientID %>');

            CN.value = "";
            Consignee.value = "";
            Consigner.value = "";
            ConsigneeAdress.value = "";
            ConsignerAdress.value = "";
            BookingDate.value = "";
            Origin.options[Origin.selectedIndex].value = "0";
            Destination.options[Destination.selectedIndex].value = "0";
            Pieces.value = "";
            Weight.value = "";
            ServiceType.options[ServiceType.selectedIndex].value = "AVIATION SALE";
            PackageContent.value = "";
            AccountNo.value = "";
            WrappingCharges.value = "";
            Comment.value = "";
            ConsigneeMobile.value = "";
            ConsigneePhoneNo.value = "";
            Message_Label.textContent = "";

            alert('Consignment Number Required!');
            CN.disabled = false;
            CN.focus();

            Reset_Button.setAttribute("style", "visibility:hidden");
            Save_Button.setAttribute("style", "visibility:hidden");
            Print_Button.setAttribute("style", "visibility:hidden");

        }

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
    </style>
    <div id="BasicInfo">
        <fieldset>
            <legend></legend>
            <asp:Panel ID="panel1" runat="server">
                <table style="font-family: Calibri; border: 1px solid black; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
                    class="input-form">
                    <center style="font-size: medium;">
                        <b>Wrapping Process</b>
                    </center>
                    <div style="padding: 0px 10px 0px 0px; font-family: Arial; font-size: small; color: red; white-space: pre; height: 15px;">
                        <label id="lbl_Message" runat="server"></label>
                    </div>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 2.4% !important; text-align: right !important; padding-right: 64px!important;">CN
                        </td>
                        <td class="input-field" style="width: 18% !important;">
                            <asp:TextBox ID="txt_ConsignmentNo" runat="server" AutoPostBack="true" autocomplete="off" OnTextChanged="txt_ConsignmentNo_TextChanged" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 4% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 15px!important;">Booking Date
                        </td>
                        <td class="input-field" style="width: 18% !important;">
                            <asp:TextBox ID="txt_BookingDate" runat="server" autocomplete="off" onkeypress="return NumberWithHyphen(event);"
                                MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender_BookingDate" runat="server" TargetControlID="txt_BookingDate"
                                Format="yyyy-MM-dd" PopupButtonID="Popup_Button2">
                            </Ajax1:CalendarExtender>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
                            <asp:ImageButton ID="Popup_Button2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                Width="20px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 7% !important; text-align: right !important; padding-right: 15px!important;">Consignee
                        </td>
                        <td class="input-field" style="width: 4.7% !important;">
                            <textarea id="txt_Consignee" runat="server" autocomplete="off" cols="24" rows="3" style="width: 400%; resize: none; font-family: Arial;" onkeypress="return onlyAlphabets(event, this.id);"></textarea>
                        </td>
                        <%--                        <td class="input-field" style="width: 18.2% !important;">
                            <asp:TextBox ID="txt_Consignee" runat="server" autocomplete="off" onkeypress="return onlyAlphabets(event, this.id);"></asp:TextBox>
                        </td>--%>
                        <td class="space" style="width: 18.5% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 5% !important; text-align: right !important; padding-right: 53px!important;">Consigner
                        </td>
                        <td class="input-field" style="width: 4.7% !important;">
                            <textarea id="txt_Consigner" runat="server" autocomplete="off" cols="24" rows="3" style="width: 400%; resize: none; font-family: Arial;" onkeypress="return onlyAlphabets(event, this.id);"></textarea>
                        </td>
                        <%--                        <td class="input-field" style="width: 18% !important;">
                            <asp:TextBox ID="txt_Consigner" runat="server" autocomplete="off" onkeypress="return onlyAlphabets(event, this.id);"></asp:TextBox>
                        </td>--%>
                        <td class="space" style="width: 17.3% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 10px!important;">Account No
                        </td>
                        <td class="input-field" style="width: 18% !important;">
                            <asp:TextBox ID="txt_AccountNo" runat="server" autocomplete="off" onkeypress="return onlyAlphaNumeric(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 6.5% !important; text-align: right !important; padding-right: 20px!important;">Consignee Address
                        </td>
                        <td class="input-field" style="width: 4.7% !important;">
                            <textarea id="txt_ConsigneeAddress" runat="server" autocomplete="off" cols="24" rows="3" style="width: 400%; resize: none; font-family: Arial;" onkeypress="return onlyAlphabets(event, this.id);"></textarea>
                        </td>
                        <td class="space" style="width: 18.7% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 6% !important; text-align: right !important; padding-right: 42px!important;">Consigner Address
                        </td>
                        <td class="input-field" style="width: 4.7% !important;">
                            <textarea id="txt_ConsignerAddress" runat="server" autocomplete="off" cols="24" rows="3" style="width: 400%; resize: none; font-family: Arial;" onkeypress="return onlyAlphabets(event, this.id);"></textarea>
                        </td>
                        <td class="space" style="width: 19% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 6% !important; text-align: right !important; padding-right: 35px!important;">Package Content
                        </td>
                        <td class="input-field" style="width: 4.6% !important;">
                            <textarea id="txt_PackageContent" runat="server" autocomplete="off" cols="24" rows="3" style="width: 400%; resize: none; font-family: Arial;" onkeypress="return onlyAlphabets(event, this.id);"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 6% !important; text-align: right !important; padding-right: 25px!important;">Consignee Mobile
                        </td>
                        <td class="input-field" style="width: 18.2% !important;">
                            <asp:TextBox ID="txt_ConsigneeMobile" runat="server" autocomplete="off" onkeypress="return NumberWithHyphen(event);"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 5.2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 5% !important; text-align: right !important; padding-right: 38px!important;">Consignee Phone
                        </td>
                        <td class="input-field" style="width: 18.2% !important; padding-left: 18px !important">
                            <asp:TextBox ID="txt_ConsigneePhoneNo" runat="server" autocomplete="off" onkeypress="return NumberWithHyphen(event);"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 5% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 5% !important; text-align: right !important; padding-right: 27px!important;">Pieces
                        </td>
                        <td class="input-field" style="width: 18.2% !important; padding-left: 22px !important">
                            <asp:TextBox ID="txt_Pieces" runat="server" autocomplete="off" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 4.8% !important; text-align: right !important; padding-right: 38px!important;">Weight
                        </td>
                        <td class="input-field" style="width: 18.2% !important;">
                            <asp:TextBox ID="txt_Weight" runat="server" autocomplete="off" onkeypress="return DecimalNumber(event, this.id);"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 5% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 4% !important; text-align: right !important; padding-right: 65px!important;">Origin
                        </td>
                        <td class="input-field" style="width: 19.5% !important;">
                            <asp:DropDownList ID="dd_Origin" runat="server" AppendDataBoundItems="true" Width="100%">
                                <asp:ListItem Value="0">Select Origin</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 4.8% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 7% !important; text-align: right !important; padding-right: 18px!important;">Destination
                        </td>
                        <td class="input-field" style="width: 19.5% !important;">
                            <asp:DropDownList ID="dd_Destination" runat="server" AppendDataBoundItems="true" Width="100%">
                                <asp:ListItem Value="0">Select Destination</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 4.7% !important; text-align: right !important; padding-right: 38px!important;">Service
                        </td>
                        <td class="input-field" style="width: 19.5% !important;">
                            <asp:DropDownList ID="dd_ServiceType" runat="server" AppendDataBoundItems="true" Width="100%">
                                <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 3.5% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 6.5% !important; text-align: right !important; padding-right: 45px!important;">Wrapping Amount
                        </td>
                        <td class="input-field" style="width: 18.4% !important;">
                            <asp:TextBox ID="txt_WrappingAmount" runat="server" autocomplete="off" onkeypress="return DecimalNumber(event, this.id);"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 5.7% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 6% !important; text-align: right !important; padding-right: 28px!important;">Comment
                        </td>
                        <td class="input-field" style="width: 4.7% !important;">
                            <textarea id="txt_Comment" runat="server" autocomplete="off" cols="24" rows="3" style="width: 400%; resize: none; font-family: Arial;" onkeypress="return onlyAlphabets(event, this.id);"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 30% !important; text-align: right !important; padding-right: 5px!important;"></td>
                        <td class="input-field" style="width: 40% !important;" colspan="2">
                            <asp:Button ID="btn_reset" runat="server" Text="Reset" Visible="false" CssClass="button" OnClientClick="ClearAll();"
                                Width="80px" />
                            <asp:Button ID="btn_save" runat="server" Text="Save" Visible="false" CssClass="button" OnClick="btn_save_Click"
                                Width="80px" />
                            <asp:Button ID="btn_print" runat="server" Text="Print" Visible="false" CssClass="button" OnClick="btn_print_Click"
                                Width="80px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
