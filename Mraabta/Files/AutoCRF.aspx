<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="AutoCRF.aspx.cs" Inherits="MRaabta.Files.AutoCRF" %>

<%@ Register Namespace="AjaxControlToolkit" TagPrefix="Ajax" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        body {
            font-family: Calibri;
        }

        .center {
            text-align: center;
        }

        .mGrid th {
            background: #f27031 none repeat scroll 0 0;
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

        .tariffTable {
            border: 1px Solid Black;
            border-collapse: collapse;
        }

            .tariffTable td {
                border: 1px Solid Black;
            }

        #Services td {
            float: left;
            min-width: 140px;
        }

        .Services {
            margin: 20px 0 0;
        }
    </style>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script type="text/javascript" language="javascript">
        function PrintCustomer() {
            var btn = document.getElementById('btn_print');
            var br = document.getElementById('<%= dd_Branch.ClientID %>');
            var branch = br.options[br.selectedIndex].value;
            var accountName = document.getElementById('<%= txt_name.ClientID %>').value;
            var accountNo = document.getElementById('<%= txt_acc_no.ClientID %>').value;
            if (branch == "0") {
                alert('Select Branch');
                return;
            }
            if (accountName == "") {
                alert('Enter Account Name');
                return;
            }
            if (accountNo == "") {
                alert('Enter Account No');
                return;
            }
            else {
                window.open("CustomerPrint.aspx?Xcode=" + accountNo + "&BCode=" + branch, "_blank");
            }
        }
        function isNumberKeydouble(evt) {
            debugger;
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46) {
                    return true;
                }
                return false;
            }
            return true;
        }
        function CheckBranchSelection() {
            var branch = document.getElementById('<%= dd_Branch.ClientID %>');
            if (branch.options[branch.selectedIndex].value == "0") {
                alert('Select Branch');
                return false;
            }
            return true;
        }
    </script>
    <div id="div2" runat="server" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0; text-align: center; vertical-align: middle; height: 100%; position: absolute; width: 83%;">
        <img src="../images/Loading_Movie-02.gif" style="" />
    </div>
    <asp:Label ID="Errorid" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
    <ul class="tab">
        <li><a href="javascript:void(0)" id="basicInfoTab" class="tablinks" onclick="openCity(event, 'BasicInfo')">Basic Info</a></li>
        <li><a href="javascript:void(0)" class="tablinks" onclick="openCity(event, 'Configuration')">Configuration</a></li>
        <li><a href="javascript:void(0)" class="tablinks" onclick="openCity(event, 'billingNsales')">Billing & Sales</a></li>
        <li><a href="javascript:void(0)" class="tablinks" onclick="openCity(event, 'billingModifier')">Billing Modifiers</a></li>
        <li><a href="javascript:void(0)" id="nationwideTab" class="tablinks" onclick="openCity(event, 'NationWide')">NationWide/Centralized</a></li>
        <li><a href="javascript:void(0)" id="serviceTab" class="tablinks" onclick="openCity(event, 'Services')">Services</a></li>
        <li><a href="javascript:void(0)" class="tablinks" onclick="openCity(event, 'Tariff')">Tariff</a></li>
    </ul>
    <div id="BasicInfo" class="tabcontent">
        <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
            <tr>
                <td>
                    <div>
                        <table width="100%">
                            <tr>
                                <td>Zone
                                </td>
                                <td>
                                    <asp:DropDownList ID="dd_zone" runat="server" AppendDataBoundItems="true" Width="62%"
                                        OnSelectedIndexChanged="dd_zone_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0">Select Zone</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="background-color: #f6c8f7;">Branch
                                </td>
                                <td>
                                    <asp:DropDownList ID="dd_Branch" runat="server" AppendDataBoundItems="true" Width="90%"
                                        OnSelectedIndexChanged="dd_Branch_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0">Select Branch</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <span>Client No</span>
                                    <asp:HiddenField ID="hd_tempClientID" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_tempclient_no" runat="server" AutoPostBack="True" onkeypress="return RestrictChar();"
                                        onchange="if ( CheckBranchSelection() == false ) return;" OnTextChanged="txt_tempclient_no_TextChanged"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="background-color: #f6c8f7;">Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_name" runat="server" onkeypress="return RestrictChar();"></asp:TextBox>
                                </td>
                                <td>
                                    <span id="div11" runat="server" visible="false">Account No</span>
                                    <asp:HiddenField ID="hd_creditClientID" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_acc_no" runat="server" AutoPostBack="True" onkeypress="return RestrictChar();"
                                        Visible="false" onchange="if ( CheckBranchSelection() == false ) return;" OnTextChanged="txt_acc_no_TextChanged"></asp:TextBox>
                                </td>
                                <td style="background-color: #f6c8f7;">Contact Person
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_contactPerson" runat="server" onkeypress="return RestrictChar();"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chk_special_cust" runat="server" Text="Mega Customer" />
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #f6c8f7;">Fax No
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_FaxNo" MaxLength="11" onkeypress="return isNumberKeydouble();"></asp:TextBox>
                                    <script type="text/javascript" language="javascript">
                                        $('#ContentPlaceHolder1_txt_FaxNo').keyup(function () {
                                            var val = this.value.replace(/\D/g, '');
                                            var newVal = '';
                                            for (i = 0; i < 1; i++) {
                                                if (val.length > 3) {
                                                    newVal += val.substr(0, 3) + '-';
                                                    val = val.substr(3);
                                                }
                                            }
                                            newVal += val;
                                            this.value = newVal;
                                        });
                                    </script>
                                </td>
                                <td style="background-color: #f6c8f7;">Phone No
                                </td>
                                <td>
                                    <%--onkeypress="return isNumberKeydouble();"
                                    
                                    --%>
                                    <asp:TextBox ID="txt_PhoneNo" runat="server" AutoPostBack="True" MaxLength="11" placeholder="XXXX-XXXXXXX"
                                        OnTextChanged="txtPhoneNo_TextChanged" onkeypress="return isNumberKeydouble();"></asp:TextBox>
                                    <script type="text/javascript" language="javascript">
                                        $('#ContentPlaceHolder1_txt_PhoneNo').keyup(function () {
                                            debugger;
                                            var val = this.value.replace(/\D/g, '');
                                            var newVal = '';
                                            for (i = 0; i < 1; i++) {
                                                if (val.length > 4) {
                                                    newVal += val.substr(0, 4) + '-';
                                                    val = val.substr(4);
                                                }
                                            }
                                            newVal += val;
                                            this.value = newVal;
                                        });
                                    </script>
                                </td>
                                <td style="background-color: #f6c8f7;">
                                    <%--Category--%>
                                    Contact Person Designation
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_designation" runat="server"></asp:TextBox>
                                    <%--<asp:DropDownList runat="server" ID="dd_CustomerCategory" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">.::Select Category::.</asp:ListItem>
                                    </asp:DropDownList>--%>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="background-color: #f6c8f7;">Official Address
                                </td>
                                <td rowspan="2" colspan="1">
                                    <asp:TextBox runat="server" ID="txt_OfficialAddress" Height="60px" Width="200px"
                                        onkeypress="return RestrictChar();"></asp:TextBox>
                                </td>
                                <td style="background-color: #f6c8f7;">Reg Date
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_RegDate"></asp:TextBox>
                                    <Ajax:CalendarExtender ID="CalendarExtender" runat="server" PopupButtonID="imgCalendar"
                                        TargetControlID="txt_RegDate" Enabled="false" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                </td>
                                <td>Beneficiary Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_benName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="background-color: #f6c8f7;">Reg End Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_RegEndDate" runat="server"></asp:TextBox>
                                    <Ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgCalendar"
                                        TargetControlID="txt_RegEndDate" Enabled="True" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                </td>
                                <td>Beneficiary Bank
                                </td>
                                <td>
                                    <asp:DropDownList ID="dd_benBank" runat="server" AppendDataBoundItems="true" Width="175px">
                                        <asp:ListItem Value="0">Select Bank</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #f6c8f7;">Mailing Address
                                </td>
                                <td rowspan="2" colspan="1">
                                    <asp:TextBox runat="server" ID="txt_MailingAddress" Height="60px" Width="200px" onkeypress="return RestrictChar();"></asp:TextBox>
                                </td>
                                <td style="background-color: #f6c8f7;">Email
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_email"></asp:TextBox>
                                </td>
                                <td>Beneficiary Bank Account
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_benAccNo" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>COD
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chk_NotCod" TextAlign="Left" Text="" onclick="CODCheck()"
                                        OnCheckedChanged="chk_NotCod_CheckedChanged" />
                                    <asp:DropDownList ID="dd_codType" runat="server" Enabled="False" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">Select COD Type</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox Text="Centralized Client" runat="server" ID="chk_centralisedClient"
                                        TextAlign="Left" onclick="CheckCentralized(this)" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_nationWideGroup" runat="server" placeholder="Group ID" disabled="true"></asp:TextBox>
                                </td>
                                <td>
                                    <a href="SearchClientGroups.aspx" target="_blank">Search Client Group</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #f6c8f7;">Pickup Instruction
                                </td>
                                <td rowspan="2" colspan="1">
                                    <asp:TextBox runat="server" ID="txt_pickupInstruction" Height="60px" Width="200px"
                                        TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </td>
                                <td>Client Type
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbtn_customerType" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                        Enabled="false">
                                        <asp:ListItem Value="1">Cash</asp:ListItem>
                                        <asp:ListItem Value="0" Selected="True">Credit</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>Status Code
                                </td>
                                <td>
                                    <asp:DropDownList ID="dd_StatusCode" runat="server" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">.::Status Code::.</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hd_statusCode" runat="server" Value="" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        <br />
                        <div style="width: 100%;">
                            <div style="float: right; background-color: #f6c8f7">
                                Mandatory Fields
                            </div>
                        </div>
                        <br />
                        <asp:Button runat="server" ID="btn_save" Text="Save To DataBase" OnClick="btn_save_Click"
                            OnClientClick="return Show_Hide_By_Display();" CssClass="button" Visible="false" />
                        <asp:Button runat="server" ID="btn_save_temp" Text="Save" OnClick="btn_save_Click_Temp"
                            OnClientClick="return Show_Hide_By_Display();" CssClass="button" />
                        <asp:Button runat="server" ID="btn_approval" Text="Approved" OnClick="btn_save_Click_Approved"
                            OnClientClick="return Show_Hide_By_Display();" CssClass="button" />
                        <asp:Button runat="server" ID="btn_cancel" Text="Cancel" OnClick="btn_cancel_Click"
                            CssClass="button" />
                        <input type="button" onclick="PrintCustomer()" value="Print Customer" id="btn_print"
                            class="button" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="Configuration" class="tabcontent">
        <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
            <tr>
                <td>
                    <div>
                        <table width="100%">
                            <tr>
                                <td style="background-color: #f6c8f7;">Industry
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="dd_Industry" AppendDataBoundItems="true">
                                        <asp:ListItem Value="0">.::Select Industry::.</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>Status
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbtn_Status" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                        Enabled="false">
                                        <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                        <asp:ListItem Value="2">Special</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <%--<asp:DropDownList ID="dd_clientGroups" runat="server" AppendDataBoundItems="true"
                                        Visible="false" Width="150px">
                                        <asp:ListItem Value="0">.::Select Group::.</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_clientGroups" runat="server"></asp:TextBox>--%>
                                </td>
                                <td>Printing Status
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbtn_printStatus" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="0">Save</asp:ListItem>
                                        <asp:ListItem Value="1" Selected="True">Save & Print</asp:ListItem>
                                        <asp:ListItem Value="2">Save CN</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <%--Redeem Window--%>
                                </td>
                                <td>
                                    <%--<asp:TextBox ID="txt_redeemWindow" runat="server" onkeypress="return RestictNumbersOnly();"></asp:TextBox>--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />
                    <fieldset>
                        <legend>Avg. Monthly Turnover</legend>
                        <table width="100%">
                            <tr>
                                <td>Domestic
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_DomesticTurnOver" onkeypress="return RestictNumbersOnly();"
                                        Text="0"></asp:TextBox>
                                </td>
                                <td>Packets
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_DomesticPackets" onkeypress="return RestictNumbersOnly();"
                                        Text="0"></asp:TextBox>
                                </td>
                                <td>Amount
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_DomesticAmount" onkeypress="return RestictNumbersOnly();"
                                        Text="0"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>International
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_InternationalTurnOver" onkeypress="return RestictNumbersOnly();"
                                        Text="0"></asp:TextBox>
                                </td>
                                <td>Packets
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_InternationalPackets" onkeypress="return RestictNumbersOnly();"
                                        Text="0"></asp:TextBox>
                                </td>
                                <td>Amount
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_InternationalAmount" onkeypress="return RestictNumbersOnly();"
                                        Text="0"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
    <div id="billingNsales" class="tabcontent">
        <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>Prepare Bill
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="dd_prepareBill" Enabled="false">
                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Billing
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbtn_billingMode" runat="server">
                                    <asp:ListItem Value="1" Selected="True">Auto</asp:ListItem>
                                    <asp:ListItem Value="2">Manual</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>Discount On Document
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txt_DiscountOnDocument" Text="0" Enabled="false"></asp:TextBox>
                            </td>
                            <td>Discount On Domestic
                            </td>
                            <td>
                                <asp:TextBox ID="txt_DiscountOnDomestic" runat="server" Text="0" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Credit Limit (Period)
                            </td>
                            <td>
                                <asp:TextBox ID="txt_CreditLimit" onkeypress="return isNumberKeydouble();" runat="server"
                                    Text="0"></asp:TextBox>
                            </td>
                            <td>Discount On Sample
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txt_DiscountOnSample" Text="0" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Minimum Billing Amt
                            </td>
                            <td>
                                <script type="text/javascript">
                                    function EnableDisableTextBox() {
                                        debugger;
                                        var status = document.getElementById('<%=rbtn_codMonthlyBillingAmount.ClientID %>_0').checked;
                                        if (status == true) {
                                            document.getElementById('<%=txt_codMonthlyAmount.ClientID %>').disabled = false;
                                        } else {
                                            document.getElementById('<%=txt_codMonthlyAmount.ClientID %>').disabled = true;
                                        }
                                    }
                                </script>
                                <asp:RadioButtonList ID="rbtn_codMonthlyBillingAmount" runat="server" RepeatDirection="Horizontal"
                                    RepeatColumns="2" Width="100px" Style="float: left;" onclick="javascript:EnableDisableTextBox();">
                                    <asp:ListItem Value="1" Text="YES"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:TextBox runat="server" ID="txt_codMonthlyAmount" onkeypress="return isNumberKeydouble();"
                                    Text="0" Width="70px"></asp:TextBox>
                            </td>
                            <td>RNR Weight
                            </td>
                            <td>
                                <asp:TextBox ID="txt_RnRWeight" runat="server" onkeypress="return isNumberKeydouble();"
                                    Text="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">Bill tax Type
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbtn_BillTaxType" runat="server" Enabled="false">
                                    <asp:ListItem Value="0">Inclusive</asp:ListItem>
                                    <asp:ListItem Value="1" Selected="True">Exclusive</asp:ListItem>
                                    <asp:ListItem Value="2">Exemption</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="vertical-align: top;">
                                <div style="margin: 2px 0px 2px 0px;">
                                    Parent
                                </div>
                                <div style="margin: 2px 0px 2px 0px;">
                                    Piece Only
                                </div>
                                <div style="margin: 2px 0px 2px 0px;">
                                    Destination
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <asp:DropDownList ID="dd_parent" Style="margin: 2px 0px 2px 0px;" Width="175px" runat="server"
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">Select Parent</asp:ListItem>
                                </asp:DropDownList>
                                <br style="margin: 2px 0px 2px 0px;" />
                                <asp:DropDownList ID="dd_pieces" Style="margin: 2px 0px 2px 0px;" runat="server"
                                    Enabled="false">
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                                <br style="margin: 2px 0px 2px 0px;" />
                                <asp:DropDownList ID="dd_destination" runat="server" Style="margin: 2px 0px 2px 0px;"
                                    Enabled="false">
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <fieldset>
                        <legend>Sales Info</legend>
                        <table width="100%">
                            <tr>
                                <td>Staff Type
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddly_stafType" AppendDataBoundItems="true" OnSelectedIndexChanged="ddly_stafType_SelectedIndexChanged"
                                        Enabled="false" AutoPostBack="true">
                                        <asp:ListItem Value="0">.::Staff Type::.</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>Staff Member
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddl_staff_member" AppendDataBoundItems="true"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_staff_member_SelectedIndexChanged"
                                        Enabled="false">
                                        <asp:ListItem Value="0">.::Select Staff::.</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:GridView ID="gv_Staffs" runat="server" Width="100%" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField HeaderText="Staff Type" DataField="StaffType" />
                                            <asp:TemplateField HeaderText="Staff Member">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="dd_gStaffMembers" runat="server" AppendDataBoundItems="true"
                                                        Width="100%">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hd_StaffID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "StaffID") %>' />
                                                    <asp:HiddenField ID="hd_StaffTypeID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "StaffTypeID")  %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td style="background-color: #f6c8f7;">Recovery Express Center
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="dd_recoveryExpID" AppendDataBoundItems="true">
                                        <asp:ListItem Value="0">.::Select Recovery EC::.</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:DropDownList runat="server" ID="dd_SalesRoute" AppendDataBoundItems="true" Width="200px">
                                        <asp:ListItem Value="0">.::Select Route::.</asp:ListItem>
                                    </asp:DropDownList>--%>
                                </td>
                                <td style="background-color: #f6c8f7;">Origin Express Center
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="dd_originEC" AppendDataBoundItems="true">
                                        <asp:ListItem Value="0">.::Select Origin EC::.</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #f6c8f7;">Sales Tax No
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_SalesTaxNo"></asp:TextBox>
                                </td>
                                <td style="background-color: #f6c8f7;">NTN NO
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_ntnNo" AutoPostBack="True" CssClass="cep" placeholder="XXXXXXX-X"
                                        MaxLength="8" OnTextChanged="txt_ntnNo_TextChanged"></asp:TextBox>
                                </td>
                                <script type="text/javascript" language="javascript">
                                    $('#ContentPlaceHolder1_txt_ntnNo').keyup(function () {
                                        var val = this.value.replace(/\D/g, '');
                                        var newVal = '';
                                        for (i = 0; i < 2; i++) {
                                            if (val.length > 7) {
                                                newVal += val.substr(0, 7) + '-';
                                                val = val.substr(7);
                                            }
                                        }
                                        newVal += val;
                                        this.value = newVal;
                                    });
                                </script>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
    <div id="billingModifier" class="tabcontent">
        <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid;">
            <tr>
                <td>
                    <asp:UpdatePanel ID="up1" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <legend>Billing Modifiers</legend>
                                <table>
                                    <tr>
                                        <td style="background-color: #f6c8f7;">FAF Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dd_fafType" runat="server">
                                                <asp:ListItem Value="">Select FAF Type</asp:ListItem>
                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Name
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddl_tab4_name" AppendDataBoundItems="true" OnSelectedIndexChanged="ddl_tab4_name_SelectedIndexChanged"
                                                AutoPostBack="false">
                                                <asp:ListItem Value="0">.::Select Price Modifier::.</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>Calculation Base
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rbtn_PriceModifierBase" Enabled="false" runat="server" RepeatColumns="2"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1">Flat</asp:ListItem>
                                                <asp:ListItem Value="2">Percentage</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pmValue" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Description
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_BM_desc"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:Button runat="server" ID="btn_BM_Add" Text="ADD" OnClick="btn_BM_Add_Click"
                                                OnClientClick="Show_Hide_By_Display();" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView runat="server" ID="gridview_tab4" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                    OnRowCommand="gridview_tab4_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="del" runat="server" Text="Delete" CommandName="del" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Price Modifier Name" DataField="Name" />
                                        <%--<asp:BoundField HeaderText="Value" DataField="CalculationValue" />--%>
                                        <asp:TemplateField HeaderText="Value">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_gValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CalculationValue") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Caclulation Base" DataField="CalculationBase" />
                                        <asp:BoundField HeaderText="Description" DataField="Description" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <div id="NationWide" class="tabcontent">
        <table style="width: 75%; margin-left: 12.5%;">
            <tr>
                <td style="width: 15%; text-align: right;"></td>
                <td style="width: 20%; text-align: left;">
                    <asp:CheckBox ID="chk_nationWide" runat="server" Text="National" onchange="National();" />
                </td>
                <td style="width: 15%; text-align: right;"></td>
                <td style="width: 20%; text-align: left;"></td>
                <td style="width: 30%; text-align: left;"></td>
            </tr>
            <tr>
                <td style="width: 15%; text-align: right;">Account No
                </td>
                <td style="width: 20%; text-align: left;">
                    <asp:TextBox ID="txt_nationWideAccountNo" runat="server"></asp:TextBox>
                </td>
                <td style="width: 15%; text-align: right;">
                    <%--Group--%>
                </td>
                <td style="width: 20%; text-align: left;">
                    <asp:DropDownList ID="dd_nationWideGroup" runat="server" AppendDataBoundItems="true"
                        Visible="false">
                        <asp:ListItem Value="0">.::Select Group::.</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 30%; text-align: left;"></td>
            </tr>
            <tr>
                <td style="width: 15%; text-align: right;">Parent Branch
                </td>
                <td style="width: 20%; text-align: left;">
                    <asp:DropDownList ID="dd_parentBranch" runat="server" AppendDataBoundItems="true"
                        onchange="ParentBranchSelection(this)">
                        <asp:ListItem Value="0">.::Select Parent::.</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 15%; text-align: right;"></td>
                <td style="width: 20%; text-align: left;">
                    <%--<a href="SearchClientGroups.aspx" target="_blank">Search Client Group</a>--%>
                </td>
                <td style="width: 30%; text-align: left;"></td>
            </tr>
            <tr>
                <td style="width: 15%; text-align: right; vertical-align: top;">Branches
                </td>
                <td colspan="3" style="width: 55%; text-align: left;">
                    <div style="height: 400px; width: 100%; overflow: scroll;">
                        <asp:CheckBoxList ID="chkList_Branches" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                            Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td style="width: 30%; text-align: left; vertical-align: top;">
                    <asp:CheckBox ID="chk_checkAll" runat="server" Text="ALL" onchange="CheckBranches(this);" />
                </td>
            </tr>
            <tr>
                <td style="width: 15%; text-align: right;"></td>
                <td style="width: 55%; text-align: right; text-align: center;" colspan="3">
                    <asp:Button ID="btn_NationWideOk" runat="server" Text="OK" OnClientClick="NationalOK();"
                        Width="100px" CssClass="button" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_NationWideCancel" runat="server" Text="Cancel" OnClientClick="NationalCancel();"
                        Width="100px" CssClass="button" />
                </td>
                <td style="width: 30%; text-align: left;"></td>
            </tr>
        </table>
    </div>
    <div id="Services" class="tabcontent">
        <table style="width: 80%; margin-left: 10%;">
            <%--<tr>
        <td>
        <input type="checkbox" value="chk_AllServices" title="Check All" />
        </td>
        </tr>--%>
            <tr>
                <td>
                    <asp:CheckBoxList ID="chkl_services" runat="server" AutoPostBack="false" RepeatColumns="3">
                    </asp:CheckBoxList>
                    <asp:Button ID="tariff_service" CssClass="button Services" runat="server" Text="Add Service in Tariff"
                        OnClick="btn_tariff_service" OnClientClick="myFunction()" />
                </td>
            </tr>
        </table>
    </div>
    <div id="Tariff" class="tabcontent" style="margin: 10px 0 0 0;">
        <fieldset>
            <legend>Tariff</legend>
            <table width="100%">
                <tr>
                    <td style="font-weight: bold;" width="10%">Service Type
                    </td>
                    <td colspan="3" width="90%">
                        <asp:DropDownList ID="TariffService" runat="server" Visible="true">
                            <asp:ListItem Value="0">.::Select Tariff Service::.</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <%--<tr>
                    <td colspan="4" style="border-bottom: 1px solid #f27031; color: #f27031; padding: 10px 0 0;
                        font-weight: bold;">
                        Additional Weight Info.
                    </td>
                </tr>--%>
                <tr>

                    <%--</tr>
                <tr>
                    <td colspan="4" style="border-bottom: 1px solid #f27031; color: #f27031; padding: 10px 0 0;
                        font-weight: bold;">
                        Weight Bracket Info.
                    </td>
                </tr>
                <tr>--%>
                    <td width="10%">From Weight
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="txt_fromWeight" runat="server" MaxLength="5" Style="width: 80px;"
                            onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                    </td>
                    <td width="7%">To Weight
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="txt_toWeight" runat="server" MaxLength="5" Style="width: 80px;"
                            onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                    </td>
                    <td width="10%" style="background: #eee;">Add. Weight
                    </td>
                    <td width="10%" style="background: #eee;">
                        <asp:TextBox ID="txt_additionalWeight" runat="server" Text="0" MaxLength="5" Style="width: 80px;"
                            onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                    </td>

                    <td width="40%">
                        <asp:Button ID="btn_price" runat="server" Text="Add Price" OnClick="btn_addPrice_Click"
                            CssClass="button" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="btn_tariffupdate" runat="server" Text="Update All Tariff Price" OnClick="btn_updatetariffPrice_Click"
                CssClass="button" Style="float: right; position: relative; right: 85px; margin: 10px 0 0;"
                Visible="false" />
            <div style="width: 100%; height: 250px; overflow: scroll;">
                <span id="Table_1" class="tbl-large">
                    <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
                    <asp:UpdatePanel ID="up_1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gv_tariff" runat="server" AutoGenerateColumns="false" CssClass="mGrid center"
                                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                                OnRowCommand="gv_tariff_RowCommand" EmptyDataText="No Tariff Available" OnDataBound="gv_tariff_DataBound"
                                OnRowDataBound="gv_tariff_RowDataBound" OnSelectedIndexChanged="gv_tariff_SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="PR" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cb_pr" runat="server" AutoPostBack="true" OnCheckedChanged="cb_pr_CheckedChanged"
                                                Checked='<%#bool.Parse(Eval("cb_pr").ToString())%>' />
                                            <%--Checked='<%#bool.Parse(Eval("cb_pr").ToString())%>'--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Service" DataField="ServiceTypeName" HeaderStyle-Width="10%" />
                                    <asp:BoundField HeaderText="From Weight" DataField="FromWeight" HeaderStyle-Width="10%" />
                                    <asp:BoundField HeaderText="To Weight" DataField="ToWeight" HeaderStyle-Width="10%" />
                                    <asp:BoundField HeaderText="Additional Weight" DataField="addFactor" HeaderStyle-Width="10%" />
                                    <asp:TemplateField HeaderText="LOCAL" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Local" Width="50px" runat="server" MaxLength="4" onkeypress="return isNumberKeydouble();"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SAME" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Same" Width="50px" runat="server" MaxLength="4" onkeypress="return isNumberKeydouble();"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DIFF" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Diff" Width="50px" runat="server" MaxLength="4" onkeypress="return isNumberKeydouble();"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btn_Update" runat="server" CssClass="button" Text="Update" CommandName="Update"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                            <asp:HiddenField ID="isUpdated" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isUPDATED") %>' />
                                            <asp:HiddenField ID="hd_id" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                            <asp:HiddenField ID="hd_isupdated" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isUpdated") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_delete" runat="server" CssClass="button" Text="Delete" CommandName="del"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                            <asp:HiddenField ID="hd_id" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                            <asp:HiddenField ID="isUpdated_" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isUPDATED") %>' />
                                            <asp:HiddenField ID="hd_isupdated" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isUpdated") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </span>
            </div>
        </fieldset>
        <%--<table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
            <tr>
                <td>
                    <div id="BankingTariff" style="display: none;">
                        <table style="width: 100%; border-collapse: collapse; font-size: 14px;" class="tariffTable">
                            <tr style="width: 100%">
                                <td style="text-align: center; width: 51%; background-color: #EC7945; color: White;">
                                    <b>Banking </b>
                                </td>
                                <td style="text-align: center; width: 20%; background-color: #EC7945; color: White;">
                                    <b>Expected Revenue:</b>
                                </td>
                                <td style="text-align: center; width: 29%;">
                                    <asp:TextBox ID="txt_bankingExpectedRevenue" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%; border-collapse: collapse; font-size: 14px;" class="tariffTable">
                            <tr style="width: 100%">
                                <td style="text-align: center; width: 11%;" class="tariffTd">
                                    Domestic
                                </td>
                                <td style="text-align: center; width: 20%;">
                                    Bank to Bank
                                </td>
                                <td style="text-align: center; width: 20%;">
                                    Bank to General
                                </td>
                                <td style="text-align: center; width: 10%;">
                                    2nd Day
                                </td>
                                <td style="text-align: center; width: 49%;">
                                    Same Day / Holiday / Special Handling Services
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%; border-collapse: collapse; font-size: 14px;" class="tariffTable">
                            <tr style="width: 100%">
                                <td style="text-align: center; width: 11%;">
                                    Weight
                                </td>
                                <td style="text-align: center; width: 10%;">
                                    Within City
                                </td>
                                <td style="text-align: center; width: 10%;">
                                    City to City
                                </td>
                                <td style="text-align: center; width: 10%;">
                                    Within City
                                </td>
                                <td style="text-align: center; width: 10%;">
                                    City to City
                                </td>
                                <td style="text-align: center; width: 10%;">
                                    City to City
                                </td>
                                <td style="text-align: center; width: 13%;">
                                    Weight
                                </td>
                                <td style="text-align: center; width: 13%;">
                                    Within City
                                </td>
                                <td style="text-align: center; width: 13%;">
                                    City to City
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="text-align: left; width: 11%;">
                                    0.0 to 0.5 Kg
                                </td>
                                <td style="text-align: center; width: 10%;">
                                    <asp:TextBox ID="txt_B2BzeroTop5WC" runat="server"></asp:TextBox>
                                </td>
                                <td style="text-align: center; width: 10%;">
                                    <asp:TextBox ID="txt_B2BzeroTop5CTC" runat="server"></asp:TextBox>
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="text-align: left; width: 11%;">
                                    0.51 to 1 Kg
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="text-align: left; width: 11%;">
                                    Upto 3 Kg
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="text-align: left; width: 11%;">
                                    Each Add Kg.
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 10%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                                <td style="text-align: center; width: 13%;">
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div id="DomesticTariff">
                        <table style="width: 100%; border-collapse: collapse; font-size: 14px;" class="tariffTable">
                            <tr style="width: 100%">
                                <td style="width: 32%; background-color: #EC7945; color: White; text-align: center;">
                                    <b>Domestic</b>
                                </td>
                                <td style="width: 17%; background-color: #EC7945; color: White; text-align: center;">
                                    <b>Expected Revenue:</b>
                                </td>
                                <td style="width: 51%;">
                                    <asp:TextBox ID="txt_domExpRev" runat="server"></asp:TextBox>
                                </td>
                                <td style="display: none; width: 17%; background-color: #EC7945; color: White; text-align: center;">
                                    <b>Expected Revenue:</b>
                                </td>
                                <td style="width: 17%; display: none;">
                                    <asp:TextBox ID="txt_rnrExpRev" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%; border-collapse: collapse; font-size: 14px;" class="tariffTable"
                            runat="server" id="tbl_domesticTariff">
                            <tr style="width: 100%">
                                <td style="width: 15%; text-align: left; font-size: 14px;" rowspan="2">
                                    <b>Weight (Kg)</b>
                                </td>
                                <td style="width: 25.5%; text-align: center; font-size: 14px;" colspan="3">
                                    <b>Domestic</b>
                                </td>
                                <td style="width: 17%; text-align: center; font-size: 14px;" colspan="2">
                                    <b>Special Handling Service</b>
                                </td>
                                <td style="width: 8.5%; text-align: center; font-size: 14px;">
                                    <b>2nd Day</b>
                                </td>
                                <td style="display: none; width: 34%; text-align: center; font-size: 14px;" colspan="4">
                                    <b>Road & Rail</b>
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 8.5%; font-size: 14px; text-align: center;">
                                    <b>Within City</b>
                                </td>
                                <td style="width: 8.5%; font-size: 14px; text-align: center;">
                                    <b>Same Zone</b>
                                </td>
                                <td style="width: 8.5%; font-size: 14px; text-align: center;">
                                    <b>Diff Zone</b>
                                </td>
                                <td style="width: 8.5%; font-size: 14px; text-align: center;">
                                    <b>Within City</b>
                                </td>
                                <td style="width: 8.5%; font-size: 14px; text-align: center;">
                                    <b>City to City</b>
                                </td>
                                <td style="width: 8.5%; font-size: 14px; text-align: center;">
                                    <b>City to City</b>
                                </td>
                                <td style="display: none; width: 8.5%; font-size: 14px; text-align: center;">
                                    <b>Zone A</b>
                                </td>
                                <td style="display: none; width: 8.5%; font-size: 14px; text-align: center;">
                                    <b>Zone B</b>
                                </td>
                                <td style="display: none; width: 8.5%; font-size: 14px; text-align: center;">
                                    <b>Zone C</b>
                                </td>
                                <td style="display: none; width: 8.5%; font-size: 14px; text-align: center;">
                                    <b>Zone D</b>
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 15%; padding-left: 5px;">
                                    0.0 to 0.5
                                </td>
                                <td style="width: 8.5%;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t21" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t22" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t23" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t24" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t25" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t26" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t27" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t28" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t29" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t210" runat="server" />
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 15%; padding-left: 5px;">
                                    0.51 to 1
                                </td>
                                <td style="width: 8.5%;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t31" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t32" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t33" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t34" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t35" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t36" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t37" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t38" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t39" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t310" runat="server" />
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 15%; padding-left: 5px;">
                                    Each Add 0.5
                                </td>
                                <td style="width: 8.5%;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t41" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t42" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t43" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t44" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t45" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t46" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t47" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t48" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t49" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t410" runat="server" />
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 15%; padding-left: 5px;">
                                    Upto 3/5
                                </td>
                                <td style="width: 8.5%;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t51" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t52" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t53" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t54" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t55" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t56" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t57" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t58" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t59" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t510" runat="server" />
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 15%; padding-left: 5px;">
                                    Minimum 5/10
                                </td>
                                <td style="width: 8.5%;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t61" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t62" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t63" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t64" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t65" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t66" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t67" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t68" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t69" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t610" runat="server" />
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 15%">
                                    Each Add Kg
                                </td>
                                <td style="width: 8.5%;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t71" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t72" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t73" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t74" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t75" runat="server" />
                                </td>
                                <td style="width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t76" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t77" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t78" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t79" runat="server" />
                                </td>
                                <td style="display: none; width: 8.5%; padding-left: 5px;">
                                    <asp:TextBox Style="width: 90%; padding-left: 5px; border-style: none;" onkeypress="return isNumberKeydouble(event);"
                                        ID="t710" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="InternationalTariff" style="display: none;">
                        <table style="width: 100%; border-collapse: collapse; font-size: 14px;" class="tariffTable"
                            id="tbl_international" runat="server">
                            <tr style="width: 100%">
                                <td colspan="8" style="width: 60%; background-color: #EC7945; color: White; text-align: center;">
                                    <b>International</b>
                                </td>
                                <td colspan="2" style="width: 16%; background-color: #EC7945; color: White; text-align: center;">
                                    <b>Expected Revenue:</b>
                                </td>
                                <td colspan="2" style="width: 16%; text-align: center;">
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 100%; text-align: center;" colspan="12">
                                    <b>Zones</b>
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 12%; text-align: left; padding-left: 5px;" rowspan="2">
                                    <b>Weight: (Kg)</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>Zones</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>1</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>2</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>3</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>4</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>5</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>6</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>7</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>8</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>9</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>10</b>
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 8%; text-align: center;">
                                    <b>Category</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>UAE</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>Middle East</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>UK</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>South Asia</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>Western Europe</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>Asia Pacific</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>USA/Canada</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>Easter Europe</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>Africa</b>
                                </td>
                                <td style="width: 8%; text-align: center;">
                                    <b>Latin America</b>
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 12%; padding-left: 5px;">
                                    0.0 to 0.5
                                </td>
                                <td style="width: 8%; text-align: center; padding-left: 5px;" rowspan="3">
                                    Documents
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_42" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_43" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_44" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_45" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_46" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_47" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_48" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_49" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_410" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_411" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 12%; padding-left: 5px;">
                                    0.51 to 1
                                </td>
                                <td style="display: none;">
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_52" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_53" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_54" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_55" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_56" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_57" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_58" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_59" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_510" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_511" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="width: 100%; border: 1px Solid Black;">
                                <td style="width: 12%; padding-left: 5px;">
                                    Each Add 0.5
                                </td>
                                <td style="display: none;">
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_62" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_63" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_64" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_65" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_66" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_67" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_68" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_69" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_610" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_611" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 12%; padding-left: 5px;">
                                    1
                                </td>
                                <td style="width: 8%; text-align: center;" rowspan="2">
                                    Parcel
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_72" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_73" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_74" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_75" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_76" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_77" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_78" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_79" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_710" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_711" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 12%; padding-left: 5px;">
                                    Each Add 0.5
                                </td>
                                <td style="display: none;">
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_82" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_83" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_84" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_85" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_86" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_87" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_88" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_89" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_810" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                                <td style="width: 8%">
                                    <asp:TextBox ID="i_811" runat="server" Style="width: 90%; padding-left: 5px; border-style: none;"
                                        onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Button ID="btn_updateTariff" runat="server" Text="UpdateTariff" OnClick="btn_updateTariff_Click" />
                </td>
            </tr>
        </table>--%>
    </div>
    <script type="text/javascript" language="javascript">
        function Myfunc(cityName) {
            var i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }
            document.getElementById(cityName).style.display = "block";
        }
        function openCity(evt, cityName) {
            debugger;
            var i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }
            document.getElementById(cityName).style.display = "block";
            evt.currentTarget.className += " active";
        }
        function myFunction() {
            debugger;
            var element = document.getElementById("Tariff");

            if (element.classList) {
                element.classList.toggle(" active");
            } else {
                var classes = element.className.split(" ");
                var i = classes.indexOf(" active");

                if (i >= 0)
                    classes.splice(i, 1);
                else
                    classes.push(" active");
                element.className = classes.join(" ");
            }
        }
        function Show_Hide_By_Display() {
            debugger;

            var branch = document.getElementById('<%= dd_Branch.ClientID %>');
            if (branch.options[branch.selectedIndex].value == "0") {
                alert('Select Branch');
                return false;
            }
            var accountName = document.getElementById('<%= txt_name.ClientID %>').value;
            if (accountName == "") {
                alert('Enter Account Name');
                return false;
            }
            var faxNo = document.getElementById('<%= txt_FaxNo.ClientID %>').value;
            if (faxNo == "") {
                alert('Enter Fax No');
                return false;
            }
            var officialAddress = document.getElementById('<%= txt_OfficialAddress.ClientID %>').value;
            if (officialAddress == "") {
                alert('Enter Official Address');
                return false;
            }
            var MailingAddress = document.getElementById('<%= txt_MailingAddress.ClientID %>').value;
            if (MailingAddress == "") {
                alert('Enter Mailing Address');
                return false;
            }
            var pickupInstruction = document.getElementById('<%= txt_pickupInstruction.ClientID %>').value;
            if (pickupInstruction == "") {
                alert('Enter Pickup Instruction');
                return false;
            }
            var PhoneNo = document.getElementById('<%= txt_PhoneNo.ClientID %>').value;
            if (PhoneNo == "") {
                alert('Enter Phone No');
                return false;
            }
            var RegDate = document.getElementById('<%= txt_RegDate.ClientID %>').value;
            if (RegDate == "") {
                alert('Enter Reg Date');
                return false;
            }
            var RegEndDate = document.getElementById('<%= txt_RegEndDate.ClientID %>').value;
            if (RegEndDate == "") {
                alert('Enter Reg End Date');
                return false;
            }
            var email = document.getElementById('<%= txt_email.ClientID %>').value;
            if (email == "") {
                alert('Enter Email');
                return false;
            }
            var contactPerson = document.getElementById('<%= txt_contactPerson.ClientID %>').value;
            if (contactPerson == "") {
                alert('Enter Contact Person');
                return false;
            }
            var designation = document.getElementById('<%= txt_designation.ClientID %>').value;
            if (designation == "") {
                alert('Enter Designation');
                return false;
            }
            var Industry = document.getElementById('<%= dd_Industry.ClientID %>');
            if (Industry.options[Industry.selectedIndex].value == "0") {
                alert('Select Industry');
                return false;
            }
            var recoveryExpID = document.getElementById('<%= dd_recoveryExpID.ClientID %>');
            if (recoveryExpID.options[recoveryExpID.selectedIndex].value == "0") {
                alert('Select Recovery Express Center');
                return false;
            }
            var originEC = document.getElementById('<%= dd_originEC.ClientID %>');
            if (originEC.options[originEC.selectedIndex].value == "0") {
                alert('Select Origin Express Center');
                return false;
            }
            var SalesTaxNo = document.getElementById('<%= txt_SalesTaxNo.ClientID %>').value;
            if (SalesTaxNo == "") {
                alert('Enter Sales Tax No');
                return false;
            }
            var ntnNo = document.getElementById('<%= txt_ntnNo.ClientID %>').value;
            if (ntnNo == "") {
                alert('Enter NTN No');
                return false;
            }
            var fafType = document.getElementById('<%= dd_fafType.ClientID %>');
            if (fafType.options[fafType.selectedIndex].value == "") {
                alert('Select FAF Type');
                return false;
            }



            document.getElementById('<%=div2.ClientID %>').style.display = "";
            return true;

        }

        function National() {
            debugger;
            var chk = document.getElementById('<%= chk_nationWide.ClientID%>');
            var centralized = document.getElementById('<%= chk_centralisedClient.ClientID %>');
            var nationAcc = document.getElementById('<%= txt_nationWideAccountNo.ClientID %>');
            if (chk.checked) {
                nationAcc.disabled = false;
                centralized.checked = true;
                centralized.disabled = true;
            }
            else {
                nationAcc.disabled = true;
                centralized.checked = false;
                centralized.disabled = false;
            }
            CheckCentralized(centralized);

        }

        function NationalOK() {
            document.getElementById('nationalDiv').style.display = "none";
            openCity(event, 'BasicInfo');
        }
        function ParentBranchSelection(dd) {
            debugger;
            var parentBranch = dd.options[dd.options.selectedIndex].value;
            var Branches = document.getElementById('<%= chkList_Branches.ClientID %>');

            for (var i = 0; i < Branches.rows.length; i++) {

                for (var j = 0; j < Branches.rows[i].cells.length; j++) {
                    var branch = Branches.rows[i].cells[j].children[0].value;
                    if (parentBranch == branch) {
                        Branches.rows[i].cells[j].children[0].disabled = true;
                        Branches.rows[i].cells[j].childNodes[0].checked = true;

                    }
                    else {

                        Branches.rows[i].cells[j].children[0].disabled = false;

                    }
                }
            }

        }
        function CheckCentralized(chk) {


            var centralized = document.getElementById('<%= chk_centralisedClient.ClientID %>');
            var nationAcc = document.getElementById('<%= txt_nationWideAccountNo.ClientID %>');
            var nationWideGroup = document.getElementById('<%= txt_nationWideGroup.ClientID %>');
            if (chk.checked) {
                nationWideGroup.disabled = false;
            }
            else {
                nationWideGroup.disabled = true;
            }


        }
        function NationalCancel() {
            debugger;
            document.getElementById('nationalDiv').style.display = "none";
            openCity(event, 'BasicInfo');
            document.getElementById('<%= chk_nationWide.ClientID%>').checked = false;
        }

        function CODCheck() {
            debugger;
            var cod = document.getElementById('<%= chk_NotCod.ClientID %>');
            var codType = document.getElementById('<%= dd_codType.ClientID %>');

            var benBank = document.getElementById('<%= dd_benBank.ClientID %>');
            var benName = document.getElementById('<%= txt_benName.ClientID %>');
            var benAccNo = document.getElementById('<%= txt_benAccNo.ClientID %>');
            if (cod.checked) {
                codType.disabled = false;
                benBank.disabled = false;
                benName.disabled = false;
                benAccNo.disabled = false;

            }
            else {
                codType.disabled = true;
                benBank.disabled = true;
                benName.disabled = true;
                benAccNo.disabled = true;
            }
        }

        function CheckBranches(chk) {
            var branches = document.getElementById('<%= chkList_Branches.ClientID %>');
            var parentBranches = document.getElementById('<%= dd_parentBranch.ClientID %>');
            var parentBranch = parentBranches.options[parentBranches.options.selectedIndex].value;
            var checked = chk.childNodes[0].checked;


            for (var i = 0; i < branches.rows.length; i++) {
                for (var j = 0; j < branches.rows[i].cells.length; j++) {
                    if (parentBranch == branches.rows[i].cells[j].childNodes[0].value) {
                        continue;
                    }
                    branches.rows[i].cells[j].childNodes[0].checked = checked;

                }
            }
        }
    </script>
</asp:Content>
