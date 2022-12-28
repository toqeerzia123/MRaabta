<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="GeneratePickupRequest.aspx.cs" Inherits="MRaabta.Files.GeneratePickupRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../mazen/css/sweetalert.css" rel="stylesheet" />
    <script src="../mazen/js/sweetalert-dev.js"></script>
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
    <style>
        #ContentPlaceHolder1_rdPickupType {
            width: 100%
        }

        span {
            display: block;
        }

        select {
            height: 33px;
            width: 234px;
            padding: 1px 2px;
            border-radius: 0;
        }

        input[type="date" i] {
            height: 29px;
            width: 220px;
            padding: 1px 2px;
            border-radius: 0;
            border-color: #ada8a8;
        }




        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }

        #overlay {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: #f8f8f8;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=90);
            opacity: 0.9;
            -moz-opacity: 0.9;
        }

        #theprogress {
            background-color: #fff;
            border: 1px solid #ccc;
            padding: 10px;
            width: 300px;
            line-height: 30px;
            text-align: center;
            filter: Alpha(Opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

        #modalprogress {
            position: absolute;
            top: 40%;
            left: 50%;
            margin: -11px 0 0 -150px;
            color: #990000;
            font-weight: bold;
            font-size: 14px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function numeric(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && ((charCode >= 48 && charCode <= 57)))
                return true;
            else {

                return false;
            }
        }
    </script>
    <%-- <form id="form1" runat="server">--%>
    <asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
        <ProgressTemplate>
            <div id="overlay">
                <div id="modalprogress">
                    <div id="theprogress">
                        <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="../mazen/images/wait.gif" />
                        Please wait...
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">

        <ContentTemplate>
            <div class="row main-body newPanel">
                <fieldset class="fieldsetSmall">
                    <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                    <legend style="font-size: medium;"><b>Generate Pickup Request</b></legend>
                    <asp:UpdatePanel runat="server" ID="panel1">
                        <%--   <Triggers>
            <asp:PostBackTrigger ControlID="btnUpdate" />
        </Triggers>--%>
                        <ContentTemplate>
                            <div class="col-lg-12 col-md-12 col-sm-12" style="border-top: 2px solid #f26726; color: #444444; font-size: 20px; padding: 3px;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 20px">
                                            <b>Pickup Type</b>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:RadioButtonList ID="rdPickupType" RepeatDirection="Horizontal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Pickup_Type_SelectedIndexChanged">
                                                <asp:ListItem Value="Cash" Text="Cash" Selected="True" />
                                                <asp:ListItem Value="Credit" Text="Credit" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="text-align: center;">
                                <h2>Customer Information</h2>
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <table style="width: 100%">
                                    <tr id="labelCredit" visible="false" runat="server">
                                        <td style="width: 140px">
                                            <b>Account Number</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Account Name</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Select Location</b>
                                        </td>
                                        <td><%--<b>Only Scheduler Pickup</b>--%></td>
                                        <td></td>

                                    </tr>

                                    <tr id="tdCredit" visible="false" runat="server">

                                        <td style="width: 170px; padding: 10px 0;">
                                            <asp:TextBox ID="txtAccountNumber" Enabled="true" runat="server" OnTextChanged="txtAccountNumber_TextChanged" AutoPostBack="true" />
                                        </td>

                                        <td style="width: 170px">
                                            <asp:TextBox ID="txtAccountName" Enabled="false" runat="server" />
                                        </td>

                                        <td style="width: 140px">
                                            <asp:DropDownList ID="ddlLocation1" Enabled="true" runat="server" OnTextChanged="ddlLocation1_TextChanged" AutoPostBack="true" />

                                        </td>
                                        <td>
                                            <asp:RadioButtonList Visible="false" ID="rdschedulerPickup" RepeatDirection="Horizontal" runat="server" Style="display: inline-block; margin: 2px 0px 15px;" AutoPostBack="true" OnSelectedIndexChanged="rdschedulerPickup_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                <asp:ListItem Text="No" Value="No" />
                                            </asp:RadioButtonList>
                                            <asp:Label ID="msg" runat="server" Enabled="false" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="CustomerID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">
                                            <b>First Name</b>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ControlToValidate="txtFirstName" ForeColor="Red" Style="display: inline-block"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Middle Name</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Last Name</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Contact No</b>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required" ControlToValidate="txtContactNo" ForeColor="Red" Style="display: inline-block"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Whatsapp No</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 170px">
                                            <asp:TextBox ID="txtFirstName" Enabled="true" runat="server" MaxLength="20" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFirstName" ForeColor="Red" ValidationExpression="^([A-z][A-Za-z]*\s+[A-Za-z]*)|([A-z][A-Za-z]*)$" ErrorMessage="Invalid Name" />

                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtMiddleName" Enabled="true" runat="server" MaxLength="20" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtMiddleName" ForeColor="Red" ValidationExpression="^([A-z][A-Za-z]*\s+[A-Za-z]*)|([A-z][A-Za-z]*)$" ErrorMessage="Invalid Name" />
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtLastName" Enabled="true" runat="server" MaxLength="20" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtLastName" ForeColor="Red" ValidationExpression="^([A-z][A-Za-z]*\s+[A-Za-z]*)|([A-z][A-Za-z]*)$" ErrorMessage="Invalid Name" />
                                            <%--                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" Display="Dynamic" runat="server" ControlToValidate="txtLastName" ForeColor="Red" ValidationExpression="^([A-z][A-Za-z]*\s+[A-Za-z]*)|([A-z][A-Za-z]*)$" ErrorMessage="Invalid Name" />--%>
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtContactNo" Enabled="true" runat="server" onKeypress="if (event.keyCode < 46 || event.keyCode > 57 || event.keyCode == 47) event.returnValue = false;" MaxLength="11" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtContactNo" ForeColor="Red" ValidationExpression="[0-9]{11}" ErrorMessage="Invalid Contact No" />

                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtWhatsappNo" Enabled="true" onKeypress="if (event.keyCode < 46 || event.keyCode > 57 || event.keyCode == 47) event.returnValue = false;" runat="server" MaxLength="11" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtWhatsappNo" ForeColor="Red" ValidationExpression="[0-9]{11}" ErrorMessage="Invalid Whatsapp No" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">
                                            <b>Email ID</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>House/Flat/Shop/Office #</b>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Required" ControlToValidate="txtHouse" ForeColor="Red" Style="display: inline-block"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Floor No</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Building/Office Name </b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Plot Number</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtEmailID" Enabled="true" runat="server" MaxLength="100" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailID"
                                                ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                                ErrorMessage="Invalid email address" />
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtHouse" Enabled="true" runat="server" MaxLength="20" Style="margin: -25px 0;" />

                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtFloorNo" Enabled="true" runat="server" MaxLength="20" Style="margin: -25px 0;" />
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtBuilding" Enabled="true" runat="server" MaxLength="50" Style="margin: -25px 0;" />
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtPlotNo" Enabled="true" runat="server" MaxLength="20" Style="margin: -25px 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">
                                            <b>Street</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Sector/Zone/Block/Phase</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Area</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Postal Code</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b></b>

                                        </td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtStreet" Enabled="true" runat="server" MaxLength="30" />
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtSector" Enabled="true" runat="server" MaxLength="30" />
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtArea" Enabled="true" runat="server" MaxLength="50" />
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtPostalCode" Enabled="true" runat="server" MaxLength="10" />
                                        </td>
                                        <td style="width: 140px"></td>
                                    </tr>
                                </table>
                            </div>
                            <div style="text-align: center;">
                                <h2>Pickup Information</h2>
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 140px">
                                            <b>Shipments (Est. Pieces)</b>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Required" ControlToValidate="txtShipments" ForeColor="Red" Style="display: inline-block"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Weight (Est.Gram/Kg)</b>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Required" ControlToValidate="txtWeight" ForeColor="Red" Style="display: inline-block"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Light/Heavy</b>
                                            <asp:CompareValidator ID="CompareValidator4" runat="server"
                                                ControlToValidate="ddlWeightTypeID" ValueToCompare="-1" Operator="NotEqual"
                                                Type="Integer" ErrorMessage="Required" ForeColor="red" Style="display: inline-block;" />
                                        </td>
                                        <td style="width: 140px">
                                            <b>Product</b>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                ErrorMessage="Required" ControlToValidate="ddlProductID"
                                                InitialValue="-1" ForeColor="red" Style="display: inline-block;" />
                                            <%-- <asp:CompareValidator ID="CompareValidator5" runat="server" 
     ControlToValidate="ddlProductID" InitialValue="" 
     Type="Integer" ErrorMessage="Required" ForeColor="red" style="    display: inline-block;" />--%>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Service</b>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                ErrorMessage="Required" ControlToValidate="ddlServiceID"
                                                InitialValue="-1" ForeColor="red" Style="display: inline-block;" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 170px">
                                            <asp:TextBox ID="txtShipments" Enabled="true" runat="server" onkeypress="return numeric(event)" MaxLength="3" />

                                        </td>
                                        <td style="width: 170px">
                                            <asp:TextBox ID="txtWeight" Enabled="true" runat="server" onKeypress="if (event.keyCode < 46 || event.keyCode > 57 || event.keyCode == 47) event.returnValue = false;" MaxLength="5" />

                                        </td>
                                        <td style="width: 140px">
                                            <asp:DropDownList ID="ddlWeightTypeID" OnSelectedIndexChanged="weighttypeselectedchanged" AutoPostBack="true" Enabled="true" runat="server" />
                                        </td>
                                        <td style="width: 170px">
                                            <asp:DropDownList ID="ddlProductID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProductID_SelectedIndexChanged" />
                                        </td>
                                        <td style="width: 140px">
                                            <asp:DropDownList ID="ddlServiceID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">
                                            <b>Need Packing Material</b>
                                            <asp:CompareValidator ID="CompareValidator2" runat="server"
                                                ControlToValidate="ddlNeedPackingMaterialID" ValueToCompare="-1" Operator="NotEqual"
                                                Type="Integer" ErrorMessage="Required" ForeColor="red" Style="display: inline-block;" />
                                        </td>
                                        <td style="width: 140px">
                                            <b>Contents</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Loaders</b>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                ControlToValidate="ddlLoaders" ValueToCompare="-1" Operator="NotEqual"
                                                Type="Integer" ErrorMessage="Required" ForeColor="red" Style="display: inline-block;" />
                                        </td>
                                        <td style="width: 140px">
                                            <b>Handling Tool </b>
                                            <asp:CompareValidator ID="CompareValidator3" runat="server"
                                                ControlToValidate="ddlHandlingToolID" ValueToCompare="-1" Operator="NotEqual"
                                                Type="Integer" ErrorMessage="Required" ForeColor="red" Style="display: inline-block;" />
                                        </td>
                                        <td style="width: 140px">
                                            <b>Additional Remarks</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">
                                            <asp:DropDownList ID="ddlNeedPackingMaterialID" Enabled="true" runat="server" Style="margin: 10px 0;" />
                                        </td>
                                        <td style="width: 170px">

                                            <asp:TextBox TextMode="multiline" Columns="30" ID="txtContents" Enabled="true" runat="server" MaxLength="250" />
                                        </td>
                                        <td style="width: 170px">
                                            <%--<asp:DropDownList ID="DropDownList1" />
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                            <asp:DropDownList ID="ddlLoaders" runat="server">
                                                <asp:ListItem Selected="True" Text="Not Required" Value="-2" />
                                                <asp:ListItem Text="1" Value="1" />
                                                <asp:ListItem Text="2" Value="2" />
                                                <asp:ListItem Text="3" Value="3" />
                                                <asp:ListItem Text="4" Value="4" />
                                                <asp:ListItem Text="5" Value="5" />
                                            </asp:DropDownList>

                                        </td>
                                        <td style="width: 140px">
                                            <asp:DropDownList ID="ddlHandlingToolID" Enabled="true" runat="server" />
                                        </td>
                                        <td style="width: 470px" colspan="5">
                                            <asp:TextBox TextMode="multiline" ID="txtAdditionalRemarks" Columns="30" Enabled="true" runat="server" MaxLength="250" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="text-align: center;">
                                <h2>Schedule Information</h2>
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 140px">
                                            <b>Pickup Date</b>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="Required" ControlToValidate="txtPickupDate" ForeColor="Red" Style="display: inline-block"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 140px; padding: 0 18px;">
                                            <b>Timeslot</b>

                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">

                                            <asp:TextBox ID="txtPickupDate" TextMode="Date" Enabled="true" ClientIDMode="Static" runat="server" Style="margin: 10px 0;" />

                                        </td>

                                        <td style="width: 140px">
                                            <asp:DropDownList ID="ddlTimeslot" Enabled="true" runat="server" Style="margin: 0px 18px;" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">
                                            <b>Schedule</b>
                                        </td>
                                        <td style="width: 140px; padding: 0 18px;" id="Label_Schedule" runat="server">
                                            <b>Scheduled Type</b>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">
                                            <asp:RadioButtonList ID="rdScheduled" RepeatDirection="Horizontal" runat="server" Style="display: inline-block; margin: 2px 0px 15px;" AutoPostBack="true" OnSelectedIndexChanged="Add_Scheduled_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                <asp:ListItem Text="No" Value="No" />
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="width: 140px" id="ddl_Schedule" runat="server">
                                            <asp:DropDownList ID="ddlScheduleType" Enabled="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Scheduled_Type_SelectedIndexChanged" Style="margin: 0px 18px;" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr id="rWeek" runat="server" visible="false">
                                        <td style="width: 140px">
                                            <b>Day of Week</b>
                                        </td>
                                        <td width="550px" colspan="3">
                                            <asp:CheckBoxList ID="chkWeekDay" RepeatDirection="Horizontal" runat="server">
                                                <asp:ListItem Text="Monday" Value="Monday" />
                                                <asp:ListItem Text="Tuesday" Value="Tuesday" />
                                                <asp:ListItem Text="Wednesday" Value="Wednesday" />
                                                <asp:ListItem Text="Thursday" Value="Thursday" />
                                                <asp:ListItem Text="Friday" Value="Friday" />
                                                <asp:ListItem Text="Saturday" Value="Saturday" />
                                                <asp:ListItem Text="Sunday" Value="Sunday" />
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr id="rMonth" runat="server" visible="false">
                                        <td style="width: 140px">
                                            <b>Day of Month</b> </td>
                                        <td width="540px" colspan="3">
                                            <asp:CheckBoxList ID="chkMonthDat" RepeatColumns="10" RepeatDirection="Horizontal" runat="server">
                                                <asp:ListItem Text="1" Value="1" />
                                                <asp:ListItem Text="2" Value="2" />
                                                <asp:ListItem Text="3" Value="3" />
                                                <asp:ListItem Text="4" Value="4" />
                                                <asp:ListItem Text="5" Value="5" />
                                                <asp:ListItem Text="6" Value="6" />
                                                <asp:ListItem Text="7" Value="7" />
                                                <asp:ListItem Text="8" Value="8" />
                                                <asp:ListItem Text="9" Value="9" />
                                                <asp:ListItem Text="10" Value="10" />
                                                <asp:ListItem Text="11" Value="11" />
                                                <asp:ListItem Text="12" Value="12" />
                                                <asp:ListItem Text="13" Value="13" />
                                                <asp:ListItem Text="14" Value="14" />
                                                <asp:ListItem Text="15" Value="15" />
                                                <asp:ListItem Text="16" Value="16" />
                                                <asp:ListItem Text="17" Value="17" />
                                                <asp:ListItem Text="18" Value="18" />
                                                <asp:ListItem Text="19" Value="19" />
                                                <asp:ListItem Text="20" Value="20" />
                                                <asp:ListItem Text="21" Value="21" />
                                                <asp:ListItem Text="22" Value="22" />
                                                <asp:ListItem Text="23" Value="23" />
                                                <asp:ListItem Text="24" Value="24" />
                                                <asp:ListItem Text="25" Value="25" />
                                                <asp:ListItem Text="26" Value="26" />
                                                <asp:ListItem Text="27" Value="27" />
                                                <asp:ListItem Text="28" Value="28" />
                                                <asp:ListItem Text="29" Value="29" />
                                                <asp:ListItem Text="30" Value="30" />
                                                <asp:ListItem Text="31" Value="31" />
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="text-align: center;">
                                <h2>Assign Route</h2>
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 140px">
                                            <b>Route Code</b>
                                        </td>
                                        <td style="width: 80px; padding: 0 18px;">
                                            <b>Rider Code</b>
                                        </td>
                                        <td style="width: 140px; padding: 0 18px;">
                                            <b>Rider</b>
                                        </td>
                                        <td style="width: 140px; padding: 0 18px;">
                                            <b>Route Description</b>
                                        </td>
                                        <td style="width: 140px">
                                            <b>Auto Assign</b>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px">
                                            <asp:DropDownList ID="ddlRouteCode" Enabled="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRouteCode_SelectedIndexChanged" />
                                        </td>
                                        <td style="width: 80px">
                                            <asp:TextBox ID="txtridercode" Style="margin: 10px 18px;" Width="80px" Enabled="false" runat="server" MaxLength="50" />
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtRiderName" Style="margin: 10px 18px;" Enabled="false" runat="server" />
                                        </td>
                                        <td style="width: 140px">

                                            <asp:TextBox TextMode="multiline" Columns="30" ID="txtRouteDescription" Enabled="false" runat="server" Style="margin: 10px 18px;" />
                                        </td>
                                        <td style="width: 140px">
                                            <asp:DropDownList ID="ddlAutoAssign" Enabled="true" runat="server">
                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="btn_div" style="margin: 15px 15px;">
                        <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" OnClick="btnSubmit_Click" Text="Save" />
                        <asp:Button ID="btnReset" CssClass="btn btn-danger" CausesValidation="false" runat="server" OnClick="btnReset_Click" Text="Reset" />
                    </div>
                </fieldset>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
