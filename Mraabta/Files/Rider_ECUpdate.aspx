<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Rider_ECUpdate.aspx.cs" Inherits="MRaabta.Files.Rider_ECUpdate" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        input {
            padding: 3px;
        }

        .auto-style1 {
            width: 3%;
        }
    </style>
    <br />
    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" />
    <br />
    <div>
        <fieldset>
            <legend>Search Form</legend>
            <table style="font-size: Small; padding-bottom: 0px; width: 100%; padding-top: 0px !important">
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Rider Code </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_riderCode" runat="server" MaxLength="12" Width="160px"></asp:TextBox>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Branch </b>
                    </td>
                    <td style="width: 12%; text-align: left;">
                        <asp:DropDownList ID="ddl_branchId" runat="server" Width="145px" Height="30px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button Text="Search" ID="btn_search" OnClick="btn_search_Click" runat="server"
                            CssClass="button1" Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                            ControlToValidate="txt_riderCode" ErrorMessage="Please insert Rider Code" />
                    </td>
                    <td></td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                            ControlToValidate="ddl_branchId" InitialValue="Select Branch" ErrorMessage="Please select Branch" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div runat="server" id="div_main">
        <fieldset>
            <legend style="font-size: medium;">Rider Entry Form</legend>
            <table style="font-size: Small; padding-bottom: 0px; width: 100%; padding-top: 0px !important">
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>First Name </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_FirstName" runat="server" MaxLength="50" Width="160px" Enabled="False"></asp:TextBox>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Middle Name</b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_MiddleName" runat="server" MaxLength="50" Width="160px" Enabled="False"></asp:TextBox>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Last Name</b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_LastName" runat="server" MaxLength="50" Width="160px" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>CNIC </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_CNIC" runat="server" oninput="myfunction(this);" MaxLength="12"
                            Width="160px" Enabled="False">
                        </asp:TextBox>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Rider Type </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:DropDownList ID="ddl_cid" runat="server" Width="145px" Height="30px" Enabled="False">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Address </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_address" runat="server" MaxLength="50" Width="160px" Enabled="False"></asp:TextBox>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Phone No </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_phoneNo" runat="server" MaxLength="12" Width="160px" Enabled="False"></asp:TextBox>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Duty Type </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:DropDownList ID="ddl_dutyType" runat="server" Width="145px" Height="30px" Enabled="False">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Email </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_email" runat="server" MaxLength="12" Width="160px" Enabled="false"></asp:TextBox>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;"></td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_lastrouteCode" Visible="false" runat="server" MaxLength="12"
                            Width="160px">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; /* margin-top: auto; */vertical-align: top; padding-right: 10px; padding-top: 9px;">
                        <b>Zone Id</b>
                    </td>
                    <td style="width: 20%; text-align: left;">
                        <asp:DropDownList ID="ddl_zoneId" runat="server" Width="120px" Height="30px" OnSelectedIndexChanged="ddl_zoneId_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ForeColor="Red" ControlToValidate="ddl_zoneId"
                            InitialValue="Select Zone" ErrorMessage="Please select Zone" />
                    </td>
                    <td style="text-align: right; /* margin-top: auto; */vertical-align: top; padding-right: 10px; padding-top: 9px;">
                        <b>Branch Id </b>
                    </td>
                    <td style="width: 20%; text-align: left;">
                        <asp:DropDownList ID="ddl_branch" runat="server" Width="120px" Height="31px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_branch_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                            ControlToValidate="ddl_branchId" InitialValue="Select Branch" ErrorMessage="Please select Branch" />
                    </td>
                    <td style="text-align: right; /* margin-top: auto; */vertical-align: top; padding-right: 10px; padding-top: 9px;">
                        <b>E Center </b>
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddl_expressCenterId" runat="server" Width="120px" Height="30px"
                            OnSelectedIndexChanged="ddl_expressCenterId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Shift </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:DropDownList ID="ddl_Shift" runat="server" Width="145px" Height="30px" Enabled="False">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
        <fieldset>
            <legend>Rider Entry Form</legend>
            <table style="font-size: Small; padding-bottom: 0px; width: 100%; padding-top: 0px !important">
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Route Code </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_routeCode" runat="server" MaxLength="12" Width="160px" AutoPostBack="True"
                            OnTextChanged="txt_routeCode_TextChanged" Enabled="False">
                        </asp:TextBox>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>City</b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:DropDownList ID="ddl_city" runat="server" Width="145px" Height="30px">
                        </asp:DropDownList>
                    </td>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Route Name </b>
                    </td>
                    <td style="width: 15%; text-align: left;">
                        <asp:TextBox ID="txt_routeName" runat="server" MaxLength="25" Width="160px" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div>
            <fieldset>
                <legend>HR Section </legend>
                <table style="font-size: Small; padding-bottom: 0px; width: 100%; padding-top: 0px !important">
                    <tr>
                        <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                            <b>HRS Code </b>
                        </td>
                        <td style="width: 15%; text-align: left;">
                            <asp:TextBox ID="txt_hrs_Code" runat="server" MaxLength="12" Width="160px" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                            <b>Date of Joining</b>
                        </td>
                        <td style="width: 15%; text-align: left;">
                            <asp:TextBox ID="txt_DOj" runat="server" MaxLength="25" Width="160px" Enabled="False"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd"
                                PopupButtonID="txt_DOj" TargetControlID="txt_DOj"></Ajax1:CalendarExtender>
                        </td>
                        <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                            <b>Date of Birth </b>
                        </td>
                        <td style="width: 15%; text-align: left;">
                            <asp:TextBox ID="txt_DOB" runat="server" MaxLength="25" Width="160px" Enabled="False"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="ceLoanTakenDate" runat="server" Format="yyyy-MM-dd" PopupButtonID="txt_DOB"
                                TargetControlID="txt_DOB"></Ajax1:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    <br />
    <div>
        <table>
            <tr>
                <td>
                    <b>Deactivate</b>
                </td>
                <td>
                    <asp:CheckBox runat="server" Enabled="False" ID="chk_Deactivate" AutoPostBack="true"
                        OnCheckedChanged="chk_Deactivate_CheckedChanged" />
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" visible="False" id="div_deactivate">
        <fieldset>
            <legend style="font-size: medium;">Rider Deactivation Form</legend>
            <table style="font-size: Small; padding-bottom: 0px; width: 100%; padding-top: 0px !important">
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Type Of Separation </b>
                    </td>
                    <td style="text-align: left;" class="auto-style1">
                        <asp:DropDownList runat="server" ID="ddl_separation" Width="171px" Height="30px">
                            <asp:ListItem Value="Select" Text="Select" />
                            <asp:ListItem Value="Left" Text="Left" />
                            <asp:ListItem Value="Resigned" Text="Resigned" />
                            <asp:ListItem Value="Terminate" Text="Terminate" />
                            <asp:ListItem Value="Dismissal" Text="Dismissal" />
                        </asp:DropDownList>
                    </td>
                    <td class="field" style="width: 12%;">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                            ControlToValidate="ddl_separation" InitialValue="Select" ErrorMessage="Please select Separation" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Date Of Leaving</b>
                    </td>
                    <td style="text-align: left;" class="auto-style1">
                        <asp:TextBox ID="txt_leaving" runat="server" MaxLength="12" Width="160px"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd"
                            PopupButtonID="txt_leaving" TargetControlID="txt_leaving"></Ajax1:CalendarExtender>
                    </td>
                    <td class="field" style="width: 12%; text-align: left;">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                            ControlToValidate="txt_leaving" ErrorMessage="Please Select Date" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="field" style="width: 12%; text-align: right; padding-right: 10px;">
                        <b>Remarks </b>
                    </td>
                    <td style="text-align: left;" class="auto-style1">
                        <asp:TextBox ID="txt_remark" runat="server" TextMode="MultiLine" Width="162px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <br />
    <br />
    <table class="input-form" style="width: 97%">
        <tr>
            <td class="field">
                <asp:Button Text="Save" ID="btn_save" OnClick="btn_save_Click" runat="server" CssClass="button1"
                    Width="60px" />
            </td>
        </tr>
    </table>
</asp:Content>
